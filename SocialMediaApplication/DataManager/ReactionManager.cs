using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
{
    public sealed class ReactionManager
    {
        private static ReactionManager Instance { get; set; }
        readonly IReactionDbHandler _reactionDbHandler = ReactionDbHandler.GetInstance;
        private static readonly object PadLock = new object();

        ReactionManager() { }

        public static ReactionManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new ReactionManager();
                        }
                    }
                }
                return Instance;
            }
        }

        public async Task<List<Reaction>> GetReactionAsync()
        {
            var reactions = await _reactionDbHandler.GetAllUserReactionAsync();
            return reactions.ToList();
        }

        public async Task AddReactionAsync(Reaction reaction)
        {
            await _reactionDbHandler.InsertUserReactionAsync(reaction);
        }

        public async Task RemoveReactionAsync(Reaction reaction)
        {
            await _reactionDbHandler.RemoveUserReactionAsync(reaction.Id);
        }

        public async Task RemoveReactionsAsync(List<Reaction> reactions)
        {
            if (reactions.Count <= 0 || !(reactions.Any())) return;

            while (true)
            {
                for (int i = 0; i < reactions.Count; i++)
                {
                    await RemoveReactionAsync(reactions[i]).ConfigureAwait(false);
                    break;
                }
                if (reactions.Count == 0) break;
            }
        }
    }
}
