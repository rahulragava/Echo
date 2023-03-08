using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;
using Windows.UI.Xaml.Controls.Primitives;
using SocialMediaApplication.Presenter.View.ReactionView;
using System.Xml.Linq;
using SocialMediaApplication.DataManager;
using Windows.UI.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.PostView
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

        private void PostControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PostControlViewModel.SetObservableCollection(PostReaction, PostComments,PostId);
            PostControlViewModel.PostId = PostId;
            AddCommentManager.CommentInserted += CommentInserted;
            RemoveCommentManager.CommentRemoved += CommentRemoved;
            //foreach (var reaction in PostReaction)
            //{
            //    if (reaction.ReactedBy == App.UserId)
            //    {
            //        switch (reaction.ReactionType)
            //        {
            //            case ReactionType.Heart:
            //                HeartReaction.IsChecked = true;
            //                break;
            //            case ReactionType.Happy:
            //                HappyReaction.IsChecked = true;
            //                break;
            //            case ReactionType.HeartBreak:
            //                HeartBreakReaction.IsChecked = true;
            //                break;
            //            case ReactionType.Mad:
            //                MadReaction.IsChecked = true;
            //                break;
            //            case ReactionType.Sad:
            //                SadReaction.IsChecked = true;
            //                break;
            //            case ReactionType.ThumbsDown:
            //                DisLikeReaction.IsChecked = true;
            //                break;
            //            case ReactionType.ThumbsUp:
            //                LikeReaction.IsChecked = true;
            //                break;
            //        }
            //    }
            //}

        }

        private void PostControl_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AddCommentManager.CommentInserted -= CommentInserted;
            RemoveCommentManager.CommentRemoved -= CommentRemoved;
        }

        //username
        public static readonly DependencyProperty PostedByUserProperty = DependencyProperty.Register(
            nameof(PostedByUser), typeof(string), typeof(TextPostUserControl), new PropertyMetadata(default(string)));

        public string PostedByUser
        {
            get => (string)GetValue(PostedByUserProperty);
            set => SetValue(PostedByUserProperty, value);
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

        private void UserReactions_OnClick(object sender, RoutedEventArgs e)
        {
            UserReactionPopup.Visibility = Visibility.Visible;
            UserReactions.Visibility = Visibility.Collapsed;
        }

        private void OpenReactionSection_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (OpenReactionSection.Flyout != null)
                OpenReactionSection.Flyout.ShowAt(OpenReactionSection, new FlyoutShowOptions());
        }

        private void OpenReactionSection_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            //if (OpenReactionSection.Flyout != null) OpenReactionSection.Flyout.Hide();
        }

        private void SetReaction(Reaction reaction)
        {
            PostControlViewModel.Reaction = reaction;
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
        }

        //private void ChangeCommentList(List<CommentBObj> CommentCacheList)
        //{
        //    PostControlViewModel.Comments.Clear();
        //    foreach (var comment in CommentCacheList)
        //    {
        //        PostControlViewModel.Comments.Add(comment);
        //    }

        //}

        private void CommentInserted()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    PostControlViewModel.GetComments();

                }
            );
        }

        private void CommentRemoved()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    PostControlViewModel.GetComments();
                }
            );
        }
    }
}
