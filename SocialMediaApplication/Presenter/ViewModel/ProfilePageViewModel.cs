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
using SocialMediaApplication.Presenter.View.ProfileView;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class ProfilePageViewModel : ObservableObject
    {
        public UserBObj User;
        public ObservableCollection<TextPostBObj> TextPosts;
        public ObservableCollection<PollPostBObj> PollPosts;
        public ObservableCollection<PostBObj> PostList;

        public ObservableCollection<string> Followers;
        public ObservableCollection<string> Followings;
        public List<MaritalStatus> MaritalStatuses;
        public List<Gender> Genders;
        public IProfileView ProfileView { get; set; }

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

        public Reaction PostReaction { get; set; }

        private string _homePageImage = "";
        public string HomePageImage
        {
            get => _homePageImage;
            set => SetProperty(ref _homePageImage, value);
        }

        public void ChangeInReactions(List<Reaction> reactions)
        {
            var flag = false;
            foreach (var reaction in reactions)
            {
                if (reaction.ReactedBy == PostReaction.ReactedBy)
                {
                    PostReactions.Remove(reaction);
                    PostReactions.Add(PostReaction);
                    flag = true;
                }
            }
            if (!flag)
            {
                PostReactions.Add(PostReaction);
            }
            flag = false;
        }

        public ProfilePageViewModel()
        {
            User = new UserBObj();
            TextPosts = new ObservableCollection<TextPostBObj>();
            PollPosts = new ObservableCollection<PollPostBObj>();
            PostList = new ObservableCollection<PostBObj>();
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

        public ObservableCollection<Reaction> PostReactions = new ObservableCollection<Reaction>();

        public void SetPostReactions(List<Reaction> reactions)
        {
            if (PostReactions != null)
            {
                PostReactions.Clear();
                foreach (var reaction in reactions)
                {
                    PostReactions.Add(reaction);
                }
            }
        }

        public ObservableCollection<Reaction> CommentReactions = new ObservableCollection<Reaction>();

        public void SetCommentReactions(List<Reaction> reactions)
        {
            if (CommentReactions != null)
            {
                CommentReactions.Clear();
                foreach (var reaction in reactions)
                {
                    CommentReactions.Add(reaction);
                }
            }
        }

        public void GetUser(string userId)
        {
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

            var followUnFollowSearchedUserRequest = new FollowUnFollowSearchedUserRequest(User.Id,AppSettings.UserId,new FollowUnFollowPresenterCallBack(this));
            var followUnFollowSearchedUserUseCase =
                new FollowUnFollowSearchedUserUseCase(followUnFollowSearchedUserRequest);
            followUnFollowSearchedUserUseCase.Execute();
        }

        public void GetUserSuccess(UserBObj userBObj)
        {
            this.User = userBObj;

            UserPostCount = User.TextPosts.Count + User.PollPosts.Count;
            UserFollowerCount = User.FollowersId.Count;
            UserFollowingCount= User.FollowingsId.Count;
            FirstName = User.FirstName;
            LastName = User.LastName;
            Place = User.Place;
            Occupation = User.Occupation;
            Education = User.Education;
            Gender = User.Gender;   
            MaritalStatus = User.MaritalStatus;
            ProfileImage = User.ProfileIcon;
            HomePageImage = User.HomePageIcon;
            CreatedAt = User.CreatedAt;
            FormattedCreatedTime = User.FormattedCreatedTime;
            UserName = User.UserName;

            foreach (var textPost in User.TextPosts)
            {
                TextPosts.Insert(0,textPost);
                PostList.Insert(0,textPost);
            }
            foreach (var pollPost in User.PollPosts)
            {
                PollPosts.Insert(0,pollPost);
                PostList.Insert(0, pollPost);
            }
          
            Followers.Clear();
            foreach (var follower in User.FollowersId)
            {
                Followers.Add(follower);
            }
            Followings.Clear();
            foreach (var following in User.FollowingsId)
            {
                Followings.Add(following);
            }

            OrderBy(PostList);
            ProfileView.GetUserSucceed();
        }

        public ObservableCollection<PostBObj> OrderBy(ObservableCollection<PostBObj> posts)
        {
            var temporaryCollection = new ObservableCollection<PostBObj>(posts.OrderByDescending(p => p.CreatedAt));
            posts.Clear();
            foreach (PostBObj post in temporaryCollection)
            {
                posts.Add(post);
            }
            return posts;
        }


        public void EditUserSuccess(UserBObj userBObj)
        {
            this.User = userBObj;
            FirstName = User.FirstName;
            LastName = User.LastName;
            Place = User.Place;
            Occupation = User.Occupation;
            Education = User.Education;
            Gender = User.Gender;
            MaritalStatus = User.MaritalStatus;
            UserName = User.UserName;
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
                         _profilePageViewModel.ProfileView.UserNameAlreadyExist();
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

                    _profilePageViewModel.ProfileView.SetProfileImage();
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

                    _profilePageViewModel.ProfileView.SetHomeImage();
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
                        _profilePageViewModel.User.FollowingsId.Add(AppSettings.UserId);
                        _profilePageViewModel.Followings.Add(AppSettings.UserId);
                        _profilePageViewModel.UserFollowingCount += 1;
                    }
                    else
                    {
                        _profilePageViewModel.Followings.Remove(AppSettings.UserId);
                        _profilePageViewModel.User.FollowingsId.Remove(AppSettings.UserId);
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
