using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Controller.Common
{
    public class DatenumConvert
    {
        #region 日期转换

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dcm"></param>
        /// <returns>2015-03-18格式</returns>
        public static string DatedecimalToString(decimal dcm)
        {
            string dcmStr = dcm.ToString();
            return dcmStr.Substring(0, 4) + "-" + dcmStr.Substring(4, 2) + "-" + dcmStr.Substring(6, 2);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">2015-03-18格式</param>
        /// <returns>20150318</returns>
        public static decimal DatestrToDecimal(string date)
        {
            string[] dates = date.Split(new char[] { '-' });
            return Convert.ToDecimal(dates[0] + dates[1] + dates[2]);

        }
        
        #endregion

        #region 时间转换
        public static string TimedecimalToString(decimal dcm)
        {
            string dcmStr = dcm.ToString();
            return dcmStr.Substring(0, 2) + ":" + dcmStr.Substring(2, 2) + ":" + dcmStr.Substring(4, 2);

        }
        public static decimal TimestrToDecimal(string time)
        {
            string[] times = time.Split(new char[] { ':' });
            return Convert.ToDecimal(times[0] + times[1] + times[2]);

        }
        #endregion
    }
}
