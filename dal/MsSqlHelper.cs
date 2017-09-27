﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using model;
using System.Collections;

namespace dal
{
    /// <summary>
    /// sqlserver数据库处理类
    /// </summary>
    public abstract class MsSqlHelper
    {
        //数据库连接字符串
        public static string ConnectionstringLocalTransaction = @"Data Source=192.168.1.221;Initial Catalog=EMAS_PROC;User ID=sa;Password=jy1314;Min Pool Size=10";
        // public static string ConnectionstringLocalTransaction = string.Format("Data Source=.\\SQLEXPRESS;AttachDbFilename={0}\\db\\JYKLTXSoftwareDB_data.MDF;Integrated Security=True;Connect Timeout=30;User Instance=True", Application.StartupPath);
        public static SqlConnection sqlconnection = null;
        private static int cmdtimeout = 10;//超时5秒

        // 用于缓存参数的HASH表
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        ///  给定连接的数据库用假设参数执行一个sql命令（不返回数据集）
        /// </summary>
        /// <param name="connectionstring">一个有效的连接字符串</param>
        /// <param name="commandType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="commandText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(string connectionstring, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            try
            {
                SqlCommand cmd = new SqlCommand();
                
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {

                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        public static DataTable GetDateTable(string connectionstring, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            //创建一个SqlCommand对象
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = cmdtimeout;
            //创建一个SqlConnection对象
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {

                //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，
                //因此commandBehaviour.CloseConnection 就不会执行
                try
                {
                    //调用 PrepareCommand 方法，对 SqlCommand 对象设置参数
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    //清除参数
                    cmd.Parameters.Clear();
                    sda.Dispose();
                    //conn.Close();
                    return dt;
                }
                catch
                {
                    conn.Close();
                    throw;
                }

            }

        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="connectionstring">数据库连接字符串</param>
        /// <param name="bathSize">块大小</param>
        /// <param name="tableName">表名</param>
        /// <param name="dt">数据</param>
        /// <param name="colName">列名</param>
        public static void BatchInsert(string connectionstring, int bathSize, string tableName, DataTable dt, string[] colName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                    {
                        bulk.BatchSize = bathSize;
                        bulk.DestinationTableName = tableName;
                        bulk.BulkCopyTimeout = 2000;
                        foreach (string c in colName)
                        {
                            bulk.ColumnMappings.Add(c, c);
                        }
                        bulk.WriteToServer(dt);

                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 批量执行SQL语句
        /// </summary>
        /// <param name="list">sqls</param>
        /// <returns>返回失败语句</returns>
        public static List<string> BatchExec(string connectionstring, List<string> list)
        {
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = cmdtimeout;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                int execNum = 0;
                List<string> errlist = new List<string>();
                foreach (string str in list)
                {
                    try
                    {
                        cmd.CommandText = str;
                        int re = cmd.ExecuteNonQuery();
                        if (re > 0)
                        {
                            execNum++;
                        }
                        else
                        {
                            errlist.Add(str);
                        }
                    }
                    catch (SqlException)
                    {
                        errlist.Add(str);
                    }
                }

                return errlist;
            }


        }


        /// <summary>
        /// 批量执行SQL语句
        /// </summary>
        /// <param name="list">sqls</param>
        /// <returns>返回失败语句</returns>
        public static List<SqlStatementModel> BatchExec(string connectionstring, List<SqlStatementModel> list)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    List<SqlStatementModel> errlist = new List<SqlStatementModel>();
                    foreach (SqlStatementModel sm in list)
                    {
                        try
                        {

                            int re = ExecuteNonQuery(conn, sm.CmdType, sm.CmdTxt, sm.MsSqlCmdParms);
                            if (re == 0)
                            {
                                errlist.Add(sm);
                            }

                        }
                        catch (Exception ex)
                        {

                            errlist.Add(sm);
                        }
                    }
                    return errlist;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }



        }

        /// <summary>
        /// 用现有的数据库连接执行一个sql命令（不返回数据集）
        /// </summary>
        /// <param name="conn">一个现有的数据库连接</param>
        /// <param name="commandType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="commandText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                int val = 0;
                SqlException ex = null;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = cmdtimeout;
                try
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                    val = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    ex = e;
                }
                finally
                {
                    cmd.Parameters.Clear();
                    connection.Close();
                    if (ex != null)
                    {
                        throw ex;
                    }
                }
                return val;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        /// <summary>
        ///使用现有的SQL事务执行一个sql命令（不返回数据集）
        /// </summary>
        /// <remarks>
        ///举例:  
        ///  int result = ExecuteNonQuery(connstring, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">一个现有的事务</param>
        /// <param name="commandType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="commandText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = cmdtimeout;
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                trans.Connection.Close();
                return val;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 用执行的数据库连接执行一个返回数据集的sql命令
        /// </summary>
        /// <remarks>
        /// 举例:  
        ///  SqlDataReader r = ExecuteReader(connstring, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionstring">一个有效的连接字符串</param>
        /// <param name="commandType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="commandText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>包含结果的读取器</returns>
        public static SqlDataReader ExecuteReader(string connectionstring, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            //创建一个SqlCommand对象
            SqlCommand cmd = new SqlCommand();
            //创建一个SqlConnection对象
            SqlConnection conn = new SqlConnection(connectionstring);

            //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，
            //因此commandBehaviour.CloseConnection 就不会执行
            try
            {
                //调用 PrepareCommand 方法，对 SqlCommand 对象设置参数
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                //调用 SqlCommand  的 ExecuteReader 方法
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //清除参数
                cmd.Parameters.Clear();
                // conn.Close();
                return reader;
            }
            catch
            {
                //关闭连接，抛出异常
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 用指定的数据库连接字符串执行一个命令并返回一个数据集的第一列
        /// </summary>
        /// <remarks>
        ///例如:  
        ///  Object obj = ExecuteScalar(connstring, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        ///<param name="connectionstring">一个有效的连接字符串</param>
        /// <param name="commandType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="commandText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns>
        public static object ExecuteScalar(string connectionstring, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = cmdtimeout;
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 用指定的数据库连接执行一个命令并返回一个数据集的第一列
        /// </summary>
        /// <remarks>
        /// 例如:  
        ///  Object obj = ExecuteScalar(connstring, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个存在的数据库连接</param>
        /// <param name="commandType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="commandText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = cmdtimeout;
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                connection.Close();
                return val;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 将参数集合添加到缓存
        /// </summary>
        /// <param name="cacheKey">添加到缓存的变量</param>
        /// <param name="cmdParms">一个将要添加到缓存的sql参数集合</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 找会缓存参数集合
        /// </summary>
        /// <param name="cacheKey">用于找回参数的关键字</param>
        /// <returns>缓存的参数集合</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 准备执行一个命令
        /// </summary>
        /// <param name="cmd">sql命令</param>
        /// <param name="conn">Sql连接</param>
        /// <param name="trans">Sql事务</param>
        /// <param name="cmdType">命令类型例如 存储过程或者文本</param>
        /// <param name="cmdText">命令文本,例如：Select * from Products</param>
        /// <param name="cmdParms">执行命令的参数</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.CommandTimeout = cmdtimeout;
                cmd.Connection = conn;
                cmd.CommandText = cmdText;

                if (trans != null)
                    cmd.Transaction = trans;

                cmd.CommandType = cmdType;

                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                        cmd.Parameters.Add(parm);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

    }//end
}
