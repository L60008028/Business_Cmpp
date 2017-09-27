using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace model
{
    public class SqlStatementModel
    {

        public string StringToMatch { get; set; }
        private string cmdTxt;

        /// <summary>
        /// sql语句
        /// </summary>
        public string CmdTxt
        {
            get { return cmdTxt; }
            set { cmdTxt = value; }
        }
        private int execTimer = 0;

        /// <summary>
        /// 已执行次数
        /// </summary>
        public int ExecTimer
        {
            get { return execTimer; }
            set { execTimer = value; }
        }

        private MySqlParameter[] cmdParms = null;
        public MySqlParameter[] MySqlCmdParms
        {
            get { return cmdParms; }
            set { cmdParms = value; }
        }

        public  SqlParameter[] MsSqlCmdParms
        {
            get;
            set;
        }

        private CommandType cmdType = CommandType.Text;
        public CommandType CmdType
        {
            get { return cmdType; }
            set { cmdType = value; }
        }


        public string ToString()
        {
            if (cmdType == CommandType.Text)
            {
                return cmdTxt;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(CmdTxt);
                sb.Append(" ");
                if (MsSqlCmdParms != null)
                {
                    foreach (SqlParameter pa in MsSqlCmdParms)
                    {
                        if (pa.SqlDbType == SqlDbType.VarChar)
                        {
                            sb.Append("'");
                            sb.Append(pa.SqlValue);
                            sb.Append("'");
                        }
                        else
                        {
                            sb.Append(pa.SqlValue);
                        }
                        sb.Append(",");
                    }
                }
                string re = sb.ToString().TrimEnd(',');
                return re;
            }
        }



    }//end
}
