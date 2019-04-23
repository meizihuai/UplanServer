using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace UplanServer
{
    public class HTTPHelper
    {
        public static string GetH(string uri, string msg)
        {
            int num = 0;
            while (true)
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri + msg);
                    req.Accept = "*/*";
                    req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    req.CookieContainer = new CookieContainer();
                    req.KeepAlive = true;
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.Method = "GET";
                    HttpWebResponse rp = (HttpWebResponse)req.GetResponse();
                    string str = new StreamReader(rp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    return str;
                }
                catch (Exception ex)
                {
                }
                num = num + 1;
                if (num == 4)
                    return "";
            }
        }

    }
}