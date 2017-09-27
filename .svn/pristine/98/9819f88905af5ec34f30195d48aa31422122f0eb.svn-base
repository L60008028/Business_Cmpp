using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using model;
using System.Threading;

namespace cmppV2
{
    public class SocketClient
    {
        public MyDelegate.DelFromWaitingQueueDelegate DelFromWaitingQueueHandler;

        private int CMPP_ACTIVE_TEST_C_TICKs = 5;//长连接的active_test测试时间秒
        private int CMPP_ACTIVE_TCP_TICKs = 10;//连接测试时间秒

        private object this_locker = new object();
        private ManualResetEvent TimeoutObject = new ManualResetEvent(false);//连接超时
        private int timeoutMSec = 500;
        private bool _isConnectionSuccessful = false;//连接状态
        private bool _isLoginSuccessful = false;//登录状态
        private bool _isLogining = false;//正在登录
        private bool _isStartHeart = true;
        private bool _isStop = false;//本服务是否终止运行
        private Timer heartSend = null;//发送心跳包
        private Timer checkState = null;//socket状态检测
        private Thread _threadReceive;//接收线程
        private Thread _threadCheckState;//检查状态线程
        private DateTime LastActiveTime;//最后一次响应时间
        private bool _bNre = false;//空引用错误，套接字错误

        private Socket _socket;
        private string _ip = "127.0.0.1";// "192.168.1.202"; "221.131.129.1"
        private int _port = 7890;

        private string _account;
        private string _password;
        private int sendsmstimes = 0;


        public SocketClient(string account, string pwd, string ip, int port)
        {
            _account = account;
            _password = pwd;
            _ip = ip;
            _port = port;

        }

        /// <summary>
        /// CMPP账号信息
        /// </summary>
        public AccountInfoModel AccountInfo
        {
            get;
            set;
        }

        public bool IsConn
        {
            get
            {
                return _isConnectionSuccessful;
            }
        }

        public bool IsLogin
        {
            get
            {
                return _isLoginSuccessful;
            }
        }


        /// <summary>
        /// 同步连接
        /// </summary>
        /// <returns></returns>
        private Socket Conn()
        {
            try
            {
                // System.Threading.Interlocked.Increment(ref i);
                this.Close();
                SMSLog.Debug("---------Conn---连接-------------");
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(_ip), _port);
                IAsyncResult result = sock.BeginConnect(ipe, null, null); //连接 
                bool success = result.AsyncWaitHandle.WaitOne(1000, false);
                if (!success)
                {
                    SMSLog.Debug("---------Conn---连接--超时-----------");
                    try { sock.Close(); }
                    catch { }
                    throw new SocketException();
                }
                sock.EndConnect(result);
                _isConnectionSuccessful = true;
                SMSLog.Debug("---------Conn---连接--成功-----------");
                return sock;
            }
            catch (SocketException se)
            {
                _isConnectionSuccessful = false;
                _bNre = true;
                SMSLog.Error("--------Conn-----连接失败-------Exception:" + se.Message);
            }
            return null;
        }

