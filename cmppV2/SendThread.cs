using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using System.Collections;
using System.Threading;
namespace cmppV2
{
    public class SendThread
    {
        public static AutoResetEvent thisqueueState = new AutoResetEvent(false);//队列信号
        private long MASG_TIMEOUT_TICKs = 30;// 消息失败时间 30秒
        private int CMPP_ACTIVE_TEST_N_COUNT = 3;//3次 

        private Timer _sendtimer;//发送
        private Timer _checkResendtimer;
        private Thread _sendthread;
        private ManualResetEvent _manualsend = new ManualResetEvent(false);
        private Queue<QueueItem> _prioritySmsQueue = null;//优先队列
        private Queue<QueueItem> _normalSmsQueue = null;//普通队列
        private SortedList _waitingSeqQueue = new SortedList(); //等待回应队列 
        private bool _isSend = true;
        private SocketClient _socketClient;
        private AccountInfoModel _accountInfo;
        //private LocalParams lp;
        public SendThread(AccountInfoModel accountInfo)
        {
            // lp = new LocalParams();
            _accountInfo = accountInfo;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            try
            {
                _socketClient = new SocketClient(_accountInfo.loginname, _accountInfo.password, _accountInfo.serviceIp, _accountInfo.servicePort);
                _socketClient.DelFromWaitingQueueHandler = this.DelFromWaitingQueue;
                _socketClient.AccountInfo = _accountInfo;
                bool b = _socketClient.Login();
                return b;
            }
            catch (Exception e)
            {

                SMSLog.Error("CmppSendThread==>Login()=>Exception:" + e.Message);
            }
            return false;
        }

        public void Exit()
        {
            try
            {
                SMSLog.Log("==========SendThread=======Exit=======");
                //thisqueueState.WaitOne();

                try
                {
                    if (_sendtimer != null)
                    {
                        _sendtimer.Dispose();
                    }
                }
                catch (Exception)
                {

                }
                SMSLog.Log("=>SendThread===Exit==_sendtimer...");

                try
                {

                    if (_checkResendtimer != null)
                    {
                        _checkResendtimer.Dispose();
                    }
                }
                catch (Exception)
                {

                }

                SMSLog.Log("=>SendThread===Exit==_checkResendtimer...");


                _socketClient.StopMe();
                SMSLog.Log("=>SendThread===Exit===停止发送线程...");
            }
            catch (Exception ex)
            {

                SMSLog.Error("CmppSendThread==>Exit Exception：" + ex.Message);
            }
           // _isSend = false;
        }


        public bool Start()
        {
            try
            {
                _prioritySmsQueue = new Queue<QueueItem>();
                _normalSmsQueue = new Queue<QueueItem>();
                //_socketClient = new SocketClient(_accountInfo.loginname, _accountInfo.password);
                // _socketClient.WriteLogHandler = WriteLogHandler;
                //_socketClient.ConnAsync();//连接，登录

                object sendLock = new object();

                //多线程
                // TimerCallback tc = new TimerCallback(Send);
                // _sendtimer = new Timer(tc, sendLock, 10, GlobalModel.Lparams.Senddelay);

                //单线程
                _sendthread = new Thread(new ThreadStart(Send));
                _sendthread.IsBackground = true;
                _sendthread.Start();


                if (GlobalModel.Lparams.IsResend.Equals("1"))
                {
                    _checkResendtimer = new Timer(new TimerCallback(CheckReSend), sendLock, 10, 10000);
                    SMSLog.Debug("CmppSendThread==>Start CheckReSend...........");
                }
                else
                {
                    SMSLog.Debug("CmppSendThread==>Start不重复提交！");
                }
                return true;
            }
            catch (Exception ex)
            {
                _isSend = false;
                SMSLog.Error("CmppSendThread==>Start Exception：" + ex.Message);
            }
            return false;
        }


        public bool IsSend
        {
            get { return _isSend; }
            set { _isSend = value; }
        }

