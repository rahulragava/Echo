using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager.CustomException;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
{
    public class SignUpManager : ISignUpManager 
    {
        private static SignUpManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private SignUpManager() { }

        public static SignUpManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new SignUpManager();
                        }
                    }
                }
                return Instance;
            }
        }
        private readonly IUserCredentialDbHandler _userCredentialDbHandler = UserCredentialDbHandler.GetInstance;
        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;


        public async Task SignUpUserAsync(SignUpRequestObj signUpRequestObj, SignUpUseCaseCallBack signUpUseCaseCallBack)
        {
            try
            {
                if (await IsUserNameAlreadyExistAsync(signUpRequestObj.UserName))
                {
                    throw new UserNameAlreadyExistException("Already User with this UserName Exist! Try Something new");
                }

                if (await IsUserMailAlreadyExistAsync(signUpRequestObj.Email))
                {
                    throw new UserMailAlreadyExistException("already existing mail. try login");
                }

                var userBObj = new UserBObj()
                {
                    UserName = signUpRequestObj.UserName,
                    MailId = signUpRequestObj.Email,
                    CreatedAt = DateTime.Now,
                };
                var userCredential = new UserCredential(userBObj.Id, signUpRequestObj.Password);
                await _userCredentialDbHandler.InsertUserCredentialAsync(userCredential);
                var user = new User()
                {
                    Id = userBObj.Id,
                    UserName = userBObj.UserName,
                    MailId = userBObj.MailId,
                    CreatedAt = DateTime.Now
                };

                await _userDbHandler.InsertUserAsync(user);

                signUpUseCaseCallBack?.OnSuccess(new SignUpResponse(userBObj));
            }
            catch (UserNameAlreadyExistException userNameAlreadyExistException)
            {
                signUpUseCaseCallBack?.OnError(userNameAlreadyExistException);
            }
            catch (UserMailAlreadyExistException userMailAlreadyExistException)
            {
                signUpUseCaseCallBack?.OnError(userMailAlreadyExistException);
            }
            catch (Exception ex)
            {
                signUpUseCaseCallBack?.OnError(ex);
            }
        }

        public async Task<bool> IsUserNameAlreadyExistAsync(string userName)
        {
            var userNames = await GetUserNamesAsync().ConfigureAwait(false);
            return userNames.Any() && userNames.Contains(userName);
        }

        public async Task<bool> IsUserMailAlreadyExistAsync(string userMail)
        {
            var userMailIds = await GetUserMailIdsAsync().ConfigureAwait(false);
            return userMailIds.Any() && userMailIds.Contains(userMail);
        }

        public async Task<List<string>> GetUserMailIdsAsync()
        {
            return (await _userDbHandler.GetAllUserAsync().ConfigureAwait(false)).Select(user => user.MailId).ToList();
        }

        public async Task<List<string>> GetUserNamesAsync()
        {
            return (await _userDbHandler.GetAllUserAsync().ConfigureAwait(false)).Select(user => user.UserName).ToList();
        }
    }
}
