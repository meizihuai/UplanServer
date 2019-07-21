using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace UplanServer.Controllers
{
    public class IPController : ApiController
    {
       [HttpGet]
       public async Task<NormalResponse> GetLocation(string ip="")
        {
            if (string.IsNullOrEmpty(ip))
            {
                if (System.Web.HttpContext.Current == null)
                {
                    return new NormalResponse(false, "System.Web.HttpContext.Current is null");
                }
                ip = IPHelper.GetIP(System.Web.HttpContext.Current.Request);
            }
            return new NormalResponse(true, "", "", await Module.LocationService.GetLocationByIp(ip));
        }
    }
}
