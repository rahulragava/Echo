using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Util;
using SocialMediaApplication.Models.Constant;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.CommentView
{
    public sealed partial class CommentUserControl : UserControl,ICommentUserControlView
    {
        public CommentViewModel CommentViewModel;
        public CommentUserControl()
        {
            CommentViewModel = new CommentViewModel();
            this.InitializeComponent();
            Loaded += CommentUserControl_Loaded;
            Unloaded -= OnUnloaded;
        }

        private void CommentUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UserManager.UserNameChanged += UserNameChanged; 
            //if (CommentDepthProperty != null)
            //{
            //    //CommentViewModel.SetStackPanelDepth(CommentDepth);
            //}

            CommentViewModel.CommentId = CommentId;
            CommentViewModel.Reactions = CommentReactions;
            CommentViewModel.CommentedBy = CommentedBy;
            CommentViewModel.CommentUserControlView = this;
            CommentViewModel.GetUser();
            CommentViewModel.GetCommentReaction();
            if (CommentViewModel.Reaction == null) 
            {
                ReactionOnComment.Visibility = Visibility.Collapsed;
                ReactionIcon.Visibility = Visibility.Collapsed;
            }
            if (CommentedBy != AppSettings.UserId)
            {
                RemoveComment.Visibility = Visibility.Collapsed;
            }
            CommentViewModel.RemoveButtonVisibility = AppSettings.UserId == CommentedBy ? Visibility.Visible : Visibility.Collapsed;
        }
        public static event Action<string> NavigateToSearchPage;

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            UserManager.UserNameChanged -= UserNameChanged;
        }
        private void UserNameChanged(string userName)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (CommentedBy == AppSettings.UserId)
                    {
                        CommentedByUserName = userName;
                    }
                }
            );
        }

        public event Action<CommentBObj, int> CommentInserted;
        public event Action<List<Reaction>> CommentReactionIconClicked;
        public event Action<List<string>> CommentsRemoved;

        //public Thickness Thickness

        public static readonly DependencyProperty CommentedByUserNameProperty = DependencyProperty.Register(
            nameof(CommentedByUserName), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string CommentedByUserName
        {
            get => (string)GetValue(CommentedByUserNameProperty);
            set => SetValue(CommentedByUserNameProperty, value);
        }

        public static readonly DependencyProperty PostCommentContentProperty = DependencyProperty.Register(
            nameof(PostCommentContent), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string PostCommentContent
        {
            get => (string)GetValue(PostCommentContentProperty);
            set => SetValue(PostCommentContentProperty, value);
        }

        public static readonly DependencyProperty CommentedAtProperty = DependencyProperty.Register(
            nameof(CommentedAt), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string CommentedAt
        {
            get => (string)GetValue(CommentedAtProperty);
            set => SetValue(CommentedAtProperty, value);
        }

        public static readonly DependencyProperty CommentedByProperty = DependencyProperty.Register(
            nameof(CommentedBy), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string CommentedBy
        {
            get => (string)GetValue(CommentedByProperty);
            set => SetValue(CommentedByProperty, value);
        }

        public static readonly DependencyProperty CommentDateTimeProperty = DependencyProperty.Register(
            nameof(CommentDateTime), typeof(DateTime), typeof(CommentUserControl), new PropertyMetadata(default(DateTime)));

        public DateTime CommentDateTime
        {
            get => (DateTime)GetValue(CommentDateTimeProperty);
            set => SetValue(CommentDateTimeProperty, value);
        }

        public static readonly DependencyProperty CommentDepthProperty = DependencyProperty.Register(
            nameof(CommentDepth), typeof(int), typeof(CommentUserControl), new PropertyMetadata(default(int)));

        public int CommentDepth
        {
            get => (int)GetValue(CommentDepthProperty);
            set => SetValue(CommentDepthProperty, value);
        }

        public static readonly DependencyProperty CommentReactionsProperty = DependencyProperty.Register(
            nameof(CommentReactions), typeof(List<Reaction>), typeof(CommentUserControl), new PropertyMetadata(default(List<Reaction>)));

        public List<Reaction> CommentReactions
        {
            get => (List<Reaction>)GetValue(CommentReactionsProperty);
            set => SetValue(CommentReactionsProperty, value);
        }

        public static readonly DependencyProperty ParentCommentIdProperty = DependencyProperty.Register(
            nameof(ParentCommentId), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string ParentCommentId
        {
            get => (string)GetValue(ParentCommentIdProperty);
            set => SetValue(ParentCommentIdProperty, value);
        }

        public static readonly DependencyProperty CommentIdProperty = DependencyProperty.Register(
            nameof(CommentId), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string CommentId
        {
            get => (string)GetValue(CommentIdProperty);
            set => SetValue(CommentIdProperty, value);
        }

        public static readonly DependencyProperty PostIdProperty = DependencyProperty.Register(
            nameof(PostId), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string PostId
        {
            get => (string)GetValue(PostIdProperty);
            set => SetValue(PostIdProperty, value);
        }

        private async void RemoveComment_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Comment Deletion",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Primary,
                Content = "The Comment about to be removed cannot be retrieved! \n Are you sure, you want to delete ?"
            };

            var result = await dialog.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.None:
                    return;
                case ContentDialogResult.Primary:
                    break;
            }
            var comment = new CommentBObj()
            {
                Id = CommentId,
                CommentedAt = CommentDateTime,
                CommentedBy = AppSettings.UserId,
                Content = PostCommentContent,
                ParentCommentId = ParentCommentId,
                PostId = PostId,
                CommentedUserName = CommentedByUserName,
                FormattedCommentDate = CommentedAt,
                Depth = CommentDepth,
                Reactions = CommentReactions
            };
            CommentViewModel.RemoveSelectedComment(comment);
        }

        private void ReactionButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Flyout != null)
                {
                    btn.Flyout.ShowAt(btn, new FlyoutShowOptions());
                }
            }
        }

        public void InsertComment(CommentBObj comment, int index)
        {
            CommentContent.Text = string.Empty;
            CommentInserted?.Invoke(comment, index);
        }

        public void RemoveComments(List<string> commentIds)
        {
            CommentsRemoved?.Invoke(commentIds);
        }

        private void ReplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            ReplyPanel.Visibility = Visibility.Visible;
        }

        private void PostCommentButton_OnClick(object sender, RoutedEventArgs e)
        {
            var content = CommentContent.Text;
            if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
            {
                return;
            }
            
            CommentViewModel.SendCommentButtonClicked(content, CommentId, PostId, CommentDepth);
            ReplyPanel.Visibility = Visibility.Collapsed;
        }

        private void SetReaction(Reaction reaction)
        {
            CommentViewModel.Reaction = reaction;
            switch (reaction.ReactionType)
            {
                case ReactionType.Heart:
                    CommentViewModel.ReactionIcon = "♥";
                    break;
                case ReactionType.ThumbsDown:
                    CommentViewModel.ReactionIcon = "👎";
                    break;
                case ReactionType.ThumbsUp:
                    CommentViewModel.ReactionIcon = "👍";

                    break;
                case ReactionType.Happy:
                    CommentViewModel.ReactionIcon = "😁";
                    break;
                case ReactionType.Mad:
                    CommentViewModel.ReactionIcon = "😡";
                    break;
                case ReactionType.HeartBreak:
                    CommentViewModel.ReactionIcon = "💔";
                    break;
                case ReactionType.Sad:
                    CommentViewModel.ReactionIcon = "😕";
                    break;
                default:
                    CommentViewModel.ReactionIcon = "👍";
                    break;
            }
            ReactionOnComment.Visibility = Visibility.Visible;
            ReactionIcon.Visibility = Visibility.Visible;
        }
        
        private void NavigateToSearchPageTextBlock_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (CommentedBy == AppSettings.UserId)
            {
                return;
            }
            NavigateToSearchPage?.Invoke(CommentedBy);
        }

        private void CommentContent_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CommentContent.Text) || string.IsNullOrEmpty(CommentContent.Text))
            {
                PostCommentButton.IsEnabled = false;
            }
            else
            {
                PostCommentButton.IsEnabled = true;
            }
        }

        private void ReactionIcon_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //CommentReactionIconClicked?.Invoke(CommentViewModel.Reactions);
            CommentReactionIconClicked?.Invoke(CommentReactions);
        }
    }

    public interface ICommentUserControlView
    {
        void InsertComment(CommentBObj comment, int index);
        void RemoveComments(List<string> commentIds);
    }
}
