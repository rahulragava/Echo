using System;
using Windows.UI.Xaml.Data;

namespace SocialMediaApplication.Util.ValueConverter
{
    public class RelativeCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var count = (int)value;
            if (count > 1000 && count <= 999999)
            {
                return $"{(double)count / 1000} k";
            }

            if (count > 1_000_000)
            {
                return $"{(double)count / 1_000_000} m";
            }

            return $"{count}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
