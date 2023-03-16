using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Util;
using Windows.System;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using User = SocialMediaApplication.Models.EntityModels.User;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View.ProfileView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        
        private readonly ProfilePageViewModel _profilePageViewModel;
        
        public ProfilePage()
        {
            _profilePageViewModel= new ProfilePageViewModel();
            this.InitializeComponent();
            Loaded += ProfilePage_Loaded;
        }

        private void ProfilePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _profilePageViewModel.GetUserSucceed += GetUserSucceed;
            _profilePageViewModel.UserNameAlreadyExist += UserNameAlreadyExist;
            _profilePageViewModel.ProfilePictureUpdated += SetProfileImage;
            _profilePageViewModel.HomePictureUpdated += SetHomeImage;
        }



        private void GetUserSucceed()
        {
            if (_profilePageViewModel.user.Id == AppSettings.UserId)
            {
                FollowButton.Visibility = Visibility.Collapsed;
                EditButton.Visibility = Visibility.Visible;
                CreatePostButton.Visibility = Visibility.Visible;
                EditPhoto.Visibility = Visibility.Visible;
                HomeIconEditButton.Visibility = Visibility.Visible;
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
            }
            SetProfileImage();
            SetHomeImage();
        }

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

        //changes has to be done,
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
            _profilePageViewModel.EditUserProfile(UserNameTextBox.Text,FirstNameTextBox.Text,LastNameTextBox.Text,gender,maritalStatus,EducationTextBox.Text,OccupationTextBox.Text,PlaceTextBox.Text);
        }

        private void UserNameAlreadyExist()
        {
            ExampleVsCodeInAppNotification.Show("Already User with this UserName Exist! Try Something new", 5000);
            return;
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
            if (content != null && content.Equals("TextPost"))
            {
                if ((PollPostListView is null) || (TextPostListView is null)) return;
                PollPostListView.Visibility = Visibility.Collapsed;
                TextPostListView.Visibility = Visibility.Visible;
            }
            else if (content != null && content.Equals("PollPost"))
            {
                if ((PollPostListView is null) || (TextPostListView is null)) return;
                TextPostListView.Visibility = Visibility.Collapsed;
                PollPostListView.Visibility = Visibility.Visible;
            }
        }

        private void PostTypeComboBox_OnDropDownClosed(object sender, object e)
        {
            FilterPopup.IsOpen = false;

        }

        private void PostControl_OnReactionPopUpButtonClicked(List<Reaction> reactions)
        {
            ReactionsPopup.Visibility = Visibility.Visible;
            ReactionsPopup.IsOpen = true;
            _profilePageViewModel.SetReactions(reactions);
            //_profilePageViewModel.Reactions = new ObservableCollection<Reaction>();
            //_profilePageViewModel.Reactions = new ObservableCollection<Reaction>(reactions);
        }

        private void FollowerFollowing_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedTag = args.SelectedItemContainer.Tag.ToString();

            if (selectedTag == "FollowerTag")
            {
                FollowerFollowingFrame.Navigate(typeof(UserListPage), _profilePageViewModel.user.FollowersId);
            }
            else if (selectedTag == "FollowingTag")
            {
                FollowerFollowingFrame.Navigate(typeof(UserListPage), _profilePageViewModel.user.FollowingsId);
            }
        }

        private void ShowFolloweePopup(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
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
            }
            else
            {
                _profilePageViewModel.UserFollowingCount -= 1;
            }


        }

        private void PollPostControl_OnPostRemoved(string postId)
        {
            _profilePageViewModel.PollPosts.Remove(_profilePageViewModel.PollPosts.SingleOrDefault(p => p.Id == postId));
        }

        private void PostControl_OnPostRemoved(string postId)
        {
            _profilePageViewModel.TextPosts.Remove(_profilePageViewModel.TextPosts.SingleOrDefault(p => p.Id == postId));
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

                _profilePageViewModel.ProfileImage = file.Path;
                _profilePageViewModel.EditProfileImage();
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }
        }

        private async void SetProfileImage()
        {
            if (_profilePageViewModel.ProfileImage != null)
            {
                BitmapImage image = new BitmapImage();
                try
                {
                    var storageFile = await StorageFile.GetFileFromPathAsync(_profilePageViewModel.ProfileImage);
                    using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        await image.SetSourceAsync(stream);
                    }
                    ProfileImage.Source = image;
                }
                catch (Exception e)
                {
                    //filenotfoundException
                }
            }
        }

        private void ProfileImage1_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //EditPhoto.Visibility = Visibility.Visible;
        }

        private void ProfileImage1_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            //EditPhoto.Visibility = Visibility.Collapsed;
        }

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

                _profilePageViewModel.HomePageImage = file.Path;
                _profilePageViewModel.EditHomeImage();
            }
            else
            {

            }
        }

        private async void SetHomeImage()
        {
            if (!string.IsNullOrEmpty(_profilePageViewModel.HomePageImage))
            {
                BitmapImage image = new BitmapImage();

                try
                {
                    var storageFile = await StorageFile.GetFileFromPathAsync(_profilePageViewModel.HomePageImage);
                    using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        await image.SetSourceAsync(stream);
                    }
                    HomeImage.Source = image;
                }
                catch (Exception e)
                {
                    //ProfileImage.Source = new BitmapImage(new Uri(@"../../../Assets/signInBackground.jpg"));
                }

            }
        }
    }

}
