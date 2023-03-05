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

        public RemoveCommentUseCase(RemoveCommentRequest removeCommentRequest)
        {
            RemoveCommentRequest = removeCommentRequest;
        }

        public override void Action()
        {
            _removeCommentManager.RemoveCommentAsync(RemoveCommentRequest, new RemoveCommentUseCaseCallBack(this));
            //_addCommentManager.InsertCommentAsync(InsertCommentRequest, new InsertCommentUseCaseCallBack(this));
            //_userManager.LoginUserAsync(LoginRequest, new LogInUseCaseCallBack(this));

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
            _removeCommentUseCase?.RemoveCommentRequest.RemoveCommentPresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _removeCommentUseCase?.RemoveCommentRequest.RemoveCommentPresenterCallBack?.OnError(ex);
        }
    }
    //request object
    public class RemoveCommentRequest
    {
        public CommentBObj Comment;
        public List<CommentBObj> Comments;
        public IPresenterCallBack<RemoveCommentResponse> RemoveCommentPresenterCallBack;

        public RemoveCommentRequest(CommentBObj comment,List<CommentBObj> comments, IPresenterCallBack<RemoveCommentResponse> removeCommentPresenterCallBack)
        {
            Comment = comment;
            Comments = comments;
            RemoveCommentPresenterCallBack = removeCommentPresenterCallBack;
        }
    }

   
    //response object
    public class RemoveCommentResponse
    {
        public List<string> RemovedCommentIds;

        public RemoveCommentResponse(List<string> removedCommentIds)
        {
            RemovedCommentIds = removedCommentIds;
        }
    }
}
