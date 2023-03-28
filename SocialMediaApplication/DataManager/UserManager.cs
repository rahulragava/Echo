using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task LoginUserAsync(LoginRequest loginRequest,LogInUseCaseCallBack loginUseCaseCallBack)
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

        public async Task SignUpUserAsync(SignUpRequestObj signUpRequestObj, SignUpUseCaseCallBack signUpUseCaseCallBack)
        {
            try
            {
                _userBObj = null;
                //_fetchPostManager.PostBobjs = null;
                var validation = new Validation();
                if (await validation.IsUserNameAlreadyExistAsync(signUpRequestObj.UserName))
                {
                    throw new Exception("Already User with this UserName Exist! Try Something new");
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
                    MailId = _userBObj.MailId,
                    CreatedAt = DateTime.Now
                };
                
                await _userDbHandler.InsertUserAsync(user);
               
                signUpUseCaseCallBack?.OnSuccess(new SignUpResponse(_userBObj));
            }
            catch (Exception ex)
            {
                signUpUseCaseCallBack?.OnError(ex);
            }
        }
        
        public async Task GetUserBObjAsync(GetUserProfileRequestObj getUserProfileRequest, GetUserProfileUseCaseCallBack getUserProfileUseCaseCallBack)
        {
            try
            {
                var userId = getUserProfileRequest.UserId;
                var user = await _userDbHandler.GetUserAsync(userId);
                var textPosts = await _fetchPostManager.GetUserTextPostBObjsAsync(userId);
                var pollPosts = await _fetchPostManager.GetUserPollPostBObjsAsync(userId);
                var followerIds = (await _followerDbHandler.GetUserFollowerIdsAsync(userId))
                    .ToList();
                var followingIds = (await _followerDbHandler.GetUserFollowingIdsAsync(userId))
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

        public async Task GetUserNamesAsync(GetUserNamesRequestObj getUserNameRequestObj, GetUserNamesUseCaseCallBack getUserNamesUseCaseCallBack)
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

        public static event Action<string> UserNameChanged;
        public async Task EditUserBObjAsync(EditUserProfileRequestObj editUserProfileRequestObj,
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
                var originalUserName = _userBObj.UserName;

                //_userBObj.UserName = userName;
                _userBObj.FirstName = firstName;
                _userBObj.LastName = lastName;
                _userBObj.Occupation = occupation;
                _userBObj.Education = education;
                _userBObj.Place = place;
                _userBObj.Gender = gender;
                _userBObj.MaritalStatus = maritalStatus;

                var users = (await _userDbHandler.GetAllUserAsync()).ToList();
                var userNames = (users.Select(u => u.UserName)).ToList();
                userNames.Remove(originalUserName);
                var isUserNameAlreadyExist = userNames.Contains(userName);
                _userBObj.UserName = isUserNameAlreadyExist ? originalUserName : userName;
                var user = ConvertBObjToEntity(_userBObj);
                await _userDbHandler.UpdateUserAsync(user);
                editUserProfileUseCaseCallBack?.OnSuccess(new EditUserProfileResponseObj(_userBObj));
                if (isUserNameAlreadyExist)
                {
                    throw new Exception("user name already exist");
                }
                UserNameChanged?.Invoke(user.UserName);
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
                HomePageIcon = userBObj.HomePageIcon,
                ProfileIcon = userBObj.ProfileIcon,
                MaritalStatus = userBObj.MaritalStatus,
            };
        }

    }
}
