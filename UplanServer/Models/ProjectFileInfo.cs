using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("PROJECTFILETABLE")]
    public class ProjectFileInfo
    {
        /// <summary>
        /// - 主键
        /// </summary>
        [Column("ID")]
        public int id { get; set; }
        /// <summary>
        /// - 入库时间
        /// </summary>
        [Column("DATETIME")]
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        [Column("DAY")]
       public string Day { get; set; }
        /// <summary>
        /// * 用户账号
        /// </summary>
        [Column("ACCOUNT")]
        public string Account { get; set; }
        /// <summary>
        /// * 文件名
        /// </summary>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// - 文件扩展名(后台自动识别)
        /// </summary>
        [Column("FILEEXT")]
        public string FileExt { get; set; }
        /// <summary>
        /// * 文件分类
        /// </summary>
        [Column("TYPE")]
        public string Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("MARK")]
        public string Mark { get; set; }
        /// <summary>
        /// * 是否公开 0否 / 1是
        /// </summary>
        [Column("ISPUBLIC")]
        public int IsPublic { get; set; }
        /// <summary>
        /// - 文件URL(上传后后台生成)
        /// </summary>
        [Column("URL")]
        public string Url { get; set; }
        /// <summary>
        /// * 文件二进制数据，base64传输
        /// </summary>
        [NotMapped]
        public string FileBase64 { get; set; }
    }
}