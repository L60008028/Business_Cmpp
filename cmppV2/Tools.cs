using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using model;

namespace cmppV2
{
    public class Tools
    {

        /// <summary>
        /// 合并两个字节数组
        /// </summary>
        /// <param name="b1">源字节数组1</param>
        /// <param name="b2">源字节数组2</param>
        /// <returns>合并后的字节数组</returns>
        public static byte[] MergeByteArray(byte[] b1, byte[] b2)
        {
            byte[] bs = new byte[b1.Length + b2.Length];

            Array.Copy(b1, 0, bs, 0, b1.Length);
            Array.Copy(b2, 0, bs, b1.Length, b2.Length);
            return bs;
        }

        /// <summary>
        /// int转为2个byte
        /// </summary>
        /// <param name="n">整型数</param>
        /// <param name="buf">输出数组</param>
        /// <param name="offset">buf的偏移量</param>
        public static void int2bytes2(int n, byte[] buf, int offset)
        {
            buf[offset] = (byte)(n >> 8);
            buf[offset + 1] = (byte)n;
        }

        /// <summary>
        /// int转为3个byte
        /// </summary>
        /// <param name="n">整型数</param>
        /// <param name="buf">输出数组</param>
        /// <param name="offset">buf的偏移量</param>
        public static int bytes2int2(byte[] b, int offset)
        {
            return (b[offset + 2] & 0xff) | (b[offset + 1] & 0xff) << 8
                    | (b[offset] & 0xff) << 16;
        }

        /// <summary>
        /// 2个byte转为short
        /// </summary>
        /// <param name="b">源字节数组</param>
        /// <param name="offset">数组偏移量</param>
        /// <returns>结果short</returns>
        public static short byte2short(byte[] b, int offset)
        {
            return (short)(b[offset + 1] & 0xff | (b[offset] & 0xff) << 8);
        }

        /// <summary>
        /// 2个byte转为short
        /// </summary>
        /// <param name="b">源字节数组</param>
        /// <returns>结果short</returns>
        public static short byte2short(byte[] b)
        {
            return (short)(b[1] & 0xff | (b[0] & 0xff) << 8);
        }

        /// <summary>
        /// 1个byte转为short
        /// </summary>
        /// <param name="b">源字节数组</param>
        /// <param name="offset">数组偏移量</param>
        /// <returns>结果short</returns>
        public static short byte2tinyint(byte[] b, int offset)
        {
            return (short)(b[offset] & 0xff);
        }



        public static string[] GetMsgArray(int radix, string content)
        {
            
            int contentLength = 0;
            contentLength = content.Length;

            int size = contentLength / radix;
            size = (contentLength - size * radix) > 0 ? size + 1 : size; 
            string[] oneContent = new string[size];
            for (int i = 0; i < size; i++)
            {
                int start = i * radix;
                int end = (i + 1) * radix;
                if (end > contentLength)
                {
                    end = contentLength;
                }
                int ilen = end - start;
                if (ilen > 0)
                {
                    oneContent[i] = content.Substring(start, end - start);
                    
                }
            }
            return oneContent;
        }


