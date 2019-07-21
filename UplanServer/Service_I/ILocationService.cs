using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UplanServer
{
  public  interface ILocationService
    {
        Task<LocationInfo> GetLocationByIp(string ip);
        Task<LocationInfo> GetLocationByGps(double lng, double lat);
    }
}
