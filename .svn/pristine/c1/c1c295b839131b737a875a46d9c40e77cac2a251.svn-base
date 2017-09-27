using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using Apache.NMS;
using System.Data;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.ActiveMQ;
using System.Reflection;

namespace bll
{
    public class ActiveMQClient
    {
        private IConnectionFactory factory;
        private IConnection connection;
        private ISession session;
        ActiveMQQueue responseQueue;
        IMessageConsumer consumer;
        public string ExceptionMsg { get; set; }
        IDBExec _dbexec = null;
         
        public bool InitCustomer(string url, string mqname, IDBExec dbexec)
        {

            try
            {
                _dbexec = dbexec;
                factory = new ConnectionFactory(url);
                connection = factory.CreateConnection();
                connection.ClientId = mqname;
                connection.RequestTimeout = new TimeSpan(10 * 1000 * 10000);//10秒
                connection.ExceptionListener += new ExceptionListener(Connection_ExceptionListener);
                connection.Start();
                session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                responseQueue = new ActiveMQQueue(mqname);
                consumer = session.CreateConsumer(responseQueue);
                SMSLog.Error("ActiveMQClient::InitCustomer:成功,mqname=" + mqname + ",url=" + url + ",ReadMobileNum=" + GlobalModel.Lparams.ReadMobileNum);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionMsg = ex.Message;
                SMSLog.Error("ActiveMQClient::InitCustomer:失败", ex.Message);
            }
            return false;
        }

        private void Connection_ExceptionListener(Exception ex)
        {
            ExceptionMsg = ex.Message;
            SMSLog.Error("ActiveMQClient::Connection_ExceptionListener:", ex.Message);
        }

        public void Stop()
        {
            try
            {
                GlobalModel.Lparams.DataId = this.DataId + "";
                GlobalModel.Lparams.Save();
            }
            catch { }
            try
            {
                if (session != null)
                {
                    session.Close();
                }
            }
            catch (Exception)
            {
 
            }

            try
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            catch (Exception)
            {
 
            }
            try
            {
                if (consumer != null)
                {
                    consumer.Close();
                    consumer = null;
                }
            }
            catch (Exception)
            {
 
            }

        }
        int dataid = 1;

        public int DataId
        {
            get { return dataid; }
            set { dataid = value; }
        }
        private object getmsglock = new object();
        public DataTable GetMessageFromActiveMQ(int count)
        {
            lock (getmsglock)
            {
                if (consumer == null)
                {
                    throw new Exception("consumer==null");
                }

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ID"));
                dt.Columns.Add(new DataColumn("UCUID"));
                dt.Columns.Add(new DataColumn("MOBILE"));
                dt.Columns.Add(new DataColumn("CID"));
                dt.Columns.Add(new DataColumn("CLIENTMSGID"));
                dt.Columns.Add(new DataColumn("EXTNUM"));
                dt.Columns.Add(new DataColumn("CONTENT"));
                dt.Columns.Add(new DataColumn("SNDID"));
                dt.Columns.Add(new DataColumn("EPRID"));
                StringBuilder sbg = new StringBuilder();
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        ITextMessage msg = consumer.ReceiveNoWait() as ITextMessage;
                         
                        if (msg == null)
                        {
                            break;
                        }
                        else
                        {

                            string txt = dataid + "," + msg.Text;
                            dataid++;
                            //SMSLog.Debug("【从ActiveMQ取到数据】" + txt);
                            sbg.AppendLine(txt);
                            string[] args = txt.Split(new char[] { ',' });
                            if(args.Length!=8)
                            {
                                SMSLog.Error("ActiveMQClient::GetMessageFromActive==>【读取内容 Exception】", "args.len!=8");
                                continue;
                            }
                            string cid = args[3];

                            DateTime dt1 = DateTime.Now;
                            
                            string content = RedisManager.GetVal<string>("sms" + cid);

                            if (string.IsNullOrEmpty(content))
                            {
                                try
                                {
                                    
                                    string sql = string.Format("select content from TBL_SMS_CONTENT where ID={0}", cid);
                                    SMSLog.Error("ActiveMQClient=====redis===== 未取到内容==" + sql);
                                    object obj = _dbexec.ExecuteScalar(sql);
                                    content = obj == null ? "" : obj.ToString();

                                }
                                catch (Exception ex)
                                {

                                    SMSLog.Error("ActiveMQClient::GetMessageFromActive==>读取内容 Exception:", ex.ToString());
                                }
                            }

                           // SMSLog.Debug("ActiveMQClient", "读取内容用时[" + (DateTime.Now - dt1).TotalMilliseconds + "]毫秒");

                            if (!string.IsNullOrEmpty(content))
                            {
                                content=MyTools.NoHTML(content);
                                DataRow dr = dt.NewRow();
                                dr["ID"] = args[0];
                                dr["UCUID"] = args[1];
                                dr["MOBILE"] = args[2];
                                dr["CID"] = cid;
                                dr["CLIENTMSGID"] = args[4];
                                dr["EXTNUM"] = args[5];
                                dr["SNDID"] = args[6];
                                dr["EPRID"] = args[7];
                                dr["CONTENT"] = content;

                                dt.Rows.Add(dr);
                                //msg.Acknowledge();
                            }
                            else
                            {
                                SMSLog.Error("ActiveMQClient::GetMessageFromActive", "没取到内容[" + cid + "]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        SMSLog.Error("ActiveMQClient::GetMessageFromActive==>for Exception:", ex.ToString());
                    }

                 
                }//for

                try
                {
                    if (!string.IsNullOrEmpty(sbg.ToString()) && sbg.Length > 10)
                    {

                        SMSLog.Debug(sbg.ToString());
                    }
                }
                catch { }
                return dt;
            }

        }
    }//end
}
