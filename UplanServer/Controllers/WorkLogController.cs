using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq.Expressions;

namespace UplanServer.Controllers
{
    /// <summary>
    /// 工作日志WorkLog接口
    /// 接口版本:1.0.0.0
    /// 更改者：梅子怀
    /// 更改时间:2019-04-24 10:00:00
    /// </summary>
    public class WorkLogController : ApiController
    {
        public WorkLogDbContext db = new WorkLogDbContext();
        /// <summary>
        /// 获取项目文件列表
        /// </summary>
        /// <param name="account">用户名</param>
        /// <param name="startTime">入库开始时间</param>
        /// <param name="endTime">入库结束时间</param>
        /// <returns></returns>
        public NormalResponse GetProjectFiles(string account = "", string startTime = "", string endTime = "")
        {
            try
            {
                if (account == null) account = "";
                if (startTime == null) startTime = "";
                if (endTime == null) endTime = "";
                bool isHaveTime = (startTime != "" && endTime != "");
                DateTime now = DateTime.Now;
                DateTime startDate = now, endDate = now;
                if (isHaveTime)
                {
                    try
                    {
                        startDate = DateTime.Parse(startTime); startTime = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                        endDate = DateTime.Parse(endTime); endTime = endDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch (Exception)
                    {
                        return new NormalResponse(false, "起始时间或结束时间格式非法");
                    }
                }
                var exp = PredicateBuilder.True<ProjectFileInfo>();
                //Expression<Func<ProjectFileInfo, bool>> exp;
                if (account == "")
                    exp = (a => a.IsPublic == 1);
                else
                    exp = (a => a.Account == account);
                if (isHaveTime)
                    exp = exp.And(a => a.DateTime.CompareTo(startTime) >= 0 && a.DateTime.CompareTo(endTime) <= 0);
                var list = db.ProjectFileTable.Where(exp).ToArray();
                return new NormalResponse(true,"", "", list);               
            }
            catch(Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }         
        }
        /// <summary>
        /// 上传项目文件
        /// </summary>
        /// <param name="pinfo">项目文件</param>
        /// <returns></returns>
        [HttpPost]
        public NormalResponse UploadProjectFile(ProjectFileInfo pinfo)
        {
            try
            {
                string rootPath = System.Web.HttpContext.Current.Server.MapPath("~/WorkLogFiles/");
                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
                string account = pinfo.Account;
                if (account == null || account == "") return new NormalResponse(false, "account不可为空");
                
                string fileName = pinfo.FileName;
                string fileExt = Module.GetFileExt(fileName);
                string base64 = pinfo.FileBase64;
                string url = "http://111.53.74.132:7063/WorkLogFiles/" + account + "/" + fileName;
                byte[] buffer = Convert.FromBase64String(base64);
                if (buffer == null) return new NormalResponse(false, "文件内容不可为空");
                string accountPath = rootPath + account + "/";
                if (!Directory.Exists(accountPath)) Directory.CreateDirectory(accountPath);
                string filePath = Module.GetFilePath(fileName, accountPath);
                File.WriteAllBytes(filePath, buffer);
                pinfo.FileBase64 = "";
                pinfo.Url = url;
                pinfo.FileExt = fileExt;
                DateTime now= DateTime.Now; 
                pinfo.DateTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                pinfo.Day = now.ToString("yyyy-MM-dd");
                db.ProjectFileTable.Add(pinfo);
                db.SaveChanges();
                return new NormalResponse(true, "文件上传成功");
            }
            catch(Exception e)
            {
                return new NormalResponse(false,e.ToString());
            }       
        }
    }
}
