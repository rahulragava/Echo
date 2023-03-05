using SocialMediaApplication.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Models.BusinessModels
{
    public abstract class PostBObj:Post
    {
        public string UserName { get;set; }
        public string FormattedCreatedTime { get; set; } 
        public List<CommentBObj> Comments { get; set; }
        public List<Reaction> Reactions { get; set; }

        
    }
}
