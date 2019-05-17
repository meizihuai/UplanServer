using System;
using System.Collections.Generic;
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

public class QoEVideoInfo
{
    public int id { get; set; }
    public string DATETIME{ get; set; }    // /*数据的入库时间*/
    public string NET_TYPE{ get; set; }    // netType
    public string BUSINESS_TYPE{ get; set; }    // 业务类型，Ex：流媒体、直播、FTP
    public long VIDEO_BUFFER_INIT_TIME{ get; set; }    // 初始缓冲时延
    public long VIDEO_BUFFER_TOTAL_TIME{ get; set; }    // 视频总的下载时长
    public int VIDEO_STALL_NUM{ get; set; }    // 卡顿次数
    public long VIDEO_STALL_TOTAL_TIME{ get; set; }    // 卡顿总时长
    public double VIDEO_STALL_DURATION_PROPORTION{ get; set; }    // 卡顿时延占比
    public int VIDEO_LOAD_SCORE{ get; set; }    // 初始加载评分
    public int VIDEO_STALL_SCORE{ get; set; }    // 卡顿评分
    public int VIDEO_BUFFER_TOTAL_SCORE{ get; set; }    // 缓冲时延评分
    public int USER_SCORE{ get; set; }    // old清晰度  已弃用
    public int VMOS{ get; set; }    // VMOS
    public double VMOS_MATCH{ get; set; }  // 自动打分和手动打分的契合度
    public long PACKET_LOSS{ get; set; }    // 丢包数
    public int ECLATIRY{ get; set; } // 清晰度
    public int ELOAD{ get; set; }    // /*用户对视频播放等待时间的评分(5：无法察觉到缓冲，4：缓冲时间很短，3：缓冲时间长度一般，2：缓冲时间较长，1：缓冲时间过长无法容忍)*/
    public int ESTALL{ get; set; }    // /*用户对流畅度的评分(5:毫无卡顿，4：略有卡顿但不影响观看，3：有卡顿对观看造成一定影响，2：有卡顿对观看造成较大影响，1：卡顿过多无法容忍)*/
    public int EVMOS{ get; set; }    // /*用户对整体视频服务的综合评分(5:非常好，4：良好，3：一般，2：较差，1：无法容忍)*/
    public int ELIGHT{ get; set; }    // /*环境光照对视频观看的影响程度(5：无影响，4：较小影响，3：有一定影响，2：较大影响，1：极大影响）*/
    public int ESTATE{ get; set; }    // /*用户对运动状态的反馈(:4：静止不动，3：偶尔走动，2：持续走动，1：交通工具上)*/
    public string CARRIER{ get; set; }    // /*运营商名称*/
    public int PLMN{ get; set; }    // /*公共陆地移动网络*/
    public int MCC{ get; set; }    // /*移动国家码*/
    public int MNC{ get; set; }    // /*移动网络号码*/
    public int TAC{ get; set; }    // tac
    public int ECI{ get; set; }    // ECI
    public int ENODEBID{ get; set; }    // enodebid
    public int CELLID{ get; set; }    // cellid
    public int SIGNAL_STRENGTH{ get; set; }    // RSRP
    public int SINR{ get; set; }    // SINR
    public string PHONE_MODEL{ get; set; }    // /*手机型号*/
    public string OPERATING_SYSTEM{ get; set; }    // /*操作系统*/
    public string UDID{ get; set; }    // /*移动设备国际身份码*/
    public string IMEI{ get; set; }    // IMEI
    public string IMSI{ get; set; }    // /*国际移动用户识别码*/
    public string USER_TEL{ get; set; }    // 用户号码
    public int PHONE_PLACE_STATE{ get; set; }    // /*手机放置状态,1表示竖屏,2表示横屏*/
    public string COUNTRY{ get; set; }    // /*国家/地区*/
    public string PROVINCE{ get; set; }    // /*省份*/
    public string CITY{ get; set; }    // /*城市*/
    public string ADDRESS{ get; set; }    // /*地址*/
    public int PHONE_ELECTRIC_START{ get; set; }    // /*开始播放时的手机电量百分比*/
    public int PHONE_ELECTRIC_END{ get; set; }    // /*播放结束时的手机电量百分比*/
    public int SCREEN_RESOLUTION_LONG{ get; set; }    // /*屏幕分辨率(长)*/
    public int SCREEN_RESOLUTION_WIDTH{ get; set; }    // /*屏幕分辨率(宽)*/
    public int LIGHT_INTENSITY{ get; set; }    // /*手机环境光照强度*/
    public int PHONE_SCREEN_BRIGHTNESS{ get; set; }    // /*手机屏幕亮度*/
    public long HTTP_RESPONSE_TIME{ get; set; }    // http响应时间
    public long PING_AVG_RTT{ get; set; }    // /*Ping
    public string VIDEO_CLARITY{ get; set; }    // 视频清晰度
    public string VIDEO_CODING_FORMAT{ get; set; }    // /*视频编码格式,如h.264*/
    public int VIDEO_BITRATE{ get; set; }    // 视频比特率
    public int FPS{ get; set; }    // 帧率
    public long VIDEO_TOTAL_TIME{ get; set; }    // 视频总时长
    public long VIDEO_PLAY_TOTAL_TIME{ get; set; }    // /*视频播放时长=结束播放的时间点-点击播放的时间点(秒)*/
    public long VIDEO_PEAK_DOWNLOAD_SPEED{ get; set; }    // /*初始缓冲阶段的峰值速率，单位kb/s*/
    public long APP_PREPARED_TIME{ get; set; }    // 手机UI加载播放器插件的准备工作时间
    public double BVRATE{ get; set; }    // BVRate
    public string STARTTIME{ get; set; }    // /*视频开始播放的时间*/
    public long FILE_SIZE{ get; set; }    // 文件大小
    public string FILE_NAME{ get; set; }    // 文件名称
    public string FILE_SERVER_LOCATION{ get; set; }    // /*视频源服务器的实际地理位置*/
    public string FILE_SERVER_IP{ get; set; }    // 服务器IP
    public string UE_INTERNAL_IP{ get; set; }    // UE
    public string ENVIRONMENTAL_NOISE{ get; set; }    // 环境噪声
    public long VIDEO_AVERAGE_PEAK_RATE{ get; set; }    // /*视频平均下载速率=总下载量/视频播放时长(kb/s)*/
    public List<int> CELL_SIGNAL_STRENGTHList{ get; set; }    // 按0.5s采集一次，保存后集中上报
    public List<XYZaSpeedInfo> ACCELEROMETER_DATAList{ get; set; }  // /*重力感应数据=X/Y/Z轴的加速度
    public List<long> INSTAN_DOWNLOAD_SPEEDList{ get; set; }    // /*全程瞬时下载速率=每3s的下载量(kb)*/
    public List<long> VIDEO_ALL_PEAK_RATEList{ get; set; }   // /*全程阶段的峰值速率，下载量每秒（kb/s）*/
    public List<GPSPoint> GPSPointList{ get; set; }    // /*GPS经度*/
    public List<string> SIGNALList{ get; set; }   // 信号汇总信息（按GPS的5个时间点来取）
    public List<ADJInfo> ADJList{ get; set; }   // 邻区ECELLID
    public List<STALLInfo> STALLlist{ get; set; }    // 卡顿信息
    public int ACCMIN{ get; set; }    // 最小接入电平   //已弃用 V1.5.5B4
    public List<string> NETWORK_TYPEList{ get; set; }
    public string USERSCENE{ get; set; }    // /*用户场景*/
    public long MOVE_SPEED{ get; set; }    // 手机移动速度
    public int ISPLAYCOMPLETED{ get; set; }    // 是否播放完成
    public string LOCALDATASAVETIME{ get; set; }    // 本地文件保存时间，延时上报的用
    public int ISUPLOADDATATIMELY{ get; set; }    // 记录是测试完了就及时上报了，还是延时上报的
    public string TASKNAME{ get; set; }    // 测试任务（包括测试间隔、测试文件、时间区间等）
    public string RECFILE{ get; set; }    // 录屏文件
    public string APKVERSION{ get; set; }    // APP版本
    public int SATELLITECOUNT{ get; set; }    // 卫星数量
    public int ISOUTSIDE{ get; set; }    // 是否室外
    public string DISTRICT{ get; set; }    // 
    public double BDLON{ get; set; }    // 
    public double BDLAT{ get; set; }    // 
    public double GDLON{ get; set; }    // 
    public double GDLAT{ get; set; }    // 
    public double ACCURACY{ get; set; }    // 
    public double ALTITUDE{ get; set; }    // 
    public double GPSSPEED{ get; set; }    // 
    public string BUSINESSTYPE{ get; set; }    // 
    public string APKNAME{ get; set; }    // 
    public string wifi_SSID{ get; set; }
    public string wifi_MAC{ get; set; }

    public double FREQ{ get; set; }
    public string cpu{ get; set; }
    public string ADJ_SIGNAL{ get; set; }
    public int Adj_ECI1{ get; set; }
    public int Adj_RSRP1{ get; set; }
    public int Adj_SINR1{ get; set; }
    public int isScreenOn{ get; set; }
    public string SCREENRECORD_FILENAME{ get; set; }
    public string NETWORK_FORMAT { get; set; }

    public PhoneInfo pi{ get; set; }    // 
    public QoEVideoInfo()
    {
    }
    public class GPSPoint
    {
        public double LONGITUDE;
        public double LATITUDE;
    }
    public class ADJInfo
    {
        public double ECI;
        public double RSRP;
    }
    public class STALLInfo
    {
        public long POINT;
        public long TIME;
        public STALLInfo()
        {
        }
        public STALLInfo(long point, long time)
        {
            this.POINT = point;
            this.TIME = time;
        }
    }
}
