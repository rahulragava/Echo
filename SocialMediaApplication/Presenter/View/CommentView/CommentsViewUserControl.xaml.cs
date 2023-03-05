using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SocialMediaApplication.Models.BusinessModels;
using Windows.UI.Xaml.Controls.Maps;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;
using Color = Windows.UI.Color;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.CommentView
{
    public sealed partial class CommentsViewUserControl : UserControl
    {
        public CommentViewModel CommentViewModel;

        public CommentsViewUserControl()
        {
            CommentViewModel = new CommentViewModel();
            this.InitializeComponent();
            Loaded += Comment_Loaded;
            CommentViewModel.CheckAnyComments += CommentExist;

        }

        private void Comment_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CommentViewModel.CommentsList = Comments;
            CommentViewModel.ClearAndUpdate();
            if (!Comments.Any())
            {
                CommentList.Visibility = Visibility.Collapsed;
                NoCommentsMessage.Visibility = Visibility.Visible;
                NoCommentFont.Visibility = Visibility.Visible;
            }

        }

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
                CommentContent.Header = new SolidColorBrush(Colors.Red);
                return;
            }
            else
            {
                CommentContent.Header = string.Empty;
            }
            CommentContent.Text = String.Empty;
            string parentCommentId;
            int depth;
            string commentOnPostId;
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

            commentOnPostId = PostId;

            CommentViewModel.SendCommentButtonClicked(content, parentCommentId, commentOnPostId, depth);
        }

        private void RemoveComment_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (CommentList.SelectedItem is CommentBObj commentToBeDeleted)
            {
                CommentViewModel.RemoveSelectedComment(commentToBeDeleted, Comments);
            }
        }

        private void CommentExist(object sender, EventArgs e)
        {
            if (Comments.Any())
            {
                CommentList.Visibility = Visibility.Visible;
                NoCommentsMessage.Visibility = Visibility.Collapsed;
                NoCommentFont.Visibility = Visibility.Collapsed;
            }
            else
            {
                CommentList.Visibility = Visibility.Collapsed;
                NoCommentsMessage.Visibility = Visibility.Visible;
                NoCommentFont.Visibility = Visibility.Visible;
            }


        }
    }
}
