using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;
using Windows.System;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed  class UserCredentialDbHandler : IUserCredentialDbHandler
    {

        private static UserCredentialDbHandler Instance { get; set; }
        private static object _padLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;

        UserCredentialDbHandler() { }

        public static UserCredentialDbHandler GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (_padLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new UserCredentialDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert userCredential to db
        public async Task InsertUserCredentialAsync(UserCredential userCredential)
        {
            await _dbAdapter.InsertInTableAsync(userCredential);
        }

        //remove a user credential from db
        public async Task RemoveUserCredentialAsync(string userCredentialId)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<UserCredential>(userCredentialId);
        }

        //edit a user credential field or fields
        public async Task UpdateUserCredentialAsync(UserCredential userCredential)
        {
            await _dbAdapter.UpdateObjectInTableAsync(userCredential);
        }

        //get all user credentials
        public async Task<IEnumerable<UserCredential>> GetAllUserCredentialAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<UserCredential>();
        }

        //get a particular user credential
        public async Task<UserCredential> GetUserCredentialAsync(string id)
        {
            return await _dbAdapter.GetObjectFromTableAsync<UserCredential>(id);
        }
    }
}
