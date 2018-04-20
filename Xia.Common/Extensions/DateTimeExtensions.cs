// -----------------------------------------------------------------------
//  <copyright file="DateTimeExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>最后修改人</last-editor>
//  <last-date>2015-05-05 11:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;


namespace Xia.Common.Extensions
{
    /// <summary>
    /// 时间扩展操作类
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 当前时间是否周末
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime dateTime)
        {
            DayOfWeek[] weeks = { DayOfWeek.Saturday, DayOfWeek.Sunday };
            return weeks.Contains(dateTime.DayOfWeek);
        }

        /// <summary>
        /// 当前时间是否工作日
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekday(this DateTime dateTime)
        {
            DayOfWeek[] weeks = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            return weeks.Contains(dateTime.DayOfWeek);
        }

        /// <summary>
        /// 获取时间相对唯一字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="milsec">是否使用毫秒</param>
        /// <returns></returns>
        public static string ToUniqueString(this DateTime dateTime, bool milsec = false)
        {
            int sedonds = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;
            string value = string.Format("{0}{1}{2}", dateTime.ToString("yy"), dateTime.DayOfYear, sedonds);
            return milsec ? value + dateTime.ToString("fff") : value;
        }
        /// <summary>
        /// 获取一个时间的序列号 时间字符串+4个随机数字
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToSerialNumber(this DateTime dateTime)
        {
            return dateTime.ToUniqueString() + new Random().GetRandomNumberString(4);
        }

        /// <summary>
        /// 返回 间隔时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetTimeSpan(this DateTime dateTime)
        {
            string strTime = "";
            DateTime date1 = DateTime.Now;
            DateTime date2 = dateTime;
            TimeSpan dt = date1 - date2;

            // 相差天数
            int days = dt.Days;
            // 时间点相差小时数
            int hours = dt.Hours;
            // 相差总小时数
            double Minutes = dt.Minutes;
            // 相差总秒数
            int second = dt.Seconds;

            if (days == 0 && hours == 0 && Minutes == 0 && second == 0)
            {
                strTime = "刚刚";
            }
            else if (days == 0 && hours == 0 && Minutes == 0)
            {
                strTime = "约" + second + "秒前";
            }
            else if (days == 0 && hours == 0)
            {
                strTime = "约" + Minutes + "分钟前";
            }
            else if (days == 0)
            {
                strTime = "约" + hours + "小时前";
            }
            else
            {
                strTime = dateTime.ToString("yyyy-MM-dd");
            }
            return strTime;
        }
    }
}