using SocialMediaApplication.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;

namespace SocialMediaApplication.Services
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
            var reactions = await Task.Run(() => _reactionDbHandler.GetAllUserReactionAsync()).ConfigureAwait(false);
            return reactions.ToList();
        }

        public async Task AddReactionAsync(Reaction reaction)
        {
            await Task.Run(()=> _reactionDbHandler.InsertUserReactionAsync(reaction)).ConfigureAwait(false);
        }

        public async Task RemoveReactionAsync(Reaction reaction)
        {
            await Task.Run(()=> _reactionDbHandler.RemoveUserReactionAsync(reaction.Id)).ConfigureAwait(false);
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
