using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using model;
using bll;
using System.Reflection;

namespace Business_Cmpp
{
    public partial class FmSet : Form
    {
        public FmSet()
        {
            InitializeComponent();
        }
        LocalParams lp = new LocalParams();
        private void FmSet_Load(object sender, EventArgs e)
        {
            try
            {
                this.cbxResend.SelectedIndex = 0;
                this.tbGateWayNum.Text = lp.GateWayNum + "";
                this.tbReadContentDealy.Value = lp.ReadContentDealy;
                this.tbReadContentNum.Value = lp.ReadContentNum;
                this.tbReadMobileNum.Value = lp.ReadMobileNum;
                this.tbSqlConnStr.Text = lp.SqlConnStr;
                this.tbSenddelay.Value = lp.Senddelay;
                this.tbServiceIp.Text = lp.ServiceIp;
                this.tbPort.Text = lp.ServicePort;
                this.tbLoginname.Text = lp.LoginName;
                this.tbPwd.Text = lp.Password;
                this.tbSpnumber.Text = lp.Spnumber;
                this.tbspid.Text = lp.Spid;
                this.tbServiceId.Text = lp.ServiceId;
                this.cbxSqlType.Text = lp.SqlExecClassName;
                this.tbGatewayName.Text = lp.GatewayName;
                this.tbSrcId.Text = lp.SrcId;
                this.tbCacheTime.Value = lp.CacheTime;
                this.tbActiveUrl.Text = lp.ActiveMQ_Url;
                this.tbActiveName.Text = lp.ActiveMQ_Name;
                this.tbRedisUrl.Text = lp.RedisUrl;
                this.tbActiveTest.Value = lp.ActiveTest_TICKs;
                this.tbCheckState.Value = lp.ACTIVE_TCP_TICKs;
                this.tbMaxExecNum.Value=lp.Report_MaxExecNum;
                this.tbMaxExecTimes.Value=lp.Report_MaxExecTimes;
                this.tbThreadNum.Value=lp.Report_Threadnum;
                this.tbVersion.Text = lp.Version + "";
                if (lp.IsResend.Equals("0"))
                {
                    this.cbxResend.SelectedItem = "否";
                }
                else
                {
                    this.cbxResend.SelectedItem = "是";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                lp.GateWayNum = int.Parse(this.tbGateWayNum.Text);
                lp.ReadContentDealy = (int)this.tbReadContentDealy.Value;
                lp.ReadContentNum = (int)this.tbReadContentNum.Value;
                lp.ReadMobileNum = (int)this.tbReadMobileNum.Value;
                lp.Senddelay = (int)this.tbSenddelay.Value;
                lp.ServiceIp = this.tbServiceIp.Text;
                lp.ServicePort = this.tbPort.Text;
                lp.LoginName = this.tbLoginname.Text;
                lp.Password = this.tbPwd.Text;
                lp.Spnumber = this.tbSpnumber.Text;
                lp.Spid = this.tbspid.Text;
                lp.ServiceId = this.tbServiceId.Text;
                lp.SqlExecClassName = this.cbxSqlType.Text;
                lp.SqlConnStr = this.tbSqlConnStr.Text;
                lp.GatewayName = this.tbGatewayName.Text.Trim();
                lp.SrcId = this.tbSrcId.Text.Trim();
                lp.CacheTime = (int)this.tbCacheTime.Value;
                lp.ActiveMQ_Name = this.tbActiveName.Text.Trim();
                lp.ActiveMQ_Url = this.tbActiveUrl.Text.Trim();
                lp.RedisUrl = this.tbRedisUrl.Text.Trim();
                lp.ACTIVE_TCP_TICKs = (int)this.tbCheckState.Value;
                lp.ActiveTest_TICKs=(int)this.tbActiveTest.Value;
                lp.Report_MaxExecNum = (int)this.tbMaxExecNum.Value;
                lp.Report_MaxExecTimes = (int)this.tbMaxExecTimes.Value;
                lp.Report_Threadnum = (int)this.tbThreadNum.Value;
                lp.Version = int.Parse(this.tbVersion.Text);
                if (this.cbxResend.SelectedItem.Equals("否"))
                {
                    lp.IsResend = "0";
                }
                else
                {
                    lp.IsResend = "1";
                }
                IDBExec dbexec = (IDBExec)Assembly.Load("bll").CreateInstance(lp.SqlExecClassName);//获取实例
                if (!dbexec.IsConn(lp.SqlConnStr))
                {
                    MessageBox.Show("数据库连接失败:" + lp.SqlConnStr);
                    return;
                }
                lp.Save();
                lp.Reload();
                GlobalModel.Lparams = lp;
                MessageBox.Show("保存成功！");
                this.Close();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
        }
    }//end
}
