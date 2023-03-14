using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.EntityModels;
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
                FeedPageViewModel.GetFeeds();
            }
            //_profilePageViewModel.GetUser();
            //Bindings.Update();
        }

        public void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                if (FeedPageViewModel.Success)
                {
                    FeedPageViewModel.ScrollPosition = scrollViewer.VerticalOffset;
                    var maxOffset = scrollViewer.ScrollableHeight;
                    if (maxOffset > 0 && FeedPageViewModel.ScrollPosition >= maxOffset - 100)
                    {
                        FeedPageViewModel.GetFeeds();
                    }
                }
            }
        }

        //private void PostControl_OnNavigateToSearchPage(string userId)
        //{

        //    Frame.Navigate(typeof(SearchPage), userId);
        //}
        private void PostControl_OnPostRemoved(string postId)
        {
            FeedPageViewModel.TextPosts.Remove(FeedPageViewModel.TextPosts.SingleOrDefault(p => p.Id == postId));
        }

        private void TextPostClicked_OnClick(object sender, RoutedEventArgs e)
        {
            TextPostClicked.IsChecked = true;
            PollPostClicked.IsChecked = false;
            TextPostListView.Visibility = Visibility.Visible;
            ListScroll.ScrollToVerticalOffset(0);
            PollPostListView.Visibility = Visibility.Collapsed;
        }

        private void PollPostClicked_OnClick(object sender, RoutedEventArgs e)
        {
            TextPostClicked.IsChecked = false;
            PollPostClicked.IsChecked = true;
            PollPostListView.Visibility = Visibility.Visible;
            ListScroll.ScrollToVerticalOffset(0);
            TextPostListView.Visibility = Visibility.Collapsed;

        }

        private void PostControl_OnReactionPopUpButtonClicked(List<Reaction> reactions)
        {
            ReactionsPopup.Visibility = Visibility.Visible;
            //ReactionsPopup.IsOpen = true;
            FeedPageViewModel.SetReactions(reactions);
        }

        private void HideReaction_OnClick(object sender, RoutedEventArgs e)
        {
            ReactionsPopup.Visibility = Visibility.Collapsed;
        }

        private void PostControl_OnReactionChanged(Reaction reaction)
        {
            FeedPageViewModel.Reaction = reaction;
            var reactions= FeedPageViewModel.Reactions.ToList();
            FeedPageViewModel.ChangeInReactions(reactions);
        }

       
    }
}
