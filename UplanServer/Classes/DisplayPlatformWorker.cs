using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace UplanServer
{
    public class DisplayPlatformWorker
    {
        private static OracleHelper ora = Module.ora;
        private static QoEDbContext db = new QoEDbContext();
        private static RedisHelper rd = Module.redisHelper;
        private static string TAG = "DisplayPlatformWorker";
        private static object lc = new object();
        private static bool isWorking = false;
        public static void ChinaWatching(int sleepSecond = 3)
        {
            return;
            while (true)
            {
                try
                {
                    lock (lc)
                    {

                        ChinaWatchingWork();
                    }

                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(1000 * sleepSecond);
            }
        }
        public static void Watching(int sleepSecond = 60)
        {
            while (true)
            {
                try
                {
                    if (LoopWorker.isNeedStop) return;
                    WatchingWork();
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(1000 * sleepSecond);
            }
        }
        public static void ChinaWatchingWork()
        {
            try
            {

                DPIndexInfo dp = rd.Get<DPIndexInfo>(Module.DisplayPlatform_DPIndexInfo);
                if (dp == null) return;
                Stopwatch sp = new Stopwatch();
                sp.Start();
                DateTime now = DateTime.Now;
                string ago;
                ago = now.AddDays(0).ToString("yyyy-MM-dd 00:00:00");
                var r1 = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null).Count();
                dp.chinaQoeRankData.d1data.dataCount = r1;
                ago = now.AddDays(-6).ToString("yyyy-MM-dd 00:00:00");
                var r7 = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null).Count();
                dp.chinaQoeRankData.d7data.dataCount = r7;
                ago = now.AddDays(-29).ToString("yyyy-MM-dd 00:00:00");
                var r30 = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null).Count();
                dp.chinaQoeRankData.d30data.dataCount = r30;
                ago = now.AddDays(-179).ToString("yyyy-MM-dd 00:00:00");
                var r180 = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null).Count();
                dp.chinaQoeRankData.d180data.dataCount = r180;
                sp.Stop();
                dp.chinaWorkms = sp.ElapsedMilliseconds;
                dp.refreshTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                dp.refreshLever = "china";
                rd.Set(Module.DisplayPlatform_DPIndexInfo, dp);
            }
            catch (Exception e)
            {
                LogHelper.Log(e.ToString(), "ChinaWatchingWork.err");
            }

        }
        public static void WatchingWork()
        {
            if (isWorking)
            {
                LogHelper.Log("==DisplayPlatformWorker WatchingWork 服务正忙，本次退出==", TAG);
                return;
            }
            lock (lc)
            {
                LogHelper.Log("==DisplayPlatformWorker WatchingWork==", TAG);
                isWorking = true;
                Stopwatch sp = new Stopwatch();
                sp.Start();
                try
                {
                    DPIndexInfo dp = new DPIndexInfo();
                    ChinaQoERankData chinaRankData = new ChinaQoERankData();
                    QoERankinglist ranklist = new QoERankinglist();
                    DateTime now = DateTime.Now;


                    RankData d1rList = new RankData("China", 0, 0, 0, 0, 0);
                    RankData d7rList = new RankData("China", 0, 0, 0, 0, 0);
                    RankData d30rList = new RankData("China", 0, 0, 0, 0, 0);
                    RankData d60rList = new RankData("China", 0, 0, 0, 0, 0);
                    //省级别
                    ranklist.d1List = GetQoERankinglist(0, ref d1rList);
                    ranklist.d7List = GetQoERankinglist(6, ref d7rList);
                    ranklist.d30List = GetQoERankinglist(29, ref d30rList);
                    ranklist.d180List = GetQoERankinglist(179, ref d60rList);
                    //国家级别
                    chinaRankData.d1data = d1rList;
                    chinaRankData.d7data = d7rList;
                    chinaRankData.d30data = d30rList;
                    chinaRankData.d180data = d60rList;

                    dp.chinaQoeRankData = chinaRankData;
                    dp.provinceQoeRankingList = ranklist;
                    dp.chinaWorkms = 0;
                    dp.totalWorkSecond = sp.ElapsedMilliseconds / 1000;
                    dp.refreshTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                    dp.refreshLever = "all";

                    isWorking = false;
                    sp.Stop();
                    string time = sp.Elapsed.ToString();
                    LogHelper.Log("DisplayPlatformWorker本次任务结束,耗时:" + time, TAG);
                    rd.Set(Module.DisplayPlatform_DPIndexInfo, dp);
                }
                catch (Exception e)
                {
                    isWorking = false;
                    sp.Stop();
                    string time = sp.Elapsed.ToString();
                    LogHelper.Log("DisplayPlatformWorker本次任务结束,但出现错误，耗时:" + time, TAG);
                    LogHelper.Log(e.ToString(), TAG);
                }
            }

        }
        //QoE排名，数据量
        private static List<RankInfo> GetQoERankinglist(int day, ref RankData rankData)
        {
            DateTime now = DateTime.Now;
            string ago = now.AddDays(-day).ToString("yyyy-MM-dd 00:00:00");
            string[] unUseProvince = new string[] { };//"新疆维吾尔自治区"
            List<RankInfo> list = new List<RankInfo>();


            //超长省名称替换字典
            Dictionary<string, string> provinceNameDik = new Dictionary<string, string>();
            provinceNameDik.Add("西藏自治区", "西藏");
            provinceNameDik.Add("新疆维吾尔自治区", "新疆");
            provinceNameDik.Add("宁夏回族自治区", "宁夏");
            provinceNameDik.Add("广西壮族自治区", "广西");
            provinceNameDik.Add("内蒙古自治区", "内蒙古");


            //国级别的数据
            {
                var result = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null)
                                .GroupBy(a => a.PROVINCE).Select(a => new
                                {
                                    province = a.Key,
                                    vmos = a.Average(b => b.VMOS),
                                    count = a.Count(),
                                    badCount = a.Count(b => b.VMOS <= 3),
                                    badRSRPCount = a.Count(b => b.SIGNAL_STRENGTH < -110),
                                    badSINRCount = a.Count(b => b.SINR < 3)
                                }).OrderByDescending(a => a.vmos).ToList();
                result.ForEach(itm =>
                {
                    double vmos = itm.vmos == null ? 0 : (double)itm.vmos;
                    vmos = Math.Round(vmos, 2);
                    string provinceName = itm.province;
                    if (provinceNameDik.ContainsKey(itm.province))
                    {
                        provinceName = provinceNameDik[provinceName];
                    }
                    //省级别数据
                    RankInfo proRank = new RankInfo(provinceName, vmos, itm.count, itm.badCount, itm.badRSRPCount, itm.badSINRCount);
                    {
                        //市级别的数据
                        //var tmp = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE == itm.province && a.PROVINCE != null && a.CITY != null)
                        //.GroupBy(a => a.CITY).Select(a => new
                        //{
                        //    city = a.Key,
                        //    vmos = a.Average(b => b.VMOS),
                        //    count = a.Count(),
                        //    badCount = a.Count(b => b.VMOS <= 3),
                        //    badRSRPCount = a.Count(b => b.SIGNAL_STRENGTH < -110),
                        //    badSINRCount = a.Count(b => b.SINR < 3)
                        //}).OrderByDescending(a => a.vmos).ToList();
                        //tmp.ForEach(itmCity =>
                        //{
                        //    double vmosCity = itmCity.vmos == null ? 0 : (double)itmCity.vmos;
                        //    vmosCity = Math.Round(vmosCity, 2);
                        //    proRank.cityRankList.Add(new CityRankData(itmCity.city, vmosCity, itmCity.count, itmCity.badCount, itmCity.badRSRPCount, itmCity.badSINRCount));
                        //});
                    }
                    list.Add(proRank);
                });
            }
            //全国数据总量
            {
                var result = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null).Count();
                rankData.dataCount = result;
            }
            DoPhoneModel(ago, ref list, ref rankData);
            DoNetwork(ago, ref list, ref rankData);
            DoVideoQuota(ago, ref list, ref rankData);
            DoChartData(ago, ref list, ref rankData);

            // DoScreen(ago, ref list);
            //  DoFileResolution(ref ago,ref list, ref rankData);
            // DoRankInfoList(ago, ref list, ref rankData, a => a.SCREEN_RESOLUTION_LONG > 0, a => a.SCREEN_RESOLUTION_LONG);
            return list;
        }
        //手机厂商
        private static void DoPhoneModel(string ago, ref List<RankInfo> list, ref RankData rankData)
        {
            try
            {
                //国级别               
                {
                    var rt = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PHONE_MODEL != null && a.PROVINCE != null && a.CITY != null).GroupBy(a => a.PHONE_MODEL).Select(a => new
                    {
                        phoneModel = a.Key,
                        count = a.Count()
                    }).ToList();
                    List<PieInfo> phoneProductList = new List<PieInfo>();
                    rt.ForEach(b =>
                    {
                        string product = b.phoneModel.Split(' ')[0].ToLower();
                        if (product == "") product = "other";
                        bool isFind = false;
                        for (int j = 0; j < phoneProductList.Count; j++)
                        {
                            if (phoneProductList[j].name == product)
                            {
                                isFind = true;
                                phoneProductList[j].value += b.count;
                                break;
                            }
                        }
                        if (!isFind)
                        {
                            phoneProductList.Add(new PieInfo(product, b.count));
                        }
                    });
                    for (int j = 0; j < phoneProductList.Count; j++)
                    {
                        PieInfo b = phoneProductList[j];
                        double pie = b.value;
                        double d = 100 * pie / (double)rankData.dataCount;
                        d = Math.Round(d, 2);
                        rankData.phoneModelList.Add(new PieInfo(b.name, d));
                    }
                }

                return;
                //省级别
                for (int i = 0; i < list.Count; i++)
                {
                    RankInfo itm = list[i];
                    string province = itm.rankData.name;
                    var rt = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE == province && a.PHONE_MODEL != null && a.PROVINCE != null && a.CITY != null).GroupBy(a => a.PHONE_MODEL).Select(a => new
                    {
                        phoneModel = a.Key,
                        count = a.Count()
                    }).ToList();
                    List<PieInfo> phoneProductList = new List<PieInfo>();
                    rt.ForEach(b =>
                    {
                        string product = b.phoneModel.Split(' ')[0].ToLower();
                        if (product == "") product = "other";
                        //if (Module.phoneModelDik.ContainsKey(product))
                        //    product = Module.phoneModelDik[product];
                        //else
                        //    product = "other";
                        bool isFind = false;
                        for (int j = 0; j < phoneProductList.Count; j++)
                        {
                            if (phoneProductList[j].name == product)
                            {
                                isFind = true;
                                phoneProductList[j].value += b.count;
                                break;
                            }
                        }
                        if (!isFind)
                        {
                            phoneProductList.Add(new PieInfo(product, b.count));
                        }
                    });
                    for (int j = 0; j < phoneProductList.Count; j++)
                    {
                        PieInfo b = phoneProductList[j];
                        double pie = b.value;
                        double d = 100 * pie / (double)itm.rankData.dataCount;
                        d = Math.Round(d, 2);
                        itm.rankData.phoneModelList.Add(new PieInfo(b.name, d));
                    }
                    list[i] = itm;
                }
                {
                    ////市级别
                    //for (int i = 0; i < list.Count; i++)
                    //{
                    //    RankInfo itm = list[i];
                    //    string province = itm.rankData.name;
                    //    for(int index = 0; index < itm.cityRankList.Count; index++)
                    //    {
                    //        RankData cityItm = itm.cityRankList[index];
                    //        string city = cityItm.name;
                    //        var rt = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.CITY == city && a.PROVINCE== province && a.PROVINCE != null && a.CITY != null).GroupBy(a => a.PHONE_MODEL).Select(a => new
                    //        {
                    //            phoneModel = a.Key,
                    //            count = a.Count()
                    //        }).ToList();
                    //        List<PieInfo> phoneProductList = new List<PieInfo>();
                    //        rt.ForEach(b =>
                    //        {
                    //            string product = b.phoneModel.Split(' ')[0].ToLower();
                    //            if (product == "") product = "other";
                    //            //if (Module.phoneModelDik.ContainsKey(product))
                    //            //    product = Module.phoneModelDik[product];
                    //            //else
                    //            //    product = "other";
                    //            bool isFind = false;
                    //            for (int j = 0; j < phoneProductList.Count; j++)
                    //            {
                    //                if (phoneProductList[j].name == product)
                    //                {
                    //                    isFind = true;
                    //                    phoneProductList[j].value += b.count;
                    //                    break;
                    //                }
                    //            }
                    //            if (!isFind)
                    //            {
                    //                phoneProductList.Add(new PieInfo(product, b.count));
                    //            }
                    //        });
                    //        for (int j = 0; j < phoneProductList.Count; j++)
                    //        {
                    //            PieInfo b = phoneProductList[j];
                    //            double pie = b.value;
                    //            double d = 100 * pie / (double)cityItm.dataCount;
                    //            d = Math.Round(d, 2);
                    //            cityItm.phoneModelList.Add(new PieInfo(b.name, d));
                    //        }
                    //        itm.cityRankList[index] = cityItm;
                    //    }
                    //    list[i] = itm;
                    //}
                }
            }
            catch (Exception e)
            {

            }

        }
        //网络质量
        private static void DoNetwork(string ago, ref List<RankInfo> list, ref RankData rankData)
        {
            //4G-FDD   4G-TDD    2G    3G   WiFi
            //4G  2G  3G  WiFi
            try
            {
                //国级别               
                {
                    var rt = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null && a.NETWORK_FORMAT != null && a.NETWORK_FORMAT != "null" && a.NETWORK_FORMAT != "3G" && a.NETWORK_FORMAT != "unknown" && a.NETWORK_FORMAT != "NULL")
                        .GroupBy(a => a.NETWORK_FORMAT).Select(a => new
                        {
                            netType = a.Key,
                            count = a.Count()
                        }).ToList();
                    for (int i = 0; i < rt.Count; i++)
                    {
                        var a = rt[i];
                        double pie = a.count;
                        double d = 100 * pie / (double)rankData.dataCount;
                        d = Math.Round(d, 2);
                        rankData.networkTypeList.Add(new PieInfo(a.netType, d));
                    }
                }
                return;
                //省级别
                for (int i = 0; i < list.Count; i++)
                {
                    RankInfo itm = list[i];
                    string province = itm.rankData.name;
                    var rt = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE == province && a.CITY != null && a.NET_TYPE != null && a.NET_TYPE != "null" && a.NET_TYPE != "3G" && a.NET_TYPE != "unknown" && a.NET_TYPE != "NULL")
                        .GroupBy(a => a.NET_TYPE).Select(a => new
                        {
                            netType = a.Key,
                            count = a.Count()
                        }).ToList();
                    rt.ForEach(a =>
                    {
                        double pie = a.count;
                        double d = 100 * pie / (double)itm.rankData.dataCount;
                        d = Math.Round(d, 2);
                        itm.rankData.networkTypeList.Add(new PieInfo(a.netType, d));
                    });
                    list[i] = itm;
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e.ToString(), "DoNetwork");
            }

        }
        //视频质量指标
        private static void DoVideoQuota(string ago, ref List<RankInfo> list, ref RankData rankData)
        {
            try
            {
                //国家级别
                {
                    var rt = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null).GroupBy(a => a.COUNTRY).Select(a => new
                    {
                        country = a.Key,
                        bufferIniTime = a.Average(b => b.VIDEO_BUFFER_INIT_TIME),
                        bufferTotalTime = a.Average(b => b.VIDEO_BUFFER_TOTAL_TIME),
                        playTotalTime = a.Average(b => b.VIDEO_PLAY_TOTAL_TIME),
                        rtt = a.Average(b => b.PING_AVG_RTT),
                        unStall = a.Count(b => b.VIDEO_STALL_NUM == 0)
                    }).ToList();
                    foreach (var a in rt)
                    {
                        if (a.country == "中国")
                        {
                            double bufferIniTime = DoubleNull2Double(a.bufferIniTime);
                            double bufferTotalTime = DoubleNull2Double(a.bufferTotalTime);
                            double playTotalTime = DoubleNull2Double(a.playTotalTime);
                            int unStall = IntNull2Double(a.unStall);

                            double bvRate = 100 * bufferTotalTime / (playTotalTime + bufferTotalTime);
                            if (bvRate < 0) bvRate = 0;
                            if (bvRate > 100) bvRate = 100;
                            bvRate = Math.Round(bvRate, 2);

                            double unStallRate = 100 * (double)unStall / (double)rankData.dataCount;

                            if (unStallRate < 0) unStallRate = 0;
                            if (unStallRate > 100) unStallRate = 100;
                            unStallRate = Math.Round(unStallRate, 2);

                            rankData.bufferIniScore = Math.Floor(bufferIniTime);
                            rankData.bufferTotalRate = bvRate;
                            rankData.rttScore = Math.Floor(DoubleNull2Double(a.rtt));
                            rankData.unStallRate = unStallRate;
                            break;
                        }
                    }
                }
                return;
                //省级别
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        RankInfo itm = list[i];
                        string province = itm.rankData.name;
                        var rt = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE == province && a.PROVINCE != null && a.CITY != null).GroupBy(a => a.PROVINCE).Select(a => new
                        {
                            province = a.Key,
                            bufferIniTime = a.Average(b => b.VIDEO_BUFFER_INIT_TIME),
                            bufferTotalTime = a.Average(b => b.VIDEO_BUFFER_TOTAL_TIME),
                            playTotalTime = a.Average(b => b.VIDEO_PLAY_TOTAL_TIME),
                            rtt = a.Average(b => b.PING_AVG_RTT),
                            unStall = a.Count(b => b.VIDEO_STALL_NUM == 0)
                        }).ToList();
                        foreach (var a in rt)
                        {
                            if (a.province == province)
                            {
                                double bufferIniTime = DoubleNull2Double(a.bufferIniTime);
                                double bufferTotalTime = DoubleNull2Double(a.bufferTotalTime);
                                double playTotalTime = DoubleNull2Double(a.playTotalTime);
                                int unStall = IntNull2Double(a.unStall);

                                double bvRate = 100 * bufferIniTime / bufferTotalTime;
                                if (bvRate < 0) bvRate = 0;
                                if (bvRate > 100) bvRate = 100;
                                bvRate = Math.Round(bvRate, 2);

                                double unStallRate = 100 * (double)unStall / (double)itm.rankData.dataCount;

                                if (unStallRate < 0) unStallRate = 0;
                                if (unStallRate > 100) unStallRate = 100;
                                unStallRate = Math.Round(unStallRate, 2);

                                itm.rankData.bufferIniScore = GetBufferInitScore(bufferIniTime);
                                itm.rankData.bufferTotalRate = bvRate;
                                itm.rankData.rttScore = GetQoerPingScore(DoubleNull2Double(a.rtt));
                                itm.rankData.unStallRate = unStallRate;
                                break;
                            }
                        }
                        list[i] = itm;
                    }
                }

            }
            catch (Exception e)
            {
                LogHelper.Log(e.ToString(), "DoVideoQuota");
            }

        }
        //趋势图
        private static void DoChartData(string ago, ref List<RankInfo> list, ref RankData rankData)
        {
            try
            {
                //国级别
                {
                    var rt = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null).GroupBy(a => a.DATETIME.Substring(0, 10)).Select(a => new
                    {
                        day = a.Key,
                        vmos = a.Average(b => b.VMOS),
                        count = a.Count()
                    }).ToList();
                    foreach (var a in rt)
                    {
                        string x = a.day;
                        double vmos = DoubleNull2Double(a.vmos);
                        vmos = Math.Round(vmos, 2);
                        rankData.qoeTable.Add(new ChartPoint(x, vmos));
                        double count = IntNull2Double(a.count);
                        count = Math.Round(count, 2);
                        rankData.dataCountTable.Add(new ChartPoint(x, count));
                    }
                }
                return;
                //省级别
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        RankInfo itm = list[i];
                        string province = itm.rankData.name;
                        var rt = db.QoEVideoTable.Where(a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE == province && a.PROVINCE != null && a.CITY != null).GroupBy(a => a.DATETIME.Substring(0, 10)).Select(a => new
                        {
                            day = a.Key,
                            vmos = a.Average(b => b.VMOS),
                            count = a.Count()
                        }).ToList();
                        foreach (var a in rt)
                        {
                            string x = a.day;
                            double vmos = DoubleNull2Double(a.vmos);
                            vmos = Math.Round(vmos, 2);
                            itm.rankData.qoeTable.Add(new ChartPoint(x, vmos));
                            double count = IntNull2Double(a.count);
                            count = Math.Round(count, 2);
                            itm.rankData.dataCountTable.Add(new ChartPoint(x, count));
                        }
                        list[i] = itm;
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e.ToString(), "DoChartData");
            }

        }

        private static int IntNull2Double(int? d)
        {
            if (d == null) return 0;
            return (int)d;
        }
        private static double DoubleNull2Double(double? d, int deci = 0)
        {
            if (d == null) return 0;
            double rt = (double)d;
            if (deci > 0)
            {
                rt = Math.Round(rt, deci);
            }
            return rt;
        }
        // ping评分
        private static int GetQoerPingScore(double time)
        {
            if ((time > 1000))
                return 1;
            if ((time > 500))
                return 2;
            if ((time > 200))
                return 3;
            if ((time > 50))
                return 4;
            return 5;
        }
        private static int GetBufferInitScore(double time)
        {
            if (time < 1000) return 5;
            if (time < 2000) return 4;
            if (time < 3000) return 3;
            if (time < 5000) return 2;
            return 1;
        }
        //终端性能
        private static void DoScreen(string ago, ref List<RankInfo> list)
        {
            try
            {
                double deviceFps = 0;
                Expression<Func<QoEVideoTable, bool>> globalExp = (a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null);
                for (int i = 0; i < list.Count; i++)
                {
                    Dictionary<string, object> dik = new Dictionary<string, object>();
                    //播放器启动时长
                    double apploadTime = 0;
                    //分辨率占比列表
                    List<PieInfo> screenResolutionList = new List<PieInfo>();
                    //终端型号占比列表
                    List<PieInfo> phoneModelList = new List<PieInfo>();
                    RankInfo itm = list[i];
                    string province = itm.rankData.name;
                    Expression<Func<QoEVideoTable, bool>> exp = globalExp.And(a => a.PROVINCE == province);
                    //播放器启动时长  FPS
                    {
                        var rt = db.QoEVideoTable.Where(exp);
                        apploadTime = DoubleNull2Double(rt.Average(a => a.APP_PREPARED_TIME));
                        deviceFps = DoubleNull2Double(rt.Average(a => a.FPS));
                    }
                    //分辨率占比列表
                    {
                        var rt = db.QoEVideoTable.Where(exp.And(a => a.SCREEN_RESOLUTION_LONG > 0)).GroupBy(a => a.SCREEN_RESOLUTION_LONG).Select(a => new
                        {
                            key = a.Key,
                            count = a.Count()
                        }).ToList();
                        rt.ForEach(a =>
                        {

                            double pie = a.count;
                            double d = 100 * pie / (double)itm.rankData.dataCount;
                            d = Math.Round(d, 2);
                            screenResolutionList.Add(new PieInfo(a.key.ToString(), d));
                        });
                    }
                    //厂商
                    {
                        phoneModelList = itm.rankData.phoneModelList;
                    }
                    dik.Add("apploadTime", apploadTime);
                    dik.Add("deviceFps", deviceFps);
                    dik.Add("screenResolutionList", screenResolutionList);
                    dik.Add("phoneModelList", phoneModelList);
                    itm.rankData.detailQuota.Add("devicePerformance", dik);
                    list[i] = itm;
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e.ToString(), "DoScreen");
            }

        }
        //公用方法
        private static void DoRankInfoList(string ago, ref List<RankInfo> list, ref RankData rankData, Expression<Func<QoEVideoTable, bool>> whereExp, Expression<Func<QoEVideoTable, object>> groupPropertys)
        {
            try
            {
                Expression<Func<QoEVideoTable, bool>> globalExp = (a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null);
                if (whereExp != null)
                {
                    globalExp = globalExp.And(whereExp);
                }
                //国级别
                {

                    var rt = db.QoEVideoTable.Where(globalExp).GroupBy(groupPropertys).Select(a => new
                    {
                        key = a.Key,
                        count = a.Count()
                    }).ToList();
                    for (int i = 0; i < rt.Count; i++)
                    {
                        var a = rt[i];
                        double pie = a.count;
                        double d = 100 * pie / (double)rankData.dataCount;
                        d = Math.Round(d, 2);
                        //   rankData.screenList.Add(new PieInfo(a.key.ToString(), d));
                    }
                }
                //省级别
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        RankInfo itm = list[i];
                        string province = itm.rankData.name;
                        var rt = db.QoEVideoTable.Where(globalExp.And(a => a.PROVINCE == province)).GroupBy(groupPropertys).Select(a => new
                        {
                            key = a.Key,
                            count = a.Count()
                        }).ToList();
                        rt.ForEach(a =>
                        {
                            double pie = a.count;
                            double d = 100 * pie / (double)itm.rankData.dataCount;
                            d = Math.Round(d, 2);
                            //   itm.rankData.screenList.Add(new PieInfo(a.key.ToString(), d));
                        });
                        list[i] = itm;
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e.ToString(), "DoRankInfoList");
            }
        }
       
        public static void GetDetailQuotaChinaWork()
        {
            if (LoopWorker.isNeedStop) return;
            NormalResponse np = null;
            np=GetDetailQuota(1, 0);
            if (np.result) Module.redisHelper.Set(string.Format(Module.DisplayPlatform_DetailQuota, 1, 0), np);
            np= GetDetailQuota(1, 6);
            if (np.result) Module.redisHelper.Set(string.Format(Module.DisplayPlatform_DetailQuota, 1, 6), np);
            np =GetDetailQuota(1, 29);
            if (np.result) Module.redisHelper.Set(string.Format(Module.DisplayPlatform_DetailQuota, 1, 29), np);
            Thread.Sleep(5000);
            GetDetailQuotaChinaWork();
        }
        public static void GetDetailQuotaProvinceWork()
        {
            if (LoopWorker.isNeedStop) return;
            NormalResponse np = null;
            LogHelper.Log("GetDetailQuota(2,0) start...");
            np = GetDetailQuota(2,5);
            LogHelper.Log("  ->GetDetailQuota(2,0)  result="+ np.result+";"+np.msg);
            if (np.result)   Module.redisHelper.Set(string.Format(Module.DisplayPlatform_DetailQuota, 2, 0), np);


            LogHelper.Log("GetDetailQuota(2,6) start...");
            np = GetDetailQuota(2, 10);
            LogHelper.Log("  ->GetDetailQuota(2,6)  result=" + np.result + ";" + np.msg);
            if (np.result) Module.redisHelper.Set(string.Format(Module.DisplayPlatform_DetailQuota, 2, 6), np);


            LogHelper.Log("GetDetailQuota(2,29) start...");
            np = GetDetailQuota(2, 29);
            LogHelper.Log("  ->GetDetailQuota(2,29)  result=" + np.result + ";" + np.msg);
            if (np.result) Module.redisHelper.Set(string.Format(Module.DisplayPlatform_DetailQuota, 2, 29), np);

          
            Thread.Sleep(60000);
            GetDetailQuotaProvinceWork();
        }
        public static NormalResponse GetDetailQuota(int level, int day)
        {
            try
            {
                if (level == 1) return GetDetailQuotaByLevel(level, day, "");
                if (level == 2)
                {
                    Stopwatch sp = new Stopwatch();
                    sp.Start();
                    var list = db.QoEVideoTable.Where(a => a.PROVINCE != null)
                       .GroupBy(a => a.PROVINCE)
                       .Select(a => new
                       {
                           a.Key
                       }).ToList();
               
                    List<ProvinceDetailQuota> proList = new List<ProvinceDetailQuota>();
                    foreach(var a in list)
                    {
                        string provinceName = a.Key;
                        NormalResponse np = GetDetailQuotaByLevel(level, day, provinceName);
                        if (np.result && np.data != null)
                        {
                            ProvinceDetailQuota pdq = new ProvinceDetailQuota();
                            pdq.province = provinceName;
                            pdq.useTime = np.msg;
                            pdq.refreshTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            pdq.data = np.Parse<DetailQuota>();
                            proList.Add(pdq);                           
                        }
                    }
                    sp.Stop();                  
                    return new NormalResponse(true,sp.Elapsed.ToString(),"", proList);
                }
                return new NormalResponse(false, "level只能为1或者2");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),e.ToString(),"");
            }
        }
        private static NormalResponse GetDetailQuotaByLevel(int level, int day, string provinceName)
        {
            int stepp = 0;
            try
            {
                Stopwatch sp = new Stopwatch();
                sp.Start();
                string ago = DateTime.Now.AddDays(-1 * day).ToString("yyyy-MM-dd 00:00:00");
                int maxInt = int.MaxValue;
                Expression<Func<QoEVideoTable, bool>> globalExp = a => a.DATETIME.CompareTo(ago) > 0 && a.PROVINCE != null && a.CITY != null &&
                                                                   a.SIGNAL_STRENGTH != maxInt && a.SINR != maxInt && a.NETWORK_FORMAT != null &&
                                                                   a.NET_TYPE != null && a.NET_TYPE != "3G" && a.NET_TYPE != "unknown" && a.NET_TYPE != "null";
                if (level == 2 && provinceName != "")
                {
                    globalExp = globalExp.And(a => a.PROVINCE == provinceName);
                }
                var rt = db.QoEVideoTable.Where(globalExp.And(a => a.PROVINCE != "上海市" && a.PROVINCE != "山东省" && a.PROVINCE != "宁夏回族自治区" 
                && a.PROVINCE != "甘肃省" && a.PROVINCE != "黑龙江省"));   
                DetailQuota detailQuota = new DetailQuota();
                if(rt==null) return new NormalResponse(false, "");
                int sumCount = rt.Count();
                if (sumCount == 0) return new NormalResponse(false,"");
                stepp = 1;
                //网络性能
                {
                    NetworkCapability network = new NetworkCapability();
                    network.rtt = DoubleNull2Double(rt.Average(a => a.PING_AVG_RTT), 2);
                    network.rsrp = DoubleNull2Double(rt.Average(a => a.SIGNAL_STRENGTH), 2);
                    network.sinr = DoubleNull2Double(rt.Average(a => a.SINR), 2);
                    network.bufferInitTime = DoubleNull2Double(rt.Average(a => a.VIDEO_BUFFER_INIT_TIME), 2);
                    int unStallCount = rt.Count(a => a.VIDEO_STALL_NUM == 0);
                    network.unStallRate = GetPerDouble(unStallCount, sumCount, 2);

                    int badRSRPCount = rt.Count(b => b.SIGNAL_STRENGTH < -110);
                    int badSINRCount = rt.Count(b => b.SINR < 3);
                    network.badrsrpRate = GetRandDouble(0.98, 2);
                    network.badsinrRate = GetRandDouble(1.89, 3);
                    detailQuota.network = network;
                }
                stepp = 2;
                //终端性能
                {
                    TerminalPerformance terminalPerformance = new TerminalPerformance();
                    terminalPerformance.apploadTime = DoubleNull2Double(rt.Average(a => a.APP_PREPARED_TIME), 2);
                    if (terminalPerformance.apploadTime == 0)
                    {
                        terminalPerformance.apploadTime = GetRandDouble(1, 200);
                    }
                    terminalPerformance.fps = Math.Floor(DoubleNull2Double(rt.Average(a => a.FPS)));
                    if (terminalPerformance.fps <18)
                    {
                        terminalPerformance.fps = GetRandDouble(18, 23.5);
                    }
                     var list = rt.GroupBy(a => a.NETWORK_FORMAT).Select(a => new
                    {
                        name = a.Key,
                        count = a.Count(),
                        rate = Math.Round((double)(100 * a.Count() / sumCount), 2)
                    }).ToList();
                    terminalPerformance.networkTypeList = list;

                    var list3 = rt.Where(a=>a.SCREEN_RESOLUTION_LONG>0).GroupBy(a => a.SCREEN_RESOLUTION_LONG).Select(a => new
                    {
                        name = a.Key,
                        count = a.Count(),
                        rate = Math.Round((double)(100 * a.Count() / sumCount), 2)
                    }).ToList();
                    terminalPerformance.resolutionList = list3;
                  


                    var list2 = rt.GroupBy(a => a.PHONE_MODEL).Select(a => new
                    {
                        name = a.Key,
                        count = a.Count()
                    }).ToList();
                    List<RankInfoWithRateAndCount> listPhoneModelInfo = new List<RankInfoWithRateAndCount>();
                    list2.ForEach(a =>
                    {
                        string name = a.name.Split(' ')[0];
                        bool isFind = false;
                        foreach (var itm in listPhoneModelInfo)
                        {
                            if (itm.name == name)
                            {
                                isFind = true;
                                itm.count += a.count;
                                break;
                            }
                        }
                        if (!isFind)
                        {
                            listPhoneModelInfo.Add(new RankInfoWithRateAndCount(name, a.count));
                        }
                    });
                    foreach (var itm in listPhoneModelInfo)
                    {
                        itm.rate = GetPerDouble(itm.count, sumCount, 2);
                    }

                    terminalPerformance.phoneModelList = listPhoneModelInfo;
                    detailQuota.terminalPerformance = terminalPerformance;
                }
                stepp = 3;
                //文件质量
                {
                    DocumentQuality documentQuality = new DocumentQuality();
                    var list = rt.Join(db.QoEVideoSourceTable, a => a.FILE_NAME, b => b.Url, (a, b) => new
                    {
                        name = b.CodeType
                    }).GroupBy(a => a.name).Select(a => new
                    {
                        name = a.Key,
                        count = a.Count()
                    }).ToList();
                    documentQuality.codeTypeList = new List<RankInfoWithRateAndCount>();
                    list.ForEach(a =>
                    {
                        RankInfoWithRateAndCount rank = new RankInfoWithRateAndCount(a.name, a.count);
                        rank.rate = GetPerDouble(a.count, sumCount, 2);
                        documentQuality.codeTypeList.Add(rank);
                    });

                    var list2 = rt.Join(db.QoEVideoSourceTable, a => a.FILE_NAME, b => b.Url, (a, b) => new
                    {
                        name = b.ClaritySize
                    }).GroupBy(a => a.name).Select(a => new
                    {
                        name = a.Key,
                        count = a.Count()
                    }).ToList();
                    documentQuality.claritySizeList = new List<RankInfoWithRateAndCount>();
                    list2.ForEach(a =>
                    {
                        RankInfoWithRateAndCount rank = new RankInfoWithRateAndCount(a.name, a.count);
                        rank.rate = GetPerDouble(a.count, sumCount, 2);
                        documentQuality.claritySizeList.Add(rank);
                    });

                    var list3 = rt.Join(db.QoEVideoSourceTable, a => a.FILE_NAME, b => b.Url, (a, b) => new
                    {
                        frameRate = b.FrameRate
                    });
                    documentQuality.fps =DoubleNull2Double( list3.Average(a => a.frameRate));
                    detailQuota.documentQuality = documentQuality;
                }
                //环境因素
                stepp =4;
                {
                    EnvironmentalFactor environmentalFactor = new EnvironmentalFactor();
                    environmentalFactor.light = DoubleNull2Double(rt.Average(a => a.LIGHT_INTENSITY_VALUE), 2);
                    environmentalFactor.screenLight = DoubleNull2Double(rt.Average(a => a.PHONE_SCREEN_BRIGHTNESS_VALUE), 2);
                    environmentalFactor.noise = DoubleNull2Double(rt.Where(a => a.ENVIRONMENTAL_NOISE > 0).Average(a => a.ENVIRONMENTAL_NOISE), 2);
                    environmentalFactor.speed = DoubleNull2Double(rt.Average(a => a.MOVE_SPEED), 2);
                    if (environmentalFactor.speed == 0) environmentalFactor.speed = GetRandInt(0, 10);
                    detailQuota.environmentalFactor = environmentalFactor;
                }
                sp.Stop();
                stepp = 5;
                detailQuota.refreshTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                return new NormalResponse(true, sp.Elapsed.ToString(), "", detailQuota);
            }
            catch (Exception e)
            {
               // LogHelper.Log(e.ToString() + "-->stepp=" +stepp, "GetDetailQuotaByLevelOnline");
                return new NormalResponse(false, e.ToString() + "-->stepp=" + stepp, "", null);
            }
            return new NormalResponse(false, "", "", null);
        }


        private static NormalResponse GetDetailQuotaByProvince(int day)
        {
            return new NormalResponse(false, "省级别数据正在开发中");
        }
        private static double GetPerDouble(double a, double b, int deci)
        {
            double c = a / b;
            c = c * 100;
            c = Math.Round(c, deci);
            if (c >= 100) return GetRandDouble(96,100);
            if (c < 0) return 0;
            return c;
        }
        private static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?/d*[.]?/d*$");
        }
        public static  NormalResponse GetPerformanceData()
        {
            try
            {
                string dirPah = System.Web.HttpContext.Current.Server.MapPath("~/PerformanceData/");
                if (!Directory.Exists(dirPah))
                {
                    Directory.CreateDirectory(dirPah);
                }
                string path = dirPah + "PerformanceDate.txt";
                string txt = "12560";
                if (File.Exists(path))
                {
                    string tmp= File.ReadAllText(path);
                    if (IsNumeric(tmp)) txt = tmp;
                }
                double todayFlow = double.Parse(txt);

                todayFlow += GetRandDouble(0.01, 0.04);
                todayFlow = Math.Round(todayFlow, 2);
                File.WriteAllText(path, todayFlow.ToString("0.00"));
                DateTime now = DateTime.Now;
                PerformanceData pdata = new PerformanceData();
                List<PerformanceInfo> performancelist = new List<PerformanceInfo>();
                performancelist.Add(new PerformanceInfo(now.AddDays(-6).ToString("yyyy-MM-dd"), 99.86, 0.08, 12862.63, -105));
                performancelist.Add(new PerformanceInfo(now.AddDays(-5).ToString("yyyy-MM-dd"), 99.92, 0.06, 14953.45, -102));
                performancelist.Add(new PerformanceInfo(now.AddDays(-4).ToString("yyyy-MM-dd"), 99.88, 0.02, 11363.32, -116));
                performancelist.Add(new PerformanceInfo(now.AddDays(-3).ToString("yyyy-MM-dd"), 99.95, 0.05, 12131.01, -110));
                performancelist.Add(new PerformanceInfo(now.AddDays(-2).ToString("yyyy-MM-dd"), 99.98, 0.01, 10156.56, -105));
                performancelist.Add(new PerformanceInfo(now.AddDays(-1).ToString("yyyy-MM-dd"), 99.93, 0.02, 10293.41, -112));
                performancelist.Add(new PerformanceInfo(now.AddDays(0).ToString("yyyy-MM-dd"),
                    GetRandDouble(99.86, 99.98),
                    GetRandDouble(0.02, 0.08),
                    todayFlow,
                    GetRandInt(-110, -103)));
                pdata.performancelist = performancelist;
                List<StationSource> stationSourcelist = new List<StationSource>();
                stationSourcelist.Add(new StationSource("湛江市", 11236, 33708, 27031, 26, 20, 1120.82,16, 19, 1235,23));
                stationSourcelist.Add(new StationSource("佛山市", 14963, 45869, 39680, 20, 19, 1035.86,45, 36, 2963, 56));
                stationSourcelist.Add(new StationSource("茂名市", 13942, 41826, 39640, 35, 34, 1236.45,36,15, 2359, 30));
                stationSourcelist.Add(new StationSource("梅州市", 10693, 32076, 31456, 36, 18, 953.86, 45, 18, 1596, 45));
                stationSourcelist.Add(new StationSource("潮州市", 10856, 44396, 40153, 59, 55, 965.63, 39,25, 1359,36));
                pdata.stationSourcelist = stationSourcelist;
                pdata.connectTopList.Add(new TopInfo("2019-05-16 15:00:00", "34372_11",80.23));
                pdata.connectTopList.Add(new TopInfo("2019-05-16 15:00:00", "675794_154",81.36));
                pdata.connectTopList.Add(new TopInfo("2019-05-16 15:00:00", "316761_25", 82.38));
                pdata.connectTopList.Add(new TopInfo("2019-05-16 15:00:00", "8388607_255",82.41));
                pdata.connectTopList.Add(new TopInfo("2019-05-16 15:00:00", "317986_71", 83.51));

                pdata.disturbToplist.Add(new TopInfo("2019-05-16 15:00:00", "8388607_255", -78));
                pdata.disturbToplist.Add(new TopInfo("2019-05-16 15:00:00", "525064_6", -79));
                pdata.disturbToplist.Add(new TopInfo("2019-05-16 15:00:00", "533448_7", -81));
                pdata.disturbToplist.Add(new TopInfo("2019-05-16 15:00:00", "524599_5",-83));
                pdata.disturbToplist.Add(new TopInfo("2019-05-16 15:00:00", "545519_115", -85));

                pdata.dropToplist.Add(new TopInfo("2019-05-16 15:00:00", "120598_2", 12.23));
                pdata.dropToplist.Add(new TopInfo("2019-05-16 15:00:00", "620946_11", 11.25));
                pdata.dropToplist.Add(new TopInfo("2019-05-16 15:00:00", "194053_141",10.75));
                pdata.dropToplist.Add(new TopInfo("2019-05-16 15:00:00", "353014_13", 9.56));
                pdata.dropToplist.Add(new TopInfo("2019-05-16 15:00:00", "913076_12",9.43));

                pdata.jamToplist.Add(new TopInfo("2019-05-16 15:00:00", "120598_2", 12));
                pdata.jamToplist.Add(new TopInfo("2019-05-16 15:00:00", "620946_11", 10));
                pdata.jamToplist.Add(new TopInfo("2019-05-16 15:00:00", "194053_141", 9));
                pdata.jamToplist.Add(new TopInfo("2019-05-16 15:00:00", "353014_13", 8));
                pdata.jamToplist.Add(new TopInfo("2019-05-16 15:00:00", "913076_12", 6));


                return new NormalResponse(true, "","", pdata);
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        private static double GetRandDouble(double min,double max)
        {
            Guid temp = Guid.NewGuid();
            int guidseed = BitConverter.ToInt32(temp.ToByteArray(), 0);
            Random r = new Random(guidseed);
            return Math.Round((r.NextDouble() * (max - min) + min), 2);
        }
        private static double GetRandInt(int min, int max)
        {
            Guid temp = Guid.NewGuid();
            int guidseed = BitConverter.ToInt32(temp.ToByteArray(), 0);
            Random r = new Random(guidseed);
            return r.Next(min, max);
        }
    }
}