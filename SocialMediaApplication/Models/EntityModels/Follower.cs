using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Models.EntityModels
{
    public class Follower
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string FollowerId { get; set; }
        public string FollowingId { get; set; }

        public Follower()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
