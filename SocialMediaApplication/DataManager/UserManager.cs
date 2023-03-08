using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.View;
using SocialMediaApplication.Services.Contract;
using SocialMediaApplication.Util;

namespace SocialMediaApplication.DataManager
{
    public class UserManager : IUserManager,IGetUserNames
    {
        private static UserManager Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly UserManagerHelper _userManagerHelper = new UserManagerHelper();

        private UserBObj _userBObj;

        private UserManager() { }

        public static UserManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new UserManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly IUserDbHandler _userDbHandler= UserDbHandler.GetInstance;
        private readonly IUserCredentialDbHandler _userCredentialDbHandler = UserCredentialDbHandler.GetInstance; 
        private readonly IFollowerDbHandler _followerDbHandler = FollowerDbHandler.GetInstance;
        private readonly FetchPostManager _fetchPostManager = FetchPostManager.GetInstance;

        public async void LoginUserAsync(LoginRequest loginRequest,LogInUseCaseCallBack loginUseCaseCallBack)
        {
            try
            {
                _userBObj = null;
                //_fetchPostManager.PostBobjs = null;
                var userManagerHelper = new UserManagerHelper();
                _userBObj = await userManagerHelper.VerifyAndGetUserBObjAsync(loginRequest.Email,
                    loginRequest.Password);
                loginUseCaseCallBack?.OnSuccess(new LoginResponse(_userBObj));

            }
            catch (Exception ex)
            {
                loginUseCaseCallBack?.OnError(ex);
            }
        }

        public async void SignUpUserAsync(SignUpRequestObj signUpRequestObj, SignUpUseCaseCallBack signUpUseCaseCallBack)
        {
            try
            {
                _userBObj = null;
                //_fetchPostManager.PostBobjs = null;
                var validation = new Validation();
                if (await validation.IsUserNameAlreadyExistAsync(signUpRequestObj.UserName))
                {
                    throw new Exception("user name already existed.");
                }

                if (await validation.IsUserMailAlreadyExistAsync(signUpRequestObj.Email))
                {
                    throw new Exception("already existing mail. try login");
                }

                _userBObj = new UserBObj()
                {
                    UserName = signUpRequestObj.UserName,
                    MailId = signUpRequestObj.Email,
                    CreatedAt = DateTime.Now,
                };
                var userCred = new UserCredential(_userBObj.Id, signUpRequestObj.Password);
                await _userCredentialDbHandler.InsertUserCredentialAsync(userCred);
                var user = new User()
                {
                    Id = _userBObj.Id,
                    UserName = _userBObj.UserName,
                    MailId = _userBObj.MailId
                };
                
                await _userDbHandler.InsertUserAsync(user);
               
                signUpUseCaseCallBack?.OnSuccess(new SignUpResponse(_userBObj));
            }
            catch (Exception ex)
            {
                signUpUseCaseCallBack?.OnError(ex);
            }
        }

