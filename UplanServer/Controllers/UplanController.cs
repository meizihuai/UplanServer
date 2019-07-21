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
using System.Data.Entity;
using System.Linq.Expressions;

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
        private readonly QoEDbContext db =new QoEDbContext();
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

                //SQL注入
                // usr= ' or 1=1 --  
                // select password,token,userName,power,state from user_Account where userName='' or 1=1 -- and pwd=
   
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
        /// <summary>
        /// 获取QoER网络黑点
        /// </summary>
        /// <param name="city">城市</param>
        /// <param name="carrier">运营商</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="getCount">查询数据量</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetQoERBlackPoints(string city = "all", string carrier = "", string startTime = "", string endTime = "", int getCount = 0)
        {
          
            try
            {
                if (city == null) city = "";                
                if (carrier == null) carrier = "";
                if (startTime == null) startTime = "";
                if (endTime == null) endTime = "";
                if (city == "all") city = "";
                bool isHaveTime = (startTime!="" && endTime!="");
                DateTime now = DateTime.Now;
                DateTime startDate= now, endDate= now;              
                if (isHaveTime)
                {
                    try
                    {
                         startDate = DateTime.Parse(startTime); startTime = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                         endDate = DateTime.Parse(endTime); endTime = endDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch (Exception)
                    {
                        return new NormalResponse(false,"起始时间或结束时间格式非法");
                    }
                }
                var query = db.QoERTable.Where(a => a.IsUploadDataTimely == 0 && a.GDlon > 0 && a.GDlat > 0 && a.NetType != null && a.NetType != "WiFi");
                query = query.Where(a => a.CI != int.MaxValue && a.RSRP!=int.MaxValue);
                if (city != "")
                    query = query.Where(a => a.City == city);
                if (carrier != "")
                    query = query.Where(a => a.Carrier == carrier);
                if (isHaveTime)
                    query = query.Where(a => a.DateTime.CompareTo(startTime) >= 0 && a.DateTime.CompareTo(endTime) <= 0);
                query = query.OrderByDescending(a => a.DateTime);
                if (getCount > 0) query = query.Take(getCount);
                var list = query.Select(a => new
                {
                    a.ID,
                    a.DateTime,
                    a.City,
                    a.Carrier,
                    a.GDlon,
                    a.GDlat,
                    a.RSRP,
                    a.SINR,
                    a.CI,
                    a.ENodeBId,
                    a.CellId
                });
                return new NormalResponse(true, "", "", list.ToArray());
                #region Expression组合动态查询
                //Expression<Func<PhoneInfo, bool>> exp = (a => a.IsUploadDataTimely == 0 && a.GDlon > 0 && a.GDlat > 0 && a.NetType != null && a.NetType != "WiFi");
                //if (city != "")
                //    exp = exp.And(a => a.City == city);
                //if (carrier != "")
                //    exp = exp.And(a => a.Carrier == carrier);
                //if (isHaveTime)
                //    exp = exp.And(a => a.DateTime.CompareTo(startTime) >= 0 && a.DateTime.CompareTo(endTime) <= 0);
                //var query = db.QoERTable.Where(exp).OrderByDescending(a => a.DateTime).AsQueryable();
                //if (getCount > 0) query = query.Take(getCount);
                //var list = query.Select(a =>new
                //{
                //    a.id,
                //    a.DateTime,
                //    a.City,
                //    a.Carrier,
                //    a.GDlon,
                //    a.GDlat,
                //    a.RSRP,
                //    a.SINR,
                //    a.CI,
                //    a.ENodeBId,
                //    a.CellId
                //});
                return new NormalResponse(true, "", "", list);
                #endregion
                #region 传统SQL
                //id,dateTime,city,carrier,gdlon,gdlat,rsrp,sinr,ci,enodebId,cellid
                //string sql = "select id,dateTime,city,carrier,gdlon,gdlat,rsrp,sinr,ci,enodebId,cellid from qoe_report_table where " +
                //             "IsUploadDataTimely=0 and gdlon>0 and gdlat>0 and netType is not null and netType<>'WiFi' " +
                //             (city == "" ? "" : $" and city='{city}'") +
                //             (carrier == "" ? "" : $" and carrier='{carrier}'") +
                //             (!isHaveTime ? "" : $" and dateTime>='{startTime}' and dateTime<='{endTime}'") +
                //             " order by dateTime desc";
                //if (getCount > 0)
                //{
                //    sql = OracleHelper.OracleSelectPage(sql, 0, getCount);
                //}
                //DataTable dt = ora.SqlGetDT(sql);
                //if (dt == null) return new NormalResponse(true, "", "", "[]");
                //dt.Columns[0].ColumnName = "id";
                //dt.Columns[1].ColumnName = "DateTime";
                //dt.Columns[2].ColumnName = "City";
                //dt.Columns[3].ColumnName = "Carrier";
                //dt.Columns[4].ColumnName = "GDlon";
                //dt.Columns[5].ColumnName = "GDlat";
                //dt.Columns[6].ColumnName = "RSRP";
                //dt.Columns[7].ColumnName = "SINR";
                //dt.Columns[8].ColumnName = "CI";
                //dt.Columns[9].ColumnName = "ENodeBId";
                //dt.Columns[10].ColumnName = "CellId";
                //dt.Columns.Remove("RN");
                //return new NormalResponse(true, "", "", dt);
                #endregion

            }
            catch (Exception e)
            {
                return new NormalResponse(false,e.ToString());
            }        
        }
        /// <summary>
        /// 新增QoE自动任务
        /// </summary>
        /// <param name="qoemission">"QoE任务参数</param>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpPost]
        public NormalResponse AddQoEMission(QoEMissionInfo qoemission,string token)
        {
            try
            {
                if (qoemission == null) return new NormalResponse(false, "QoE任务参数不可为空");
                if (qoemission.MissionBody == null) return new NormalResponse(false, "QoE任务详细字段不可为空");
                if (string.IsNullOrEmpty(qoemission.MissionBody.VideoType)) return new NormalResponse(false, "QoE任务详细字段中，VideoType不可为空");
                if (!Module.CheckAdminPower(token)) return new NormalResponse(false, "提交失败,您的权限不足");
                string aid = qoemission.AID;
                var aidDevice = db.DeviceTable.Where(a => a.AID == aid).FirstOrDefault();
                if (aidDevice == null) return new NormalResponse(false, "该AID未注册");
                DateTime now = DateTime.Now;
                DateTime startTime = now;
                DateTime endTime = now;
                try
                {
                    startTime = DateTime.Parse(qoemission.STARTTIME);
                    endTime = DateTime.Parse(qoemission.ENDTIME);
                    if (endTime <= now.AddMinutes(1)) return new NormalResponse(false, $"结束时间不允许小于服务器时间后1分钟，服务器时间:{now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    if (startTime >= endTime) return new NormalResponse(false, "起始时间不可等于结束时间");
                    qoemission.STARTTIME = startTime.ToString("yyyy-MM-dd HH:mm:ss");
                    qoemission.ENDTIME = endTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch (Exception e)
                {
                    return new NormalResponse(false, "起始时间或结束时间格式非法");
                }
                var rt = db.QoEMissionTable.Where(a => a.AID == aid).FirstOrDefault();
                if (rt != null)
                {
                    db.Entry(rt).State = EntityState.Deleted;
                }
                qoemission.DATETIME = now.ToString("yyyy-MM-dd HH:mm:ss");
                qoemission.STATUS = "未开启";
                qoemission.ISCLOSED = -1;
                qoemission.MISSIONBODY = JsonConvert.SerializeObject(qoemission.MissionBody);
                db.QoEMissionTable.Add(qoemission);
                db.SaveChanges();
                return new NormalResponse(true, "添加成功");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        /// <summary>
        /// 获取QoE自动任务列表
        /// </summary>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetQoEMission(string token)
        {
            try
            {
                if (!Module.CheckAdminPower(token)) return new NormalResponse(false, "提交失败,您的权限不足");
                var list = db.QoEMissionTable.ToList();
                return new NormalResponse(true, "", "", list);
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        /// <summary>
        /// 修改QoE自动任务状态
        /// </summary>
        /// <param name="id">QoE任务主键</param>
        /// <param name="code">-1为允许执行，-2为暂停执行</param>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse SetQoEMissionStatus(int id,int code,string token="")
        {
            try
            {
                if (!Module.CheckAdminPower(token)) return new NormalResponse(false, "提交失败,您的权限不足");
                var rt = db.QoEMissionTable.Find(id);
                if (rt == null) return new NormalResponse(false, "没有该任务");
                if(rt.ISCLOSED==1)return new NormalResponse(false, "该任务已执行完毕");
                rt.ISCLOSED = code;  //-1为允许执行，-2为暂停执行
                db.Update(rt, a => a.ISCLOSED);
                return new NormalResponse(true, "修改成功", "", "");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }
        /// <summary>
        /// 删除QoE自动任务
        /// </summary>
        /// <param name="id">QoE任务主键</param>
        /// <param name="token">token,需要管理员权限</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse DeleteQoEMission(int id,string token = "")
        {
            try
            {
                if (!Module.CheckAdminPower(token)) return new NormalResponse(false, "提交失败,您的权限不足");
                var rt = db.QoEMissionTable.Find(id);
                if (rt == null) return new NormalResponse(false, "没有该任务");
                db.Entry(rt).State = EntityState.Deleted;
                db.SaveChanges();
                return new NormalResponse(true, "删除成功", "", "");
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
        }

        /// <summary>
        /// 获取URL资源文件大小
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetUrlContentLength(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "HEAD";
                req.Timeout = 3000;
                req.ReadWriteTimeout = 3000;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                long length = res.ContentLength;
                return new NormalResponse(true, "","", length);
            }
            catch (Exception e)
            {
                return new NormalResponse(true, "",e.Message , 0);
            }
           
        }
    }
}
