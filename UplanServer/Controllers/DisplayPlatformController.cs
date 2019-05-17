using System;
using System.Collections.Generic;
using System.Linq;
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
        RedisHelper rd = Module.redisHelper;
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
            //LoginInfo uInfo = Module.GetUsrInfo(token);
            //if (uInfo.usr == "") return new NormalResponse(false, "token无效");
            DisplayPlatformWorker.WatchingWork();
            DPIndexInfo dPIndexInfo = rd.Get<DPIndexInfo>(Module.DisplayPlatform_DPIndexInfo);
            return new NormalResponse(true, "", "", dPIndexInfo);
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
            var provinceInfo = db.QoEVideoTable.Where(a => a.PROVINCE != null)
                .GroupBy(a => a.PROVINCE)
                .Select(a => new
                {
                    province = a.Key,
                    count = a.Count()
                }).OrderBy(a => a.count).Take(5).OrderBy(a=>Guid.NewGuid()).FirstOrDefault();
            if (provinceInfo != null)
            {
                string pName = provinceInfo.province;
                var info = db.QoERTable.Find(GetBaseRandId(pName));
                return new NormalResponse(true, $"By provinceName={pName}", "", info);
            }
            else
            {
                var info = db.QoERTable.Find(GetBaseRandId());
                return new NormalResponse(true, "", "", info);
            }
          
        }
        private int GetBaseRandId(string province="")
        {
            int[] ids = new int[] { 2891323, 4091046, 4034183, 4033756, 3832323, 4110266, 4111401, 3451431, 4051829, 2429976, 2482849, 4119787, 4101395, 4034909, 4067135, 4129652, 2848094, 3375659, 3953055, 4053606, 4083136, 4057700, 4099683, 4105219, 4125643, 3958440, 3973266, 4088333, 3970681 };
            string[] provinces = new string[] { "上海市", "云南省", "内蒙古自治区", "北京市", "台湾省", "吉林省", "四川省", "天津市", "宁夏回族自治区", "山东省", "山西省", "广东省", "广西壮族自治区", "新疆维吾尔自治区", "江苏省", "江西省", "河北省", "河南省", "浙江省", "海南省", "湖北省", "湖南省", "福建省", "西藏自治区", "贵州省", "辽宁省", "陕西省", "青海省", "黑龙江省" };
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
                if (level != 1 && level != 2) return new NormalResponse(false, "level只能为1或者2");
                day--;
                if (day != 0 && day != 6 && day != 29)
                {
                    return new NormalResponse(false, "day只能为1或者7或者30");
                }
                //if (level == 1)
                //{
                //    return DisplayPlatformWorker.GetDetailQuota(level, day);
                //}
               

                NormalResponse np = rd.Get<NormalResponse>(string.Format(Module.DisplayPlatform_DetailQuota, level, day));
                if (np != null)
                {
                    return np;
                }
                //if (level == 1)
                //{
                //    NormalResponse np = rd.Get<NormalResponse>(string.Format(Module.DisplayPlatform_DetailQuota, level, day));
                //    if (np != null)
                //    {
                //        return np;
                //    }
                //}
                //else
                //{
                //    return DisplayPlatformWorker.GetDetailQuota(level, day);
                //}

                return new NormalResponse(false, "缓存构建中，请稍候再试");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
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
    }
}
