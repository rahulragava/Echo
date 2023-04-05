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
    public class RemoveCommentUseCase : UseCaseBase<RemoveCommentResponse>
    {
        private readonly IRemoveCommentManager _removeCommentManager = RemoveCommentManager.GetInstance;
        public readonly RemoveCommentRequest RemoveCommentRequest;

        public RemoveCommentUseCase(RemoveCommentRequest removeCommentRequest, IPresenterCallBack<RemoveCommentResponse> removeCommentPresenterCallBack) : base(removeCommentPresenterCallBack)
        {
            RemoveCommentRequest = removeCommentRequest;
        }

        public override void Action()
        {
            _removeCommentManager.RemoveCommentAsync(RemoveCommentRequest, new RemoveCommentUseCaseCallBack(this));
        }

    }

    //useCase call back
    public class RemoveCommentUseCaseCallBack : IUseCaseCallBack<RemoveCommentResponse>
    {
        private readonly RemoveCommentUseCase _removeCommentUseCase;

        public RemoveCommentUseCaseCallBack(RemoveCommentUseCase removeCommentUseCase)
        {
            _removeCommentUseCase = removeCommentUseCase;
        }

        public void OnSuccess(RemoveCommentResponse responseObj)
        {
            _removeCommentUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _removeCommentUseCase?.PresenterCallBack?.OnError(ex);
        }
    }
    //request object
    public class RemoveCommentRequest
    {
        public CommentBObj Comment;

        public RemoveCommentRequest(CommentBObj comment)    
        {
            Comment = comment;
        }
    }

   
    //response object
    public class RemoveCommentResponse
    {
        public List<string> RemovedCommentIds;

        public RemoveCommentResponse(List<string> removedCommentIds)
        {
            RemovedCommentIds = removedCommentIds;
            //RemovedCommentIds = removedCommentIds;
        }
    }
}
