using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class MyDelegate
    {
        public delegate void WriteLogDelegate(string msg);
        public delegate void WriteLog4Delegate(string msg,string type);
        public delegate void UpdateMobileSubmitStateDelegate(string id, string state, string msgid, string remark, string srcno);
       
        public delegate void UpdateMobileByBatchNumDelegate(int state, string batchnum, string errorCode, string errMsg);
        public delegate void UpdateSMSContentSubmitStatuDelegate(string id, int cTSubmitStatus);
        public delegate void DeleteContentDelegate(string id);
        

        public delegate void DelFromWaitingQueueDelegate(uint seq);
        public delegate void TransferDataProc(int id, int operatorId, int submitstatus, string msgid);
        public delegate void TransferDataDelegate(int id,int operatorId);
        public delegate void UpdateSubmitStateDelegate(string[] args);
        public delegate void UpdateReportStateDelegate(string[] args);
        public delegate void SaveMoDelegate(string[] args);

        public delegate void SetStatusStripInfoDelegate(bool b);
    }
}
