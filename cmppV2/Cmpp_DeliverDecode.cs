using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cmppV2
{
    /// <summary>
    /// 解析状态报告
    /// </summary>
   public  class Cmpp_DeliverDecode:DePackage
    {
        public override void Decode(System.IO.BinaryReader br)
        {
            Msg_Id=this.ReadUInt64(br);
            Dest_Id = this.ReadString(br, 21);
            Service_Id = this.ReadString(br,10);
            TP_pid = this.ReadUInt(br);
            TP_udhi = this.ReadUInt(br);
            Msg_Fmt = this.ReadUInt(br);
            Src_terminal_Id = this.ReadString(br, 21);
            Registered_Delivery = this.ReadUInt(br);
            Msg_Length = this.ReadUInt(br);
            if (Registered_Delivery == 1)
            {
                ReportMsgId = this.ReadUInt64(br);
                ReportStat = this.ReadString(br, 7);
                ReportSubmitTime = this.ReadString(br, 10);
                ReportDoneTime = this.ReadString(br, 10);
                ReportDestTerminalId = this.ReadString(br, 21);
                ReportSeqId = (uint)this.ReadInt32(br);
            }
            else
            {
                byte[] con=this.ReadBytes(br,(int)Msg_Length);
                switch (Msg_Fmt)
                {
                    case (uint)MsgContentFormat.ASCII:
                        Msg_Content = Encoding.ASCII.GetString(con);
                        break;
                    case (uint)MsgContentFormat.GBK:
                        Msg_Content = Encoding.Default.GetString(con);
                        break;
                    case (uint)MsgContentFormat.UCS2:
                        Msg_Content = Encoding.BigEndianUnicode.GetString(con);
                        break;
                    case (uint)MsgContentFormat.BINARY:
                    case (uint)MsgContentFormat.WRITE:
                        Msg_Content = Tools.ByteToHex(con, 0, (int)Msg_Length);
                        break;
                    default:
                        Msg_Content = "";
                        break;
                }
                
            }
        }

        //状态报告定义的字段

       /// <summary>
       /// 信息标识SP提交短信（CMPP_SUBMIT）操作时，与SP相连的ISMG产生的Msg_Id。
       /// </summary>
        public UInt64 ReportMsgId{get;set;} 

       /// <summary>
        /// 发送短信的应答结果
       /// </summary>
        public string ReportStat { get; set; }

       /// <summary>
        /// YYMMDDHHMM（YY为年的后两位00-99，MM：01-12，DD：01-31，HH：00-23，MM：00-59）
       /// </summary>
        public string ReportSubmitTime { get; set; }

       /// <summary>
        /// YYMMDDHHMM
       /// </summary>
        public string ReportDoneTime { get; set; }

       /// <summary>
        /// 目的终端MSISDN号码(SP发送CMPP_SUBMIT消息的目标终端)
       /// </summary>
        public string ReportDestTerminalId { get; set; }

       /// <summary>
        /// 取自SMSC发送状态报告的消息体中的消息标识。
       /// </summary>
        public uint ReportSeqId { get; set; }



        /// <summary>
        /// 8位,信息标识，由SP侧短信网关本身产生，本处填空。
        /// </summary>
        public UInt64 Msg_Id { get; set; }

       /// <summary>
       /// 21位,目的号码
       /// </summary>
       public string Dest_Id{get;set;}

       /// <summary>
       /// 10位,业务类型，是数字、字母和符号的组合。
       /// </summary>
       public string  Service_Id{get;set;}

       /// <summary>
       /// 1位,GSM协议类型
       /// </summary>
        public uint TP_pid{get;set;}

       /// <summary>
       /// 1位,GSM协议类型
       /// </summary>
        public uint TP_udhi{get;set;}

       /// <summary>
       /// 1位,信息格式
       /// 0：ASCII串
       /// 3：短信写卡操作
       /// 4：二进制信息
       /// 8：UCS2编码
       /// 15：含GB汉字 
       /// </summary>
        public uint Msg_Fmt{get;set;}

       /// <summary>
       /// 21位，源终端MSISDN号码（状态报告时填为CMPP_SUBMIT消息的目的终端号码）
       /// </summary>
        public string Src_terminal_Id{get;set;}

       /// <summary>
       /// 1位，是否为状态报告0：非状态报告1：状态报告
       /// </summary>
        public uint Registered_Delivery { get; set; }
 

        /// <summary>
        /// 1位，信息长度(Msg_Fmt值为0时：小于160个字节；其它小于140个字节)
        /// </summary>
        public uint Msg_Length { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>
        public string Msg_Content { get; set; }

        /// <summary>
        /// 8位，保留
        /// </summary>
        public string Reserve { get; set; }


    }//end
}//end
