using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cmppV2;
using model;
using System.IO;
using System.Data;
using System.Threading;

namespace bll
{
    public class SmsService_business
    {

        private Dictionary<int, SendThread> _cstDic = null;//cmpp企业用户
        private Dictionary<int, AccountInfoModel> _cmppAccountDic = null;//cmpp账号
        private TimeoutCache cache = null;//缓存
        private Timer _collDataTimer;//取数据的线程
        SMDQueue[] ReportQueue;//report状态队列
        SMDQueue[] ReportSeqQueue;//状态流水队列
        private int _reportQueueNum = 10;
        private  LocalParams lp;
        private string[] Keyword = { "注册", "密码", "验证" };

        public bool Stop()
        {
            SMSLog.Debug("SmsService==>Stop=>开始停止");
            GlobalModel.IsStopCollect = true;
            if (_cstDic != null)
            {
                foreach (int key in _cstDic.Keys)
                {
                    _cstDic[key].Exit();
                }
            }
            if (ReportQueue != null && ReportQueue.Length > 0)
            {
                foreach (SMDQueue smsq in ReportQueue)
                {
                    smsq.Stop();
                }
            }
            if (ReportSeqQueue != null && ReportSeqQueue.Length > 0)
            {
                foreach (SMDQueue smsq in ReportSeqQueue)
                {
                    smsq.Stop();
                }
            }
            SMSLog.Debug("SmsService==>Stop=>已结束...");
            return false;
        }

        IDBExec dbexec = null;
        public bool Start()
        {
            lp = new LocalParams();
            if (!dbexec.IsConn(lp.SqlConnStr))
            {
                SMSLog.Debug("SmsService==>Init=>sql登录失败[" + lp.SqlConnStr + "]");
                return false;
            }
            MySqlDBExec.ConnectionstringLocalTransaction = lp.SqlConnStr;
            _cstDic = new Dictionary<int, SendThread>();
            _cmppAccountDic = new Dictionary<int, AccountInfoModel>();
            //初始数据标识
            string sql1 = "update t_sendsms set CTSubmitStatus=0 Where  CTGatewayNum='" + lp.GateWayNum + "'";
            string sql2 = "update t_smsmobile_t set  smsFlag='0' where SubmitStatus='0' and GatewayNum='" + lp.GateWayNum + "'";
            int re1 = MySqlDBExec.ExecuteNonQuery(sql1, null);
            SMSLog.Debug("SmsService==>Init初始内容表：" + re1);
            int re2 = MySqlDBExec.ExecuteNonQuery(sql2, null);
            SMSLog.Debug("SmsService==>Init初始手机号码表：" + re2);


            cache = new TimeoutCache(60 * 30);//缓存
            ReportQueue = new SMDQueue[_reportQueueNum];
            ReportSeqQueue = new SMDQueue[_reportQueueNum];
            for (int i = 0; i < _reportQueueNum; i++)
            {
                string title = "Report_" + i;
                ReportQueue[i] = new SMDQueue(title);
                ReportQueue[i].Start(3000);

                string seqt = "ReportSeq_" + i;
                ReportSeqQueue[i] = new SMDQueue(seqt);
                ReportSeqQueue[i].Start(4000);
            }
            /*
            GlobalModel.UpdateMobileSubmitStateHandler = this.UpdateMobileSubmitState;
            GlobalModel.UpdateReportStateHandler = this.UpdateReportState;
            GlobalModel.UpdateMobileByBatchNumHandler = this.UpdateMobileByBatchNum;
            GlobalModel.UpdateSMSContentSubmitStatuHandler = this.UpdateSMSContentSubmitStatu;
            GlobalModel.DeleteContentHandler = this.DeleteContent;
            GlobalModel.SaveMoHandler = this.SaveMo;
             * */

            //读取企业CMPP账号信息
            List<AccountInfoModel> accountLst = LoadCmppAccount();
            if (accountLst != null && accountLst.Count > 0)
            {
                foreach (AccountInfoModel m in accountLst)
                {
                    SendThread cmpp = new SendThread(m);

                    if (cmpp.Login())
                    {
                        _cstDic.Add(m.eprId, cmpp);
                        _cmppAccountDic.Add(m.eprId, m);
                        cmpp.IsSend = true;
                        cmpp.Start();

                    }
                    else
                    {
                        SMSLog.Debug("SmsService==>Init[" + m.eprId + "]登录失败");
                    }
                    Thread.Sleep(10);
                }
            }
            else
            {
                SMSLog.Debug("SmsService==>[Init]读取cmpp账号失败");
                return false;
            }

            GlobalModel.IsStopCollect = false;
            CollectObject_Business co = new CollectObject_Business();
            _collDataTimer = new Timer(new TimerCallback(CollectThread), co, lp.ReadContentDealy, lp.ReadContentDealy);
            SMSLog.Debug("SmsService==>[Init]启动成功");
            return true;
        }

