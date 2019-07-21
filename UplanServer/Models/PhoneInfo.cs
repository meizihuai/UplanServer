using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using UplanServer;

[Table("QOE_REPORT_TABLE")]
public class PhoneInfo
{
    [Column("ID")]
    public int ID { get; set; }
    [Column("AID")]
    public string AID { get; set; }
    [Column("RID")]
    public string RID { get; set; }
    [Column("DATETIME")]
    public string DateTime { get; set; }
    [Column("BUSINESSTYPE")]
    public string BusinessType { get; set; }
    [Column("APKNAME")]
    public string ApkName { get; set; }
    [Column("PHONEMODEL")]
    public string PhoneModel { get; set; }
    [Column("PHONENAME")]
    public string PhoneName { get; set; }
    [Column("PHONEOS")]
    public string PhoneOS { get; set; }
    [Column("PHONEPRODUCT")]
    public string PhoneProduct { get; set; }
    [Column("CARRIER")]
    public string Carrier { get; set; }
    [Column("IMSI")]
    public string IMSI { get; set; }
    [Column("IMEI")]
    public string IMEI { get; set; }
    [Column("RSRP")]
    public double RSRP { get; set; }
    [Column("SINR")]
    public double SINR { get; set; }
    [Column("RSRQ")]
    public double RSRQ { get; set; }
    [Column("TAC")]
    public int TAC { get; set; }
    [Column("PCI")]
    public int? PCI { get; set; }
    [Column("EARFCN")]
    public int? EarFcn { get; set; }
    [Column("CI")]
    public int CI { get; set; }
    [Column("ENODEBID")]
    public int ENodeBId { get; set; }
    [Column("CELLID")]
    public int CellId { get; set; }
    [Column("NETTYPE")]
    public string NetType { get; set; }
    [Column("SIGNALTYPE")]
    public string SigNalType { get; set; }
    [Column("SIGNALINFO")]
    public string SigNalInfo { get; set; }
    [Column("LON")]
    public double? Lon { get; set; }
    [Column("LAT")]
    public double? Lat { get; set; }


    [NotMapped]
    public string Lon_Encry { get; set; }
    [NotMapped]
    public string Lat_Encry { get; set; }


    [Column("ACCURACY")]
    public double Accuracy { get; set; }
    [Column("ALTITUDE")]
    public double Altitude { get; set; }
    [Column("GPSSPEED")]
    public double GpsSpeed { get; set; }
    [Column("SATELLITECOUNT")]
    public int? SatelliteCount { get; set; }
    [Column("ADDRESS")]
    public string Address { get; set; }
    [Column("APKVERSION")]
    public string ApkVersion { get; set; }
    [Column("ISUPLOADDATATIMELY")]
    public int? IsUploadDataTimely { get; set; }
    [Column("DAY")]
    public string Day { get; set; }
    [Column("MNC")]
    public string MNC { get; set; }
    [Column("WIFI_SSID")]
    public string WiFi_SSID { get; set; }
    [Column("WIFI_MAC")]
    public string WiFi_MAC { get; set; }
    [Column("PING_AVG_RTT")]
    public int? Ping_Avg_Rtt { get; set; }
    [Column("FREQ")]
    public int? Freq { get; set; }
    [Column("CPU")]
    public string CPU { get; set; }
    [Column("ADJ_SIGNAL")]
    public string Adj_Signal { get; set; }
    [NotMapped]
    public List<Neighbour> NeighbourList { get; set; }
    [Column("ADJ_PCI1")]
    public int? Adj_PCI1 { get; set; }
    [Column("ADJ_RSRP1")]
    public int? Adj_RSRP1 { get; set; }
    [Column("ADJ_EARFCN1")]
    public int? ADJ_EARFCN1 { get; set; }
    [Column("ISSCREENON")]
    public int? IsScreenOn { get; set; }
    [Column("ISGPSOPEN")]
    public int? IsGPSOpen { get; set; }
    [Column("ISOUTSIDE")]
    public int? IsOutSide { get; set; }
    [Column("ENODEBID_CELLID")]
    public string EnodebId_CellId { get; set; }
    [Column("PHONE_ELECTRIC")]
    public int? Phone_Electric { get; set; }
    [Column("PHONE_SCREEN_BRIGHTNESS")]
    public int? Phone_Screen_Brightness { get; set; }
    [NotMapped]
    public XYZaSpeedInfo XyZaSpeed { get; set; }
    [Column("XYZASPEED")]
    public string XYZASpeedString { get; set; }
    [Column("PROVINCE")]
    public string Province { get; set; }
    [Column("CITY")]
    public string City { get; set; }
    [Column("DISTRICT")]
    public string District { get; set; }
    [Column("BDLON")]
    public double? BDlon { get; set; }
    [Column("BDLAT")]
    public double? BDlat { get; set; }
    [Column("GDLON")]
    public double? GDlon { get; set; }
    [Column("GDLAT")]
    public double? GDlat { get; set; }
    [Column("VMOS")]
    public int? VMOS { get; set; }
    [Column("HTTP_URL")]
    public string HTTP_URL { get; set; }
    [Column("HTTP_RESPONSE_TIME")]
    public long? Http_Response_Time { get; set; }
    [Column("HTTP_BUFFERSIZE")]
    public long? HTTP_BufferSize { get; set; }
    [Column("IP")]
    public string IP { get; set; }

    /// <summary>
    /// 数据解密，AES算法，目前主要是经纬度解密
    /// </summary>
    public void Decode()
    {
        if (!string.IsNullOrEmpty(this.Lon_Encry) && !string.IsNullOrEmpty(this.Lat_Encry))
        {
            string lonStr = AESHelper.Decode(this.Lon_Encry);
            string latStr = AESHelper.Decode(this.Lat_Encry);
            if (Utils.IsNumberic(lonStr) && Utils.IsNumberic(latStr))
            {
                this.Lon = double.Parse(lonStr);
                this.Lat = double.Parse(latStr);
            }
        }
    }
}