        public int GetQueuqNum()
        {
            try
            {
                int a = _normalSmsQueue.Count;
                int b = _prioritySmsQueue.Count;
                return a + b;
            }
            catch (Exception)
            {

            }
            return 0;
        }

        /// <summary>
        /// 添加普通队列
        /// </summary>
        /// <param name="sm"></param>
        public void AddNormalQueue(QueueItem qi)
        {
            try
            {
                lock (((ICollection)_normalSmsQueue).SyncRoot)
                {
                    _normalSmsQueue.Enqueue(qi);
                }
            }
            catch (Exception ex)
            {

                SMSLog.Error("CmppSendThread==>AddNormalQueue Exception：" + ex.Message);
            }
        }

        /// <summary>
        /// 添加到优先发送队列
        /// </summary>
        /// <param name="sm"></param>
        public void AddPriorityQueue(QueueItem qi)
        {
            try
            {
                lock (((ICollection)_prioritySmsQueue).SyncRoot)
                {
                    _prioritySmsQueue.Enqueue(qi);
                }
            }
            catch (Exception ex)
            {

                SMSLog.Error("CmppSendThread==>AddPriorityQueue Exception：" + ex.Message);
            }
        }


        private QueueItem GetMessage()
        {

            try
            {
                if (_prioritySmsQueue != null && _prioritySmsQueue.Count > 0)
                {

                    lock (((ICollection)_prioritySmsQueue).SyncRoot)
                    {

                        QueueItem smm = _prioritySmsQueue.Dequeue();
                        return smm;

                    }
                }


                if (_normalSmsQueue != null && _normalSmsQueue.Count > 0)
                {

                    lock (((ICollection)_normalSmsQueue).SyncRoot)
                    {

                        QueueItem smm = _normalSmsQueue.Dequeue();
                        return smm;

                    }
                }
            }
            catch (System.Exception e)
            {
                SMSLog.Error("【" + _accountInfo.eprId + "】SendService.GetMessage()=>Exception:" + e.Message);
            }
            // thisqueueState.Set();
            return null;
        }

        //单线程
        private void Send()
        {


            while (_isSend)
            {
                try
                {
                    _manualsend.WaitOne(GlobalModel.Lparams.Senddelay);
                    Submit();
                }
                catch (Exception ex)
                {
                    SMSLog.Error("CmppSendThread==>Submit Exception：" + ex.Message);
                }

            } 

        }


        //多线程
        private void Send(object obj)
        {
            try
            {
                Submit();
            }
            catch (Exception ex)
            {
                SMSLog.Error("CmppSendThread==>Submit Exception：" + ex.Message);
            }
        }

        ManualResetEvent longMsgWait = new ManualResetEvent(false);

