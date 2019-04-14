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

public class PhoneInfo
{
    public string AID;
    public string RID;
    public string DATETIME;
    public string businessType;
    public string apkName;
    public string phoneModel;
    public string phoneName;
    public string phoneOS;
    public string phonePRODUCT;
    public string carrier;
    public string IMSI;
    public string IMEI;
    public double RSRP;
    public double SINR;
    public double RSRQ;
    public int TAC;
    public int PCI;
    public int EARFCN;

    public int CI;
    public int eNodeBId;
    public int cellId;
    public string netType;
    public string sigNalType;
    public string sigNalInfo;
    public double lon;
    public double lat;
    public double accuracy;
    public double altitude;
    public double gpsSpeed;
    public int satelliteCount;
    public string address;
    public string apkVersion;
    public int ISUPLOADDATATIMELY;

    public int MNC;
    public string wifi_SSID;
    public string wifi_MAC;
    public float PING_AVG_RTT;
    public double FREQ;
    public string cpu;
    public string ADJ_SIGNAL;
    public List<Neighbour> neighbourList;
    public int Adj_ECI1;
    public int Adj_RSRP1;
    public int Adj_SINR1;
    public int isScreenOn;
    public int isGPSOpen;
    public int PHONE_ELECTRIC;
    public int PHONE_SCREEN_BRIGHTNESS;
    public XYZaSpeedInfo xyZaSpeed;

    public string province;
    public string city;
    public string district;
    public string DetailAddress;
    public double bdlon;
    public double bdlat;
    public double gdlon;
    public double gdlat;

    public int VMOS;
    public string HTTP_URL;
    public long HTTP_RESPONSE_TIME;
    public long HTTP_BUFFERSIZE;
}
