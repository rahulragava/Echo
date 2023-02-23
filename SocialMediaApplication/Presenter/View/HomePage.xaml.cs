using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void NavigationMenu_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

            var selectedTag = args.SelectedItemContainer.Tag.ToString();

            switch (selectedTag)
            {
                case "HomePage":
                    //this.Frame.Navigate(typeof(HomePage));
                    ContentFrame.Navigate(typeof(FeedsPage));
                    break;
                case "SearchPage":
                    ContentFrame.Navigate(typeof(SearchPage));
                    break;
                case "CreatePage":
                    ContentFrame.Navigate(typeof(PostPage));
                    break;
                case "LabelPage":
                    ContentFrame.Navigate(typeof(LabelPage));
                    break;
                case "ProfilePage":
                    ContentFrame.Navigate(typeof(ProfilePage));
                    break;
                case "Logout":
                    var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    localSettings.Values.Remove("user");
                    this.Frame.Navigate(typeof(LoginInPage));
                    break;
            }
        }

        

        private void UserLogOut_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            App.LocalSettings.Values.Remove("user");
            this.Frame.Navigate(typeof(LoginInPage));
        }
    }
}
