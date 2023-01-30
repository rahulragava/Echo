using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface IFollowerDbHandler
    {
        Task InsertFollowerAsync(Follower follower);
        Task UpdateFollowerAsync(Follower follower);
        Task<Follower> GetFollowerAsync(string viewingUserId, string searchedUserId);
        Task<IEnumerable<string>> GetUserFollowerIdsAsync(string userId);
        Task<IEnumerable<string>> GetUserFollowingIdsAsync(string userId);
        Task<IEnumerable<Follower>> GetAllFollowerAsync();
        Task RemoveFollowerAsync(string followerId);
    }
}
