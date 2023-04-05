using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;

namespace SocialMediaApplication.DataManager
{
    public class EditUserManager : IEditUserBObj
    {
        private static EditUserManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private EditUserManager() { }

        public static EditUserManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new EditUserManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly IUserDbHandler _userDbHandler= UserDbHandler.GetInstance;
        
        public static event Action<string> UserNameChanged;

        public async Task EditUserBObjAsync(EditUserProfileRequestObj editUserProfileRequestObj, EditUserProfileUseCaseCallBack editUserProfileUseCaseCallBack)
        {
            try
            {
                var user = await _userDbHandler.GetUserAsync(editUserProfileRequestObj.UserId);
                var userName = editUserProfileRequestObj.UserName;
                var firstName = editUserProfileRequestObj.FirstName;
                var lastName = editUserProfileRequestObj.LastName;
                var occupation = editUserProfileRequestObj.Occupation;
                var education= editUserProfileRequestObj.Education;
                var place = editUserProfileRequestObj.Place;
                var gender = editUserProfileRequestObj.Gender;
                var maritalStatus = editUserProfileRequestObj.MaritalStatus;
                var originalUserName = user.UserName;

                user.FirstName = firstName;
                user.LastName = lastName;
                user.Occupation = occupation;
                user.Education = education;
                user.Place = place;
                user.Gender = gender;
                user.MaritalStatus = maritalStatus;

                var userNames = (await _userDbHandler.GetAllUserAsync()).Select(u => u.UserName).ToList();
                userNames.Remove(originalUserName);
                var isUserNameAlreadyExist = userNames.Contains(userName);
                user.UserName = isUserNameAlreadyExist ? originalUserName : userName;
                await _userDbHandler.UpdateUserAsync(user);
                editUserProfileUseCaseCallBack?.OnSuccess(new EditUserProfileResponseObj(user));
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
    }
}
