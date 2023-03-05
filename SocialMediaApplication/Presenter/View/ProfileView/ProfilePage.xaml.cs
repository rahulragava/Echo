using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Util;

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
            _profilePageViewModel.GetUser(ProfileUserId == AppSettings.LocalSettings.Values["user"].ToString()
                ? null
                : ProfileUserId);
            if (ProfileUserId != null)
            {
                FollowButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Collapsed;
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
            if (!EditPopUp.IsOpen) { EditPopUp.IsOpen = true; }

        }

        //changes has to be done,
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (string.IsNullOrEmpty(UserNameTextBox.Text))
            {
                ExampleVsCodeInAppNotification.Show("User Name Field Cannot Be Empty!", 5000);
                return;
            }
            if (EditPopUp.IsOpen) { EditPopUp.IsOpen = false; }

            var gender = GenderComboBox.SelectedIndex;
            var maritalStatus = MaritalStatusComboBox.SelectedIndex;
            _profilePageViewModel.EditUserProfile(UserNameTextBox.Text,FirstNameTextBox.Text,LastNameTextBox.Text,gender,maritalStatus,EducationTextBox.Text,OccupationTextBox.Text,PlaceTextBox.Text);
        }

        private void ShowFollowerPopup(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!FollowerPopUp.IsOpen) { FollowerPopUp.IsOpen = true; }

        }

        //changes has to be done,
        private void CloseFollowerPopup(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (FollowerPopUp.IsOpen) { FollowerPopUp.IsOpen = false; }

            //_profilePageViewModel.EditUserProfile();
        }

        private void ShowFilterPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!FilterPopup.IsOpen) { FilterPopup.IsOpen = true; }

        }

        private void CloseFilterPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (FilterPopup.IsOpen) { FilterPopup.IsOpen = false; }
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
    }

}
