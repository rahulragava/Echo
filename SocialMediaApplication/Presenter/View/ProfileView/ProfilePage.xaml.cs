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

        // Handles the Click event on the Button on the page and opens the Popup. 
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!EditPopUp.IsOpen)
            {
                EditPopUp.IsOpen = true;
            }
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (string.IsNullOrEmpty(UserNameTextBox.Text) || (string.IsNullOrWhiteSpace(UserNameTextBox.Text)))
            {
                ExampleVsCodeInAppNotification.Show("User Name Field Cannot Be Empty!", 5000);
                return;
            }
            if (EditPopUp.IsOpen) { EditPopUp.IsOpen = false; }

            var gender = GenderComboBox.SelectedIndex;
            var maritalStatus = MaritalStatusComboBox.SelectedIndex;
            _profilePageViewModel.EditUserProfile(UserNameTextBox.Text, FirstNameTextBox.Text, LastNameTextBox.Text, gender, maritalStatus, EducationTextBox.Text, OccupationTextBox.Text, PlaceTextBox.Text);
        }

        public void UserNameAlreadyExist()
        {
            ExampleVsCodeInAppNotification.Show("Already User with this UserName Exist! Try Something new", 5000);
        }

        private void ShowFollowerPopup(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            FollowerPopUp.Visibility = Visibility.Visible;
            if (!FollowerPopUp.IsOpen)
            {
                FollowerPopUp.IsOpen = true;
            }
            NavigationViewItem itemContent = FollowerFollowing.MenuItems.ElementAt(0) as NavigationViewItem;
            FollowerFollowing.SelectedItem = itemContent;
        }

        private void ShowFilterPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!FilterPopup.IsOpen)
            {
                FilterPopup.IsOpen = true;
            }
        }

        private void CloseFilterPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (FilterPopup.IsOpen)
            {
                FilterPopup.IsOpen = false;
            }
        }

        private void PostTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems[0] is ComboBoxItem comboBoxItem)) return;
            var content = comboBoxItem.Content as string;

            if (content != null && content.Equals("Text Post"))
            {
                PostListView.ItemsSource = _profilePageViewModel.TextPosts;
            }
            else if (content != null && content.Equals("Poll Post"))
            {
                PostListView.ItemsSource = _profilePageViewModel.PollPosts;
            }
            else if (content != null && content.Equals("All"))
            {
                if (PostListView != null)
                {
                    PostListView.ItemsSource = _profilePageViewModel.PostList;
                }
            }
        }

        private void PostTypeComboBox_OnDropDownClosed(object sender, object e)
        {
            FilterPopup.IsOpen = false;
        }

        private void PostControl_OnReactionPopUpButtonClicked(List<Reaction> reactions)
        {
            PostReactionsPopup.Visibility = Visibility.Visible;
            PostReactionsPopup.IsOpen = true;
            _profilePageViewModel.SetPostReactions(reactions);
        }


        private void PollPostControl_OnCommentReactionPopUpButtonClicked(List<Reaction> commentReactions)
        {
            CommentReactionsPopup.Visibility = Visibility.Visible;
            CommentReactionsPopup.IsOpen = true;
            _profilePageViewModel.SetCommentReactions(commentReactions);
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

        private void ShowFollowingsPopup(object sender, TappedRoutedEventArgs e)
        {
            FollowerPopUp.Visibility = Visibility.Visible;
            if (!FollowerPopUp.IsOpen) { FollowerPopUp.IsOpen = true; }
            NavigationViewItem itemContent = FollowerFollowing.MenuItems.ElementAt(2) as NavigationViewItem;
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

        //var picker = new FileOpenPicker();
        //picker.ViewMode = PickerViewMode.Thumbnail;
        //picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //picker.FileTypeFilter.Add(".jpg");
        //picker.FileTypeFilter.Add(".jpeg");
        //picker.FileTypeFilter.Add(".png");

        //StorageFile file = await picker.PickSingleFileAsync();

        //    // If a file was selected, save it in the app folder
        //    if (file != null)
        //{
        //    StorageFolder appFolder = ApplicationData.Current.LocalFolder;
        //    StorageFile newFile = await file.CopyAsync(appFolder, file.Name, NameCollisionOption.ReplaceExisting);

        //    // Update the image source with the new file
        //    var temp = new BitmapImage(new Uri(newFile.Path));
        //    ProfilePath = temp.UriSource.LocalPath.ToString();

        //    Initial.ProfilePicture = temp;




        public async void SetProfileImage()
        {
            if (_profilePageViewModel.ProfileImage != null)
            {

                var imageConversion = new StringToImageUtil();
                var profileIcon = await imageConversion.GetImageFromStringAsync(_profilePageViewModel.ProfileImage);
                ProfileImage.Source = profileIcon;
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
                var imageConversion = new StringToImageUtil();
                var homeIcon = await imageConversion.GetImageFromStringAsync(_profilePageViewModel.HomePageImage);
                HomeImage.Source = homeIcon;
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
            ExampleVsCodeInAppNotification.Show("Post  is Successfully Created!", 2000);
            _profilePageViewModel.PostList.Insert(0, textPost);
            _profilePageViewModel.TextPosts.Insert(0, textPost);
        }

        private void PostControl_OnReactionChanged(Reaction reaction)
        {
            _profilePageViewModel.PostReaction = reaction;
            var reactions = _profilePageViewModel.PostReactions.ToList();
            _profilePageViewModel.ChangeInReactions(reactions);
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