        private List<AccountInfoModel> LoadCmppAccount()
        {
            List<AccountInfoModel> lst = null;
            string sql = "select * from t_spaccountinfo";
            DataTable dt = MySqlDBExec.GetDateTable(sql, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                lst = new List<AccountInfoModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AccountInfoModel aim = new AccountInfoModel();
                    aim.eprId = int.Parse(dt.Rows[i]["EprId"].ToString());
                    aim.loginname = dt.Rows[i]["LoginName"].ToString();
                    aim.password = dt.Rows[i]["Password"].ToString();
                    aim.senddelay = int.Parse(dt.Rows[i]["SendDelay"].ToString());
                    aim.serviceid = dt.Rows[i]["ServiceId"].ToString();
                    aim.spid = dt.Rows[i]["Pid"].ToString();
                    aim.protocolType = dt.Rows[i]["ProtocolType"].ToString();
                    aim.serviceIp = dt.Rows[i]["ServiceIp"].ToString();
                    aim.servicePort = int.Parse(dt.Rows[i]["ServicePort"].ToString());
                    aim.spnumber = dt.Rows[i]["Spnumber"].ToString();
                    lst.Add(aim);
                }
            }
            return lst;
        }


        public void AddQueue(int eprId, SmsModel sms)
        {
            bool ispriority = false;
            foreach (string str in Keyword)
            {
                if (sms.content.Contains(str))
                {
                    ispriority = true;
                    break;
                }
            }
            QueueItem qi = new QueueItem();
            qi.InQueueTime = DateTime.Now;
            qi.MsgObj = sms;
            qi.MsgState = 0;
            qi.MsgType = (uint)Cmpp_Command.CMPP_SUBMIT;
            qi.Sequence = sms.id;

            if (_cstDic.ContainsKey(eprId))
            {

                if (ispriority)
                {
                    _cstDic[eprId].AddPriorityQueue(qi);
                }
                else
                {
                    _cstDic[eprId].AddNormalQueue(qi);
                }
            }
            else
            {

                AccountInfoModel ai = this.GetAccountInfo(sms.eprId);
                if (ai != null)
                {
                    SendThread cmpp = new SendThread(ai);

                    if (cmpp.Login())
                    {
                        if (!_cstDic.ContainsKey(ai.eprId))
                        {
                            _cstDic.Add(ai.eprId, cmpp);
                        }
                        if (!_cmppAccountDic.ContainsKey(ai.eprId))
                        {
                            _cmppAccountDic.Add(ai.eprId, ai);
                        }
                        cmpp.AddPriorityQueue(qi);
                        cmpp.IsSend = true;
                        cmpp.Start();

                    }
                    else
                    {
                        this.DeleteContent(sms.contentId);
                        UpdateMobileSubmitState(sms.id + "", "-1", "", "CMPP账号登录失败", "");
                        SMSLog.Debug("SmsService==>AddQueue[" + ai.eprId + "]登录失败");
                    }


                }
                else
                {

                    this.DeleteContent(sms.contentId);
                    UpdateMobileSubmitState(sms.id + "", "-1", "", "无CMPP账号", "");
                }
            }
        }


