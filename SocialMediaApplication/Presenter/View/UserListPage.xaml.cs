using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserListPage : Page
    {
        public GetUsersDetailViewModel GetUserDetailViewModel;

        public UserListPage()
        {
            GetUserDetailViewModel = new GetUsersDetailViewModel();
            this.InitializeComponent();
            Loaded += UserList_Loaded;
        }

        private void UserList_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GetUserDetailViewModel.GotUser += GotUser;
            //GetUserDetailViewModel.UserIds = userIds;
            //GetUserDetailViewModel.GetUsers();
        }

        private void GotUser(object sender, EventArgs e)
        {
            if (GetUserDetailViewModel.Users.Count > 0)
            {
                NoUserFont.Visibility = Visibility.Collapsed;
                //NoUserMessage.Visibility = Visibility.Collapsed;
                UserList.Visibility = Visibility.Visible;
            }
            else
            {
                NoUserFont.Visibility = Visibility.Visible;
                //NoUserMessage.Visibility = Visibility.Visible;
                UserList.Visibility = Visibility.Collapsed;
            }
            

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var userIds = (List<string>)e.Parameter;

            GetUserDetailViewModel.UserIds = userIds;
            GetUserDetailViewModel.GetUsers();
        }

        public static event Action<string> NavigateToUser;

        private void UserList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView list)
            {
                var user = list.SelectedItem as User;
                if (user != null && user.Id == AppSettings.UserId)
                {
                    return;
                }

                if (user != null)
                {
                    NavigateToUser?.Invoke(user.Id);
                }
            }
        }

        public void SetImageSource()
        {

        }
    }
    
}