        public async void RemoveUserAsync(UserBObj user)
        {
            try
            {
                await _userDbHandler.RemoveUserAsync(user.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async void GetUserBObjAsync(GetUserProfileRequestObj getUserProfileRequest, GetUserProfileUseCaseCallBack getUserProfileUseCaseCallBack)
        {
            try
            {
                var userId = getUserProfileRequest.UserId;
                var user = await Task.Run(() => _userDbHandler.GetUserAsync(userId)).ConfigureAwait(false);
                var textPosts = await _fetchPostManager.GetUserTextPostBObjsAsync(userId).ConfigureAwait(false);
                var pollPosts = await _fetchPostManager.GetUserPollPostBObjsAsync(userId).ConfigureAwait(false);
                var followerIds = (await _followerDbHandler.GetUserFollowerIdsAsync(userId).ConfigureAwait(false))
                    .ToList();
                var followingIds = (await _followerDbHandler.GetUserFollowingIdsAsync(userId).ConfigureAwait(false))
                    .ToList();
                foreach (var t in textPosts)
                {
                    t.UserName = user.UserName;
                }
                foreach (var t in pollPosts)
                {
                    t.UserName = user.UserName;
                }
                var userBusinessObject = _userManagerHelper.ConvertModelToBObj(user, textPosts, pollPosts, followerIds, followingIds);
                _userBObj = userBusinessObject;
                getUserProfileUseCaseCallBack?.OnSuccess(new GetUserProfileResponseObj(userBusinessObject));

            }
            catch (Exception ex)
            {
                getUserProfileUseCaseCallBack?.OnError(ex);
            }
        }

        public async void GetUserNamesAsync(GetUserNamesRequestObj getUserNameRequestObj, GetUserNamesUseCaseCallBack getUserNamesUseCaseCallBack)
        {
            try
            {
                var users = await _userDbHandler.GetAllUserAsync();
                var userNames = users.Select(u => u.UserName).ToList();
                var userIds= users.Select(u => u.Id).ToList();
                userIds.Remove(AppSettings.UserId);
                var currentUserName = (await _userDbHandler.GetUserAsync(AppSettings.UserId)).UserName;
                userNames.Remove(currentUserName);
                getUserNamesUseCaseCallBack?.OnSuccess(new GetUserNamesResponseObj(userNames,userIds));
            }
            catch (Exception ex)
            {
                getUserNamesUseCaseCallBack?.OnError(ex);
            }
        }

        public async void EditUserBObjAsync(EditUserProfileRequestObj editUserProfileRequestObj,
            EditUserProfileUseCaseCallBack editUserProfileUseCaseCallBack)
        {
            try
            {
                var userName = editUserProfileRequestObj.UserName;
                var firstName = editUserProfileRequestObj.FirstName;
                var lastName = editUserProfileRequestObj.LastName;
                var occupation = editUserProfileRequestObj.Occupation;
                var education= editUserProfileRequestObj.Education;
                var place = editUserProfileRequestObj.Place;
                var gender = editUserProfileRequestObj.Gender;
                var maritalStatus = editUserProfileRequestObj.MaritalStatus;

                _userBObj.UserName = userName;
                _userBObj.FirstName = firstName;
                _userBObj.LastName = lastName;
                _userBObj.Occupation = occupation;
                _userBObj.Education = education;
                _userBObj.Place = place;
                _userBObj.Gender = gender;
                _userBObj.MaritalStatus = maritalStatus;
                
                var user = ConvertBObjToEntity(_userBObj);
                await _userDbHandler.UpdateUserAsync(user);
                editUserProfileUseCaseCallBack?.OnSuccess(new EditUserProfileResponseObj(_userBObj));
            }
            catch (Exception ex)
            {
                editUserProfileUseCaseCallBack?.OnError(ex);
            }
        }

        public static User ConvertBObjToEntity(UserBObj userBObj)
        {
            return new User()
            {
                Id = userBObj.Id,
                UserName = userBObj.UserName,
                FirstName = userBObj.FirstName,
                LastName = userBObj.LastName,
                Occupation = userBObj.Occupation,
                Education = userBObj.Education,
                Place = userBObj.Place,
                MailId = userBObj.MailId,
                Gender = userBObj.Gender,
                CreatedAt = userBObj.CreatedAt,
                MaritalStatus = userBObj.MaritalStatus,
            };
        }

        //public async Task<List<PostBObj>> GetUserPostBObjsAsync(string userId)
        //{
        //    var postBObjs = new List<PostBObj>();
        //    if (_userBObj == null)
        //    {
        //        postBObjs = await _fetchPostManager.GetUserPostBObjsAsync(userId).ConfigureAwait(false);
        //    }
        //    else
        //    {
        //        postBObjs.AddRange(_userBObj.TextPosts);
        //        postBObjs.AddRange(_userBObj.PollPosts);
        //    }
        //    return postBObjs;
        //}

        public async Task<UserBObj> GetUserBObjWithoutIdAsync(string userMailId)
        {
            var user = (await Task.Run(() => _userDbHandler.GetAllUserAsync()).ConfigureAwait(false)).ToList().SingleOrDefault(u => u.MailId == userMailId);
            if (user == null) return null;
            var textPosts = (await Task.Run(() => _fetchPostManager.GetUserTextPostBObjsAsync(user.Id)).ConfigureAwait(false)).ToList();
            var pollPosts = (await Task.Run(() => _fetchPostManager.GetUserPollPostBObjsAsync(user.Id)).ConfigureAwait(false)).ToList();
            var followerIds = (await Task.Run(() => _followerDbHandler.GetUserFollowerIdsAsync(user.Id)).ConfigureAwait(false)).ToList();
            var followingIds = (await Task.Run(() => _followerDbHandler.GetUserFollowingIdsAsync(user.Id)).ConfigureAwait(false)).ToList();
            var userBObj = _userManagerHelper.ConvertModelToBObj(user, textPosts, pollPosts, followerIds, followingIds);

            return userBObj;
        }

        //public UserBObj ConvertModelToBObj(User user, List<TextPostBObj> textPosts, List<PollPostBObj> pollPosts, List<string> followersId, List<string> followingsId)
        //{
        //    var userBObj = new UserBObj
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Gender = user.Gender,
        //        CreatedAt = user.CreatedAt,
        //        FormattedCreatedTime = user.CreatedAt.ToString("dddd, dd MMMM yyyy"),
        //        MaritalStatus = user.MaritalStatus,
        //        Occupation = user.Occupation,
        //        Education = user.Education,
        //        Place = user.Place,
        //        TextPosts = textPosts,
        //        PollPosts = pollPosts,
        //        FollowersId = followersId,
        //        FollowingsId = followingsId
        //    };

        //    return userBObj;
        //}


        public async Task UnFollowAsync(string viewingUserId, string searchedUserId)
        {
            var userFollowerId = (await Task.Run(() => _followerDbHandler.GetFollowerAsync(viewingUserId,searchedUserId)).ConfigureAwait(false)).Id;
            await Task.Run(() => _followerDbHandler.RemoveFollowerAsync(userFollowerId)).ConfigureAwait(false);

            _userBObj?.FollowingsId.Remove(searchedUserId);
        }

        public async Task FollowAsync(string viewingUserId, string searchedUserId)
        {
            var userFollowerId = (await Task.Run(() => _followerDbHandler.GetFollowerAsync(viewingUserId, searchedUserId)).ConfigureAwait(false)).Id;
            await Task.Run(() => _followerDbHandler.InsertFollowerAsync(new Follower(){FollowerId = viewingUserId, FollowingId = searchedUserId})).ConfigureAwait(false);

            _userBObj?.FollowingsId.Add(searchedUserId);
        }


        
    }
}
