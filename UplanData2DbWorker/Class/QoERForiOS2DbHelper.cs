using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UplanServer;

namespace UplanData2DbWorker
{
    public static class QoERForiOS2DbHelper
    {
        private static MTask mTask;
        private static MQCustomer mq;
        private static string queueName = MQConfig.MQ_Queue_QoERForiOS2DB;
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
                       // Console.WriteLine("1");
                        Program.Log("<QoER For iOS>连接MQ...");
                       // Console.WriteLine("2");
                        mq = new MQCustomer(MQConfig.HostName, MQConfig.UserName, MQConfig.Password);
                        flagNeedReConnect = mq.Connect(queueName);
                        if (flagNeedReConnect)
                        {
                            Program.Log("<QoER For iOS>连接成功");
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
                    QoEReportIOSInfo qoer = JsonConvert.DeserializeObject<QoEReportIOSInfo>(msg);
                    if (qoer == null) return true;
                    Program.Log($"上传 QoER For iOS , AID={qoer.AID} , DateTime={qoer.DateTime}");
                    NormalResponse np = await HttpHelper.Post("http://localhost:7063/api/Data2Db/UploadQoERForiOS", qoer);
                    Program.Log($"      ==>{np.result}");
                }
                catch (Exception e)
                {
                    Program.Log(e.ToString());
                }
                return true;
            });
        }
    }
}
