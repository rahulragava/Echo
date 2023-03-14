using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Util;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.PostView.TextPostView
{
    public sealed partial class TextPostUserControl : UserControl
    {
        public PostControlViewModel PostControlViewModel;

        public TextPostUserControl()
        {
            PostControlViewModel = new PostControlViewModel();
            this.InitializeComponent();
            Loaded += PostControl_Loaded;
            Unloaded += PostControl_Unloaded;

            //ReactionUserControl.GetReactionList += PostControlViewModel.
        }

        public event Action<List<Reaction>> ReactionPopUpButtonClicked;
        public event Action<string> PostRemoved;
        public event Action<ObservableCollection<string>> FollowerListChanged;
        public event Action<Reaction> ReactionChanged;
        public static event Action<string> NavigateToSearchPage;
        private void PostControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PostControlViewModel.SetObservableCollection(PostReaction, PostComments,PostId);
            PostControlViewModel.PostId = PostId;
            UserManager.UserNameChanged += UserNameChanged;
            PostControlViewModel.PostFontStyle = PostContentFont;
            PostControlViewModel.GetUserMiniDetailSuccess += GetUserMiniDetailSuccess;
            PostControlViewModel.FollowUnFollowActionDone += FollowUnFollowActionDone;
            PostControlViewModel.PostRemoved += RemovedPost;

            if (PostedByUserId != AppSettings.UserId)
            {
                RemovePost.Visibility = Visibility.Collapsed;
            }
            PostControlViewModel.RemoveButtonVisibility = AppSettings.UserId == PostedByUserId ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RemovedPost(string postId)
        {
            PostRemoved?.Invoke(postId);
        }

        private void FollowUnFollowActionDone()
        {
            FollowerListChanged?.Invoke(PostControlViewModel.UserFollowingList);
        }

        private void GetUserMiniDetailSuccess()
        {
            if (PostControlViewModel.UserFollowingList.Contains(AppSettings.UserId))
            {
                FollowButton.Visibility = Visibility.Collapsed;
                UnFollowButton.Visibility = Visibility.Visible;
            }
            else
            {
                FollowButton.Visibility = Visibility.Visible;
                UnFollowButton.Visibility = Visibility.Collapsed;
            }
        }

        private void UserNameChanged(string userName)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    PostedByUserName = userName;
                }
            );
        }

        private void PostControl_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UserManager.UserNameChanged -= UserNameChanged;
            
        }

        //username
        public static readonly DependencyProperty PostedByUserNameProperty = DependencyProperty.Register(
            nameof(PostedByUserName), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostedByUserName
        {
            get => (string)GetValue(PostedByUserNameProperty);
            set => SetValue(PostedByUserNameProperty, value);
        }

        
        //content
        public static readonly DependencyProperty PostContentProperty = DependencyProperty.Register(
            nameof(PostContent), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostContent
        {
            get => (string)GetValue(PostContentProperty);
            set => SetValue(PostContentProperty, value);
        }

        //post created at 
        public static readonly DependencyProperty PostCreatedAtProperty = DependencyProperty.Register(
            nameof(PostCreatedAt), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostCreatedAt
        {
            get => (string)GetValue(PostCreatedAtProperty);
            set => SetValue(PostCreatedAtProperty, value);
        }

        public static readonly DependencyProperty PostedByUserIdProperty = DependencyProperty.Register(
            nameof(PostedByUserId), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostedByUserId
        {
            get => (string)GetValue(PostedByUserIdProperty);
            set => SetValue(PostedByUserIdProperty, value);
        }

        public static readonly DependencyProperty PostContentFontProperty = DependencyProperty.Register(
            nameof(PostContentFont), typeof(PostFontStyle), typeof(TextPostUserControl), new PropertyMetadata(default(PostFontStyle)));

        public PostFontStyle PostContentFont
        {
            get => (PostFontStyle)GetValue(PostContentFontProperty);
            set => SetValue(PostContentFontProperty, value);
        }
        //title
        public static readonly DependencyProperty PostTitleProperty = DependencyProperty.Register(
            nameof(PostTitle), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostTitle
        {
            get => (string)GetValue(PostTitleProperty);
            set => SetValue(PostTitleProperty, value);
        }

        public static readonly DependencyProperty CreatedAtProperty = DependencyProperty.Register(
            nameof(CreatedAt), typeof(DateTime), typeof(TextPostUserControl), new PropertyMetadata(default(DateTime)));

        public DateTime CreatedAt
        {
            get => (DateTime)GetValue(CreatedAtProperty);
            set => SetValue(CreatedAtProperty, value);
        }
        //CommentCacheList
        public static readonly DependencyProperty PostCommentsProperty = DependencyProperty.Register(
            nameof(PostComments), typeof(List<CommentBObj>), typeof(TextPostUserControl),
            new PropertyMetadata(default(List<CommentBObj>)));

        public List<CommentBObj> PostComments
        {
            get => (List<CommentBObj>)GetValue(PostCommentsProperty);
            set => SetValue(PostCommentsProperty, value);
        }

        //reaction
        public static readonly DependencyProperty PostReactionProperty = DependencyProperty.Register(
            nameof(PostReaction), typeof(List<Reaction>), typeof(TextPostUserControl),
            new PropertyMetadata(default(List<Reaction>)));

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

        private void OpenCommentSection_OnClick(object sender, RoutedEventArgs e)
        {
            //OpenCommentSection.Visibility = Visibility.Collapsed;
            //OpenReactionSection.Visibility = Visibility.Collapsed;
            if (CommentComponent.Visibility == Visibility.Collapsed)
            {
                CommentComponent.Visibility = Visibility.Visible;
            }
            else
            {
                CommentComponent.Visibility = Visibility.Collapsed;
            }
        }


        private void OpenReactionSection_OnClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //private void UserReactions_OnClick(object sender, RoutedEventArgs e)
        //{
        //    UserReactionPopup.Visibility = Visibility.Visible;
        //    UserReactions.Visibility = Visibility.Collapsed;
        //}

        private void OpenReactionSection_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (OpenReactionSection.ContextFlyout != null)
            {
                OpenReactionSection.ContextFlyout.ShowAt(OpenReactionSection, new FlyoutShowOptions());

            }
        }

        private void OpenReactionSection_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            //if (OpenReactionSection.ContextFlyout != null)
            //{
            //    OpenReactionSection.ContextFlyout.Hide();
            //    //var a = OpenReactionSection.ContextFlyout.IsOpen;
            //    //if (a)
            //    //{

            //    //}
            //}
        }

        private void SetReaction(Reaction reaction)
        {
            PostControlViewModel.Reaction = reaction;
            var reactionIndex = PostControlViewModel.Reactions.IndexOf(PostControlViewModel.Reactions.FirstOrDefault(r => r.ReactedBy == AppSettings.UserId));

            if (reactionIndex != -1)
            {
                PostControlViewModel.Reactions[reactionIndex] = reaction;
            }
            switch (reaction.ReactionType)
            {
                case ReactionType.Heart:
                    PostControlViewModel.ReactionIcon = "♥";
                    break;
                case ReactionType.ThumbsDown:
                    PostControlViewModel.ReactionIcon = "👎";
                    break;
                case ReactionType.ThumbsUp:
                    PostControlViewModel.ReactionIcon = "👍";

                    break;
                case ReactionType.Happy:
                    PostControlViewModel.ReactionIcon = "😁";
                    break;
                case ReactionType.Mad:
                    PostControlViewModel.ReactionIcon = "😡";
                    break;
                case ReactionType.HeartBreak:
                    PostControlViewModel.ReactionIcon = "💔";
                    break;
                case ReactionType.Sad:
                    PostControlViewModel.ReactionIcon = "😕";
                    break;
                default:
                    PostControlViewModel.ReactionIcon = "👍";
                    break;
            }

            ReactionChanged?.Invoke(reaction);
        }

        //private void ChangeCommentList(List<CommentBObj> CommentCacheList)
        //{
        //    PostControlViewModel.Comments.Clear();
        //    foreach (var comment in CommentCacheList)
        //    {
        //        PostControlViewModel.Comments.Add(comment);
        //    }

        //}

        //private void CommentInserted()
        //{
        //    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //        () =>
        //        {
        //            PostControlViewModel.GetComments();

        //        }
        //    );
        //}

        //private void CommentRemoved()
        //{
        //    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //        () =>
        //        {
        //            PostControlViewModel.GetComments();
        //        }
        //    );
        //}

        private void UserReactions_OnClick(object sender, RoutedEventArgs e)
        {
            ReactionPopUpButtonClicked?.Invoke(PostControlViewModel.Reactions.ToList());
        }

        //private void UserReactions_OnClick(object sender, RoutedEventArgs e)
        //{
        //    ReactionsPopup.Visibility = Visibility.Visible;
        //    ReactionsPopup.IsOpen = true;
        //}

        //private void ReactionsPopup_OnLoaded(object sender, RoutedEventArgs e)
        //{

        //    ReactionsPopup.HorizontalOffset = (Window.Current.Bounds.Width - UserSelectedReactionControl.ActualWidth) / 2;
        //    ReactionsPopup.VerticalOffset = (Window.Current.Bounds.Height - UserSelectedReactionControl.ActualHeight) / 2;

        //}
        private void CommentsViewUserControl_OnCommentCountChanged(int commentCount)
        {
            PostControlViewModel.TotalComments = commentCount;
        }

        private void ProfilePanel_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (AppSettings.UserId == PostedByUserId)
            {
                return;
            }
            ProfileMiniPopUp.Visibility = Visibility.Visible;
            ProfileMiniPopUp.IsOpen = true;
            PostControlViewModel.GetMiniProfileDetails(PostedByUserId);
        }

        private void FollowButton_OnClick(object sender, RoutedEventArgs e)
        {
            PostControlViewModel.FollowUnFollowSearchedUser(PostedByUserId);
            FollowButton.Visibility = Visibility.Collapsed;
            UnFollowButton.Visibility = Visibility.Visible;
        }

        private void UnFollowButton_OnClick(object sender, RoutedEventArgs e)
        {
            PostControlViewModel.FollowUnFollowSearchedUser(PostedByUserId);
            UnFollowButton.Visibility = Visibility.Collapsed;
            FollowButton.Visibility = Visibility.Visible;
        }

        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProfileMiniPopUp.IsOpen = false;
        }

        private void UserTextBlock_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigateToSearchPage?.Invoke(PostedByUserId);
        }

        private void RemovePost_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var postToBeRemoved = new TextPostBObj()
            {
                Id = PostId,
                Comments = PostControlViewModel.CommentsList,
                Reactions = PostControlViewModel.Reactions.ToList(),
            };
            PostControlViewModel.RemovePost(postToBeRemoved);
        }
    }
}
