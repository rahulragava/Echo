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
                        //FeedPageViewModel.IsLoading = true;
                    }
                }
            }
        }

        private void PostControl_OnPostRemoved(string postId)
        {
            FeedPageViewModel.PostBObjList.Remove(FeedPageViewModel.PostBObjList.SingleOrDefault(p => p.Id == postId));
        }

        private void PostControl_OnReactionPopUpButtonClicked(List<Reaction> reactions)
        {
            FeedPageViewModel.SetReactions(reactions);
            PopInStoryboard.Begin();
            PostReactionPopup.IsOpen = true;
        }

        private void PostControl_OnReactionChanged(Reaction reaction)
        {
            FeedPageViewModel.Reaction = reaction;
            var reactions = FeedPageViewModel.Reactions.ToList();
            FeedPageViewModel.ChangeInReactions(reactions);
        }

        private void PostControl_OnCommentReactionPopUpButtonClicked(List<Reaction> reactions)
        {
            FeedPageViewModel.SetCommentReactions(reactions);
            CommentPopInStoryboard.Begin();
            CommentReactionPopup.IsOpen = true;
        }



        private void MiniTextPostCreation_OnOnTextPostCreationSuccess(TextPostBObj textPost)
        {
            ExampleVsCodeInAppNotification.Show("Post  is Successfully Created!", 2000);
            FeedPageViewModel.PostBObjList.Insert(0, textPost);
        }

        private void PostControl_OnCommentReactionChanged(Reaction reaction)
        {
            FeedPageViewModel.CommentReaction = reaction;
            var reactions = FeedPageViewModel.CommentReactions.ToList();
            FeedPageViewModel.ChangeInCommentReactions(reactions);
        }

        private void PostControl_OnEditTextPostClicked(string textPostId)
        {
            FeedPageViewModel.TextPostBObj = FeedPageViewModel.PostBObjList.SingleOrDefault(t => t.Id == textPostId) as TextPostBObj;
            EditTextPopInStoryboard.Begin();
            EditTextPopup.IsOpen = true;
        }

        private void EditTextPostUserControl_OnCloseEdit(TextPostBObj obj)
        {
            EditTextPopOutStoryboard.Begin();
            EditTextPopup.IsOpen = false;
        }
    }
}
