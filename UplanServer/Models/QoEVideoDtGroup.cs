using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace UplanServer
{
    /// <summary>
    /// QOE Video监控组
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
            if (IsExist())
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
        public  bool IsExist(int Id = 0,string inputGroupId="" )
        {
            if (inputGroupId == "")inputGroupId = groupId;
            string sql = "select id from " + tableName + " where groupId='" + inputGroupId + "'";
            if (id > 0)
            {
                sql= "select id from " + tableName + " where id=" + Id + "";
            }
            return ora.SqlIsIn(sql);
        }
        public static QoEVideoDtGroup Get(int id = 0, string where = "")
        {
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
            if (!IsExist(id))
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

    }
    public class QoEVideoDtGroupMember
    {
        private static OracleHelper ora = Module.ora;
        private string tableName = "QoE_Video_Dt_Group_Member";
        public int id; //主键  自增了
        public string dateTime; //注册时间
        public string groupId;
        public string name;
        public string tel;
        public string imei;
        public string imsi;
        public string carrier;
        public string status; //状态   正常   异常
        public string lastDateTime; //最后一次组员上传qoe视频数据
        public string lastDay; //最后一次上传时间的 yyyy-MM-dd
        public int qoe_total_time; //累计qoe数据量
        public int qoe_total_E_time;//累计qoe打分量
        public int qoe_today_time;//今日累计qoe数据量
        public int qoe_today_E_time;//今日累计qoe打分量
        public NormalResponse Create()
        {
            if (imsi == "") return new NormalResponse( "imsi不可为空");
            if (imei == "") return new NormalResponse("imei不可为空");
            if (groupId == "") return new NormalResponse("groupId不可为空");
            if (IsExist())
            {
                return new NormalResponse("该用户已存在");
            }
            DateTime now = DateTime.Now;
            Dictionary<string, object> dik = new Dictionary<string, object>();
            dik.Add("dateTime", dateTime);
            dik.Add("groupId", groupId);
            dik.Add("name", name);
            dik.Add("tel", tel);
            dik.Add("imei", imei);
            dik.Add("imsi", imsi);
            dik.Add("carrier", carrier);
            dik.Add("status", status);
            dik.Add("lastDateTime", now.ToString("yyyy-MM-dd HH:mm:ss"));
            dik.Add("lastDay", now.ToString("yyyy-MM-dd"));
            return ora.InsertByDik(tableName, dik);
        }
        public bool IsExist()
        {
            string sql = "select id from "+tableName +" where imsi='"+imsi+"'";
            return ora.SqlIsIn(sql);
        }
        public string Update()
        {
            return "success";
        }
        public string Get()
        {
            return "success";
        }
    }
}