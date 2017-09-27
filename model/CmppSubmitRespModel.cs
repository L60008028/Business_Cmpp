using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// 提交返回
    /// </summary>
    public class CmppSubmitRespModel
    {
        public UInt64 Msg_Id { get; set; }
        public uint Result { get; set; }

    }
}
