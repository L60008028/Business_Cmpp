using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using model;

namespace bll
{
    /// <summary>
    /// 数据库执行接口类
    /// </summary>
    public abstract class IDBExec
    {
        public static string ConnectionstringLocalTransaction = "";
        public static string SqlErrMsg = "";
        public  abstract bool IsConn(string constr);
        public abstract object ExecuteScalar(string sql);
        public  abstract int ExecuteNonQuery(int cmdType, string cmdText, object commandParameters);
        public  abstract DataTable GetDateTable(string cmdText);
        public abstract DataTable GetDateTable(int cmdType, string cmdText, object commandParameters);
        public  abstract int BatchExec(List<string> list);
        public  abstract List<SqlStatementModel> BatchExec(List<SqlStatementModel> list);
        // m_Conn = (IDBExec)Assembly.Load("SMSG").CreateInstance(ns);//获取实例
    }
}
