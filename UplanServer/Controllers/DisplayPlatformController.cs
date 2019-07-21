using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UplanServer.Controllers
{
    /// <summary>
    /// 展示平台接口
    /// 接口版本:1.0.0.0
    /// 更改者：梅子怀
    /// 更改时间:2019-05-09 10:00:00
    /// </summary>
    public class DisplayPlatformController : ApiController
    {
        IRedisHelper rd = Module.oracleCacheHelper;
        QoEDbContext db = new QoEDbContext();
        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetDPIndexData(string token = "")
        {
            //LoginInfo uInfo = Module.GetUsrInfo(token);
            //if (uInfo.usr == "") return new NormalResponse(false, "token无效");
            DPIndexInfo dPIndexInfo = rd.Get<DPIndexInfo>(Module.DisplayPlatform_DPIndexInfo);
            //    dPIndexInfo.provinceQoeRankingList.d1List.
            return new NormalResponse(true, "", "", dPIndexInfo);
        }
        /// <summary>
        /// 让后台立即刷新首页数据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse DoRefreshDPIndexData(string token = "")
        {
            return new NormalResponse(false, "接口已抛弃", "", "");
            //LoginInfo uInfo = Module.GetUsrInfo(token);
            //if (uInfo.usr == "") return new NormalResponse(false, "token无效");
            //DisplayPlatformWorker.WatchingWork();
            //DPIndexInfo dPIndexInfo = rd.Get<DPIndexInfo>(Module.DisplayPlatform_DPIndexInfo);
            //return new NormalResponse(true, "", "", dPIndexInfo);
        }
        /// <summary>
        /// 获取QoE模拟数据基础数据
        /// </summary>
        /// <returns></returns>
        public NormalResponse GetQoEMoniterBaseData(string province = "")
        {
            //.Where(a => a.Lon>0 && a.Lat>0 && a.Province!=null & a.City !=null )
            //  var info = db.QoEVideoTable.Where(a=>a.CITY!=null).OrderBy(a => Guid.NewGuid()).First();
            int minId = 1023137;
            int maxId = 4092435;
            //try
            //{
            //   var provinceInfo = db.QoEVideoTable.Where(a => a.PROVINCE != null )
            //  .GroupBy(a => a.PROVINCE)
            //  .Select(a => new
            //  {
            //      province = a.Key,
            //      count = a.Count()
            //  }).OrderBy(a => a.count).Take(5).OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            //    PhoneInfo info = null;
            //    string provinceName = provinceInfo.province;
            //    int kk = int.MaxValue;
            //    // a.PhoneModel.Split(' ')[0].ToLower() != "xiaomi" &&
            //    var checkId = db.QoERTable.Where(a => a.Province == provinceName && a.RSRP != kk && a.DateTime.CompareTo("2019-04-01 00:00:00") > 0 && a.EarFcn!=null  ).OrderBy(a => Guid.NewGuid()).Take(1);
            //    // info = checkId.FirstOrDefault();
            //    return new NormalResponse(true, "", "", checkId.FirstOrDefault());
            //}
            //catch (Exception e)
            //{
            //    return new NormalResponse(false, e.ToString());
            //}
            //int kk = int.MaxValue;
            //var rt1 = db.QoERTable.Where(a => a.DateTime.CompareTo("2019-05-01 00:00:00") > 0).OrderBy(a => a.id).Take(1).Select(a => new { a.id }).FirstOrDefault();
            //if (rt1 == null) return new NormalResponse(false, "rt1 == null");
            //minId = rt1.id;
            //var rt2 = db.QoERTable.Where(a => a.DateTime.CompareTo("2019-05-01 00:00:00") > 0).OrderByDescending(a => a.id).Take(1).Select(a => new { a.id }).FirstOrDefault();
            //if (rt2 == null) return new NormalResponse(false, "rt2 == null");
            //maxId = rt2.id;
            //while (true)
            //{
            //    Random r = new Random();
            //    int id = r.Next(minId, maxId);
            //    var info = db.QoERTable.Find(id);
            //    if (info != null)
            //    {
            //        if (info.EarFcn == null) continue;
            //        if (info.CI == int.MaxValue) continue;
            //        if (info.RSRP == int.MaxValue) continue;
            //        if (info.City == null) continue;
            //        return new NormalResponse(true, "", "", info);
            //    }

            //}
            //var provinceInfo = db.QoEVideoTable.Where(a => a.PROVINCE != null)
            //    .GroupBy(a => a.PROVINCE)
            //    .Select(a => new
            //    {
            //        province = a.Key,
            //        count = a.Count()
            //    }).OrderBy(a => a.count).Take(5).OrderBy(a=>Guid.NewGuid()).FirstOrDefault();
            //if (provinceInfo != null)
            //{
            //    string pName = provinceInfo.province;
            //    var info = db.QoERTable.Find(GetBaseRandId(pName));
            //    return new NormalResponse(true, $"By provinceName={pName}", "", info);
            //}
            //else
            //{
            //    var info = db.QoERTable.Find(GetBaseRandId());
            //    return new NormalResponse(true, "", "", info);
            //}

            // int[] ids = new int[] { 2155194, 3512927, 2543822, 4467577, 4492504,  2525160, 4147885, 3930474, 2253242, 2017748, 3513088, 2253503, 2257703 };

              int[] ids = new int[] { 2155194, 4318738, 4328966, 4328836, 4328829, 4316962, 4316954, 4330377, 4330369, 4329324, 4329317, 4328887, 4328868, 4320614, 4318737, 4318723, 4329760, 4329755, 4328827, 3512927, 3512909, 3512895, 3512929, 3512926, 3512958, 3512900, 3512896, 3512963, 3512916, 3512913, 3512974, 3512924, 3512910, 3512908, 2543822, 2543830, 4467577, 4492504, 4492612, 4492312, 4492225, 4492591, 4492308, 4492307, 4492213, 4492201, 4492564, 4492559, 4491960, 4492198, 4492191, 4492181, 4492548, 4491951, 4492523, 4492508, 2850606, 2850603, 2850599, 2850606, 2850603, 2850599, 2525160, 4327659, 4326645, 4326367, 4326363, 4328768, 4328760, 4328759, 4328710, 4328699, 4323568, 4323538, 4322907, 4322883, 4322853, 4322493, 4322490, 4322488, 4328700, 4147885, 4325864, 4323984, 4325605, 4325584, 4325822, 4325882, 4325862, 4325702, 4325693, 4325662, 4325818, 4325808, 4323951, 4325508, 4325492, 4325474, 4325802, 4325796, 3930474, 3930691, 3930688, 3930695, 3930468, 3930701, 3930698, 2253242, 3214877, 3214933, 3214930, 3214897, 3214880, 3216380, 3216378, 3216370, 3215820, 3215819, 3215798, 3215789, 3219197, 3219185, 3218561, 3218551, 3219448, 3219440, 2017748, 4527312, 4527300, 4526927, 4527539, 4527494, 4528077, 4528069, 4527242, 4526672, 4527463, 4526903, 4528004, 4527878, 4527460, 4526394, 4527167, 4526897, 4526369, 3513088, 3513381, 3513426, 3513368, 3513358, 3513348, 3513339, 3513334, 3513311, 3513829, 3513827, 3513651, 3513631, 3513244, 3513627, 3513619, 3513241, 3513600, 3513587, 2253503, 4135658, 4135600, 4135591, 4135610, 4135594, 4135664, 4135651, 4135647, 4135641, 4135634, 4135621, 3220910, 3220692, 3220345, 3220334, 3219804, 3219764, 3219813, 2257703, 2257715, 2254002, 2254329, 2254381, 2254382, 2254674, 2254675, 2254681, 2254682, 2254692, 2255123, 2255127, 2255131, 2255452, 2255494, 2255497, 2255449, 2257365, 4467577, 4467577, 4467577, 4467577, 4467577, 4467577, 4467577, 4467577, 4467577, 4467577, 4467577, 2543822, 2543830, 2543822,
                  2543830, 2543822, 2543830,4590427,4590440,4591045,4591066,4591400,4591408,4579233,4579639,4579656,4581693,4581694, 4582336,4582351};

            //2G Ids
            // int[] ids = new int[] { 2976653, 2976688, 2965623, 2976473, 2976477, 2976485, 2976504, 2976509, 2976596, 2976604, 2976632, 2976648, 2976673, 2976677, 2976694, 2976715, 2976569, 2976491, 2976708, 2976746, 2976828, 2976697, 2976488, 2976528, 2976535, 2976586, 2977880 };


            //int[] ids = new int[] {2336951 };

            Guid temp = Guid.NewGuid();

            int guidseed = BitConverter.ToInt32(temp.ToByteArray(), 0);
            Random r = new Random(guidseed);
            int id = r.Next(ids.Length - 1);
            var info = db.QoERTable.Find(ids[id]);
            return new NormalResponse(true, $"id={ids[id]}", "", info);



        }
        private int GetBaseRandId(string province = "")
        {
            int[] ids = new int[] { 2891323, 4091046, 4034183, 4033756, 3832323, 4110266, 4111401, 3451431, 4051829, 2429976, 2482849, 4119787, 4101395, 4034909, 4067135, 4129652, 2848094, 3375659, 3953055, 4053606, 4083136, 4057700, 4099683, 4105219, 4125643, 3958440, 3973266, 4088333, 3970681, 3477980, 4053298, 4048762 };
            string[] provinces = new string[] { "上海市", "云南省", "内蒙古自治区", "北京市", "台湾省", "吉林省", "四川省", "天津市", "宁夏回族自治区", "山东省", "山西省", "广东省", "广西壮族自治区", "新疆维吾尔自治区", "江苏省", "江西省", "河北省", "河南省", "浙江省", "海南省", "湖北省", "湖南省", "福建省", "西藏自治区", "贵州省", "辽宁省", "陕西省", "青海省", "黑龙江省", "甘肃省", "安徽省", "重庆市" };

            if (string.IsNullOrEmpty(province))
            {
                Guid temp = Guid.NewGuid();
                int guidseed = BitConverter.ToInt32(temp.ToByteArray(), 0);
                Random r = new Random(guidseed);
                int id = r.Next(ids.Length - 1);
                return ids[id];
            }
            else
            {
                for (int i = 0; i < provinces.Length; i++)
                {
                    if (provinces[i] == province)
                    {
                        return ids[i];
                    }
                }
                Guid temp = Guid.NewGuid();
                int guidseed = BitConverter.ToInt32(temp.ToByteArray(), 0);
                Random r = new Random(guidseed);
                int id = r.Next(ids.Length - 1);
                return ids[id];
            }
        }
        /// <summary>
        /// 获取QoE有数据的省份
        /// </summary>
        /// <returns></returns>
        public NormalResponse GetQoEProvince()
        {
            var list = db.QoEVideoTable.Where(a => a.PROVINCE != null)
                .GroupBy(a => a.PROVINCE)
                .Select(a => new
                {
                    a.Key
                }).ToList();
            List<string> tmplist = new List<string>();
            list.ForEach(a =>
            {
                tmplist.Add(a.Key);
            });
            return new NormalResponse(true, "", "", tmplist);
        }

        /// <summary>
        /// 获取分析平台详细指标数据
        /// </summary>
        /// <param name="level">数据级别，1为国，2为省</param>
        /// <param name="day">数据天数，如 1  7  30  180</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetDetailQuota(int level, int day)
        {
            
            try
            {
                if (level != 1 && level != 2 && level != 3 && level != 4) return new NormalResponse(false, "level只能为1 2 3 4");
                // day--;
                int[] days = new int[] { 7, 30, 180 };
                if (!days.Contains(day))
                {
                    return new NormalResponse(false, "day只能为1 7 30 180");
                }
                NormalResponse np = rd.Get<NormalResponse>(string.Format(Module.DisplayPlatform_DetailQuota, level, day));
                if (np != null)
                {
                    return np;
                }
                return new NormalResponse(false, "缓存构建中，请稍候再试");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString(),"", rd.Get(string.Format(Module.DisplayPlatform_DetailQuota, level, day)));
            }
        }
        /// <summary>
        /// 获取分析平台性能相关指标
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetPerformanceData()
        {
            try
            {
                return DisplayPlatformWorker.GetPerformanceData();
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        /// <summary>
        /// 获取分析平台(AQoE)网络相关指标TOP列表
        /// </summary>
        /// <param name="level">级别，1国，2省，3市，4区</param>
        /// <param name="day">时间粒度(天) 7 30 180</param>
        /// <param name="levelName">级别地区名称，如 level=3 (市级别)，levelName=广州市</param>
        /// <param name="quotaName">指标名称，rsrp,sinr,rtt,bufferInit,unStall</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetAQoEDetailQuotaTable(int level, int day, string levelName, string quotaName)
        {
            try
            {
                if (level != 1 && level != 2 && level != 3 && level != 4) return new NormalResponse(false, "level设置非法");
                if (day != 7 && day != 30 && day != 180) return new NormalResponse(false, "day设置非法");
                if (quotaName != "rsrp" && quotaName != "sinr" && quotaName != "rtt" && quotaName != "bufferInit" && quotaName != "unStall")
                    return new NormalResponse(false, "quotaName设置非法");
                day--;
                string ago = DateTime.Now.AddDays(-day).ToString("yyyy-MM-dd 00:00:00");
                int maxInt = int.MaxValue;
                Stopwatch sp = new Stopwatch();
                sp.Start();
                //Expression<Func<QoEVideoTable, bool>> globalExp = a => a.DATETIME.CompareTo(ago) >= 0 && a.PROVINCE != null && a.CITY != null &&
                //                                                  a.SIGNAL_STRENGTH != maxInt && a.SINR != maxInt && a.NETWORK_FORMAT != null &&
                //                                                  a.NET_TYPE != null && a.NET_TYPE != "3G" && a.NET_TYPE != "unknown" && a.NET_TYPE != "null" &&
                //                                                  a.ECI > 100000;
                Expression<Func<QoEVideoTable, bool>> globalExp = a => a.DATETIME.CompareTo(ago) >= 0  &&
                                                                a.SIGNAL_STRENGTH != maxInt && a.SINR != maxInt && a.SIGNAL_STRENGTH<0 &&
                                                                a.ECI > 100000 && a.PING_AVG_RTT>0;
                var rt = db.QoEVideoTable.Where(globalExp).AsQueryable();
                if (level == 2) rt = rt.Where(a => a.PROVINCE == levelName);
                if (level == 3) rt = rt.Where(a => a.CITY == levelName);
                if (level == 4) rt = rt.Where(a => a.DISTRICT == levelName);
                //var queryTmp = rt.AsQueryable();
                if (quotaName == "rsrp") rt = rt.OrderBy(a => a.SIGNAL_STRENGTH);
                if (quotaName == "sinr") rt = rt.OrderBy(a => a.SINR);
                if (quotaName == "rtt") rt = rt.OrderByDescending(a => a.PING_AVG_RTT);
                if (quotaName == "bufferInit") rt = rt.OrderByDescending(a => a.VIDEO_BUFFER_INIT_TIME);
                if (quotaName == "unStall") rt = rt.OrderByDescending(a => a.VIDEO_STALL_NUM);
                rt = rt.Take(50);
                //queryTmp = queryTmp.Take(50);
                //if (quotaName == "unStall")
                //{
                //    int count = queryTmp.Count();
                //    if (count == 0)
                //    {
                //        rt = rt.OrderByDescending(a => a.VIDEO_STALL_NUM);
                //        rt = rt.Take(50);
                //    }
                //}
                //else
                //{
                //    rt = queryTmp;
                //}
                var pointRt = rt.Where(a => a.GDLON > 0 && a.GDLAT > 0).GroupBy(a => new { a.GDLON, a.GDLAT }).Select(a => new
                {
                    gdlon = a.Key.GDLON,
                    gdlat = a.Key.GDLAT,
                    rsrp = a.Average(b => b.SIGNAL_STRENGTH) < 0 ? a.Average(b => b.SIGNAL_STRENGTH) : -85,
                    sinr = a.Average(b => b.SINR),
                    rtt = a.Average(b => b.PING_AVG_RTT),
                    bufferInit = a.Average(b => b.VIDEO_BUFFER_INIT_TIME),
                    unStall = a.Count(b => b.VIDEO_STALL_NUM > 0)
                });
                var siteRt = rt.GroupBy(a => a.ECI).Select(a => new
                {
                    ECI = a.Key,
                    rsrp = a.Average(b => b.SIGNAL_STRENGTH),
                    sinr = a.Average(b => b.SINR),
                    rtt = a.Average(b => b.PING_AVG_RTT),
                    bufferInit = a.Average(b => b.VIDEO_BUFFER_INIT_TIME),
                    unStall = a.Count(b => b.VIDEO_STALL_NUM > 0)
                });
              

                //if (quotaName == "rsrp")
                //{
                //    pointRt = pointRt.OrderBy(a => a.rsrp);
                //    siteRt = siteRt.OrderBy(a => a.rsrp);
                //}
                //if (quotaName == "sinr")
                //{
                //    pointRt = pointRt.OrderBy(a => a.sinr);
                //    siteRt = siteRt.OrderBy(a => a.sinr);
                //}
                //if (quotaName == "rtt")
                //{
                //    pointRt = pointRt.OrderByDescending(a => a.rtt);
                //    siteRt = siteRt.OrderByDescending(a => a.rtt);
                //}
                //if (quotaName == "bufferInit")
                //{
                //    pointRt = pointRt.OrderByDescending(a => a.bufferInit);
                //    siteRt = siteRt.OrderByDescending(a => a.bufferInit);
                //}
                //if (quotaName == "unStall")
                //{
                //    pointRt = pointRt.OrderByDescending(a => a.unStall);
                //    siteRt = siteRt.OrderByDescending(a => a.unStall);
                //}

                sp.Stop();
                string msg = $"step1耗时:{sp.Elapsed.ToString()}";
                sp.Start();
                //var siteTable = siteRt.Take(5).ToList();
                //var siteNewTable = siteTable.Join(db.SysTJCellTable, a => a.ECI, b => b.ECI, (a, b) => new
                //{
                //    a.ECI,
                //    a.rsrp,
                //    a.sinr,
                //    a.rtt,
                //    a.bufferInit,
                //    a.unStall,
                //    b.Name
                //}).ToList();
                Dictionary<string, object> dik = new Dictionary<string, object>();
                dik.Add("pointTable", pointRt.Take(5).ToList());
                dik.Add("siteTable", siteRt.Take(5).ToList());
                sp.Stop();
                msg = msg + $"  step2耗时:{sp.Elapsed.ToString()}";
                return new NormalResponse(true, msg, "", dik);
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        //public NormalResponse GetAQoEDetailQuotaTable(int level,int day,string levelName,string quotaName)
        //{
        //    try
        //    {
        //        if (level != 1 && level != 2 && level != 3 && level != 4) return new NormalResponse(false, "level设置非法");
        //        if (day != 7 && day != 30 && day !=180) return new NormalResponse(false, "day设置非法");
        //        if(quotaName!="rsrp" && quotaName != "sinr" && quotaName != "rtt" && quotaName != "bufferInit" && quotaName != "unStall" )
        //            return new NormalResponse(false, "quotaName设置非法");
        //        day--;
        //        string ago = DateTime.Now.AddDays(-day).ToString("yyyy-MM-dd 00:00:00");
        //        int maxInt = int.MaxValue;
        //        Stopwatch sp = new Stopwatch();
        //        sp.Start();
        //        Expression<Func<QoEVideoTable, bool>> globalExp = a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null &&
        //                                                          a.SIGNAL_STRENGTH != maxInt && a.SINR != maxInt && a.NETWORK_FORMAT != null &&
        //                                                          a.NET_TYPE != null && a.NET_TYPE != "3G" && a.NET_TYPE != "unknown" && a.NET_TYPE != "null";
        //        var rt = db.QoEVideoTable.Where(globalExp).Where(a => a.DATETIME.CompareTo(ago) >= 0);
        //        if (level == 2) rt = rt.Where(a => a.PROVINCE == levelName);
        //        if (level == 3) rt = rt.Where(a => a.CITY == levelName);
        //        if (level == 4) rt = rt.Where(a => a.DISTRICT == levelName);
        //        var pointRt= rt.Where(a=>a.GDLON>0 && a.GDLAT>0).GroupBy(a=>new { a.GDLON,a.GDLAT}).Select(a => new
        //        {
        //            gdlon=a.Key.GDLON,
        //            gdlat=a.Key.GDLAT,
        //            rsrp = a.Average(b => b.SIGNAL_STRENGTH)<0? a.Average(b => b.SIGNAL_STRENGTH):-85,
        //            sinr = a.Average(b => b.SINR),
        //            rtt = a.Average(b => b.PING_AVG_RTT),
        //            bufferInit = a.Average(b => b.VIDEO_BUFFER_INIT_TIME),
        //            unStall = a.Count(b => b.VIDEO_STALL_NUM > 0)
        //        });

        //        var siteRt = rt.GroupBy(a => a.ECI).Select(a => new
        //        {
        //            ECI = a.Key,
        //            rsrp=a.Average(b=>b.SIGNAL_STRENGTH),
        //            sinr=a.Average(b=>b.SINR),
        //            rtt=a.Average(b=>b.PING_AVG_RTT),
        //            bufferInit=a.Average(b=>b.VIDEO_BUFFER_INIT_TIME),
        //            unStall=a.Count(b=>b.VIDEO_STALL_NUM==0)
        //        });
        //        if (quotaName == "rsrp")
        //        {
        //            pointRt = pointRt.OrderBy(a => a.rsrp);
        //            siteRt = siteRt.OrderBy(a => a.rsrp);
        //        }
        //        if (quotaName == "sinr")
        //        {
        //            pointRt = pointRt.OrderBy(a => a.sinr);
        //            siteRt = siteRt.OrderBy(a => a.sinr);
        //        }
        //        if (quotaName == "rtt")
        //        {
        //            pointRt = pointRt.OrderByDescending(a => a.rtt);
        //            siteRt = siteRt.OrderByDescending(a => a.rtt);
        //        }
        //        if (quotaName == "bufferInit")
        //        {
        //            pointRt = pointRt.OrderByDescending(a => a.bufferInit);
        //            siteRt = siteRt.OrderByDescending(a => a.bufferInit);
        //        }
        //        if (quotaName == "unStall")
        //        {
        //            pointRt = pointRt.OrderByDescending(a => a.unStall);
        //            siteRt = siteRt.OrderByDescending(a => a.unStall);
        //        }
        //        sp.Stop();
        //        string msg = $"step1耗时:{sp.Elapsed.ToString()}";
        //        sp.Start();
        //        Dictionary<string, object> dik = new Dictionary<string, object>();
        //        dik.Add("pointTable", pointRt.Take(5).ToList());
        //        dik.Add("siteTable", siteRt.Take(5).ToList());
        //        sp.Stop();
        //        msg = msg + $"  step2耗时:{sp.Elapsed.ToString()}";
        //        return new NormalResponse(true, msg, "", dik);
        //    }
        //    catch (Exception e)
        //    {
        //        return new NormalResponse(false, e.ToString());
        //    }
        //}

        /// <summary>
        /// [5G自动驾驶模拟器使用]获取QoER基数
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetAutoBusQoERData(string imei, string startTime, string endTime)
        {
            return new NormalResponse(false, "接口开发中");
        }
        /// <summary>
        /// [5G自动驾驶模拟器使用]添加自动驾驶汽车模拟器
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public NormalResponse AddAutoBus(SysAutoBusInfo auto, string token)
        {
            try
            {
                if (!Module.CheckAdminPower(token)) return new NormalResponse(false, "您的权限不足，无法调用此接口");
                if (auto == null) return new NormalResponse(false, "AutoBus格式非法");
                var rt = db.SysAutoBusTable.Where(a => a.BusName == auto.BusName).FirstOrDefault();
                if (rt != null) return new NormalResponse(false, "该BusName已存在");
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                auto.DateTime = time;
                auto.LastDateTime = time;
                var qoer = db.QoERTable.Where(a => a.IMEI == auto.Imei_cm && a.DateTime.CompareTo(auto.StartTime) >= 0 && a.DateTime.CompareTo(auto.EndTime) <= 0);
                var startQoER = qoer.OrderBy(a => a.ID).Select(a => new { a.ID }).FirstOrDefault();
                if (startQoER != null) auto.StartQoERId = startQoER.ID;
                var endQoER = qoer.OrderByDescending(a => a.ID).Select(a => new { a.ID }).FirstOrDefault();
                if (endQoER != null) auto.EndQoERId = endQoER.ID;
                db.SysAutoBusTable.Add(auto);
                db.SaveChanges();
                return new NormalResponse(true, "添加成功");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        /// <summary>
        /// [5G自动驾驶模拟器使用]查询自动驾驶模拟器列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetAutoBus(string token)
        {
            try
            {
                if (!Module.CheckAdminPower(token)) return new NormalResponse(false, "您的权限不足，无法调用此接口");
                var list = db.SysAutoBusTable.ToList();
                return new NormalResponse(true, "", "", list);
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        /// <summary>
        /// [5G自动驾驶模拟器使用]查询自动驾驶模拟器对应QoER数据列表
        /// </summary>
        /// <param name="busId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetAutoBusQoERBaseData(int busId, string token)
        {
            try
            {
                if (!Module.CheckAdminPower(token)) return new NormalResponse(false, "您的权限不足，无法调用此接口");
                var bus = db.SysAutoBusTable.Find(busId);
                if (bus == null) return new NormalResponse(false, "该busId不存在");
                string imei = bus.Imei_cm;
                int startQoERId = bus.StartQoERId;
                int endQoERId = bus.EndQoERId;
                var list = db.QoERTable.Where(a => a.IMEI == imei && a.RSRP < 0 && a.ID >= startQoERId && a.ID <= endQoERId).Select(a => new
                {
                    a.ID,
                    a.DateTime,
                    a.RSRP,
                    a.SINR,
                    a.GDlon,
                    a.GDlat,
                    a.GpsSpeed,
                    a.Ping_Avg_Rtt,
                    a.VMOS,
                    a.NetType
                }).OrderBy(a => a.ID).ToList();
                return new NormalResponse(true, "", "", list);
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        /// <summary>
        /// [5G自动驾驶模拟器使用]更新汽车信息
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public NormalResponse UpdateAutoBusInfo(SysAutoBusInfo auto, string token)
        {
            try
            {
                DateTime now = DateTime.Now;
                int id = auto.ID;
                var rt = db.SysAutoBusTable.Find(id);
                if (rt == null) return new NormalResponse(false, "busId无效");

                if (rt.CurrentQoERId > auto.CurrentQoERId)
                {
                    //新一轮循环
                    rt.RunTimes++;
                    rt.TotalCount = 1;
                    rt.BrakeCount = 0;
                    rt.ShakeCount = 0;
                    rt.ASpeed = 0;
                }
                else
                {
                    if (!string.IsNullOrEmpty(rt.LastDateTime))
                    {
                        DateTime oldTime = now;
                        DateTime.TryParse(rt.LastDateTime, out oldTime);
                        int second = (int)Math.Ceiling((now - oldTime).TotalSeconds);
                        if (second < 1) second = 1;
                        double aSpeed = (rt.Speed - auto.Speed) / (3.6 * second); // km/h的 speed 要转换为  m/s
                        if (aSpeed < -3.5) rt.BrakeCount++; //刹车
                        if (Math.Abs(aSpeed) > 1.5) rt.ShakeCount++; //抖动
                        rt.ASpeed = aSpeed;
                    }
                    rt.TotalCount++;
                }

                rt.LastDateTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                rt.RSRP = auto.RSRP;
                rt.SINR = auto.SINR;
                rt.GDLon = auto.GDLon;
                rt.GDLat = auto.GDLat;
                rt.RTT = auto.RTT;
                if (rt.RTT == 0)
                {
                    rt.RTT = new Random().Next(20, 100);
                }
                rt.Speed = auto.Speed;
                rt.CurrentQoERId = auto.CurrentQoERId;
                rt.VMOS = auto.VMOS;
                rt.Network = auto.Network;
                if (string.IsNullOrEmpty(rt.Network) || rt.Network.ToLower() == "null")
                {
                    rt.Network = "4G";
                }
                int minInt = int.MinValue;
                int maxInt = int.MaxValue;
                if (rt.RSRP >= 0 || rt.RSRP == maxInt || rt.RSRP == minInt) rt.RSRP = -80;
                if (rt.SINR == maxInt || rt.SINR == minInt) rt.SINR = 10;



                db.Update(rt, a => a.LastDateTime,
                                a => a.RSRP,
                                a => a.SINR,
                                a => a.GDLon,
                                a => a.GDLat,
                                a => a.RTT,
                                a => a.Speed,
                                a => a.CurrentQoERId,
                                a => a.VMOS,
                                a => a.Network,
                                a => a.RunTimes,
                               a => a.TotalCount,
                               a => a.BrakeCount,
                               a => a.ShakeCount,
                               a => a.ASpeed
                                );
                return new NormalResponse(true, "success");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        /// <summary>
        /// 获取5G指标数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse Get5GQuotaData()
        {
            try
            {
                var rt = db.SysAutoBusTable.AsQueryable();
                AutoBusQutoaInfo bus = new AutoBusQutoaInfo();
                bus.rtt = Math.Round(rt.Average(a => a.RTT), 2);
                bus.rsrp = Math.Floor(rt.Average(a => a.RSRP));
                bus.sinr = Math.Floor(rt.Average(a => a.SINR));
                var rtList = rt.OrderBy(a => a.ID).ToList();
                rtList.ForEach(a =>
                {
                    bus.autoBusList.Add(a);
                });
                foreach (var itm in bus.autoBusList)
                {
                    //itm.AvgVMOS = DisplayPlatformWorker.DoubleNull2Double(db.QoERTable.Where(a => a.ID >= itm.StartQoERId && a.ID <= itm.CurrentQoERId).Average(a => a.VMOS),1);
                    double a = Math.Abs(itm.ASpeed);
                    if (a < 0.4)
                    {
                        itm.AvgVMOS = Math.Round(DisplayPlatformWorker.GetRandDouble(4.5, 4.7),2);
                    }
                    if (a >= 0.4 && a < 0.6)
                    {
                        itm.AvgVMOS = Math.Round(DisplayPlatformWorker.GetRandDouble(3.7, 3.9),2);
                    }
                    if (a >= 0.6)
                    {
                        itm.AvgVMOS = Math.Round(DisplayPlatformWorker.GetRandDouble(3.2, 3.4),2);
                    }

                }
                //itm.AvgVMOS = DisplayPlatformWorker.GetRandDouble(3.0, 4.5);

                var networklist = rt.GroupBy(a => a.Network).Select(a => new
                {
                    network = a.Key,
                    count = a.Count()
                }).ToList();
                networklist.ForEach(a =>
                {
                    double value = 100 * (double)a.count / (double)rtList.Count;
                    PieInfo p = new PieInfo(a.network, value, a.count);
                    bus.networkList.Add(p);
                });
                return new NormalResponse(true, "", "", bus);
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }

        /// <summary>
        /// 获取5G自动驾驶汽车详细历史信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse Get5GAutoBusDetail(int id)
        {
            try
            {
                var bus = db.SysAutoBusTable.Find(id);
                if (bus == null) return new NormalResponse(false, "没有该车辆");
                int startId = bus.StartQoERId;
                int currentId = bus.CurrentQoERId;
                int midId = bus.MidQoERId;
                if (currentId > midId)
                {
                    startId = midId;
                }
                string imei = bus.Imei_cm;
                var list = db.QoERTable.Where(a => a.IMEI == imei && a.RSRP < 0 && a.ID >= startId && a.ID <= currentId).Select(a => new
                {
                    a.ID,
                    a.DateTime,
                    a.RSRP,
                    a.SINR,
                    a.GDlon,
                    a.GDlat,
                    a.GpsSpeed,
                    a.Ping_Avg_Rtt,
                    a.VMOS,
                    a.NetType
                }).OrderBy(a => a.ID).ToList();
                return new NormalResponse(true, "", "", list);
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }


    }
}
