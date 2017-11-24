using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Controller.Common.Helpers
{
    public class DateDel
    {
        #region 获取 本周、本月、本季度、本年 的开始时间或结束时间
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? GetTimeStartByType(string TimeType, DateTime now)
        {
            switch (TimeType)
            {
                case "Day":
                    return Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                case "Week":
                    return Convert.ToDateTime(now.AddDays(-(int)now.DayOfWeek + 1).ToString("yyyy-MM-dd 00:00:00"));
                case "Month":
                    return Convert.ToDateTime(now.AddDays(-now.Day + 1).ToString("yyyy-MM-dd 00:00:00"));;
                case "Season":
                    var time = now.AddMonths(0 - ((now.Month - 1) % 3));
                    return Convert.ToDateTime(time.AddDays(-time.Day + 1).ToString("yyyy-MM-dd 00:00:00"));;
                case "Year":
                    return Convert.ToDateTime(now.AddDays(-now.DayOfYear + 1).ToString("yyyy-MM-dd 00:00:00"));;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? GetTimeEndByType(string TimeType, DateTime now)
        {
            switch (TimeType)
            {
                case "Day":
                    return Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                case "Week":
                    return Convert.ToDateTime(now.AddDays(7 - (int)now.DayOfWeek).ToString("yyyy-MM-dd 23:59:59"));;
                case "Month":
                    return Convert.ToDateTime(now.AddMonths(1).AddDays(-now.AddMonths(1).Day + 1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));;
                case "Season":
                    var time = now.AddMonths((3 - ((now.Month - 1) % 3) - 1));
                    return Convert.ToDateTime(time.AddMonths(1).AddDays(-time.AddMonths(1).Day + 1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));;
                case "Year":
                    var time2 = now.AddYears(1);
                    return Convert.ToDateTime(time2.AddDays(-time2.DayOfYear).ToString("yyyy-MM-dd 23:59:59"));;
                default:
                    return null;
            }
        }
        #endregion
    }
}
