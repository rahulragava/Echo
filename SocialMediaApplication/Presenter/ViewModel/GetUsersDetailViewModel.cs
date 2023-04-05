using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.EntityModels;
using Windows.UI.Xaml.Media.Imaging;
using SocialMediaApplication.Util;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class GetUsersDetailViewModel : ObservableObject
    {
        public List<string> UserIds;
        public List<User> Users;
        public ObservableCollection<UserVObj> UserList;

        public GetUsersDetailViewModel()
        {
            UserIds = new List<string>();
            Users = new List<User>();
            UserList = new ObservableCollection<UserVObj>();
        }

        private string _userName;

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private BitmapImage _profileIcon;

        public BitmapImage ProfileIcon
        {
            get => _profileIcon;
            set => SetProperty(ref _profileIcon, value);
        }

        public void GetUsers()
        {
            var getUserRequest = new GetUserRequestObj(UserIds);
            var getUserUseCase = new GetUserUseCase(getUserRequest, new GetUserDetailViewModelPresenterCallBack(this));
            getUserUseCase.Execute();
        }

        public async Task SetProfileIconAsync(string imagePath)
        {
            var imageConversion = new StringToImageUtil();
            var profileIcon = await imageConversion.GetImageFromStringAsync(imagePath);
            ProfileIcon = profileIcon;
        }

        

        public EventHandler GotUser;
        public async Task GetUserSuccessAsync(List<User> users)
        {
            Users = users;
            UserList.Clear();
            foreach (var user in Users)
            {
                var userVObj = new UserVObj();
                await SetProfileIconAsync(user.ProfileIcon);
                userVObj.Id = user.Id;
                userVObj.UserName = user.UserName;
                userVObj.ProfileImage = ProfileIcon;
                UserList.Add(userVObj);
            }

            GotUser?.Invoke(this, EventArgs.Empty);

        }

        public class GetUserDetailViewModelPresenterCallBack : IPresenterCallBack<GetUserResponseObj>
        {
            private readonly GetUsersDetailViewModel _getUsersDetailViewModel;

            public GetUserDetailViewModelPresenterCallBack(GetUsersDetailViewModel getUsersDetailViewModel)
            {
                _getUsersDetailViewModel = getUsersDetailViewModel;
            }

            public void OnSuccess(GetUserResponseObj getUserResponseObj)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _getUsersDetailViewModel?.GetUserSuccessAsync(getUserResponseObj.Users);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }
    }
    public class UserVObj : User
    {
        public BitmapImage ProfileImage { get; set; }
    }
}
