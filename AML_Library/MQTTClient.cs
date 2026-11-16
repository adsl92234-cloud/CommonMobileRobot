using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace AML
{
   
    /// </summary>
    public class MQTTClient
    {
        private IMqttClient mqttClient;
        private MqttClientOptions clientOptions;
        private readonly SemaphoreSlim clientLock = new SemaphoreSlim(1, 1);
        private static AML_LogPage Log;
        public static string ConnString = "server= 127.0.0.1;uid=root;pwd=admin;database=amldb_foupcart";
        

        //=====設定=====
        public string Broker { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 1883;
        public string ClientId { get; set; } = "AML_Client";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int DefaultQos { get; set; } = 0;

        //=====狀態=====
        public bool IsConnected => mqttClient != null && mqttClient.IsConnected;

        //=====Events=====
        /// <summary>收到訊息時觸發 (Topic, Payload)</summary>
        public event Action<string, string> OnMessageReceived;

        /// <summary>連線成功時觸發</summary>
        public event Action OnConnected;

        /// <summary>連線中斷時觸發</summary>
        public event Action OnDisconnected;

        /// <summary>Log 輸出事件</summary>
        public event Action<string> OnLog;

        //=====Initial=====
        //public void Initial(string broker, int port, string clientId, string username = "", string password = "", AML_LogPage _Log = null)
        //{
        //    Broker = broker;
        //    Port = port;
        //    ClientId = clientId;
        //    Username = username;
        //    Password = password;
        //    if (_Log != null)
        //        Log = _Log;
        //    ConnectAsync();
        //}
        public void Initial(string broker, int port, string clientId, string username = "", string password = "")
        {
            Broker = broker;
            Port = port;
            ClientId = clientId;
            Username = username;
            Password = password;
            ConnectAsync();
        }

        //=====Connect / Disconnect=====
        public async Task<bool> ConnectAsync()
        {
            await clientLock.WaitAsync();
            try
            {
                //if (mqttClient != null && mqttClient.IsConnected)
                //{
                //    Log.WriteSystemLog("MQTT 已連線，跳過");
                //    return true;
                //}

                try
                {
                    var factory = new MqttFactory();
                    mqttClient = factory.CreateMqttClient();

                    var builder = new MqttClientOptionsBuilder()
                        .WithTcpServer(Broker, Port)
                        .WithClientId(ClientId)
                        .WithCleanSession();

                    if (!string.IsNullOrEmpty(Username))
                        builder.WithCredentials(Username, Password);

                    clientOptions = builder.Build();

                    // 綁定事件
                    mqttClient.ApplicationMessageReceivedAsync += ea =>
                    {
                        var seg = ea.ApplicationMessage.PayloadSegment;
                        var payload = Encoding.UTF8.GetString(seg.Array, seg.Offset, seg.Count);
                        Log.WriteSystemLog($"📩 收到 [{ea.ApplicationMessage.Topic}]: {payload}");
                        OnMessageReceived?.Invoke(ea.ApplicationMessage.Topic, payload);
                        return Task.CompletedTask;
                    };

                    mqttClient.DisconnectedAsync += ea =>
                    {
                        Log.WriteSystemLog("⚠ 已斷線");
                        OnDisconnected?.Invoke();
                        return Task.CompletedTask;
                    };

                    await mqttClient.ConnectAsync(clientOptions, CancellationToken.None);
                    //Log.WriteSystemLog($"✅ 已連線到 {Broker}:{Port}");
                    OnConnected?.Invoke();
                    return true;
                }
                catch (Exception ex)
                {
                    //Log.WriteSystemLog($"❌ 連線失敗: {ex.Message}");
                    return false;
                }
            }
            finally
            {
                clientLock.Release();
            }
        }

        public async Task DisconnectAsync()
        {
            await clientLock.WaitAsync();
            try
            {
                try
                {
                    if (mqttClient != null && mqttClient.IsConnected)
                    {
                        await mqttClient.DisconnectAsync();
                        //Log.WriteSystemLog("🔌 已手動斷線");
                    }
                }
                catch (Exception ex)
                {
                    //Log.WriteSystemLog($"❌ 斷線失敗: {ex.Message}");
                }
            }
            finally
            {
                clientLock.Release();
            }
        }

        //=====Subscribe / Unsubscribe=====
        public async Task<bool> SubscribeAsync(string topic, int qos = 0)
        {
            if (mqttClient == null || !mqttClient.IsConnected)
            {
                Log.WriteSystemLog("❌ Subscribe 失敗: 尚未連線");
                return false;
            }

            try
            {
                var qosLevel = (MqttQualityOfServiceLevel)qos;
                var topicFilter = new MqttTopicFilterBuilder()
                    .WithTopic(topic)
                    .WithQualityOfServiceLevel(qosLevel)
                    .Build();

                await mqttClient.SubscribeAsync(topicFilter, CancellationToken.None);
                //Log.WriteSystemLog($"✅ 已訂閱: {topic} (QoS={qos})");
                return true;
            }
            catch (Exception ex)
            {
                //Log.WriteSystemLog($"❌ 訂閱失敗: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UnsubscribeAsync(string topic)
        {
            if (mqttClient == null || !mqttClient.IsConnected)
            {
                //Log.WriteSystemLog("❌ Unsubscribe 失敗: 尚未連線");
                return false;
            }

            try
            {
                await mqttClient.UnsubscribeAsync(topic, CancellationToken.None);
                //Log.WriteSystemLog($"🚫 已取消訂閱: {topic}");
                return true;
            }
            catch (Exception ex)
            {
                //Log.WriteSystemLog($"❌ 取消訂閱失敗: {ex.Message}");
                return false;
            }
        }

        //=====Publish=====
        ///// <summary>
        ///// 發佈訊息
        ///// </summary>
        ///// <param name="topic">Topic</param>
        ///// <param name="payload">Payload (字串或 JSON)</param>
        ///// <param name="qos">QoS 等級 (0/1/2)</param>
        ///// <param name="retain">是否 Retain</param>
        public async Task<bool> PublishAsync(string topic, string payload, int qos = 0, bool retain = false)
        {
            if (mqttClient == null || !mqttClient.IsConnected)
            {
                Log.WriteSystemLog("❌ Publish 失敗: 尚未連線");
                return false;
            }

            try
            {
                var qosLevel = (MqttQualityOfServiceLevel)qos;

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithQualityOfServiceLevel(qosLevel)
                    .WithRetainFlag(retain)
                    .Build();

                await mqttClient.PublishAsync(message, CancellationToken.None);
                //Log.WriteSystemLog($"📤 已發佈到 [{topic}]: {payload}");
                return true;
            }
            catch (Exception ex)
            {
                //Log.WriteSystemLog($"❌ 發佈失敗: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 發佈 JSON 物件 (簡易 helper)
        /// </summary>
        public async Task<bool> PublishJsonAsync(string topic, Dictionary<string, object> json, int qos = 0, bool retain = false)
        {
            string payload = DictionaryToJson(json);
            return await PublishAsync(topic, payload, qos, retain);
        }

        //=====JSON Helper=====
        /// <summary>
        /// 將 Dictionary 轉為格式化 JSON 字串 (4 格縮排, 與 MQTT.cs 的 JsonBuilder 風格一致)
        /// </summary>
        public static string DictionaryToJson(Dictionary<string, object> dict)
        {
            var sb = new StringBuilder();
            sb.Append("{\n");
            BuildObject(sb, dict, 1);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 格式化 Object 內容 (不含外層大括號)
        /// </summary>
        private static void BuildObject(StringBuilder sb, Dictionary<string, object> dict, int indent)
        {
            string pad = new string(' ', indent * 4);
            int i = 0;
            foreach (var kv in dict)
            {
                string comma = (i < dict.Count - 1) ? "," : "";
                sb.Append($"{pad}\"{EscapeJson(kv.Key)}\": ");
                AppendJsonValue(sb, kv.Value, indent);
                sb.Append($"{comma}\n");
                i++;
            }
        }

        /// <summary>
        /// 格式化 Array 內容 (不含外層方括號)
        /// </summary>
        private static void BuildArray(StringBuilder sb, List<object> list, int indent)
        {
            string pad = new string(' ', indent * 4);
            for (int i = 0; i < list.Count; i++)
            {
                string comma = (i < list.Count - 1) ? "," : "";
                sb.Append(pad);
                AppendJsonValueInline(sb, list[i], indent, out _);
                sb.Append(comma + "\n");
            }
        }

        /// <summary>
        /// 格式化單一節點值。若值為 Object/Array，則跨行展開。
        /// </summary>
        private static void AppendJsonValue(StringBuilder sb, object value, int indent)
        {
            if (value == null)
                sb.Append("null");
            else if (value is string s)
                sb.Append($"\"{EscapeJson(s)}\"");
            else if (value is bool b)
                sb.Append(b ? "true" : "false");
            else if (value is Dictionary<string, object> d)
            {
                if (d.Count == 0)
                    sb.Append("{}");
                else
                {
                    sb.Append("{\n");
                    BuildObject(sb, d, indent + 1);
                    sb.Append(new string(' ', indent * 4) + "}");
                }
            }
            else if (value is List<object> list)
            {
                if (list.Count == 0)
                    sb.Append("[]");
                else
                {
                    sb.Append("[\n");
                    BuildArray(sb, list, indent + 1);
                    sb.Append(new string(' ', indent * 4) + "]");
                }
            }
            else if (value is float f)
                sb.Append(f.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
            else if (value is double dd)
                sb.Append(dd.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
            else if (value is decimal dec)
                sb.Append(dec.ToString(System.Globalization.CultureInfo.InvariantCulture));
            else
                sb.Append(value.ToString());
        }

        /// <summary>
        /// Array 內項目格式化。回傳值 isContainer 表示該值是否為跨行的 Object/Array。
        /// </summary>
        private static void AppendJsonValueInline(StringBuilder sb, object value, int indent, out bool isContainer)
        {
            isContainer = false;
            if (value is Dictionary<string, object> d && d.Count > 0)
            {
                isContainer = true;
                sb.Append("{\n");
                BuildObject(sb, d, indent + 1);
                sb.Append(new string(' ', indent * 4) + "}");
            }
            else if (value is List<object> list && list.Count > 0)
            {
                isContainer = true;
                sb.Append("[\n");
                BuildArray(sb, list, indent + 1);
                sb.Append(new string(' ', indent * 4) + "]");
            }
            else
            {
                AppendJsonValue(sb, value, indent);
            }
        }

        private static string EscapeJson(string s)
        {
            if (s == null) return "";
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"")
                    .Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
        }

        //=====Log=====
        //private void Log(string message)
        //{
        //    var logLine = $"[{DateTime.Now:HH:mm:ss}] {message}";
        //    OnLog?.Invoke(logLine);
        //}
    }
}