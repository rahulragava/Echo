using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed class ReactionDbHandler : IReactionDbHandler
    {

        private static ReactionDbHandler Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;

        ReactionDbHandler() { }

        public static ReactionDbHandler GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new ReactionDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert reaction to db
        public async Task InsertReactionAsync(Reaction reaction)
        {
            await _dbAdapter.InsertInTableAsync(reaction);
        }

        //remove a Reaction from db
        public async Task RemoveReactionAsync(string reactionId)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<Reaction>(reactionId);
        }

        //edit a reaction field or fields
        public async Task UpdateUserReactionAsync(Reaction reaction)
        {
            await _dbAdapter.UpdateObjectInTableAsync(reaction);

        }

        //get all reactions
        public async Task<IEnumerable<Reaction>> GetAllReactionAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<Reaction>();
        }

        //get a particular reaction
        public async Task<Reaction> GetReactionAsync(string id)
        {
            return await _dbAdapter.GetObjectFromTableAsync<Reaction>(id);
        }

        public async Task<IEnumerable<Reaction>> GetReactionsAsync(string reactionOnId)
        {
            return (await GetAllReactionAsync()).Where(c => c.ReactionOnId == reactionOnId);
        }
    }
}
