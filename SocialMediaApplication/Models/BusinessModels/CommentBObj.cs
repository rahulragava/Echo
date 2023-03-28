using SocialMediaApplication.Models.EntityModels;
using System.Collections.Generic;

namespace SocialMediaApplication.Models.BusinessModels
{
    public class CommentBObj : Comment
    {
        public int Depth { get; set; }
        public string FormattedCommentDate { get; set; }
        public string CommentedUserName { get; set; }
        public List<Reaction> Reactions { get; set; }
    }
}
