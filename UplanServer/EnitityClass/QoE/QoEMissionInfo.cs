using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UplanServer
{
    [Table("QOE_MISSION_TABLE")]
    public class QoEMissionInfo
    {
        /// <summary>
        /// - 数据库主键ID
        /// </summary>
        [Column("ID")]
        public int ID { get; set; }
        /// <summary>
        /// - 入库时间
        /// </summary>
        [Column("DATETIME")]
        public string DATETIME { get; set; }
        /// <summary>
        /// * 终端AID
        /// </summary>
        [Column("AID")]
        public string AID { get; set; }
        /// <summary>
        /// * 任务开始时间
        /// </summary>
        [Column("STARTTIME")]
        public string STARTTIME { get; set; }
        /// <summary>
        /// * 任务结束时间
        /// </summary>
        [Column("ENDTIME")]
        public string ENDTIME { get; set; }
        /// <summary>
        /// * 任务执行时间间隔，单位秒，建议设置默认0
        /// </summary>
        [Column("INTERVAL")]
        public int INTERVAL { get; set; }
        /// <summary>
        /// - 任务状态
        /// </summary>
        [Column("STATUS")]
        public string STATUS { get; set; }
        /// <summary>
        /// - 任务是否关闭 -1 未执行  0  正在执行   1 执行完毕
        /// </summary>
        [Column("ISCLOSED")]
        public int ISCLOSED { get; set; }
        /// <summary>
        /// * 任务类型 请设置为"QoEVideo"
        /// </summary>
        [Column("TYPE")]
        public string TYPE { get; set; }
        [Column("MISSIONBODY")]
        public string MISSIONBODY { get; set; }
        /// <summary>
        /// * 任务详细字段
        /// </summary>
        [NotMapped]
        public QoEMissionBody MissionBody { get; set; }
        public void ParseMissionBody()
        {
            if (string.IsNullOrEmpty(MISSIONBODY)) return;
            try
            {
                MissionBody = JsonConvert.DeserializeObject<QoEMissionBody>(MISSIONBODY);
            }
            catch (Exception e)
            {

            }
        }
    }
    /// <summary>
    /// QoE自动任务详细字段
    /// </summary>
    public class QoEMissionBody
    {
        /// <summary>
        /// * 请求视频类型 全部/电影/小视频/小品...
        /// </summary>
        public string VideoType { get; set; }
    }
}