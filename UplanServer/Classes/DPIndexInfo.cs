using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UplanServer
{
    public class DPIndexInfo
    {
        public string refreshLever;
        public string refreshTime;
        public long totalWorkSecond;
        public long chinaWorkms;
     
        public ChinaQoERankData chinaQoeRankData;
        public QoERankinglist provinceQoeRankingList;
       
        public DPIndexInfo()
        {

        }
    }
    public class ChinaQoERankData
    {
        public RankData d1data;
        public RankData d7data;
        public RankData d30data;
        public RankData d180data;
    }
    public class QoERankinglist
    {
        public List<RankInfo> d1List;
        public List<RankInfo> d7List;
        public List<RankInfo> d30List;
        public List<RankInfo> d180List;
        public QoERankinglist()
        {
            this.d1List = new List<RankInfo>();
            this.d7List = new List<RankInfo>();
            this.d30List = new List<RankInfo>();
            this.d180List = new List<RankInfo>();
        }
    }
    public class RankInfo
    {
        public RankData rankData;
        public List<CityRankData> cityRankList;
        public RankInfo(string name, double vmos, long dataCount,long badQoECount,long badRSRPCount,long badSINRCount)
        {
            this.rankData = new RankData(name, vmos, dataCount, badQoECount, badRSRPCount, badSINRCount);
            this.cityRankList = new List<CityRankData>();
        }
    }
    public class CityRankData
    {
        public string name;
        public double vmos;
        public long dataCount;
        public long badQoECount;
        public long badRSRPCount;
        public long badSINRCount;
        public long screen;
        public CityRankData(string name, double vmos, long dataCount, long badQoECount, long badRSRPCount, long badSINRCount)
        {
            this.name = name;
            this.vmos = vmos;
            this.dataCount = dataCount;
            this.badQoECount = badQoECount;
            this.badRSRPCount = badRSRPCount;
            this.badSINRCount = badSINRCount;
        }
    }
    public class RankData
    {
        public string name;
        public double vmos;
        public long dataCount;
        public long badQoECount;
        public long badRSRPCount;
        public long badSINRCount;
       
        public List<PieInfo> phoneModelList;
        public List<PieInfo> networkTypeList;
        public Dictionary<string, object> detailQuota;
        public double bufferIniScore;
        public double bufferTotalRate;
        public double rttScore;
        public double unStallRate;
        public List<ChartPoint> qoeTable;
        public List<ChartPoint> dataCountTable;
     

        public RankData(string name, double vmos, long dataCount,long badQoECount,long badRSRPCount,long badSINRCount)
        {
            this.name = name;
            this.vmos = vmos;
            this.dataCount = dataCount;
            this.badQoECount = badQoECount;
            this.badRSRPCount = badRSRPCount;
            this.badSINRCount = badSINRCount;  
            this.phoneModelList = new List<PieInfo>();
            this.networkTypeList = new List<PieInfo>();
            this.detailQuota = new Dictionary<string, object>();
            this.qoeTable = new List<ChartPoint>();
            this.dataCountTable = new List<ChartPoint>();
            
        }
    }
    public class ChartPoint
    {
        public string x;
        public double y;
        public ChartPoint(string x,double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    public class PieInfo
    {
        public string name;
        public double value;
        public int count;
        public PieInfo(string name,double value,int count=0)
        {
            this.name = name;
            this.value = value;
            this.count = count;
        }
    }
    public class ProvinceDetailQuota
    {
        public string province;
        public string useTime;
        public string refreshTime;
        public DetailQuota data;
    }
    public class DetailQuota
    {
        public string refreshTime;
        public NetworkCapability network;
        public TerminalPerformance terminalPerformance;
        public DocumentQuality documentQuality;
        public EnvironmentalFactor environmentalFactor;
    }
    //网络能力
    public class NetworkCapability
    {
        public double rtt;
        public double rsrp;
        public double sinr;
        public double bufferInitTime;
        public double unStallRate;
        public double badrsrpRate;
        public double badsinrRate;
    }
    public class TerminalPerformance
    {
        public double apploadTime;
        public double fps;
        public object phoneModelList;
        public object networkTypeList;
        public object resolutionList;
    }
    public class DocumentQuality
    {
        public double fps;
        public List<RankInfoWithRateAndCount> codeTypeList;
        public List<RankInfoWithRateAndCount> claritySizeList;
    }
    public class EnvironmentalFactor
    {
        public double light;
        public double speed;
        public double noise;
        public double screenLight;
    }
    public class RankInfoWithRateAndCount
    {
        public string name;
        public int count;
        public double rate;
        public RankInfoWithRateAndCount(string name, int count)
        {
            this.name = name;
            this.count = count;
        }
    }
    public class PerformanceData
    {
        public List<PerformanceInfo> performancelist;
        public List<StationSource> stationSourcelist;
        public List<TopInfo> connectTopList, disturbToplist, dropToplist, jamToplist;
        public PerformanceData()
        {
            this.connectTopList = new List<TopInfo>();
            this.disturbToplist = new List<TopInfo>();
            this.dropToplist = new List<TopInfo>();
            this.jamToplist = new List<TopInfo>();
        }
    }
    public class TopInfo
    {
        public string time;
        public string eNodebId_CellId;
        public double rate;
        public TopInfo(string time,string eNodebId_CellId,double rate)
        {
            this.time = time;
            this.eNodebId_CellId = eNodebId_CellId;
            this.rate = rate;
        }
           
    }
    public class StationSource
    {
        public string city;
        public int stationCount;
        public int cellCount;
        public int useCellCount;
        public int unUseCellCount;
        public int zeroCellCount;
        public double flow;
        public int brokenStationCount;
        public int highCellCount;
        public int busyCount;
        public int distrubCellCount;
        public StationSource(string city, int stationCount, int cellCount, int useCellCount, int unUseCellCount, int zeroCellCount, double flow,
            int brokenStationCount, int highCellCount, int busyCount, int distrubCellCount)
        {
            this.city = city;
            this.stationCount = stationCount;
            this.cellCount = cellCount;
            this.useCellCount = useCellCount;
            this.unUseCellCount = unUseCellCount;
            this.zeroCellCount = zeroCellCount;
            this.flow = flow;
            this.brokenStationCount = brokenStationCount;
            this.highCellCount = highCellCount;
            this.busyCount = busyCount;
            this.distrubCellCount = distrubCellCount;
        }
    }
    public class PerformanceInfo
    {
        //接通率    掉线率    流量     干扰
        //站点资源
        //TOP小区  接通 干扰 掉线  拥塞
        public string dateTime;
        public double connectRate;
        public double dropDate;
        public double flow;
        public double disturb;
        public PerformanceInfo(string dateTime,double connectRate, double dropDate, double flow, double disturb)
        {
            this.dateTime = dateTime;
            this.connectRate = connectRate;
            this.dropDate = dropDate;
            this.flow = flow;
            this.disturb = disturb;
        }
    }
    public class AutoBusQutoaInfo
    {
        public List<SysAutoBusInfo> autoBusList;
        public double rtt;
        public double rsrp;
        public double sinr;
        public List<PieInfo> networkList;
        public AutoBusQutoaInfo()
        {
            autoBusList = new List<SysAutoBusInfo>();
            networkList = new List<PieInfo>();
            rtt = 0;
        }
    }
}