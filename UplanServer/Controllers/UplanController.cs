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

namespace UplanServer.Controllers
{
    /// <summary>
    /// UPLAN项目接口
    /// 接口版本:1.0.1.1
    /// 更改者：梅子怀
    /// 更改时间:2019-03-31 11:12:00
    /// </summary>
    public class UplanController : ApiController
    {
        private readonly OracleHelper ora = Module.ora;
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
        /// QoE测试组获取组内成员
        /// </summary>
        /// <param name="groupId">测试组Id</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse QoEVideoDtGroupGetMembers(string groupId,string token)
        {
            if (!Module.CheckToken(token)) return new NormalResponse("token无效");
            return QoEVideoDtGroup.GetMembers(groupId);
        }
    }
}
