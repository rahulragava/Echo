using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface IUserPollChoiceSelectionDbHandler
    {
        Task InsertUserPollChoiceSelectionAsync(UserPollChoiceSelection userPollChoiceSelection);
        Task UpdateUserPollChoiceSelectionAsync(UserPollChoiceSelection userPollChoiceSelection);
        Task<UserPollChoiceSelection> GetUserPollChoiceSelectionAsync(string id);
        Task InsertUserPollChoiceSelectionsAsync(List<UserPollChoiceSelection> userPollChoiceSelections);
        Task<IEnumerable<UserPollChoiceSelection>> GetAllUserPollChoiceSelectionAsync();
        Task<IEnumerable<UserPollChoiceSelection>> GetSelectiveUserPollChoicesSelectionAsync(string choiceId);
        
        Task RemoveUserPollChoiceSelectionAsync(string userPollChoiceSelectionId);
        Task RemoveUserPollChoiceSelectionsAsync(List<UserPollChoiceSelection> userPollChoiceSelections);

    }
}
