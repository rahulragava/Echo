using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;

namespace SocialMediaApplication.Domain.UseCase
{
    
    public class PollPostCreationUseCase : UseCaseBase<PollPostCreationResponse>
    {
        private readonly ICreatePostManager _postManager = CreatePostManager.GetInstance;
        public readonly PollPostCreationRequest PollPostCreationRequest;

        public PollPostCreationUseCase(PollPostCreationRequest pollPostCreationRequest, IPresenterCallBack<PollPostCreationResponse> pollPostCreationPresenterCallBack) : base(pollPostCreationPresenterCallBack)
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

        public PollPostCreationRequest(PollPost pollPost, List<PollChoice> choices)
        {
            PollPost = pollPost;
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
            _pollPostCreationUseCase?.PresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _pollPostCreationUseCase?.PresenterCallBack?.OnError(ex);
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
