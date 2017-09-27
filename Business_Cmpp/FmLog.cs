using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using model;

namespace Business_Cmpp
{
    public partial class FmLog : Form
    {
        public FmLog()
        {
            InitializeComponent();
            GlobalModel.WriteLogHandler += WriteLog;
        }


        private void WriteLog(string msg)
        {
            try
            {
                if (this.rtbLog.InvokeRequired)
                {
                    this.Invoke(new Action<string>(x => WriteLog(x)), new object[] { msg });
                }
                else
                {
                    Application.DoEvents();
                    if(rtbLog.TextLength>=rtbLog.MaxLength)
                    {
                        rtbLog.Clear();
                    }
                    //rtbLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "==>" + msg + Environment.NewLine);
                    rtbLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "==>" + msg);
                    rtbLog.AppendText("\r\n");
                    if(GlobalModel.IsLogScroll)
                    {
                      rtbLog.SelectionStart = rtbLog.Text.Length;
                      rtbLog.ScrollToCaret();
                    }
                }
            }
            catch (Exception)
            {


            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked == true)
                {
                    GlobalModel.IsLogScroll = true;
                }
                else
                {
                    GlobalModel.IsLogScroll = false;
                }
            }
            catch (Exception )
            {
 
            }
        }

        private void cboxIsShowLog_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboxIsShowLog.Checked == true)
                {
                    GlobalModel.IsShowLog = true;
                }
                else
                {
                    GlobalModel.IsShowLog = false;
                }
            }
            catch (Exception)
            {

            }
        }


    }
}
