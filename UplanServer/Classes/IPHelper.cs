using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace UplanServer
{
    /// <summary>
    /// IP帮助类
    /// </summary>
    public class IPHelper
    {
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static string GetIP(System.Web.HttpRequest req)
        {
            string ip = "";
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0]);
            if (string.IsNullOrEmpty(ip))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            if (string.IsNullOrEmpty(ip))
                ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            return ip;
        }
    }
}