using System;

namespace UplanData2DbWorker
{
    class Program
    {
        private static readonly string title = "UplanData2DbWorker V 1.0.1";
        static void Main(string[] args)
        {
            Run();
            Console.ReadLine();
        }
        static void Run()
        {
            Log("开启 QoER 入库线程...");
            QoERData2DbHelper.Start();
            Log("开启 QoER For iOS 入库线程...");
            QoERForiOS2DbHelper.Start();
            Log("开启 QoE Video 入库线程...");
            QoEVideo2DbHelper.Start();

        }
       public static void Log(string str)
        {
            Console.WriteLine(TimeUtil.Now().ToString("[HH:mm:ss] ") + str);
        }
    }
}
