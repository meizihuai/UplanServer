using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UplanServer
{
    public class QoEVideoScore
    {
        public string GUID { get; set; }
        //EVMOS，ELOAD，EFLUENCY
        public int EVMOS { get; set; }
        public int ELOAD { get; set; }
        public int EFLUENCY { get; set; }
    }
}