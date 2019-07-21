using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UplanServer
{
    public class LoopWorker
    {
        private static List<Thread> threads;
        private static string TAG ="LoopWorker";
        public static bool isNeedStop = false;
        public static void Start()
        {
            isNeedStop = false;
            threads = new List<Thread>();
            threads.Add(new Thread(() => { QoEGroupWatcher.Watching(60); }) { Name= "QoE视频测试组监控" });//QoE视频测试组监控，60秒一次
            threads.Add(new Thread(() => { QoEMissionWatcher.Watching(60); }) { Name = "QoE自动任务监控" });//QoE视频测试组监控，60秒一次
            threads.Add(new Thread(() => { QoERMission.Watching(60); }) { Name = "QoER数据监控" });//QoER数据监控，60秒一次
            //threads.Add(new Thread(() => { DisplayPlatformWorker.Watching(6); }) { Name = "展示平台数据统计器" });
            //threads.Add(new Thread(() => { DisplayPlatformWorker.GetDetailQuotaChinaWork(); }) { Name = "分析页面国级别数据" });
            //threads.Add(new Thread(() => { DisplayPlatformWorker.GetDetailQuotaProvinceWork(); }) { Name = "分析页面省级别数据" });
            LogHelper.Log("===========UPLAN Server启动，开启循环工作线程===========", TAG);
            foreach(Thread t in threads)
            {
                string threadName = t.Name;
                try
                {                                   
                    t.Start();
                    LogHelper.Log($"   =>线程[{threadName}]已启动", TAG);
                }
                catch(Exception e)
                {
                    LogHelper.Log($"   =><error>线程[{threadName}]启动失败！！！"+e.ToString(), TAG);
                }               
            }
        }
     
        public static void Stop()
        {
           
            LogHelper.Log("===========UPLAN Server关闭，关闭所有线程===========", TAG);
            foreach (Thread t in threads)
            {
                try
                {
                    if (t != null) t.Abort();
                }
                catch (Exception e)
                {

                }
            }
            threads = null;
            isNeedStop = true;
        }
    }
}