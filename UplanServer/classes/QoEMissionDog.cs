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
    public class QoEMissionDog
    {
        private static Thread mThread;
        private static int sleepSecond = 1*60;
        private static object appMissionTableLock = new object();
        private static OracleHelper ora = Module.ora;
        private static string TAG= "QoEMissionDog";
        public static void StartWatching()
        {
            StopWatching();
            try
            {
                LogHelper.Log("=====QoEMissionDog开启监控=====", TAG);
                mThread = new Thread(Watching);
                mThread.Start();
            }
            catch (Exception ex)
            {
            }
        }
        public static void StopWatching()
        {
            if (mThread != null)
            {
                try
                {
                    mThread.Abort();
                }
                catch (Exception ex)
                {
                }
            }
        }
        private static void Watching()
        {
            while (true)
            {
                try
                {
                    WatchingWork();
                }
                catch (Exception ex)
                {
                    string path = @"d:\QoEWatchingDogErr.txt";
                    File.WriteAllText(path, ex.ToString());
                }
                Thread.Sleep(1000 * sleepSecond);
            }
        }
        private static void WatchingWork()
        {
            try
            {
                LogHelper.Log("==QoEMissionDog 监控==", TAG);
                int minMinute = 1;
                string minlastTime = DateTime.Now.AddMinutes(-1* minMinute).ToString("yyyy-MM-dd HH:mm:ss");
                DateTime nowTmp = DateTime.Now;
                string sql = "update qoe_video_dt_group set lastdateTime=dateTime,lastDay=dateTime where lastdateTime='' or lastdateTime is null or lastDay='' or lastDay is null";
                ora.SqlCMD(sql);
                sql = "update qoe_video_dt_group_member set lastdateTime=dateTime,lastDay=dateTime where lastdateTime='' or lastdateTime is null or lastDay='' or lastDay is null";
                ora.SqlCMD(sql);
                string todayDay = DateTime.Now.ToString("yyyy-MM-dd");
                sql = "update qoe_video_dt_group set qoe_today_time=0,qoe_today_E_time=0  where lastday<>'" + todayDay + "'";
                ora.SqlCMD(sql);
                sql = "update qoe_video_dt_group_member set qoe_today_time=0,qoe_today_E_time=0,qoe_vmos_match_today=0 where lastday<>'" + todayDay + "'";
                ora.SqlCMD(sql);
                sql = "update user_bp_table set qoe_today_time=0,qoe_today_E_time=0  where lastday<>'" + todayDay + "'";
                ora.SqlCMD(sql);

                sql = "update qoe_video_dt_group set status='异常' where  iswatching=1 and lastdateTime<='" + minlastTime + "'";
                ora.SqlCMD(sql);
                sql = "update qoe_video_dt_group_member set status='异常' where lastdateTime<='" + minlastTime + "' and groupId in( select groupId from qoe_video_dt_group where iswatching=1)";
                ora.SqlCMD(sql);
            }
            catch (Exception e)
            {

            }
        }
    }
}