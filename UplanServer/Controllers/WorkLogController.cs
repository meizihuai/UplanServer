using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
                        return new NormalResponse(true, "", "", db.ProjectFileTable.Where(a => a.IsPublic == 1 && a.DateTime >=startDate && a.DateTime<= endDate).ToArray());
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
                    return new NormalResponse(true, "", "", db.ProjectFileTable.Where(a => a.Account==account).ToArray());
                }
                else
                {
                    try
                    {
                        DateTime startDate = DateTime.Parse(startTime); startTime = startDate.ToString("yyyy-MM-dd HH:mm:ss");
                        DateTime endDate = DateTime.Parse(endTime); endTime = endDate.ToString("yyyy-MM-dd HH:mm:ss");
                        return new NormalResponse(true, "", "", db.ProjectFileTable.Where(a => a.Account == account && a.DateTime >= startDate && a.DateTime <= endDate).ToArray());
                    }
                    catch (Exception e)
                    {
                        return new NormalResponse(false, "日期格式有误");
                    }
                }
            }
        }
    }
}
