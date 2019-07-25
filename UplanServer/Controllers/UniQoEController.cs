using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace UplanServer.Controllers
{
    /// <summary>
    /// UniQoE接口
    /// 接口版本:1.0.0.0
    /// 更改者：梅子怀
    /// 更改时间:2019-04-22 10:00:00
    /// </summary>
    public class UniQoEController : ApiController
    {
        private readonly OracleHelper ora = Module.ora;
        private readonly QoEDbContext db = new QoEDbContext();
        /// <summary>
        /// [APP]手机登录，获取AID
        /// </summary>
        /// <param name="phoneId">* 手机唯一识别号，android用imsi,iPhone用uuid</param>
        /// <param name="system">操作系统，Android 或 iOS</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse Login(string phoneId, string system = "Android")
        {
            if (string.IsNullOrEmpty(phoneId)) return new NormalResponse(false, "phoneId不可为空");
            system = system.ToLower();
            system = (system == "android" ? "Android" : "iOS");
            DeviceInfo d = null;
            d = db.DeviceTable.Where(a => (system == "Android" ? a.IMSI : a.UUID) == phoneId).FirstOrDefault();
            if (d != null)
            {
                d.System = system;
                db.SaveChanges();
                return new NormalResponse(true, "", "", d);
            }
            d = new DeviceInfo();
            d.AID = Module.GetNewAid();
            if (system == "Android")
            {
                d.IMSI = phoneId;
            }
            else
            {
                d.UUID = phoneId;
            }
            d.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            db.DeviceTable.Add(d);
            db.SaveChanges();
            return new NormalResponse(true, "", "", d);
        }
        /// <summary>
        /// [APP]iOS终端上传QoER数据
        /// </summary>
        /// <param name="qoer">QoER 结构体</param>
        /// <returns></returns>
        public Task<NormalResponse> UploadQoERDataForiOS(QoEReportIOSInfo qoer)
        {
            qoer.IP = IPHelper.GetIP(HttpContext.Current.Request);
            return Task.Run(() =>
            {
                if (qoer.AID == "") return new NormalResponse(false, "AID不可为空");
                if (qoer.BusinessType == null || qoer.BusinessType == "") qoer.BusinessType = "QOER";
                qoer.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                using (var p = MQProducter.Creat())
                {
                    if (p.Connect(Module.AppSetting.MQ_Queue_QoERForiOS2DB))
                    {
                        p.Send(qoer);
                    }
                }
                return new NormalResponse(true, "success");
            });

        }


        /// <summary>
        /// [APP]终端获取QoE自动监测任务
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetQoEMission(string aid)
        {
            try
            {
                if (string.IsNullOrEmpty(aid)) return new NormalResponse(false, "aid不可为空");
                string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var rt = db.QoEMissionTable.Where(a => a.AID == aid && a.ISCLOSED < 1 && a.ISCLOSED > -2 &&
                         a.STARTTIME.CompareTo(now) <= 0 && a.ENDTIME.CompareTo(now) > 0).FirstOrDefault();
                if (rt == null)
                {
                    return new NormalResponse(false, "没有任务");
                }
                rt.ISCLOSED = 0;
                rt.STATUS = "执行中";
                db.Update(rt, a => a.ISCLOSED, a => a.STATUS);
                rt.ParseMissionBody();
                rt.MISSIONBODY = "";
                return new NormalResponse(true, "", "", rt);
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        /// <summary>
        /// [APP]终端上传QoE黑点数据
        /// </summary>
        /// <param name="qoe"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse> UploadQoEBlackPoint(QoEBlackPoint qoe)
        {
            return QoEDataToDbHelper.UploadQoEBlackPoint(qoe);
        }
        /// <summary>
        /// [APP]终端上传QoER数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse> UploadQoER(PhoneInfo pi)
        {
            pi.IP = IPHelper.GetIP(HttpContext.Current.Request);
            return Task.Run(() =>
            {
                try
                {
                    if (pi.IsUploadDataTimely == null) pi.IsUploadDataTimely = 1;
                    if (pi.IsUploadDataTimely == -1)
                    {
                        //离线数据
                        if (string.IsNullOrEmpty(pi.DateTime))
                        {
                            DateTime piTime = DateTime.Now;
                            DateTime.TryParse(pi.DateTime, out piTime);
                            pi.DateTime = piTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            pi.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    else
                    {
                        pi.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    using (var q = MQProducter.Creat())
                    {
                        if (q.Connect(Module.AppSetting.MQ_Queue_QoER2DB))
                        {
                            q.Send(pi);
                        }
                    }
                    return new NormalResponse(true, "success");
                }
                catch (Exception e)
                {
                    return new NormalResponse(false, e.Message);
                }

            });
            // return QoEDataToDbHelper.UploadQoER(pi);
        }
        [HttpPost]
        public Task<NormalResponse> UploadQoEVideo(QoEVideoTable qoe)
        {
            qoe.IP = IPHelper.GetIP(HttpContext.Current.Request);
            return Task.Run(() =>
            {
                try
                {
                    //if (qoe.LIGHT_INTENSITY_list == null)
                    //{
                    //    File.WriteAllText(@"d:\errBefore.txt", "qoe.LIGHT_INTENSITY_list==null");
                    //}
                    //else
                    //{
                    //    File.WriteAllText(@"d:\errBefore.txt", $"qoe.LIGHT_INTENSITY_list.Count={qoe.LIGHT_INTENSITY_list.Count}");
                    //}
                    if (qoe.PI == null)
                    {
                        return new NormalResponse(false, "PI不可为空","",qoe);
                    }
                    qoe.AID = qoe.PI.AID;
                    if (qoe.ISUPLOADDATATIMELY == null) qoe.ISUPLOADDATATIMELY = 1;
                    if (qoe.ISUPLOADDATATIMELY == -1)
                    {
                        //离线数据
                        if (string.IsNullOrEmpty(qoe.DATETIME))
                        {
                            DateTime qoeTime = DateTime.Now;
                            DateTime.TryParse(qoe.DATETIME, out qoeTime);
                            qoe.DATETIME = qoeTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            qoe.DATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    else
                    {
                        qoe.DATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    using (var q = MQProducter.Creat())
                    {
                        if (q.Connect(Module.AppSetting.MQ_Queue_QoE2DB))
                        {
                            q.Send(qoe);
                        }
                    }
                    return new NormalResponse(true, "success");
                }
                catch (Exception e)
                {
                    return new NormalResponse(false, e.Message);
                }
            });
        }
        /// <summary>
        /// [APP]上传APP运行过程中生成的日志，多为重要错误日志
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse> UploadException(AppExceptionInfo info)
        {
            return Task.Run<NormalResponse>(async () =>
            {
                try
                {
                    if (info == null) return new NormalResponse(false, "AppExceptionInfo为空");
                    info.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    db.AppExceptionTable.Add(info);
                    int successCount = await db.SaveChangesAsync();
                    if (successCount > 0)
                    {
                        return new NormalResponse(true, "success");
                    }
                    else
                    {
                        return new NormalResponse(false, $"入库失败，成功数量为：{successCount}");
                    }
                }
                catch (Exception e)
                {
                    return new NormalResponse(false, e.ToString());
                }
            });
        }

    }
}
