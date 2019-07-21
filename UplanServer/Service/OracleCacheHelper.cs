using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UplanServer
{
    public class OracleCacheHelper : IRedisHelper
    {
        private QoEDbContext db;
        public OracleCacheHelper()
        {
            this.db = new QoEDbContext();
        }
        public string Get(string key)
        {
            var rt = db.SysCacheTable.Where(a => a.Key == key).FirstOrDefault();
            if (rt == null) return "";
            if (string.IsNullOrEmpty(rt.Value)) return "";
            return rt.Value;
        }

        public T Get<T>(string key)
        {
            string result = Get(key);
            if (string.IsNullOrEmpty(result)) return default(T);
            T t = JsonConvert.DeserializeObject<T>(result);
            return t;
        }

        public bool Set(string key, string value)
        {
            try
            {
                var rt = db.SysCacheTable.Where(a => a.Key == key).FirstOrDefault();
                if (rt != null)
                {
                    db.Entry(rt).State = System.Data.Entity.EntityState.Deleted;
                }
                SysCacheInfo cache = new SysCacheInfo();
                cache.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cache.Key = key;
                cache.Value = value;
                cache.Datalen = value.Length;
                db.SysCacheTable.Add(cache);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Set(string key, object value)
        {
            string json = "";
            if (value != null)
            {
                json = JsonConvert.SerializeObject(value);
            }
            return Set(key, json);
        }
    }
}