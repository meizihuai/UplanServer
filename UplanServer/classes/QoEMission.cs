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
    public class QoEMission
    {
        private static OracleHelper ora = Module.ora;
        private static string TAG= "QoEMission";
        public static void Watching(int sleepSecond=60)
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
                LogHelper.Log("==QoEMission 监控==", TAG);
                int minMinute = 1;
                string minlastTime = DateTime.Now.AddMinutes(-1* minMinute).ToString("yyyy-MM-dd HH:mm:ss");
                string minlastAskVideoTime = DateTime.Now.AddMinutes(-5 * minMinute).ToString("yyyy-MM-dd HH:mm:ss");
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

                sql = $"update user_bp_table set isPlayingVideo=0 where isPlayingVideo=1 and LastAskVideoTime<='{minlastAskVideoTime}' ";
                ora.SqlCMD(sql);

            }
            catch (Exception e)
            {

            }
        }
    }
}