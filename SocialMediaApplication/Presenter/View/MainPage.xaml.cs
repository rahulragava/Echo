using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Presenter.View.SignUp;
using SocialMediaApplication.Util;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;

namespace SocialMediaApplication.Presenter.View
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            UserAlreadyLoggedIn();
        }

        public void UserAlreadyLoggedIn()
        {
            if (AppSettings.LocalSettings.Values["user"] is null)
            {
                SignIn.Visibility = Visibility.Visible;
            }
            else
            {
                LoginInPage_OnGoToHome();
            }
        }

        private void SignUpPage_OnGoToLogInControl()
        {
            SignUp.Visibility = Visibility.Collapsed;
            SignIn.Visibility = Visibility.Visible;
        }

        private void LoginInPage_OnGoToHome()
        {
            this.Frame.Navigate(typeof(HomePage));
        }

        private void LoginInPage_OnGoToSignUpControl()
        {
            SignUp.Visibility = Visibility.Visible;
            SignIn.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SignIn.Visibility = Visibility.Visible;
        }
    }
}
