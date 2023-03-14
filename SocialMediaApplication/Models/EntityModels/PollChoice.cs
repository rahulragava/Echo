using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Models.Constant;

namespace SocialMediaApplication.Models.EntityModels
{
    public class PollChoice
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string PostId { get; set; }
        public string Choice { get; set; }
        public PostFontStyle PostFontStyle { get; set; }

        public PollChoice()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