        private void CollectThread(object obj)
        {
            if (GlobalModel.IsStopCollect)
            {
                return;
            }
            string sql = GetLoadMessageSql();
            if (!string.IsNullOrEmpty(sql))
            {
                DataTable dt = MySqlDBExec.GetDateTable(sql, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string CTGatewayNum = dt.Rows[i]["CTGatewayNum"].ToString();
                        string id = dt.Rows[i]["Id"].ToString();
                        string batchnum = dt.Rows[i]["BatchNum"].ToString();
                        string eprId = dt.Rows[i]["EprId"].ToString();
                        string con = dt.Rows[i]["Content"].ToString();//短信内容
                        int mobileCount = 0;
                        int.TryParse(dt.Rows[i]["MobileCount"].ToString(), out mobileCount);
                        if (CTGatewayNum.Equals("1001"))
                        {
                            SMSLog.Debug("SMDDatabase==>该批次号[" + batchnum + "]1001不发送，修改tcsubmitstate状态为1 并试图删除");
                            UpdateSMSContentSubmitStatu(id, 1);
                            this.DeleteContent(id);
                            UpdateMobileByBatchNum(-1, batchnum, "1001", "限制网关");
                            continue;
                        }

                        DataTable dt_mobile = this.LoadMobile(batchnum);//根据批次号取发送号码
                        if (dt_mobile != null && dt_mobile.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt_mobile.Rows.Count; j++)
                            {
                                SmsModel msg = new SmsModel();
                                msg.content = con;//短信内容
                                msg.contentId = id;//内容表ID
                                if (FillMessage(dt_mobile.Rows[j], msg))
                                {
                                    AddQueue(msg.eprId, msg);//加入待发队列
                                    AddCache(msg.id + "", msg);//加入缓存
                                }
                            }
                            if (dt_mobile.Rows.Count <= mobileCount)
                            {
                                SMSLog.Debug("SMDDatabase==>该批次号[" + batchnum + "]已读取完，修改tcsubmitstate状态为1 并试图删除");
                                UpdateSMSContentSubmitStatu(id, 1);
                                this.DeleteContent(id);
                            }
                            else
                            {
                                UpdateSMSContentSubmitStatu(id, 0);//改变状态，待下次读取
                            }

                        }
                        else
                        {
                            try
                            {
                                SMSLog.Debug("SMDDatabase==>无该批次号[" + batchnum + "]手机号码,修改tcsubmitstate状态为1 并试图删除");
                                UpdateSMSContentSubmitStatu(id, 1);
                                this.DeleteContent(id);
                            }
                            catch (Exception ex)
                            {
                                SMSLog.Error("SMDDatabase::CollectThread", "删除异常: " + ex.Message);
                            }
                        }

                    }//for
                }
            }
        }



        private void AddCache(string msgid, SmsModel msg)
        {
            try
            {
                cache.Add(msgid, msg);
            }
            catch (Exception ex)
            {
                SMSLog.Error("SMDDatabase::[" + msgid + "],Id=" + msg.id + " AddCache:Exception:", ex.Message);
            }
        }
        private SmsModel GetFormCache(string msgid)
        {
            try
            {
                SmsModel obj = cache.Get(msgid) as SmsModel;
                return obj;
            }
            catch (Exception ex)
            {
                SMSLog.Error("SMDDatabase:: GetFormCache:Exception:", ex.Message);
            }
            return null;
        }


        private bool FillMessage(DataRow row, SmsModel msg)
        {
            try
            {
                msg.id = UInt32.Parse(row["Id"].ToString());//消息ID
                msg.batchNum = row["BatchNum"].ToString();
                msg.eprId = int.Parse(row["EprId"].ToString());
                msg.mobile = row["Mobile"].ToString();	//手机号码
                return true;
            }
            catch (Exception ex)
            {
                SMSLog.Debug("SmsService==>FillMessage[Exception]：" + ex.Message);
            }
            return false;
        }

        private AccountInfoModel GetAccountInfo(int eprId)
        {
            string sql = "select * from t_accountInfo where EprId=" + eprId;
            DataTable dt = MySqlDBExec.GetDateTable(sql, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                AccountInfoModel aim = new AccountInfoModel();
                aim.eprId = int.Parse(dt.Rows[0]["EprId"].ToString());
                aim.loginname = dt.Rows[0]["LoginName"].ToString();
                aim.password = dt.Rows[0]["Password"].ToString();
                aim.senddelay = int.Parse(dt.Rows[0]["SendDelay"].ToString());
                aim.serviceid = dt.Rows[0]["ServiceId"].ToString();
                aim.spid = dt.Rows[0]["Pid"].ToString();
                aim.protocolType = dt.Rows[0]["ProtocolType"].ToString();
                aim.serviceIp = dt.Rows[0]["ServiceIp"].ToString();
                aim.servicePort = int.Parse(dt.Rows[0]["ServicePort"].ToString());
                return aim;
            }
            return null;
        }
        /// <summary>
        /// 加载待发短信内容sql
        /// </summary>
        /// <returns></returns>
        protected string GetLoadMessageSql()
        {

            string rand = GetTimesTamp(DateTime.Now).ToString();
            string updatesql = "update t_sendsms set CTSubmitStatus='" + rand + "'  where SendTime < NOW() and CheckStatus='1' and CTSubmitStatus='0' and (CTGatewayNum ='" + lp.GateWayNum + "' or CTGatewayNum='1001') order by Priority,SendTime limit " + lp.ReadContentNum + " ";
            int result = MySqlDBExec.ExecuteNonQuery(updatesql, null);
            if (result > 0)
            {
                string selecesql = "select * from t_sendsms where CTSubmitStatus='" + rand + "' and (CTGatewayNum ='" + lp.GateWayNum + "' or CTGatewayNum='1001') order by Priority,SendTime ";
                return selecesql;
            }

            return "";
        }

        /// <summary>
        /// 读取手机号码
        /// </summary>
        /// <param name="batchnum">批次号</param>
        /// <returns></returns>
        public DataTable LoadMobile(string batchnum)
        {
            string gwno = lp.GateWayNum + "";
            string rand = GetTimesTamp(DateTime.Now).ToString();
            //string sendtime = DateTime.Now.AddHours(-1).ToString();
            string updatecondition = " where GatewayNum ='" + gwno + "' and smsFlag='0' and SubmitStatus='0' and SendTime>date_add(now(),interval -1 hour) and BatchNum='" + batchnum + "'";
            string updatesql = "update t_smsmobile_t set smsFlag=" + rand + " where GatewayNum ='" + gwno + "' and smsFlag='0' and SubmitStatus='0'  and BatchNum='" + batchnum + "' order by SendTime  limit " + lp.ReadMobileNum;
            if (MySqlDBExec.ExecuteNonQuery(updatesql, null) > 0)
            {
                string selectsql = "select * from t_smsmobile_t where SendTime>date_add(now(),interval -1 hour)  and smsFlag='" + rand + "' and GatewayNum ='" + gwno + "'  and SubmitStatus='0' and BatchNum='" + batchnum + "'";
                DataTable dt = MySqlDBExec.GetDateTable(selectsql, null);
                return dt;
            }
            return null;
        }




        /// <summary>
        /// 根据批次号修改手机号码表状态
        /// </summary>
        /// <param name="state">状态:0未提交，-1失败,1成功</param>
        /// <param name="batchnum">批次号</param>
        /// <param name="errorCode">错误码</param>
        /// <param name="errMsg">错误说明</param>
        /// <returns></returns>
        private void UpdateMobileByBatchNum(int state, string batchnum, string errorCode, string errMsg)
        {
            string sql = string.Format("update t_smsmobile_t set SubmitStatus={0},ErrorCode='{1}',Remark='{2}' where BatchNum='{3}'", state, errorCode, errMsg, batchnum);
            int result = MySqlDBExec.ExecuteNonQuery(sql, null);
            if (result == 0)
            {
                SMSLog.Debug("SmsService==>UpdateSMSContentSubmitStatu[FailSql]：" + sql);
            }

        }

        /// <summary>
        /// 修改ct提交状态,待下次读取
        /// </summary>
        /// <param name="id">内容表id</param>
        /// <param name="cTSubmitStatus">状态0 可读取,1已发完</param>
        /// <returns></returns>
        private void UpdateSMSContentSubmitStatu(string id, int cTSubmitStatus)
        {
            string sql = string.Format("update  t_sendsms set CTSubmitStatus={0} where Id={1} ", cTSubmitStatus, id);
            int result = MySqlDBExec.ExecuteNonQuery(sql, null);
            if (result == 0)
            {
                SMSLog.Debug("SmsService==>UpdateSMSContentSubmitStatu[FailSql]：" + sql);
            }

        }

        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="id">内容表id</param>
        /// <returns></returns>
        private void DeleteContent(string id)
        {
            string delsql = string.Format("delete from t_sendsms where id={0} and CMSubmitStatus='1' and CUSubmitStatus='1' and CTSubmitStatus='1' ", id);
            int result = MySqlDBExec.ExecuteNonQuery(delsql, null);
            if (result == 0)
            {
                SMSLog.Debug("SmsService==>DeleteContent[FailSql]：" + delsql);
            }

        }



        /// <summary>
        /// 更新手机号码提交状态、MSGID
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="state">提交结果</param>
        /// <param name="msgid">msgid</param>
        /// <param name="srcno">发送号码</param>
        /// <returns></returns>
        private void UpdateMobileSubmitState(string id, string state, string msgid, string remark, string srcno)
        {
            int submitstatu = 0;

            if (state.Equals("0"))
            {
                submitstatu = 1;
                remark = "成功";
            }
            string sql = string.Format("update t_smsmobile_t set MsgId='{0}',SubmitStatus='{1}',ErrorCode='{2}',Remark='{3}',SrcNo='{4}' where  id='{5}' ", msgid, submitstatu, state, remark, srcno, id);
            int re = 0;
            int i = 0;
            do
            {
                re = MySqlDBExec.ExecuteNonQuery(sql, null);
                i++;
            } while (re <= 0 && i < 3);

            if (re < 1)
            {
                SMSLog.Debug("SmsService==>UpdateMobileSubmitState[FailSql]：" + sql);
            }

            //重新加入缓存
            SmsModel sm = this.GetFormCache(id);
            if (sm != null)
            {
                this.AddCache(msgid, sm);
            }
            //失败状态，写入状态流水表
            if (submitstatu == 0)
            {

                if (sm != null)
                {
                    object[] obj = { sm.eprId, sm.userId, sm.mobile, sm.clientMsgId, submitstatu };
                    string sqlstate = string.Format("insert into t_pushsmsmsg(EprId,UserId,Mobile,ClientMsgId,Status,AddTime) values('{0}','{1}','{2}','{3}','{4}',now())", obj);
                    Random rand = new Random();
                    int ii = rand.Next(0, _reportQueueNum + 1);
                    SqlStatementModel seq = new SqlStatementModel();
                    seq.MySqlCmdParms = null;
                    seq.CmdTxt = sqlstate;
                    seq.CmdType = CommandType.Text;
                    seq.ExecTimer = 1;
                    ReportSeqQueue[ii].AddSql(seq);
                }
            }


        }

        /// <summary>
        /// 更新状态回执
        /// </summary>
        /// <param name="msgid">msgid</param>
        /// <param name="reportstatus">状态</param>
        /// <returns></returns>
        private void UpdateReportState(string msgid, string srcnumber, string reportstatus)
        {
            string status = reportstatus;
            if (reportstatus.Equals("DELIVRD"))
            {
                status = "1";
            }
            else
            {
                status = "-1";
            }
            SmsModel sm = this.GetFormCache(msgid);

            string sql = string.Format("update t_smsmobile_t set ReportStatus='{0}' ,ErrorCode='{1}',SrcNo='{2}' where  MsgId='{3}' ", status, reportstatus, srcnumber, msgid);
            if (sm != null)
            {
                sql = string.Format("update t_smsmobile_t set ReportStatus='{0}' ,ErrorCode='{1}',SrcNo='{2}' where  Id='{3}' ", status, reportstatus, srcnumber, sm.id);
            }
            Random rand = new Random();
            int i = rand.Next(0, _reportQueueNum + 1);
            SqlStatementModel sqlm = new SqlStatementModel();
            sqlm.MySqlCmdParms = null;
            sqlm.CmdTxt = sql;
            sqlm.CmdType = CommandType.Text;
            sqlm.ExecTimer = 1;
            ReportQueue[i].AddSql(sqlm);

            string sqlstate = "";
            if (sm != null)
            {
                object[] obj = { sm.eprId, sm.userId, sm.mobile, sm.clientMsgId, status };
                sqlstate = string.Format("insert into t_pushsmsmsg(EprId,UserId,Mobile,ClientMsgId,Status,AddTime) values('{0}','{1}','{2}','{3}','{4}',now())", obj);

            }
            else
            {
                string sel = string.Format("select * from t_smsmobile_t where MsgId='{0}'", msgid);
                DataTable dt = MySqlDBExec.GetDateTable(sel, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    object[] obj = { dt.Rows[0]["EprId"], dt.Rows[0]["UserId"], dt.Rows[0]["Mobile"], dt.Rows[0]["ClientMsgId"], status };
                    sqlstate = string.Format("insert into t_pushsmsmsg(EprId,UserId,Mobile,ClientMsgId,Status,AddTime) values('{0}','{1}','{2}','{3}','{4}',now())", obj);
                }
            }
            if (!string.IsNullOrEmpty(sqlstate))
            {
                SqlStatementModel seq = new SqlStatementModel();
                seq.MySqlCmdParms = null;
                seq.CmdTxt = sqlstate;
                seq.CmdType = CommandType.Text;
                seq.ExecTimer = 1;
                ReportSeqQueue[i].AddSql(seq);
            }


        }


        private void SaveMo(int eprId, string mobile, string recvnumber, string content)
        {
            string userId = "";
            string client_msgid = "";
            int IsPush = 0;
            string sel = string.Format("select * from t_smsmobile_t where EprId='{0}' and Mobile='{1}' order by SendTime desc limit 1", eprId, mobile);
            DataTable dt = MySqlDBExec.GetDateTable(sel, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                userId = dt.Rows[0]["UserID"].ToString();
                client_msgid = dt.Rows[0]["ClientMsgId"].ToString();
            }
            object[] obj = {						
                        eprId,
						userId,
						"",
						mobile,
						recvnumber,
						DateTime.Now.ToString(),
						content,
						1,
						IsPush,
						DateTime.Now.ToString(),
						0,
						client_msgid
                           };
            string insql = string.Format("insert into t_morecord(EprId,UserId,Name,SendNum,RecvNum,RecvTime,Content,IsNew,IsPush,AddTime,IsDel,ClientMsgId) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", obj);
            int re = MySqlDBExec.ExecuteNonQuery(insql, null);
            if (re < 1)
            {
                SMSLog.Debug("SmsService==>SaveMo[FailSql]：" + insql);
            }
        }


        private long GetTimesTamp(DateTime dateTime)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long timestamp = Convert.ToInt64(ts.TotalMilliseconds);
            return timestamp;
        }


    }//end

    class CollectObject_Business
    {
        public int EprId { get; set; }
        public int LoadContentNum { get; set; }
        public int LoadMobileNum { get; set; }
    }
}//end