        private object loginlocker = new object();
        /// <summary>
        /// 同步登录
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            lock (loginlocker)
            {
                try
                {
                    SMSLog.Debug("Login==>begin...");
                    Socket sock;
                    try
                    {
                        if (_isStop)
                        {
                            SMSLog.Debug("Login==>已停止不再登录...");
                            return false;
                        }

                        if (_isLoginSuccessful)
                        {
                            SMSLog.Debug("Login==>已登录成功...");
                            return true;
                        }

                        if (_isLogining)
                        {
                            SMSLog.Debug("Login==>正在登录...");
                            return false;
                        }
                        _isLogining = true;
                        sock = this.Conn();
                        GlobalModel.IsLogin = false;
                        if (sock == null)
                        {
                            SMSLog.Error("SocketClient==>Login()...sock连接短信中心失败....");
                            if (sendsmstimes < 1)
                            {
                               // MyTools.SendSMS("网关[" + GlobalModel.Lparams.GatewayName + "]连接短信中心失败");
                                sendsmstimes++;
                            }
                            _isLogining = false;
                            return false;
                        }

                        //发送登录验证包    
                        //uint seq = Tools.GetSequence_Id(); //取得一个流水号 
                        CmppConnectModel cc = new CmppConnectModel();
                        cc.Source_Addr = _account;
                        cc.Password = _password;
                        cc.Version =  (uint) GlobalModel.Lparams.Version;//0x20
                        cc.Timestamp = Convert.ToUInt32(DateTime.Now.ToString("MMddHHmmss"));
                        Cmpp_Login login = new Cmpp_Login(cc);
                        int sendlen = sock.Send(login.Encode());
                        SMSLog.Debug("连接成功发送登录包[" + sendlen + "]...");

                        if (!sock.Poll(1000000, SelectMode.SelectWrite))
                        {
                           
                            SMSLog.Error("=>cmpp::Login", "短信中心超时未应答，登录失败！");
                            sock.Close(); 
                            return _isLogining=false;
                        }

                    }
                    catch (SocketException se)
                    {
                        SMSLog.Error("登录异常【" + _account + "】：" + se.Message);
                        _isLogining = false;
                        return _isLogining = false;
                    }
                    DateTime t1 = DateTime.Now;

                    byte[] rbuf = new Byte[400];
                    int l;
                    try
                    {
                        l = sock.Receive(rbuf);
                        SMSLog.Debug("接收到数据长度：" + l);
                        if (l > 16)
                        {
                            MemoryStream ms = new MemoryStream(rbuf);
                            BinaryReader br = new BinaryReader(ms);
                            Cmpp_HeaderDecode head = new Cmpp_HeaderDecode();
                            head.Decode(br);//解析包头
                            SMSLog.Debug(head.Header.Command_Id + "");
                            if ((Cmpp_Command)head.Header.Command_Id == Cmpp_Command.CMPP_CONNECT_RESP)
                            {
                                Cmpp_LoginDecode login = new Cmpp_LoginDecode();
                                login.Decode(br);
                                if (login.Status == 0)
                                {
                                    _isLoginSuccessful = true;
                                    SMSLog.Debug("登录成功：[" + _account + "],version=0x" + login.Version.ToString("x8"));
                                    this.LastActiveTime = DateTime.Now;  //更新当前最后成功收发套接字的时间

                                }
                                else
                                {
                                    _isLoginSuccessful = false;
                                    SMSLog.Error("登录失败：[" + _account + "],result=" + login.Status);


                                }

                            }

                        }

                    }
                    catch (SocketException ex)
                    {
                        _isLoginSuccessful = false;
                        SMSLog.Error("登录接收异常：" + ex.Message);
                        if (sock != null)
                        {
                            sock.Close();
                        }
                    }



                    if (this._isLoginSuccessful)
                    {
                        sendsmstimes = 0;
                        this.LastActiveTime = DateTime.Now;
                        _socket = sock;
                        MyTools.StopThread(_threadReceive);
                        _threadReceive = new Thread(new ThreadStart(() => { Receive(_socket); }));
                        _threadReceive.Start();
                        //登录ok,就立即发送active_test包
                        CMPP_ActiveTest heart = new CMPP_ActiveTest();
                        this.Send(heart.Encode()); 
                        GlobalModel.IsLogin = true;
                        // 启动 主监视程序de线程
                        this.HeartStart();
                        _isLogining = false;
                        return (true);
                    }
                    else
                    {
                        if (sendsmstimes < 1)
                        {
                            MyTools.SendSMS("网关[" + GlobalModel.Lparams.GatewayName + "]登录失败");
                            sendsmstimes++;
                        }
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();
                        _isLogining = false;
                        return (false);
                    }

                }
                catch (Exception ex)
                {
                    SMSLog.Error("登录异常：" + ex.Message);
                }
                finally
                {
                    _isLogining = false;
                }
                return false;
            }
        }

        public void Close()
        {

            try
            {
                if (_socket != null)
                {
                    try
                    {
                        uint seq = Tools.GetSequence_Id();
                        CMPP_Terminate t = new CMPP_Terminate();
                        this.Send(t.Encode());
                        SMSLog.Debug("===Close==发送退出命令======");
                        // MyTools.WaitTime(100);
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        SMSLog.Error("Close Error", ex.Message);
                    }
                    
                    
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                }
            }
            catch
            {

            }


        }




        /// <summary>
        /// 异步连接
        /// </summary>
        /// <returns></returns>
        public bool ConnAsync()
        {
            try
            {
                _isConnectionSuccessful = false;
                TimeoutObject.Reset();
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(_ip), _port);
                _socket.BeginConnect(ipe, new AsyncCallback(ConnectCallBack), _socket);
                if (TimeoutObject.WaitOne(timeoutMSec, false))
                {
                    if (_isConnectionSuccessful && _isLoginSuccessful)
                    {
                        _isLogining = false;
                        return true;
                    }
                    else
                    {
                        SMSLog.Error("【" + _account + "】ConnAsync=====>连接超时");
                    }
                }
            }
            catch (Exception ex)
            {
                _bNre = true;
                SMSLog.Error("【" + _account + "】conn=====>" + ex.ToString());
            }
            return false;

        }

        /// <summary>
        /// 异步连接回调,登录
        /// </summary>
        /// <param name="ir"></param>
        private void ConnectCallBack(IAsyncResult ir)
        {
            try
            {

                if (ir.IsCompleted)
                {
                    Socket sok = ir.AsyncState as Socket;
                    if (sok != null && sok.Connected)
                    {
                        sok.EndConnect(ir);
                        _isConnectionSuccessful = true;
                        CmppConnectModel cc = new CmppConnectModel();
                        cc.Source_Addr = _account;
                        cc.Password = _password;
                        cc.Version = 0x20;
                        cc.Timestamp = Convert.ToUInt32(DateTime.Now.ToString("MMddHHmmss"));
                        Cmpp_Login login = new Cmpp_Login(cc);
                        SendAsync(login.Encode());
                        Receive(sok);
                    }
                    else
                    {
                        SMSLog.Error("【" + _account + "】====>ConnectCallBack(),连接失败");
                    }
                }
            }
            catch (Exception ex)
            {
                _isConnectionSuccessful = false;
                SMSLog.Error("【" + _account + "】====>ConnectCallBack()Exception:" + ex.ToString());
            }

        }



        /// <summary>
        /// 同步发送
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public bool Send(byte[] buff)
        {
            try
            {

 
                    //while (!_isLoginSuccessful)
                    //{
                    //    this.Login();
                    //}
                    if (_socket == null)
                    {
                        _isLoginSuccessful = false;
                        SMSLog.Error("SocketClient=>Send() _socket==null....");
                        return false;
                    }
                    if (_socket.Connected == false)
                    {
                        _isLoginSuccessful = false;
                        SMSLog.Error("SocketClient=>Send() _socket未连接.....");
                        return false;
                    }
                    int re = _socket.Send(buff);
                    if (re > 0 && re == buff.Length)
                    {
                       
                        return true;
                    }
                    else
                    {
                        _isLoginSuccessful = false;
                    }
                

            }
            catch (Exception ex)
            {
                _isLoginSuccessful = false;
                SMSLog.Error("【" + _account + "】SocketClient=>Send():" + ex.Message);
            }
            return false;
        }

        private object sendAsynclocker = new object();
        /// <summary>
        /// 异步发送 
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public bool SendAsync(byte[] buff)
        {
            try
            {


                lock (sendAsynclocker)
                {
                    if (!_isLoginSuccessful)
                    {
                        // Login();
                        // MyTools.WaitTime(50);
                        return false;
                    }
                    if (_socket == null)
                    {
                        _isLoginSuccessful = false;
                        SMSLog.Error("SocketClient=>SendAsync():_socket==null.........");
                        return false;
                    }
                    if (_socket.Connected == false)
                    {
                        _isLoginSuccessful = false;
                        SMSLog.Error("SocketClient=>SendAsync():_socket未连接.........");
                        return false;
                    }
                    IAsyncResult ir = _socket.BeginSend(buff, 0, buff.Length, 0, new AsyncCallback(SendAsyncCallback), _socket);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _isLoginSuccessful = false;
                _isConnectionSuccessful = false;
                //_bNre = true;
                SMSLog.Error("SocketClient=>SendAsync():异常" + ex.Message);
            }
            return false;
        }


        /// <summary>
        /// 异步发送回调
        /// </summary>
        /// <param name="ir"></param>
        public void SendAsyncCallback(IAsyncResult ir)
        {
            try
            {
                if (ir.IsCompleted)
                {

                    Socket sok = ir.AsyncState as Socket;
                    SocketError err;
                    int b = sok.EndSend(ir, out err);
                    if (b > 0 && err == SocketError.Success)
                    {
                        _isLoginSuccessful = true;
                    }
                    else
                    {
                        _isLoginSuccessful = false;
                    }
                }
            }
            catch (Exception ex)
            {

                SMSLog.Error("SocketClient::SendAsyncCallback:Exception:", ex.Message);
            }
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sok"></param>
        public void Receive(Socket sok)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = sok;
                SocketError err;
                sok.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out err, new AsyncCallback(ReciveCallback), state);
            }
            catch (Exception ex)
            {
                SMSLog.Error("【" + _account + "】SocketClient=>接收数据异常Receive()：" + ex.Message);
            }
        }


        //接收头数据回调
        public void ReciveCallback(IAsyncResult ir)
        {

            SocketError err;
            StateObject state = new StateObject();
            Socket sok = null;
            try
            {
                if (ir.IsCompleted)
                {

                    state = ir.AsyncState as StateObject;
                    sok = state.workSocket;

                    int bytesread = sok.EndReceive(ir, out err);
                    if (bytesread > 0 && err == SocketError.Success)
                    {

                        _isConnectionSuccessful = true;
                        byte[] b = Tools.GetSubBytes(state.buffer, 0, bytesread);
                        //处理数据
                        MemoryStream ms = new MemoryStream(b);
                        BinaryReader br = new BinaryReader(ms);
                        _isConnectionSuccessful = true;
                        Cmpp_HeaderDecode head = new Cmpp_HeaderDecode();
                        head.Decode(br);//解析包头
                        br.Close();
                        ms.Close();
                        Cmpp_Command cmd = (Cmpp_Command)head.Header.Command_Id;
                        if (!Tools.CheckCommand(head.Header.Command_Id))
                        {
                            SMSLog.Error("解析到未知Cmpp_CommandID[重新登录]：" + head.Header.Command_Id.ToString("x8"));
                            // _isLoginSuccessful = false;
                            //Login();
                            return;
                        }



                        try
                        {
                            SMSLog.Debug("接收到：" + cmd + ",cmd=0x" + head.Header.Command_Id.ToString("x8"));
                            if((head.Header.Total_Length - GlobalModel.HeaderLength)>0)
                            {
                                byte[] body = new byte[head.Header.Total_Length - GlobalModel.HeaderLength];
                                int bodyLen = sok.Receive(body);//接收包体
                                if (bodyLen != head.Header.Total_Length - GlobalModel.HeaderLength)
                                {
                                    SMSLog.Error("接收包体失败[重新登录]：" + head.Header.Command_Id);
                                    // _isLoginSuccessful = false;
                                    //Login();
                                    return;
                                }
                                ms = new MemoryStream(body);
                                br = new BinaryReader(ms);
                            }

                        }
                        catch (Exception ex)
                        {

                            SMSLog.Error("接收包体异常：" + ex.Message);
                        }

                        LastActiveTime = DateTime.Now;//最后一次交互时间
                        switch (cmd)
                        {
                            case Cmpp_Command.CMPP_CONNECT_RESP:
                                Cmpp_LoginDecode login = new Cmpp_LoginDecode();
                                login.Decode(br);
                                if (login.Status == 0)
                                {
                                    _isLoginSuccessful = true;
                                    this.HeartStart();
                                }
                                else
                                {
                                    _isLoginSuccessful = false;
                                }
                                SMSLog.Log("【CMPP_CONNECT_RESP】login=>status=" + login.Status);
                                break;
                            case Cmpp_Command.CMPP_SUBMIT_RESP:
                                try
                                {
                                    Cmpp_SubmitDecode sr = new Cmpp_SubmitDecode();
                                    sr.Decode(br);
                                    SMSLog.Log("【CMPP_SUBMIT_RESP】submit_resp=>result=" + sr.Result + ",msgid=" + sr.MsgId + ",seq=" + head.Header.Sequence_Id);
                                    if (GlobalModel.Lparams.IsResend.Equals("1"))
                                    {
                                        DelFromWaitingQueueHandler.BeginInvoke(head.Header.Sequence_Id, new AsyncCallback((IAsyncResult) => { }), null);
                                    }
                                    string[] submitArgs = { head.Header.Sequence_Id + "", sr.MsgId + "", sr.Result + "" };
                                    GlobalModel.UpdateSubmitStateHandler.BeginInvoke(submitArgs, new AsyncCallback((IAsyncResult) => { }), null);


                                }
                                catch (Exception ex)
                                {
                                    SMSLog.Error("【CMPP_SUBMIT_RESP】异常：" + ex.Message);
                                }
                                break;
                            case Cmpp_Command.CMPP_DELIVER:
                                try
                                {
                                    Cmpp_DeliverDecode cd = new Cmpp_DeliverDecode();
                                    cd.Decode(br);
                                    CMPP_DeliverResp cdresp = new CMPP_DeliverResp(cd.Msg_Id, 0, head.Header.Sequence_Id); 
                                    this.SendAsync(cdresp.Encode());
                                    if (cd.Registered_Delivery == 1)
                                    {
                                        SMSLog.Log("【CMPP_DELIVER】CMPP_DELIVER=>stat=" + cd.ReportStat + ",msgid=" + cd.ReportMsgId + ",Dest_Id=" + cd.Dest_Id + ",mobile=" + cd.ReportDestTerminalId+",seq="+head.Header.Sequence_Id);
                                        string[] repportArgs = { cd.ReportMsgId + "", cd.Dest_Id, cd.ReportStat, cd.ReportDestTerminalId };
                                        GlobalModel.UpdateReportStateHandler.BeginInvoke(repportArgs, new AsyncCallback((IAsyncResult) => { }), null);
                                    }
                                    else
                                    {
                                        SMSLog.Log("【CMPP_MO】CMPP_DELIVER=>mo=" + cd.Msg_Content + ",msgid=" + cd.ReportMsgId + ",接收Dest_Id=" + cd.Dest_Id + ",来自mobile=" + cd.Src_terminal_Id);
                                        string[] moArgs = { cd.Src_terminal_Id, cd.Dest_Id, cd.Msg_Content };
                                        GlobalModel.SaveMoHandler.BeginInvoke(moArgs, new AsyncCallback((IAsyncResult) => { }), null);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    SMSLog.Error("【CMPP_DELIVER】异常：" + ex.Message);

                                }
                                break;

                            case Cmpp_Command.CMPP_ACTIVE_TEST:
                                try
                                {
                                    CMPP_ActiveTestDecode catde = new CMPP_ActiveTestDecode();
                                    catde.Decode(br);
                                    CMPP_ActiveTestResp cat = new CMPP_ActiveTestResp();
                                    cat.SequenceId = head.Header.Sequence_Id;
                                    this.SendAsync(cat.Encode());
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            case Cmpp_Command.CMPP_ACTIVE_TEST_RESP:
                                SMSLog.Log("【CMPP_ACTIVE_TEST_RESP】不处理");
                                //CMPP_ActiveTestDecode catdecode = new CMPP_ActiveTestDecode();
                                //catdecode.Decode(br);
                                //CMPP_ActiveTestResp cata = new CMPP_ActiveTestResp();
                                //cata.SequenceId = head.Header.Sequence_Id;
                                //this.SendAsync(cata.Encode());
                                break;
                            case Cmpp_Command.CMPP_TERMINATE_RESP:
                                SMSLog.Log("【CMPP_TERMINATE_RESP】不处理");
                                //this.HeartStop();
                                //CMPP_TerminateResp tr = new CMPP_TerminateResp();
                                //tr.SequenceId = head.Header.Sequence_Id;
                                //this.SendAsync(tr.Encode());
                                // this._isStop = true;//通知其他线程可以退出了
                                break;
                            case Cmpp_Command.CMPP_TERMINATE:
                                try
                                {
                                    SMSLog.Log("【CMPP_TERMINATE】退出命令");
                                    _isLoginSuccessful = false;
                                    CMPP_TerminateDecode ctrd = new CMPP_TerminateDecode();
                                    ctrd.Decode(br);
                                    CMPP_TerminateResp ctr = new CMPP_TerminateResp();
                                    ctr.SequenceId = head.Header.Sequence_Id;
                                    this.SendAsync(ctr.Encode());
                                    //this.StopMe();

                                }
                                catch (Exception ex)
                                {
                                    SMSLog.Error("【CMPP_TERMINATE】异常：" + ex.Message);
                                }
                                break;
                            default:
                                break;
                        }
                        br.Close();
                        ms.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                SMSLog.Error("【" + _account + "】SocketClient=>ReciveCallback()异常：" + ex.ToString());
            }
            finally
            {
                try
                {
                    if (sok != null)
                    {
                        sok.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out err, new AsyncCallback(ReciveCallback), state);
                    }
                }
                catch (Exception ex)
                {
                    SMSLog.Error("SocketClient=>再次启动BeginReceive()异常：" + ex.ToString());
                }

            }
        }

        public void StopMe()
        {
            try
            {
                if (!_isStop)
                {
                    if (this.TcpIsCanUse())//发送一条对服务器的通告
                    {

                        uint seq = Tools.GetSequence_Id();
                        CMPP_Terminate t = new CMPP_Terminate();
                        this.Send(t.Encode());
                        this.HeartStop();
                        SMSLog.Log("=SocketClient==》StopMe()==HeartStop==");
                    }
                    MyTools.WaitTime(10);  //等待1000ms,告知服务器  
                    _isStop = true;
                    if (_socket != null)
                    {
                        _socket.Shutdown(SocketShutdown.Both);
                        _socket.Close();
                        _socket = null;
                    }
                    SMSLog.Log("=SocketClient==》StopMe()==_socket==");
                }

            }
            catch (Exception ex)
            {
                SMSLog.Log("=SocketClient==》StopMe()异常：" + ex.Message);
            }

        }

        /// <summary>
        /// 强制停止线程
        /// </summary>
        /// <param name="t"></param>
        private void ForcedSubThread(Thread t)
        {
            try
            {
                t.Abort();
                t.Join();
            }
            catch (Exception)
            { }
        }


        /// <summary>
        /// 启动心跳包
        /// </summary>
        private void HeartStart()
        {
            try
            {
                if (_isStartHeart)
                {
                    SMSLog.Log("==启动心跳包=" + DateTime.Now);
                    heartSend = new Timer(new TimerCallback(SendHeartCallBack), null, 3000, CMPP_ACTIVE_TEST_C_TICKs * 1000);
                    //checkState = new Timer(new TimerCallback(CheckStateCallBack), null, 30000, CMPP_ACTIVE_TCP_TICKs * 1000);
                    _threadCheckState = new Thread(new ThreadStart(CheckStateThread));
                    _threadCheckState.Start();


                }
                _isStartHeart = false;//只启动一次
            }
            catch (Exception ex)
            {

                SMSLog.Error("【" + _account + "】SocketClient=>启动心跳包=Exception:" + ex.ToString());
            }
        }

        private void HeartStop()
        {

            try
            {
                if (heartSend != null)
                {
                    heartSend.Dispose();
                }
            }
            catch (Exception)
            {


            }
            try
            {
                if (checkState != null)
                {
                    checkState.Dispose();
                }
            }
            catch (Exception)
            {

            }
           
           // MyTools.StopThread(_threadCheckState);

        }




        /// <summary>
        /// 是否到了ping一次的时间
        /// </summary>
        /// <returns></returns>
        private bool IsPingTime()
        {
            TimeSpan l = (DateTime.Now - this.LastActiveTime);

            if (l.TotalSeconds >= (CMPP_ACTIVE_TEST_C_TICKs))
            {
                lock (this)
                {
                    return (true);
                }
            }
            else
            {
                return (false);
            }
        }

        private bool TcpIsCanUse()  //测试当前tcp是否可用
        {
            bool reval = true;
            DateTime t = DateTime.Now;
            TimeSpan ts = t - this.LastActiveTime;
            if (ts.TotalSeconds > CMPP_ACTIVE_TCP_TICKs)
            {
                reval = false;  //不可用
            }

            return reval;
        }


        /// <summary>
        /// 心跳包
        /// </summary>
        /// <param name="obj"></param>
        private void SendHeartCallBack(object obj)
        {

            try
            {
                if (IsPingTime() && !_isStop)
                {
                    
                    CMPP_ActiveTest heart = new CMPP_ActiveTest();
                    this.Send(heart.Encode());
                    SMSLog.Debug("SocketClient=>发送心跳包......");
                }
            }
            catch (Exception ex)
            {

                SMSLog.Error("【" + _account + "】SocketClient=>发送心跳包异常：SendAsync():" + ex.Message);
            }

        }

        ManualResetEvent checkStateWait = new ManualResetEvent(false);

        private void CheckStateThread()
        {
            while (!_isStop)
            {
                
                try
                {
                    checkStateWait.WaitOne(CMPP_ACTIVE_TCP_TICKs * 1000);
                    //MyTools.WaitTime(CMPP_ACTIVE_TCP_TICKs*1000);
                    if (((DateTime.Now - LastActiveTime).TotalSeconds > CMPP_ACTIVE_TCP_TICKs) && !_isStop)
                    {
                        this.LastActiveTime = DateTime.Now;
                        _isLoginSuccessful = false;
                        SMSLog.Log("CheckStateThread......检查状态........重新登录....begin.........LastActiveTime=" + LastActiveTime); 
                         Login();
                        SMSLog.Log("CheckStateThread......检查状态........重新登录.....end........" + DateTime.Now);
                    } 
                }
                catch (Exception ex)
                {
                    SMSLog.Error("SocketClient=>CheckStateThread()检查状态=>Exception:" + ex.ToString());
                }

            }
        }
        //检查状态
        private void CheckStateCallBack(object obj)
        {
            try
            {
                if (((DateTime.Now - this.LastActiveTime).TotalSeconds > CMPP_ACTIVE_TCP_TICKs) && !_isStop)
                {
                    this.LastActiveTime = DateTime.Now;
                    _isLoginSuccessful = false;
                    SMSLog.Log("CheckStateCallBack......检查状态........重新登录.............=" + DateTime.Now);
                    if(!_isLogining)
                    {
                        Login();
                    }
                   
                }


            }
            catch (Exception ex)
            {
                SMSLog.Error("SocketClient=>CheckStateCallBack()检查状态=>Exception:" + ex.ToString());
            }
        }



    }//end

    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 12;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

}
