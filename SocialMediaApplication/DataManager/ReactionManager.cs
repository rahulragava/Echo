using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
{
    public sealed class ReactionManager : IReactionManager
    {
        private static ReactionManager Instance { get; set; }
        readonly IReactionDbHandler _reactionDbHandler = ReactionDbHandler.GetInstance;
        private static readonly object PadLock = new object();

        ReactionManager() { }
        private  List<Reaction> reactions = new List<Reaction>();
        
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
            var reactionList = await _reactionDbHandler.GetAllUserReactionAsync();
            return reactionList.ToList();
        }

        public async Task AddReactionAsync(ReactionToPostRequestObj reactionToPostRequestObj,
            ReactionToPostUseCaseCallBack reactionToPostUseCaseCallBack)
        {
            try
            {
                
                if (reactions == null)
                {
                    reactions = await GetReactionAsync();
                }
                var reaction = reactions.SingleOrDefault(r =>
                    r.ReactedBy == App.UserId && r.ReactionOnId == reactionToPostRequestObj.ReactionOnId && r.ReactionType == reactionToPostRequestObj.ReactionType);
                if (reactionToPostRequestObj.Reaction is null &&
                    reactionToPostRequestObj.ReactionType == ReactionType.None)
                {
                    //var reaction = reactions.SingleOrDefault(r =>
                    //    r.ReactedBy == App.UserId && r.ReactionOnId == reactionToPostRequestObj.ReactionOnId && r.ReactionType == reactionToPostRequestObj.ReactionType);
                    if (reaction != null)
                    {
                        await RemoveReactionAsync(reaction.Id);
                        reactions.Remove(reaction);
                    }
                }
                else if(reactionToPostRequestObj.ReactionType == ReactionType.None)
                {
                    if (reactionToPostRequestObj.Reaction != null)
                    {
                        await _reactionDbHandler.InsertUserReactionAsync(reactionToPostRequestObj.Reaction);
                        reactions.Add(reactionToPostRequestObj.Reaction);
                    }
                }
                else
                {
                    if (reactionToPostRequestObj.Reaction != null)
                    {
                        if (reaction != null)
                        {
                            await RemoveReactionAsync(reaction.Id);
                            reactions.Remove(reaction);
                            await _reactionDbHandler.InsertUserReactionAsync(reactionToPostRequestObj.Reaction);
                            reactions.Add(reactionToPostRequestObj.Reaction);
                        }
                    }
                }
                    //if (reactionToPostRequestObj.Reaction == null)
                        //{
                        //    var reaction = reactions.SingleOrDefault(r =>
                        //        r.ReactedBy == App.UserId && r.ReactionOnId == reactionToPostRequestObj.ReactionOnId);

                        //}

                        //logic error 
                    //if (reactions.Exists(r => r.Id == reactionToPostRequestObj.Reaction.Id && r.ReactionType != reactionToPostRequestObj.Reaction.ReactionType))
                    //{
                    //    if (reactionToPostRequestObj.Reaction != null)
                    //    {
                    //        await RemoveReactionAsync(reactionToPostRequestObj.Reaction.Id);
                    //        reactions.Remove(reactionToPostRequestObj.Reaction);
                    //        await _reactionDbHandler.InsertUserReactionAsync(reactionToPostRequestObj.Reaction);
                    //        reactions.Add(reactionToPostRequestObj.Reaction);
                    //    }
                    //}
                    //else
                    //{
                    //    if (reactionToPostRequestObj.Reaction != null)
                    //    {
                    //        await _reactionDbHandler.InsertUserReactionAsync(reactionToPostRequestObj.Reaction);
                    //        reactions.Add(reactionToPostRequestObj.Reaction);
                    //    }

                    //}
                reactionToPostUseCaseCallBack?.OnSuccess(new ReactionToPostResponse(reactions));


            }
            catch (Exception e)
            {
                reactionToPostUseCaseCallBack?.OnError(e);
            }
        }

        public async Task AddReactionAsync(Reaction reaction)
        {
            await _reactionDbHandler.InsertUserReactionAsync(reaction);
        }

        public async Task RemoveReactionAsync(string reactionId)
        {
            try
            {
                await _reactionDbHandler.RemoveUserReactionAsync(reactionId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task RemoveReactionsAsync(List<Reaction> reactions)
        {
            if (reactions.Count <= 0 || !(reactions.Any())) return;

            while (true)
            {
                for (int i = 0; i < reactions.Count; i++)
                {
                    await RemoveReactionAsync(reactions[i].Id).ConfigureAwait(false);
                    break;
                }
                if (reactions.Count == 0) break;
            }
        }

        
    }
}
