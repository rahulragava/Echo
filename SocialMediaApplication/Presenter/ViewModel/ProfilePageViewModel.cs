using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;
using SocialMediaApplication.Presenter.View.ProfileView;
using Windows.UI.Xaml.Controls;

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
        public List<string> MaritalStatuses;
        public List<string> Genders;
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

        private string _gender;
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private string _maritalStatus;
        public string MaritalStatus
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

        private TextPostBObj _textPost;

        public TextPostBObj TextPost
        {
            get => _textPost;
            set => SetProperty(ref _textPost, value);
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

        public Reaction PostReaction { get; set; }

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

        public Reaction CommentReaction { get; set; }

        public void ChangeInCommentReactions(List<Reaction> reactions)
        {
            var flag = false;
            foreach (var reaction in reactions)
            {
                if (reaction.ReactedBy == CommentReaction.ReactedBy)
                {
                    CommentReactions.Remove(reaction);
                    CommentReactions.Add(CommentReaction);
                    flag = true;
                }
            }
            if (!flag)
            {
                CommentReactions.Add(CommentReaction);
            }
            flag = false;
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
        public ResourceLoader ResourceLoader = ResourceLoader.GetForCurrentView();

        public ProfilePageViewModel()
        {
            User = new UserBObj();
            TextPosts = new ObservableCollection<TextPostBObj>();
            PollPosts = new ObservableCollection<PollPostBObj>();
            PostList = new ObservableCollection<PostBObj>();
            Followers = new ObservableCollection<string>();
            Followings = new ObservableCollection<string>();
            Genders = new List<string>();
            MaritalStatuses = new List<string>();
            var genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            var maritalStatuses = Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>().ToList();

            foreach (var gender in genders)
            {
                Genders?.Add(ResourceLoader.GetString(gender.ToString()));
            }
            foreach (var maritalStatus in maritalStatuses)
            {
                MaritalStatuses?.Add(ResourceLoader.GetString(maritalStatus.ToString()));
            }
        }
            
        public void EditProfileImage()
        {
            var editProfileImageRequest = 
                new EditProfileImageRequest(AppSettings.UserId, ProfileImage);
            var editProfileImageUseCase = new EditProfileImageUseCase(editProfileImageRequest, new EditProfileImagePresenterCallBack(this));
            editProfileImageUseCase.Execute();
        }

        public void EditHomeImage()
        {
            var editHomeIconRequest =
                new EditHomeImageRequest(AppSettings.UserId, HomePageImage);
            var editHomeIconUseCase = new EditHomeImageUseCase(editHomeIconRequest, new EditHomeImagePresenterCallBack(this));
            editHomeIconUseCase.Execute();
        }

        public void GetUser(string userId)
        {
            var getUserProfileRequestObj = new GetUserProfileRequestObj(userId);
            var getUserProfileUseCase = new GetUserProfileUseCase(getUserProfileRequestObj, new GetUserProfilePresenterCallBack(this));
            getUserProfileUseCase.Execute();
        }

        public void EditUserProfile(string userId,string userName,string firstName,string lastName,int genderInt,int maritalStatusInt,string education,string occupation,string place)
        {
            Gender gender = (Gender)Enum.Parse(typeof(Gender), genderInt.ToString());
            MaritalStatus maritalStatus= (MaritalStatus)Enum.Parse(typeof(MaritalStatus), maritalStatusInt.ToString());
            var editUserProfileRequestObj = new EditUserProfileRequestObj(userId,userName,firstName,lastName,gender,maritalStatus,education,occupation,place);
            var editUserProfileUseCase = new EditUserProfileUseCase(editUserProfileRequestObj, new EditUserProfilePresenterCallBack(this));
            editUserProfileUseCase.Execute();
        }

        public void FollowUnFollowSearchedUser()
        {
            var followUnFollowSearchedUserRequest = new FollowUnFollowSearchedUserRequest(User.Id,AppSettings.UserId);
            var followUnFollowSearchedUserUseCase =
                new FollowUnFollowSearchedUserUseCase(followUnFollowSearchedUserRequest, new FollowUnFollowPresenterCallBack(this));
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
            Gender = ResourceLoader.GetString(User.Gender.ToString());   
            MaritalStatus = ResourceLoader.GetString(User.MaritalStatus.ToString());   
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

            OrderByDescending(PostList);

            ProfileView.GetUserSucceed();
        }

        public ObservableCollection<T> OrderByDescending<T>(ObservableCollection<T> posts) where T:PostBObj
        {
            var temporaryCollection = new ObservableCollection<T>(posts.OrderByDescending(p => p.CreatedAt));
            posts.Clear();
            foreach (T post in temporaryCollection)
            {
                posts.Add(post);
            }
            return posts;
        }

        public ObservableCollection<T> OrderByAscending<T>(ObservableCollection<T> posts) where T:PostBObj
        {
            var temporaryCollection = new ObservableCollection<T>(posts.OrderBy(p => p.CreatedAt));
            posts.Clear();
            foreach (T post in temporaryCollection)
            {
                posts.Add(post);
            }
            return posts;
        }


        public void EditUserSuccess(User user)
        {
            this.User.FirstName = user.FirstName;
            this.User.LastName = user.LastName;
            this.User.Place = user.Place;
            this.User.Occupation = user.Occupation;
            this.User.Education= user.Education;
            this.User.Gender = user.Gender;
            this.User.MaritalStatus = user.MaritalStatus;
            this.User.UserName = user.UserName;

            FirstName = user.FirstName;
            LastName = user.LastName;
            Place = user.Place;
            Occupation = user.Occupation;
            Education = user.Education;
            Gender = ResourceLoader.GetString(user.Gender.ToString()) ;
            MaritalStatus = ResourceLoader.GetString(user.MaritalStatus.ToString());
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
