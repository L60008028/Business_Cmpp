using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using System.IO;

namespace cmppV2
{
    /// <summary>
    /// 解析提交返回
    /// </summary>
    public class Cmpp_SubmitDecode:DePackage
    {
        /// <summary>
        /// 信息标识
        /// </summary>
        public UInt64 MsgId { get; set; }
        /// <summary>
        /// 0：正确
        /// 1：消息结构错
        /// 2：命令字错
        /// 3：消息序号重复
        /// 4：消息长度错
        /// 5：资费代码错
        /// 6：超过最大信息长
        /// 7：业务代码错
        /// 8：流量控制错
        /// 9~ ：其他错误
        /// </summary>
        public uint Result { get; set; }


        public override void Decode(BinaryReader br)
        {
            CmppSubmitRespModel sr = new CmppSubmitRespModel();
            MsgId = this.ReadUInt64(br);
            Result = this.ReadUInt(br);     
             
        }

    }//end
}//end
