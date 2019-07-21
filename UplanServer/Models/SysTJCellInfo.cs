using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("SYS_TJ_CELLINFO_T")]
    public class SysTJCellInfo
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("ECI")]
        public int ECI { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
    }
}