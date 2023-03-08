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
using SocialMediaApplication.Util;

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
        }


        private void PostControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PostControlViewModel.SetObservableCollection(PostReaction, PostComments,PostId);
            PostControlViewModel.SetPollChoiceCollection(PollChoiceList);
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

        private void CommentButton_OnClick(object sender, RoutedEventArgs e)
        {
            CommentButton.Visibility = Visibility.Collapsed;
            ReactionButton.Visibility = Visibility.Collapsed;
            CommentComponent.Visibility = Visibility.Visible;
        }

        private void CloseCommentSection_OnClick(object sender, RoutedEventArgs e)
        {
            CommentButton.Visibility = Visibility.Visible;
            ReactionButton.Visibility = Visibility.Visible;
            CommentComponent.Visibility = Visibility.Collapsed;
        }

        //private void PollChoiceList_OnItemClick(object sender, ItemClickEventArgs e)
        //{
        //    var choiceList = sender as ListView;
        //    if (choiceList != null)
        //    {
        //        var choiceListSelectedItem = choiceList.SelectedItem as PollChoiceBObj;
        //        if (choiceListSelectedItem != null)
        //            PostControlViewModel.InsertUserSelectedChoice(
        //                new UserPollChoiceSelection(choiceListSelectedItem.Id, AppSettings.UserId));
        //    }
        //}

        private void PollChoiceList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var choiceList = sender as ListView;
            if (choiceList != null)
            {
                var choiceListSelectedItem = choiceList.SelectedItem as PollChoiceBObj;
                if (choiceListSelectedItem != null)
                    PostControlViewModel.InsertUserSelectedChoice(PostId,
                        new UserPollChoiceSelection(choiceListSelectedItem.Id, AppSettings.UserId));
            }
        }
    }
}
