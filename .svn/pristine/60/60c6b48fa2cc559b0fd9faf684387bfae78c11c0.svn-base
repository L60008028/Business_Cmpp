using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cmppV2
{
    /// <summary>
    /// TP_Udhi协议头，加到内容中
    /// </summary>
    public class TP_UdhiHeader:EnPackage
    {
        private uint _pkTotal;
        private uint _pkNumber;
        public TP_UdhiHeader(uint pkTotal, uint pkNumber)
        {
            _pkTotal = pkTotal;
            _pkNumber = pkNumber;
        }
        public override byte[] Encode()
        {
            byte[] tp_udhiHead = new byte[6];
            tp_udhiHead[0] = 0x05;//表示剩余协议头的长度
            tp_udhiHead[1] = 0x00;//表示随后的这批超长短信的标识位长度为1
            tp_udhiHead[2] = 0x03;//剩下短信标识的长度
            tp_udhiHead[3] = 0x01;//这批短信的唯一标志，事实上，SME(手机或者SP)把消息合并完之后，就重新记录，所以这个标志是否唯 一并不是很 重要。
            tp_udhiHead[4] = (byte)_pkTotal;//总条数
            tp_udhiHead[5] = (byte)_pkNumber;//第几条
            return tp_udhiHead;
        }
    }
}
