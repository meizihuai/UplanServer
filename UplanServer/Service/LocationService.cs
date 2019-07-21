using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UplanServer
{
    public class LocationService:ILocationService
    {
        public async Task<LocationInfo> GetLocationByGps(double lng, double lat)
        {
            string ak = "5cW7OlxZXThtbkq1Y0u5yNO6";
            // miYue = "LwK1QyeofWEysKHf2sdySVcbnhXLnLIj"
            //   ak = "tb3xO9tSnOggEciyQBO03vEUydIvdTsY";
            ak = "wdM2ngn7AIV9dRPpQYqslRwNkiPbFhfh"; // 国伟的，认证过，一天30万
            //bd09ll、bd09mc、gcj02、wgs84
            string url = $"http://api.map.baidu.com/geocoder/v2/?ak={ak}&location={lat},{lng}&output=json&pois=0&coordtype=bd09ll";
            string msg = await HttpHelper.Get(url);
            LocationInfo la = new LocationInfo();
            try
            {
                JObject p = (JObject)JsonConvert.DeserializeObject(msg);
                string province = p["result"]["addressComponent"]["province"].ToString();
                string city = p["result"]["addressComponent"]["city"].ToString();
                string district = p["result"]["addressComponent"]["district"].ToString();
                string street = p["result"]["addressComponent"]["street"].ToString();
                string sematic_description = p["result"]["sematic_description"].ToString();
                string formatted_address = p["result"]["formatted_address"].ToString();
                string business = p["result"]["business"].ToString();
                string str = city + "," + district + "," + sematic_description + "," + business;
                str = formatted_address + "," + sematic_description + "," + business;
                la.Province = province;
                la.City = city;
                la.DetailAddress = str;
                la.District = district;
                return la;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<LocationInfo> GetLocationByIp(string ip)
        {
            try
            {
                if (ip.Contains(":")) ip = ip.Split(':')[0];
                string ak = "wdM2ngn7AIV9dRPpQYqslRwNkiPbFhfh";
                string url = $"http://api.map.baidu.com/location/ip?ip={ip}&ak={ak}&coor=bd09ll";
                string msg = await HttpHelper.Get(url);
                LocationInfo la = new LocationInfo();
                JObject p = (JObject)JsonConvert.DeserializeObject(msg);
                int status = int.Parse(p["status"].ToString());
                if (status != 0) return null;
                la.DetailAddress = p["content"]["address"].ToString();
                la.Province = p["content"]["address_detail"]["province"].ToString();
                la.City = p["content"]["address_detail"]["city"].ToString();
                la.District = p["content"]["address_detail"]["district"].ToString();
                if (string.IsNullOrEmpty(la.DetailAddress))
                {
                    la.DetailAddress = $"{ la.Province}{ la.City }{ la.District }";
                }
                return la;
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}