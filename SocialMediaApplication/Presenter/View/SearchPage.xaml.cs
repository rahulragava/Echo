using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SocialMediaApplication.Presenter.View.ProfileView;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchViewModel SearchViewModel;
        public SearchPage()
        {
            SearchViewModel = new SearchViewModel();
            this.InitializeComponent();
            Loaded += SearchPage_Loaded;
        }
        private void SearchPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SearchViewModel.NavigateToProfilePage += NavigateToProfilePage;
            SearchViewModel.GetUserNames();
            //Bindings.Update();
        }

        private void NavigateToProfilePage(object sender, EventArgs e)
        {
            UserProfileFrame.Navigate(typeof(ProfilePage), SearchViewModel.UserId);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string userId)
            {
                SearchViewModel.UserId = userId;
                UserProfileFrame.Navigate(typeof(ProfilePage), SearchViewModel.UserId);
            }
        }


        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                SearchViewModel.FilterSuggestions(sender.Text);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            SearchViewModel.PerformSearch(args.QueryText);
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //if (args.SelectedItem is string selectedUserName)
            //{
            //    SearchViewModel.PerformSearch(selectedUserName);
            //}
        }
    }
}
