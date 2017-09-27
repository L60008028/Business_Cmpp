using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace cmppV2
{
    public class MyBitConverter
    {
        public static byte[] UintToBytes(uint un)
        {
            byte[] re = BitConverter.GetBytes(un);
            Array.Reverse(re);
            return re;
        }




        /// <summary>
        /// 合并两个字节数组
        /// </summary>
        /// <param name="b1">源字节数组1</param>
        /// <param name="b2">源字节数组2</param>
        /// <returns>合并后的字节数组</returns>
        public static byte[] MergeByteArray(byte[] b1, byte[] b2)
        {
            byte[] bs = new byte[b1.Length + b2.Length];

            Array.Copy(b1, 0, bs, 0, b1.Length);
            Array.Copy(b2, 0, bs, b1.Length, b2.Length);
            return bs;
        }

        /// <summary>
        /// int转为2个byte
        /// </summary>
        /// <param name="n">整型数</param>
        /// <param name="buf">输出数组</param>
        /// <param name="offset">buf的偏移量</param>
        public static void int2bytes2(int n, byte[] buf, int offset)
        {
            buf[offset] = (byte)(n >> 8);
            buf[offset + 1] = (byte)n;
        }

        /// <summary>
        /// int转为3个byte
        /// </summary>
        /// <param name="n">整型数</param>
        /// <param name="buf">输出数组</param>
        /// <param name="offset">buf的偏移量</param>
        public static int bytes2int2(byte[] b, int offset)
        {
            return (b[offset + 2] & 0xff) | (b[offset + 1] & 0xff) << 8 | (b[offset] & 0xff) << 16;
        }

        /// <summary>
        /// 2个byte转为short
        /// </summary>
        /// <param name="b">源字节数组</param>
        /// <param name="offset">数组偏移量</param>
        /// <returns>结果short</returns>
        public static short byte2short(byte[] b, int offset)
        {
            return (short)(b[offset + 1] & 0xff | (b[offset] & 0xff) << 8);
        }

        /// <summary>
        /// 2个byte转为short
        /// </summary>
        /// <param name="b">源字节数组</param>
        /// <returns>结果short</returns>
        public static short byte2short(byte[] b)
        {
            return (short)(b[1] & 0xff | (b[0] & 0xff) << 8);
        }

        /// <summary>
        /// 1个byte转为short
        /// </summary>
        /// <param name="b">源字节数组</param>
        /// <param name="offset">数组偏移量</param>
        /// <returns>结果short</returns>
        public static short byte2tinyint(byte[] b, int offset)
        {
            return (short)(b[offset] & 0xff);
        }
    }//end
}//end
