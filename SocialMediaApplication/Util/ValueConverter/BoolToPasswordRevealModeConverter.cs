using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace SocialMediaApplication.Util.ValueConverter
{
    public class BoolToPasswordRevealModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? PasswordRevealMode.Hidden : PasswordRevealMode.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}