using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;

namespace cmppV2
{
    /// <summary>
    /// 解析头
    /// </summary>
    public class Cmpp_HeaderDecode:DePackage
    {
        public CmppHeaderModel Header { get; set; }
        public override void Decode(System.IO.BinaryReader br)
        {
            CmppHeaderModel hh = new CmppHeaderModel();
            hh.Total_Length =(uint) this.ReadInt(br, 4);
            hh.Command_Id = (uint)this.ReadInt(br,4);
            hh.Sequence_Id = (uint)this.ReadInt(br, 4);
            this.Header = hh;
        }
    }//end
}//end
