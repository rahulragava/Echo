using SocialMediaApplication.Models.EntityModels;
using System.Collections.Generic;

namespace SocialMediaApplication.Models.BusinessModels
{
    public class UserBObj : User
    {
        public List<PollPostBObj> PollPosts { get; set; }
        public List<TextPostBObj> TextPosts { get; set; }
        //no need to have other user info, so having id of followers/followings is enough
        public List<string> FollowersId { get; set; }
        public List<string> FollowingsId { get; set; }
    }
}
