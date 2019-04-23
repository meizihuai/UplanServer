using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ConvertGPS;

namespace UplanServer
{
    public class CoordInfo
    {
        public double x;
        public double y;
        public CoordInfo(double x,double y)
        {
            this.x = x;
            this.y = y;
        }
        public static CoordInfo GetBDCoord(double lon,double lat)
        {
            PointLatLng p = new PointLatLng(lat, lon);
            p = Gps84_To_bd09(p);
            return new CoordInfo(p.Lng, p.Lat);
        }
        public static CoordInfo GetGDCoord(double lon, double lat)
        {
            PointLatLng p = new PointLatLng(lat, lon);
            p = gps84_To_Gcj02(p);
            return new CoordInfo(p.Lng, p.Lat);
        }
    }
}