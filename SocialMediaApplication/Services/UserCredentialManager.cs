using SocialMediaApplication.Models.EntityModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;

namespace SocialMediaApplication.Services
{
    public sealed class UserCredentialManager
    {
        private static UserCredentialManager Instance { get; set; }
        private static readonly object PadLock = new object();

        UserCredentialManager() { }

        public static UserCredentialManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new UserCredentialManager();
                        }
                    }
                }
                return Instance;
            }
        }

        IUserCredentialDbHandler _userCredentialDAO = UserCredentialDbHandler.GetInstance;

        public async Task<List<UserCredential>> GetUserCredentialsAsync() => (await Task.Run(() => _userCredentialDAO.GetAllUserCredentialAsync()).ConfigureAwait(false)).ToList();

        public async Task AddUserCredentialAsync(UserCredential userCredential)
        {
            if(userCredential == null)
            {
                return;
            }
            await Task.Run(() => _userCredentialDAO.InsertUserCredentialAsync(userCredential)).ConfigureAwait(false);
        }

        public async Task RemoveUserCredential(UserCredential userCredential)
        {
            if(userCredential == null)
            {
                return;
            }
            await Task.Run(() => _userCredentialDAO.RemoveUserCredentialAsync(userCredential.UserId));
        }

        public async Task<UserCredential> GetUserCredential(string userId) => (await Task.Run(() => _userCredentialDAO.GetUserCredentialAsync(userId)).ConfigureAwait(false));
    }
}
