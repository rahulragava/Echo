using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler.Contract
{
    public interface IUserCredentialDbHandler
    {
        Task InsertUserCredentialAsync(UserCredential userCredential);
        Task UpdateUserCredentialAsync(UserCredential userCredential);
        Task<UserCredential> GetUserCredentialAsync(string userCredentialId);
        Task<IEnumerable<UserCredential>> GetAllUserCredentialAsync();
        Task RemoveUserCredentialAsync(string userCredentialId);
    }
}
