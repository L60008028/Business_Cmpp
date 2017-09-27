using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class CmppSubmitLongModel
    {
        /// <summary>
        /// 消息流水号，通常用标识列
        /// </summary>
        public uint Sequence_Id { get; set; }


        /// <summary>
        /// 信息标识，由SP侧短信网关本身产生，本处填空。
        /// </summary>
        public uint Msg_Id { get; set; }

        /// <summary>
        /// 相同Msg_Id的信息总条数，从1开始
        /// </summary>
        public uint Pk_total { get; set; }

        /// <summary>
        /// 相同Msg_Id的信息序号，从1开始
        /// </summary>
        public uint Pk_number { get; set; }

        /// <summary>
        /// 是否要求返回状态确认报告：0：不需要1：需要2：产生SMC话单（该类型短信仅供网关计费使用，不发送给目的终端)
        /// </summary>
        public uint Registered_Delivery { get; set; }

        /// <summary>
        /// 信息级别
        /// </summary>
        public uint Msg_level { get; set; }

        /// <summary>
        /// 业务类型，是数字、字母和符号的组合。
        /// </summary>
        public string Service_Id { get; set; }

        /// <summary>
        /// 计费用户类型字段
        /// 0：对目的终端MSISDN计费；
        /// 1：对源终端MSISDN计费；
        /// 2：对SP计费;
        /// 3：表示本字段无效，对谁计费参见Fee_terminal_Id字段。
        /// </summary>
        public uint Fee_UserType { get; set; }

        /// <summary>
        /// 被计费用户的号码（如本字节填空，则表示本字段无效，对谁计费参见Fee_UserType字段，本字段与Fee_UserType字段互斥）
        /// </summary>
        public uint Fee_terminal_Id { get; set; }

        /// <summary>
        /// GSM协议类型。详细是解释请参考GSM03.40中的9.2.3.9
        /// </summary>
        public uint TP_pId { get; set; }

        /// <summary>
        /// GSM协议类型。详细是解释请参考GSM03.40中的9.2.3.23,仅使用1位，右对齐
        /// </summary>
        public uint TP_udhi { get; set; }

        /// <summary>
        /// 信息格式0：ASCII串 3：短信写卡操作4：二进制信息8：UCS2编码15：含GB汉字   
        /// </summary>
        public uint Msg_Fmt { get; set; }

        /// <summary>
        /// 信息内容来源(SP_Id)
        /// </summary>
        public string  Msg_src { get; set; }

        /// <summary>
        /// 资费类别
        /// 01：对“计费用户号码”免费
        /// 02：对“计费用户号码”按条计信息费
        /// 03：对“计费用户号码”按包月收取信息费
        /// 04：对“计费用户号码”的信息费封顶
        /// 05：对“计费用户号码”的收费是由SP实现
        /// </summary>
        public string  FeeType { get; set; }

        /// <summary>
        /// 资费代码（以分为单位）
        /// </summary>
        public string FeeCode { get; set; }

        /// <summary>
        /// 存活有效期，格式遵循SMPP3.3协议
        /// </summary>
        public string ValId_Time { get; set; }

        /// <summary>
        /// 定时发送时间，格式遵循SMPP3.3协议
        /// </summary>
        public string At_Time { get; set; }

        /// <summary>
        /// 源号码SP的服务代码或前缀为服务代码的长号码, 网关将该号码完整的填到SMPP协议Submit_SM消息相应的source_addr字段，该号码最终在用户手机上显示为短消息的主叫号码
        /// </summary>
        public string Src_Id { get; set; }

        /// <summary>
        /// 接收信息的用户数量(小于100个用户)
        /// </summary>
        public uint DestUsr_tl { get; set; }

        /// <summary>
        /// 接收短信的MSISDN号码
        /// </summary>
        public string Dest_terminal_Id { get; set; }

        /// <summary>
        /// 信息长度(Msg_Fmt值为0时：小于160个字节；其它小于140个字节)
        /// </summary>
        public uint Msg_Length { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>
        public string Msg_Content { get; set; }

        /// <summary>
        /// 保留
        /// </summary>
        public string Reserve { get; set; }

    }
}
