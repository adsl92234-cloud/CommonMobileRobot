using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json.Linq;


namespace AML
{
    /// <summary>
    /// 純 C# 封裝的 MQTT 通訊類別 (無 UI 依賴，仿 Socket 風格)
    /// </summary>
    public class MqttClient : IDisposable
    {
        private IMqttClient _mqttClient;
        private MqttClientOptions _lastOptions;
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, MqttQualityOfServiceLevel> _subscriptions =
            new System.Collections.Concurrent.ConcurrentDictionary<string, MqttQualityOfServiceLevel>();
        private volatile bool _manualDisconnect;
        private volatile bool _disposed;
        private int _reconnecting;

        /// <summary>
        /// 非主動斷線時是否自動重連
        /// </summary>
        public bool AutoReconnect { get; set; } = true;

        /// <summary>
        /// 自動重連間隔 (ms)
        /// </summary>
        public int ReconnectDelayMs { get; set; } = 5000;

        // 1. 資料到達事件 (類似 Socket DataArrival，回傳 Topic 與原始 JSON Payload)
        public event Action<string, string> DataArrival;

        // 2. 連線狀態變更事件 (true: 已連線, false: 已斷線)
        public event Action<bool> ConnectionChanged;

        // 3. 錯誤訊息事件
        public event Action<string> OnError;

        /// <summary>
        /// 目前是否已連線
        /// </summary>
        public bool IsConnected => _mqttClient != null && _mqttClient.IsConnected;

        public MqttClient()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            // 綁定底層接收事件
            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                string topic = e.ApplicationMessage.Topic;
                string payload = string.Empty;

                if (e.ApplicationMessage.PayloadSegment.Count > 0)
                {
                    payload = Encoding.UTF8.GetString(
                        e.ApplicationMessage.PayloadSegment.Array,
                        e.ApplicationMessage.PayloadSegment.Offset,
                        e.ApplicationMessage.PayloadSegment.Count);
                }

                // 觸發 DataArrival 事件
                DataArrival?.Invoke(topic, payload);
                return Task.CompletedTask;
            };

            // 連線狀態通知
            _mqttClient.ConnectedAsync += e =>
            {
                ConnectionChanged?.Invoke(true);
                return Task.CompletedTask;
            };

            _mqttClient.DisconnectedAsync += e =>
            {
                ConnectionChanged?.Invoke(false);
                if (e.ClientWasConnected && AutoReconnect && !_manualDisconnect && !_disposed && _lastOptions != null)
                    _ = ReconnectLoopAsync();
                return Task.CompletedTask;
            };
        }

        private async Task ReconnectLoopAsync()
        {
            if (Interlocked.Exchange(ref _reconnecting, 1) == 1) return;
            try
            {
                while (AutoReconnect && !_manualDisconnect && !_disposed && !IsConnected)
                {
                    await Task.Delay(ReconnectDelayMs);
                    if (_manualDisconnect || _disposed || IsConnected) break;
                    try
                    {
                        await _mqttClient.ConnectAsync(_lastOptions, CancellationToken.None);
                        foreach (var sub in _subscriptions)
                        {
                            var filter = new MqttTopicFilterBuilder()
                                .WithTopic(sub.Key)
                                .WithQualityOfServiceLevel(sub.Value)
                                .Build();
                            await _mqttClient.SubscribeAsync(filter);
                        }
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke($"Reconnect Exception: {ex.Message}");
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref _reconnecting, 0);
            }
        }

        /// <summary>
        /// 連線至 MQTT Broker (Connect)
        /// </summary>
        public async Task<bool> ConnectAsync(string brokerIp, int port = 1883, string clientId = "")
        {
            try
            {
                if (string.IsNullOrEmpty(clientId))
                {
                    clientId = "MR_Client_" + Guid.NewGuid().ToString("N").Substring(0, 6);
                }

                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(brokerIp, port)
                    .WithClientId(clientId)
                    .WithCleanSession()
                    .Build();

                _manualDisconnect = false;
                _lastOptions = options;
                await _mqttClient.ConnectAsync(options, CancellationToken.None);
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Connect Exception: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 訂閱指定的 Topic (Subscribe)
        /// </summary>
        public async Task<bool> SubscribeAsync(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce)
        {
            if (!IsConnected) return false;

            try
            {
                var filter = new MqttTopicFilterBuilder()
                    .WithTopic(topic)
                    .WithQualityOfServiceLevel(qos)
                    .Build();

                await _mqttClient.SubscribeAsync(filter);
                _subscriptions[topic] = qos;
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Subscribe Exception ({topic}): {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 發送資料 (Send / Publish)
        /// </summary>
        public async Task<bool> SendAsync(string topic, string jsonPayload, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce)
        {
            if (!IsConnected)
            {
                OnError?.Invoke("Send Failed: MQTT is not connected.");
                return false;
            }

            try
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(jsonPayload)
                    .WithQualityOfServiceLevel(qos)
                    .Build();

                await _mqttClient.PublishAsync(message, CancellationToken.None);
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Send Exception ({topic}): {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 斷開連線 (Disconnect)
        /// </summary>
        public async Task DisconnectAsync()
        {
            _manualDisconnect = true;
            if (IsConnected)
            {
                await _mqttClient.DisconnectAsync();
            }
        }

        public void Dispose()
        {
            _disposed = true;
            _mqttClient?.Dispose();
        }
    }
}