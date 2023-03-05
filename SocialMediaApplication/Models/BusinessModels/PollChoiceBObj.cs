using SocialMediaApplication.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Models.BusinessModels
{
    public class PollChoiceBObj : PollChoice
    {
        public List<UserPollChoiceSelection> ChoiceSelectedUsers { get; set; }
    }
}
