using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UplanServer
{
    public class Utils
    {
        public static int IntNull2Int(int? d)
        {
            if (d == null) return 0;
            return (int)d;
        }
        public static double DoubleNull2Double(double? d, int deci = 0)
        {
            if (d == null) return 0;
            double rt = (double)d;
            if (deci > 0)
            {
                rt = Math.Round(rt, deci);
            }
            return rt;
        }
        public static bool IsNumberic(string str)
        {
            //Regex regExp = new Regex("^[0-9]*$");
            //return regExp.IsMatch(str);
            if (string.IsNullOrEmpty(str)) return false;
            try
            {
                double d = double.Parse(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool IsInt(string str)
        {
            //Regex regExp = new Regex("^[0-9]*$");
            //return regExp.IsMatch(str);
            if (string.IsNullOrEmpty(str)) return false;
            try
            {
                int d = int.Parse(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}