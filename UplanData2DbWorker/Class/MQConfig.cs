using System;
using System.Collections.Generic;
using System.Text;

namespace UplanData2DbWorker
{
   public static  class MQConfig
    {
        public static string HostName = "localhost";
        public static string UserName = "guest";
        public static string Password = "guest";
        public static string MQ_Queue_QoERForiOS2DB= "MQ_Queue_QoERForiOS2DB";
        public static string MQ_Queue_QoER2DB = "MQ_Queue_QoER2DB";
        public static string MQ_Queue_QoE2DB = "MQ_Queue_QoE2DB";
    }
}
