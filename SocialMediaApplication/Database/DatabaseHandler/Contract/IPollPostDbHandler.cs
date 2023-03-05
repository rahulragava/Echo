using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface IPollPostDbHandler
    {
        Task InsertPollPostAsync(PollPost pollPost);
        Task UpdatePollPostAsync(PollPost pollPost);
        Task<PollPost> GetPollPostAsync(string pollPostId);
        Task<IEnumerable<PollPost>> GetAllPollPostAsync();
        Task RemovePollPostAsync(string pollPostId);
        Task<IEnumerable<PollPost>> GetSpecificPostAsync(int takAmount, int skipAmount);
    }
}
