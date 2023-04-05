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
        private readonly IGetUserManager _getUserManager = GetUserManager.GetInstance;
        public readonly GetUserProfileRequestObj GetUserProfileRequest;

        public GetUserProfileUseCase(GetUserProfileRequestObj getUserProfileRequest, IPresenterCallBack<GetUserProfileResponseObj> profilePresenterCallBack) : base(profilePresenterCallBack)
        {
            GetUserProfileRequest = getUserProfileRequest;
        }

        public override void Action()
        {
            _getUserManager.GetUserBObjAsync(GetUserProfileRequest, new GetUserProfileUseCaseCallBack(this));
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


        public void OnSuccess(GetUserProfileResponseObj responseObj)
        {
            _getUserProfileUseCase?.PresenterCallBack?.OnSuccess(responseObj);

        }

        public void OnError(Exception ex)
        {
            _getUserProfileUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    public class GetUserProfileRequestObj
    {
        public string UserId { get; }

        public GetUserProfileRequestObj(string userId)
        {
            UserId = userId;
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



