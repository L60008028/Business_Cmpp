using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// 头实体
    /// </summary>
    public class CmppHeaderModel
    {
        /// <summary>
        /// 消息总长度(含消息头及消息体)
        /// </summary>
        public uint Total_Length { get; set; }

        /// <summary>
        /// 命令或响应类型
        /// </summary>
        public uint Command_Id { get; set; }


        /// <summary>
        /// 消息流水号,顺序累加,步长为1,循环使用（一对请求和应答消息的流水号必须相同）
        /// </summary>
        public uint Sequence_Id { get; set; }

 

    }
}
