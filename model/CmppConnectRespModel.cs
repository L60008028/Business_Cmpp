using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// 连接返回
    /// </summary>
    public class CmppConnectRespModel
    {
        /// <summary>
        /// 状态
        /// 0：正确
        /// 1：消息结构错
        /// 2：非法源地址
        /// 3：认证错
        /// 4：版本太高
        /// 5~ ：其他错误
        /// </summary>
        public uint Status { get; set; }


        /// <summary>
        /// ISMG认证码，用于鉴别ISMG
        /// </summary>
        public string AuthenticatorISMG { get; set; }


        /// <summary>
        /// 服务器支持的最高版本号
        /// </summary>
        public uint Version { get; set; }

    }//end
}
