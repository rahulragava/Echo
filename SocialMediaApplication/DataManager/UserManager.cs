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
using SocialMediaApplication.Services.Contract;
using SocialMediaApplication.Util;

namespace SocialMediaApplication.DataManager
{
    public class UserManager : IUserManager
    {
        private static UserManager Instance { get; set; }
        private static readonly object PadLock = new object();

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
        private readonly PostManager _postManager = PostManager.GetInstance;

        public async Task LoginUserAsync(LoginRequest loginRequest,LogInUseCaseCallBack loginUseCaseCallBack)
        {
            _userBObj = null;
            _postManager.PostBobjs = null;
            var userManagerHelper = new UserManagerHelper();
            _userBObj = await userManagerHelper.VerifyAndGetUserBObjAsync(loginRequest.Email,loginRequest.Password);

            loginUseCaseCallBack?.OnSuccess(new LoginResponse(_userBObj));
        }

        public async Task SignUpUserAsync(SignUpRequestObj signUpRequestObj, SignUpUseCaseCallBack signUpUseCaseCallBack)
        {
            _userBObj = null;
            _postManager.PostBobjs = null;
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
            await _userDbHandler.InsertUserAsync(_userBObj);

            signUpUseCaseCallBack?.OnSuccess(new SignUpResponse(_userBObj));
        }

        public async Task RemoveUserAsync(UserBObj user)
        {
            await Task.Run(() => _userDbHandler.RemoveUserAsync(user.Id)).ConfigureAwait(false);
        }

        public async Task<List<PostBObj>> GetUserPostBObjsAsync(string userId)
        {
            var postBObjs = new List<PostBObj>();
            if (_userBObj == null)
            {
                postBObjs = await _postManager.GetUserPostBObjsAsync(userId).ConfigureAwait(false);
            }
            else
            {
                postBObjs.AddRange(_userBObj.TextPosts);
                postBObjs.AddRange(_userBObj.PollPosts);
            }
            return postBObjs;
        }

        

        public async Task<UserBObj> GetUserBObjAsync(string userId)
        {
            if (_userBObj != null && _userBObj.MailId == userId) return _userBObj;
            var user = await Task.Run(() => _userDbHandler.GetUserAsync(userId)).ConfigureAwait(false);
            var textPosts = await _postManager.GetUserTextPostBObjsAsync(userId).ConfigureAwait(false);
            var pollPosts = await _postManager.GetUserPollPostBObjsAsync(userId).ConfigureAwait(false);
            var followerIds = (await Task.Run(() => _followerDbHandler.GetUserFollowerIdsAsync(userId)).ConfigureAwait(false)).ToList();
            var followingIds = (await Task.Run(() => _followerDbHandler.GetUserFollowingIdsAsync(userId)).ConfigureAwait(false)).ToList();
            var userBusinessObject = ConvertModelToBObj(user,textPosts,pollPosts,followerIds,followingIds);

            return userBusinessObject;
        }

        public async Task<UserBObj> GetUserBObjWithoutId(string userMailId)
        {
            var user = (await Task.Run(() => _userDbHandler.GetAllUserAsync()).ConfigureAwait(false)).ToList().SingleOrDefault(u => u.MailId == userMailId);
            if (user == null) return null;
            var textPosts = (await Task.Run(() => _postManager.GetUserTextPostBObjsAsync(user.Id)).ConfigureAwait(false)).ToList();
            var pollPosts = (await Task.Run(() => _postManager.GetUserPollPostBObjsAsync(user.Id)).ConfigureAwait(false)).ToList();
            var followerIds = (await Task.Run(() => _followerDbHandler.GetUserFollowerIdsAsync(user.Id)).ConfigureAwait(false)).ToList();
            var followingIds = (await Task.Run(() => _followerDbHandler.GetUserFollowingIdsAsync(user.Id)).ConfigureAwait(false)).ToList();
            var userBObj = ConvertModelToBObj(user, textPosts, pollPosts, followerIds, followingIds);

            return userBObj;
        }

        public UserBObj ConvertModelToBObj(User user, List<TextPostBObj> textPosts, List<PollPostBObj> pollPosts, List<string> followersId, List<string> followingsId)
        {
            var userBObj = new UserBObj
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                CreatedAt = user.CreatedAt,
                MaritalStatus = user.MaritalStatus,
                Occupation = user.Occupation,
                Education = user.Education,
                Place = user.Place,
                TextPosts = textPosts,
                PollPosts = pollPosts,
                FollowersId = followersId,
                FollowingsId = followingsId
            };

            return userBObj;
        }


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
