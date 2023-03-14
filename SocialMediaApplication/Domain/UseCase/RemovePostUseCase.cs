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
    
    public class RemovePostUseCase : UseCaseBase<InsertCommentResponse>
    {
        private readonly IRemovePostManager _removePostManager = RemovePostManager.GetInstance;
        public readonly RemovePostRequest RemovePostRequest;

        public RemovePostUseCase(RemovePostRequest removePostRequest)
        {
            RemovePostRequest = removePostRequest;
        }

        public override void Action()
        {
            _removePostManager.RemovePost(RemovePostRequest, new RemovePostUseCaseCallBack(this));
            //_addCommentManager.InsertCommentAsync(InsertCommentRequest, new InsertCommentUseCaseCallBack(this));
            //_userManager.LoginUserAsync(LoginRequest, new LogInUseCaseCallBack(this));

        }
    }

    public class RemovePostUseCaseCallBack : IUseCaseCallBack<RemovePostResponse>
    {
        private readonly RemovePostUseCase _removePostUseCase;

        public RemovePostUseCaseCallBack(RemovePostUseCase removePostUseCase)
        {
            _removePostUseCase = removePostUseCase;
        }


        public void OnSuccess(RemovePostResponse responseObj)
        {
            _removePostUseCase?.RemovePostRequest.RemovePresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _removePostUseCase?.RemovePostRequest.RemovePresenterCallBack?.OnError(ex);
        }
    }

    //request object
    public class RemovePostRequest
    {
        public RemovePostRequest(PostBObj postBObj ,PostControlViewModel.RemovePostPresenterCallBack removePresenterCallBack)
        {
            PostBObj = postBObj;
            RemovePresenterCallBack = removePresenterCallBack;
        }

        public PostBObj PostBObj { get; }
        public IPresenterCallBack<RemovePostResponse> RemovePresenterCallBack{ get; }

    }

    public class RemovePostResponse
    {
        public string PostId;
        public RemovePostResponse(string postId)
        {
            PostId = postId;
        }
    }
}
