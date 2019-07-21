using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace UplanServer.Controllers
{
    public class Data2DbController : ApiController
    {
        /// <summary>
        /// QoER数据入库
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse> UploadQoER(PhoneInfo pi)
        {
            return QoEDataToDbHelper.UploadQoER(pi, true);
        }
        /// <summary>
        /// QoER For iOS数据入库
        /// </summary>
        /// <param name="qoer"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse> UploadQoERForiOS(QoEReportIOSInfo qoer)
        {
            return QoEDataToDbHelper.UploadQoERForiOS(qoer);
        }
        /// <summary>
        /// QoE数据入库
        /// </summary>
        /// <param name="qoe"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse>UploadQoEVideo(QoEVideoTable qoe)
        {
            return QoEDataToDbHelper.UploadQoE(qoe);
        }
    }
}
