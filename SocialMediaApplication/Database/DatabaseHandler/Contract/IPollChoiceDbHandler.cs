using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface IPollChoiceDbHandler
    {
        Task InsertPollChoiceAsync(PollChoice pollChoice);
        Task InsertPollChoicesAsync(List<PollChoice> pollChoices);
        Task UpdatePollChoiceAsync(PollChoice pollChoice);
        Task<PollChoice> GetPollChoiceAsync(string pollchoiceId);
        Task<IEnumerable<PollChoice>> GetAllPollChoiceAsync();
        Task RemovePollChoiceAsync(string pollChoiceId);
    }
}
