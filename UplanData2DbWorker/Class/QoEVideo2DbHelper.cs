using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UplanServer;

namespace UplanData2DbWorker
{
   public  class QoEVideo2DbHelper
    {
        private static MTask mTask;
        private static MQCustomer mq;
        private static string queueName = MQConfig.MQ_Queue_QoE2DB;
        public static void Start()
        {
            mTask = new MTask(async () =>
            {
                while (!mTask.IsCancelled())
                {
                    bool flagNeedReConnect = true;
                    if (mq != null)
                    {
                        if (!mq.IsConnected())
                        {
                            mq.Dispose();
                            flagNeedReConnect = true;
                        }
                        else
                        {
                            flagNeedReConnect = false;
                        }
                    }
                    else
                    {
                        flagNeedReConnect = true;
                    }
                    if (flagNeedReConnect)
                    {
                        Program.Log("<QoE>连接MQ...");
                        mq = new MQCustomer(MQConfig.HostName, MQConfig.UserName, MQConfig.Password);
                        flagNeedReConnect = mq.Connect(queueName);

                        if (flagNeedReConnect)
                        {
                            Program.Log("<QoE>连接成功");
                            mq.OnMsg += Mq_OnMsg;
                        }
                    }
                    await Task.Delay(3000);
                }
            });
            mTask.Start();
        }

        private static Task<bool> Mq_OnMsg(string msg)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(msg)) return true;
                    QoEVideoTable qoe = JsonConvert.DeserializeObject<QoEVideoTable>(msg);
                    if (qoe == null) return true;
                    Program.Log($"上传 QoE , AID={qoe.AID} , DateTime={qoe.DATETIME}");
                    //if (qoe.LIGHT_INTENSITY_list == null)
                    //{
                    //    //return new NormalResponse(false, "qoe.LIGHT_INTENSITY_list==null");
                      
                    //    Program.Log("qoe.LIGHT_INTENSITY_list==null");
                    //}
                    //else
                    //{
                    //    File.WriteAllText(@"d:\err.txt", $"qoe.LIGHT_INTENSITY_list.Count={qoe.LIGHT_INTENSITY_list.Count}");
                    //    //return new NormalResponse(false, $"qoe.LIGHT_INTENSITY_list.Count={qoe.LIGHT_INTENSITY_list.Count}");
                    //}
                    NormalResponse np = await HttpHelper.Post("http://localhost:7063/api/Data2Db/UploadQoEVideo", qoe);
                    Program.Log($"      ==>{np.result}");
                }
                catch (Exception e)
                {

                }
                return true;
            });
        }
    }
}
