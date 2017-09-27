using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cmppV2
{
    /// <summary>
    /// 解析心跳包
    /// </summary>
    public class CMPP_ActiveTestDecode: DePackage
    {
        public uint Reserved { get; set; }
        public override void Decode(System.IO.BinaryReader br)
        {
            Reserved = this.ReadUInt(br);
        }
    }//end
}//end
