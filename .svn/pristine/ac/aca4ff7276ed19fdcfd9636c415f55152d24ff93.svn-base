using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using System.Threading;
using System.Collections;
using System.Reflection;

namespace bll
{
    public class SMDQueue
    {
        private Queue<SqlStatementModel> sqlueue = new Queue<SqlStatementModel>();
        public static AutoResetEvent queueStatus = new AutoResetEvent(false);//队列信号
        private System.Threading.Timer queueTimer;
        private string _title = "";
        IDBExec dbexec = null;
        private int _maxexectimer = 20;
        private int _maxexecnum = 1;

        public SMDQueue(string title, int maxexectimer, int maxexecnum)
        {
            //LocalParams lp = new LocalParams();
            dbexec = (IDBExec)Assembly.Load("bll").CreateInstance(GlobalModel.Lparams.SqlExecClassName);//获取实例
            _title = title;
            _maxexectimer = maxexectimer;
            _maxexecnum = maxexecnum;
        }


        public SMDQueue(string title)
        {
            //LocalParams lp = new LocalParams();
            dbexec = (IDBExec)Assembly.Load("bll").CreateInstance(GlobalModel.Lparams.SqlExecClassName);//获取实例
            _title = title;
 
        }

        public bool AddSql(SqlStatementModel sm)
        {
            bool ret = false;
            try
            {
                lock (((ICollection)sqlueue).SyncRoot)
                {
                    sqlueue.Enqueue(sm);
                }
                ret = true;
            }
            catch (Exception ex)
            {
                MySqlDBExec.ExecuteNonQuery(sm.CmdTxt, sm.MySqlCmdParms);
                SMSLog.Error("SMDQueue::addSql", "=================>Exception:" + ex.Message);
                ret = false;
            }
            return ret;
        }


        /// <summary>
        /// 批量保存
        /// </summary>
        /// <returns></returns>
        private void BatchSaveSql(object obj)
        {

            try
            {
                List<SqlStatementModel> sqlList = new List<SqlStatementModel>();
                try
                {
                    if (sqlueue != null && sqlueue.Count > 0)
                    {
                        lock (((ICollection)sqlueue).SyncRoot)
                        {
                            for (int i = 0; i < _maxexecnum; i++)
                            {
                                if (i > sqlueue.Count)
                                {
                                    break;
                                }
                                SqlStatementModel sm = sqlueue.Dequeue();
                                if (sm != null)
                                {
 
                                    sqlList.Add(sm);
                                    
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                    }
                    else
                    {
                        queueStatus.Set();
                    }
                }
                catch (Exception ex)
                {
                    SMSLog.Error("SMDQueue::BatchSaveSql(Dequeue):Exception:", ex.Message);
                }

                if (sqlList != null && sqlList.Count > 0)
                {
                    DateTime tmStart = DateTime.Now;
                    List<SqlStatementModel> reList = dbexec.BatchExec(sqlList);
                    int re = 0;
                    if (reList != null)
                    {
                        re = reList.Count;
                        foreach (SqlStatementModel resm in reList)
                        {
                            resm.ExecTimer++;
                            if (resm.ExecTimer < _maxexectimer)
                            {
                                SMSLog.Debug("SMDQueue==>BatchSaveSql=>Result Fail[" + resm.ExecTimer+ "]" + resm.ToString());
                                this.AddSql(resm);
                            }
                            else
                            {
                                if (resm.CmdType == System.Data.CommandType.Text)
                                {
                                    SMSLog.Debug("SMDQueue==>BatchSaveSql[FailSql]：" + resm.ToString());
                                }
                                else
                                {
                                    SMSLog.Debug("SMDQueue==>BatchSaveSql[FailProc]：" + resm.ToString());
                                }
                            }
                        }
                    }
                    SMSLog.Debug("SMDQueue::BatchSaveSql", " BatchExcSql (" + sqlList.Count + "),Result Fail:" + re + "]时间: " + (DateTime.Now - tmStart).TotalMilliseconds + "毫秒");
                }
            }
            catch (Exception EX)
            {
                SMSLog.Error("SMDQueue::BatchSaveSql:Exception:", EX.Message);
            }
        }

        /// <summary>
        /// 启动Timer对象，定时保存数据
        /// </summary>
        /// <param name="duttime"></param>
        public void Start(int duttime)
        {
            System.Threading.TimerCallback saveQueue = new System.Threading.TimerCallback(BatchSaveSql);
            queueTimer = new System.Threading.Timer(saveQueue, null, duttime, duttime);
            SMSLog.Debug("SMDQueue::Start", "启动["+_title+"]对象，定时["+duttime+"]保存数据");
        }


        public void Stop()
        {
            try
            {
                queueStatus.WaitOne();
                SMSLog.Debug("SMDQueue::Stop", "[" + _title + "]Timer对象停止...");
                queueTimer.Change(0, System.Threading.Timeout.Infinite);
                queueTimer.Dispose();//销毁定时器，即停止定时器 
                queueTimer = null;
            }
            catch //(Exception ex)
            {
            }

        }


    }//end
}//end
