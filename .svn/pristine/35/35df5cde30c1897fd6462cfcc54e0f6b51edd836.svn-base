using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using model;

namespace Business_Cmpp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FmMain());
            }
            catch (Exception)
            {
 
            }
        }


        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

            try
            {
                string str = "";
                string strDateInfo = "应用程序的主入口点。未处理的异常：" + DateTime.Now.ToString() + "\r\n";
                Exception error = e.Exception as Exception;
                if (error != null)
                {
                    str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                         error.GetType().Name, error.Message, error.StackTrace);
                }
                else
                {
                    str = string.Format("应用程序线程错误:{0}", e);
                }

                MyTools.SendSMS("网关【" + GlobalModel.Lparams.GatewayName + "】ThreadException,请查看状态！");
                SMSLog.Error(str);
                MessageBox.Show("FUCK！FUCK！FUCK！", "Application_ThreadException", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                
                
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                string str = "";
                Exception error = e.ExceptionObject as Exception;
                string strDateInfo = "应用程序的主入口点。未处理的异常：" + DateTime.Now.ToString() + "\r\n";
                if (error != null)
                {
                    str = string.Format(strDateInfo + "Application UnhandledException:{0};\n\r堆栈信息:{1}", error.Message, error.StackTrace);
                }
                else
                {
                    str = string.Format("Application UnhandledError:{0}", e);
                }
                MyTools.SendSMS("网关【" + GlobalModel.Lparams.GatewayName + "】UnhandledError,请查看状态！");
                SMSLog.Error(str);
                MessageBox.Show("FUCK！FUCK！FUCK！", "CurrentDomain_Unhandled", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
 
            }
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="str"></param>
        static void writeLog(string str)
        {
            try
            {
                if (!Directory.Exists("ErrLog"))
                {
                    Directory.CreateDirectory("ErrLog");
                }

                using (StreamWriter sw = new StreamWriter(@"ErrLog\ErrLog.txt", true))
                {
                    sw.WriteLine(str);
                    sw.WriteLine("---------------------------------------------------------");
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
 
            }
        }

    }//end
}
