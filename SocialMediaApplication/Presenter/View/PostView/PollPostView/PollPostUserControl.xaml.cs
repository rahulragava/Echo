using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.View.PostView.TextPostView;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Util;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.PostView.PollPostView
{
    public sealed partial class PollPostUserControl : UserControl,IPollPostUserControl
    {
        public PostControlViewModel PostControlViewModel;

        public PollPostUserControl()
        {
            PostControlViewModel = new PostControlViewModel();
            this.InitializeComponent();
            Loaded += PostControl_Loaded;
            Unloaded += PollPostUserControl_Unloaded;
        }

        public static event Action<string> NavigateToSearchPage;
        public event Action<string> PostRemoved;
        public event Action<List<Reaction>> CommentReactionPopUpButtonClicked;
        public event Action<ObservableCollection<string>> FollowerListChanged;
        public event Action<List<Reaction>> ReactionPopUpButtonClicked;
        public event Action<Reaction> ReactionChanged;
        public event Action<Reaction> CommentReactionChanged;


        private void PostControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PostControlViewModel.PollPostUserControl = this;
            EditUserManager.UserNameChanged += UserNameChanged;
            PostControlViewModel.FormattedTime = RelativeDateTime.DateTimeConversion(CreatedAt);
            PostControlViewModel.SetObservableCollection(PostReaction, PostComments,PostId);
            PostControlViewModel.SetPollChoiceCollection(PollChoiceList);
            PostControlViewModel.PostedById= PostedByUserId;
            PollChoices.SelectedItem = PostControlViewModel.PollChoiceBObj;
            PostControlViewModel.PostFontStyle = PostContentFont;
            PostControlViewModel.GetUser();
            EditProfileImageManager.ProfileUpdated += EditProfileImageManagerOnProfileUpdated;
            if (PostedByUserId != AppSettings.UserId)
            {
                RemovePost.Visibility = Visibility.Collapsed;
            }

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
                    ReactionCountBlock.Visibility = Visibility.Collapsed;

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
                    if (PostControlViewModel.TotalReaction >= 1)
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
                    AndTextBlock.Visibility = Visibility.Collapsed;
                    OthersTextBlock.Visibility = Visibility.Collapsed;
                    ReactionCountBlock.Visibility = Visibility.Collapsed;
                }
            }

            PostControlViewModel.RemoveButtonVisibility = AppSettings.UserId == PostedByUserId ? Visibility.Visible : Visibility.Collapsed;
        }

        private void EditProfileImageManagerOnProfileUpdated(string profileIcon)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    PostControlViewModel.ProfileIcon = new BitmapImage(new Uri(profileIcon));
                }
            );
        }


        private void PollPostUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            EditUserManager.UserNameChanged -= UserNameChanged;
            EditProfileImageManager.ProfileUpdated -= EditProfileImageManagerOnProfileUpdated;
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

        public void RemovedPost(string postId)
        {
            PostRemoved?.Invoke(postId);
        }

        private void UserNameChanged(string userName)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    PostedByUser = userName;
                }
            );
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

        //post created at 
        public static readonly DependencyProperty PostCreatedAtProperty = DependencyProperty.Register(
            nameof(PostCreatedAt), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostCreatedAt
        {
            get => (string)GetValue(PostCreatedAtProperty);
            set => SetValue(PostCreatedAtProperty, value);
        }

        public static readonly DependencyProperty CreatedAtProperty = DependencyProperty.Register(
            nameof(CreatedAt), typeof(DateTime), typeof(TextPostUserControl), new PropertyMetadata(default(DateTime)));

        public DateTime CreatedAt
        {
            get => (DateTime)GetValue(CreatedAtProperty);
            set => SetValue(CreatedAtProperty, value);
        }

        public static readonly DependencyProperty PollChoicesProperty = DependencyProperty.Register(
            nameof(PollChoiceList), typeof(List<PollChoiceBObj>), typeof(PollPostUserControl), new PropertyMetadata(default(List<PollChoiceBObj>)));

        public List<PollChoiceBObj> PollChoiceList
        {
            get => (List<PollChoiceBObj>)GetValue(PollChoicesProperty);
            set => SetValue(PollChoicesProperty, value);
        }

        //CommentCacheList
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
        

        private void OpenCommentSection_OnClick(object sender, RoutedEventArgs e)
        {
            if (CommentComponent.Visibility == Visibility.Collapsed)
            {
                CommentComponent.Visibility = Visibility.Visible;
            }
            else
            {
                CommentComponent.Visibility = Visibility.Collapsed;
            }
        }

        public bool IsReactionSectionEntered = false;

        private void OpenReactionSection_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ReactionPopUp.IsOpen = true;
        }

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

            ReactionType maxReaction = ReactionType.None;
            try
            {
                //maxReaction = PostControlViewModel.Reactions.Max(r => r.ReactionType);
                maxReaction = PostControlViewModel.Reactions[0].ReactionType;
                for (int i = 1; i < PostControlViewModel.Reactions.Count; i++)
                {
                    var reactionCount = PostControlViewModel.Reactions.Count(r => r.ReactionType == PostControlViewModel.Reactions[i].ReactionType);
                    var maxReactionCount = PostControlViewModel.Reactions.Count(r => r.ReactionType == maxReaction);

                    if (reactionCount > maxReactionCount)
                    {
                        maxReaction = PostControlViewModel.Reactions[i].ReactionType;
                    }
                    //if (PostControlViewModel.Reactions[i])
                }
                PostControlViewModel.SetReactionIcon(maxReaction, false, true);
            }
            catch
            {
                PostControlViewModel.MaxReactionIcon = string.Empty;
                maxReaction = ReactionType.None;
            }
            try
            {
                if (maxReaction == ReactionType.None)
                {
                    PostControlViewModel.SecondMaxReactionIcon = string.Empty;
                }
                else
                {
                    ReactionType secondMax = ReactionType.None;
                    for (int i = 0; i < PostControlViewModel.Reactions.Count; i++)
                    {
                        if (PostControlViewModel.Reactions[i].ReactionType != maxReaction)
                        {
                            var reactionCount = PostControlViewModel.Reactions.Count(r => r.ReactionType == PostControlViewModel.Reactions[i].ReactionType);
                            var maxReactionCount = PostControlViewModel.Reactions.Count(r => r.ReactionType == secondMax);

                            if (reactionCount > maxReactionCount)
                            {
                                secondMax = PostControlViewModel.Reactions[i].ReactionType;
                            }
                        }
                    }
                    PostControlViewModel.SetReactionIcon(secondMax, false, false);
                }
            }
            catch
            {
                PostControlViewModel.SecondMaxReactionIcon = string.Empty;
                // there is no second max reaction
            }
            if (PostControlViewModel.ReactionIcon == PostControlViewModel.MaxReactionIcon)
            {
                PostControlViewModel.MaxReactionIcon = string.Empty;
            }
            else if (PostControlViewModel.ReactionIcon == PostControlViewModel.SecondMaxReactionIcon)
            {
                PostControlViewModel.SecondMaxReactionIcon = string.Empty;
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
                PostControlViewModel.TotalReaction = PostControlViewModel.Reactions.Count - 1;
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
                    if (PostControlViewModel.TotalReaction >= 1)
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
                    AndTextBlock.Visibility = Visibility.Collapsed;
                    OthersTextBlock.Visibility = Visibility.Collapsed;
                    ReactionCountBlock.Visibility = Visibility.Collapsed;
                }
            }
            ReactionChanged?.Invoke(reaction);

            //ReactionPopUp.IsOpen = false;
            IsReactionSectionEntered = false;
        }

        private void CommentsViewUserControl_OnCommentCountChanged(int commentCount)
        {
            PostControlViewModel.TotalComments = commentCount > 0 ? commentCount.ToString() : string.Empty;
        }

        private void CommentsViewUserControl_OnCommentReactionChanged(Reaction reaction)
        {
            CommentReactionChanged?.Invoke(reaction);
        }

        private void CommentsViewUserControl_OnCommentReactionButtonClicked(List<Reaction> commentReactions)
        {
            CommentReactionPopUpButtonClicked?.Invoke(commentReactions);
        }

        private void UserReactions_OnClick(object sender, RoutedEventArgs e)
        {
            ReactionPopUpButtonClicked?.Invoke(PostControlViewModel.Reactions.ToList());

            //PostReactionsPopup.IsOpen = true;
        }

        private void PollChoiceList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ListView choiceList))
            {
                return;
            }
            if (choiceList.SelectedItem is PollChoiceBObj choiceSelectedItem)
            {
                PostControlViewModel.PollChoiceBObj = choiceSelectedItem;
                PostControlViewModel.InsertUserSelectedChoice(PostId, new UserPollChoiceSelection(choiceSelectedItem.Id, AppSettings.UserId));
            }
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
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            ContentDialog dialog = new ContentDialog
            {
                Title = resourceLoader.GetString("PostDeletion"),
                PrimaryButtonText = resourceLoader.GetString("Yes"),
                //Background = ( AppSettings.Theme == ElementTheme.Dark) ? new SolidColorBrush(Color.FromArgb(127,20,20,20)) : new SolidColorBrush(Color.FromArgb(127, 255, 255, 255)), 
                CloseButtonText = resourceLoader.GetString("No"),
                DefaultButton = ContentDialogButton.Primary,
                Content = resourceLoader.GetString("PostRemovalWarning"),
            };

            var result = await dialog.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.None:
                    return;
                case ContentDialogResult.Primary:
                    var postToBeRemoved = new PollPostBObj()
                    {
                        Id = PostId,
                        Comments = PostControlViewModel.CommentsList,
                        Choices = PostControlViewModel.PollChoices.ToList(),
                        Reactions = PostControlViewModel.Reactions.ToList(),
                    };
                    PostControlViewModel.RemovePost(postToBeRemoved);
                    break;
            }
        }

        private void UserSelectedChoice_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is PersonPicture personPicture)
            {
                personPicture.ContextFlyout.ShowAt(personPicture, new FlyoutShowOptions());
            }
        }

        private void ReactionPopUp_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            IsReactionSectionEntered = true;

            //throw new NotImplementedException();
        }

        private void ReactionPopUp_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            IsReactionSectionEntered = false;
            //ReactionPopUp.IsOpen = false;
            //throw new NotImplementedException();
        }

        private void PollPostGrid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ReactionPopUp.IsOpen)
            {
                ReactionPopUp.IsOpen = false;
            }
        }

        private void RemovePost_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 15;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            }
        }

        private void RemovePost_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.FontSize = 13;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }

        private void CommentSection_OnClick(object sender, RoutedEventArgs e)
        {
            CommentComponent.Visibility = CommentComponent.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
    }   
}
