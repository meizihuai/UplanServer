using System;
using System.Collections.Generic;
using System.Text;

namespace UplanData2DbWorker
{
   public class Utils
    {
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
