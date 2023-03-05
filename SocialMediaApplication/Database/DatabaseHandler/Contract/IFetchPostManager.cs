using SocialMediaApplication.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface IFetchPostManager
    {
        Task<List<PollPostBObj>> GetUserPollPostBObjsAsync(string userId);
        Task<List<TextPostBObj>> GetUserTextPostBObjsAsync(string userId);
    }
}
