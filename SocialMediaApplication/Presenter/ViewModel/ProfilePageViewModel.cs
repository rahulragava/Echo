using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class ProfilePageViewModel : ObservableObject
    {
        public UserBObj user;

        public ObservableCollection<TextPostBObj> TextPosts;
        public ObservableCollection<PollPostBObj> PollPosts;
        public ObservableCollection<string> Followers;
        public ObservableCollection<string> Followings;

        private int _userPostCount;
        public int UserPostCount
        {
            get => _userPostCount;
            set => SetProperty(ref _userPostCount, value);
        }



        private int _userFollowerCount;
        public int UserFollowerCount
        {
            get => _userFollowerCount;
            set => SetProperty(ref _userFollowerCount, value);
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        private string _firstName = "-";
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _formattedCreatedTime;
        public string FormattedCreatedTime
        {
            get => _formattedCreatedTime;
            set => SetProperty(ref _formattedCreatedTime, value);
        }
        private string _lastName = "-";
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName , value);
        }

        private string _education = "-";
        public string Education
        {
            get => _education;
            set => SetProperty(ref _education, value);
        }

        private string _occupation = "-";
        public string Occupation
        {
            get => _occupation;
            set => SetProperty(ref _occupation, value);
        }

        private string _place = "-";
        public string Place
        {
            get => _place;
            set => SetProperty(ref _place, value);
        }

        private Gender _gender;
        public Gender Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private MaritalStatus _maritalStatus;
        public MaritalStatus MaritalStatus
        {
            get => _maritalStatus;
            set => SetProperty(ref _maritalStatus, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private int _userFollowingCount;
        public int UserFollowingCount
        {
            get => _userFollowingCount;
            set => SetProperty(ref _userFollowingCount, value);
        }
        
        public ProfilePageViewModel()
        {
            user = new UserBObj();
            TextPosts = new ObservableCollection<TextPostBObj>();
            PollPosts = new ObservableCollection<PollPostBObj>();
            Followers = new ObservableCollection<string>();
            Followings = new ObservableCollection<string>();
        }
        public void GetUser(string userId)
        {
            if (userId == null)
            {
                userId = AppSettings.LocalSettings.Values["user"].ToString();
            }
            else if(string.Empty == userId)
            {
                return;
            }  

            var getUserProfileRequestObj = new GetUserProfileRequestObj(userId,new GetUserProfilePresenterCallBack(this));
            var getUserProfileUseCase = new GetUserProfileUseCase(getUserProfileRequestObj);
            getUserProfileUseCase.Execute();
        }

        public void EditUserProfile(string userName,string firstName,string lastName,int genderInt,int maritalStatusInt,string education,string occupation,string place)
        {
            Gender gender = (Gender)Enum.Parse(typeof(Gender), genderInt.ToString());
            MaritalStatus maritalStatus= (MaritalStatus)Enum.Parse(typeof(MaritalStatus), maritalStatusInt.ToString());
            var editUserProfileRequestObj = new EditUserProfileRequestObj(userName,firstName,lastName,gender,maritalStatus,education,occupation,place, new EditUserProfilePresenterCallBack(this));
            var editUserProfileUseCase = new EditUserProfileUseCase(editUserProfileRequestObj);
            editUserProfileUseCase.Execute();
        }

        public void GetUserSuccess(UserBObj userBObj)
        {
            this.user = userBObj;
            UserPostCount = user.TextPosts.Count + user.PollPosts.Count;
            UserFollowerCount = user.FollowersId.Count;
            UserFollowingCount= user.FollowingsId.Count;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Place = user.Place;
            Occupation = user.Occupation;
            Education = user.Education;
            Gender = user.Gender;   
            MaritalStatus = user.MaritalStatus;
            CreatedAt = user.CreatedAt;
            FormattedCreatedTime = user.FormattedCreatedTime;
            UserName = user.UserName;
            TextPosts.Clear();
            foreach (var textPost in user.TextPosts)
            {
                TextPosts.Add(textPost);
            }
            PollPosts.Clear();
            foreach (var pollPost in user.PollPosts)
            {
                PollPosts.Add(pollPost);
            }
            Followers.Clear();
            foreach (var follower in user.FollowersId)
            {
                Followers.Add(follower);
                
            }
            Followings.Clear();
            foreach (var following in user.FollowingsId)
            {
                Followers.Add(following);
            }
        }

        public void EditUserSuccess(UserBObj userBObj)
        {
            this.user = userBObj;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Place = user.Place;
            Occupation = user.Occupation;
            Education = user.Education;
            Gender = user.Gender;
            MaritalStatus = user.MaritalStatus;
            UserName = user.UserName;
        }


    }

    public class GetUserProfilePresenterCallBack : IPresenterCallBack<GetUserProfileResponseObj>
    {
        private readonly ProfilePageViewModel _profilePageViewModel;

        public GetUserProfilePresenterCallBack(ProfilePageViewModel profilePageViewModel)
        {
            this._profilePageViewModel = profilePageViewModel;
        }

        public void OnSuccess(GetUserProfileResponseObj getUserProfileResponseObj)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _profilePageViewModel.GetUserSuccess(getUserProfileResponseObj.User);
                }
            );
        }

        public void OnError(Exception ex)
        {
        }
    }

    public class EditUserProfilePresenterCallBack : IPresenterCallBack<EditUserProfileResponseObj>
    {
        private readonly ProfilePageViewModel _profilePageViewModel;

        public EditUserProfilePresenterCallBack(ProfilePageViewModel profilePageViewModel)
        {
            this._profilePageViewModel = profilePageViewModel;
        }

        public void OnSuccess(EditUserProfileResponseObj editedUserProfileResponseObj)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _profilePageViewModel.EditUserSuccess(editedUserProfileResponseObj.User);
                }
            );
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
