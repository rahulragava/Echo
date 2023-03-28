using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.PostView.TextPostView
{
    public sealed partial class MiniTextPostCreation : UserControl, IMiniTextPostCreationView
    {
        public FeedPageViewModel FeedPageViewModel;
        public MiniTextPostCreation()
        {
            FeedPageViewModel = new FeedPageViewModel();
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        public event Action<TextPostBObj> OnTextPostCreationSuccess;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FeedPageViewModel.MiniTextPostCreationView = this;
            if (FeedPageViewModel != null)
            {
                FeedPageViewModel.GetUser();
            }
        }

        public void PostedSuccess()
        {
            var textPost = FeedPageViewModel.TextPost;
            var textPostBObj = new TextPostBObj()
            {
                Id = textPost.Id,
                Content = textPost.Content,
                CreatedAt = textPost.CreatedAt,
                FontStyle = textPost.FontStyle,
                LastModifiedAt = textPost.LastModifiedAt,
                Title = textPost.Title,
                PostedBy = textPost.PostedBy,
                UserName = FeedPageViewModel.User.UserName,

            };
            OnTextPostCreationSuccess?.Invoke(textPostBObj);
        }

        private void PostClicked_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ContentBox.Text) || string.IsNullOrWhiteSpace(ContentBox.Text))
            {
                return;
            }
            var post = new TextPost()
            {
                Content = ContentBox.Text,
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now,
                FontStyle = FeedPageViewModel.FontStyle,
                PostedBy = AppSettings.LocalSettings.Values["user"].ToString(),
            };
            FeedPageViewModel.CreateTextPost(post);
            ContentBox.Text = string.Empty;
        }

        private void ContentBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ContentBox.Text.Length > 0)
            {
                PostClicked.IsEnabled = true;
            }
            else
            {
                PostClicked.IsEnabled = false;
            }
        }
    }

    public interface IMiniTextPostCreationView
    {
        void PostedSuccess();
    }
}
