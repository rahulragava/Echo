using SocialMediaApplication.Presenter.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SocialMediaApplication.Presenter.View.ProfileView;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page, ISearchView
    {
        public SearchViewModel SearchViewModel;
        public SearchPage()
        {
            SearchViewModel = new SearchViewModel();
            this.InitializeComponent();
            Loaded += SearchPage_Loaded;
        }
        private void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            SearchViewModel.SearchView = this;
            SearchViewModel.GetUserNames();
        }

        public void NavigateToProfilePage()
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
    }

    public interface ISearchView
    {
        void NavigateToProfilePage();
    }
}
