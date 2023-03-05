using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Models.EntityModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using SocialMediaApplication.Models.Constant;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.PostView
{
    public sealed partial class PollPostUserControl : UserControl
    {
        public PostControlViewModel PostControlViewModel;
        //public ObservableCollection<PollChoiceBObj> PollChoices;

        public PollPostUserControl()
        {
            PostControlViewModel = new PostControlViewModel();
            this.InitializeComponent();
            Loaded += PostControl_Loaded;
            //var popUpWidth = Window.Current.Bounds.Width;
            //var popUpHeight= Window.Current.Bounds.Height;
            //CommentPopUp.Width = popUpWidth;
            //CommentPopUp.Height = popUpHeight;
            //CommentHoldingStackPanel.Width = popUpWidth;
            //CommentHoldingStackPanel.Height = popUpHeight;
            //CommentHoldingStackPanel.Width = CommentPopUp.Width - 400;
            //CommentHoldingStackPanel.Height = CommentPopUp.Height - 400;
        }


        private void PostControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PostControlViewModel.SetObservableCollection(PostReaction, PostComments);
            PostControlViewModel.SetPollChoiceCollection(PollChoiceList);
            foreach (var reaction in PostReaction)
            {
                if (reaction.ReactedBy == App.UserId)
                {
                    switch (reaction.ReactionType)
                    {
                        case ReactionType.Heart:
                            HeartReaction.IsChecked = true;
                            break;
                        case ReactionType.Happy:
                            HappyReaction.IsChecked = true;
                            break;
                        case ReactionType.HeartBreak:
                            HeartBreakReaction.IsChecked = true;
                            break;
                        case ReactionType.Mad:
                            MadReaction.IsChecked = true;
                            break;
                        case ReactionType.Sad:
                            SadReaction.IsChecked = true;
                            break;
                        case ReactionType.ThumbsDown:
                            DisLikeReaction.IsChecked = true;
                            break;
                        case ReactionType.ThumbsUp:
                            LikeReaction.IsChecked = true;
                            break;
                    }
                }
            }
        }

        private void ClearAndUpdate(ObservableCollection<PollChoiceBObj> pollChoices)
        {
            //PollChoiceList.Clear();
            //foreach (var choice in PollChoiceList)
            //{
            //    pollChoices.Add(choice);
            //}
        }

        //private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        //{
        //    // open the Popup if it isn't open already 
        //    CommentPopUp.Visibility = Visibility.Visible;
        //    if (!CommentPopUp.IsOpen) { CommentPopUp.IsOpen = true; }

        //}

        //private void ClosePopupClicked(object sender, RoutedEventArgs e)
        //{
        //    // if the Popup is open, then close it 
        //    if (CommentPopUp.IsOpen) { CommentPopUp.IsOpen = false; }
        //    CommentPopUp.Visibility = Visibility.Collapsed;
        //}


        //public double GetPostSpecificVotePercentage()
        //{
        //    return ((pollChoice.ChoiceSelectedUsers.Count * 100) / GetTotalVotes());
        //}

        //public int GetTotalVotes()
        //{
        //    var totalVotes = 0;
        //    foreach (var pollChoice in PollChoiceList)
        //    {
        //        totalVotes += pollChoice.ChoiceSelectedUsers.Count;
        //    }
        //    return totalVotes;
        //}
        //username
        public static readonly DependencyProperty PostedByUserProperty = DependencyProperty.Register(
            nameof(PostedByUser), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostedByUser
        {
            get => (string)GetValue(PostedByUserProperty);
            set => SetValue(PostedByUserProperty, value);
        }

        //content
        public static readonly DependencyProperty PostQuestionProperty = DependencyProperty.Register(
            nameof(PostQuestion), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostQuestion
        {
            get => (string)GetValue(PostQuestionProperty);
            set => SetValue(PostQuestionProperty, value);
        }

        //post created at 
        public static readonly DependencyProperty PostCreatedAtProperty = DependencyProperty.Register(
            nameof(PostCreatedAt), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostCreatedAt
        {
            get => (string)GetValue(PostCreatedAtProperty);
            set => SetValue(PostCreatedAtProperty, value);
        }

        //title
        public static readonly DependencyProperty PostTitleProperty = DependencyProperty.Register(
            nameof(PostTitle), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostTitle
        {
            get => (string)GetValue(PostTitleProperty);
            set => SetValue(PostTitleProperty, value);
        }

        public static readonly DependencyProperty PollChoicesProperty = DependencyProperty.Register(
            nameof(PollChoiceList), typeof(List<PollChoiceBObj>), typeof(PollPostUserControl), new PropertyMetadata(default(List<PollChoiceBObj>)));

        public List<PollChoiceBObj> PollChoiceList
        {
            get => (List<PollChoiceBObj>)GetValue(PollChoicesProperty);
            set => SetValue(PollChoicesProperty, value);
        }

        //comments
        public static readonly DependencyProperty PostCommentsProperty = DependencyProperty.Register(
            nameof(PostComments), typeof(List<CommentBObj>), typeof(TextPostUserControl), new PropertyMetadata(default(List<CommentBObj>)));

        public List<CommentBObj> PostComments
        {
            get => (List<CommentBObj>)GetValue(PostCommentsProperty);
            set => SetValue(PostCommentsProperty, value);
        }

        //reaction
        public static readonly DependencyProperty PostReactionProperty = DependencyProperty.Register(
            nameof(PostReaction), typeof(List<Reaction>), typeof(TextPostUserControl), new PropertyMetadata(default(List<Reaction>)));

        public List<Reaction> PostReaction
        {
            get => (List<Reaction>)GetValue(PostReactionProperty);
            set => SetValue(PostReactionProperty, value);
        }

        public static readonly DependencyProperty PostIdProperty = DependencyProperty.Register(
            nameof(PostId), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostId
        {
            get => (string)GetValue(PostIdProperty);
            set => SetValue(PostIdProperty, value);
        }

        private void DisLikeReaction_OnClick(object sender, RoutedEventArgs e)
        {
            //already added reaction is removed and new reaction is added
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                switch (reactionType)
                {
                    case ReactionType.Heart:
                        if (HeartReaction.IsChecked != null && (bool)HeartReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Heart, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
                            HeartReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Happy:
                        if (HappyReaction.IsChecked != null && (bool)HappyReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Happy, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
                            HappyReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.HeartBreak:
                        if (HeartBreakReaction.IsChecked != null && (bool)HeartBreakReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.HeartBreak, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
                            HeartBreakReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Mad:
                        if (MadReaction.IsChecked != null && (bool)MadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Mad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
                            MadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Sad:
                        if (SadReaction.IsChecked != null && (bool)SadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Sad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
                            SadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsUp:
                        if (LikeReaction.IsChecked != null && (bool)LikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsUp, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
                            LikeReaction.IsChecked = false;
                            return;
                        }
                        break;
                }
            }

            //just new reaction is added
            if (DisLikeReaction.IsChecked != null && (bool)DisLikeReaction.IsChecked)
            {
                PostControlViewModel.ReactedToPost(ReactionType.None,
                    new Reaction()
                    { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown },
                    PostId);
            }

            //reaction by user is removed
            else
            {
                PostControlViewModel.ReactedToPost(ReactionType.None, null, PostId);
            }
        }

        private void HeartReaction_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                switch (reactionType)
                {
                    case ReactionType.ThumbsDown:
                        if (DisLikeReaction.IsChecked != null && (bool)DisLikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsDown, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            DisLikeReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Happy:
                        if (HappyReaction.IsChecked != null && (bool)HappyReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Happy, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            HappyReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.HeartBreak:
                        if (HeartBreakReaction.IsChecked != null && (bool)HeartBreakReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.HeartBreak, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            HeartBreakReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Mad:
                        if (MadReaction.IsChecked != null && (bool)MadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Mad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            MadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Sad:
                        if (SadReaction.IsChecked != null && (bool)SadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Sad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            SadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsUp:
                        if (LikeReaction.IsChecked != null && (bool)LikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsUp, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            LikeReaction.IsChecked = false;
                            return;
                        }
                        break;
                }
            }

            if (HeartReaction.IsChecked != null && (bool)HeartReaction.IsChecked)
            {
                PostControlViewModel.ReactedToPost(ReactionType.None,
                    new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart },
                    PostId);
            }

            else
            {
                PostControlViewModel.ReactedToPost(ReactionType.None, null, PostId);
            }
        }

        private void LikeReaction_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                switch (reactionType)
                {
                    case ReactionType.Heart:
                        if (HeartReaction.IsChecked != null && (bool)HeartReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Heart, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsUp }, PostId);
                            HeartReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Happy:
                        if (HappyReaction.IsChecked != null && (bool)HappyReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Happy, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsUp }, PostId);
                            HappyReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.HeartBreak:
                        if (HeartBreakReaction.IsChecked != null && (bool)HeartBreakReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.HeartBreak, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsUp }, PostId);
                            HeartBreakReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Mad:
                        if (MadReaction.IsChecked != null && (bool)MadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Mad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsUp }, PostId);
                            MadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Sad:
                        if (SadReaction.IsChecked != null && (bool)SadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Sad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsUp }, PostId);
                            SadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsDown:
                        if (DisLikeReaction.IsChecked != null && (bool)DisLikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsDown, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsUp }, PostId);
                            LikeReaction.IsChecked = false;
                            return;
                        }
                        break;
                }
            }
            if (LikeReaction.IsChecked != null && (bool)LikeReaction.IsChecked)
            {
                PostControlViewModel.ReactedToPost(ReactionType.None,
                    new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsUp },
                    PostId);
            }

            else
            {
                PostControlViewModel.ReactedToPost(ReactionType.None, null, PostId);
            }
        }

        private void HeartBreakReaction_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                switch (reactionType)
                {
                    case ReactionType.Heart:
                        if (HeartReaction.IsChecked != null && (bool)HeartReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Heart, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            HeartReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Happy:
                        if (HappyReaction.IsChecked != null && (bool)HappyReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Happy, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            HappyReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsDown:
                        if (DisLikeReaction.IsChecked != null && (bool)DisLikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsDown, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            HeartBreakReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Mad:
                        if (MadReaction.IsChecked != null && (bool)MadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Mad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            MadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Sad:
                        if (SadReaction.IsChecked != null && (bool)SadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Sad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            SadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsUp:
                        if (LikeReaction.IsChecked != null && (bool)LikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsUp, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
                            LikeReaction.IsChecked = false;
                            return;
                        }
                        break;
                }
            }
            if (HeartBreakReaction.IsChecked != null && (bool)HeartBreakReaction.IsChecked)
            {
                PostControlViewModel.ReactedToPost(ReactionType.None,
                    new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.HeartBreak },
                    PostId);
            }
            else
            {
                PostControlViewModel.ReactedToPost(ReactionType.None, null, PostId);
            }
        }

        private void HappyReaction_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                switch (reactionType)
                {
                    case ReactionType.Heart:
                        if (HeartReaction.IsChecked != null && (bool)HeartReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Heart, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Happy }, PostId);
                            HeartReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsDown:
                        if (DisLikeReaction.IsChecked != null && (bool)DisLikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsDown, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Happy }, PostId);
                            HappyReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.HeartBreak:
                        if (HeartBreakReaction.IsChecked != null && (bool)HeartBreakReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.HeartBreak, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Happy }, PostId);
                            HeartBreakReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Mad:
                        if (MadReaction.IsChecked != null && (bool)MadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Mad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Happy }, PostId);
                            MadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Sad:
                        if (SadReaction.IsChecked != null && (bool)SadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Sad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Happy }, PostId);
                            SadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsUp:
                        if (LikeReaction.IsChecked != null && (bool)LikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsUp, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Happy }, PostId);
                            LikeReaction.IsChecked = false;
                            return;
                        }
                        break;
                }
            }

            if (HappyReaction.IsChecked != null && (bool)HappyReaction.IsChecked)
            {
                PostControlViewModel.ReactedToPost(ReactionType.None,
                    new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Happy },
                    PostId);
            }
            else
            {
                PostControlViewModel.ReactedToPost(ReactionType.None, null, PostId);
            }
        }

        private void SadReaction_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                switch (reactionType)
                {
                    case ReactionType.Heart:
                        if (HeartReaction.IsChecked != null && (bool)HeartReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Heart, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Sad }, PostId);
                            HeartReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Happy:
                        if (HappyReaction.IsChecked != null && (bool)HappyReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Happy, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Sad }, PostId);
                            HappyReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.HeartBreak:
                        if (HeartBreakReaction.IsChecked != null && (bool)HeartBreakReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.HeartBreak, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Sad }, PostId);
                            HeartBreakReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Mad:
                        if (MadReaction.IsChecked != null && (bool)MadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Mad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Sad }, PostId);
                            MadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsDown:
                        if (DisLikeReaction.IsChecked != null && (bool)DisLikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsDown, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Sad }, PostId);
                            SadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsUp:
                        if (LikeReaction.IsChecked != null && (bool)LikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsUp, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Sad }, PostId);
                            LikeReaction.IsChecked = false;
                            return;
                        }
                        break;
                }
            }
            if (SadReaction.IsChecked != null && (bool)SadReaction.IsChecked)
            {
                PostControlViewModel.ReactedToPost(ReactionType.None,
                    new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Sad },
                    PostId);
            }
            else
            {
                PostControlViewModel.ReactedToPost(ReactionType.None, null, PostId);
            }
        }

        private void MadReaction_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                switch (reactionType)
                {
                    case ReactionType.Heart:
                        if (HeartReaction.IsChecked != null && (bool)HeartReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Heart, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Mad }, PostId);
                            HeartReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Happy:
                        if (HappyReaction.IsChecked != null && (bool)HappyReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Happy, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Mad }, PostId);
                            HappyReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.HeartBreak:
                        if (HeartBreakReaction.IsChecked != null && (bool)HeartBreakReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.HeartBreak, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Mad }, PostId);
                            HeartBreakReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsDown:
                        if (DisLikeReaction.IsChecked != null && (bool)DisLikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsDown, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Mad }, PostId);
                            MadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.Sad:
                        if (SadReaction.IsChecked != null && (bool)SadReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.Sad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Mad }, PostId);
                            SadReaction.IsChecked = false;
                            return;
                        }
                        break;
                    case ReactionType.ThumbsUp:
                        if (LikeReaction.IsChecked != null && (bool)LikeReaction.IsChecked)
                        {
                            PostControlViewModel.ReactedToPost(ReactionType.ThumbsUp, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Mad }, PostId);
                            LikeReaction.IsChecked = false;
                            return;
                        }
                        break;
                }
            }
            if (MadReaction.IsChecked != null && (bool)MadReaction.IsChecked)
            {
                PostControlViewModel.ReactedToPost(ReactionType.None,
                    new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Mad },
                    PostId);
            }
            else
            {
                PostControlViewModel.ReactedToPost(ReactionType.None, null, PostId);
            }
        }


        //private void HappyReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    SolidColorBrush desiredBrush = new SolidColorBrush(Colors.LightSlateGray);
        //    if (HeartReaction.Background != null && HeartReaction.Background.Equals(desiredBrush))
        //    {
        //        HeartReaction.Background = new SolidColorBrush(Colors.DarkSlateGray);
        //        PostControlViewModel.ReactedToPost(new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Happy }, PostId);
        //    }
        //    else
        //    {
        //        PostControlViewModel.ReactedToPost(null, PostId);
        //        HeartReaction.Background = new SolidColorBrush(Colors.LightSlateGray);
        //    }
        //}

        //private void DisLikeReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    SolidColorBrush desiredBrush = new SolidColorBrush(Colors.LightSlateGray);
        //    if (HeartReaction.Background != null && HeartReaction.Background.Equals(desiredBrush))
        //    {
        //        HeartReaction.Background = new SolidColorBrush(Colors.DarkSlateGray);
        //        PostControlViewModel.ReactedToPost(new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
        //    }
        //    else
        //    {
        //        PostControlViewModel.ReactedToPost(null, PostId);
        //        HeartReaction.Background = new SolidColorBrush(Colors.LightSlateGray);
        //    }
        //}

        //private void LikeReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    SolidColorBrush desiredBrush = new SolidColorBrush(Colors.LightSlateGray);
        //    if (HeartReaction.Background != null && HeartReaction.Background.Equals(desiredBrush))
        //    {
        //        HeartReaction.Background = new SolidColorBrush(Colors.DarkSlateGray);
        //        PostControlViewModel.ReactedToPost(new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsUp }, PostId);
        //    }
        //    else
        //    {
        //        PostControlViewModel.ReactedToPost(null, PostId);
        //        HeartReaction.Background = new SolidColorBrush(Colors.LightSlateGray);
        //    }
        //}

        //private void HeartBreakReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    SolidColorBrush desiredBrush = new SolidColorBrush(Colors.LightSlateGray);
        //    if (HeartReaction.Background != null && HeartReaction.Background.Equals(desiredBrush))
        //    {
        //        HeartReaction.Background = new SolidColorBrush(Colors.DarkSlateGray);
        //        PostControlViewModel.ReactedToPost(new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.HeartBreak }, PostId);
        //    }
        //    else
        //    {
        //        PostControlViewModel.ReactedToPost(null, PostId);
        //        HeartReaction.Background = new SolidColorBrush(Colors.LightSlateGray);
        //    }
        //}

        //private void HeartReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    SolidColorBrush desiredBrush = new SolidColorBrush(Colors.LightSlateGray);
        //    if (HeartReaction.Background != null && HeartReaction.Background.Equals(desiredBrush))
        //    {
        //        HeartReaction.Background = new SolidColorBrush(Colors.DarkSlateGray);
        //        PostControlViewModel.ReactedToPost(new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Heart }, PostId);
        //    }
        //    else
        //    {
        //        PostControlViewModel.ReactedToPost(null, PostId);
        //        HeartReaction.Background = new SolidColorBrush(Colors.LightSlateGray);
        //    }

        //}

        //private void SadReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    SolidColorBrush desiredBrush = new SolidColorBrush(Colors.LightSlateGray);
        //    if (HeartReaction.Background != null && HeartReaction.Background.Equals(desiredBrush))
        //    {
        //        HeartReaction.Background = new SolidColorBrush(Colors.DarkSlateGray);
        //        PostControlViewModel.ReactedToPost(new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Sad }, PostId);
        //    }
        //    else
        //    {
        //        PostControlViewModel.ReactedToPost(null, PostId);
        //        HeartReaction.Background = new SolidColorBrush(Colors.LightSlateGray);
        //    }
        //}

        //private void MadReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    SolidColorBrush desiredBrush = new SolidColorBrush(Colors.LightSlateGray);
        //    if (HeartReaction.Background != null && HeartReaction.Background.Equals(desiredBrush))
        //    {
        //        HeartReaction.Background = new SolidColorBrush(Colors.DarkSlateGray);
        //        PostControlViewModel.ReactedToPost(new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.Mad }, PostId);
        //    }
        //    else
        //    {
        //        PostControlViewModel.ReactedToPost(null, PostId);
        //        HeartReaction.Background = new SolidColorBrush(Colors.LightSlateGray);
        //    }
        //}
       

        private void CommentButton_OnClick(object sender, RoutedEventArgs e)
        {
            CommentButton.Visibility = Visibility.Collapsed;
            CommentComponent.Visibility = Visibility.Visible;
        }

        private void CloseCommentSection_OnClick(object sender, RoutedEventArgs e)
        {
            CommentButton.Visibility = Visibility.Visible;
            CommentComponent.Visibility = Visibility.Collapsed;
        }
    }
}
