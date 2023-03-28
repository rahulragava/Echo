using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.Presenter.View.ProfileView;
using SocialMediaApplication.Presenter.View.CommentView;
using SocialMediaApplication.Presenter.View.FeedView;
using SocialMediaApplication.Presenter.View.LogInView;
using SocialMediaApplication.Presenter.View.PostView;
using SocialMediaApplication.Presenter.View.PostView.PollPostView;
using SocialMediaApplication.Presenter.View.PostView.TextPostView;
using SocialMediaApplication.Util;
using Windows.UI.ViewManagement;
using Windows.UI;

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page,INotifyPropertyChanged
    {
        public HomePage()
        {
            this.InitializeComponent();
            FrameworkElement root = (FrameworkElement)Window.Current.Content;
            root.RequestedTheme = AppSettings.Theme;
            SetThemeToggle(AppSettings.Theme);
            Loaded += HomePage_Loaded;
            Unloaded += HomePage_Unloaded;
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            TextPostUserControl.NavigateToSearchPage += UserControlOnNavigateToSearchPage;
            CommentUserControl.NavigateToSearchPage += UserControlOnNavigateToSearchPage;
            PollPostUserControl.NavigateToSearchPage += UserControlOnNavigateToSearchPage;
            UserListUserControl.NavigateToSearchPage += UserControlOnNavigateToSearchPage;
            UserListPage.NavigateToUser += UserListPageOnNavigateToUser;
            ProfilePage.NavigateToPostCreationPage += ProfilePageOnNavigateToPostCreationPage;
            NavigationViewItem itemContent = NavigationMenu.MenuItems.ElementAt(0) as NavigationViewItem;
            NavigationMenu.SelectedItem = itemContent;
        }

        private void HomePage_Unloaded(object sender, RoutedEventArgs e)
        {
            TextPostUserControl.NavigateToSearchPage -= UserControlOnNavigateToSearchPage;
            CommentUserControl.NavigateToSearchPage -= UserControlOnNavigateToSearchPage;
            PollPostUserControl.NavigateToSearchPage -= UserControlOnNavigateToSearchPage;
            ProfilePage.NavigateToPostCreationPage -= ProfilePageOnNavigateToPostCreationPage;
            UserListUserControl.NavigateToSearchPage -= UserControlOnNavigateToSearchPage;
            UserListPage.NavigateToUser -= UserListPageOnNavigateToUser;
        }
        
        private void UserListPageOnNavigateToUser(string userId)
        {
            UserId = userId;
            SearchPageParameter = true;
            NavigationViewItem itemContent = NavigationMenu.MenuItems.ElementAt(1) as NavigationViewItem;
            NavigationMenu.SelectedItem = itemContent;
            ContentFrame.Navigate(typeof(SearchPage), UserId);
            SearchPageParameter = false;
        }

        private void ProfilePageOnNavigateToPostCreationPage()
        {
            NavigationViewItem itemContent = NavigationMenu.MenuItems.ElementAt(2) as NavigationViewItem;
            NavigationMenu.SelectedItem = itemContent;

        }

        private void SetThemeToggle(ElementTheme theme)
        {
            if (theme == AppSettings.LightTheme)
            {
                ThemeIcon = "";
                ThemeChangerNavigationItem.Content = "Dark";
                var titleBar = AppSettings.TitleBar;
                titleBar.BackgroundColor = Colors.White;
                titleBar.ForegroundColor = Colors.Black;
                titleBar.ButtonBackgroundColor = Colors.White;
                titleBar.ButtonForegroundColor = Colors.Black;

                //ThemeChanger.Glyph = "&#xE945;";
            }
            else
            {
                //ThemeChanger.Glyph = "&#E793;";
                ThemeIcon = "";
                var titleBar = AppSettings.TitleBar;
                titleBar.BackgroundColor = Colors.Black;
                titleBar.ForegroundColor = Colors.White;
               
                titleBar.ButtonBackgroundColor = Colors.Black;
                titleBar.ButtonForegroundColor = Colors.White;
                ThemeChangerNavigationItem.Content = "Light";

            }
        }

        public readonly List<string> LanguageList = new List<string>(){"En","Ta"};
        
        

        private void NavigationMenu_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

            var selectedTag = args.SelectedItemContainer.Tag.ToString();

            switch (selectedTag)
            {
                case "HomePage":
                    NavigationMenu.IsPaneOpen = false;
                    //this.Frame.Navigate(typeof(HomePage));
                    ContentFrame.Navigate(typeof(FeedsPage));
                    break;
                case "SearchPage":
                    NavigationMenu.IsPaneOpen = false;
                    if (SearchPageParameter)
                    {
                        ContentFrame.Navigate(typeof(SearchPage),UserId);
                        SearchPageParameter = false;
                    }
                    else
                    {
                        ContentFrame.Navigate(typeof(SearchPage));
                    }
                    break;
                case "CreatePage":
                    NavigationMenu.IsPaneOpen = false;
                    ContentFrame.Navigate(typeof(PostPage));
                    break;
                case "ProfilePage":
                    NavigationMenu.IsPaneOpen = false;
                    ContentFrame.Navigate(typeof(ProfilePage),AppSettings.UserId);
                    break;
                case "Logout":
                    //var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    //localSettings.Values.Remove("user");
                    AppSettings.LocalSettings.Values.Remove("user");
                    AppSettings.UserId = null;
                    this.Frame.Navigate(typeof(MainPage));
                    break;
            }
        }

        public bool SearchPageParameter = false;
        public string UserId { get; set; }
        private void UserControlOnNavigateToSearchPage(string userId)
        {
            UserId = userId;
            SearchPageParameter = true;
            NavigationViewItem itemContent = NavigationMenu.MenuItems.ElementAt(1) as NavigationViewItem;
            NavigationMenu.SelectedItem = itemContent;
        }

        private void UserLogOut_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            AppSettings.LocalSettings.Values.Remove("user");
            AppSettings.UserId = null;
            this.Frame.Navigate(typeof(MainPage));

        }

        private void ThemeChanger_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement window = (FrameworkElement)Window.Current.Content;

            if (ThemeIcon == "")
            {
                AppSettings.Theme = AppSettings.DarkTheme;
                var titleBar = AppSettings.TitleBar;
                titleBar.BackgroundColor = Colors.Black;
                titleBar.ForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = Colors.Black;
                titleBar.ButtonForegroundColor = Colors.White;
                window.RequestedTheme = AppSettings.DarkTheme;
                ThemeChangerNavigationItem.Content = "Light";
                //ThemeChanger.Glyph = "&#E793;";
                ThemeIcon = "";
            }
            else
            {
                AppSettings.Theme = AppSettings.LightTheme;
                window.RequestedTheme = AppSettings.LightTheme;
                //ThemeChanger.Glyph = "&#xE945;";
                var titleBar = AppSettings.TitleBar;
                titleBar.BackgroundColor = Colors.White;
                titleBar.ForegroundColor = Colors.Black;
                titleBar.ButtonBackgroundColor = Colors.White;
                titleBar.ButtonForegroundColor = Colors.Black;
                ThemeChangerNavigationItem.Content = "Dark";
                ThemeIcon = "";

            }
        }

        private string _themeIcon;

        public string ThemeIcon
        {
            get => _themeIcon;
            set => SetField(ref _themeIcon, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}