using SocialMediaApplication.Models.EntityModels;
using System.Collections.Generic;

namespace SocialMediaApplication.Models.BusinessModels
{
    public class UserBObj : User
    {
        public string FormattedCreatedTime { get; set; }
        public List<PollPostBObj> PollPosts { get; set; }
        public List<TextPostBObj> TextPosts { get; set; }
        public List<string> FollowersId { get; set; }
        public List<string> FollowingsId { get; set; }
    }
}
