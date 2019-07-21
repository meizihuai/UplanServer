using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("SYS_AUTOBUS_TABLE")]
    public class SysAutoBusInfo
    {
        [Column("ID")]
        public int ID{get;set;}
        [Column("DATETIME")]
        public string DateTime{get;set;}
        [Column("BUSNAME")]
        public string BusName{get;set;}     
        [Column("STARTLOCATION")]
        public string StartLocation { get; set; }
        [Column("ENDLOCATION")]
        public string EndLocation{get;set;}
        [Column("IMEI_CM")]
        public string Imei_cm { get; set; }
        [Column("IMEI_CU")]
        public string Imei_cu{get;set;}
        [Column("IMEI_CT")]
        public string Imei_ct{get;set;}
        [Column("STARTTIME")]
        public string StartTime{get;set;}
        [Column("ENDTIME")]
        public string EndTime{get;set;}     
        [Column("LASTDATETIME")]
        public string LastDateTime { get; set; }
        [Column("GDLON")]
        public double GDLon { get; set; }
        [Column("GDLAT")]
        public double GDLat { get; set; }
        [Column("RSRP")]
        public double RSRP { get; set; }
        [Column("SINR")]
        public double SINR { get; set; }
        [Column("RTT")]
        public double RTT { get; set; }
        [Column("SPEED")]
        public double Speed { get; set; }
        [Column("CURRENTQOERID")]
        public int CurrentQoERId { get; set; }
        [Column("VMOS")]
        public double VMOS { get; set; }
        [Column("NETWORK")]
        public string Network { get; set; }
        [Column("RUNTIMES")]
        public int RunTimes { get; set; }
        [Column("STARTQOERID")]
        public int StartQoERId { get; set; }
        [Column("ENDQOERID")]
        public int EndQoERId { get; set; }
        [Column("MIDQOERID")]
        public int MidQoERId { get; set; }
        [Column("TOTALCOUNT")]
        public int TotalCount { get; set; }
        [Column("BRAKECOUNT")]
        public int BrakeCount { get; set; }
        [Column("SHAKECOUNT")]
        public int ShakeCount { get; set; }
        [Column("ASPEED")]
        public double ASpeed { get; set; }
        [NotMapped]
        public double AvgVMOS{ get; set; }
    }
}