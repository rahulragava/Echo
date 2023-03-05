using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Services.Contract;

namespace SocialMediaApplication.Util
{
    public class UserManagerHelper
    {
        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;
        private readonly IFollowerDbHandler _followerDbHandler = FollowerDbHandler.GetInstance;

        readonly FetchPostManager _fetchPostManager= FetchPostManager.GetInstance;
        
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
            throw new Exception("No such user exists");
        }

        public async Task<UserBObj> GetUserBObjWithoutIdAsync(string userMailId)
        {   
            var user = (await _userDbHandler.GetAllUserAsync().ConfigureAwait(false)).SingleOrDefault(u => u.MailId == userMailId);
            //var users = await _userDbHandler.GetAllUserAsync().ConfigureAwait(false);
            if (user == null) return null;
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
                Place = user.Place,
                TextPosts = textPosts,
                PollPosts = pollPosts,
                FollowersId = followersId,
                FollowingsId = followingsId
            };
            foreach (var textPost in userBObj.TextPosts)
            {
                foreach (var comment in textPost.Comments)
                {
                    comment.CommentedUserName = userBObj.UserName;
                }
            }
            foreach (var pollPost in userBObj.PollPosts)
            {
                foreach (var comment in pollPost.Comments)
                {
                    comment.CommentedUserName = userBObj.UserName;
                }
            }
            return userBObj;
        }
    }
}
