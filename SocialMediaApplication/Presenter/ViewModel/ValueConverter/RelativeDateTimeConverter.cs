using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SocialMediaApplication.Presenter.ViewModel.ValueConverter
{
    public class RelativeDateTimeConvertor : IValueConverter
    {
        private const int Second = 1;
        private const int Minute = 60 * Second;
        private const int Hour = 60 * Minute;
        private const int Day = 24 * Hour;
        private const int Month = 30 * Day;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return string.Empty;

            var currentDay = DateTime.Today;
            var postedData = (DateTime)value;

            var ts = new TimeSpan(DateTime.Now.Ticks - postedData.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * Minute)
            {
                if (ts.Seconds < 0)
                {
                    return "sometime ago";
                }

                return ts.Seconds == 1 ? "1s" : ts.Seconds + "s";
            }

            if (delta < 2 * Minute)
                return "1m";

            if (delta < 45 * Minute)
            {
                if (ts.Seconds < 0)
                {
                    return "sometime ago";
                }

                return ts.Minutes + "m";
            }

            if (delta <= 90 * Minute)
                return "An hour ago";

            if (delta < 24 * Hour)
            {
                if (ts.Hours < 0)
                {
                    return "sometime ago";
                }

                if (ts.Hours == 1)
                    return "1h ago";

                return ts.Hours + "h";
            }

            if (delta < 48 * Hour)
                return $"Yesterday at {postedData:t}";

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
            else
            {
                int years = (int)(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "1y" : years + "y";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }
}
