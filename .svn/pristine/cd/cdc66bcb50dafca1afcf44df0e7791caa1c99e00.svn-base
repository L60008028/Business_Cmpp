using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using model;

namespace bll
{
    public class MySqlDBExec:IDBExec
    {
        public static string ConnectionstringLocalTransaction = "";
        public static string SqlErrMsg = "";


        /// <summary>
        /// 检测数据库连接串是否有效
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public override bool IsConn(string constr)
        {
            try
            {
                string sql = "select now()";
              
                object obj = MySqlHelper.ExecuteScalar(constr, sql, null);
                if (obj != null)
                {
                    return true;
                }
            }
            catch (MySqlException ex)
            {

            }
            return false;
        }


        public static int ExecuteNonQuery( string cmdText, params MySqlParameter[] commandParameters)
        {
            int re = 0;
            DateTime dt1 = DateTime.Now;
            try
            {
                //SMSGLog.Error(cmdText);
                SqlErrMsg = "";
                re = MySqlHelper.ExecuteNonQuery(ConnectionstringLocalTransaction, cmdText, commandParameters);
                if ((DateTime.Now - dt1).TotalMilliseconds > 3000)
                {
                    SMSLog.Debug("SqlDBExec =>ExecuteNonQuery 耗时：" + (DateTime.Now - dt1).TotalMilliseconds + "毫秒,result=" + re + ",sql=" + cmdText);
                }
                return re;
            }
            catch (MySqlException ex)
            {
                SqlErrMsg = ex.Message;
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception:" + cmdText);
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception: ", ex.Message);

            }
            return 0;
        }
        public static DataTable GetDateTable(  string cmdText, params MySqlParameter[] commandParameters)
        {
            try
            {
                DataSet ds = MySqlHelper.ExecuteDataset(ConnectionstringLocalTransaction, cmdText, commandParameters);
                return ds.Tables[0];
            }catch(MySqlException ex)
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
                return dal.MySqlHelper.BatchExec(ConnectionstringLocalTransaction, list);
            }
            catch (MySqlException ex)
            {
                SqlErrMsg = ex.Message;
                SMSLog.Error("SqlDBExec =>BatchExec Exception: ", ex.Message);
            }
            return null;
        }


 

        public override DataTable GetDateTable(string cmdText)
        {
            try
            {
                DataSet ds = MySqlHelper.ExecuteDataset(ConnectionstringLocalTransaction, cmdText, null);
                return ds.Tables[0];
            }
            catch (MySqlException ex)
            {
            }
            return null;
        }

        public override int ExecuteNonQuery(int cmdType, string cmdText, object commandParameters)
        {
            int re = 0;
            DateTime dt1 = DateTime.Now;
            try
            {
                //SMSGLog.Error(cmdText);
                SqlErrMsg = "";
                MySqlParameter[] cmdParameters = null;
                if (commandParameters != null)
                {
                    cmdParameters = commandParameters as MySqlParameter[];
                }
                re = MySqlHelper.ExecuteNonQuery(ConnectionstringLocalTransaction, cmdText, cmdParameters);
                if ((DateTime.Now - dt1).TotalMilliseconds > 3000)
                {
                    SMSLog.Debug("SqlDBExec =>ExecuteNonQuery 耗时：" + (DateTime.Now - dt1).TotalMilliseconds + "毫秒,result=" + re + ",sql=" + cmdText);
                }
                return re;
            }
            catch (MySqlException ex)
            {
                SqlErrMsg = ex.Message;
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception:" + cmdText);
                SMSLog.Error("SqlDBExec =>ExecuteNonQuery Exception: ", ex.Message);

            }
            return 0;
        }

        public override DataTable GetDateTable(int cmdType, string cmdText, object commandParameters)
        {
            try
            {
                DataSet ds = MySqlHelper.ExecuteDataset(ConnectionstringLocalTransaction, cmdText, null);
                return ds.Tables[0];
            }
            catch (MySqlException ex)
            {
            }
            return null;
        }

        public override object ExecuteScalar(string sql)
        {
            throw new NotImplementedException();
        }
    }
}
