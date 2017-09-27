using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace model
{
    /// <summary>
    /// 全局变量
    /// </summary>
    public class GlobalModel
    {
        public static uint HeaderLength = 12;//包头长
        public static uint HeaderMaxSeqID = 0x7FFFFFFE;//包头最大序列号
        public static uint Sequence_Id = 0;
        public static bool IsStopCollect = true;
        public static bool ServiceIsStop = true;
        public static bool IsLogScroll = true;
        public static bool IsShowLog=true;
        public static bool IsUseActiveMq = false;
        public static bool IsLogin = false;
        public static AutoResetEvent WaitSendQueue= new AutoResetEvent(false);//待发队列信号
        public static AutoResetEvent CollectDataAutoResetEvent = new AutoResetEvent(false);//读取信号
        private static LocalParams _lparams;
        public static LocalParams Lparams
        {
            get 
            { 
                if(_lparams==null)
                {
                    _lparams = new LocalParams();
                }
                return GlobalModel._lparams; 
            }
            set { GlobalModel._lparams = value; }
        }
 
        public static MyDelegate.WriteLogDelegate WriteLogHandler;

        public static MyDelegate.UpdateMobileSubmitStateDelegate UpdateMobileSubmitStateHandler;
        public static MyDelegate.UpdateReportStateDelegate UpdateReportStateHandler;
        public static MyDelegate.UpdateMobileByBatchNumDelegate UpdateMobileByBatchNumHandler;
        public static MyDelegate.UpdateSMSContentSubmitStatuDelegate UpdateSMSContentSubmitStatuHandler;
        public static MyDelegate.DeleteContentDelegate DeleteContentHandler;
        public static MyDelegate.TransferDataProc TransferDataProcHandler;
        public static MyDelegate.TransferDataDelegate TransferDataHandler;
        public static MyDelegate.UpdateSubmitStateDelegate UpdateSubmitStateHandler;
        public static MyDelegate.UpdateReportStateDelegate UpdateReprotStateHandler;
        public static MyDelegate.SaveMoDelegate SaveMoHandler;
        public static MyDelegate.SetStatusStripInfoDelegate SetStatusStripInfoHandler;

       
        


    }
}
