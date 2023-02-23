using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaApplication.Domain.UseCase
{

    public class GetUserNameUseCase : UseCaseBase<GetUserNameResponseObj>
    {
        private readonly IGetUserName _userManager = UserManager.GetInstance;
        public readonly GetUserNameRequestObj GetUserNameRequestObj;

        public GetUserNameUseCase(GetUserNameRequestObj getUserNameRequestObj)
        {
            GetUserNameRequestObj = getUserNameRequestObj;
        }

        public override void Action()
        {
            _userManager.GetUserNameAsync(GetUserNameRequestObj, new GetUserNamesUseCaseCallBack(this));
        }
    }

    //Get user name use case call back
    public class GetUserNamesUseCaseCallBack : IUseCaseCallBack<GetUserNameResponseObj>
    {
        private readonly GetUserNameUseCase _getUserNameUseCase;

        public GetUserNamesUseCaseCallBack(GetUserNameUseCase getUserNameUseCase)
        {
            _getUserNameUseCase = getUserNameUseCase;
        }

        public void OnSuccess(GetUserNameResponseObj responseObj)
        {
            _getUserNameUseCase?.GetUserNameRequestObj.GetUserNamePresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _getUserNameUseCase?.GetUserNameRequestObj.GetUserNamePresenterCallBack?.OnError(ex);
        }
    }

    //Request obj
    public class GetUserNameRequestObj
    {
        public GetUserNameRequestObj(string userId, IPresenterCallBack<GetUserNameResponseObj> getUserNamePresenterCallBack, CancellationToken cancellationToken)
        {
            UserId = userId;
            CancellationToken = cancellationToken;
            GetUserNamePresenterCallBack = getUserNamePresenterCallBack;
        }

        public string UserId { get; }
        public CancellationToken CancellationToken { get; } 
        public IPresenterCallBack<GetUserNameResponseObj> GetUserNamePresenterCallBack { get; }

    }

    //response object
    public class GetUserNameResponseObj
    {
        public string UserName { get; }
        public GetUserNameResponseObj(string userName)
        {
            UserName = userName;
        }
    }
}
