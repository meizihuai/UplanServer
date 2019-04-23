using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UplanServer
{
    public class LocationAddressInfo
    {
        public string Province;
        public string City;
        public string District;
        public string DetailAddress;

        public static LocationAddressInfo GetAddressByLngLat(double lng, double lat)
        {
            string miYue = "5cW7OlxZXThtbkq1Y0u5yNO6";
            // miYue = "LwK1QyeofWEysKHf2sdySVcbnhXLnLIj"
            miYue = "tb3xO9tSnOggEciyQBO03vEUydIvdTsY";
            miYue = "wdM2ngn7AIV9dRPpQYqslRwNkiPbFhfh"; // 国伟的，认证过，一天30万
            string url = "http://api.map.baidu.com/geocoder/v2/?ak=" + miYue + "&location=" + lat + "," + lng + "&output=json&pois=0&coordtype=wgs84ll";
            string msg =HTTPHelper.GetH(url, "");
            LocationAddressInfo la = new LocationAddressInfo();
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
            return la;
        }
    }

}