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
using SocialMediaApplication.Util;

namespace SocialMediaApplication.DataManager
{
    public sealed class FollowUnFollowManager : IFollowUnFollowManager
    {
        private static FollowUnFollowManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private FollowUnFollowManager()
        {
        }

        public static FollowUnFollowManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new FollowUnFollowManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly IFollowerDbHandler _followerDbHandler = FollowerDbHandler.GetInstance;

        public async Task FollowUnFollowAsync(FollowUnFollowSearchedUserRequest followUnFollowSearchedUserRequest,
            FollowUnFollowSearchedUserUseCaseCallBack followUnFollowSearchedUserUseCaseCallBack)
        {
            try
            {
                var searchedUserFollowers = (await _followerDbHandler.GetUserFollowerIdsAsync(followUnFollowSearchedUserRequest.SearchedUserId)).ToList();
                var follower = await _followerDbHandler.GetFollowerAsync(followUnFollowSearchedUserRequest.ViewingUserId,
                    followUnFollowSearchedUserRequest.SearchedUserId);
                var followingSuccess = false;
                if (follower != null)
                {
                    await _followerDbHandler.RemoveFollowerAsync(follower.Id);
                }
                else
                {
                    follower = new Follower()
                    {
                        FollowerId = followUnFollowSearchedUserRequest.ViewingUserId,
                        FollowingId = followUnFollowSearchedUserRequest.SearchedUserId
                    };
                    await _followerDbHandler.InsertFollowerAsync(follower);
                    followingSuccess = true;
                }
                followUnFollowSearchedUserUseCaseCallBack?.OnSuccess(new FollowUnFollowSearchedUserResponse(follower,followingSuccess));
            }
            catch (Exception e)
            {
                followUnFollowSearchedUserUseCaseCallBack?.OnError(e);
            }
        }
    }
}
