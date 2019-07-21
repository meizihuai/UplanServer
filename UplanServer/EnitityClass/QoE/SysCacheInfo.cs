using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("SYS_CACHE_TABLE")]
    public class SysCacheInfo
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("DATETIME")]
        public string DateTime { get; set; }
        [Column("KEY")]
        public string Key { get; set; }
        [Column("VALUE")]
        public string Value { get; set; }
        [Column("DATALEN")]
        public long Datalen { get; set; }
    }
}