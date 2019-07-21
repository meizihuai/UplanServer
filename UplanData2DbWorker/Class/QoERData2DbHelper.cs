using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UplanServer;

namespace UplanData2DbWorker
{
    public static class QoERData2DbHelper
    {
        private static MTask mTask;
        private static MQCustomer mq;
        private static string queueName = MQConfig.MQ_Queue_QoER2DB;
        public static void Start()
        {
            mTask = new MTask(async() =>
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
                        Program.Log("<QoER>连接MQ...");
                        mq = new MQCustomer(MQConfig.HostName, MQConfig.UserName, MQConfig.Password);
                        flagNeedReConnect = mq.Connect(queueName);

                        if (flagNeedReConnect)
                        {
                            Program.Log("<QoER>连接成功");
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
            return Task.Run(async() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(msg)) return true;
                    PhoneInfo pi = JsonConvert.DeserializeObject<PhoneInfo>(msg);
                    if (pi == null) return true;
                    Program.Log($"上传 QoER , AID={pi.AID} , DateTime={pi.DateTime}");
                    NormalResponse np = await HttpHelper.Post("http://localhost:7063/api/Data2Db/UploadQoER", pi);
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
