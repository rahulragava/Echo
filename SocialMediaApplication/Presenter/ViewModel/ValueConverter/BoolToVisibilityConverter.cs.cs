using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SocialMediaApplication.Presenter.ViewModel.ValueConverter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var res = (bool)value ? Visibility.Visible : Visibility.Collapsed;
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
