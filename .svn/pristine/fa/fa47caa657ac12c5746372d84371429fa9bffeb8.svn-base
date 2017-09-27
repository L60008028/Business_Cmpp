using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using model;

namespace cmppV2
{
    /// <summary>
    /// 解析登录返回结果
    /// </summary>
   public class Cmpp_LoginDecode:DePackage
    {

       public override void Decode(BinaryReader br)
       {
           CmppConnectRespModel cc = new CmppConnectRespModel();
           Status = this.ReadUInt(br);
           Console.WriteLine("登录status=" + cc.Status);
           AuthenticatorISMG = this.ReadString(br, 16);
           Version = this.ReadUInt(br);

       }

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
