using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager.CustomException;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
{
    public class LogInManager : ILoginManager
    {
        private static LogInManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private LogInManager() { }

        public static LogInManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new LogInManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;
        private readonly IFollowerDbHandler _followerDbHandler = FollowerDbHandler.GetInstance;
        private readonly FetchPostManager _fetchPostManager = FetchPostManager.GetInstance;

        public async Task LoginUserAsync(LoginRequest loginRequest, LogInUseCaseCallBack loginUseCaseCallBack)
        {
            try
            {
                var userBObj = await VerifyAndGetUserBObjAsync(loginRequest.Email, loginRequest.Password);
                loginUseCaseCallBack?.OnSuccess(new LoginResponse(userBObj));

            }
            catch (NoSuchUserException noSuchUserException)
            {
                loginUseCaseCallBack?.OnError(noSuchUserException);
            }
            catch (Exception ex)
            {
                loginUseCaseCallBack?.OnError(ex);
            }
        }

        public async Task<UserBObj> VerifyAndGetUserBObjAsync(string userMailId, string userPassword)
        {
            var userCredentialManager = UserCredentialManager.GetInstance;
            var userToBeVerified = await GetUserBObjWithoutIdAsync(userMailId);
            UserCredential userCredential;
            if (userToBeVerified != null)
                userCredential = await userCredentialManager.GetUserCredentialAsync(userToBeVerified.Id);
            else
            {
                userCredential = null;
            }
            if (userCredential != null && userCredential.Password == userPassword)
            {
                return userToBeVerified;
            }
            
            throw new NoSuchUserException("No such user exists");
        }

        public async Task<UserBObj> GetUserBObjWithoutIdAsync(string userMailId)
        {
            var user = (await _userDbHandler.GetAllUserAsync().ConfigureAwait(false)).SingleOrDefault(u => u.MailId == userMailId);
            if (user == null)
            {
                return null;
            }

            var textPosts = await _fetchPostManager.GetUserTextPostBObjsAsync(user.Id);
            var pollPosts = await _fetchPostManager.GetUserPollPostBObjsAsync(user.Id);
            var followerIds = (await _followerDbHandler.GetUserFollowerIdsAsync(user.Id)).ToList();
            var followingIds = (await _followerDbHandler.GetUserFollowingIdsAsync(user.Id)).ToList();
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
                MailId = user.MailId,
                Gender = user.Gender,
                CreatedAt = user.CreatedAt,
                FormattedCreatedTime = user.CreatedAt.ToString("dddd, dd MMMM yyyy"),
                MaritalStatus = user.MaritalStatus,
                Occupation = user.Occupation,
                Education = user.Education,
                ProfileIcon = user.ProfileIcon,
                HomePageIcon = user.HomePageIcon,
                Place = user.Place,
                TextPosts = textPosts,
                PollPosts = pollPosts,
                FollowersId = followersId,
                FollowingsId = followingsId
            };
            return userBObj;
        }
    }
}

