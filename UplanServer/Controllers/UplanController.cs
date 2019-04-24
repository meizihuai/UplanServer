using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.IO;
using System.Data;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UplanServer.Controllers
{
    /// <summary>
    /// UPLAN项目接口
    /// 接口版本:1.0.1.1
    /// 更改者：梅子怀
    /// 更改时间:2019-04-22 10:00:00
    /// </summary>
    public class UplanController : ApiController
    {
        private readonly OracleHelper ora = Module.ora;
        private readonly QoEContext db=new QoEContext();
        /// <summary>
        /// 接口测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse Test()
        {
            return new NormalResponse(true, "消息区", "错误区", "数据区");
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="usr">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns>带token的用户身份信息</returns>
       [HttpGet]
       public NormalResponse Login(string usr,string pwd)
        {
            try
            {
                string account = usr;
                string passWord = pwd;
                if (account == "")
                    return new NormalResponse(false, "用户名为空");
                if (passWord == "")
                    return new NormalResponse(false, "密码为空");
                string sql = "select password,token,userName,power,state from user_Account where userName='" + account + "' and state<>0";
                DataTable dt = ora.SqlGetDT(sql);
                List<string> list = new List<string>();
                var itm = list.Select(a => a.ToString() == "123");
                if (dt==null)
                    return new NormalResponse(false, "该用户不存在");
                if (dt.Rows.Count == 0)
                    return new NormalResponse(false, "该用户不存在");
                DataRow row = dt.Rows[0];
                string OraPwd = row["password".ToUpper()].ToString();
                string OraToken = row["token".ToUpper()].ToString();
                string oraName = row["userName".ToUpper()].ToString();
                int power = int.Parse(row["power".ToUpper()].ToString());
                int state = int.Parse(row["state".ToUpper()].ToString());
                if (OraToken==DBNull.Value.ToString())
                    OraToken = "";
                if (OraPwd == passWord)
                {
                    if (OraToken == "")
                        OraToken = Module.GetNewToken(account, true);
                    LoginInfo linfo = new LoginInfo(account, oraName, OraToken, power, state);
                    return new NormalResponse(true, "success", "", linfo);
                }
                else
                    return new NormalResponse(false, "用户名或密码错误", "", "");
            }
            catch (Exception ex)
            {
                return new NormalResponse(false, ex.ToString());
            }
        }
        /// <summary>
        /// 创建新QoE测试组
        /// </summary>
        /// <param name="data">QoEVideoDtGroup class</param>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpPost]
        public NormalResponse CreateQoEVideoDtGroup(QoEVideoDtGroup data, string token="")
        {
            LoginInfo uInfo = Module.GetUsrInfo(token);
            if (uInfo.usr=="")
            {
                return new NormalResponse(false, "token无效");
            }
            if (uInfo.power < 9)
            {
                return new NormalResponse(false, "提交失败,您的权限不足","","usr="+uInfo.usr);
            }
            return data.Create();
        }
        /// <summary>
        /// 获取所有QoE测试组
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetAllQoEVideoDtGroup(string token = "")
        {
            if (!Module.CheckToken(token)) return new NormalResponse("token无效");
            return new NormalResponse(true, "", "", QoEVideoDtGroup.SelectToList());
        }
        /// <summary>
        /// 更新QOE测试组
        /// </summary>
        /// <param name="data">QoEVideoDtGroup class</param>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpPost]
        public NormalResponse UpdateQoEVideoDtGroup(QoEVideoDtGroup data,string token)
        {
            LoginInfo uInfo = Module.GetUsrInfo(token);
            if (uInfo.usr == "")
            {
                return new NormalResponse(false, "token无效");
            }
            if (uInfo.power < 9)
            {
                return new NormalResponse(false, "提交失败,您的权限不足", "", "usr=" + uInfo.usr);
            }
            return data.Update();
        }
        /// <summary>
        /// 删除QoE测试组
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse DeleteQoEVideoDtGroup(int id,string token = "")
        {
            LoginInfo uInfo = Module.GetUsrInfo(token);
            if (uInfo.usr == "")
            {
                return new NormalResponse(false, "token无效");
            }
            if (uInfo.power < 9)
            {
                return new NormalResponse(false, "提交失败,您的权限不足", "", "usr=" + uInfo.usr);
            }
            return QoEVideoDtGroup.Delete(id);
        }
        /// <summary>
        /// 创建新QoE测试组成员
        /// </summary>
        /// <param name="data">QoEVideoDtGroupMember</param>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpPost]
        public NormalResponse CreateQoEVideoDtGroupMember(QoEVideoDtGroupMember data,string token)
        {
            LoginInfo uInfo = Module.GetUsrInfo(token);
            if (uInfo.usr == "")
            {
                return new NormalResponse(false, "token无效");
            }
            if (uInfo.power < 9)
            {
                return new NormalResponse(false, "提交失败,您的权限不足", "", "usr=" + uInfo.usr);
            }
            return data.Create();
        }
        /// <summary>
        /// 获取所有QoE测试成员
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetAllQoEVideoDtGroupMember(string token = "")
        {
            if (!Module.CheckToken(token)) return new NormalResponse("token无效");
            return new NormalResponse(true, "", "", QoEVideoDtGroupMember.SelectToList());
        }
        /// <summary>
        /// 更新QoE测试组成员
        /// </summary>
        /// <param name="data">QoEVideoDtGroupMember</param>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpPost]
        public NormalResponse UpdateQoEVideoDtGroupMember(QoEVideoDtGroupMember data, string token)
        {
            LoginInfo uInfo = Module.GetUsrInfo(token);
            if (uInfo.usr == "")
            {
                return new NormalResponse(false, "token无效");
            }
            if (uInfo.power < 9)
            {
                return new NormalResponse(false, "提交失败,您的权限不足", "", "usr=" + uInfo.usr);
            }
            return data.Update();
        }
        /// <summary>
        /// 删除QoE测试组成员
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse DeleteQoEVideoDtGroupMember(int id, string token = "")
        {
            LoginInfo uInfo = Module.GetUsrInfo(token);
            if (uInfo.usr == "")
            {
                return new NormalResponse(false, "token无效");
            }
            if (uInfo.power < 9)
            {
                return new NormalResponse(false, "提交失败,您的权限不足", "", "usr=" + uInfo.usr);
            }
            return QoEVideoDtGroupMember.Delete(id);
        }
        /// <summary>
        /// QoE测试组获取组内成员，用于实时监控界面
        /// </summary>
        /// <param name="groupId">测试组Id</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse QoEVideoDtGroupGetMembers(string token, string groupId = "")
        {
            if (!Module.CheckToken(token)) return new NormalResponse("token无效");
            if(groupId=="" || groupId == "all")
            {
                return GetAllQoEVideoDtGroupMember(token);
            }
            return QoEVideoDtGroup.GetMembers(groupId);
        }
        /// <summary>
        /// QoE测试组成员当天QoE Gis数据，用于实时监控界面地理化呈现
        /// </summary>
        /// <param name="imsi"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetQoEVideoDtGroupMemberTodayGisData(string imsi,string token)
        {
            string sql = "select imsi,gdlon,gdlat,vmos,evmos,ECLATIRY,ELOAD,ESTALL,ELIGHT,ESTATE,dateTime from qoe_video_table where imsi='{0}' and dateTime between '{1}' and '{2}' order by dateTime desc";
            DateTime now = DateTime.Now;
            sql = string.Format(sql, new string[] {imsi,now.ToString("yyyy-MM-dd 00:00:00"), now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") });
            DataTable dt = ora.SqlGetDT(sql);
            return new NormalResponse(true, "", "", dt);
        }
        /// <summary>
        /// 查询QoE测试组汇总统计城市列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetQoEVideoDtCollectCitys()
        {
            string sql = "select city from qoe_video_table  where city is not null group by city";
            DataTable dt = ora.SqlGetDT(sql);
            List<string> list = new List<string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string name = row["city"].ToString();
                    list.Add(name);
                }
            }
            return new NormalResponse(true, "", "", list);
        }
        /// <summary>
        /// 根据时间和城市，查询有数据的QoE测试成员
        /// </summary>
        /// <param name="startTime">起始时间，精确到秒，格式yyyy-MM-dd HH:mm:ss</param>
        /// <param name="endTime">终止时间，精确到秒，格式yyyy-MM-dd HH:mm:ss</param>
        /// <param name="city">城市，全部用'all'表示，默认all</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetQoEVideoDtCollectMembers(DateTime startTime,DateTime endTime,string city="all")
        {
            if (endTime <= startTime) return new NormalResponse("结束时间需大于起始时间");
            string startTimestr = startTime.ToString("yyyy-MM-dd HH:mm:ss");
            string endTimestr = endTime.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "select name from qoe_video_dt_group_member where imsi in (" +
                "select imsi from qoe_video_table where city='{0}' and dateTime between '{1}' and '{2}')";
            sql = string.Format(sql, new string[] { city, startTimestr, endTimestr });
            if (city=="all" || city == "")
            {
                sql = "select name from qoe_video_dt_group_member where imsi in (" +
                "select imsi from qoe_video_table where   dateTime between '{0}' and '{1}')";
                sql = string.Format(sql, new string[] { startTimestr, endTimestr });
            }
         
            DataTable dt = ora.SqlGetDT(sql);            
            List<string> list = new List<string>();
            if(dt!=null && dt.Rows.Count > 0)
            {
                //取dataTable某列为list
                list = (from d in dt.AsEnumerable() select d.Field<string>("name")).ToList();
                //foreach(DataRow row in dt.Rows)
                //{
                //    string name = row["name"].ToString();
                //    list.Add(name);
                //}
            }
            return new NormalResponse(true, "", "", list);
        }
        /// <summary>
        /// 查询QoE测试组汇总统计数据
        /// </summary>
        /// <param name="startTime">起始时间，精确到秒，格式yyyy-MM-dd HH:mm:ss</param>
        /// <param name="endTime">终止时间，精确到秒，格式yyyy-MM-dd HH:mm:ss</param>
        /// <param name="city">城市，全部用'all'表示，默认all</param>
        /// <param name="memberName">测试人员，全部用'all'表示，默认all</param>
        /// <param name="dataType">返回数据类型，0为统计数据，1为详细数据</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetQoEVideoDtCollectData(DateTime startTime, DateTime endTime, string city = "all",string memberName="all",int dataType=0)
        {
            if (endTime <= startTime) return new NormalResponse("结束时间需大于起始时间");
            string startTimestr = startTime.ToString("yyyy-MM-dd HH:mm:ss");
            string endTimestr = endTime.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = @"select A.*,QOE_VIDEO_DT_GROUP_MEMBER.name from(
select 
imsi,
city,
count(imsi) as 观看视频次数,
count(CASE WHEN evmos>0 THEN 1 ELSE null END) as 有效打分次数,
count(CASE WHEN ISSCREENRECORDUPLOADED>0 THEN 1 ELSE null END) as 上传视频个数,
round(100-100*abs(sum(CASE WHEN evmos>0 THEN VMOS ELSE null END)-sum(evmos))/(count(CASE WHEN evmos>0 THEN 1 ELSE null END)*4),2) as 打分匹配度
from QOE_VIDEO_TABLE where imsi in
(select imsi from QOE_VIDEO_DT_GROUP_MEMBER)
and dateTime between '2018-04-01 00:00:00' and '2019-04-02 00:00:00' and city is not null GROUP BY imsi,city)A, QOE_VIDEO_DT_GROUP_MEMBER where A.imsi=QOE_VIDEO_DT_GROUP_MEMBER.imsi";
            DataTable dt = ora.SqlGetDT(sql);
            return new NormalResponse(true, "接口还没做完，稍等","", dt);
        }
        /// <summary>
        /// 获取在线设备，包含在线QoE和QoER在线人数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetOnlineInfo()
        {
            
            var onlineQoER = db.DeviceTable.Where(a => a.IsOnline == 1).Select(a=>new { a.AID,a.LastDateTime}).ToList();
            var onlineQoE = db.UserBPTable.Where(a => a.IsPlayingVideo == 1).Select(a => new { a.AID ,a.LastAskVideoTime}).ToList();
          
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("OnlineQoERCount", onlineQoER.Count);
            dic.Add("OnlineQoECount", onlineQoE.Count);
            dic.Add("OnlineQoERList", onlineQoER);
            dic.Add("OnlineQoEList", onlineQoE);                
            return new NormalResponse(true,"", "", dic);
        }      
    }
}
