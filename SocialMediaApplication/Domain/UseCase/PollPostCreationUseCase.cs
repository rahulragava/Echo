using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Domain.UseCase
{
    
    public class PollPostCreationUseCase : UseCaseBase<PollPostCreationResponse>
    {
        private readonly ICreatePostManager _postManager = CreatePostManager.GetInstance;
        public readonly PollPostCreationRequest PollPostCreationRequest;

        public PollPostCreationUseCase(PollPostCreationRequest pollPostCreationRequest)
        {
            PollPostCreationRequest = pollPostCreationRequest;
        }

        public override void Action()
        {
            _postManager.CreatePollPostAsync(PollPostCreationRequest,new PollPostCreationUseCaseCallBack(this));
        }
    }

    //req obj
    public class PollPostCreationRequest
    {
        public PollPost PollPost;
        public List<PollChoice> Choices;
        public IPresenterCallBack<PollPostCreationResponse> PollPostCreationPresenterCallBack;

        public PollPostCreationRequest(PollPost pollPost, List<PollChoice> choices, IPresenterCallBack<PollPostCreationResponse> pollPostCreationPresenterCallBack)
        {
            PollPost = pollPost;
            PollPostCreationPresenterCallBack = pollPostCreationPresenterCallBack;
            Choices = choices;
        }
    }



    public class PollPostCreationUseCaseCallBack : IUseCaseCallBack<PollPostCreationResponse>
    {
        private readonly PollPostCreationUseCase _pollPostCreationUseCase;

        public PollPostCreationUseCaseCallBack(PollPostCreationUseCase pollPostCreationUseCase)
        {
            _pollPostCreationUseCase = pollPostCreationUseCase;
        }

        public void OnSuccess(PollPostCreationResponse responseObj)
        {
            _pollPostCreationUseCase?.PollPostCreationRequest.PollPostCreationPresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _pollPostCreationUseCase?.PollPostCreationRequest.PollPostCreationPresenterCallBack?.OnError(ex);
        }
    }

    public class PollPostCreationResponse
    {
        public bool Success;

        public PollPostCreationResponse(bool success)
        {
            Success = success;
        }
    }
}
