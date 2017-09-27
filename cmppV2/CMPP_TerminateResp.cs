using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;

namespace cmppV2
{
    public class CMPP_TerminateResp : EnPackage
    {
        public uint SequenceId { get; set; }
        public override byte[] Encode()
        {
            CmppHeaderModel head = new CmppHeaderModel();
            head.Command_Id = (uint)Cmpp_Command.CMPP_TERMINATE_RESP;
            head.Total_Length = GlobalModel.HeaderLength;
            head.Sequence_Id = SequenceId;
            return this.Header(head);
        }
    }
}
