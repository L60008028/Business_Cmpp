using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class SmsModel
    {
       
         
 
        public List<ReportMsgidStatus> ReportMsgIdLst { get; set; }

        public int GetReportMsgIdCount
        {
            get
            {
                if (ReportMsgIdLst!=null)
                {
                    return ReportMsgIdLst.Count;
                }
                return 0;
            }
           
        }
        
        public void SetReportMsgIdState(string msgid,string state)
        {
            try
            {
                if(msgid.Equals(SaveReportMsgId))
                {
                    SaveReportState = state;
                }
                if (ReportMsgIdLst != null)
                {
                    foreach (ReportMsgidStatus rms in ReportMsgIdLst)
                    {
                        if (rms.Msgid.Equals(msgid))
                        {
                            rms.ReportState = state;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SMSLog.Error("SmsModel=>SetReportMsgIdState Exception:"+ex.Message);
            }
        }

        public bool IsUpdateReport
        {
            get
            {
                try
                {
                    if (ReportMsgIdLst != null)
                    {
                        foreach (ReportMsgidStatus rms in ReportMsgIdLst)
                        {
                            if (!string.IsNullOrEmpty(rms.ReportState))
                            {
                                return true;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                return false;
            }
        }

        public string SaveReportMsgId { get; set; }//接口返回并保存的msgid
        public string  SaveReportState { get; set; }//状态回执
        public string srcnum{get;set;}
        public uint id { get; set; }// 主键
        public int operatorId { get; set; }//网关编号

        public string content { get; set; }//内容

        public int eprId { get; set; }// 企业ID

        public string userId { get; set; }// 用户ID

        public string batchNum { get; set; }// 批次号(唯一)

        public string clientMsgId { get; set; }// 客户端msgId

        public string mobile { get; set; }// 发送的手机号码

        public string name { get; set; }// 发送人名称

        public string msgId { get; set; }// 短信编号

        public DateTime saveTime { get; set; }// 提交时间

        public DateTime sendTime { get; set; }// 发送时间

        public int gatewayNum { get; set; }// 网关编号

        public int submitStatus { get; set; }// 发送状态(未提交0，已提交1，失败-1)

        public int reportStatus { get; set; }// 回执状态

        public int smsCount { get; set; }// 短信数量（内容长度计算）

        public int sendCount { get; set; }// 发送次数（默认0）

        public string  subId {get;set; }//扩展号码

        public string contentId { get; set; }//内容表ID 


        public string cid { get; set; }

        public int sendId { get; set; }
    }//end

    public class ReportMsgidStatus
    {
        public string Msgid { get; set; }
        public string SubmitState { get; set; }
        public string ReportState { get; set; }
    }
}//end
