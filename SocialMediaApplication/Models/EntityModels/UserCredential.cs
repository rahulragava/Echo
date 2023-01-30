using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Models.EntityModels
{
    public class UserCredential
    {
        [PrimaryKey]
        public string UserId { get; set; }

        public string Password { get; set; }

        public UserCredential() {}
        public UserCredential(string userId, string password)
        {
            UserId = userId;
            Password = password;
        }
    }
}
