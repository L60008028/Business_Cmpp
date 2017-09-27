using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using model;

namespace cmppV2
{
    /// <summary>
    /// 打包基类
    /// </summary>
   public abstract class EnPackage
    {
        public abstract byte[] Encode(); 

        /// <summary>
        /// 打包包头
        /// </summary>
        /// <returns></returns>
        protected byte[] Header(CmppHeaderModel _head)
        {
            ArrayList al = new ArrayList(this.Uint4ToBytes(_head.Total_Length));
            al.AddRange(this.Uint4ToBytes(_head.Command_Id));
            al.AddRange(this.Uint4ToBytes(_head.Sequence_Id));
            return al.ToArray(typeof(byte)) as byte[];
        }

       /// <summary>
       /// 4个字节,int转byte[]
       /// </summary>
       /// <param name="m"></param>
       /// <returns></returns>
        protected byte[] Int32ToBit(int m)
        {
            byte[] bt = new byte[4];
            int tm = IPAddress.HostToNetworkOrder(m);//转成大端
            bt = BitConverter.GetBytes(tm);
            return bt;
        }

       /// <summary>
       /// 4个字节,uint 转成byte[]
       /// </summary>
       /// <param name="un"></param>
       /// <returns></returns>
        protected byte[] Uint4ToBytes(uint un)
        {
           // int tm = IPAddress.HostToNetworkOrder((int)un);//转成大端
            byte[] re = BitConverter.GetBytes(un);
            Array.Reverse(re);
            return re;
        }

       /// <summary>
       /// uint64 转byte[]
       /// </summary>
       /// <param name="un"></param>
       /// <returns></returns>
        protected byte[] Uint64ToBytes(UInt64 un,int len)
        {
            // int tm = IPAddress.HostToNetworkOrder((int)un);//转成大端
            byte[] b=new byte[len];
            byte[] re = BitConverter.GetBytes(un);
            Array.Reverse(re);
            re.CopyTo(b, 0);
            return b;
        }

       /// <summary>
       /// uint转byte[]
       /// </summary>
       /// <param name="m"></param>
       /// <param name="len"></param>
       /// <returns></returns>
        protected byte[] UintToBytes(uint m, int len)
        {
            byte[] re = new byte[len];
            byte[] tmp = BitConverter.GetBytes(m);
            Array.Reverse(tmp);
            tmp.CopyTo(re, 0);
            return re;
        }

       /// <summary>
       /// 1位uint转byte[]
       /// </summary>
       /// <param name="m"></param>
       /// <returns></returns>
        protected byte[] Uint1ToBytes(uint m)
        {
            byte[] re = new byte[1];
            re[0] = (byte)m;
            return re;
        }

        protected byte[] StringToBytes(string m, int len)
        {
            byte[] re = new byte[len];
            if(string.IsNullOrEmpty(m))
            {              
                return re;
            }
            if(m.Length>len)
            {
                m = m.Substring(0, len);
            }
             
            byte[] strbt = Encoding.ASCII.GetBytes(m);
            strbt.CopyTo(re, 0);

            return re;
        }

    }//end
}//end
