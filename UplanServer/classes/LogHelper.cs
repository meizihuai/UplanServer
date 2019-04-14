using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace UplanServer
{
    public class LogHelper
    {
        public static string rootPath = HttpContext.Current.Server.MapPath("/Logs/");
        private static object lc = new object();
        public static void CheckDir()
        {
            DirectoryInfo dir = new DirectoryInfo(rootPath);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }
        public static void Log(string content, string tagName = "default")
        {
            lock (lc)
            {
                try
                {
                    CheckDir();
                    DateTime now = DateTime.Now;
                    string str = now.ToString("[HH:mm:ss] ") + "<" + tagName + "> " + content + "\r\n";
                    string filePath = rootPath + now.ToString("yyyy_MM_dd") + ".txt";
                    File.AppendAllText(filePath, str);
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}