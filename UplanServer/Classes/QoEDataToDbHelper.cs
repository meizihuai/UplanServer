using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace UplanServer
{
    public class QoEDataToDbHelper
    {
        public static async Task<NormalResponse> UploadQoERForiOS(QoEReportIOSInfo qoer)
        {
            qoer.Decode();
            if (qoer.Lon > 0 && qoer.Lat > 0)
            {
                CoordInfo gds = CoordInfo.GetGDCoord(qoer.Lon, qoer.Lat);
                if (gds != null)
                {
                    qoer.GDLon = gds.x;
                    qoer.GDLat = gds.y;
                }
                gds = CoordInfo.GetBDCoord(qoer.Lon, qoer.Lat);
                if (gds != null)
                {
                    qoer.BDLon = gds.x;
                    qoer.BDLat = gds.y;
                }
                LocationInfo la = await new LocationService().GetLocationByGps(qoer.BDLon, qoer.BDLat);
                if (la != null)
                {
                    qoer.Province = la.Province;
                    qoer.City = la.City;
                    qoer.District = la.District;
                    qoer.Address = la.DetailAddress;
                }
            }
            else
            {
                LocationInfo la =await new LocationService().GetLocationByIp(qoer.IP);
                if (la != null)
                {
                    qoer.Province = la.Province;
                    qoer.City = la.City;
                    qoer.District = la.District;
                    qoer.Address = la.DetailAddress;
                }
            }
            using(var db=new QoEDbContext())
            {
                DeviceInfo device = db.DeviceTable.Where(a => a.AID == qoer.AID).FirstOrDefault();
                if (device != null) qoer.UUID = device.UUID;
                if (qoer.NetType.ToLower() == "2g") qoer.NetType = "2G";
                if (qoer.NetType.ToLower() == "3g") qoer.NetType = "3G";
                if (qoer.NetType.ToLower() == "4g") qoer.NetType = "4G";
                if (qoer.NetType.ToLower() == "wifi") qoer.NetType = "WiFi";
                db.QoERIOSTable.Add(qoer);
                db.SaveChanges();
                return new NormalResponse(true, "success");
            }          
        }
        public static Task<NormalResponse> UploadQoER(PhoneInfo pi,bool isNeedHandlePi=true)
        {
            return Task.Run(async()=>
            {
                if (isNeedHandlePi)
                {
                    pi =await HandlePhoneInfo(pi);
                }
                using (var db = new QoEDbContext())
                {
                    db.QoERTable.Add(pi);
                    db.SaveChanges();
                    return new NormalResponse(true, "success");
                }
            });
                
        }
        private static async Task<PhoneInfo> HandlePhoneInfo(PhoneInfo pi)
        {
            if (string.IsNullOrEmpty(pi.BusinessType)) pi.BusinessType = "QOER";           
            if (string.IsNullOrEmpty(pi.ApkName)) pi.ApkName = "UniQoE";
            pi.IsOutSide = 0;
            pi.Freq = pi.EarFcn;
            pi.EnodebId_CellId = pi.ENodeBId + "_" + pi.CellId;
            pi.Day = pi.DateTime.Substring(0, 10);
            if (!string.IsNullOrEmpty(pi.HTTP_URL) && pi.Http_Response_Time!=null)
            {
                long time = (long)pi.Http_Response_Time;
                pi.VMOS = ScoreHelper.GetQoEHttpResponseScore(time);
            }
            if (pi.NeighbourList != null && pi.NeighbourList.Count>0)
            {
                Neighbour nb = pi.NeighbourList[0];
                pi.Adj_RSRP1 = nb.RSRP;
                pi.Adj_PCI1 = nb.PCI;
                pi.ADJ_EARFCN1 = nb.EARFCN;
            }
            if (pi.XyZaSpeed != null) pi.XYZASpeedString = JsonConvert.SerializeObject(pi.XyZaSpeed);
            if (pi.SatelliteCount >= 4) pi.IsOutSide = 1;
            //AES解密
            pi.Decode();
            double lon = (pi.Lon == null ? 0 : (double)pi.Lon);
            double lat = (pi.Lat == null ? 0 : (double)pi.Lat);
           
            pi.BDlon = 0;
            pi.BDlat = 0;
            pi.GDlon = 0;
            pi.GDlat = 0;
            if (lon > 0 && lat > 0)
            {
                CoordInfo bdCoor = CoordInfo.GetBDCoord(lon, lat);
                CoordInfo gdCoor = CoordInfo.GetGDCoord(lon, lat);
                pi.BDlon = bdCoor.x;
                pi.BDlat = bdCoor.y;
                pi.GDlon = gdCoor.x;
                pi.GDlat = gdCoor.y;
                LocationInfo la = await new LocationService().GetLocationByGps(Utils.DoubleNull2Double(pi.BDlon), Utils.DoubleNull2Double(pi.BDlat));
                if (la != null)
                {
                    pi.Province = la.Province;
                    pi.City = la.City;
                    pi.District = la.District;
                    pi.Address = la.DetailAddress;
                }
            }
            else
            {
                LocationInfo la = await new LocationService().GetLocationByIp(pi.IP);
                if (la != null)
                {
                    pi.Province = la.Province;
                    pi.City = la.City;
                    pi.District = la.District;
                    pi.Address = la.DetailAddress;
                }

            }
            return pi;
        }
      

        public static Task<NormalResponse> UploadQoEBlackPoint(QoEBlackPoint qoe)
        {
            return Task.Run(async()=>
            {
                PhoneInfo pi = qoe.Pi;
                if (pi == null) return new NormalResponse(false, "QoER字段不可为空");
                string rid = Guid.NewGuid().ToString("N");
                pi.RID = rid;
                pi.BusinessType = "QoEBlackPoint";
                pi =await HandlePhoneInfo(pi);
                if ( !(await UploadQoER(pi, false)).result) rid = "";
                qoe.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (string.IsNullOrEmpty(qoe.AID)) qoe.AID = pi.AID;
                qoe.Lat = Utils.DoubleNull2Double(pi.Lat);
                qoe.Lon = Utils.DoubleNull2Double(pi.Lon);
                qoe.GDLat = Utils.DoubleNull2Double(pi.GDlat);
                qoe.GDLon = Utils.DoubleNull2Double(pi.GDlon);
                qoe.Privince = pi.Province;
                qoe.City = pi.City;
                qoe.District = pi.District;
                qoe.PiRID = rid;
                using (var db = new QoEDbContext())
                {
                    db.QoEBlackPointTable.Add(qoe);
                    db.SaveChanges();
                }
                return new NormalResponse(true, "success");
            });
            
        }  

        public static Task<NormalResponse> UploadQoE(QoEVideoTable qoe)
        {
            return Task.Run(async () =>
            {
                qoe = await HandleQoEVideo(qoe);               
                using (var db = new QoEDbContext())
                {
                    db.QoEVideoTable.Add(qoe);
                    db.SaveChanges();
                    return new NormalResponse(true, "success");
                }
            });
        }
        private static async Task<QoEVideoTable>HandleQoEVideo(QoEVideoTable qoe)
        {
            if (string.IsNullOrEmpty(qoe.BUSINESS_TYPE)) qoe.BUSINESS_TYPE = "";
            PhoneInfo npi = qoe.PI;
            if (npi == null) return qoe;
            npi =await HandlePhoneInfo(npi);
            qoe.MNC = npi.MNC;
            qoe.WIFI_SSID = npi.WiFi_SSID;
            qoe.WIFI_MAC = npi.WiFi_MAC;
            qoe.PING_AVG_RTT = npi.Ping_Avg_Rtt;
            qoe.FREQ = npi.Freq;
            qoe.CPU = npi.CPU;
            //qoe.ADJ_SIGNAL = npi.Adj_Signal;
            qoe.ADJ_PCI1 = npi.Adj_PCI1;
            qoe.ADJ_RSRP1 = npi.Adj_RSRP1;
            qoe.ISSCREENON = npi.IsScreenOn;
            qoe.PHONE_MODEL = npi.PhoneModel;
            qoe.CARRIER = npi.Carrier;
            qoe.ENODEBID = npi.ENodeBId;
            qoe.TAC = npi.TAC;
            qoe.ISGPSOPEN = npi.IsGPSOpen;
            qoe.ISOUTSIDE = npi.IsOutSide;
            qoe.NET_TYPE = npi.NetType;
            if (!string.IsNullOrEmpty(qoe.NET_TYPE))
            {
                if (qoe.NET_TYPE.ToLower() == "wifi")
                {
                    qoe.NET_TYPE = "WiFi";
                }
                else
                {
                    qoe.NET_TYPE = qoe.NET_TYPE.ToUpper();
                }
            }
            qoe.NETWORK_FORMAT = GetNetFormat(qoe.NET_TYPE, Utils.IntNull2Int(npi.EarFcn));
            qoe.OPERATING_SYSTEM = npi.PhoneOS;
            qoe.SINR = npi.SINR;
            qoe.SIGNAL_STRENGTH = npi.RSRP;
         
            qoe.APKVERSION = npi.ApkVersion;
            qoe.ACCURACY = npi.Accuracy;
            qoe.ALTITUDE = npi.Altitude;
            qoe.MOVE_SPEED = npi.GpsSpeed;
            qoe.SATELLITECOUNT = npi.SatelliteCount;
            qoe.ECI = npi.CI;
            qoe.MCC = "460";
            qoe.PLMN = qoe.MCC + qoe.MNC;
            qoe.ISOUTSIDE = (qoe.SATELLITECOUNT>=4?1:0);
            if (qoe.VIDEO_TOTAL_TIME > 0)
                qoe.VIDEO_STALL_DURATION_PROPORTION = Math.Round(100 *Utils.IntNull2Int(qoe.VIDEO_STALL_TOTAL_TIME) / (double)qoe.VIDEO_TOTAL_TIME, 2);
            if (qoe.VIDEO_TOTAL_TIME > 0)
                qoe.BVRATE = 100 * qoe.VIDEO_BUFFER_TOTAL_TIME / (double)qoe.VIDEO_TOTAL_TIME;
            int Video_LOAD_Score = ScoreHelper.GetVideo_LOAD_Score(qoe.VIDEO_BUFFER_INIT_TIME, qoe.VIDEO_BUFFER_TOTAL_TIME, qoe.VIDEO_TOTAL_TIME);
            int Video_STALL_Score = ScoreHelper.GetVideo_STALL_Score(qoe.VIDEO_STALL_TOTAL_TIME, qoe.VIDEO_STALL_NUM, qoe.VIDEO_TOTAL_TIME);
            qoe.VIDEO_BUFFER_TOTAL_SCORE = ScoreHelper.GetVideo_Buffer_Total_Score(qoe.BVRATE);
            qoe.VIDEO_LOAD_SCORE = Video_LOAD_Score;
            qoe.VIDEO_STALL_SCORE = Video_STALL_Score;
            qoe.VMOS = ScoreHelper.GetVMOS(Video_LOAD_Score, Video_STALL_Score);
            qoe.PROVINCE = npi.Province;
            qoe.CITY = npi.City;
            qoe.DISTRICT = npi.District;
            qoe.ADDRESS = npi.Address;
            qoe.IMEI = npi.IMEI;
            qoe.IMSI = npi.IMSI;
            qoe.COUNTRY = "中国";
           if(qoe.PING_AVG_RTT==null || qoe.PING_AVG_RTT==0) qoe.PING_AVG_RTT = npi.Ping_Avg_Rtt;
            if (qoe.LIGHT_INTENSITY_list != null && qoe.LIGHT_INTENSITY_list.Count>0)
            {
                qoe.LIGHT_INTENSITY_VALUE = qoe.LIGHT_INTENSITY_list[0];
                qoe.LIGHT_INTENSITY = JsonConvert.SerializeObject(qoe.LIGHT_INTENSITY_list);               
            }
            if (qoe.PHONE_SCREEN_BRIGHTNESS_list != null && qoe.PHONE_SCREEN_BRIGHTNESS_list.Count>0)
            {
                qoe.PHONE_SCREEN_BRIGHTNESS_VALUE = qoe.PHONE_SCREEN_BRIGHTNESS_list[0];
                qoe.PHONE_SCREEN_BRIGHTNESS = JsonConvert.SerializeObject(qoe.PHONE_SCREEN_BRIGHTNESS_list);
            }
            if(qoe.CELL_SIGNAL_STRENGTHList!=null && qoe.CELL_SIGNAL_STRENGTHList.Count > 0)
            {
                qoe.CELL_SIGNAL_STRENGTH = JsonConvert.SerializeObject(qoe.CELL_SIGNAL_STRENGTHList);
            }
            if(qoe.ACCELEROMETER_DATAList!=null && qoe.ACCELEROMETER_DATAList.Count > 0)
            {
                qoe.ACCELEROMETER_DATA = JsonConvert.SerializeObject(qoe.ACCELEROMETER_DATAList);
            }
            long video_All_Peak_Speed = 0;
            if (qoe.INSTAN_DOWNLOAD_SPEEDList!=null && qoe.INSTAN_DOWNLOAD_SPEEDList.Count > 0)
            {
                qoe.INSTAN_DOWNLOAD_SPEED = JsonConvert.SerializeObject(qoe.INSTAN_DOWNLOAD_SPEEDList);
                qoe.INSTAN_DOWNLOAD_SPEEDList.ForEach(a => { if (a > video_All_Peak_Speed) video_All_Peak_Speed = a; });
            }
            qoe.VIDEO_ALL_PEAK_RATE = video_All_Peak_Speed;
            qoe.VIDEO_PEAK_DOWNLOAD_SPEED = video_All_Peak_Speed * 8;
            qoe.LONGITUDE_1 = npi.Lon;
            qoe.LATITUDE_1 = npi.Lat;
            if (qoe.GPSPointList!=null && qoe.GPSPointList.Count > 0)
            {
                //if (qoe.GPSPointList.Count >= 1)
                //{
                //    qoe.LONGITUDE_1 = qoe.GPSPointList[0].LONGITUDE;
                //    qoe.LATITUDE_1 = qoe.GPSPointList[0].LATITUDE;
                //}
                if (qoe.GPSPointList.Count >= 2)
                {
                    qoe.LONGITUDE_2 = qoe.GPSPointList[1].LONGITUDE;
                    qoe.LATITUDE_2 = qoe.GPSPointList[1].LATITUDE;
                }
                if (qoe.GPSPointList.Count >= 3)
                {
                    qoe.LONGITUDE_3 = qoe.GPSPointList[2].LONGITUDE;
                    qoe.LATITUDE_3 = qoe.GPSPointList[2].LATITUDE;
                }
                if (qoe.GPSPointList.Count >= 4)
                {
                    qoe.LONGITUDE_4 = qoe.GPSPointList[3].LONGITUDE;
                    qoe.LATITUDE_4 = qoe.GPSPointList[3].LATITUDE;
                }
            }
            if(qoe.VIDEO_STALL_NUM>0)
            {
                if (qoe.VIDEO_STALL_TOTAL_TIME == 0) qoe.VIDEO_STALL_TOTAL_TIME = 1000;
            }
            if(qoe.STALLlist==null || qoe.STALLlist.Count < qoe.VIDEO_STALL_NUM)
            {
                qoe.STALLlist = new List<QoEVideoTable.STALLInfo>();
                long totalStallTime = Utils.IntNull2Int(qoe.VIDEO_STALL_TOTAL_TIME);
                long perTotalTime = (long)Math.Ceiling((double)totalStallTime / (double)Utils.IntNull2Int(qoe.VIDEO_STALL_NUM));
                for (int i = 0; i < qoe.VIDEO_STALL_NUM; i++)
                {
                    qoe.STALLlist.Add(new QoEVideoTable.STALLInfo() { POINT = perTotalTime * i, TIME = perTotalTime });
                }
            }
            if(qoe.STALLlist!=null && qoe.STALLlist.Count > 0)
            {
                if (qoe.STALLlist.Count >= 1)
                {
                    qoe.STALL_DURATION_LONG_1 = qoe.STALLlist[0].TIME;
                    qoe.STALL_DURATION_LONG_POINT_1 = qoe.STALLlist[0].POINT;
                }
                if (qoe.STALLlist.Count >= 2)
                {
                    qoe.STALL_DURATION_LONG_2 = qoe.STALLlist[1].TIME;
                    qoe.STALL_DURATION_LONG_POINT_2 = qoe.STALLlist[1].POINT;
                }
                if (qoe.STALLlist.Count >= 3)
                {
                    qoe.STALL_DURATION_LONG_3 = qoe.STALLlist[2].TIME;
                    qoe.STALL_DURATION_LONG_POINT_3 = qoe.STALLlist[2].POINT;
                }
                if (qoe.STALLlist.Count >= 4)
                {
                    qoe.STALL_DURATION_LONG_4= qoe.STALLlist[3].TIME;
                    qoe.STALL_DURATION_LONG_POINT_4 = qoe.STALLlist[3].POINT;
                }
                if (qoe.STALLlist.Count >= 5)
                {
                    qoe.STALL_DURATION_LONG_5 = qoe.STALLlist[4].TIME;
                    qoe.STALL_DURATION_LONG_POINT_5 = qoe.STALLlist[4].POINT;
                }
                if (qoe.STALLlist.Count >= 6)
                {
                    qoe.STALL_DURATION_LONG_6 = qoe.STALLlist[5].TIME;
                    qoe.STALL_DURATION_LONG_POINT_6 = qoe.STALLlist[5].POINT;
                }
                if (qoe.STALLlist.Count >= 7)
                {
                    qoe.STALL_DURATION_LONG_7 = qoe.STALLlist[6].TIME;
                    qoe.STALL_DURATION_LONG_POINT_7 = qoe.STALLlist[6].POINT;
                }
                if (qoe.STALLlist.Count >= 8)
                {
                    qoe.STALL_DURATION_LONG_8 = qoe.STALLlist[7].TIME;
                    qoe.STALL_DURATION_LONG_POINT_8 = qoe.STALLlist[7].POINT;
                }
                if (qoe.STALLlist.Count >= 9)
                {
                    qoe.STALL_DURATION_LONG_9 = qoe.STALLlist[8].TIME;
                    qoe.STALL_DURATION_LONG_POINT_9 = qoe.STALLlist[8].POINT;
                }
                if (qoe.STALLlist.Count >=10)
                {
                    qoe.STALL_DURATION_LONG_10 = qoe.STALLlist[9].TIME;
                    qoe.STALL_DURATION_LONG_POINT_10 = qoe.STALLlist[9].POINT;
                }
            }
            if(npi.NeighbourList!=null && npi.NeighbourList.Count > 0)
            {             
                if (npi.NeighbourList.Count >= 1)
                {
                    qoe.ADJ_PCI1 = npi.NeighbourList[0].PCI;
                    qoe.ADJ_RSRP1 = npi.NeighbourList[0].RSRP;
                    qoe.ADJ_EARFCN1 = npi.NeighbourList[0].EARFCN;
                }
                if (npi.NeighbourList.Count >= 2)
                {
                    qoe.ADJ_PCI2 = npi.NeighbourList[1].PCI;
                    qoe.ADJ_RSRP2 = npi.NeighbourList[1].RSRP;
                }
                if (npi.NeighbourList.Count >= 3)
                {
                    qoe.ADJ_PCI3 = npi.NeighbourList[2].PCI;
                    qoe.ADJ_RSRP3 = npi.NeighbourList[2].RSRP;
                }
                if (npi.NeighbourList.Count >= 4)
                {
                    qoe.ADJ_PCI4 = npi.NeighbourList[3].PCI;
                    qoe.ADJ_RSRP4 = npi.NeighbourList[3].RSRP;
                }
                if (npi.NeighbourList.Count >= 5)
                {
                    qoe.ADJ_PCI5 = npi.NeighbourList[4].PCI;
                    qoe.ADJ_RSRP5 = npi.NeighbourList[4].RSRP;
                }
                if (npi.NeighbourList.Count >= 6)
                {
                    qoe.ADJ_PCI6 = npi.NeighbourList[5].PCI;
                    qoe.ADJ_RSRP6= npi.NeighbourList[5].RSRP;
                }
            }
            if(qoe.Network_Typelist != null && qoe.Network_Typelist.Count > 0)
            {
                qoe.NETWORK_SET = JsonConvert.SerializeObject(qoe.Network_Typelist);
            }
            qoe.BDLON = npi.BDlon;
            qoe.BDLAT = npi.BDlat;
            qoe.GDLON = npi.GDlon;
            qoe.GDLAT = npi.GDlat;
            if (qoe.EVMOS > 0)
            {
                qoe.VMOS_MATCH = Math.Round(100 * (1 - Math.Abs(Utils.DoubleNull2Double(qoe.EVMOS) - Utils.DoubleNull2Double(qoe.VMOS))) / 4, 2);
            }
            qoe.PCI = npi.PCI;
            qoe.APKNAME = npi.ApkName;
            qoe.APKVERSION = npi.ApkVersion;
            return qoe;
        }
        private static string GetNetFormat(string net,int earfcn)
        {
            if (string.IsNullOrEmpty(net)) return net;
            if (net.ToLower() != "4g") return net;
            return earfcn < 30000 ? (net + "-FDD") : (net + "-TDD");           
        }
    }
}