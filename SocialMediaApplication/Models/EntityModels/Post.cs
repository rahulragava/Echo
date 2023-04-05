using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Models.Constant;

namespace SocialMediaApplication.Models.EntityModels
{
    public abstract class Post
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string PostedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public PostFontStyle FontStyle { get; set; }
        public string Title { get; set; }

        protected Post()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
