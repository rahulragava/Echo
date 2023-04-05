using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

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
            Unloaded += OnUnloaded;
        }


        public event Action<TextPostBObj> OnTextPostCreationSuccess;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FeedPageViewModel.MiniTextPostCreationView = this;
            EditUserManager.UserNameChanged += EditUserManagerOnUserNameChanged;
            EditProfileImageManager.ProfileUpdated += EditProfileImageManagerOnProfileUpdated;
            if (FeedPageViewModel != null)
            {
                FeedPageViewModel.GetUser();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            EditUserManager.UserNameChanged -= EditUserManagerOnUserNameChanged;
            EditProfileImageManager.ProfileUpdated -= EditProfileImageManagerOnProfileUpdated;
        }

        private void EditUserManagerOnUserNameChanged(string userName)
        {
            FeedPageViewModel.User.UserName = userName;
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

        private void EditProfileImageManagerOnProfileUpdated(string image)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    FeedPageViewModel.ProfileImage = new BitmapImage(new Uri(image));
                }
            );
        }

        private void ContentBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            PostClicked.IsEnabled = ContentBox.Text.Length > 0;
        }

        public string GetUserDetailSuccess(string name)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            var userThoughtText = resourceLoader.GetString("UserThought");
            return (userThoughtText + " " + name + " ?");
        }

        private void PostClicked_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            }
        }

        private void PostClicked_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }
    }

    public interface IMiniTextPostCreationView
    {
        void PostedSuccess();
        string GetUserDetailSuccess(string name);
    }
}
