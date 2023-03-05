using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using SocialMediaApplication.Presenter.View.ProfileView;
using Windows.UI.Xaml.Controls.Primitives;
using SocialMediaApplication.Presenter.View.FeedView;
using SocialMediaApplication.Presenter.View.PostView;
using SocialMediaApplication.Util;

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
            FrameworkElement root = (FrameworkElement)Window.Current.Content;
            root.RequestedTheme = AppSettings.Theme;
            SetThemeToggle(AppSettings.Theme);
        }

        private void SetThemeToggle(ElementTheme theme)
        {
            if (theme == AppSettings.LightTheme)
            {
                
                ThemeChanger.Glyph = "&#xE945;";
            }
            else
            {
                ThemeChanger.Glyph = "&#E793;";
            }
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
            AppSettings.LocalSettings.Values.Remove("user");
            this.Frame.Navigate(typeof(LoginInPage));
        }

        private void ThemeChanger_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement window = (FrameworkElement)Window.Current.Content;

            if (ThemeChanger.Glyph.ToString() == "&#xE945;")
            {
                AppSettings.Theme = AppSettings.DarkTheme;
                window.RequestedTheme = AppSettings.DarkTheme;
                ThemeChanger.Glyph = "&#E793;";
            }
            else
            {
                AppSettings.Theme = AppSettings.LightTheme;
                window.RequestedTheme = AppSettings.LightTheme;
                ThemeChanger.Glyph = "&#xE945;";
            }
        }
    }
}
