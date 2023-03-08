using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface IReactionDbHandler
    {
        Task InsertReactionAsync(Reaction reaction);
        Task UpdateUserReactionAsync(Reaction reaction);
        Task<Reaction> GetReactionAsync(string reactionId);
        Task<IEnumerable<Reaction>> GetAllReactionAsync();
        Task RemoveReactionAsync(string reactionId);
        Task<IEnumerable<Reaction>> GetReactionsAsync(string reactionOnId);
    }
}
