using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Domain.UseCase
{
    public class GetCommentUseCase : UseCaseBase<GetCommentResponse>
    {
        private readonly IGetCommentManager _getCommentManager= GetCommentManager.GetInstance;
        public readonly GetCommentRequest GetCommentRequest;

        public GetCommentUseCase(GetCommentRequest getCommentRequest)
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
            _getCommentUseCase?.GetCommentRequest.GetCommentPresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _getCommentUseCase?.GetCommentRequest.GetCommentPresenterCallBack?.OnError(ex);
        }
    }


    public class GetCommentRequest
    {
        public GetCommentRequest(string postId,  IPresenterCallBack<GetCommentResponse> getCommentPresenterCallBack)
        {
            PostId = postId;
            GetCommentPresenterCallBack = getCommentPresenterCallBack;
        }

        public string PostId { get; }
        public IPresenterCallBack<GetCommentResponse> GetCommentPresenterCallBack { get; }

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
