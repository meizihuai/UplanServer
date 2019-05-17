using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("DEVICETABLE")]
    public class DeviceInfo
    {
        [Column("ID")]
        public int id { get; set; }
        [Column("DATETIME")]
        public string DateTime { get; set; }
        [Column("AID")]
        public string AID { get; set; }
        [Column("USERNAME")]
        public string UserName { get; set; }
        [Column("IMEI")]
        public string IMEI { get; set; }
        [Column("GROUPID")]
        public string GroupId { get; set; }
        [Column("LASTDATETIME")]
        public string LastDateTime { get; set; }
        [Column("ISBUSY")]
        public int IsBusy { get; set; }
        [Column("PROVINCE")]
        public string Province { get; set; }
        [Column("CITY")]
        public string City { get; set; }
        [Column("DISTRICT")]
        public string District { get; set; }
        [Column("LON")]
        public decimal Lon { get; set; }
        [Column("LAT")]
        public decimal Lat { get; set; }
        [Column("BDLON")]
        public decimal BDLon { get; set; }
        [Column("BDLAT")]
        public decimal BDLat { get; set; }
        [Column("GDLON")]
        public decimal GDLon { get; set; }
        [Column("GDLAT")]
        public decimal GDLat { get; set; }
        [Column("POWER")]
        public int Power { get; set; }
        [Column("PHONEMODEL")]
        public string PhoneModel { get; set; }
        [Column("APKVERSION")]
        public string ApkVersion { get; set; }
        [Column("IMSI")]
        public string IMSI { get; set; }      
        [Column("ISONLINE")]
        public int IsOnline { get; set; }
        [Column("SYSTEM")]
        public string System { get; set; }
        [Column("SYSVERSION")]
        public string SysVersion { get; set; }
        [Column("UUID")]
        public string UUID { get; set; }
    }

}