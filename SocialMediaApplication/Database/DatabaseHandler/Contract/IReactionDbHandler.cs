using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface IReactionDbHandler
    {
        Task InsertUserReactionAsync(Reaction reaction);
        Task UpdateUserReactionAsync(Reaction reaction);
        Task<Reaction> GetUserReactionAsync(string reactionId);
        Task<IEnumerable<Reaction>> GetAllUserReactionAsync();
        Task RemoveUserReactionAsync(string reactionId);
    }
}
