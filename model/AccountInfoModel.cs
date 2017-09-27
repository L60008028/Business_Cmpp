using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// cmpp道账号信息
    /// </summary>
    public  class AccountInfoModel
    {
        /// <summary>
        /// 企业ID
        /// </summary>
        public int eprId { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string loginname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }


        /// <summary>
        /// 发送号码
        /// </summary>
        public string spnumber { get; set; }

        /// <summary>
        /// 信息内容来源(SP_Id)
        /// </summary>
        public string spid { get; set; }

        /// <summary>
        /// 服务id
        /// </summary>
        public string serviceid { get; set; }

        /// <summary>
        /// 提交频率毫秒
        /// </summary>
        public int senddelay { get; set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        public string protocolType { get; set; }

        /// <summary>
        /// 服务IP
        /// </summary>
        public string serviceIp { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int servicePort { get; set; }



    }
}
