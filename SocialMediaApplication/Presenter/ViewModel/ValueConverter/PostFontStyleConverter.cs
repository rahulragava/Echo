using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using SocialMediaApplication.Models.Constant;

namespace SocialMediaApplication.Presenter.ViewModel.ValueConverter
{
    public class PostFontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var family = new FontFamily("Arial");
            if (value is PostFontStyle)
            {
                switch (value)
                {
                    case PostFontStyle.Simple:
                        family = new FontFamily("Arial");
                        break;
                    case PostFontStyle.Casual:
                        family = new FontFamily("Comic Sans MS");
                        break;
                    case PostFontStyle.Fancy:
                        family = new FontFamily("Segoe Print");
                        break;
                }
            }

            return family;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
