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
        private readonly IReactionDbHandler _reactionDbHandler = ReactionDbHandler.GetInstance;
        private static readonly object PadLock = new object();

        private ReactionManager() { }
        
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

        public async Task GetUserReactionAsync(GetUserReactionRequest getUserReactionRequest, GetUserReactionUseCaseCallBack getUserReactionUseCallBack)
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
    }
}
