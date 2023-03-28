using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SocialMediaApplication.Util.ValueConverter
{
    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new Thickness(System.Convert.ToInt32(value), -10, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
