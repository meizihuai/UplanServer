﻿using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace UplanData2DbWorker
{
    public class TimeUtil
    {
        public static DateTime GetCstDateTime()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            var shanghaiZone = DateTimeZoneProviders.Tzdb["Asia/Shanghai"];
            return now.InZone(shanghaiZone).ToDateTimeUnspecified();
        }
        public static DateTime Now()
        {
            return DateTime.Now.ToCstTime();
        }
    }
    public static class DateTimeExtentions
    {
        public static DateTime ToCstTime(this DateTime time)
        {
            return TimeUtil.GetCstDateTime();
        }
    }
}
