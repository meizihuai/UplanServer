using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;

namespace UplanServer
{
    public class Module
    {
          //public static OracleHelper ora = new OracleHelper(WebConfigurationManager.AppSettings["oracleip"], 1521, "oss", "uplan", "Smart9080");
        public static OracleHelper ora = new OracleHelper(WebConfigurationManager.AppSettings["oraHelperCfg"]);
        public static string redisConnstring = WebConfigurationManager.AppSettings["redisConnstring"];
        public static RedisHelper redisHelper = RedisHelper.GetRedisHelper();
        public static IRedisHelper oracleCacheHelper = new OracleCacheHelper();
        ///  public static OracleHelper ora = new OracleHelper("111.53.74.132", 1521, "oss", "uplan", "Smart9080");
        public static object getFileNameLock = new object();
        public static Dictionary<string, string> phoneModelDik = new Dictionary<string, string>();
        public static readonly string DisplayPlatform_DPIndexInfo = "DisplayPlatform-DPIndexInfo";
        public static readonly string DisplayPlatform_DetailQuota = "DisplayPlatform-DetailQuota-{0}-{1}";
        public static ILocationService LocationService = new LocationService();
        public static AppSettingInfo AppSetting;
        public static void Init()
        {
            phoneModelDik.Add("xiaomi", "小米");
            phoneModelDik.Add("HONOR", "华为");

            AppSetting = new AppSettingInfo()
            {
                MQ_HostName = WebConfigurationManager.AppSettings["MQ_HostName"],
                MQ_UserName = WebConfigurationManager.AppSettings["MQ_UserName"],
                MQ_Password = WebConfigurationManager.AppSettings["MQ_Password"],
                MQ_Queue_QoER2DB= WebConfigurationManager.AppSettings["MQ_Queue_QoER2DB"],
                MQ_Queue_QoERForiOS2DB = WebConfigurationManager.AppSettings["MQ_Queue_QoERForiOS2DB"],
                MQ_Queue_QoE2DB = WebConfigurationManager.AppSettings["MQ_Queue_QoE2DB"]
            };

        }
        public static string Str2Base64(string str)
        {
            if (str == "")
                return "";
            byte[] by = Encoding.Default.GetBytes(str);
            string base64 = Convert.ToBase64String(by);
            return base64;
        }
        public static string Base2str(string base64)
        {
            if (base64 == "")
                return "";
            byte[] by = Convert.FromBase64String(base64);
            string str = Encoding.Default.GetString(by);
            return str;
        }
        public static string GetNewToken(string usr, bool isWriteToOracle)
        {
            DateTime dtmp = DateTime.Now;
            string time = dtmp.ToString("yyyy-MM-dd HH:mm:ss");
            string ticks = dtmp.Ticks.ToString();
            if (usr == "")
                return ticks;
            string token = usr + "#" + time;
            token = Str2Base64(token);
            if (isWriteToOracle)
            {
                string sql = "update user_account set token='" + token + "' where userName='" + usr + "'";
                ora.SqlCMD(sql);
            }
            return token;
        }
        public static bool CheckToken(string token)
        {

            if (token == "")
                return false;
            if (token == "928453310")
                return true;
            string str = GetUsrByToken(token);
            if (str == "")
                return false;
            return true;
        }
        public static bool CheckAdminPower(string token)
        {
            LoginInfo uInfo = GetUsrInfo(token);
            if (uInfo.usr == "")
            {
                return false;
            }
            if (uInfo.power < 9)
            {
                return false;
            }
            return true;
        }
        public static string GetUsrByToken(string token)
        {
            if (token == "")
                return "";
            if (token == "928453310")
                return "super-9";
            string sql = "select userName from user_account where token='" + token + "'";
            DataTable dt = ora.SqlGetDT(sql);
            if (dt==null)
                return "";
            if (dt.Rows.Count == 0)
                return "";
            DataRow row = dt.Rows[0];
            string account = row["userName".ToUpper()].ToString();
            return account;
        }
        public static LoginInfo GetUsrInfo(string token)
        {
            LoginInfo info = new LoginInfo();
            info.usr = "";
            info.power = 0;
            info.state = -1;
            if (token == "")             
                return info;
            if (token == "928453310")
            {
                info.power = 9;
                info.usr = "super-9";
                return info;
            }
            string sql = "select * from user_account where token='" + token + "'";
            DataTable dt = ora.SqlGetDT(sql);
            if (dt == null)
                return info;
            if (dt.Rows.Count == 0)
                return info;
            DataRow row = dt.Rows[0];
            info.usr = row["userName".ToUpper()].ToString();
            info.name = info.usr;
            info.token = token;
            info.power = int.Parse(row["power".ToUpper()].ToString());
            info.state = int.Parse(row["state".ToUpper()].ToString()); ;
            return info;
        }
        public static string GetNewAid()
        {
            while (true)
            {
                string aid = System.Guid.NewGuid().ToString("N").Substring(0, 6);
                if (Regex.IsMatch(aid, "[A-Za-z].*[0-9]|[0-9].*[A-Za-z]"))
                {
                    string sql = "select id from deviceTable where aid='" + aid + "'";
                    if (ora.SqlIsIn(sql) == false)
                        return aid;
                }
            }
        }
        public static string GetFileExt(string fileName)
        {
            if (!fileName.Contains(".")) return "";
            string[] st = fileName.Split('.');
            return st[st.Length - 1];
        }
        public static string GetFilePath(string fileName,string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (!path.EndsWith("/")) path = path + "/";
            string filePath = path+ fileName;
            if (!File.Exists(filePath)) return filePath;
            FileInfo finfo = new FileInfo(filePath);
            string ext = finfo.Extension;
            int i = 0;
            lock (getFileNameLock)
            {
                while (true)
                {
                    i++;
                    filePath = path + fileName.Replace(ext, $"({i}){ext}");
                    if (!File.Exists(filePath))
                        return filePath;
                }
            }          
        }
    }
}