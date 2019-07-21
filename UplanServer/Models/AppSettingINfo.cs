using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UplanServer
{
    public class AppSettingInfo
    {
        public string MQ_HostName { get; set; }
        public string MQ_UserName { get; set; }
        public string MQ_Password { get; set; }
        public string MQ_Queue_QoER2DB { get; set; }
        public string MQ_Queue_QoERForiOS2DB { get; set; }
        public string MQ_Queue_QoE2DB { get; set; }
    }
}