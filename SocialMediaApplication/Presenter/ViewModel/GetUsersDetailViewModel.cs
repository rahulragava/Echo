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

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class GetUsersDetailViewModel : ObservableObject
    {
        public List<string> UserIds;
        public List<User> Users;
        public ObservableCollection<User> UserList;

        public GetUsersDetailViewModel()
        {
            UserIds = new List<string>();
            Users = new List<User>();
            UserList = new ObservableCollection<User>();
        }

        private string _userName;

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }


        public void GetUsers()
        {
            var getUserRequest = new GetUserRequestObj(UserIds, new GetUserDetailViewModelPresenterCallBack(this));
            var getUserUseCase = new GetUserUseCase(getUserRequest);
            getUserUseCase.Execute();
        }

        public EventHandler GotUser;
        public void GetUserSuccess(List<User> users)
        {
            Users = users;
            UserList.Clear();
            foreach (var user in Users)
            {
                UserList.Add(user);
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
                        _getUsersDetailViewModel.GetUserSuccess(getUserResponseObj.Users);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}
