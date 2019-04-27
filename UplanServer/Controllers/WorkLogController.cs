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
        public WorkLogContent db = new WorkLogContent();
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
                if (account == "")
                {
                    if (startTime == "" || endTime == "")
                    {
                        return new NormalResponse(true, "", "", db.ProjectFileTable.Where(a => a.IsPublic == 1).ToArray());
                    }
                    else
                    {
                        try
                        {
                            DateTime startDate = DateTime.Parse(startTime); startTime = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                            DateTime endDate = DateTime.Parse(endTime); endTime = endDate.ToString("yyyy-MM-dd HH:mm:ss");
                            var list = db.ProjectFileTable.Where(a => a.IsPublic == 1 &&
                                                       DbFunctions.DiffSeconds(DateTime.Parse(a.DateTime), startDate) >= 0 &&
                                                       DbFunctions.DiffSeconds(DateTime.Parse(a.DateTime), endDate) <= 0).ToArray();
                            return new NormalResponse(true, "", "", list);
                        }
                        catch (Exception e)
                        {
                            return new NormalResponse(false, "日期格式有误");
                        }
                    }
                }
                else
                {
                    if (startTime == "" || endTime == "")
                    {
                        return new NormalResponse(true, "", "", db.ProjectFileTable.Where(a => a.Account == account).ToArray());
                    }
                    else
                    {
                        try
                        {
                            DateTime startDate = DateTime.Parse(startTime); startTime = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                            DateTime endDate = DateTime.Parse(endTime); endTime = endDate.ToString("yyyy-MM-dd HH:mm:ss");
                            var list = db.ProjectFileTable.Where(a => a.Account == account &&
                                                       a.DateTime.CompareTo(startTime)>=0 &&
                                                       a.DateTime.CompareTo(endTime) <= 0).ToArray();
                            return new NormalResponse(true,"", "", list);
                        }
                        catch (Exception e)
                        {
                            return new NormalResponse(false, "日期格式有误",e.ToString(),"");
                        }
                    }
                }
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
