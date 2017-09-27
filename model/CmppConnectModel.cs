using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// 连接
    /// </summary>
    public class CmppConnectModel
    {

        public string Password { get; set; }
        /// <summary>
        /// 源地址，此处为SP_Id，即SP的企业代码。
        /// </summary>
        public string Source_Addr	{get;set;}


       public string AuthenticatorSource{get;set;}

        /// <summary>
       /// 双方协商的版本号(高位4bit表示主版本号,低位4bit表示次版本号)
        /// </summary>
       public uint Version { get; set; }

        /// <summary>
        /// 时间戳的明文,由客户端产生,格式为MMDDHHMMSS，即月日时分秒，10位数字的整型，右对齐 。
        /// </summary>
       public uint Timestamp { get; set; }

    }
}
