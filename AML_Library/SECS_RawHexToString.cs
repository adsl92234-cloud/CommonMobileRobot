using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AML
{
    public class SECS_RawHexToString
    {
        public static string RawHexToString(string RawHex)
        {
            int tt = 0;
            string[] temp;
            string Hex_string = "";
            temp = RawHex.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length == 0)
            {
                Hex_string += "     < Header only >\r\n";
            }
            else
            {
                while (tt != temp.Length)
                {
                    Hex_string += decode_secs(temp, tt, 1, out tt);
                }
            }
            return Hex_string;
        }
        private static string decode_secs(string[] RawHex, int index, int layer, out int later_index)
        {
            string format = "";
            int length_byte = 0, length = 0;
            format = ((Convert.ToByte(Hex_string_to_Dec(RawHex[index])) & 0xFC) / 4).ToString();
            length_byte = Convert.ToByte(Hex_string_to_Dec(RawHex[index])) & 0x3;
            for (int i = 0; i < length_byte; i++)
            {
                length += Hex_string_to_Dec(RawHex[index + length_byte - i]) * (int)Math.Pow(256, i);
            }
            string message = "";

            for (int i = 0; i < layer; i++)
            {
                message += "     ";
            }
            if (format == "0")
            {
                message += "< L" + length.ToString() + "\r\n";
                later_index = index + length_byte + 1;
                for (int i = 0; i < length; i++)
                {
                    message += decode_secs(RawHex, later_index, layer + 1, out later_index);
                }
                for (int j = 0; j < layer; j++)
                {
                    message += "     ";
                }
                message += ">\r\n";
            }
            else
            {
                if (length == 0)
                {
                    message += "< ASCII = nothing ";
                }
                else
                {
                    message += "< ";
                    switch (format)
                    {
                        case "8":
                            {
                                message += "Binary = ";
                                for (int i = 0; i < length; i++)
                                    message += "0x" + RawHex[index + length_byte + 1 + i] + " ";
                                break;
                            }
                        case "9":
                            {
                                message += "Boolean = ";
                                for (int i = 0; i < length; i++)
                                {
                                    if (RawHex[index + length_byte + 1 + i] == "00")
                                        message += "False";
                                    else
                                        message += "True";
                                    message += " ";
                                }
                                break;
                            }
                        case "17":
                            {
                                message += "JIS-8 = Unknown ";
                                break;
                            }
                        case "18":
                            {
                                message += "2-byte character = ";
                                byte[] data_UNICODE = new byte[length];
                                for (int i = 0; i < length; i++)
                                {
                                    data_UNICODE[i] = (byte)Hex_string_to_Dec(RawHex[index + length_byte + 1 + i]);
                                }
                                message += Encoding.Unicode.GetChars(data_UNICODE).ToString();
                                break;
                            }
                        case "24":
                            {
                                message += "I8 = ";
                                byte[] data_I8 = new byte[length];
                                for (int i = 0; i < length; i++)
                                {
                                    data_I8[i] = (byte)Hex_string_to_Dec(RawHex[index + length_byte + length - i]);
                                }
                                for (int i = 0; i < length / 8; i++)
                                    message += BitConverter.ToInt64(data_I8, length - (i + 1) * 8).ToString() + " ";
                                break;
                            }
                        case "25":
                            {
                                message += "I1 = ";
                                for (int i = 0; i < length; i++)
                                {
                                    message += ConvertToInt8(RawHex[index + length_byte + 1 + i]).ToString() + " ";
                                }
                                break;
                            }
                        case "26":
                            {
                                message += "I2 = ";
                                byte[] data_I2 = new byte[length];
                                for (int i = 0; i < length; i++)
                                {
                                    data_I2[i] = (byte)Hex_string_to_Dec(RawHex[index + length_byte + length - i]);
                                }
                                for (int i = 0; i < length / 2; i++)
                                    message += BitConverter.ToInt16(data_I2, length - (i + 1) * 2).ToString() + " ";
                                break;
                            }
                        case "28":
                            {
                                message += "I4 = ";
                                byte[] data_I4 = new byte[length];
                                for (int i = 0; i < length; i++)
                                {
                                    data_I4[i] = (byte)Hex_string_to_Dec(RawHex[index + length_byte + length - i]);
                                }
                                for (int i = 0; i < length / 4; i++)
                                    message += BitConverter.ToInt32(data_I4, length - (i + 1) * 4).ToString() + " ";
                                break;
                            }
                        case "32":
                            {
                                message += "F8 = ";
                                byte[] data_F8 = new byte[length];
                                for (int i = 0; i < length; i++)
                                {
                                    data_F8[i] = (byte)Hex_string_to_Dec(RawHex[index + length_byte + length - i]);
                                }
                                for (int i = 0; i < length / 8; i++)
                                    message += BitConverter.ToDouble(data_F8, length - (i + 1) * 8).ToString() + " ";
                                break;
                            }
                        case "36":
                            {
                                message += "F4 = ";
                                byte[] data_F4 = new byte[length];
                                for (int i = 0; i < length; i++)
                                {
                                    data_F4[i] = (byte)Hex_string_to_Dec(RawHex[index + length_byte + length - i]);
                                }
                                for (int i = 0; i < length / 4; i++)
                                    message += BitConverter.ToSingle(data_F4, length - (i + 1) * 4).ToString() + " ";
                                break;
                            }
                        case "40":
                            {
                                message += "U8 = ";
                                //UInt64 data_U8 = 0;
                                //for (int i = 0; i < length; i++)
                                //{
                                //    data_U8 += (UInt64)(Hex_string_to_Dec(RawHex[index + length_byte + length - i]) * Math.Pow(256, i));
                                //}
                                //message += data_U8.ToString();
                                byte[] data_U8 = new byte[length];
                                for (int i = 0; i < length; i++)
                                {
                                    data_U8[i] = (byte)Hex_string_to_Dec(RawHex[index + length_byte + length - i]);
                                }
                                for (int i = 0; i < length / 8; i++)
                                    message += BitConverter.ToUInt64(data_U8, length - (i + 1) * 8).ToString() + " ";
                                break;
                            }
                        case "41":
                            {
                                message += "U1 = ";
                                for (int i = 0; i < length; i++)
                                    message += Hex_string_to_Dec((RawHex[index + length_byte + 1 + i]).ToString()) + " ";
                                break;
                            }
                        case "42":
                            {
                                message += "U2 = ";
                                byte[] data_U2 = new byte[length];
                                for (int i = 0; i < length; i++)
                                {
                                    data_U2[i] = (byte)Hex_string_to_Dec(RawHex[index + length_byte + length - i]);
                                }
                                for (int i = 0; i < length / 2; i++)
                                    message += BitConverter.ToUInt16(data_U2, length - (i + 1) * 2).ToString() + " ";
                                break;
                            }
                        case "44":
                            {
                                message += "U4 = ";
                                byte[] data_U4 = new byte[length];
                                for (int i = 0; i < length; i++)
                                {
                                    data_U4[i] = (byte)Hex_string_to_Dec(RawHex[index + length_byte + length - i]);
                                }
                                for (int i = 0; i < length / 4; i++)
                                    message += BitConverter.ToUInt32(data_U4, length - (i + 1) * 4).ToString() + " ";
                                break;
                            }
                        default:
                            {
                                message += "ASCII = ";
                                for (int i = 0; i < length; i++)
                                    message += Convert.ToChar(Convert.ToUInt32(Hex_string_to_Dec(RawHex[index + length_byte + 1 + i]))).ToString();
                                message += " ";
                                break;
                            }
                    }
                }
                message += ">\r\n";
                later_index = index + length_byte + length + 1;
            }
            return message;
        }
        private static int Hex_string_to_Dec(string Hex)
        {
            return Convert.ToInt32(Hex[0].ToString(), 16) * 16 + Convert.ToInt32(Hex[1].ToString(), 16);
        }
        private static int ConvertToInt8(string Hex)
        {
            int data_I1 = Hex_string_to_Dec(Hex);
            if (data_I1 < 128)
                return data_I1;
            else
                return (Convert.ToByte(data_I1) & 0x7f) - 128;
        }
    }
}
