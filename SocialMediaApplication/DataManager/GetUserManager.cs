using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;

namespace SocialMediaApplication.DataManager
{
    public sealed class GetUserManager : IGetUserManager
    {
        private static GetUserManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private GetUserManager() { }

        public static GetUserManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new GetUserManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;
        public async Task GetUserAsync(GetUserRequestObj getUserRequestObj, GetUserUseCaseCallBack getUserUseCaseCallBack)
        {
            try
            {
                List<User> users = new List<User>();
                foreach (var userId in getUserRequestObj.UserIds)
                {
                    var user = await _userDbHandler.GetUserAsync(userId);
                    users.Add(user);
                } 
                getUserUseCaseCallBack?.OnSuccess(new GetUserResponseObj(users));
            }
            catch (Exception e)
            {
                getUserUseCaseCallBack?.OnError(e);
            }
        }
    }
}
