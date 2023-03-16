//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Storage;
//using Windows.Storage.Streams;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Media.Imaging;

//namespace SocialMediaApplication.Presenter.ViewModel.ValueConverter
//{
//    public class StringToImageConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, string language)
//        {
//            if (value != null)
//            {
//                string name = (string)value;

//                var requestImage = new Image
//                {
//                    Height = 16,
//                    Width = 16,
//                    HorizontalAlignment = HorizontalAlignment.Center,
//                    Source = new BitmapImage(new Uri("pack://application:,,,/Images/service.png"))
//                };

//                return requestImage;
//            }
//            else return null;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, string language)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
