using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using Windows.UI.Xaml.Media.Animation;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Presenter.View.ReactionView;
using Windows.UI.Core;

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
            Unloaded += Comment_UnLoaded;
            
        }

        private void Comment_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Comments != null)
            {
                CommentViewModel.CommentsList = Comments;
                CommentViewModel.ClearAndUpdate();
                CommentViewModel.CheckAnyComments += CommentExist;
                CommentViewModel.ParentCommentInserted += ParentCommentInserted;
                if (!Comments.Any())
                {
                    CommentList.Visibility = Visibility.Collapsed;
                    NoCommentsMessage.Visibility = Visibility.Visible;
                    NoCommentFont.Visibility = Visibility.Visible;
                }
            }
            //AddCommentManager.CommentInserted += CommentInserted;
            
            //RemoveCommentManager.CommentRemoved += CommentRemoved;
            CommentViewModel.PostId = PostId;
        }

        private void Comment_UnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //AddCommentManager.CommentInserted -= CommentInserted;
            //RemoveCommentManager.CommentRemoved -= CommentRemoved;
            CommentViewModel.CheckAnyComments -= CommentExist;
        }

        public event Action<int> CommentCountChanged;

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

        public void RemoveCommentsInList(List<string> commentIds)
        {
            foreach (var commentId in commentIds)
            {
                CommentViewModel.CommentsList.Remove(CommentViewModel.CommentsList.SingleOrDefault(c=>c.Id == commentId));
                CommentViewModel.PostComments.Remove(CommentViewModel.PostComments.SingleOrDefault(c=>c.Id == commentId));
            }

            CommentExist(this,EventArgs.Empty);
            CommentCountChanged?.Invoke(CommentViewModel.CommentsList.Count);
        }
        
        private void CommentExist(object sender, EventArgs e)
        {
            if (CommentViewModel.CommentsList.Any())
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

        private void ParentCommentInserted(object sender, EventArgs e)
        {
            CommentCountChanged?.Invoke(CommentViewModel.CommentsList.Count);
        }
        //private void CommentInserted()
        //{
        //    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //        () =>
        //        {
        //            CommentViewModel.GetComments();
        //        }
        //    );

        //}

        //private void CommentRemoved()
        //{
        //    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //        () =>
        //        {
        //            CommentViewModel.GetComments();
        //        }
        //    );
        //}


        private void ReplyButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ReactionButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Flyout.ShowAt(btn, new FlyoutShowOptions());
        }

    }
}
