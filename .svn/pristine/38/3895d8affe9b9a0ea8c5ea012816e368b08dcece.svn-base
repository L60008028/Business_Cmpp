using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using System.Collections;

namespace cmppV2
{
    /// <summary>
    /// 长知信提交,打包
    /// </summary>
    public class Cmpp_SubmitLong:EnPackage
    {
        CmppSubmitLongModel _submit;
        public Cmpp_SubmitLong(CmppSubmitLongModel submit)
        {
            _submit = submit;
        }
        public override byte[] Encode()
        {
            ArrayList body = new ArrayList();
            body.AddRange(this.UintToBytes(_submit.Msg_Id,8));
            body.AddRange(this.Uint1ToBytes(_submit.Pk_total));
            body.AddRange(this.Uint1ToBytes(_submit.Pk_number));
            body.AddRange(this.Uint1ToBytes(_submit.Registered_Delivery));
            body.AddRange(this.Uint1ToBytes(_submit.Msg_level));
            body.AddRange(this.StringToBytes(_submit.Service_Id,10));
            body.AddRange(this.Uint1ToBytes(_submit.Fee_UserType));
            body.AddRange(this.UintToBytes(_submit.Fee_terminal_Id,21));
            body.AddRange(this.Uint1ToBytes(_submit.TP_pId));
            body.AddRange(this.Uint1ToBytes(_submit.TP_udhi));
            body.AddRange(this.Uint1ToBytes(_submit.Msg_Fmt));
            body.AddRange(this.StringToBytes(_submit.Msg_src,6));
            body.AddRange(this.StringToBytes(_submit.FeeType,2));
            body.AddRange(this.StringToBytes(_submit.FeeCode,6));
            body.AddRange(this.StringToBytes(_submit.ValId_Time, 17));
            body.AddRange(this.StringToBytes(_submit.At_Time, 17));
            body.AddRange(this.StringToBytes(_submit.Src_Id, 21));
            body.AddRange(this.Uint1ToBytes(_submit.DestUsr_tl));
            body.AddRange(this.StringToBytes(_submit.Dest_terminal_Id, 21));          
            byte[] msgb = Tools.MsgContentToBytes(_submit.Msg_Content, (MsgContentFormat)_submit.Msg_Fmt);
            body.AddRange(this.Uint1ToBytes((uint)_submit.Msg_Length));
            TP_UdhiHeader udhi = new TP_UdhiHeader(_submit.Pk_total, _submit.Pk_number);//长短信标识
            body.AddRange(udhi.Encode());
            body.AddRange(msgb);
            body.AddRange(this.StringToBytes(_submit.Reserve, 8));
            byte[] bodybt = body.ToArray(typeof(byte)) as byte[];

            //头
            CmppHeaderModel head = new CmppHeaderModel();
            head.Command_Id = (uint)Cmpp_Command.CMPP_SUBMIT;
            head.Total_Length =(uint) bodybt.Length+GlobalModel.HeaderLength;
            head.Sequence_Id = _submit.Sequence_Id;
        
            ArrayList submit = new ArrayList(this.Header(head));
            submit.AddRange(bodybt);
            byte[] submitb = submit.ToArray(typeof(byte)) as byte[];
            return submitb;
        }

  

    }//end
}//end
