using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using model;

namespace cmppV2
{
    /// <summary>
    /// 打包状态响应
    /// </summary>
    public class CMPP_DeliverResp:EnPackage
    {

        private UInt64 _msg_Id;//8字节
        private uint _result = 0;//结果
        private uint _seq;//序号
        public CMPP_DeliverResp(UInt64 msg_Id, uint result, uint seq)
        {
            _msg_Id = msg_Id;
            _result = result;
            _seq = seq;
        }
        public override byte[] Encode()
        {
            CmppHeaderModel head = new CmppHeaderModel();
            head.Command_Id = (uint)Cmpp_Command.CMPP_DELIVER_RESP;
            head.Total_Length = GlobalModel.HeaderLength + 9;
            head.Sequence_Id = _seq;
            byte[] headb = this.Header(head);
            ArrayList al = new ArrayList(headb);
            al.AddRange(this.Uint64ToBytes(_msg_Id,8));
            al.AddRange(this.Uint1ToBytes(_result));
            return al.ToArray(typeof(byte)) as byte[];

        }
    }//end
}//end
