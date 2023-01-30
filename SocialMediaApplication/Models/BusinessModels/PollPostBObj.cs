using SocialMediaApplication.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Models.BusinessModels
{
    public class PollPostBObj : PostBObj
    {
        public string Question { get; set; }
        public List<PollChoiceBObj> Choices { get; set; }
    }
}
