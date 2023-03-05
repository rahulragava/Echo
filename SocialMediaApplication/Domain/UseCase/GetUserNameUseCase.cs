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

    public class GetUserNamesUseCase : UseCaseBase<GetUserNamesResponseObj>
    {
        private readonly IGetUserNames _userManager = UserManager.GetInstance;
        public readonly GetUserNamesRequestObj GetUserNamesRequestObj;

        public GetUserNamesUseCase(GetUserNamesRequestObj getUserNameRequestObj)
        {
            GetUserNamesRequestObj = getUserNameRequestObj;
        }

        public override void Action()
        {
            _userManager.GetUserNamesAsync(GetUserNamesRequestObj, new GetUserNamesUseCaseCallBack(this));
        }
    }

    //Get user name use case call back
    public class GetUserNamesUseCaseCallBack : IUseCaseCallBack<GetUserNamesResponseObj>
    {
        private readonly GetUserNamesUseCase _getUserNameUseCase;

        public GetUserNamesUseCaseCallBack(GetUserNamesUseCase getUserNameUseCase)
        {
            _getUserNameUseCase = getUserNameUseCase;
        }

        public void OnSuccess(GetUserNamesResponseObj responseObj)
        {
            _getUserNameUseCase?.GetUserNamesRequestObj.GetUserNamePresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _getUserNameUseCase?.GetUserNamesRequestObj.GetUserNamePresenterCallBack?.OnError(ex);
        }
    }

    //Request obj
    public class GetUserNamesRequestObj
    {
        public GetUserNamesRequestObj(IPresenterCallBack<GetUserNamesResponseObj> getUserNamePresenterCallBack)
        {
            GetUserNamePresenterCallBack = getUserNamePresenterCallBack;
        }

        public IPresenterCallBack<GetUserNamesResponseObj> GetUserNamePresenterCallBack { get; }

    }

    //response object
    public class GetUserNamesResponseObj
    {
        public List<string> UserNames { get; }
        public List<string> UserIds { get; }
        public GetUserNamesResponseObj(List<string> userNames, List<string> userIds)
        {
            UserNames = userNames;
            UserIds = userIds;
        }
    }
}
