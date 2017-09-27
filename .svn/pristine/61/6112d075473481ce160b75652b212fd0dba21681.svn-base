using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using System.Collections;

namespace cmppV2
{
    /// <summary>
    /// 打包激活响应包
    /// </summary>
    public class CMPP_ActiveTestResp : EnPackage
    {
        public uint SequenceId { get; set; }
        private uint _reserved;
        public override byte[] Encode()
        {
            CmppHeaderModel head = new CmppHeaderModel();
            head.Command_Id = (uint)Cmpp_Command.CMPP_ACTIVE_TEST_RESP;
            head.Total_Length = GlobalModel.HeaderLength+1;
            _reserved=head.Sequence_Id = SequenceId;
            byte[] headb = this.Header(head);
            ArrayList al = new ArrayList(headb);
            al.AddRange(this.Uint1ToBytes(_reserved));
            return al.ToArray(typeof(byte)) as byte[];
        }
    }
}
