using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Models.EntityModels
{
    public class Comment
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string CommentedBy { get; set; }
        public DateTime CommentedAt { get; set; }
        public string Content { get; set; }
        public string PostId { get; set; }
        public string ParentCommentId { get; set; }

        public Comment()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
