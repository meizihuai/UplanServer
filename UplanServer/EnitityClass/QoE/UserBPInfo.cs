using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("USER_BP_TABLE")]
    public class UserBPInfo
    {
        [Column("ID")]
        public long id { get; set; }
        [Column("DATETIME")]
        public string DateTime { get; set; }
        [Column("USERNAME")]
        public string UserName { get; set; }
        [Column("IMSI")]
        public string IMSI { get; set; }
        [Column("IMEI")]
        public string IMEI { get; set; }
        [Column("LASTDATETIME")]
        public string LastDateTime { get; set; }
        [Column("BONUSPOINTS")]
        public float BonusPoints { get; set; }
        [Column("EVMOS")]
        public int EVMOS { get; set; }
        [Column("VMOS")]
        public int VMOS { get; set; }
        [Column("LASTBONUSPOINTS")]
        public float LASTBONUSPOINTS { get; set; }
        [Column("LASTVIDEOURL")]
        public string LastVideoUrl { get; set; }
        [Column("LASTASKVIDEOURL")]
        public string LastAskVideoUrl { get; set; }
        [Column("VIDEOURLS")]
        public string VideoUrls { get; set; }
        [Column("QOE_TOTAL_TIME")]
        public long QoE_Total_Time { get; set; }
        [Column("QOE_TOTAL_E_TIME")]
        public long QOE_TOTAL_E_TIME { get; set; }
        [Column("QOE_TODAY_TIME")]
        public long QOE_TODAY_TIME { get; set; }
        [Column("QOE_TODAY_E_TIME")]
        public long QOE_TODAY_E_TIME { get; set; }
        [Column("LASTDAY")]
        public string LastDay { get; set; }
        [Column("AID")]
        public string AID { get; set; }
        [Column("LASTASKVIDEO_ID")]
        public long LastAskVideo_ID { get; set; }
        [Column("LASTASKVIDEOTIME")]
        public string LastAskVideoTime { get; set; }
        [Column("ISPLAYINGVIDEO")]
        public int IsPlayingVideo { get; set; }
    }

}