using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed class PollChoiceDbHandler : IPollChoiceDbHandler
    {

        private static PollChoiceDbHandler Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;


        PollChoiceDbHandler() { }

        public static PollChoiceDbHandler GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new PollChoiceDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert poll choice to db
        public async Task InsertPollChoiceAsync(PollChoice pollChoice)
        {
            await _dbAdapter.InsertInTableAsync(pollChoice);
        }

        //remove a pollChoice from db
        public async Task RemovePollChoiceAsync(string pollChoiceId)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<PollChoice>(pollChoiceId);
        }

        //edit a pollChoice field or fields
        public async Task UpdatePollChoiceAsync(PollChoice pollChoice)
        {
            await _dbAdapter.UpdateObjectInTableAsync(pollChoice);
        }

        //get all pollChoice
        public async Task<IEnumerable<PollChoice>> GetAllPollChoiceAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<PollChoice>();
        }

        //get a particular pollChoice
        public async Task<PollChoice> GetPollChoiceAsync(string pollChoiceid)
        {
            return await _dbAdapter.GetObjectFromTableAsync<PollChoice>(pollChoiceid);
        }

        //inser given list of pollchoice to db
        public async Task InsertPollChoicesAsync(List<PollChoice> pollChoices)
        {
            await _dbAdapter.InsertMultipleObjectInTableAsync(pollChoices);
        }

        public async Task<IEnumerable<PollChoice>> GetPostPollChoicesAsync(string postId)
        {
            return (await GetAllPollChoiceAsync()).Where(p => p.PostId== postId);
        }
    }
}
