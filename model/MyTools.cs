using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace model
{
    public class MyTools
    {
        
        public static void SendSMS(string msg)
        {

            try
            {
                HttpHelper hh = new HttpHelper();
                string url = "http://121.40.143.163:6666/sms.aspx?action=send&userid=71&account=szjiaying0515&password=szjiaying0515&mobile=15820455385,18988776279,18926046543,13682488577&content=" + hh.UrlEncoderUTF8(msg);
                string re = hh.Get(url,"GBK");
                SMSLog.Debug("SendSMS==>re=" + re + ",url=" + url);
            }
            catch (Exception)
            {
 
            }
 
        }
        /// <summary>
        /// 等待
        /// </summary>
        /// <param name="t">毫秒</param>
        public static void WaitTime(int t)
        {
            try
            {
                ManualResetEvent mrv = new ManualResetEvent(false);
                mrv.WaitOne(t);

                //DateTime now = DateTime.Now;
                //while (now.AddMilliseconds(t) > DateTime.Now)
                //{
                //}
            }
            catch (Exception)
            {
 
            }
        }



        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string NoHTML(string Htmlstring)
        {

            try
            {
                //删除脚本

                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

                //删除HTML

                //Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);

                // Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);

                //Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);

                //Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

                Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

                // Htmlstring.Replace("<", "");

                // Htmlstring.Replace(">", "");

                //Htmlstring.Replace("\r\n", "");

                // Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            }
            catch (Exception ex)
            {

            }

            return Htmlstring.Trim();

        }


        public static void StopThread(Thread t)
        {
            try
            {
                if (t != null)
                {
                    t.Abort();
                    t.Join();
                }
            }
            catch (Exception ex)
            {

            }
        }



        public static int GetRand()
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return ran.Next();
        }

        public static byte[] HexStringToByte(string hex)
        {
            byte[] bb = new byte[hex.Length];
            try
            {
                string[] hexarray = hex.Split(' ');

                for (int i = 0; i < hexarray.Length; i++)
                {
                    bb[i] = byte.Parse(hexarray[i], System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception)
            {
 
            }
            return bb;
        
        }

        public static string ByteToHex(byte[] bs)
        {
            StringBuilder sbResult = new StringBuilder();

            try
            {
                for (int i = 0; i < bs.Length; i++)
                {
                    sbResult.Append(bs[i].ToString("X2"));
                    sbResult.Append(" ");
                }

            }
            catch (Exception)
            {
 
            }
            return sbResult.ToString();
        }


        public static string ClearSign(string str)
        {
            string re = str;

            try
            {
                int iStart = -1;
                int iEnd = -1;
                iStart = str.IndexOf("【");
                iEnd = str.IndexOf("】");
                if (iStart == 0 && iEnd > iStart)
                {
                    re = str.Substring(iEnd + 1); //去签名
                }
                iStart = re.LastIndexOf("【");
                iEnd = re.LastIndexOf("】");
                if (iEnd == re.Length - 1 && iStart < iEnd)
                {
                    re = re.Substring(0, iStart); //去签名
                }
            }
            catch (Exception ex)
            {
                SMSLog.Debug("COMMON.ClearSign==>Exception:" + ex.Message);
            }

            return re;
        }
        /// <summary>
        /// 前置签名变后置
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SignAfter(string str)
        {

            string re = str;
            try
            {
                int iStart = -1;
                int iEnd = -1;
                iStart = str.IndexOf("【");
                iEnd = str.IndexOf("】");
                re = str.Substring(iEnd + 1); //去签名
                string sign = str.Substring(0, iEnd + 1);//签名
                re = re + sign;
            }
            catch (Exception ex)
            {
                SMSLog.Debug("COMMON.SignAfter==>Exception:" + ex.Message);
            }
            return re;
        }
        /// <summary>
        /// 后置签名变签名前置
        /// </summary>
        /// <param name="str">内容</param>
        /// <returns></returns>
        public static string SignBefore(string str)
        {
            string re = str;
            try
            {
                int iStart = -1;
                int iEnd = -1;
                iStart = str.LastIndexOf("【");
                iEnd = str.LastIndexOf("】");
                re = str.Substring(0, iStart); //去签名
                string sign = str.Substring(iStart, (iEnd - iStart) + 1);//签名
                re = sign + re;
            }
            catch (Exception ex)
            {
                SMSLog.Debug("COMMON.SignBefore==>Exception:" + ex.Message);
            }
            return re;
        }

        /// <summary>
        /// 判断签名是否前置
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsFirstsign(string str)
        {

            try
            {
                str = str.Trim();
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }
                int beginix = str.IndexOf("【");
                int endix = str.IndexOf("】");
                // Console.WriteLine("beginix={0},endix={1},msglen={2}", beginix, endix, str.Length);
                if (beginix == 0 && endix > 1 && endix < str.Length - 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;

            }
            return false;
        }

        /// <summary>
        /// 判断签名是否后置
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLastsign(string str)
        {
            str = str.Trim();
            //string str = "发送信息给 1 个手机号码 【aaa】";
            bool isOk = false;
            try
            {
                if (str == null || str.Length == 0)
                    isOk = false;

                int iStart = -1;
                int iEnd = -1;
                iStart = str.LastIndexOf("【");
                iEnd = str.LastIndexOf("】");

                string stxt = "";
                stxt = str.Substring((iStart + 1), (iEnd - iStart - 1));
                //Console.WriteLine("iStart=" + iStart);
                //Console.WriteLine("iEnd=" + iEnd);
                //Console.WriteLine("str.Length=" + str.Length);
                //Console.WriteLine("stxt=" + stxt);
                int iNum = 0;
                if ((iStart <= 0) || (iEnd <= 0) || (iEnd != (str.Length - 1)) || (stxt.Length == 0))
                {
                    isOk = false;
                    Console.WriteLine("无签名....");
                }
                else
                {
                    isOk = true;
                }

            }
            catch (Exception ex)
            {
                isOk = false;
            }


            return isOk;
        }



 
    }//end
}
