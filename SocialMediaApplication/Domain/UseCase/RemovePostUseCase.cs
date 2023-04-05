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
    
    public class RemovePostUseCase : UseCaseBase<RemovePostResponse>
    {
        private readonly IRemovePostManager _removePostManager = RemovePostManager.GetInstance;
        public readonly RemovePostRequest RemovePostRequest;

        public RemovePostUseCase(RemovePostRequest removePostRequest, IPresenterCallBack<RemovePostResponse> removePostPresenterCallBack) : base(removePostPresenterCallBack)
        {
            RemovePostRequest = removePostRequest;
        }

        public override void Action()
        {
            _removePostManager.RemovePostAsync(RemovePostRequest, new RemovePostUseCaseCallBack(this));
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
            _removePostUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _removePostUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    //request object
    public class RemovePostRequest
    {
        public RemovePostRequest(PostBObj postBObj)
        {
            PostBObj = postBObj;
        }

        public PostBObj PostBObj { get; }

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
