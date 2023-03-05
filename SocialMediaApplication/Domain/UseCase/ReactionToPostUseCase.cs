using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Domain.UseCase
{
    public class ReactionToPostUseCase : UseCaseBase<ReactionToPostResponse>
    {
        public IReactionManager ReactionManager = DataManager.ReactionManager.GetInstance;
        public readonly ReactionToPostRequestObj ReactionToPostRequestObj;

        public ReactionToPostUseCase(ReactionToPostRequestObj reactionToPostRequestObj)
        {
            ReactionToPostRequestObj = reactionToPostRequestObj;
        }
        public override void Action()
        {
            
            ReactionManager.AddReactionAsync(ReactionToPostRequestObj, new ReactionToPostUseCaseCallBack(this));
        }
        
    }
    public class ReactionToPostUseCaseCallBack : IUseCaseCallBack<ReactionToPostResponse>
    {
        private readonly ReactionToPostUseCase _reactionToPostUseCase;
        public ReactionToPostUseCaseCallBack(ReactionToPostUseCase reactionToPostUseCase)
        {
            _reactionToPostUseCase = reactionToPostUseCase;
        }
        public void OnSuccess(ReactionToPostResponse responseObj)
        {
            _reactionToPostUseCase?.ReactionToPostRequestObj.ReactionToPostResponsePresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _reactionToPostUseCase?.ReactionToPostRequestObj.ReactionToPostResponsePresenterCallBack?.OnError(ex);
        }
    }

    //Request obj
    
    public class ReactionToPostRequestObj
    {
        public Reaction Reaction { get; }
        public string ReactionOnId { get; }
        public ReactionType ReactionType { get; }
        public IPresenterCallBack<ReactionToPostResponse> ReactionToPostResponsePresenterCallBack { get; }


        public ReactionToPostRequestObj(ReactionType reactionType, Reaction reaction, string reactionOnId, IPresenterCallBack<ReactionToPostResponse> reactionToPostResponsePresenterCallBack)
        {
            ReactionType = reactionType;
            Reaction = reaction;
            ReactionToPostResponsePresenterCallBack = reactionToPostResponsePresenterCallBack;
            ReactionOnId = reactionOnId;
        }
    }

    //response object
 
    public class ReactionToPostResponse
    {
        public List<Reaction> Reactions;

        public ReactionToPostResponse(List<Reaction> reactions)
        {
            Reactions = reactions;
        }
    }
}
