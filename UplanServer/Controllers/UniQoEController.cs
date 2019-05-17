using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        /// 手机登录，获取AID
        /// </summary>
        /// <param name="phoneId">* 手机唯一识别号，android用imsi,iPhone用uuid</param>
        /// <param name="system">操作系统，Android 或 iOS</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse Login(string phoneId,string system="Android")
        {
            if (phoneId == "") return new NormalResponse(false, "phoneId不可为空");
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
        /// iOS终端上传QoER数据
        /// </summary>
        /// <param name="qoer">QoER 结构体</param>
        /// <returns></returns>
        public NormalResponse UploadQoERDataForiOS(QoEReportIOSInfo qoer)
        {
            try
            {
                if (qoer.AID == "") return new NormalResponse(false, "AID不可为空");
                if (qoer.BusinessType==null || qoer.BusinessType == "") qoer.BusinessType = "QOER";
                 qoer.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
               // qoer.DateTime = DateTime.Now;
                if (qoer.Lon > 0 && qoer.Lat > 0)
                {
                    CoordInfo gds = CoordInfo.GetGDCoord(qoer.Lon, qoer.Lat);
                    if (gds != null)
                    {
                        qoer.GDLon = gds.x;
                        qoer.GDLat = gds.y;
                    }
                    LocationAddressInfo la = LocationAddressInfo.GetAddressByLngLat(qoer.Lon, qoer.Lat);
                    if (la != null)
                    {
                        qoer.Province = la.Province;
                        qoer.City = la.City;
                        qoer.District = la.District;
                        qoer.Address = la.DetailAddress;
                    }
                }
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
            catch(Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }          
        }

    }
}
