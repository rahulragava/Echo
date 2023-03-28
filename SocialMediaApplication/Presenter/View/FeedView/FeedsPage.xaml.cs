using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.Models.BusinessModels;
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

        private void PostControl_OnPostRemoved(string postId)
        {
            FeedPageViewModel.PostBObjList.Remove(FeedPageViewModel.PostBObjList.SingleOrDefault(p => p.Id == postId));
        }

        private void TextPostClicked_OnClick(object sender, RoutedEventArgs e)
        {
            TextPostListView.Visibility = Visibility.Visible;
            ListScroll.ScrollToVerticalOffset(0);
        }

        private void PollPostClicked_OnClick(object sender, RoutedEventArgs e)
        {
            ListScroll.ScrollToVerticalOffset(0);
            TextPostListView.Visibility = Visibility.Collapsed;
        }

        private void PostControl_OnReactionPopUpButtonClicked(List<Reaction> reactions)
        {
            if (ReactionsPopup.Visibility == Visibility.Visible)
            {
                ReactionsPopup.Visibility = Visibility.Collapsed;
            }

            if (CommentReactionsPopup.Visibility == Visibility.Visible)
            {
                CommentReactionsPopup.Visibility = Visibility.Collapsed;
            }
            ReactionsPopup.Visibility = Visibility.Visible;
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

        private void MiniTextPostCreation_OnOnTextPostCreationSuccess(TextPostBObj textPost)
        {
            ExampleVsCodeInAppNotification.Show("Post  is Successfully Created!", 2000);
            FeedPageViewModel.PostBObjList.Insert(0, textPost);
        }

        private void PostControl_OnCommentReactionPopUpButtonClicked(List<Reaction> reactions)
        {
            if (ReactionsPopup.Visibility == Visibility.Visible)
            {
                ReactionsPopup.Visibility = Visibility.Collapsed;
            }

            if (CommentReactionsPopup.Visibility == Visibility.Visible)
            {
                CommentReactionsPopup.Visibility = Visibility.Collapsed;
            }
            FeedPageViewModel.SetCommentReactions(reactions);
            CommentReactionsPopup.Visibility = Visibility.Visible;
        }

        private void HideCommentReaction_OnClick(object sender, RoutedEventArgs e)
        {
            CommentReactionsPopup.Visibility = Visibility.Collapsed;
        }
    }
}