        int i = 0;
        private void Submit()
        {
            try
            {
                //if (!_socketClient.IsLogin)
                //{
                //    GlobalModel.WaitSendQueue.Set();
                //    //thisqueueState.Set();
                //    continue;
                //}
                //if (GlobalModel.ServiceIsStop)
                //{
                //    GlobalModel.WaitSendQueue.Set();
                //    continue;
                //}
                QueueItem qi = this.GetMessage();
                //if(i==0)
                //{
                //    qi = new QueueItem();
                //    SmsModel s = new SmsModel();
                //    s.content = "【比亚迪】您好！400客服接到F3用户王文刚（13842636503）其它，需要贵店给予支持，详情请查阅CRM派单信息1,详情请查阅CRM派单信息2,详情请查阅CRM派单信息3！";
                //    s.content = "【比亚迪】Hello World!";
                //    s.mobile = "13682488577";
                //    s.id = 1;
                //    s.subId = "123";
                //    qi.MsgObj = s;
                //    i++;
                //}

                
                if (qi == null)
                {
                    GlobalModel.WaitSendQueue.Set();
                    //thisqueueState.Set();
                    return;
                }
                SmsModel sms = qi.MsgObj as SmsModel;
                if (sms != null)
                {
                    bool b = false;
                    try
                    {

                        if (!MyTools.IsFirstsign(sms.content) && !MyTools.IsLastsign(sms.content))
                        {
                            SMSLog.Debug("CmppSendThread=>Send无签名[" + sms.id + "]to[" + sms.mobile + "]content=" + sms.content);
                            GlobalModel.TransferDataProcHandler.BeginInvoke((int)sms.id, sms.operatorId, 31013, "", new AsyncCallback((IAsyncResult) => { }), null);
                            return;
                        }
                        if (!MyTools.IsFirstsign(sms.content) && MyTools.IsLastsign(sms.content))
                        {
                            sms.content = MyTools.SignBefore(sms.content);
                        }
                        //SMSLog.Debug("CmppSendThread=>Send[" + sms.id + "][" + sms.srcnum + "]to[" + sms.mobile + "]content=" + sms.content);

                        string srcid = sms.srcnum;//显示号码  
                        string con = sms.content.Trim();
                        int msgnum = 1;


                        if (con.Length < 71)
                        {
                            msgnum = 1;
                        }
                        else
                        {
                            msgnum = (int)Math.Ceiling(con.Length / 67.0);
                        }

                        //SMSLog.Debug("CmppSendThread=>msgnum=[" + msgnum + "]");
                        if (msgnum > 1)
                        {

                            CmppSubmitLongModel sub = new CmppSubmitLongModel();

                            sub.Sequence_Id = sms.id;
                            sub.Msg_Id = Tools.Timestamp();
                            sub.Pk_total = (uint)msgnum;
                            sub.Registered_Delivery = 1;
                            sub.Msg_level = 0;
                            sub.Service_Id = GlobalModel.Lparams.ServiceId;//_accountInfo.serviceid;
                            sub.Fee_UserType = 2;
                            sub.Fee_terminal_Id = 0;
                            sub.TP_pId = 0;
                            sub.TP_udhi = 1;
                            sub.Msg_Fmt = (uint)MsgContentFormat.UCS2;
                            sub.Msg_src = GlobalModel.Lparams.Spid;// _accountInfo.spid;
                            sub.FeeType = "02";
                            sub.FeeCode = "000001";
                            sub.At_Time = "";
                            sub.ValId_Time = Tools.GetValIdTime(DateTime.Now);
                            sub.Src_Id = srcid;
                            sub.DestUsr_tl = 1;
                            sub.Dest_terminal_Id = sms.mobile;
                            sub.Reserve = "";
                            string[] msgarr = Tools.GetMsgArray(67, con);
                            string[] scontent = msgarr;
                            //SMSLog.Error("SendThread==>Send==>【msgarr】" + msgarr.Length);

                            int iLength = msgarr.Length;
                            int m_LongMsg = 70;
                            string lastMsg = msgarr[iLength - 1];
                            if (lastMsg.Length > m_LongMsg)
                            {
                                iLength = iLength + 1;
                                scontent = new string[iLength];
                                for (int i = 0; i < (iLength - 2); i++)
                                {
                                    scontent[i] = msgarr[i];
                                }
                                string[] stmp = Tools.GetMsgArray(m_LongMsg, lastMsg);
                                scontent[iLength - 2] = stmp[0];
                                scontent[iLength - 1] = stmp[1];
                            }
                            else
                            {
                                scontent = msgarr;
                            }

                            for (int i = 0; i < scontent.Length; i++)
                            {
                                try
                                {
                                    string splitcon = scontent[i];
                                    sub.Pk_number = (uint)(i + 1);
                                    sub.Msg_Length = (uint)(Encoding.BigEndianUnicode.GetBytes(splitcon).Length + 6);//内容长度+6字节头
                                    sub.Msg_Content = splitcon;
                                    Cmpp_SubmitLong cs = new Cmpp_SubmitLong(sub);
                                    byte[] byt = cs.Encode();

                                    b = _socketClient.SendAsync(byt);
                                    SMSLog.Debug("SendThread=>Send[" + b + "] Hex[" + sms.id + "]to[" + sms.mobile + "]index=" + i + ",extnum=" + srcid + ",content=" + splitcon);
                                    //SMSLog.Debug("SendThread=>Send Hex[" + MyTools.ByteToHex(byt) + "]", false);
                                    if (b)
                                    {

                                        longMsgWait.WaitOne(10);
                                        //MyTools.WaitTime(10);
                                    }
                                    else
                                    {
                                        break;
                                    }


                                }
                                catch (Exception ex)
                                {
                                    SMSLog.Error("SendThread==>Send==>【(for)SendAsync 发送异常：】" + ex.ToString());
                                }


                            }

                        }
                        else
                        {
                            CmppSubmitModel sub = new CmppSubmitModel();
                            try
                            {
                                sub.Sequence_Id = sms.id;
                                sub.Msg_Id = Tools.Timestamp();
                                sub.Pk_total = 1;
                                sub.Pk_number = 1;
                                sub.Registered_Delivery = 1;
                                sub.Msg_level = 0;
                                sub.Service_Id = GlobalModel.Lparams.ServiceId;// _accountInfo.serviceid;
                                sub.Fee_UserType = 2;
                                sub.Fee_terminal_Id = 0;
                                sub.TP_pId = 0;
                                sub.TP_udhi = 0;
                                sub.Msg_Fmt = (uint)MsgContentFormat.UCS2;
                                sub.Msg_src = GlobalModel.Lparams.Spid;// _accountInfo.spid;
                                sub.FeeType = "02";
                                sub.FeeCode = "000001";
                                sub.At_Time = "";
                                sub.ValId_Time = Tools.GetValIdTime(DateTime.Now);
                                sub.Src_Id = srcid;
                                sub.DestUsr_tl = 1;
                                sub.Dest_terminal_Id = sms.mobile;
                                sub.Msg_Length = (uint)Encoding.BigEndianUnicode.GetBytes(sms.content.Trim()).Length;
                                sub.Msg_Content = sms.content;
                                sub.Reserve = "";
                            }
                            catch (Exception ex)
                            {
                                SMSLog.Error("SendThread==>_socketClient==>sub 发送异常：" + ex.StackTrace);
                            }

                            try
                            {
                                Cmpp_Submit cs = new Cmpp_Submit(sub);
                                byte[] byt = cs.Encode();
                                b = _socketClient.SendAsync(byt);
                                SMSLog.Debug("SendThread=>Send[" + b + "] Hex[" + sms.id + "]to[" + sms.mobile + "],extnum=" + srcid + ",content=" + sms.content);
                                //SMSLog.Debug("SendThread=>Send Hex[" + MyTools.ByteToHex(byt) + "]", false);
                            }
                            catch (Exception ex)
                            {
                                SMSLog.Error("SendThread==>_socketClient==>SendAsync 【发送异常】：" + ex.ToString());
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        b = false;
                        SMSLog.Error("SendThread==>Send 【发送异常】：" + ex.Message);

                    }

                    try
                    {
                        if (b)
                        {
                            qi.MsgState = (int)MSG_STATE.SENDED_WAITTING;

                            GlobalModel.TransferDataProcHandler.BeginInvoke((int)sms.id, sms.operatorId, 0, "", new AsyncCallback((IAsyncResult) => { }), null);

                            if (GlobalModel.Lparams.IsResend.Equals("1"))
                            {
                                AddToWaitingQueue(qi);//添加到等待队列
                            }
                        }
                        else
                        {
                            if (qi.FailedCount < 3)
                            {
                                qi.FailedCount++;
                                this.AddPriorityQueue(qi);
                                SMSLog.Debug("提交失败重新加入队列:CID=" + sms.cid + ",mobile=" + sms.mobile + ",CONTENT=" + sms.content);

                            }
                            else
                            {
                                GlobalModel.TransferDataProcHandler.BeginInvoke((int)sms.id, sms.operatorId, 31999, "", new AsyncCallback((IAsyncResult) => { }), null);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        SMSLog.Error("CmppSendThread==>Send 转移异常：" + ex.Message);
                    }

                }
                else
                {
                    GlobalModel.WaitSendQueue.Set(); 
                }

            }
            catch (Exception ex)
            {

                SMSLog.Error("CmppSendThread==>Send Exception：" + ex.Message);
            }
        }
        private void CheckReSend(object obj)
        {
            try
            {
                for (int i = 0; i < this._waitingSeqQueue.Count; i++)
                {
                    QueueItem q = (QueueItem)_waitingSeqQueue.GetByIndex(i);
                    if (q != null)
                    {
                        DateTime this_time = DateTime.Now; //去当前时间
                        TimeSpan t = this_time - q.InQueueTime;
                        if (t.TotalSeconds > MASG_TIMEOUT_TICKs) //达到超时时间
                        {
                            this.DelFromWaitingQueue(q); //从等待队列中删除 

                            //需要重新发送消息
                            if (q.FailedCount >= CMPP_ACTIVE_TEST_N_COUNT)
                            {
                                //报告消息发送失败
                                //GlobalModel.UpdateMobileSubmitStateHandler.BeginInvoke(q.Sequence + "", "-1", "", "提交失败", "", new AsyncCallback((IAsyncResult ar) => { }), null);
                                string[] args = { q.Sequence + "", "", "31010", "超过最大重发次数", "" };
                                GlobalModel.UpdateSubmitStateHandler.BeginInvoke(args, new AsyncCallback((IAsyncResult ar) => { }), null);
                                // q.MsgState =(int)MSG_STATE.SENDED_WAITTING; 
                            }
                            else
                            {
                                //尝试再次发送
                                q.InQueueTime = this_time;
                                q.FailedCount++;
                                q.MsgState = (int)MSG_STATE.SENDED_WAITTING;
                                SMSLog.Debug("从等待队列==》添加到待发队列：" + q.Sequence);
                                this.AddNormalQueue(q);
                            }


                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                SMSLog.Error("CmppSendThread==>CheckReSend: Exception：" + ex.Message);
            }


        }

        private void AddToWaitingQueue(QueueItem q)
        {

            try
            {
                lock (this._waitingSeqQueue.SyncRoot)
                {
                    if (!this._waitingSeqQueue.ContainsKey(q.Sequence))
                    {
                        //SMSLog.Debug("添加到等待队列：" + q.Sequence);
                        q.InQueueTime = DateTime.Now;
                        q.FailedCount = q.FailedCount + 1;
                        this._waitingSeqQueue.Add(q.Sequence, q);
                    }
                }
            }
            catch (Exception ex)
            {

                SMSLog.Error("CmppSendThread==>AddToWaitingQueue: Exception：" + ex.Message);
            }
        }


        private void DelFromWaitingQueue(QueueItem q)
        {
            try
            {
                lock (this._waitingSeqQueue.SyncRoot)
                {

                    //SMSLog.Debug("从等待队列删除QueueItem：" + q.Sequence);
                    this._waitingSeqQueue.Remove(q.Sequence);
                }
            }
            catch (Exception ex)
            {

                SMSLog.Error("CmppSendThread==>DelFromWaitingQueue: Exception：" + ex.Message);
            }

        }

        private void DelFromWaitingQueue(uint seq)
        {

            try
            {
                lock (this._waitingSeqQueue.SyncRoot)
                {
                    //SMSLog.Debug("从等待队列删除seq：" + seq);
                    this._waitingSeqQueue.Remove(seq);
                }
            }
            catch (Exception ex)
            {

                SMSLog.Error("CmppSendThread==>DelFromWaitingQueue: Exception：" + ex.Message);
            }

        }




    }//end
}//end
