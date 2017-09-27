using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;

namespace cmppV2
{
    /// <summary>
    /// 打包心跳包
    /// </summary>
    public class CMPP_ActiveTest:EnPackage
    {

        public override byte[] Encode()
        {
            CmppHeaderModel head = new CmppHeaderModel();
            head.Command_Id = (uint)Cmpp_Command.CMPP_ACTIVE_TEST;
            head.Total_Length =GlobalModel.HeaderLength;
            head.Sequence_Id = Tools.GetSequence_Id();
            return this.Header(head);
        }
    }//end
}
