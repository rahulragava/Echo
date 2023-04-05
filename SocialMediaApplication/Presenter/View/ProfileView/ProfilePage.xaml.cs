using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Util;
using Windows.UI.Xaml.Input;
using User = SocialMediaApplication.Models.EntityModels.User;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using SocialMediaApplication.Models.BusinessModels;
using Windows.UI.Core;
using Windows.UI.Text;
using static System.Net.Mime.MediaTypeNames;
using Windows.UI.Xaml.Controls.Primitives;
using static System.Collections.Specialized.BitVector32;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View.ProfileView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page, IProfileView
    {

        private readonly ProfilePageViewModel _profilePageViewModel;

        public ProfilePage()
        {
            _profilePageViewModel = new ProfilePageViewModel
            {
                ProfileView = this
            };
            this.InitializeComponent();
        }

        public void GetUserSucceed()
        {
            if (_profilePageViewModel.User.Id == AppSettings.UserId)
            {
                FollowButton.Visibility = Visibility.Collapsed;
                EditButton.Visibility = Visibility.Visible;
                CreatePostButton.Visibility = Visibility.Visible;
                EditPhoto.Visibility = Visibility.Visible;
                UserThoughtPost.Visibility = Visibility.Visible;
                HomeIconEditButton.Visibility = Visibility.Visible;
                PostGrid.SetValue(Grid.RowProperty, 4);
                PostGrid.SetValue(Grid.RowSpanProperty, 2);
            }
            else
            {
                if (_profilePageViewModel.Followings.Contains(AppSettings.UserId))
                {
                    UnFollowButton.Visibility = Visibility.Visible;
                    FollowButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    FollowButton.Visibility = Visibility.Visible;
                    UnFollowButton.Visibility = Visibility.Collapsed;
                }
                EditButton.Visibility = Visibility.Collapsed;
                EditPhoto.Visibility = Visibility.Collapsed;
                HomeIconEditButton.Visibility = Visibility.Collapsed;
                CreatePostButton.Visibility = Visibility.Collapsed;
                UserThoughtPost.Visibility = Visibility.Collapsed;
                PostGrid.SetValue(Grid.RowProperty, 3);
                PostGrid.SetValue(Grid.RowSpanProperty, 3);
            }
            SetProfileImage();
            SetHomeImage();
        }
        public StorageFolder AppFolder = ApplicationData.Current.LocalFolder;

        public static event Action NavigateToPostCreationPage;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var userId = (string)e.Parameter;
            if (userId != null)
            {
                _profilePageViewModel.GetUser(userId);
            }
        }

        public static readonly DependencyProperty ProfileUserIdProperty = DependencyProperty.Register(
            nameof(ProfileUserId), typeof(string), typeof(ProfilePage), new PropertyMetadata(default(string)));

        public string ProfileUserId
        {
            get => (string)GetValue(ProfileUserIdProperty);
            set => SetValue(ProfileUserIdProperty, value);
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (string.IsNullOrEmpty(UserNameTextBox.Text) || (string.IsNullOrWhiteSpace(UserNameTextBox.Text)))
            {
                ExampleVsCodeInAppNotification.Show("User Name Field Cannot Be Empty!", 5000);
                return;
            }
          
            if (EditButton.Flyout != null)
            {
                EditButton.Flyout.Hide();
            }


            var gender = GenderComboBox.SelectedIndex;
            var maritalStatus = MaritalStatusComboBox.SelectedIndex;
            _profilePageViewModel.EditUserProfile(AppSettings.UserId,UserNameTextBox.Text, FirstNameTextBox.Text, LastNameTextBox.Text, gender, maritalStatus, EducationTextBox.Text, OccupationTextBox.Text, PlaceTextBox.Text);
        }

        public void UserNameAlreadyExist()
        {
            ExampleVsCodeInAppNotification.Show("Already User with this UserName Exist! Try Something new", 5000);
        }

        private void ShowFollowingPopupTapped(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            FollowerPopUp.Visibility = Visibility.Visible;
            FollowerPopInStoryboard.Begin();
            if (!FollowerPopUp.IsOpen)
            {
                FollowerPopUp.IsOpen = true;
            }
            NavigationViewItem itemContent = FollowerFollowing.MenuItems.ElementAt(0) as NavigationViewItem;
            FollowerFollowing.SelectedItem = itemContent;
        }

        private void ShowFilterPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                icon.ContextFlyout.ShowAt(icon,new FlyoutShowOptions());
            }
        }

        private void FollowerFollowing_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedTag = args.SelectedItemContainer.Tag.ToString();

            if (selectedTag == "FollowerTag")
            {
                FollowerFollowingFrame.Navigate(typeof(UserListPage), _profilePageViewModel.User.FollowersId);
            }
            else if (selectedTag == "FollowingTag")
            {
                FollowerFollowingFrame.Navigate(typeof(UserListPage), _profilePageViewModel.User.FollowingsId);
            }
        }

        private void ShowFollowerPopupTapped(object sender, TappedRoutedEventArgs e)
        {
            FollowerPopUp.Visibility = Visibility.Visible;
            FollowerPopInStoryboard.Begin();
            if (!FollowerPopUp.IsOpen) { FollowerPopUp.IsOpen = true; }
            NavigationViewItem itemContent = FollowerFollowing.MenuItems.ElementAt(1) as NavigationViewItem;
            FollowerFollowing.SelectedItem = itemContent;

        }

        private void FollowButton_OnClick(object sender, RoutedEventArgs e)
        {
            _profilePageViewModel.FollowUnFollowSearchedUser();
            FollowButton.Visibility = Visibility.Collapsed;
            UnFollowButton.Visibility = Visibility.Visible;
        }

        private void UnFollowButton_OnClick(object sender, RoutedEventArgs e)
        {
            _profilePageViewModel.FollowUnFollowSearchedUser();
            UnFollowButton.Visibility = Visibility.Collapsed;
            FollowButton.Visibility = Visibility.Visible;
        }

        private void PostControl_OnFollowerListChanged(ObservableCollection<string> followings)
        {
            _profilePageViewModel.Followings = followings;
            if (_profilePageViewModel.Followings.Contains(AppSettings.UserId))
            {
                _profilePageViewModel.UserFollowingCount += 1;
                UnFollowButton.Visibility = Visibility.Visible;
                FollowButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                _profilePageViewModel.UserFollowingCount -= 1;
                FollowButton.Visibility = Visibility.Visible;
                UnFollowButton.Visibility = Visibility.Collapsed;
            }


        }

        private void PollPostControl_OnPostRemoved(string postId)
        {
            _profilePageViewModel.PollPosts.Remove(_profilePageViewModel.PollPosts.SingleOrDefault(p => p.Id == postId));
            _profilePageViewModel.PostList.Remove(_profilePageViewModel.PostList.SingleOrDefault(p => p.Id == postId));
        }

        private void PostControl_OnPostRemoved(string postId)
        {
            _profilePageViewModel.TextPosts.Remove(_profilePageViewModel.TextPosts.SingleOrDefault(p => p.Id == postId));
            _profilePageViewModel.PostList.Remove(_profilePageViewModel.PostList.SingleOrDefault(p => p.Id == postId));
        }

        private void CreatePostButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigateToPostCreationPage?.Invoke();
        }

        private async void EditPhoto_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");


            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                StorageFile newFile = await file.CopyAsync(AppFolder, file.Name, NameCollisionOption.ReplaceExisting);
                _profilePageViewModel.ProfileImage = newFile.Path;
                _profilePageViewModel.EditProfileImage();
            }
        }

        public async void SetProfileImage()
        {
            if (_profilePageViewModel.ProfileImage != null)
            {
                //var imageConversion = new StringToImageUtil();
                //var profileIcon = await imageConversion.GetImageFromStringAsync(_profilePageViewModel.ProfileImage);
                ProfileImage.Source = new BitmapImage(new Uri(_profilePageViewModel.ProfileImage));
            }
        }

        private StorageFile HomePagePathFile { get; set; }

        private async void HomeIconEditButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {

                StorageFile newFile = await file.CopyAsync(AppFolder, file.Name, NameCollisionOption.ReplaceExisting);
                await CroppedImage.LoadImageFromFile(newFile);
                CroppedImage.CropShape = CropShape.Rectangular;
                CroppedImage.AspectRatio = 16d / 9d;
                HomePagePathFile = newFile;
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }
            ResizePopup.IsOpen = true;
        }

        public async void SetHomeImage()
        {
            if (!string.IsNullOrEmpty(_profilePageViewModel.HomePageImage))
            {
                HomeImage.Source = new BitmapImage(new Uri(_profilePageViewModel.HomePageImage));
            }
        }

        private void Unset_OnClick(object sender, RoutedEventArgs e)
        {
            ResizePopup.IsOpen = false;
        }

        private async void Set_OnClick(object sender, RoutedEventArgs e)
        {
            using (var fileStream = await HomePagePathFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                await CroppedImage.SaveAsync(fileStream, BitmapFileFormat.Png);
            }


            BitmapImage image = new BitmapImage();
            using (IRandomAccessStream stream = await HomePagePathFile.OpenAsync(FileAccessMode.Read))
            {
                await image.SetSourceAsync(stream);
            }
            _profilePageViewModel.HomePageImage = HomePagePathFile.Path;
            _profilePageViewModel.EditHomeImage();
            HomeImage.Source = image;
            ResizePopup.IsOpen = false;

        }

        private void MiniTextPostCreation_OnOnTextPostCreationSuccess(TextPostBObj textPost)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            ExampleVsCodeInAppNotification.Show(resourceLoader.GetString("TextPostCreatedNotification"), 2000);
            _profilePageViewModel.PostList.Insert(0, textPost);
            _profilePageViewModel.TextPosts.Insert(0, textPost);
        }

        private void TextBlock_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is TextBlock text)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            }
        }

        private void TextBlock_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is TextBlock text)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }

        private void FontIcon_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
            }
        }

        private void FontIcon_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is FontIcon icon)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }

        private void PostControl_OnReactionChanged(Reaction reaction)
        {
            _profilePageViewModel.PostReaction = reaction;
            var reactions = _profilePageViewModel.PostReactions.ToList();
            _profilePageViewModel.ChangeInReactions(reactions);
        }

        private void PostControl_OnReactionPopUpButtonClicked(List<Reaction> reactions)
        {
            //PostReactionsPopup.Visibility = Visibility.Visible;
            _profilePageViewModel.SetPostReactions(reactions);
            PopInStoryboard.Begin();
            PostReactionPopup.IsOpen = true;
        }


        private void PostControl_OnCommentReactionPopUpButtonClicked(List<Reaction> commentReactions)
        {
            //CommentReactionsPopup.Visibility = Visibility.Visible;
            _profilePageViewModel.SetCommentReactions(commentReactions);
            CommentPopInStoryboard.Begin();
            CommentReactionPopup.IsOpen = true;
        }

        private void PostSortComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems[0] is ComboBoxItem comboBoxItem)){ return;}
                var tag = comboBoxItem.Tag as string;

                if (tag != null && tag.Equals("OldToNew"))
                {
                    _profilePageViewModel.OrderByAscending(_profilePageViewModel.PostList);
                    _profilePageViewModel.OrderByAscending(_profilePageViewModel.TextPosts);
                    _profilePageViewModel.OrderByAscending(_profilePageViewModel.PollPosts);
                }
                else if (tag != null && tag.Equals("NewToOld"))
                {
                    _profilePageViewModel.OrderByDescending(_profilePageViewModel.PostList);
                    _profilePageViewModel.OrderByDescending(_profilePageViewModel.TextPosts);
                    _profilePageViewModel.OrderByDescending(_profilePageViewModel.PollPosts);
                }
        }

        private void SortPost_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //SortPopup.IsOpen = true;
            if (sender is FontIcon icon)
            {
                icon.ContextFlyout.ShowAt(icon, new FlyoutShowOptions());
            }
        }

        //private void PostSortComboBox_OnDropDownClosed(object sender, object e)
        //{
        //    SortPopup.IsOpen = false;
        //}

        private void PollPostControl_OnCommentReactionChanged(Reaction reaction)
        {
            _profilePageViewModel.CommentReaction = reaction;
            var reactions = _profilePageViewModel.CommentReactions.ToList();
            _profilePageViewModel.ChangeInCommentReactions(reactions);
        }

        private void NewestFirst_OnClick(object sender, RoutedEventArgs e)
        {
           
            if (sender is MenuFlyoutItem menuItem && menuItem.Tag is string tag && tag.Equals("NewToOld"))
            {
                _profilePageViewModel.OrderByDescending(_profilePageViewModel.PostList);
                _profilePageViewModel.OrderByDescending(_profilePageViewModel.TextPosts);
                _profilePageViewModel.OrderByDescending(_profilePageViewModel.PollPosts);
            }
        }

        private void OldestFirst_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.Tag is string tag && tag.Equals("OldToNew"))
            {
                _profilePageViewModel.OrderByAscending(_profilePageViewModel.PostList);
                _profilePageViewModel.OrderByAscending(_profilePageViewModel.TextPosts);
                _profilePageViewModel.OrderByAscending(_profilePageViewModel.PollPosts);
            }
        }


        private void PostTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems[0] is ComboBoxItem comboBoxItem)) return;
            var tag = comboBoxItem.Tag as string;

            if (tag != null && tag.Equals("TextPost"))
            {
                PostListView.ItemsSource = _profilePageViewModel.TextPosts;
            }
            else if (tag != null && tag.Equals("PollPost"))
            {
                PostListView.ItemsSource = _profilePageViewModel.PollPosts;
            }
            else if (tag != null && tag.Equals("All"))
            {
                if (PostListView != null)
                {
                    PostListView.ItemsSource = _profilePageViewModel.PostList;
                }
            }
        }

        private void AllPostFilter_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.Tag is string tag && tag.Equals("All"))
            {
                PostListView.ItemsSource = _profilePageViewModel.PostList;
            }
        }

        private void TextPostFilter_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.Tag is string tag && tag.Equals("TextPost"))
            {
                PostListView.ItemsSource = _profilePageViewModel.TextPosts;
            }

        }

        private void PollPostFilter_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.Tag is string tag && tag.Equals("PollPost"))
            {
                PostListView.ItemsSource = _profilePageViewModel.PollPosts;
            }

        }

        private void PostControl_OnEditTextPostClicked(string textPostId)
        {
            _profilePageViewModel.TextPost = _profilePageViewModel.TextPosts.SingleOrDefault(t => t.Id == textPostId);
            EditTextPopup.IsOpen = false;
            EditTextPopInStoryboard.Begin();
            EditTextPopup.IsOpen = true;
        }

        private void EditTextPostUserControl_OnCloseEdit(TextPostBObj textPostBObj)
        {
            EditTextPopOutStoryboard.Begin();
            EditTextPopup.IsOpen = false;
        }
    }

    public interface IProfileView
    {
        void SetProfileImage();
        void UserNameAlreadyExist();
        void GetUserSucceed();
        void SetHomeImage();
    }
}
