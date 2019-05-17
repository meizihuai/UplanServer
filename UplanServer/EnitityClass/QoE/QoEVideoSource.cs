using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("QOE_VIDEO_SOURCE")]
    public class QoEVideoSource
    {
        [Column("ID")]
        public long id { get; set; }
        [Column("DATETIME")]
        public string DateTime { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("URL")]
        public string Url { get; set; }
        [Column("FILESIZE")]
        public long FileSize { get; set; }
        [Column("VIDEOSECOND")]
        public long VideoSecond { get; set; }
        [Column("FILESIZEM")]
        public long FileSizeM { get; set; }
        [Column("TYPE")]
        public string Type { get; set; }
        [Column("ISUSE")]
        public int IsUse { get; set; }
        [Column("MOVIENAME")]
        public string MovieName { get; set; }
        [Column("MOVIEINDEX")]
        public long MovieIndex { get; set; }
        [Column("VIDEO_CLARITY")]
        public string Video_Clarity { get; set; }
        [Column("ISRAND")]
        public int IsRand { get; set; }
        [Column("SECONDGRAD")]
        public string SecondGrad { get; set; }
        [Column("CODETYPE")]
        public string CodeType { get; set; }
        [Column("CLARITYSIZE")]
        public string ClaritySize { get; set; }
        [Column("FRAMERATE")]
        public double? FrameRate { get; set; }
    }

}