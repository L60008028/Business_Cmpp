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
using bll;

namespace Business_Cmpp
{
    public partial class FmMain : Form
    {
        public FmMain()
        {
            
            InitializeComponent();

        }
       // LocalParams lp = new LocalParams();
        SmsService_jy smssrv = new SmsService_jy();
        private void FmMain_Load(object sender, EventArgs e)
        {
            try
            {
                this.timer1.Enabled = true;
                log4net.Config.XmlConfigurator.Configure();
                FmLog fmlog = new FmLog();
                fmlog.TopLevel = false;
                fmlog.Dock = DockStyle.Fill;
                this.panelMain.Controls.Add(fmlog);
                fmlog.Show();
                this.Text = GlobalModel.Lparams.GatewayName;
                GlobalModel.SetStatusStripInfoHandler = SetStartState;
                // GatewayConfig gc = new GatewayConfig();
                // gc.Read();
            }
            catch (Exception)
            {
 
            }

        }

        private void SetStartState(bool b)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<bool>(x => SetStartState(x)), new object[] { b });
                }
                else
                {
                    if (b)
                    {
                        toolStripBtnStart.Enabled = false;
                        lblState.Text = "已启动";
                        lblStartTime.Text = DateTime.Now.ToString("yyyy/MM/dd H:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    }
                    else
                    {
                        toolStripBtnStart.Enabled = true;
                        lblState.Text = "未启动";

                    }
                }
            }
            catch (Exception)
            {
 
            }
        }
      
        private void toolStripBtnStart_Click(object sender, EventArgs e)
        {

            try
            {

                bool b = smssrv.Start();
                if (b)
                {
                    SetIcon(GlobalModel.Lparams.GatewayName);

                }
            }
            catch (Exception)
            {
 
            }
        }

        private void toolStripBtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (smssrv != null)
                {
                    smssrv.Stop();
                }
            }
            catch (Exception)
            {
 
            }

            Application.Exit();
  
        }

        private void toolStripBtnSet_Click(object sender, EventArgs e)
        {
            try
            {
                FmSet fs = new FmSet();
                fs.ShowDialog();
            }
            catch (Exception)
            {
 
            }
        }

        private void FmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("确定要关闭？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    if (smssrv != null)
                    {
                        smssrv.Stop();
                    }
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception)
            {
 
            }
        }

        private void FmMain_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                {
                    this.Hide();
                    this.notifyIcon1.Visible = true;
                }
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.notifyIcon1.Visible = false;
                }
            }
            catch (Exception)
            {
 
            }
        }

 

        private void SetIcon(string text)
        {
            try
            {
                this.notifyIcon1.Text = text;

                if (text.Length > 0)
                {
                    // 动态生成一个文字图标对象，用于托盘图标的显示
                    Icon icon = ImageCreater.CreateTrayIcon(text);
                    if (icon != null)
                    {
                        notifyIcon1.Icon = icon;
                    }
                }
                else
                {
                    // 如果没有相关文字信息则使用主窗口图标作为托盘图标
                    notifyIcon1.Icon = Business_Cmpp.Properties.Resources.main;
                }
            }
            catch (Exception)
            {
 
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
            catch (Exception)
            {
 
            }
 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                this.lblCurrTime.Text = DateTime.Now.ToString();
            }
            catch (Exception ex)
            {

            }
        }

 

    }//end
}//end
