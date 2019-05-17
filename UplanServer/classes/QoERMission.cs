using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Text;
using System.IO;

namespace UplanServer
{
    public class QoERMission
    {
        private static OracleHelper ora = Module.ora;
        private static string TAG = "QoERMission";
        public static void Watching(int sleepSecond = 60)
        {
            while (true)
            {
                try
                {
                    WatchingWork();
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(1000 * sleepSecond);
            }
        }
        private static void WatchingWork()
        {
            try
            {
               // LogHelper.Log("==QoERMission 监控==", TAG);
                string minlastTime = DateTime.Now.AddSeconds(-30).ToString("yyyy-MM-dd HH:mm:ss");
                DateTime nowTmp = DateTime.Now;
                string sql = "";
                sql = $"update deviceTable set isOnline=0 where isOnline=1 and LastDateTime<='{minlastTime}' ";
                ora.SqlCMD(sql);
            }
            catch (Exception e)
            {

            }
        }
    }
}