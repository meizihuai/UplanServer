using System;
using System.Collections.Generic;
using System.Text;

namespace UplanServer
{
  public  class ScoreHelper
    {
        /// <summary>
        /// 加载速度评分
        /// </summary>
        /// <param name="iniTime"></param>
        /// <param name="bufferTotalTime"></param>
        /// <param name="totalTime"></param>
        /// <returns></returns>
        public static  int GetVideo_LOAD_Score(int? iniTime, int? bufferTotalTime, int? totalTime)
        {
            iniTime = Utils.IntNull2Int(iniTime);
            bufferTotalTime = Utils.IntNull2Int(bufferTotalTime);
            totalTime = Utils.IntNull2Int(totalTime);
            float iniScore = 1;
            if (iniTime <= 1000)
                iniScore = 5;
            if (1000 < iniTime & iniTime <= 2000)
                iniScore = 4;
            if (iniTime < 2000 & iniTime <= 3000)
                iniScore = 3;
            if (iniTime < 3000 & iniTime <= 5000)
                iniScore = 2;
            if (iniTime > 5000)
                iniScore = 1;

            double bvRate = 100 * Utils.IntNull2Int(bufferTotalTime) / (double)totalTime;
            int bvRateScore = GetVideo_Buffer_Total_Score(bvRate);
            double score = bvRateScore * 0.5 + iniScore * 0.5;
            return (int)Math.Floor(score);
        }
        // 视频卡顿评分
        public static int GetVideo_STALL_Score(int? stallTime, int? stallCount, int? totalTime)
        {
            stallTime = Utils.IntNull2Int(stallTime);
            stallCount = Utils.IntNull2Int(stallCount);
            totalTime = Utils.IntNull2Int(totalTime);

            double Score = 1;
            double p = 100 * Utils.IntNull2Int(stallTime) / (double)totalTime;
            if (p == 0)
                Score = 5;
            if (0 < p & p <= 0.1)
                Score = 4;
            if (0.1 < p & p <= 1)
                Score = 3;
            if (1 < p & p <= 5)
                Score = 2;
            if (5 < p)
                Score = 1;

            if (stallCount == 0)
                Score = (Score + 5) / (double)2;
            if (stallCount == 1)
                Score = (Score + 4) / (double)2;
            if (stallCount == 2)
                Score = (Score + 3) / (double)2;
            if (stallCount == 3)
                Score = (Score + 2) / (double)2;
            if (stallCount > 3)
                Score = (Score + 1) / (double)2;

            return (int)Math.Floor(Score);
        }
        // 缓存评分
        public static  int GetVideo_Buffer_Total_Score(double? bVRate)
        {
            bVRate = Utils.DoubleNull2Double(bVRate);

            if (bVRate <= 10)
                return 5;
            if (bVRate <= 30)
                return 4;
            if (bVRate <= 50)
                return 3;
            if (bVRate <= 70)
                return 2;
            return 1;
        }
        // VMOS 总评分
        public static  float GetVMOS(int Video_LOAD_Score, int Video_STALL_Score)
        {
            double d = 0.5 * Video_LOAD_Score + 0.5 * Video_STALL_Score;
            // Dim i As Single = Math.Ceiling(d)
            return (float)Math.Round(d, 2);
        }
        /// <summary>
        /// HTTP响应时间评分
        /// </summary>
        /// <param name="responseRime"></param>
        /// <returns></returns>
        public static int GetQoEHttpResponseScore(long responseRime)
        {
            if ((responseRime > 5000))
                return 1;
            if ((responseRime > 3000))
                return 2;
            if ((responseRime > 2000))
                return 3;
            if ((responseRime > 500))
                return 4;
            return 5;
        }
    }
}
