using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;

namespace SocialMediaApplication.Domain.UseCase
{
    public class GetCommentUseCase : UseCaseBase<GetCommentResponse>
    {
        private readonly IGetCommentManager _getCommentManager= GetCommentManager.GetInstance;
        public readonly GetCommentRequest GetCommentRequest;

        public GetCommentUseCase(GetCommentRequest getCommentRequest, IPresenterCallBack<GetCommentResponse> getCommentPresenterCallBack) : base(getCommentPresenterCallBack)
        {
            GetCommentRequest = getCommentRequest;
        }

        public override void Action()
        {
            _getCommentManager.GetPostCommentsAsync(GetCommentRequest, new GetCommentUseCaseCallBack(this));

        }
    }


    public class GetCommentUseCaseCallBack : IUseCaseCallBack<GetCommentResponse>
    {
        private readonly GetCommentUseCase _getCommentUseCase;

        public GetCommentUseCaseCallBack(GetCommentUseCase getCommentUseCase)
        {
            _getCommentUseCase = getCommentUseCase;
        }

        public void OnSuccess(GetCommentResponse responseObj)
        {
            _getCommentUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _getCommentUseCase?.PresenterCallBack?.OnError(ex);
        }
    }


    public class GetCommentRequest
    {
        public GetCommentRequest(string postId)
        {
            PostId = postId;
        }

        public string PostId { get; }

    }


    public class GetCommentResponse
    {
        public List<CommentBObj> Comments;

        public GetCommentResponse(List<CommentBObj> comments)
        {
            Comments = comments;
        }
    }
}
