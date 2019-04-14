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
    public string DATETIME;    // /*数据的入库时间*/
    public string NET_TYPE;    // netType
    public string BUSINESS_TYPE;    // 业务类型，Ex：流媒体、直播、FTP
    public long VIDEO_BUFFER_INIT_TIME;    // 初始缓冲时延
    public long VIDEO_BUFFER_TOTAL_TIME;    // 视频总的下载时长
    public int VIDEO_STALL_NUM;    // 卡顿次数
    public long VIDEO_STALL_TOTAL_TIME;    // 卡顿总时长
    public double VIDEO_STALL_DURATION_PROPORTION;    // 卡顿时延占比
    public int VIDEO_LOAD_SCORE;    // 初始加载评分
    public int VIDEO_STALL_SCORE;    // 卡顿评分
    public int VIDEO_BUFFER_TOTAL_SCORE;    // 缓冲时延评分
    public int USER_SCORE;    // old清晰度  已弃用
    public int VMOS;    // VMOS
    public double VMOS_MATCH;  // 自动打分和手动打分的契合度
    public long PACKET_LOSS;    // 丢包数
    public int ECLATIRY; // 清晰度
    public int ELOAD;    // /*用户对视频播放等待时间的评分(5：无法察觉到缓冲，4：缓冲时间很短，3：缓冲时间长度一般，2：缓冲时间较长，1：缓冲时间过长无法容忍)*/
    public int ESTALL;    // /*用户对流畅度的评分(5:毫无卡顿，4：略有卡顿但不影响观看，3：有卡顿对观看造成一定影响，2：有卡顿对观看造成较大影响，1：卡顿过多无法容忍)*/
    public int EVMOS;    // /*用户对整体视频服务的综合评分(5:非常好，4：良好，3：一般，2：较差，1：无法容忍)*/
    public int ELIGHT;    // /*环境光照对视频观看的影响程度(5：无影响，4：较小影响，3：有一定影响，2：较大影响，1：极大影响）*/
    public int ESTATE;    // /*用户对运动状态的反馈(:4：静止不动，3：偶尔走动，2：持续走动，1：交通工具上)*/
    public string CARRIER;    // /*运营商名称*/
    public int PLMN;    // /*公共陆地移动网络*/
    public int MCC;    // /*移动国家码*/
    public int MNC;    // /*移动网络号码*/
    public int TAC;    // tac
    public int ECI;    // ECI
    public int ENODEBID;    // enodebid
    public int CELLID;    // cellid
    public int SIGNAL_STRENGTH;    // RSRP
    public int SINR;    // SINR
    public string PHONE_MODEL;    // /*手机型号*/
    public string OPERATING_SYSTEM;    // /*操作系统*/
    public string UDID;    // /*移动设备国际身份码*/
    public string IMEI;    // IMEI
    public string IMSI;    // /*国际移动用户识别码*/
    public string USER_TEL;    // 用户号码
    public int PHONE_PLACE_STATE;    // /*手机放置状态,1表示竖屏,2表示横屏*/
    public string COUNTRY;    // /*国家/地区*/
    public string PROVINCE;    // /*省份*/
    public string CITY;    // /*城市*/
    public string ADDRESS;    // /*地址*/
    public int PHONE_ELECTRIC_START;    // /*开始播放时的手机电量百分比*/
    public int PHONE_ELECTRIC_END;    // /*播放结束时的手机电量百分比*/
    public int SCREEN_RESOLUTION_LONG;    // /*屏幕分辨率(长)*/
    public int SCREEN_RESOLUTION_WIDTH;    // /*屏幕分辨率(宽)*/
    public int LIGHT_INTENSITY;    // /*手机环境光照强度*/
    public int PHONE_SCREEN_BRIGHTNESS;    // /*手机屏幕亮度*/
    public long HTTP_RESPONSE_TIME;    // http响应时间
    public long PING_AVG_RTT;    // /*Ping
    public string VIDEO_CLARITY;    // 视频清晰度
    public string VIDEO_CODING_FORMAT;    // /*视频编码格式,如h.264*/
    public int VIDEO_BITRATE;    // 视频比特率
    public int FPS;    // 帧率
    public long VIDEO_TOTAL_TIME;    // 视频总时长
    public long VIDEO_PLAY_TOTAL_TIME;    // /*视频播放时长=结束播放的时间点-点击播放的时间点(秒)*/
    public long VIDEO_PEAK_DOWNLOAD_SPEED;    // /*初始缓冲阶段的峰值速率，单位kb/s*/
    public long APP_PREPARED_TIME;    // 手机UI加载播放器插件的准备工作时间
    public double BVRATE;    // BVRate
    public string STARTTIME;    // /*视频开始播放的时间*/
    public long FILE_SIZE;    // 文件大小
    public string FILE_NAME;    // 文件名称
    public string FILE_SERVER_LOCATION;    // /*视频源服务器的实际地理位置*/
    public string FILE_SERVER_IP;    // 服务器IP
    public string UE_INTERNAL_IP;    // UE
    public string ENVIRONMENTAL_NOISE;    // 环境噪声
    public long VIDEO_AVERAGE_PEAK_RATE;    // /*视频平均下载速率=总下载量/视频播放时长(kb/s)*/
    public List<int> CELL_SIGNAL_STRENGTHList;    // 按0.5s采集一次，保存后集中上报
    public List<XYZaSpeedInfo> ACCELEROMETER_DATAList;  // /*重力感应数据=X/Y/Z轴的加速度
    public List<long> INSTAN_DOWNLOAD_SPEEDList;    // /*全程瞬时下载速率=每3s的下载量(kb)*/
    public List<long> VIDEO_ALL_PEAK_RATEList;   // /*全程阶段的峰值速率，下载量每秒（kb/s）*/
    public List<GPSPoint> GPSPointList;    // /*GPS经度*/
    public List<string> SIGNALList;   // 信号汇总信息（按GPS的5个时间点来取）
    public List<ADJInfo> ADJList;   // 邻区ECELLID
    public List<STALLInfo> STALLlist;    // 卡顿信息
    public int ACCMIN;    // 最小接入电平   //已弃用 V1.5.5B4
    public List<string> NETWORK_TYPEList;
    public string USERSCENE;    // /*用户场景*/
    public long MOVE_SPEED;    // 手机移动速度
    public int ISPLAYCOMPLETED;    // 是否播放完成
    public string LOCALDATASAVETIME;    // 本地文件保存时间，延时上报的用
    public int ISUPLOADDATATIMELY;    // 记录是测试完了就及时上报了，还是延时上报的
    public string TASKNAME;    // 测试任务（包括测试间隔、测试文件、时间区间等）
    public string RECFILE;    // 录屏文件
    public string APKVERSION;    // APP版本
    public int SATELLITECOUNT;    // 卫星数量
    public int ISOUTSIDE;    // 是否室外
    public string DISTRICT;    // 
    public double BDLON;    // 
    public double BDLAT;    // 
    public double GDLON;    // 
    public double GDLAT;    // 
    public double ACCURACY;    // 
    public double ALTITUDE;    // 
    public double GPSSPEED;    // 
    public string BUSINESSTYPE;    // 
    public string APKNAME;    // 


    public string wifi_SSID;
    public string wifi_MAC;

    public double FREQ;
    public string cpu;
    public string ADJ_SIGNAL;
    public int Adj_ECI1;
    public int Adj_RSRP1;
    public int Adj_SINR1;
    public int isScreenOn;
    public string SCREENRECORD_FILENAME;

    public PhoneInfo pi;    // 
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
