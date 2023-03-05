using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Util;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UserAlreadyLoggedIn();
        }

        public void UserAlreadyLoggedIn()
        {
            //var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //this.Frame.Navigate(localSettings.Values["user"] == null ? typeof(SignUpPage) : typeof(HomePage));
            this.Frame.Navigate(AppSettings.LocalSettings.Values["user"] is null ? typeof(SignUpPage) : typeof(HomePage));
        }


        //to signin/singnup/logout
        //public void Dummy()
        //{AppSettings.LocalSettings.Values["user"].ToString();
        //    UserBObj userBObj = new UserBObj();
        //    var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        //    localSettings.Values["user"] = userBObj;


        //    localSettings.Values.Remove("user");
        //}
    }
}
