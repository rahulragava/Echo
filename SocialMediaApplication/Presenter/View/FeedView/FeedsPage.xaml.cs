using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Presenter.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View.FeedView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedsPage : Page
    {
        public FeedPageViewModel FeedPageViewModel;
        public FeedsPage()
        {
            FeedPageViewModel = new FeedPageViewModel();
            this.InitializeComponent();
            Loaded += FeedsPage_Loaded;

        }
        private void FeedsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (FeedPageViewModel != null)
            {
                FeedPageViewModel.GetFeeds(0,0,0,0);
            }
            //_profilePageViewModel.GetUser();
            //Bindings.Update();
        }

    }
}
