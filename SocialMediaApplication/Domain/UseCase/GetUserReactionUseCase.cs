using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Domain.UseCase
{
    public class GetUserReactionUseCase : UseCaseBase<GetUserReactionResponse>
    {
        public readonly IReactionManager ReactionManager = DataManager.ReactionManager.GetInstance;
        public readonly GetUserReactionRequest GetUserReactionRequest;
        public GetUserReactionUseCase(GetUserReactionRequest getUserReactionRequest, IPresenterCallBack<GetUserReactionResponse> getUserReactionPresenterCallBack) : base(getUserReactionPresenterCallBack)
        {
            GetUserReactionRequest = getUserReactionRequest;
        }

        public override void Action()
        {
            ReactionManager.GetUserReactionAsync(GetUserReactionRequest, new GetUserReactionUseCaseCallBack(this));
        }
    }


    public class GetUserReactionUseCaseCallBack : IUseCaseCallBack<GetUserReactionResponse>
    {
        private readonly GetUserReactionUseCase _getUserReactionUseCase;

        public GetUserReactionUseCaseCallBack(GetUserReactionUseCase getUserReactionUseCase)
        {
            this._getUserReactionUseCase = getUserReactionUseCase;
        }

        public void OnSuccess(GetUserReactionResponse responseObj)
        {
            _getUserReactionUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _getUserReactionUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    //Request obj
    public class GetUserReactionRequest
    {
        public GetUserReactionRequest(string userId, string reactionOnId)
        {
            UserId = userId;
            ReactionOnId = reactionOnId;
        }

        public string UserId { get; }
        public string ReactionOnId { get; }
    }

    //response object
    public class GetUserReactionResponse
    {
        public GetUserReactionResponse(Reaction reaction)
        {
            Reaction = reaction;
        }

        public Reaction Reaction { get; }
    }
}
