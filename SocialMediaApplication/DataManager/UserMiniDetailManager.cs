using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
{
    public sealed class UserMiniDetailManager : IUserMiniDetailManager
    {
        private static UserMiniDetailManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private UserMiniDetailManager() { }

        public static UserMiniDetailManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new UserMiniDetailManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;
        private readonly IFollowerDbHandler _followerDbHandler= FollowerDbHandler.GetInstance;

        public async Task GetUserMiniDetailsAsync(UserMiniDetailRequest userMiniDetailRequest,
            UserMiniDetailUseCaseCallBack userMiniDetailUseCaseCallBack)
        {
            try
            {
                var user = await _userDbHandler.GetUserAsync(userMiniDetailRequest.UserId);
                var follower = (await _followerDbHandler.GetUserFollowerIdsAsync(userMiniDetailRequest.UserId)).ToList();
                var following = (await _followerDbHandler.GetUserFollowingIdsAsync(userMiniDetailRequest.UserId)).ToList();
                userMiniDetailUseCaseCallBack?.OnSuccess(new UserMiniDetailResponse(user,follower,following));
            }
            catch (Exception e)
            {
                userMiniDetailUseCaseCallBack?.OnError(e);
            }
        }
    }
}
