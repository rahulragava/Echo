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
        //private  List<Reaction> reactions = new List<Reaction>();
        
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
            var reactionList = await _reactionDbHandler.GetAllReactionAsync();
            return reactionList.ToList();
        }

        public async Task AddReactionAsync(ReactionToPostRequestObj reactionToPostRequestObj,
            ReactionToPostUseCaseCallBack reactionToPostUseCaseCallBack)
        {
            try
            {
                var reactions = await _reactionDbHandler.GetReactionsAsync(reactionToPostRequestObj.Reaction.ReactionOnId);
                var reaction = reactions.SingleOrDefault(r => r.ReactedBy == reactionToPostRequestObj.Reaction.ReactedBy);

                if (reaction != null)
                {
                    if (reaction.ReactionType == reactionToPostRequestObj.Reaction.ReactionType)
                    {
                        //remove
                        await _reactionDbHandler.RemoveReactionAsync(reaction.Id);
                    }
                    else
                    {
                        //remove and insert
                        await _reactionDbHandler.RemoveReactionAsync(reaction.Id);
                        await _reactionDbHandler.InsertReactionAsync(reactionToPostRequestObj.Reaction);
                    }

                }
                else
                {
                    //insert
                    await _reactionDbHandler.InsertReactionAsync(reactionToPostRequestObj.Reaction);
                }
                reactionToPostUseCaseCallBack?.OnSuccess(new ReactionToPostResponse(true));
            }
            catch (Exception e)
            {
                reactionToPostUseCaseCallBack?.OnError(e);
            }
        }

        public async Task GetUserReaction(GetUserReactionRequest getUserReactionRequest, GetUserReactionUseCaseCallBack getUserReactionUseCallBack)
        {
            try
            {
                var reactions = await _reactionDbHandler.GetReactionsAsync(getUserReactionRequest.ReactionOnId);
                var reaction =  reactions.SingleOrDefault(r => r.ReactedBy == getUserReactionRequest.UserId);

                getUserReactionUseCallBack?.OnSuccess(new GetUserReactionResponse(reaction));
            }
            catch (Exception e)
            {
                getUserReactionUseCallBack?.OnError(e);
            }
        }
        //public async Task AddReactionAsync(Reaction reaction)
        //{
        //    await _reactionDbHandler.InsertReactionAsync(reaction);
        //}

        //public async Task RemoveReactionAsync(string reactionId)
        //{
        //    try
        //    {
        //        await _reactionDbHandler.RemoveReactionAsync(reactionId);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}

        //public async Task RemoveReactionsAsync(List<Reaction> reactions)
        //{
        //    if (reactions.Count <= 0 || !(reactions.Any())) return;

        //    while (true)
        //    {
        //        for (int i = 0; i < reactions.Count; i++)
        //        {
        //            await RemoveReactionAsync(reactions[i].Id).ConfigureAwait(false);
        //            break;
        //        }
        //        if (reactions.Count == 0) break;
        //    }
        //}

        
    }
}
