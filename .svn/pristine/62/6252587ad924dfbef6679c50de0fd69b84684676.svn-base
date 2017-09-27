using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class SMSGErr
    {
        public const int IS_OK = 0;						//没有错误

        public const int MESSAGE_FORMAT = 31001;	//短信格式错误
        public const int DIRTY_MOBILE = 31002;	//错误手机号码
        public const int MOBILE_NUMBER = 31003;	//手机号码格式错
        public const int SERVER_NUMBER = 31004;	//服务号码格式错
        public const int CONTENT_LENGTH = 31005;	//内容超长
        public const int REITERATION = 31006;	//重复短信
        public const int LAWLESSWORD = 31007;	//短信中含有非法字符
        public const int BLACKLIST = 31008;	//黑名单用户
        public const int PACKENCODE = 31009;	//短信打包编码错误
        public const int MAXRESEND = 31010;	//超过最大重发次数
        public const int EXPTIME = 31011;	//短信超时
        public const int SMCERROR = 31012;	//短信中心出错
        public const int NOSIGNTXT = 31013;//无签名

        public const int ERROR = 31999;					//发送失败

        //特殊状态
        public const int UNCLEAR = 32000;	//短信状态未明(未收到submit应答)

        //短信需要重发的状态, 必须定义在32001-32767之间
        public const int SMIBUSY = 32001;	//短信接口忙
        public const int SMCBUSY = 32002;	//短信中心忙
        public const int UNSEND = 32003;	//因程序退出短信未发出
        public const int SMCDEAD = 32004;	//短信中心不可用
        public const int SYSERROR = 32005;	//系统出错
    }
}
