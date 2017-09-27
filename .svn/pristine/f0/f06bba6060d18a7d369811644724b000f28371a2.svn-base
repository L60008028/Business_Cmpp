using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dal;
using System.Data.SqlClient;
using System.Data;
using model;

namespace bll
{
    public class MsSqlDBExec:IDBExec
    {
 


        /// <summary>
        /// 检测数据库连接串是否有效
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public override bool IsConn(string constr)
        {
            try
            {
                string sql = "select getdate()";
              
                object obj = MsSqlHelper.ExecuteScalar(constr,CommandType.Text, sql, null);
                if (obj != null)
                {
                    return true;
                }
            }
            catch (SqlException ex)
            {

            }
            return false;
        }


        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            int re = 0;
            DateTime dt1 = DateTime.Now;
            try
            {
                //SMSGLog.Error(cmdText);
                SqlErrMsg = "";
                re = MsSqlHelper.ExecuteNonQuery(ConnectionstringLocalTransaction,cmdType, cmdText, commandParameters);
                if ((DateTime.Now - dt1).TotalMilliseconds > 3000)
                {
                    SMSLog.Debug("SqlDBExec =>ExecuteNonQuery 耗时：" + (DateTime.Now - dt1).TotalMilliseconds + "毫秒,result=" + re + ",sql=" + cmdText);
                }
                return re;
            }
            catch (SqlException ex)
            {
                SqlErrMsg = ex.Message;
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception:" + cmdText);
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception: ", ex.Message);

            }
            return 0;
        }



        public static int ExecuteNonQuery( string cmdText)
        {
            int re = 0;
            DateTime dt1 = DateTime.Now;
            try
            {
                //SMSGLog.Error(cmdText);
                SqlErrMsg = "";
                re = MsSqlHelper.ExecuteNonQuery(ConnectionstringLocalTransaction, CommandType.Text, cmdText, null);
                if ((DateTime.Now - dt1).TotalMilliseconds > 3000)
                {
                    SMSLog.Debug("SqlDBExec =>ExecuteNonQuery 耗时：" + (DateTime.Now - dt1).TotalMilliseconds + "毫秒,result=" + re + ",sql=" + cmdText);
                }
                return re;
            }
            catch (SqlException ex)
            {
                SqlErrMsg = ex.Message;
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception:" + cmdText);
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception: ", ex.Message);

            }
            return 0;
        }

        public static DataTable GetDateTable( CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                DateTime dt1 = DateTime.Now;
                DataTable ds = MsSqlHelper.GetDateTable(ConnectionstringLocalTransaction,cmdType, cmdText, commandParameters);
                if ((DateTime.Now - dt1).TotalMilliseconds > 3000)
                {
                    SMSLog.Debug("SqlDBExec =>GetDateTable 耗时：" + (DateTime.Now - dt1).TotalMilliseconds + "毫秒");
                }
                return ds ;
            }catch(SqlException ex)
            {
            }
            return null;
        }


        public override int BatchExec(List<string> list)
        {
            List<string> relist = dal.MySqlHelper.BatchExec(ConnectionstringLocalTransaction, list);
            return relist.Count;
        }


        public override List<SqlStatementModel> BatchExec(List<SqlStatementModel> list)
        {
            try
            {
                SqlErrMsg = "";
                return dal.MsSqlHelper.BatchExec(ConnectionstringLocalTransaction, list);
            }
            catch (SqlException ex)
            {
                SqlErrMsg = ex.Message;
                SMSLog.Error("SqlDBExec =>BatchExec Exception: ", ex.Message);
            }
            return null;
        }



        public override int ExecuteNonQuery(int cmdType, string cmdText, object commandParameters)
        {
            int re = 0;
            DateTime dt1 = DateTime.Now;
            try
            {
                SqlParameter[] cmdParameters = null;
                if (commandParameters!=null)
                {
                    cmdParameters = commandParameters as SqlParameter[];
                }
                CommandType cmdtype;
                if (cmdType == 1)
                {
                    cmdtype = CommandType.StoredProcedure;
                }
                else
                {
                    cmdtype = CommandType.Text;
                }
 
                SqlErrMsg = "";
                re = MsSqlHelper.ExecuteNonQuery(ConnectionstringLocalTransaction, cmdtype, cmdText, cmdParameters);
                if ((DateTime.Now - dt1).TotalMilliseconds > 3000)
                {
                    SMSLog.Debug("SqlDBExec =>ExecuteNonQuery 耗时：" + (DateTime.Now - dt1).TotalMilliseconds + "毫秒,result=" + re + ",sql=" + cmdText);
                }
                return re;
            }
            catch (SqlException ex)
            {
                SqlErrMsg = ex.Message;
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception:" + cmdText);
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception: ", ex.Message);

            }
            return 0;
        }

        public override DataTable GetDateTable(string cmdText)
        {
            try
            {
                DataTable ds = MsSqlHelper.GetDateTable(ConnectionstringLocalTransaction, CommandType.Text, cmdText, null);
                return ds;
            }
            catch (SqlException ex)
            {
            }
            return null;
        }

        public override DataTable GetDateTable(int cmdtype, string cmdText, object commandParameters)
        {
            try
            {
                CommandType cmdType;
                if (cmdtype == 1)
                {
                    cmdType = CommandType.StoredProcedure;
                }
                else
                {
                    cmdType = CommandType.Text;
                }
                SqlParameter[] cmdParameters=null;
                if (commandParameters!=null)
                {
                    cmdParameters = commandParameters as SqlParameter[];
                }
                DataTable ds = MsSqlHelper.GetDateTable(ConnectionstringLocalTransaction, cmdType, cmdText, cmdParameters);
                return ds;
            }
            catch (SqlException ex)
            {
            }
            return null;
        }

        public override object ExecuteScalar(string sql)
        {
            return MsSqlHelper.ExecuteScalar(ConnectionstringLocalTransaction, CommandType.Text, sql, null);
        }
    }
}
