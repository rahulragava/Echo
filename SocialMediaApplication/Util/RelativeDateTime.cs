using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace SocialMediaApplication.Util
{
    public static class RelativeDateTime
    {
        private const int Second = 1;
        private const int Minute = 60 * Second;
        private const int Hour = 60 * Minute;
        private const int Day = 24 * Hour;
        private const int Month = 30 * Day;
        private static readonly ResourceLoader ResourceLoader = ResourceLoader.GetForCurrentView();

        public static string DateTimeConversion(DateTime createdTime)
        {
            var postedData = (DateTime)createdTime;

            var ts = new TimeSpan(DateTime.Now.Ticks - postedData.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * Minute)
            {
                if (ts.Seconds < 0)
                {
                    return ResourceLoader.GetString("SomeTimeAgo");
                }

                return ts.Seconds < 30 ? ResourceLoader.GetString("JustNow") : ts.Seconds + "s " +" ago";
            }

            if (delta < 2 * Minute)
                return "1m";

            if (delta < 45 * Minute)
            {
                if (ts.Seconds < 0)
                {
                    return ResourceLoader.GetString("SomeTimeAgo");

                }

                return ts.Minutes + "m";
            }

            if (delta <= 90 * Minute)
                return "OneHourAgo";

            if (delta < 24 * Hour)
            {
                if (ts.Hours < 0)
                {
                    return ResourceLoader.GetString("SomeTimeAgo");
                }

                if (ts.Hours == 1)
                    return "1h ago";

                return ts.Hours + "h";
            }

            if (delta < 48 * Hour)
                return ResourceLoader.GetString("Yesterday") + $"{postedData:t}";

            if (delta < 30 * Day)
            {
                if (ts.Days == 1)
                    return "1 d";

                return ts.Days + "d";
            }


            if (delta < 12 * Month)
            {
                int months = (int)(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "1M" : months + "M";
            }

            int years = (int)(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "1y" : years + "y";
        }
    }
}
