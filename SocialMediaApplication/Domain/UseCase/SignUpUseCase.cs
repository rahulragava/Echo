//using SocialMediaApplication.DataManager.ResponseObj;
using SocialMediaApplication.DataManager;
//using SocialMediaApplication.Presenter.ViewModel.RequestObj;
using System;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System.Threading;

namespace SocialMediaApplication.Domain.UseCase
{
    //sign up use case 
    public class SignUpUseCase : UseCaseBase<SignUpResponse>
    {
        private readonly IUserManager _userManager = UserManager.GetInstance;
        public readonly SignUpRequestObj SignUpRequestObj;
        public SignUpUseCase(SignUpRequestObj signUpRequestObj)
        {
            SignUpRequestObj = signUpRequestObj;
        }

        public override void Action()
        {
            _userManager.SignUpUserAsync(SignUpRequestObj, new SignUpUseCaseCallBack(this));
        }
    }

    //sign up use case call back
    public class SignUpUseCaseCallBack : IUseCaseCallBack<SignUpResponse>
    {
        private readonly SignUpUseCase _signUpUseCase;
        public SignUpUseCaseCallBack(SignUpUseCase signUpUseCase)
        {
            _signUpUseCase = signUpUseCase;
        }

        public void OnSuccess(SignUpResponse response)
        {
            _signUpUseCase?.SignUpRequestObj.SignUpPresenterCallBack?.OnSuccess(response);
        }

        public void OnError(Exception ex)
        {
            _signUpUseCase?.SignUpRequestObj.SignUpPresenterCallBack?.OnError(ex);
        }
    }

    //Request obj
    public class SignUpRequestObj
    {
        public string UserName { get; }
        public string Email { get; }
        public string Password { get; }
        public string RetypePassword { get; }
        public IPresenterCallBack<SignUpResponse> SignUpPresenterCallBack { get; }

        public SignUpRequestObj(string userName, string email, string password, string retypePassword, IPresenterCallBack<SignUpResponse> loginPresenterCallBack)
        {
            UserName = userName;
            Email = email;
            Password = password;
            RetypePassword = retypePassword;
            SignUpPresenterCallBack = loginPresenterCallBack;
        }
    }

    //response object
    public class SignUpResponse
    {
        public UserBObj User { get; }

        public SignUpResponse(UserBObj user)
        {
            User = user;
        }
    }
}
