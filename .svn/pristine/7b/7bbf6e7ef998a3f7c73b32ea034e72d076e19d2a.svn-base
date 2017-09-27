using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using model;
using cmppV2;

namespace Business_Cmpp
{
    public partial class FmMain : Form
    {
        public FmMain()
        {
            
            InitializeComponent();
        }

        private void FmMain_Load(object sender, EventArgs e)
        {
            GlobalModel.WriteLogHandler += WriteLog;
            log4net.Config.XmlConfigurator.Configure(); 
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
                    rtbLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "==>" + msg + Environment.NewLine);
                    rtbLog.SelectionStart = rtbLog.Text.Length;
                    rtbLog.ScrollToCaret();
                }
            }
            catch (Exception)
            {
                
                 
            }
        }

 


        Dictionary<int, CmppSendThread> _cstDic = new Dictionary<int, CmppSendThread>();
        private void toolStripBtnStart_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 3;i++ )
            {
                AccountInfoModel aim = new AccountInfoModel();
                aim.eprId = 77+i;
                aim.loginname = "44191"+i;
                aim.password = "690468"+i;
                aim.senddelay = 50;
                aim.serviceid = "MJS6919908"+i;
                aim.spid = "44191"+i;
                aim.spnumber = "10690468"+i;
                CmppSendThread cst = new CmppSendThread(aim);
                cst.WriteLogHandler += new MyDelegate.WriteLogDelegate(WriteLog);
                _cstDic.Add(aim.eprId,cst);
                cst.Start();
            }


        }

        private void toolStripBtnStop_Click(object sender, EventArgs e)
        {
            foreach(int k in _cstDic.Keys)
            {
                SmsModel sm = new SmsModel();
                sm.eprId = k;
                sm.content = "aaaaaaaabbbbbb中方术"+k;
                sm.subId = k+"";
                sm.mobile = "136824885" + k;
                _cstDic[k].AddPriorityQueue(sm);
            }
        }



    }//end
}//end
