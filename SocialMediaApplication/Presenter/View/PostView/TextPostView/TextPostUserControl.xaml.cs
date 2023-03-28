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
using static System.Collections.Specialized.BitVector32;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.PostView.TextPostView
{
    public sealed partial class TextPostUserControl : UserControl, ITextPostUserControl
    {
        public PostControlViewModel PostControlViewModel;

        public TextPostUserControl()
        {
            PostControlViewModel = new PostControlViewModel();
            this.InitializeComponent();
            Loaded += PostControl_Loaded;
            Unloaded += PostControl_Unloaded;
        }
        public event Action<List<Reaction>> ReactionPopUpButtonClicked;
        public event Action<string> PostRemoved;
        public event Action<ObservableCollection<string>> FollowerListChanged;
        public event Action<Reaction> ReactionChanged;
        public static event Action<string> NavigateToSearchPage;
        public event Action<List<Reaction>> CommentReactionPopUpButtonClicked;


        private void PostControl_Loaded(object sender, RoutedEventArgs e)
        {
            PostControlViewModel.TextPostUserControl = this;
            PostControlViewModel.SetObservableCollection(PostReaction, PostComments,PostId);
            PostControlViewModel.PostId = PostId;
            UserManager.UserNameChanged += UserNameChanged;
            
            if (PostControlViewModel.Reaction == null)
            {
                if (PostControlViewModel.Reactions.Count > 0)
                {
                    ReactionCountBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    PostControlViewModel.ReactionIcon = "👍";
                    UserReaction.Visibility = Visibility.Visible;
                }
                YouTextBlock.Visibility = Visibility.Collapsed;
                AndTextBlock.Visibility = Visibility.Collapsed;
                OthersTextBlock.Visibility = Visibility.Collapsed;
                PostControlViewModel.TotalReaction = PostControlViewModel.Reactions.Count;
            }
            else
            {
                if (PostControlViewModel.Reactions.Contains(PostControlViewModel.Reaction))
                {
                    PostControlViewModel.TotalReaction = PostControlViewModel.Reactions.Count - 1;
                }
                if (PostControlViewModel.TotalReaction > 0)
                {
                    YouTextBlock.Visibility = Visibility.Visible;
                    if (PostControlViewModel.TotalReaction > 1)
                    {
                        AndTextBlock.Visibility = Visibility.Visible;
                        OthersTextBlock.Visibility = Visibility.Visible;
                        ReactionCountBlock.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        AndTextBlock.Visibility = Visibility.Collapsed;
                        OthersTextBlock.Visibility = Visibility.Collapsed;
                        ReactionCountBlock.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    YouTextBlock.Visibility = Visibility.Visible;
                }
            }
            PostControlViewModel.PostedById = PostedByUserId;
            PostControlViewModel.PostFontStyle = PostContentFont;
            PostControlViewModel.GetUser();
            if (PostedByUserId != AppSettings.UserId)
            {
                RemovePost.Visibility = Visibility.Collapsed;
            }
            PostControlViewModel.RemoveButtonVisibility = AppSettings.UserId == PostedByUserId ? Visibility.Visible : Visibility.Collapsed;
        }

        public void RemovedPost(string postId)
        {
            PostRemoved?.Invoke(postId);
        }

        public void SetReactionVisibility()
        {
            if (string.IsNullOrEmpty(PostControlViewModel.MaxReactionIcon))
            {
                MaxReaction.Visibility = Visibility.Collapsed;
            }
            else if (string.IsNullOrEmpty(PostControlViewModel.SecondMaxReactionIcon))
            {
                SecondMaxReaction.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserReaction.Visibility = Visibility.Collapsed;
            }
        }

        public void FollowUnFollowActionDone()
        {
            FollowerListChanged?.Invoke(PostControlViewModel.UserFollowingList);
        }

        public void GetUserMiniDetailSuccess()
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

        private void PostControl_Unloaded(object sender, RoutedEventArgs e)
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
            CommentComponent.Visibility = CommentComponent.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
        
        private void OpenReactionSection_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ReactionPopUp.IsOpen = true;
        }

        public bool IsReactionSectionEntered = false;

        private void OpenReactionSection_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (IsReactionSectionEntered)
            {
                ReactionPopUp.IsOpen = false;
                IsReactionSectionEntered = false;
            }
        }


        private void SetReaction(Reaction reaction)
        {
            PostControlViewModel.Reaction = reaction;
            var reactionIndex = PostControlViewModel.Reactions.IndexOf(PostControlViewModel.Reactions.FirstOrDefault(r => r.ReactedBy == AppSettings.UserId));

            if (reactionIndex != -1)
            {
                PostControlViewModel.Reactions[reactionIndex] = reaction;
            }
            else
            {
                PostControlViewModel.Reactions.Add(reaction);
            }
            UserReaction.Visibility = Visibility.Visible;
            YouTextBlock.Visibility = Visibility.Visible;
            switch (reaction.ReactionType)
            {
                case ReactionType.Heart:
                    PostControlViewModel.ReactionIcon = "♥";
                    PostControlViewModel.ReactionText = "Heart";
                    break;
                case ReactionType.ThumbsDown:
                    PostControlViewModel.ReactionIcon = "👎";    
                    PostControlViewModel.ReactionText = "Dislike";

                    break;
                case ReactionType.ThumbsUp:
                    PostControlViewModel.ReactionIcon = "👍";
                    PostControlViewModel.ReactionText = "Like";
                    break;
                case ReactionType.Happy:
                    PostControlViewModel.ReactionIcon = "😁";
                    PostControlViewModel.ReactionText = "Happy";

                    break;
                case ReactionType.Mad:
                    PostControlViewModel.ReactionIcon = "😠";
                    PostControlViewModel.ReactionText = "Mad";
                    break;
                case ReactionType.HeartBreak:
                    PostControlViewModel.ReactionIcon = "💔";
                    PostControlViewModel.ReactionText = "HeartBreak";
                    break;
                case ReactionType.Sad:
                    PostControlViewModel.ReactionIcon = "😕";
                    PostControlViewModel.ReactionText = "Sad";
                    break;
                default:
                    PostControlViewModel.ReactionIcon = "👍";
                    PostControlViewModel.ReactionText = "React";
                    break;
            }

            if (!string.IsNullOrEmpty(PostControlViewModel.MaxReactionIcon))
            {
                MaxReaction.Visibility = PostControlViewModel.MaxReactionIcon == PostControlViewModel.ReactionIcon ? Visibility.Collapsed : Visibility.Visible;
            }

            if (!string.IsNullOrEmpty(PostControlViewModel.SecondMaxReactionIcon))
            {
                SecondMaxReaction.Visibility = PostControlViewModel.MaxReactionIcon == PostControlViewModel.ReactionIcon ? Visibility.Collapsed : Visibility.Visible;
            }

            if (PostControlViewModel.Reactions.Contains(reaction))
            {
                PostControlViewModel.TotalReaction = PostControlViewModel.Reactions.Count-1;
            }
            else
            {
                PostControlViewModel.TotalReaction = PostControlViewModel.Reactions.Count;
            }

            if (PostControlViewModel.Reaction == null)
            {
                if (PostControlViewModel.TotalReaction > 0)
                {
                    ReactionCountBlock.Visibility = Visibility.Visible;
                }
                YouTextBlock.Visibility = Visibility.Collapsed;
                AndTextBlock.Visibility = Visibility.Collapsed;
                OthersTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (PostControlViewModel.TotalReaction > 0)
                {
                    YouTextBlock.Visibility = Visibility.Visible;
                    if (PostControlViewModel.TotalReaction > 1)
                    {
                        AndTextBlock.Visibility = Visibility.Visible;
                        OthersTextBlock.Visibility = Visibility.Visible;
                        ReactionCountBlock.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        AndTextBlock.Visibility = Visibility.Collapsed;
                        OthersTextBlock.Visibility = Visibility.Collapsed;
                        ReactionCountBlock.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    YouTextBlock.Visibility = Visibility.Visible;
                }
            }
            
            ReactionChanged?.Invoke(reaction);
       
            ReactionPopUp.IsOpen = false;
            IsReactionSectionEntered = false;
        }

        private void UserReactions_OnClick(object sender, RoutedEventArgs e)
        {
            ReactionPopUpButtonClicked?.Invoke(PostControlViewModel.Reactions.ToList());
        }
        void CommentsViewUserControl_OnCommentCountChanged(int commentCount)
        {
            PostControlViewModel.TotalComments = commentCount > 0 ? commentCount.ToString() : string.Empty;
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

        private async void RemovePost_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Post Deletion",
                PrimaryButtonText = "Yes",
                //Background = ( AppSettings.Theme == ElementTheme.Dark) ? new SolidColorBrush(Color.FromArgb(127,20,20,20)) : new SolidColorBrush(Color.FromArgb(127, 255, 255, 255)), 
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Primary,
                Content = "The post about to be removed cannot be retrieved! \n Are you sure, you want to delete ?"
            };

            var result = await dialog.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.None:
                    return;
                case ContentDialogResult.Primary:
                    break;
            }

            var postToBeRemoved = new TextPostBObj()
            {
                Id = PostId,
                Comments = PostControlViewModel.CommentsList,
                Reactions = PostControlViewModel.Reactions.ToList(),
            };
            PostControlViewModel.RemovePost(postToBeRemoved);
        }

        private void CommentSection_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            CommentComponent.Visibility = CommentComponent.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }


        private void ReactionPopUp_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            IsReactionSectionEntered = true;
        }

        private void ReactionPopUp_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            IsReactionSectionEntered = false;
          
        }

        private void TextPostGrid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ReactionPopUp.IsOpen)
            {
                ReactionPopUp.IsOpen = false;
            }
        }


        private void CommentsViewUserControl_OnCommentReactionButtonClicked(List<Reaction> commentReactions)
        {
            CommentReactionPopUpButtonClicked?.Invoke(commentReactions);
        }
    }
}
