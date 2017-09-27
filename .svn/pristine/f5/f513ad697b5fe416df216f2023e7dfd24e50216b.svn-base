using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// 队列消息
    /// </summary>
    public class QueueItem
    {
        private uint _sequence;    //消息索引 就是流水号


        private uint _msgType;    //消息的类别就是COMMAND_ID,根据此值决定 Object _msgObj的原类型


        private int _failedCount = 0;   //失败计数，如果失败次数超过3此需要进行清理


        private int _msgState = 0;   //当前消息状态,具体为 MSG_STATE的枚举类型


        private object _msgObj;    //存放消息对象,具体类型参考 _msgType


        private DateTime _inQueueTime;  //消息进入队列的时间,上次消息的发送时间


        public uint Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        public uint MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        public int FailedCount
        {
            get { return _failedCount; }
            set { _failedCount = value; }
        }

        public int MsgState
        {
            get { return _msgState; }
            set { _msgState = value; }
        }


        public object MsgObj
        {
            get { return _msgObj; }
            set { _msgObj = value; }
        }

        public DateTime InQueueTime
        {
            get { return _inQueueTime; }
            set { _inQueueTime = value; }
        }


    }//end
}//end
