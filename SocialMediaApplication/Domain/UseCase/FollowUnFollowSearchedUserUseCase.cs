using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Domain.UseCase
{
    
    public class FollowUnFollowSearchedUserUseCase : UseCaseBase<FollowUnFollowSearchedUserResponse>
    {
        public IFollowUnFollowManager FollowUnFollowManager =  DataManager.FollowUnFollowManager.GetInstance;
        public FollowUnFollowSearchedUserRequest FollowUnFollowSearchedUserRequest;

        public FollowUnFollowSearchedUserUseCase(FollowUnFollowSearchedUserRequest followUnFollowSearchedUserRequest, IPresenterCallBack<FollowUnFollowSearchedUserResponse> followUnFollowSearchedUserPresenterCallback) : base(followUnFollowSearchedUserPresenterCallback)
        {
            FollowUnFollowSearchedUserRequest = followUnFollowSearchedUserRequest;
        }

        public override void Action()
        {
            FollowUnFollowManager.FollowUnFollowAsync(FollowUnFollowSearchedUserRequest,
                new FollowUnFollowSearchedUserUseCaseCallBack(this));
        }
    }

    public class FollowUnFollowSearchedUserUseCaseCallBack : IUseCaseCallBack<FollowUnFollowSearchedUserResponse>
    {
        private readonly FollowUnFollowSearchedUserUseCase _followUnFollowSearchedUserUseCase;

        public FollowUnFollowSearchedUserUseCaseCallBack(FollowUnFollowSearchedUserUseCase followUnFollowSearchedUserUseCase)
        {
            _followUnFollowSearchedUserUseCase = followUnFollowSearchedUserUseCase;
        }

        public void OnSuccess(FollowUnFollowSearchedUserResponse responseObj)
        {
            _followUnFollowSearchedUserUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _followUnFollowSearchedUserUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    public class FollowUnFollowSearchedUserRequest
    {
        public string SearchedUserId;
        public string ViewingUserId;

        public FollowUnFollowSearchedUserRequest(string searchedUserId, string viewingUserId)
        {
            SearchedUserId = searchedUserId;
            ViewingUserId = viewingUserId;
        }
    }

    public class FollowUnFollowSearchedUserResponse
    {
        public Follower Follower;
        public bool FollowingSuccess;

        public FollowUnFollowSearchedUserResponse(Follower follower, bool followingSuccess)
        {
            Follower = follower;
            FollowingSuccess = followingSuccess;
        }
    }
}
