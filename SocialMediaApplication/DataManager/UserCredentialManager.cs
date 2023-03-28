using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
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

        private readonly IUserCredentialDbHandler _userCredentialDbHandler = UserCredentialDbHandler.GetInstance;

        public async Task<List<UserCredential>> GetUserCredentialsAsync() => (await _userCredentialDbHandler.GetAllUserCredentialAsync()).ToList();

        public async Task AddUserCredentialAsync(UserCredential userCredential)
        {
            if(userCredential == null)
            {
                return;
            }
            await _userCredentialDbHandler.InsertUserCredentialAsync(userCredential);
        }

        
        public async Task<UserCredential> GetUserCredentialAsync(string userId) => (await _userCredentialDbHandler.GetUserCredentialAsync(userId));
    }
}
