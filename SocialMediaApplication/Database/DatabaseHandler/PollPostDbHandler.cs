using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed  class PollPostDbHandler : IPollPostDbHandler
    {

        private static PollPostDbHandler Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;


        PollPostDbHandler() { }

        public static PollPostDbHandler GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new PollPostDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert poll post to db
        public async Task InsertPollPostAsync(PollPost pollPost)
        {
            await DbAdapter.GetInstance.InsertInTableAsync(pollPost);
        }


        //remove a poll post from db
        public async Task RemovePollPostAsync(string pollPostId)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<PollPost>(pollPostId);
        }

        //edit a poll post field or fields
        public async Task UpdatePollPostAsync(PollPost pollPost)
        {
            await _dbAdapter.UpdateObjectInTableAsync(pollPost);
        }

        //get all pollposts
        public async Task<IEnumerable<PollPost>> GetAllPollPostAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<PollPost>();
        }

        //get a particular textpost
        public async Task<PollPost> GetPollPostAsync(string id)
        {
            return await _dbAdapter.GetObjectFromTableAsync<PollPost>(id);
        }

        public async Task<IEnumerable<PollPost>> GetSpecificPostAsync(int takeAmount, int skipAmount)
        {
            return (await _dbAdapter.GetSpecificObjectsInTableAsync<PollPost>(takeAmount, skipAmount)).ToList();
        }
    }
}
