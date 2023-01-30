using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed class UserPollChoiceSelectionDbHandler : IUserPollChoiceSelectionDbHandler
    {
        private static UserPollChoiceSelectionDbHandler Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;

        UserPollChoiceSelectionDbHandler() { }

        public static UserPollChoiceSelectionDbHandler GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new UserPollChoiceSelectionDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert poll choice selection to db
        public async Task InsertUserPollChoiceSelectionAsync(UserPollChoiceSelection userPollChoiceSelection)
        {
            await _dbAdapter.InsertInTableAsync(userPollChoiceSelection);
        }

        public async Task InsertUserPollChoiceSelectionsAsync(List<UserPollChoiceSelection> userPollChoiceSelections)
        {
            await _dbAdapter.InsertMultipleObjectInTableAsync(userPollChoiceSelections);
        }

        //remove a user pollChoice selection from db
        public async Task RemoveUserPollChoiceSelectionAsync(string userPollChoiceSelectionId)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<UserPollChoiceSelection>(userPollChoiceSelectionId);
        }

        public async Task RemoveUserPollChoiceSelectionsAsync(List<UserPollChoiceSelection> userPollChoiceSelections)
        {
            foreach (var userPollChoiceSelection in userPollChoiceSelections)
            {
                await RemoveUserPollChoiceSelectionAsync(userPollChoiceSelection.Id);
            }
        }

        //edit a user pollChoice selection field or fields
        public async Task UpdateUserPollChoiceSelectionAsync(UserPollChoiceSelection userPollChoiceSelection)
        {
            await _dbAdapter.UpdateObjectInTableAsync(userPollChoiceSelection);
        }

        //get all user pollChoice selection
        public async Task<IEnumerable<UserPollChoiceSelection>> GetAllUserPollChoiceSelectionAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<UserPollChoiceSelection>();
        }

        //get a particular UserpollChoiceSelection
        public async Task<UserPollChoiceSelection> GetUserPollChoiceSelectionAsync(string userPollChoiceId)
        {
            return await _dbAdapter.GetObjectFromTableAsync<UserPollChoiceSelection>(userPollChoiceId);
        }

        
    }
}
