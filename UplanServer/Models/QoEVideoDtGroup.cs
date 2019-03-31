using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace UplanServer
{
    /// <summary>
    /// QOE Video测试组
    /// </summary>
    public class QoEVideoDtGroup
    {
        private static OracleHelper ora = Module.ora;
        private static string tableName = "QoE_Video_Dt_Group";
        /// <summary>
        /// -主键，数据库自增
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// -组创建时间 
        /// </summary>
        public string dateTime { get; set; } 
        /// <summary>
        /// *组ID
        /// </summary>
        public string groupId { get; set; }
        /// <summary>
        /// *城市  以 '市' 结尾
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 管理员/负责人/项目经理
        /// </summary>
        public string manager { get; set; }
        /// <summary>
        /// 管理者电话
        /// </summary>
        public string manager_tel { get; set; }
        /// <summary>
        /// 管理者邮箱
        /// </summary>
        public string manager_email { get; set; }
        /// <summary>
        /// -组状态   正常   异常  
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// -最后一次组员上传qoe视频数据时间
        /// </summary>
        public string lastDateTime { get; set; }
        /// <summary>
        /// -最后一次组员上传qoe视频数据日期
        /// </summary>
        public string lastDay { get; set; }


        public NormalResponse Create()
        {
            if (groupId == "") return new NormalResponse( "groupId不可为空");
            if (IsExist(true))
            {
                return new NormalResponse("该组id已存在");
            }
            DateTime now = DateTime.Now;
            Dictionary<string, object> dik = new Dictionary<string, object>();
            dik.Add("dateTime", now.ToString("yyyy-MM-dd HH:mm:ss"));
            dik.Add("groupId", groupId);
            dik.Add("city", city);
            dik.Add("manager", manager);
            dik.Add("manager_tel", manager_tel);
            dik.Add("manager_email", manager_email);
            dik.Add("status", status);
            dik.Add("lastDateTime", now.ToString("yyyy-MM-dd HH:mm:ss"));
            dik.Add("lastDay", now.ToString("yyyy-MM-dd"));
            return ora.InsertByDik(tableName, dik);
        }
        private  bool IsExist(bool useGroupId=false)
        {
            string sql = sql = "select id from " + tableName + " where id=" + id + "";
            if (useGroupId)
            {
                sql = "select id from " + tableName + " where groupId=" + groupId + "";
            }
            return ora.SqlIsIn(sql);
        }
        public static QoEVideoDtGroup Get(int id = 0, string where = "")
        {
            if (id == 0 && where == "") return null;
            string sql = "select * from " + tableName + (where == "" ? "" : " where " + where);
            if (id > 0)
            {
                sql = "select * from " + tableName + " where id="+id;
            }
            DataTable dt = ora.SqlGetDT(sql);
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;
            DataRow row = dt.Rows[0];
            QoEVideoDtGroup qoe = new QoEVideoDtGroup();
            qoe.id = int.Parse(row["ID"].ToString());
            qoe.dateTime = row["dateTime"].ToString();
            qoe.groupId = row["groupId"].ToString();
            qoe.city = row["city"].ToString();
            qoe.manager = row["manager"].ToString();
            qoe.manager_tel = row["manager_tel"].ToString();
            qoe.manager_email = row["manager_email"].ToString();
            qoe.status = row["status"].ToString();
            qoe.lastDateTime = row["lastDateTime"].ToString();
            qoe.lastDay = row["lastDay"].ToString();
            return qoe;
        }
        public static List<QoEVideoDtGroup> SelectToList(string where="")
        {
            List<QoEVideoDtGroup> list = new List<QoEVideoDtGroup>();
            string sql = "select * from "+tableName+(where==""?"":" where " + where);
            DataTable dt = ora.SqlGetDT(sql);
            if (dt == null) return list;
            if (dt.Rows.Count == 0) return list;
            foreach(DataRow row in dt.Rows)
            {
                QoEVideoDtGroup qoe = new QoEVideoDtGroup();
                //PropertyInfo[] pros = typeof(QoEVideoDtGroup).GetProperties();
                //foreach(var p in pros)
                //{
                //    try
                //    {
                //        if (p.PropertyType == typeof(int))
                //        {

                //        }
                //    }catch(Exception e)
                //    {

                //    }
                //}
                qoe.id =int.Parse( row["ID"].ToString());
                qoe.dateTime = row["dateTime"].ToString();
                qoe.groupId = row["groupId"].ToString();
                qoe.city = row["city"].ToString();
                qoe.manager = row["manager"].ToString();
                qoe.manager_tel = row["manager_tel"].ToString();
                qoe.manager_email = row["manager_email"].ToString();
                qoe.status = row["status"].ToString();
                qoe.lastDateTime= row["lastDateTime"].ToString();
                qoe.lastDay = row["lastDay"].ToString();
                list.Add(qoe);
            }
            return list;
        }
        public NormalResponse Update()
        {
            if (id == 0) return new NormalResponse("id不可为0");       
            if (!IsExist())
            {
                return new NormalResponse("该组不存在");
            }
            else
            {
                QoEVideoDtGroup qoe = Get(0,"groupId='" + groupId + "'");
                if (qoe != null)
                {
                    if (qoe.id != this.id)
                    {
                        return new NormalResponse("该组id已被使用");
                    }
                }
            }
            DateTime now = DateTime.Now;
            Dictionary<string, object> dik = new Dictionary<string, object>();
            dik.Add("groupId", groupId);
            dik.Add("city", city);
            dik.Add("manager", manager);
            dik.Add("manager_tel", manager_tel);
            dik.Add("manager_email", manager_email);
            return ora.UpdateByDik(tableName, dik,id);
        }
        public static NormalResponse Delete(int id)
        {
            if (id == 0) return new NormalResponse("id不可为0");
            if (Get(id)==null)
            {
                return new NormalResponse("该组不存在");
            }
            string sql = "delete from " + tableName + " where id=" + id;
            string result = ora.SqlCMD(sql);
            return new NormalResponse(result);
        }
        public static NormalResponse GetMembers(string groupId)
        {
            if (groupId =="") return new NormalResponse("groupId不可为空");
            if (Get(0, "groupId='" + groupId + "'") == null)
            {
                return new NormalResponse("该组id不存在");
            }
            List<QoEVideoDtGroupMember> list = QoEVideoDtGroupMember.SelectToList("groupId='"+ groupId+"'");
            return new NormalResponse(true, "", "", list);
        }

    }
    public class QoEVideoDtGroupMember
    {
        private static OracleHelper ora = Module.ora;
        private static  string tableName = "QoE_Video_Dt_Group_Member";
        /// <summary>
        /// -主键
        /// </summary>
        public int id;
        /// <summary>
        /// -注册时间
        /// </summary>
        public string dateTime;
        /// <summary>
        /// *组id
        /// </summary>
        public string groupId;
        /// <summary>
        /// *名字
        /// </summary>
        public string name;
        /// <summary>
        /// 电话
        /// </summary>
        public string tel;
        /// <summary>
        /// -imei
        /// </summary>
        public string imei;
        /// <summary>
        /// *imsi
        /// </summary>
        public string imsi;
        /// <summary>
        /// -运营商
        /// </summary>
        public string carrier;
        /// <summary>
        /// -状态   正常   异常
        /// </summary>
        public string status;
        /// <summary>
        /// -最后一次上传qoe视频数据时间 yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string lastDateTime;
        /// <summary>
        /// -最后一次上传qoe视频数据日期 yyyy-MM-dd
        /// </summary>
        public string lastDay;
        /// <summary>
        /// -累计qoe数据量
        /// </summary>
        public int qoe_total_time;
        /// <summary>
        /// -累计qoe打分量
        /// </summary>
        public int qoe_total_E_time;
        /// <summary>
        /// -今日累计qoe数据量
        /// </summary>
        public int qoe_today_time;
        /// <summary>
        /// -今日累计qoe打分量
        /// </summary>
        public int qoe_today_E_time;
        /// <summary>
        /// -累计打分匹配度
        /// </summary>
        public int qoe_vmos_match_total;
        /// <summary>
        /// -今日打分匹配度
        /// </summary>
        public int qoe_vmos_match_today;
        public NormalResponse Create()
        {
            if (imsi == "") return new NormalResponse( "imsi不可为空");
            if (groupId == "") return new NormalResponse("groupId不可为空");
            if (QoEVideoDtGroup.Get(0, "groupId='" + groupId + "'") == null)
            {
                return new NormalResponse("您选择的组id不存在");
            }
            if (IsExist(true))
            {
                return new NormalResponse("该imsi号已存在");
            }
            DateTime now = DateTime.Now;
            Dictionary<string, object> dik = new Dictionary<string, object>();
            dik.Add("dateTime", now.ToString("yyyy-MM-dd HH:mm:ss"));
            dik.Add("groupId", groupId);
            dik.Add("name", name);
            dik.Add("tel", tel);
            dik.Add("imsi", imsi);
            dik.Add("lastDateTime", now.ToString("yyyy-MM-dd HH:mm:ss"));
            dik.Add("lastDay", now.ToString("yyyy-MM-dd"));
            return ora.InsertByDik(tableName, dik);
        }
        private bool IsExist(bool useImsi=false)
        {
            string sql = "select id from "+tableName +" where id="+id+"";
            if (useImsi)
            {
                sql = "select id from " + tableName + " where imsi='" + imsi + "'";
            }
            return ora.SqlIsIn(sql);
        }
        public static List<QoEVideoDtGroupMember> SelectToList(string where = "")
        {
            List<QoEVideoDtGroupMember> list = new List<QoEVideoDtGroupMember>();
            string sql = "select * from " + tableName + (where == "" ? "" : " where " + where);
            DataTable dt = ora.SqlGetDT(sql);
            if (dt == null) return list;
            if (dt.Rows.Count == 0) return list;
            foreach (DataRow row in dt.Rows)
            {
                QoEVideoDtGroupMember qoe = new QoEVideoDtGroupMember();
                qoe.id = int.Parse(row["ID"].ToString());
                qoe.dateTime = row["dateTime"].ToString();
                qoe.groupId = row["groupId"].ToString();
                qoe.name = row["name"].ToString();
                qoe.tel = row["tel"].ToString();
                qoe.imei = row["imei"].ToString();
                qoe.imsi = row["imsi"].ToString();
                qoe.carrier = row["carrier"].ToString();
                qoe.status = row["status"].ToString();
                qoe.lastDateTime = row["lastDateTime"].ToString();
                qoe.lastDay = row["lastDay"].ToString();
                int.TryParse(row["qoe_total_time"].ToString(), out qoe.qoe_total_time);
                int.TryParse(row["qoe_total_E_time"].ToString(), out qoe.qoe_total_E_time);
                int.TryParse(row["qoe_today_time"].ToString(), out qoe.qoe_today_time);
                int.TryParse(row["qoe_today_E_time"].ToString(), out qoe.qoe_today_E_time);
                int.TryParse(row["qoe_vmos_match_total"].ToString(), out qoe.qoe_vmos_match_total);
                int.TryParse(row["qoe_vmos_match_today"].ToString(), out qoe.qoe_vmos_match_today);
                list.Add(qoe);
            }
            return list;
        }
        public NormalResponse Update()
        {
            if (id == 0) return new NormalResponse("id不可为0");
            if (imsi == "") return new NormalResponse("imsi不可为空");
            if (groupId == "") return new NormalResponse("groupId不可为空");
            if (QoEVideoDtGroup.Get(0, "groupId='" + groupId + "'") == null)
            {
                return new NormalResponse("您选择的组id不存在");
            }
            if (!IsExist())
            {
                return new NormalResponse("该成员不存在");
            }
            else
            {
                QoEVideoDtGroupMember qoe = Get(0, "imsi='" + imsi + "'");
                if (qoe != null)
                {
                    if (qoe.id != this.id)
                    {
                        return new NormalResponse("该imsi已被使用");
                    }
                }
            }
            DateTime now = DateTime.Now;
            Dictionary<string, object> dik = new Dictionary<string, object>();
            dik.Add("groupId", groupId);
            dik.Add("name", name);
            dik.Add("tel", tel);
            dik.Add("imsi", imsi);
            return ora.UpdateByDik(tableName, dik, id);
        }
        public static QoEVideoDtGroupMember Get(int id = 0, string where = "")
        {
            if (id == 0 && where == "") return null;
            string sql = "select * from " + tableName + (where == "" ? "" : " where " + where);
            if (id > 0)
            {
                sql = "select * from " + tableName + " where id=" + id;
            }
            DataTable dt = ora.SqlGetDT(sql);
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return null;
            DataRow row = dt.Rows[0];
            QoEVideoDtGroupMember qoe = new QoEVideoDtGroupMember();
            qoe.id = int.Parse(row["ID"].ToString());
            qoe.dateTime = row["dateTime"].ToString();
            qoe.groupId = row["groupId"].ToString();
            qoe.name = row["name"].ToString();
            qoe.tel = row["tel"].ToString();
            qoe.imei = row["imei"].ToString();
            qoe.imsi = row["imsi"].ToString();
            qoe.carrier = row["carrier"].ToString();
            qoe.status = row["status"].ToString();
            qoe.lastDateTime = row["lastDateTime"].ToString();
            qoe.lastDay = row["lastDay"].ToString();
            int.TryParse(row["qoe_total_time"].ToString(), out qoe.qoe_total_time);
            int.TryParse(row["qoe_total_E_time"].ToString(), out qoe.qoe_total_E_time);
            int.TryParse(row["qoe_today_time"].ToString(), out qoe.qoe_today_time);
            int.TryParse(row["qoe_today_E_time"].ToString(), out qoe.qoe_today_E_time);
            int.TryParse(row["qoe_vmos_match_total"].ToString(), out qoe.qoe_vmos_match_total);
            int.TryParse(row["qoe_vmos_match_today"].ToString(), out qoe.qoe_vmos_match_today);
            return qoe;
        }
        public static NormalResponse Delete(int id)
        {
            if (id == 0) return new NormalResponse("id不可为0");
            if (Get(id) == null)
            {
                return new NormalResponse("该成员不存在");
            }
            string sql = "delete from " + tableName + " where id=" + id;
            string result = ora.SqlCMD(sql);
            return new NormalResponse(result);
        }
    }
}