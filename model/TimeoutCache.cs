using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace model
{
    /// <summary>
    /// 缓存
    /// </summary>
    public class TimeoutCache
    {
        private ConcurrentDictionary<object, object> _cachedic = new ConcurrentDictionary<object, object>();
        private int _totalSeconds;
        private System.Threading.Timer _timer;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="seconds">秒</param>
        public TimeoutCache(int seconds)
        {
            _totalSeconds = seconds;
            _timer = new System.Threading.Timer(new System.Threading.TimerCallback(TCallback2), null, 0, seconds * 1000);
        }

        private void TCallback2(object obj)
        {
            SMSLog.Debug("TimeoutCache=====TCallback2======>开始清除缓存数据:" + DateTime.Now);

            try
            {
                int c = _cachedic.Keys.Count;
                object[] keys = new object[c];
                object[] vals = new object[c];
                _cachedic.Keys.CopyTo(keys, 0);
                _cachedic.Values.CopyTo(vals, 0);
                for (int i = 0; i < vals.Length; i++)
                {
                    try
                    {
                        TimeoutCacheObject tco = vals[i] as TimeoutCacheObject;
                        if (tco != null)
                        {
                            TimeSpan ts = DateTime.Now - tco.Timestamp;
                            if (ts.TotalSeconds > _totalSeconds)
                            {
                                if (_cachedic.ContainsKey(keys[i]))
                                {
                                    object movobj = null;
                                    _cachedic.TryRemove(keys[i], out movobj);

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SMSLog.Error("TimeoutCache=TCallback2=>删除缓存数据异常：" + ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                SMSLog.Error("TimeoutCache=TCallback2=>删除缓存数据异常：" + e.Message);
            }

            SMSLog.Debug("TimeoutCache=====TCallback2======>结束清除缓存数据:" + DateTime.Now);

        }
        private void TCallback(object obj)
        {
            try
            {
                SMSLog.Debug("TimeoutCache===========>开始清除缓存数据:" + DateTime.Now);
                IDictionary<object, object> tcachedic = new Dictionary<object, object>();
                tcachedic = _cachedic;
                if (tcachedic != null)
                {
                    foreach (object key in tcachedic.Keys)
                    {
                        try
                        {
                            TimeoutCacheObject tco = (TimeoutCacheObject)tcachedic[key];
                            TimeSpan ts = DateTime.Now - tco.Timestamp;
                            if (ts.TotalSeconds > _totalSeconds)
                            {
                                if (_cachedic.ContainsKey(key))
                                {
                                    object movobj = null;
                                    _cachedic.TryRemove(key, out movobj);

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            SMSLog.Error("TimeoutCache==>删除缓存数据异常：" + ex.Message);
                        }
                    }
                    tcachedic.Clear();
                }
                SMSLog.Debug("TimeoutCache===========>结束清除缓存数据:" + DateTime.Now);
            }
            catch (Exception ex)
            {

                SMSLog.Error("TimeoutCache==>清除缓存数据异常：" + ex.Message);
            }
        }
        /// <summary>
        /// 秒
        /// </summary>
        public int TotalSeconds
        {
            get { return _totalSeconds; }
            set { _totalSeconds = value; }
        }


        public void Add(object key, object obj)
        {
            try
            {
                //lock (_cachedic)
                // {
                if (!_cachedic.ContainsKey(key))
                {

                    TimeoutCacheObject tco = new TimeoutCacheObject(obj, DateTime.Now);
                    _cachedic.TryAdd(key, tco);

                }
                // }
            }
            catch (Exception ex)
            {

                SMSLog.Error("TimeoutCache：Add=>添加缓存数据异常：" + ex.Message);
            }
        }


        public void Del(object key)
        {
            try
            {
                //lock (_cachedic)
                // {
                if (_cachedic.ContainsKey(key))
                {

                    object obj;
                    _cachedic.TryRemove(key,out obj);

                }
                // }
            }
            catch (Exception ex)
            {

                SMSLog.Error("TimeoutCache：Add=>添加缓存数据异常：" + ex.Message);
            }
        }

        public object Get(object key)
        {
            try
            {
                if (_cachedic.ContainsKey(key))
                {
                    TimeoutCacheObject tco = (TimeoutCacheObject)_cachedic[key];
                    if (tco != null)
                    {
                        TimeSpan ts = DateTime.Now - tco.Timestamp;
                        if (ts.TotalSeconds > _totalSeconds)
                        {
                            object movobj = null;
                            _cachedic.TryRemove(key, out movobj);
                            return tco.Obj;
                        }
                        else
                        {
                            return tco.Obj;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                SMSLog.Debug("TimeoutCache：Get=>读取存数据异常：" + ex.Message);
            }
            return null;
        }
    }//end

    class TimeoutCacheObject
    {
        public TimeoutCacheObject(object obj, DateTime dt)
        {
            _obj = obj;
            _timestamp = dt;
        }

        private object _obj;

        public object Obj
        {
            get { return _obj; }
            set { _obj = value; }
        }
        private DateTime _timestamp;

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }


    }//end
}//end