        public static bool CheckCommand(uint id)
        {
            if (((uint)Cmpp_Command.CMPP_SUBMIT == id) ||
                ((uint)Cmpp_Command.CMPP_SUBMIT_RESP == id) ||
                ((uint)Cmpp_Command.CMPP_DELIVER == id) ||
                ((uint)Cmpp_Command.CMPP_DELIVER_RESP == id) ||
                ((uint)Cmpp_Command.CMPP_CONNECT == id) ||
                ((uint)Cmpp_Command.CMPP_CONNECT_RESP == id) ||
                ((uint)Cmpp_Command.CMPP_TERMINATE == id) ||
                ((uint)Cmpp_Command.CMPP_TERMINATE_RESP == id) ||
                ((uint)Cmpp_Command.CMPP_ACTIVE_TEST == id) ||
                ((uint)Cmpp_Command.CMPP_ACTIVE_TEST_RESP == id))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 时间戳/msgid
        /// </summary>
        /// <returns></returns>
        public static uint  Timestamp()
        {
            return Convert.ToUInt32(DateTime.Now.ToString("MMddHHmmss"));
        }
        /// <summary>
        /// 取序号
        /// </summary>
        /// <returns></returns>
        public static uint GetSequence_Id()
        {
            if (GlobalModel.Sequence_Id >= GlobalModel.HeaderMaxSeqID)
            {
                GlobalModel.Sequence_Id = 0;
            }
            else
            {
                GlobalModel.Sequence_Id++;
            }
            return GlobalModel.Sequence_Id;
        }
        /// <summary>
        /// 短信内容转byte[]
        /// </summary>
        /// <param name="msg">短信内容</param>
        /// <param name="fmt">Msg_Fmt</param>
        /// <returns></returns>
        public static byte[] MsgContentToBytes(string msg,MsgContentFormat fmt)
        {
            byte[] result;
            switch(fmt)
            {
                case MsgContentFormat.ASCII:
                    result=Encoding.ASCII.GetBytes(msg);
                    break;
                case MsgContentFormat.WRITE:
                case MsgContentFormat.BINARY:
                    result = Convert.FromBase64String(msg);
                    break;
                case MsgContentFormat.GBK:
                    result= Encoding.Default.GetBytes(msg);
                    break;
                case MsgContentFormat.UCS2:
                    result = Encoding.BigEndianUnicode.GetBytes(msg);
                    break;       
                default:
                    result = Encoding.ASCII.GetBytes(msg);
                    break;
            }
            return result;
        }
        /// <summary>
        /// 返回短信存活时间48小时
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string GetValIdTime(DateTime d) 
        {
            DateTime n = d.AddHours(48); //24小时
            return (n.Year.ToString().Substring(2) + n.Month.ToString().PadLeft(2, '0') + n.Day.ToString().PadLeft(2, '0') + n.Hour.ToString().PadLeft(2, '0') + n.Minute.ToString().PadLeft(2, '0') + n.Second.ToString().PadLeft(2, '0') + "032+");
        }

        public static  byte[] MakeMd5AuthCode(string account, string password, string tmstamp)
        {
            //创建MD5类别
            MD5 md5 = new MD5CryptoServiceProvider();
            int offset;
            byte[] buf;

            if (tmstamp.Length > 10)
            {
                tmstamp = tmstamp.Substring(0, 10);
            }

            buf = new byte[account.Length + 9 + password.Length + tmstamp.Length];
            Array.Clear(buf, 0, buf.Length);
            offset = 0;
            Array.Copy(Encoding.ASCII.GetBytes(account), 0, buf, offset, account.Length);
            offset += account.Length;
            offset += 9;
            Array.Copy(Encoding.ASCII.GetBytes(password), 0, buf, offset, password.Length);
            offset += password.Length;
            Array.Copy(Encoding.ASCII.GetBytes(tmstamp), 0, buf, offset, tmstamp.Length);

            return md5.ComputeHash(buf);
        }

        /// <summary>
        /// 返回一个时间戳字符串
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            DateTime tm = DateTime.Now;
            string u = tm.Month.ToString("00");
            u += tm.Day.ToString("00");
            u += tm.Hour.ToString("00");
            u += tm.Minute.ToString("00");
            u += tm.Second.ToString("00");
            return u;
        }

        public static byte[] GetSubBytes(byte[] buffer, int index, int count)
        {
            if (count + index > buffer.Length)
            {
                count = buffer.Length - index;
            }
            MemoryStream memoryStream = new MemoryStream(buffer, index, count);
            byte[] b = memoryStream.ToArray();
            memoryStream.Close();
            return b;
        }


        /// <summary>
        /// MD5 16位加密
        /// </summary>
        /// <param name="ConvertString">源字符串</param>
        /// <returns>加密后的字节数组</returns>
        public static byte[] GetMd5Bytes(string source)
        {
 
            byte[] bytes = Encoding.ASCII.GetBytes(source);
            byte[] hashValue = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
            return hashValue;
        }

        /// <summary>
        /// 检测字符串是否含有中文
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>true：含中文；false：不含中文</returns>
        public static bool ChenkIsIncludChinese(string str)
        {
            //\u4e00-\u9fa5 汉字的范围。
            //^[\u4e00-\u9fa5]$ 汉字的范围的正则
            bool b = false;
            for (int i = 0; i < str.Length; i++)
            {
                Regex rx = new Regex(@"^[\u4e00-\u9fa5]$");
                if (rx.IsMatch(str[i].ToString()))
                {
                    b = true;
                }
            }
            return b;
        }


        /// <summary>
        /// 将字节数组转换为16进制串
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ByteToHex(byte[] bs, int offset, int length)
        {
            StringBuilder sbResult = new StringBuilder(length * 2);

            for (int i = 0; i < length; i++)
            {
                sbResult.Append(bs[offset].ToString("X2"));
                sbResult.Append(" ");
                offset++;
            }

            return sbResult.ToString();
        }

    }//end
}//end
