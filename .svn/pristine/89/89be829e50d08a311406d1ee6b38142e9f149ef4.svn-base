using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using ServiceStack.Redis;

namespace bll
{
    public class RedisManager
    {
        private static PooledRedisClientManager _pcm = null;

        private static bool Init()
        {
            try
            {
                List<string> listWrite = new List<string>() { "192.168.10.58:6379" };// "192.168.10.58:6379"
                List<string> readHosts = new List<string>() { GlobalModel.Lparams.RedisUrl };
                _pcm = CreateManager(readHosts.ToArray(), listWrite.ToArray(), 0);
                return true;
            }
            catch (Exception ex)
            {
                SMSLog.Error("RedisManager::Init", ex.Message);
            }
            return false;
        }

        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts, int initialDB = 0)
        {
            if (_pcm == null)
            {
                _pcm = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig()
                {
                    MaxWritePoolSize = 10,
                    MaxReadPoolSize = 10,
                    AutoStart = true
                });
            }
            return _pcm;
        }

        public static T GetVal<T>(string key)
        {
            T content = default(T);
            try
            {
                if (_pcm == null)
                {
                    Init();
                }
                using (var redis = _pcm.GetClient())
                {

                    content = redis.Get<T>(key);
                }
            }
            catch (Exception ex)
            {
                _pcm = null;
                SMSLog.Error("RedisManager::GetVal:从redis中取数据异常：", ex.Message);
            }
            return content;
        }

    }//end
}//end
