using System;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Util;

namespace SocialMediaApplication.DataManager
{
    public class GetUserNamesManager : IGetUserNamesManager
    {

        private static GetUserNamesManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private GetUserNamesManager() { }

        public static GetUserNamesManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new GetUserNamesManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;

        public async Task GetUserNamesAsync(GetUserNamesRequestObj getUserNameRequestObj, GetUserNamesUseCaseCallBack getUserNamesUseCaseCallBack)
        {
            try
            {
                var users = await _userDbHandler.GetAllUserAsync();
                var userNames = users.Select(u => u.UserName).ToList();
                var userIds = users.Select(u => u.Id).ToList();
                userIds.Remove(AppSettings.UserId);
                var currentUserName = (await _userDbHandler.GetUserAsync(AppSettings.UserId)).UserName;
                userNames.Remove(currentUserName);
                getUserNamesUseCaseCallBack?.OnSuccess(new GetUserNamesResponseObj(userNames, userIds));
            }
            catch (Exception ex)
            {
                getUserNamesUseCaseCallBack?.OnError(ex);
            }
        }
    }
}
