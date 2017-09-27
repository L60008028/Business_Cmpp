using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cmppV2
{
    /// <summary>
    /// 源ISMG请求拆除到目的ISMG的连接（CMPP¬_TERMINATE）操作
    /// </summary>
    public class CMPP_TerminateDecode : DePackage
    {
        public uint Reserved { get; set; }

        public override void Decode(System.IO.BinaryReader br)
        {
            Reserved = this.ReadUInt(br);
        }
    }
}
