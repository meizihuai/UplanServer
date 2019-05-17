using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UplanServer
{
    [Table("QOE_VIDEO_TABLE")]
    public class QoEVideoTable
    {
        [Column("ID")]
        public long ID{ get; set; }
        [Column("DATETIME")]
        public string DATETIME { get; set; }
        [Column("NET_TYPE")]
        public string NET_TYPE { get; set; }
        [Column("BUSINESS_TYPE")]
        public string BUSINESS_TYPE { get; set; }
        [Column("VIDEO_BUFFER_INIT_TIME")]
        public double? VIDEO_BUFFER_INIT_TIME { get; set; }
        [Column("VIDEO_BUFFER_TOTAL_TIME")]
        public double? VIDEO_BUFFER_TOTAL_TIME { get; set; }
        [Column("VIDEO_STALL_NUM")]
        public double? VIDEO_STALL_NUM { get; set; }
        [Column("VIDEO_STALL_TOTAL_TIME")]
        public double? VIDEO_STALL_TOTAL_TIME { get; set; }
        [Column("VIDEO_STALL_DURATION_PROPORTION")]
        public double? VIDEO_STALL_DURATION_PROPORTION { get; set; }
        [Column("VIDEO_LOAD_SCORE")]
        public double? VIDEO_LOAD_SCORE { get; set; }
        [Column("VIDEO_STALL_SCORE")]
        public double? VIDEO_STALL_SCORE { get; set; }
        [Column("VIDEO_BUFFER_TOTAL_SCORE")]
        public double? VIDEO_BUFFER_TOTAL_SCORE { get; set; }
        [Column("ECLATIRY")]
        public double? ECLATIRY { get; set; }
        [Column("VMOS")]
        public double? VMOS { get; set; }
        [Column("PACKET_LOSS")]
        public double? PACKET_LOSS { get; set; }
        [Column("ELOAD")]
        public double? ELOAD { get; set; }
        [Column("ESTALL")]
        public double? ESTALL { get; set; }
        [Column("EVMOS")]
        public double? EVMOS { get; set; }
        [Column("ELIGHT")]
        public double? ELIGHT { get; set; }
        [Column("ESTATE")]
        public double? ESTATE { get; set; }
        [Column("CARRIER")]
        public string CARRIER { get; set; }
        [Column("PLMN")]
        public string PLMN { get; set; }
        [Column("MCC")]
        public string MCC { get; set; }
        [Column("MNC")]
        public string MNC { get; set; }
        [Column("TAC")]
        public string TAC { get; set; }
        [Column("ECI")]
        public double? ECI { get; set; }
        [Column("ENODEBID")]
        public string ENODEBID { get; set; }
        [Column("CELLID")]
        public string CELLID { get; set; }
        [Column("SIGNAL_STRENGTH")]
        public double? SIGNAL_STRENGTH { get; set; }
        [Column("CELL_SIGNAL_STRENGTH")]
        public string CELL_SIGNAL_STRENGTH { get; set; }
        [Column("SINR")]
        public double? SINR { get; set; }
        [Column("PHONE_MODEL")]
        public string PHONE_MODEL { get; set; }
        [Column("OPERATING_SYSTEM")]
        public string OPERATING_SYSTEM { get; set; }
        [Column("UDID")]
        public string UDID { get; set; }
        [Column("IMEI")]
        public string IMEI { get; set; }
        [Column("IMSI")]
        public string IMSI { get; set; }
        [Column("USER_TEL")]
        public string USER_TEL { get; set; }
        [Column("PHONE_PLACE_STATE")]
        public string PHONE_PLACE_STATE { get; set; }
        [Column("COUNTRY")]
        public string COUNTRY { get; set; }
        [Column("PROVINCE")]
        public string PROVINCE { get; set; }
        [Column("CITY")]
        public string CITY { get; set; }
        [Column("ADDRESS")]
        public string ADDRESS { get; set; }
        [Column("PHONE_ELECTRIC_START")]
        public double? PHONE_ELECTRIC_START { get; set; }
        [Column("PHONE_ELECTRIC_END")]
        public double? PHONE_ELECTRIC_END { get; set; }
        [Column("SCREEN_RESOLUTION_LONG")]
        public double? SCREEN_RESOLUTION_LONG { get; set; }
        [Column("SCREEN_RESOLUTION_WIDTH")]
        public double? SCREEN_RESOLUTION_WIDTH { get; set; }
        [Column("LIGHT_INTENSITY")]
        public string LIGHT_INTENSITY { get; set; }
        [Column("LIGHT_INTENSITY_VALUE")]
        public double? LIGHT_INTENSITY_VALUE { get; set; }
        [Column("PHONE_SCREEN_BRIGHTNESS")]
        public string PHONE_SCREEN_BRIGHTNESS { get; set; }
        [Column("PHONE_SCREEN_BRIGHTNESS_VALUE")]
        public double? PHONE_SCREEN_BRIGHTNESS_VALUE { get; set; }
        [Column("HTTP_RESPONSE_TIME")]
        public double? HTTP_RESPONSE_TIME { get; set; }
        [Column("PING_AVG_RTT")]
        public double? PING_AVG_RTT { get; set; }
        [Column("VIDEO_CLARITY")]
        public string VIDEO_CLARITY { get; set; }
        [Column("VIDEO_CODING_FORMAT")]
        public string VIDEO_CODING_FORMAT { get; set; }
        [Column("VIDEO_BITRATE")]
        public double? VIDEO_BITRATE { get; set; }
        [Column("FPS")]
        public double? FPS { get; set; }
        [Column("VIDEO_TOTAL_TIME")]
        public double? VIDEO_TOTAL_TIME { get; set; }
        [Column("VIDEO_PLAY_TOTAL_TIME")]
        public double? VIDEO_PLAY_TOTAL_TIME { get; set; }
        [Column("VIDEO_PEAK_DOWNLOAD_SPEED")]
        public double? VIDEO_PEAK_DOWNLOAD_SPEED { get; set; }
        [Column("APP_PREPARED_TIME")]
        public double? APP_PREPARED_TIME { get; set; }
        [Column("BVRATE")]
        public double? BVRATE { get; set; }
        [Column("STARTTIME")]
        public string STARTTIME { get; set; }
        [Column("FILE_SIZE")]
        public double? FILE_SIZE { get; set; }
        [Column("FILE_NAME")]
        public string FILE_NAME { get; set; }
        [Column("FILE_SERVER_LOCATION")]
        public string FILE_SERVER_LOCATION { get; set; }
        [Column("FILE_SERVER_IP")]
        public string FILE_SERVER_IP { get; set; }
        [Column("UE_INTERNAL_IP")]
        public string UE_INTERNAL_IP { get; set; }
        [Column("ENVIRONMENTAL_NOISE")]
        public double? ENVIRONMENTAL_NOISE { get; set; }
        [Column("ACCELEROMETER_DATA")]
        public string ACCELEROMETER_DATA { get; set; }
        [Column("INSTAN_DOWNLOAD_SPEED")]
        public string INSTAN_DOWNLOAD_SPEED { get; set; }
        [Column("VIDEO_ALL_PEAK_RATE")]
        public string VIDEO_ALL_PEAK_RATE { get; set; }
        [Column("VIDEO_AVERAGE_PEAK_RATE")]
        public double? VIDEO_AVERAGE_PEAK_RATE { get; set; }
        [Column("LONGITUDE_1")]
        public double? LONGITUDE_1 { get; set; }
        [Column("LATITUDE_1")]
        public double? LATITUDE_1 { get; set; }
        [Column("LONGITUDE_2")]
        public double? LONGITUDE_2 { get; set; }
        [Column("LATITUDE_2")]
        public double? LATITUDE_2 { get; set; }
        [Column("LONGITUDE_3")]
        public double? LONGITUDE_3 { get; set; }
        [Column("LATITUDE_3")]
        public double? LATITUDE_3 { get; set; }
        [Column("LONGITUDE_4")]
        public double? LONGITUDE_4 { get; set; }
        [Column("LATITUDE_4")]
        public double? LATITUDE_4 { get; set; }
        [Column("LONGITUDE_5")]
        public double? LONGITUDE_5 { get; set; }
        [Column("LATITUDE_5")]
        public double? LATITUDE_5 { get; set; }
        [Column("SIGNAL_INFO1")]
        public string SIGNAL_INFO1 { get; set; }
        [Column("SIGNAL_INFO2")]
        public string SIGNAL_INFO2 { get; set; }
        [Column("SIGNAL_INFO3")]
        public string SIGNAL_INFO3 { get; set; }
        [Column("SIGNAL_INFO4")]
        public string SIGNAL_INFO4 { get; set; }
        [Column("SIGNAL_INFO5")]
        public string SIGNAL_INFO5 { get; set; }
        [Column("ADJ_PCI1")]
        public double? ADJ_PCI1 { get; set; }
        [Column("ADJ_PCI2")]
        public double? ADJ_PCI2 { get; set; }
        [Column("ADJ_PCI3")]
        public double? ADJ_PCI3 { get; set; }
        [Column("ADJ_PCI4")]
        public double? ADJ_PCI4 { get; set; }
        [Column("ADJ_PCI5")]
        public double? ADJ_PCI5 { get; set; }
        [Column("ADJ_PCI6")]
        public double? ADJ_PCI6 { get; set; }
        [Column("ADJ_RSRP1")]
        public double? ADJ_RSRP1 { get; set; }
        [Column("ADJ_RSRP2")]
        public double? ADJ_RSRP2 { get; set; }
        [Column("ADJ_RSRP3")]
        public double? ADJ_RSRP3 { get; set; }
        [Column("ADJ_RSRP4")]
        public double? ADJ_RSRP4 { get; set; }
        [Column("ADJ_RSRP5")]
        public double? ADJ_RSRP5 { get; set; }
        [Column("ADJ_RSRP6")]
        public double? ADJ_RSRP6 { get; set; }
        [Column("NETWORK_SET")]
        public string NETWORK_SET { get; set; }
        [Column("USERSCENE")]
        public string USERSCENE { get; set; }
        [Column("MOVE_SPEED")]
        public double? MOVE_SPEED { get; set; }
        [Column("ISPLAYCOMPLETED")]
        public double? ISPLAYCOMPLETED { get; set; }
        [Column("LOCALDATASAVETIME")]
        public string LOCALDATASAVETIME { get; set; }
        [Column("ISUPLOADDATATIMELY")]
        public double? ISUPLOADDATATIMELY { get; set; }
        [Column("STALL_DURATION_LONG_1")]
        public double? STALL_DURATION_LONG_1 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_1")]
        public double? STALL_DURATION_LONG_POINT_1 { get; set; }
        [Column("STALL_DURATION_LONG_2")]
        public double? STALL_DURATION_LONG_2 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_2")]
        public double? STALL_DURATION_LONG_POINT_2 { get; set; }
        [Column("STALL_DURATION_LONG_3")]
        public double? STALL_DURATION_LONG_3 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_3")]
        public double? STALL_DURATION_LONG_POINT_3 { get; set; }
        [Column("STALL_DURATION_LONG_4")]
        public double? STALL_DURATION_LONG_4 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_4")]
        public double? STALL_DURATION_LONG_POINT_4 { get; set; }
        [Column("STALL_DURATION_LONG_5")]
        public double? STALL_DURATION_LONG_5 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_5")]
        public double? STALL_DURATION_LONG_POINT_5 { get; set; }
        [Column("STALL_DURATION_LONG_6")]
        public double? STALL_DURATION_LONG_6 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_6")]
        public double? STALL_DURATION_LONG_POINT_6 { get; set; }
        [Column("STALL_DURATION_LONG_7")]
        public double? STALL_DURATION_LONG_7 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_7")]
        public double? STALL_DURATION_LONG_POINT_7 { get; set; }
        [Column("STALL_DURATION_LONG_8")]
        public double? STALL_DURATION_LONG_8 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_8")]
        public double? STALL_DURATION_LONG_POINT_8 { get; set; }
        [Column("STALL_DURATION_LONG_9")]
        public double? STALL_DURATION_LONG_9 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_9")]
        public double? STALL_DURATION_LONG_POINT_9 { get; set; }
        [Column("STALL_DURATION_LONG_10")]
        public double? STALL_DURATION_LONG_10 { get; set; }
        [Column("STALL_DURATION_LONG_POINT_10")]
        public double? STALL_DURATION_LONG_POINT_10 { get; set; }
        [Column("TASKNAME")]
        public string TASKNAME { get; set; }
        [Column("HOLD_1")]
        public string HOLD_1 { get; set; }
        [Column("APKVERSION")]
        public string APKVERSION { get; set; }
        [Column("SATELLITECOUNT")]
        public double? SATELLITECOUNT { get; set; }
        [Column("ISOUTSIDE")]
        public double? ISOUTSIDE { get; set; }
        [Column("DISTRICT")]
        public string DISTRICT { get; set; }
        [Column("BDLON")]
        public double? BDLON { get; set; }
        [Column("BDLAT")]
        public double? BDLAT { get; set; }
        [Column("GDLON")]
        public double? GDLON { get; set; }
        [Column("GDLAT")]
        public double? GDLAT { get; set; }
        [Column("ACCURACY")]
        public double? ACCURACY { get; set; }
        [Column("ALTITUDE")]
        public double? ALTITUDE { get; set; }
        [Column("VMOS_MATCH")]
        public double? VMOS_MATCH { get; set; }
        [Column("PCI")]
        public string PCI { get; set; }
        [Column("APKNAME")]
        public string APKNAME { get; set; }
        [Column("GRID")]
        public double? GRID { get; set; }
        [Column("WIFI_SSID")]
        public string WIFI_SSID { get; set; }
        [Column("WIFI_MAC")]
        public string WIFI_MAC { get; set; }
        [Column("FREQ")]
        public double? FREQ { get; set; }
        [Column("CPU")]
        public string CPU { get; set; }
        [Column("ADJ_SIGNAL")]
        public string ADJ_SIGNAL { get; set; }
        [Column("ADJ_EARFCN1")]
        public double? ADJ_EARFCN1 { get; set; }
        [Column("ISSCREENON")]
        public double? ISSCREENON { get; set; }
        [Column("ISGPSOPEN")]
        public double? ISGPSOPEN { get; set; }
        [Column("SCREENRECORD_FILENAME")]
        public string SCREENRECORD_FILENAME { get; set; }
        [Column("ISSCREENRECORDUPLOADED")]
        public int? ISSCREENRECORDUPLOADED { get; set; }
        [Column("NETWORK_FORMAT")]
        public string NETWORK_FORMAT { get; set; }
    }

}