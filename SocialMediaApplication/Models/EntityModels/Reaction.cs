using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Models.Constant;

namespace SocialMediaApplication.Models.EntityModels
{
    public class Reaction
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string ReactedBy { get; set; }
        public string ReactionOnId { get; set; }
        public ReactionType reactionType { get; set; }

        public Reaction()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
