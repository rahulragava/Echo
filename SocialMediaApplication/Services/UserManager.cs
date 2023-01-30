using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;

namespace SocialMediaApplication.Services
{
    public class UserManager
    {
        private static UserManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private UserBObj _userBObj;

        UserManager() { }

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
        private readonly IFollowerDbHandler _followerDbHandler = FollowerDbHandler.GetInstance;
        private readonly PostManager _postManager = PostManager.GetInstance;

        public async Task SignInUser(string userId)
        {
            _userBObj = null;
            _postManager.PostBobjs = null;
            _userBObj = await Task.Run(() => GetUserBObjAsync(userId)).ConfigureAwait(false);
        }

        public async Task AddUserAsync(User user)
        {
            await Task.Run(() => _userDbHandler.InsertUserAsync(user)).ConfigureAwait(false);
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

        public async Task<List<TextPostBObj>> GetUserTextPostBObjsAsync(string userId)
        {
            List<TextPostBObj> textPostBObjs;
            textPostBObjs = (_userBObj == null) ? (await _postManager.GetUserPostBObjsAsync(userId).ConfigureAwait(false)).OfType<TextPostBObj>().ToList() : _userBObj.TextPosts;

            return textPostBObjs;
        }

        public async Task<List<PollPostBObj>> GetUserPollPostBObjsAsync(string userId)
        {
            List<PollPostBObj> pollPostBObjs;
            pollPostBObjs = (_userBObj == null) ? (await _postManager.GetUserPostBObjsAsync(userId).ConfigureAwait(false)).OfType<PollPostBObj>().ToList() : _userBObj.PollPosts;

            return pollPostBObjs;
        }

        public async Task<UserBObj> GetUserBObjAsync(string userId)
        {
            if (_userBObj != null && _userBObj.Id == userId) return _userBObj;
            var user = await Task.Run(() => _userDbHandler.GetUserAsync(userId)).ConfigureAwait(false);
            var textPosts = await GetUserTextPostBObjsAsync(userId);
            var pollPosts = await GetUserPollPostBObjsAsync(userId);
            var followerIds = (await Task.Run(() => _followerDbHandler.GetUserFollowerIdsAsync(userId)).ConfigureAwait(false)).ToList();
            var followingIds = (await Task.Run(() => _followerDbHandler.GetUserFollowingIdsAsync(userId)).ConfigureAwait(false)).ToList();
            var userBusinessObject = ConvertModelToBObj(user,textPosts,pollPosts,followerIds,followingIds);

            return userBusinessObject;
        }

        public UserBObj ConvertModelToBObj(User user, List<TextPostBObj> textPosts, List<PollPostBObj> pollPosts, List<string> followersId, List<string> followingsId)
        {
            var userBobj = new UserBObj
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

            return userBobj;
        }

        public async Task<List<string>> GetUserNames()
        {
            return (await Task.Run(() => _userDbHandler.GetAllUserAsync()).ConfigureAwait(false)).Select(user => user.UserName).ToList();
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
