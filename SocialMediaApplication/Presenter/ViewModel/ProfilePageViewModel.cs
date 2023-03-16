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
        public List<MaritalStatus> MaritalStatuses;
        public List<Gender> Genders;

        public event Action UserNameAlreadyExist;


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

        private string _profileImage;
        public string ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }

        private string _homePageImage = "";
        public string HomePageImage
        {
            get => _homePageImage;
            set => SetProperty(ref _homePageImage, value);
        }

        public bool ProfilePageIconUpdation { get; set; }

        public Action ProfilePictureUpdated;
        public Action HomePictureUpdated;

        public ProfilePageViewModel()
        {
            user = new UserBObj();
            TextPosts = new ObservableCollection<TextPostBObj>();
            PollPosts = new ObservableCollection<PollPostBObj>();
            Followers = new ObservableCollection<string>();
            Followings = new ObservableCollection<string>();
            Genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            MaritalStatuses = Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>().ToList();
        }

        public void EditProfileImage()
        {
            var editProfileImageRequest = 
                new EditProfileImageRequest(AppSettings.UserId, ProfileImage, new EditProfileImagePresenterCallBack(this));
            var editProfileImageUseCase = new EditProfileImageUseCase(editProfileImageRequest);
            editProfileImageUseCase.Execute();
        }

        public void EditHomeImage()
        {
            var editHomeIconRequest =
                new EditHomeImageRequest(AppSettings.UserId, HomePageImage, new EditHomeImagePresenterCallBack(this));
            var editHomeIconUseCase = new EditHomeImageUseCase(editHomeIconRequest);
            editHomeIconUseCase.Execute();
        }

        public ObservableCollection<Reaction> Reactions = new ObservableCollection<Reaction>();

        public void SetReactions(List<Reaction> reactions)
        {
            Reactions.Clear();
            foreach (var reaction in reactions)
            {
                Reactions.Add(reaction);
            }
        }

        public void GetUser(string userId)
        {
            //if (userId == null)
            //{
            //    userId = AppSettings.LocalSettings.Values["user"].ToString();
            //}
            //else if(string.Empty == userId)
            //{
            //    return;
            //}  

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

        public void FollowUnFollowSearchedUser()
        {

            var followUnFollowSearchedUserRequest = new FollowUnFollowSearchedUserRequest(user.Id,AppSettings.UserId,new FollowUnFollowPresenterCallBack(this));
            var followUnFollowSearchedUserUseCase =
                new FollowUnFollowSearchedUserUseCase(followUnFollowSearchedUserRequest);
            followUnFollowSearchedUserUseCase.Execute();
        }

        public Action GetUserSucceed;
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
            ProfileImage = user.ProfileIcon;
            HomePageImage = user.HomePageIcon;
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
                Followings.Add(following);
            }
            GetUserSucceed?.Invoke();
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
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                         _profilePageViewModel.UserNameAlreadyExist?.Invoke();
                    }
                );
            }
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

    public class EditProfileImagePresenterCallBack : IPresenterCallBack<EditProfileImageResponse>
    {
        private readonly ProfilePageViewModel _profilePageViewModel;
        public EditProfileImagePresenterCallBack(ProfilePageViewModel profilePageViewModel)
        {
            _profilePageViewModel = profilePageViewModel;
        }

        public void OnSuccess(EditProfileImageResponse response)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {

                    _profilePageViewModel.ProfilePictureUpdated?.Invoke();
                }
            );
        }

        public void OnError(Exception ex)
        {
        }
    }

    public class EditHomeImagePresenterCallBack : IPresenterCallBack<EditHomeImageResponse>
    {
        private readonly ProfilePageViewModel _profilePageViewModel;
        public EditHomeImagePresenterCallBack(ProfilePageViewModel profilePageViewModel)
        {
            _profilePageViewModel = profilePageViewModel;
        }

        public void OnSuccess(EditHomeImageResponse response)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {

                    _profilePageViewModel.HomePictureUpdated?.Invoke();
                }
            );
        }

        public void OnError(Exception ex)
        {
        }
    }

    public class FollowUnFollowPresenterCallBack : IPresenterCallBack<FollowUnFollowSearchedUserResponse>
    {
        private readonly ProfilePageViewModel _profilePageViewModel;

        public FollowUnFollowPresenterCallBack(ProfilePageViewModel profilePageViewModel)
        {
            this._profilePageViewModel = profilePageViewModel;
        }
        public void OnSuccess(FollowUnFollowSearchedUserResponse followUnFollowSearchedUserResponse)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (followUnFollowSearchedUserResponse.FollowingSuccess)
                    {
                        _profilePageViewModel.Followers.Add(AppSettings.UserId);
                        _profilePageViewModel.UserFollowingCount += 1;
                    }
                    else
                    {
                        _profilePageViewModel.Followers.Remove(AppSettings.UserId);
                        _profilePageViewModel.UserFollowingCount -= 1;
                    }
                }
            );
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }

    
}
