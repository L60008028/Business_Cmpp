using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace cmppV2
{
    /// <summary>
    /// 解析
    /// </summary>
    public abstract class DePackage
    {
        public abstract void Decode(BinaryReader br);


        /// <summary>
        /// 读int
        /// </summary>
        /// <param name="br"></param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        protected int ReadInt(BinaryReader br,int len)
        {
            try
            {
                byte[] temp = br.ReadBytes(len);
                Array.Reverse(temp);
                return BitConverter.ToInt32(temp, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decode=>ReadInt()Exception:" + ex.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 读int32,4个字节
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected int ReadInt32(BinaryReader br)
        {
            try
            {
                byte[] temp = br.ReadBytes(4);
                Array.Reverse(temp);
                return BitConverter.ToInt32(temp, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decode=>ReadInt()Exception:" + ex.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 读1个字节转成uint
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected uint ReadUInt(BinaryReader br)
        {
            try
            {
                Console.WriteLine("ReadUInt==>br.BaseStream.Length=" + br.BaseStream.Length);
                byte[] temp = br.ReadBytes(1);
                Array.Reverse(temp);              
                uint re = Convert.ToUInt32(temp[0]); 
                return re;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decode=>ReadInt()Exception:" + ex.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 读取8个字节转成long
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected UInt64 ReadUInt64(BinaryReader br)
        {

            try
            {
                Console.WriteLine("ReadUInt64==>br.BaseStream.Length=" + br.BaseStream.Length); 
                byte[] temp = br.ReadBytes(8);
                Array.Reverse(temp);
                UInt64 re = BitConverter.ToUInt64(temp, 0);
                return re;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Decode=>ReadUInt64()Exception:" + ex.ToString());
            }
            return 0;
        }


    
        /// <summary>
        /// 读N个字节转成uint
        /// </summary>
        /// <param name="br">流</param>
        /// <param name="len">读取的长度</param>
        /// <returns></returns>
        protected uint ReadUInt(BinaryReader br,int len)
        {
            try
            {
                byte[] temp = br.ReadBytes(len);
                Array.Reverse(temp);
                uint re = Convert.ToUInt32(temp);
                return re;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decode=>ReadInt()Exception:" + ex.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 读byte[]
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected byte[] ReadBytes(BinaryReader br, int len)
        {
            byte[] b=new byte[len];
            try
            {

                b= br.ReadBytes(len);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decode=>ReadBytes()Exception:" + ex.ToString());
            }
            return b;
        }




        /// <summary>
        /// 读string
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        protected string ReadString(BinaryReader br,int len)
        {
            try
            { 
                string re = this.BytesToString(br.ReadBytes(len));
                return re.Trim('\0');
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decode=>ReadString()Exception:" + ex.ToString());
            }
            return "";
        }
        /// <summary>
        /// byte转string
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        protected string BytesToString(byte[] by)
        {
            return Encoding.ASCII.GetString(by);
        }

    }//end
}
