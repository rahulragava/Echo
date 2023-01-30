using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface IUserDbHandler
    {
        Task InsertUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User> GetUserAsync(string id);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task RemoveUserAsync(string userId);
    }
}
