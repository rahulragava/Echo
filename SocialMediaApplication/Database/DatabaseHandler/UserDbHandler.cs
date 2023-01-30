using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed class UserDbHandler : IUserDbHandler
    {
        private static UserDbHandler Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;

        UserDbHandler() { }

        public static UserDbHandler GetInstance
        {
            get
            {
                if(Instance == null)
                {
                    lock (PadLock)
                    {
                        if(Instance == null)
                        {
                            Instance = new UserDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert user to db
        public async Task InsertUserAsync(User user)
        {
            await _dbAdapter.InsertInTableAsync(user);
        }

        //remove a user from db
        public async Task RemoveUserAsync(string id)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<User>(id);

        }

        //edit a user field or fields
        public async Task UpdateUserAsync(User user)
        {
            await _dbAdapter.UpdateObjectInTableAsync(user);
        }

        //get all user
        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<User>();
        }
        
        //get a particular user
        public async Task<User> GetUserAsync(string id)
        {
            return await _dbAdapter.GetObjectFromTableAsync<User>(id);
        }
    
    }
}