#nullable enable

namespace Utility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using Modele;

    public static class Utility
    {
        public static string GetID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string? GetEnumDescription(this Enum? enumVal)
        {
            var memInfo = enumVal?.GetType().GetMember(enumVal.ToString());
            var attribute = (memInfo?[0] ?? throw new InvalidOperationException())
                .GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description;
        }

        public static Timeframe ToTimeFrame(this string tf)
        {
            switch (tf)
            {
                case "1m":
                    return Timeframe.OneMinute;

                case "5m":
                    return Timeframe.FiveMinutes;

                case "15m":
                    return Timeframe.FifteenMinutes;

                case "30m":
                    return Timeframe.ThirtyMinutes;

                case "1h":
                    return Timeframe.OneHour;

                case "4h":
                    return Timeframe.FourHour;

                case "1d":
                    return Timeframe.Daily;

                case "1w":
                    return Timeframe.Weekly;

                case "1mn":
                    return Timeframe.Monthly;

                default:
                    throw new Exception("tf n'existe pas");
            }
        }

        public static DateTime ConvertToDatetime(this double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return origin.AddSeconds(timestamp);
        }

        public static DateTime ConvertToDatetime(this long? timestamp)
        {
            var test = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Convert.ToDouble(timestamp))
                .ToLocalTime();
            return test;
            var date = (long)timestamp;
            return DateTimeOffset.FromUnixTimeMilliseconds(date).DateTime.ToLocalTime();
        }

        public static long ConvertToUnixTime(this DateTime dateTime)
        {
            var datetimeOFfset = new DateTimeOffset(dateTime);
            return datetimeOFfset.ToUnixTimeMilliseconds();
        }

        public static void AddMany<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
        }
    }
}