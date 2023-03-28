using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using SocialMediaApplication.Presenter.ViewModel;

namespace SocialMediaApplication.Util
{
    public class StringToImageUtil
    {
        public async Task<BitmapImage> GetImageFromStringAsync(string imagePath)
        {
            BitmapImage image = new BitmapImage();
            try
            {
                if (imagePath != null)
                {
                    var storageFile = await StorageFile.GetFileFromPathAsync(imagePath);
                    using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        await image.SetSourceAsync(stream);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                //filenotfoundException
            }
            return image;
        }
    }
}
