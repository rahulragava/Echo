using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.CommentView
{
    public sealed partial class CommentsViewUserControl : UserControl,ICommentsViewUserControlView
    {
        public CommentViewModel CommentViewModel;

        public CommentsViewUserControl()
        {
            CommentViewModel = new CommentViewModel();
            this.InitializeComponent();
            Loaded += Comment_Loaded;
            
        }

        private void Comment_Loaded(object sender, RoutedEventArgs e)
        {
            if (Comments != null)
            {
                CommentViewModel.CommentsList = Comments;
                CommentViewModel.CommentViewUserControlView = this;
                CommentViewModel.ClearAndUpdate();
       
                if (!Comments.Any())
                {
                    CommentList.Visibility = Visibility.Collapsed;
                    NoCommentFont.Visibility = Visibility.Visible;
                }
            }
            CommentViewModel.PostId = PostId;
        }
        public event Action<int> CommentCountChanged;
        public event Action<Reaction> CommentReactionChanged;
        public event Action<List<Reaction>> CommentReactionButtonClicked;

        public static readonly DependencyProperty CommentsProperty = DependencyProperty.Register(
            nameof(Comments), typeof(List<CommentBObj>), typeof(CommentsViewUserControl),
            new PropertyMetadata(default(List<CommentBObj>)));

        public List<CommentBObj> Comments
        {
            get => (List<CommentBObj>)GetValue(CommentsProperty);
            set => SetValue(CommentsProperty, value);
        }

        public static readonly DependencyProperty PostIdProperty = DependencyProperty.Register(
            nameof(PostId), typeof(string), typeof(CommentsViewUserControl), new PropertyMetadata(default(string)));

        public string PostId
        {
            get => (string)GetValue(PostIdProperty);
            set => SetValue(PostIdProperty, value);
        }

        private void PostCommentButton_OnClick(object sender, RoutedEventArgs e)
        {
            var content = CommentContent.Text;
            if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
            {
                CommentContent.Header = "text should not be empty";
                CommentContent.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            else
            {
                CommentContent.Header = string.Empty;
            }
            CommentContent.Text = String.Empty;
            string parentCommentId;
            int depth;
            if (CommentList.SelectedItem is null)
            {
                parentCommentId = null;

            }

            var c = CommentList.SelectedItem as CommentBObj;
            parentCommentId = c?.Id;
            if (c?.Depth != null) depth = (int)c?.Depth;
            else
            {
                depth = 0;
            }

            var commentOnPostId = PostId;

            CommentViewModel.SendCommentButtonClicked(content, parentCommentId, commentOnPostId, depth);
        }

        public void InsertingCommentInList(CommentBObj comment, int index)
        {
            CommentViewModel.CommentsList.Insert(index, comment);
            CommentViewModel.PostComments.Insert(index, comment);
            CommentCountChanged?.Invoke(CommentViewModel.CommentsList.Count);

        }

        private void CommentUserControl_OnCommentReactionIconClicked(List<Reaction> reactions)
        {
            CommentReactionButtonClicked?.Invoke(reactions);
        }


        public void RemoveCommentsInList(List<string> commentIds)
        {
            foreach (var commentId in commentIds)
            {
                CommentViewModel.CommentsList.Remove(CommentViewModel.CommentsList.SingleOrDefault(c=>c.Id == commentId));
                CommentViewModel.PostComments.Remove(CommentViewModel.PostComments.SingleOrDefault(c=>c.Id == commentId));
            }

            CommentExist();
            CommentCountChanged?.Invoke(CommentViewModel.CommentsList.Count);
        }
        
        public void CommentExist()
        {
            if (CommentViewModel.CommentsList.Any())
            {
                CommentList.Visibility = Visibility.Visible;
                //NoCommentsMessage.Visibility = Visibility.Collapsed;
                NoCommentFont.Visibility = Visibility.Collapsed;
            }
            else
            {
                CommentList.Visibility = Visibility.Collapsed;
                //NoCommentsMessage.Visibility = Visibility.Visible;
                NoCommentFont.Visibility = Visibility.Visible;
            }
        }

        public void ParentCommentInserted()
        {
            CommentCountChanged?.Invoke(CommentViewModel.CommentsList.Count);
        }

        private void ReactionButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Flyout?.ShowAt(btn, new FlyoutShowOptions());
            }
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

        private void CommentUserControl_OnChangeInReaction(Reaction reaction)
        {
            CommentReactionChanged?.Invoke(reaction);
        }
    }

    public interface ICommentsViewUserControlView
    {
        void ParentCommentInserted();
        void CommentExist();
    }
}
