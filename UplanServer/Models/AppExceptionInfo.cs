using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("SYS_APP_EXCEPTION")]
    public class AppExceptionInfo
    {
        /// <summary>
        /// - 主键ID
        /// </summary>
        [Column("ID")]
        public int ID { get; set; }
        /// <summary>
        /// - 入库时间
        /// </summary>
        [Column("DATETIME")]
        public string DateTime { get; set; }
        /// <summary>
        /// * AID
        /// </summary>
        [Column("AID")]
        public string AID { get; set; }
        /// <summary>
        /// * 操作系统类型，ios/android
        /// </summary>
        [Column("OSTYPE")]
        public string OSType { get; set; }
        /// <summary>
        /// * App名称，可为包名
        /// </summary>
        [Column("APPNAME")]
        public string AppName { get; set; }
        /// <summary>
        /// SDK名称(如无SDK，可忽略)
        /// </summary>
        [Column("SDKNAME")]
        public string SDKName { get; set; }
        /// <summary>
        /// * 操作系统版本号
        /// </summary>
        [Column("OSVERSION")]
        public string OSVersion { get; set; }
        /// <summary>
        /// * APP版本号，VersionName
        /// </summary>
        [Column("APPVERSION")]
        public string AppVersion { get; set; }
        /// <summary>
        /// SDK版本号(如无SDK，可忽略)
        /// </summary>
        [Column("SDKVERSION")]
        public string SDKVersion { get; set; }
        /// <summary>
        /// * 错误日志的TAG，用于过滤
        /// </summary>
        [Column("TAG")]
        public string TAG { get; set; }
        /// <summary>
        /// 错误日志的等级 0-9 默认 0
        /// </summary>
        [Column("EXPLEVEL")]
        public int ExpLevel { get; set; }
        /// <summary>
        /// * 错误日志的主体内容
        /// </summary>
        [Column("BODY")]
        public string Body { get; set; }
    }
}