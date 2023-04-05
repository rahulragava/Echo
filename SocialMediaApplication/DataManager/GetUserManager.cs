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
    public class GetUserManager : IGetUserManager
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

        private readonly IFollowerDbHandler _followerDbHandler = FollowerDbHandler.GetInstance;
        private readonly IFetchPostManager _fetchPostManager = FetchPostManager.GetInstance;
        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;

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
                var userBObj = ConvertModelToBObj(user, textPosts, pollPosts, followerIds, followingIds);
                getUserProfileUseCaseCallBack?.OnSuccess(new GetUserProfileResponseObj(userBObj));
            }
            catch (Exception ex)
            {
                getUserProfileUseCaseCallBack?.OnError(ex);
            }
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
