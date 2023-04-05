using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Domain.UseCase
{
    public class ReactionToPostUseCase : UseCaseBase<ReactionToPostResponse>
    {
        public IReactionManager ReactionManager = DataManager.ReactionManager.GetInstance;
        public readonly ReactionToPostRequestObj ReactionToPostRequestObj;

        public ReactionToPostUseCase(ReactionToPostRequestObj reactionToPostRequestObj, IPresenterCallBack<ReactionToPostResponse> reactionToPostResponsePresenterCallBack) :base(reactionToPostResponsePresenterCallBack)
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
            _reactionToPostUseCase?.PresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _reactionToPostUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    //Request obj
    public class ReactionToPostRequestObj
    {
        public Reaction Reaction { get; }

        public ReactionToPostRequestObj(Reaction reaction)
        {
            Reaction = reaction;
        }
    }

    //response object
    public class ReactionToPostResponse
    {
        public bool ReactionSuccess;

        public ReactionToPostResponse(bool reactionSuccess)
        {
            ReactionSuccess = reactionSuccess;
        }
    }
}
