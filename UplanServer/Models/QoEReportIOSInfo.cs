using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    /// <summary>
    /// iOS版本 QoER数据结构
    /// </summary>
    [Table("QOE_REPORT_IOS_TABLE")]
    public class QoEReportIOSInfo
    {
        /// <summary>
        /// - 主键ID
        /// </summary>
        [Column("ID")]
        public int id { get; set; }
        /// <summary>
        /// - 入库时间
        /// </summary>
        [Column("DATETIME")]
        public DateTime DateTime { get; set; }
        /// <summary>
        /// * 系统 AID
        /// </summary>
        [Column("AID")]
        public string AID { get; set; }
        /// <summary>
        /// - UUID
        /// </summary>
        [Column("UUID")]
        public string UUID { get; set; }
        /// <summary>
        /// - 手机型号
        /// </summary>
        [Column("PHONEMODEL")]
        public string PhoneModel { get; set; }
        /// <summary>
        /// 网络类型
        /// </summary>
        [Column("NETTYPE")]
        public string NetType { get; set; }
        /// <summary>
        /// * 运营商
        /// </summary>
        [Column("CARRIER")]
        public string Carrier { get; set; }
        /// <summary>
        /// * 经度
        /// </summary>
        [Column("LON")]
        public double Lon { get; set; }
        /// <summary>
        /// * 纬度
        /// </summary>
        [Column("LAT")]
        public double Lat { get; set; }
        /// <summary>
        /// - 百度经度
        /// </summary>
        [Column("BDLON")]
        public double BDLon { get; set; }
        /// <summary>
        /// - 百度纬度
        /// </summary>
        [Column("BDLAT")]
        public double BDLat { get; set; }
        /// <summary>
        /// - 高德经度
        /// </summary>
        [Column("GDLON")]
        public double GDLon { get; set; }
        /// <summary>
        /// - 高德纬度
        /// </summary>
        [Column("GDLAT")]
        public double GDLat { get; set; }
        /// <summary>
        /// - 省
        /// </summary>
        [Column("PROVINCE")]
        public string Province { get; set; }
        /// <summary>
        /// - 市
        /// </summary>
        [Column("CITY")]
        public string City { get; set; }
        /// <summary>
        /// - 区
        /// </summary>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// - 详细地址
        /// </summary>
        [Column("ADDRESS")]
        public string Address { get; set; }
        /// <summary>
        /// * 移动速度
        /// </summary>
        [Column("GPSSPEED")]
        public float GPSSpeed { get; set; }
        /// <summary>
        /// * 三轴加速度
        /// </summary>
        [Column("XYZSPEED")]
        public string XZYSpeed { get; set; }
        /// <summary>
        /// * 信号强度
        /// </summary>
        [Column("SIGNALSTRENGTH")]
        public int SignalStrength { get; set; }
        /// <summary>
        /// * 业务类型
        /// </summary>
        [Column("BUSINESSTYPE")]
        public string BusinessType { get; set; }
        /// <summary>
        /// * 系统版本
        /// </summary>
        [Column("IOSVERSION")]
        public string IOSVersion { get; set; }
        /// <summary>
        /// * APP版本号
        /// </summary>
        [Column("APPVERSION")]
        public string APPVersion { get; set; }
        /// <summary>
        /// * SDK环境版本号
        /// </summary>
        [Column("SDKVERSION")]
        public string SDKVersion { get; set; }
        /// <summary>
        /// * APP名称， 例如：UniQoE 或者 10086
        /// </summary>
        [Column("APPNAME")]
        public string APPName { get; set; }

    }
}