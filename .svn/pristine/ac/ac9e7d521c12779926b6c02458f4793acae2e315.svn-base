using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace bll
{
    /// <summary>
    /// 网关配置文件
    /// </summary>
    public class GatewayConfig
    {
        public void Read()
        {
            XElement xe = XElement.Load("config.xml");
            if(xe!=null)
            {
                var gatewayname = xe.Element("gatewayname").Value;
                var gatewaynum = xe.Element("gatewaynum").Value;
                var readcontentdealy = xe.Element("readcontentdealy").Value;
                var readcontentnum = xe.Element("readcontentnum").Value;
                var readmobilenum = xe.Element("readmobilenum").Value;
                var cachetime = xe.Element("cachetime").Value;
                var sqlconnstr = xe.Element("sqlconnstr").Value;
                var sqlexecclassname = xe.Element("sqlexecclassname").Value;
                var serviceip = xe.Element("serviceip").Value;
                var serviceport = xe.Element("serviceport").Value;
                var loginname = xe.Element("loginname").Value;
                var password = xe.Element("password").Value;
                var spnumber = xe.Element("spnumber").Value;
                var srcid = xe.Element("srcid").Value;
                var spid = xe.Element("spid").Value;
                var serviceid = xe.Element("serviceid").Value;
                var senddelay = xe.Element("senddelay").Value;

 
            }
        }
    }
}
