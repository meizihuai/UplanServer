using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace UplanServer
{
    public class QoEMissionWatcher
    {
        private static OracleHelper ora = Module.ora;
        private static string TAG = "QoEMissionWatcher";
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
                string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = $"update qoe_mission_table set status='已关闭',isClosed=1 where endTime<='{now}'";
                ora.SqlCMD(sql);
            }
            catch (Exception e)
            {

            }
        }
    }
}