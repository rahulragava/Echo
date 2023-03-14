using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Domain.UseCase
{
    
    public class GetUserUseCase : UseCaseBase<GetUserResponseObj>
    {
        //profile page use case 
        private readonly IGetUserManager _getUserManager = GetUserManager.GetInstance;
        public readonly GetUserRequestObj GetUserRequest;

        public GetUserUseCase(GetUserRequestObj getUserRequestObj)
        {
            GetUserRequest = getUserRequestObj;
        }

        public override void Action()
        {
            _getUserManager.GetUserAsync(GetUserRequest, new GetUserUseCaseCallBack(this));
        }
    }

    //profile page use case call back
    public class GetUserUseCaseCallBack : IUseCaseCallBack<GetUserResponseObj>
    {
        private readonly GetUserUseCase _getUserUseCase;

        public GetUserUseCaseCallBack(GetUserUseCase getUserUseCase)
        {
            _getUserUseCase = getUserUseCase;
        }

        public void OnSuccess(GetUserResponseObj response)
        {
            _getUserUseCase?.GetUserRequest.GetUserPresenterCallBack?.OnSuccess(response);
        }

        public void OnError(Exception ex)
        {
            _getUserUseCase?.GetUserRequest.GetUserPresenterCallBack?.OnError(ex);
        }
    }



    public class GetUserRequestObj
    {
        public List<string> UserIds { get; }
        public IPresenterCallBack<GetUserResponseObj> GetUserPresenterCallBack { get; }

        public GetUserRequestObj(List<string> userIds, IPresenterCallBack<GetUserResponseObj> profilePresenterCallBack)
        {
            UserIds = userIds;
            GetUserPresenterCallBack = profilePresenterCallBack;
        }
    }

    public class GetUserResponseObj
    {
        public List<User> Users { get; }

        public GetUserResponseObj(List<User> users)
        {
            Users = users;
        }
    }
}
