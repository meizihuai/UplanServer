using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("QOE_BLACKPOINT_TABLE")]
    public class QoEBlackPoint
    {
        [Column("ID")]
        public int ID{get;set;}
        [Column("DATETIME")]
        public string DateTime{get;set;}
        [Column("AID")]
        public string AID{get;set;}
        [NotMapped]
        public PhoneInfo Pi{get;set;}
        [Column("LON")]
        public double Lon { get; set; }
        [Column("LAT")]
        public double Lat { get; set; }
        [Column("GDLON")]
        public double GDLon { get; set; }
        [Column("GDLAT")]
        public double GDLat { get; set; }
        [Column("PROVINCE")]
        public string Privince { get; set; }
        [Column("CITY")]
        public string City { get; set; }
        [Column("DISTRICT")]
        public string District { get; set; }
        [Column("PIRID")]
        public string PiRID { get; set; }
        [Column("VIDEOURL")]
        public string VideoUrl{get;set;}
        [Column("TYPE")]
        public string Type{get;set;}
        [Column("MARK")]
        public string Mark{get;set;}
        
    }
}