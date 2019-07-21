using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace UplanServer
{
    public class RedisHelper
    {
        /// <summary>
        /// 连接字符串，一般写在配置文件里面
        /// </summary>
        private static readonly string ConnectionString = Module.redisConnstring;
        /// <summary>
        /// 上锁，单例模式
        /// </summary>
        private static object locker = new object();
        /// <summary>
        /// 连接对象
        /// </summary>
        private volatile IConnectionMultiplexer _connection;
        /// <summary>
        /// 数据库
        /// </summary>
        private IDatabase _db;
        #region 创建Redis实例
        public RedisHelper()
        {
            _connection = ConnectionMultiplexer.Connect(ConnectionString);
            _db = GetDatabase();
        }


        private static RedisHelper redisHelper;

        public static RedisHelper GetRedisHelper()
        {

            if (redisHelper == null)
            {
                lock (locker)
                {
                    if (redisHelper == null)
                    {
                        redisHelper = new RedisHelper();
                    }
                }
            }
            return redisHelper;
        }
        #endregion


        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        protected IConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection;
            }
            lock (locker)
            {
                if (_connection != null && _connection.IsConnected)
                {
                    return _connection;
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                }
                _connection = ConnectionMultiplexer.Connect(ConnectionString);
            }

            return _connection;
        }
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">过期时间</param>
        public virtual bool Set(string key, object data, int? cacheTime = null)
        {
            try
            {
                if (data == null)
                {
                    LogHelper.Log("Redis Set failed data is null");
                    return false;
                }
                var entryBytes = Serialize(data);
                if (cacheTime != null)
                {
                    var expiresIn = TimeSpan.FromSeconds(Convert.ToDouble(cacheTime));
                    return _db.StringSet(key, entryBytes, expiresIn);
                }
                else
                {
                    return _db.StringSet(key, entryBytes);
                }
            }
            catch (Exception e)
            {
                LogHelper.Log("Redis Set Error " + e.ToString());
                return false;
            }
            
        }

        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            try
            {
                var rValue = _db.StringGet(key);
                if (!rValue.HasValue)
                {
                    return default(T);
                }
                try
                {
                    var result = Deserialize<T>(rValue);
                    return result;
                }
                catch (Exception e)
                {
                    return default(T);
                }
            }
            catch (Exception)
            {

                return default(T);
            }
            

        }
        public virtual bool Delete(string key)
        {
            return _db.KeyDelete(key);
        }
        /// <summary>
        /// 根据键获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string Get(string key)
        {
            try
            {
                var rValue = _db.StringGet(key);
                if (!rValue.HasValue)
                {
                    return "";
                }

                return rValue.ToString();
            }
            catch (Exception)
            {
                return "";
            }
            
        }

        /// <summary>
        /// 判断键是否已存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExit(string key)
        {
            return _db.KeyExists(key);
        }

        /// <summary>
        /// 判断是否已经设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return _db.KeyExists(key);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns>byte[]</returns>
        private byte[] Serialize(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(json);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return default(T);
            }
            var json = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}