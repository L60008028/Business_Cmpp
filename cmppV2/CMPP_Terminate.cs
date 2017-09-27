using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;

namespace cmppV2
{
    /// <summary>
    /// 请求拆除连接（CMPP_TERMINATE）操作
    /// </summary>
    public class CMPP_Terminate : EnPackage
    {
        public override byte[] Encode()
        {
            CmppHeaderModel head = new CmppHeaderModel();
            head.Command_Id = (uint)Cmpp_Command.CMPP_TERMINATE;
            head.Total_Length = GlobalModel.HeaderLength;
            head.Sequence_Id = Tools.GetSequence_Id();
            return this.Header(head);
        }
    }
}
