using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;

namespace SocialMediaApplication.Domain.UseCase
{

    public class GetUserProfileUseCase : UseCaseBase<GetUserProfileResponseObj>
    {
        //profile page use case 
        private readonly IUserManager _userManager = UserManager.GetInstance;
        public readonly GetUserProfileRequestObj GetUserProfileRequest;

        public GetUserProfileUseCase(GetUserProfileRequestObj getUserProfileRequest)
        {
            GetUserProfileRequest = getUserProfileRequest;
        }

        public override void Action()
        {
            _userManager.GetUserBObjAsync(GetUserProfileRequest, new GetUserProfileUseCaseCallBack(this));
        }
    }

    //profile page use case call back
    public class GetUserProfileUseCaseCallBack : IUseCaseCallBack<GetUserProfileResponseObj>
    {
        private readonly GetUserProfileUseCase _getUserProfileUseCase;

        public GetUserProfileUseCaseCallBack(GetUserProfileUseCase getUserProfileUseCase)
        {
            _getUserProfileUseCase = getUserProfileUseCase;
        }

        public void OnSuccess(GetUserProfileResponseObj response)
        {
            _getUserProfileUseCase?.GetUserProfileRequest.ProfilePresenterCallBack?.OnSuccess(response);
        }

        public void OnError(Exception ex)
        {
            _getUserProfileUseCase?.GetUserProfileRequest.ProfilePresenterCallBack?.OnError(ex);
        }
    }



    public class GetUserProfileRequestObj
    {
        public string UserId { get; }
        public IPresenterCallBack<GetUserProfileResponseObj> ProfilePresenterCallBack { get; }

        public GetUserProfileRequestObj(string userId, IPresenterCallBack<GetUserProfileResponseObj> profilePresenterCallBack)
        {
            UserId = userId;
            ProfilePresenterCallBack = profilePresenterCallBack;
        }
    }

    public class GetUserProfileResponseObj
    {
        public UserBObj User { get; }

        public GetUserProfileResponseObj(UserBObj user)
        {
            User = user;
        }
    }
}



