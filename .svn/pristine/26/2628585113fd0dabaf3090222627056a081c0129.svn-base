using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace model
{
  /// <summary>
    /// 公共参数
    /// </summary>
    public class LocalParams : ApplicationSettingsBase
    {

        /// <summary>
        /// 版本
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("20")]
        public int Version
        {
            get { return int.Parse(this["Version"].ToString()); }
            set { this["Version"] = value; }

        }


        /// <summary>
        /// 状态回执,每批执行数量
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("10")]
        public int Report_MaxExecNum
        {
            get { return int.Parse(this["Report_MaxExecNum"].ToString()); }
            set { this["Report_MaxExecNum"] = value; }

        }

        /// <summary>
        /// 状态回执，执行失败次数
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("10")]
        public int Report_MaxExecTimes
        {
            get { return int.Parse(this["Report_MaxExecTimes"].ToString()); }
            set { this["Report_MaxExecTimes"] = value; }
        }



        /// <summary>
        /// 状态回执,线程数量
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("10")]
        public int Report_Threadnum
        {
            get { return int.Parse(this["Report_Threadnum"].ToString()); }
            set { this["Report_Threadnum"] = value; }
        }



        /// <summary>
        /// Active激活测试时间,秒
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("30")]
        public int ActiveTest_TICKs
        {
            get { return int.Parse(this["ActiveTest_TICKs"].ToString()); }
            set { this["ActiveTest_TICKs"] = value; }
        }



        /// <summary>
        /// 链路联接检测时间
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("30")]
        public int ACTIVE_TCP_TICKs
        {
            get { return int.Parse(this["ACTIVE_TCP_TICKs"].ToString()); }
            set { this["ACTIVE_TCP_TICKs"] = value; }
        }



        /// <summary>
        /// ActiveMq取数据程序生成的ID
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("1")]
        public string DataId
        {
            get { return this["DataId"].ToString(); }
            set { this["DataId"] = value; }
        }

        /// <summary>
        /// 是否重复提交0否，1是
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("0")]
        public string IsResend
        {
            get { return this["IsResend"].ToString(); }
            set { this["IsResend"] = value; }
        }


        /// <summary>
        /// RedisUrl
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string RedisUrl
        {
            get { return this["RedisUrl"].ToString(); }
            set { this["RedisUrl"] = value; }
        }

        /// <summary>
        /// ActiveMQUrl
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string ActiveMQ_Url
        {
            get { return this["ActiveMQ_Url"].ToString(); }
            set { this["ActiveMQ_Url"] = value; }
        }

        /// <summary>
        /// ActiveMQ name
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string ActiveMQ_Name
        {
            get { return this["ActiveMQ_Name"].ToString(); }
            set { this["ActiveMQ_Name"] = value; }
        }

        /// <summary>
        /// 网关名
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string GatewayName
        {
            get { return this["GatewayName"].ToString(); }
            set { this["GatewayName"] = value; }
        }


        /// <summary>
        /// 读取内容间隔时间
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("1000")]
        public int ReadContentDealy
        {
            get { return int.Parse(this["ReadContentDealy"].ToString()); }
            set { this["ReadContentDealy"] = value; }
        }


        /// <summary>
        /// 读取内容数量
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("100")]
        public int ReadContentNum
        {
            get { return int.Parse(this["ReadContentNum"].ToString()); }
            set { this["ReadContentNum"] = value; }
        }


        /// <summary>
        /// 读取手机号码数量
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("100")]
        public int ReadMobileNum
        {
            get { return int.Parse(this["ReadMobileNum"].ToString()); }
            set { this["ReadMobileNum"] = value; }
        }



        /// <summary>
        /// 网关编号
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("1000")]
        public int GateWayNum
        {
            get { return int.Parse(this["GateWayNum"].ToString()); }
            set { this["GateWayNum"] = value; }
        }


        /// <summary>
        /// 缓存时长
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("60")]
        public int CacheTime
        {
            get { return int.Parse(this["CacheTime"].ToString()); }
            set { this["CacheTime"] = value; }
        }


        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("server=127.0.0.1;user id=root;pwd=jy1314;persistsecurityinfo=True;database=gdkltx;")]
        public string SqlConnStr
        {
            get { return this["SqlConnStr"].ToString(); }
            set { this["SqlConnStr"] = value; }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string SqlExecClassName
        {
            get { return this["SqlExecClassName"].ToString(); }
            set { this["SqlExecClassName"] = value; }
        }


        /// <summary>
        /// 登录名
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string LoginName
        {
            get { return this["LoginName"].ToString(); }
            set { this["LoginName"] = value; }
        }

        /// <summary>
        /// 登录密码
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string Password
        {
            get { return this["Password"].ToString(); }
            set { this["Password"] = value; }
        }


        /// <summary>
        /// 发送号码
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string Spnumber
        {
            get { return this["Spnumber"].ToString(); }
            set { this["Spnumber"] = value; }
        }


        /// <summary>
        /// 发送号码
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string SrcId
        {
            get { return this["SrcId"].ToString(); }
            set { this["SrcId"] = value; }
        }


        /// <summary>
        /// 信息内容来源(SP_Id)
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string Spid
        {
            get { return this["Spid"].ToString(); }
            set { this["Spid"] = value; }
        }


        /// <summary>
        /// 服务id
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string ServiceId
        {
            get { return this["ServiceId"].ToString(); }
            set { this["ServiceId"] = value; }
        }


        /// <summary>
        ///提交频率
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public int Senddelay
        {
            get { return int.Parse(this["Senddelay"].ToString()); }
            set { this["Senddelay"] = value; }
        }


        /// <summary>
        /// 服务IP
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string ServiceIp
        {
            get { return this["ServiceIp"].ToString(); }
            set { this["ServiceIp"] = value; }
        }

        /// <summary>
        /// 服务端口
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string ServicePort
        {
            get { return this["ServicePort"].ToString(); }
            set { this["ServicePort"] = value; }
        }



       

    }//end



}
