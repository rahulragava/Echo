using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Util;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View
{
    public sealed partial class UserListUserControl : UserControl
    {
        public GetUsersDetailViewModel GetUserDetailViewModel;
        public UserListUserControl()
        {
            GetUserDetailViewModel = new GetUsersDetailViewModel();
            this.InitializeComponent();
            Loaded += UserListControl_Loaded;
        }
        public static event Action<string> NavigateToSearchPage;

        private void UserListControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GetUserDetailViewModel.UserIds.Clear();
            if (UserPollChoiceSelectionList != null)
            {
                foreach (var userPollChoiceSelection in UserPollChoiceSelectionList)
                {
                    GetUserDetailViewModel.UserIds.Add(userPollChoiceSelection.SelectedBy);
                }
            }

            if (UserIdList != null)
            {
                GetUserDetailViewModel.UserIds = UserIdList;
            }

            if (GetUserDetailViewModel.UserIds.Count > 0)
            {
                GetUserDetailViewModel.GetUsers();
                UserList.Visibility = Visibility.Visible;
                NoUserFont.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserList.Visibility = Visibility.Collapsed;
                NoUserFont.Visibility = Visibility.Visible;
            }
        }

        public static readonly DependencyProperty UserIdListProperty = DependencyProperty.Register(
            nameof(UserIdList), typeof(List<string>), typeof(UserListUserControl), new PropertyMetadata(default(List<string>)));

        public List<string> UserIdList
        {
            get => (List<string>)GetValue(UserIdListProperty);
            set => SetValue(UserIdListProperty, value);
        }

        public static readonly DependencyProperty UserPollChoiceSelectionListProperty = DependencyProperty.Register(
            nameof(UserPollChoiceSelectionList), typeof(List<UserPollChoiceSelection>), typeof(UserListUserControl), new PropertyMetadata(default(List<UserPollChoiceSelection>)));

        public List<UserPollChoiceSelection> UserPollChoiceSelectionList
        {
            get => (List<UserPollChoiceSelection>)GetValue(UserPollChoiceSelectionListProperty);
            set => SetValue(UserPollChoiceSelectionListProperty, value);
        }

        private void UserList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is User selectedUser)
            {
                if (selectedUser.Id == AppSettings.UserId)
                {
                    return;
                }
                NavigateToSearchPage?.Invoke(selectedUser.Id);
            }
        }
    }
}
