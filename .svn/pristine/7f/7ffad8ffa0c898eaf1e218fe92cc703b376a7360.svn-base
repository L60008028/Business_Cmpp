using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cmppV2
{


    /// <summary>
    /// 命令
    /// </summary>
    public enum Cmpp_Command:uint
    {
         CMPP_CONNECT	=0x00000001,//	请求连接
        CMPP_CONNECT_RESP=	0x80000001	,//请求连接应答
        CMPP_TERMINATE=	0x00000002,//	终止连接
        CMPP_TERMINATE_RESP=	0x80000002	,//终止连接应答
        CMPP_SUBMIT		=0x00000004	,//提交短信
        CMPP_SUBMIT_RESP	=0x80000004	,//提交短信应答
        CMPP_DELIVER	=0x00000005	,//短信下发
        CMPP_DELIVER_RESP	=0x80000005	,//下发短信应答
        CMPP_QUERY=	0x00000006	,//发送短信状态查询
        CMPP_QUERY_RESP=	0x80000006	,//发送短信状态查询应答
        CMPP_CANCEL	=	0x00000007	,//删除短信
        CMPP_CANCEL_RESP=	0x80000007	,//删除短信应答
        CMPP_ACTIVE_TEST=	0x00000008	,//激活测试
        CMPP_ACTIVE_TEST_RESP=	0x80000008	,//激活测试应答
        CMPP_FWD=	0x00000009	,//消息前转
        CMPP_FWD_RESP	=0x80000009	,//消息前转应答
        CMPP_MT_ROUTE=	0x00000010	//MT路由请求

    }

    /// <summary>
    /// 消息包类型
    /// </summary>
    public enum MsgContentFormat : uint
    {
        ASCII = 0,	//纯ASCII字符串
        WRITE = 3,	//写卡操作
        BINARY = 4,	//二进制编码
        UCS2 = 8,	//UCS2编码
        GBK = 15	//GBK编码
    }

    enum MSG_STATE     //CMPP消息在队列中的状态枚举值
    {
        NEW = 0,      //加入到队列等待发送出去
        SENDING = 1,     //正被某个线程锁定
        SENDED_WAITTING = 2,   //发送出去，现在等待resp消息返回
        SENDING_FINISHED = 3 ,  //得到回应，一般等待被清理出队列
        SENDED_FAILD=4 //提交失败
    }

}
