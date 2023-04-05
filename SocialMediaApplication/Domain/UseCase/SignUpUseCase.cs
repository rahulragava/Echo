using SocialMediaApplication.DataManager;
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
        private readonly ISignUpManager _signUpManager = SignUpManager.GetInstance;
        public readonly SignUpRequestObj SignUpRequestObj;
        public SignUpUseCase(SignUpRequestObj signUpRequestObj,IPresenterCallBack<SignUpResponse> signUpPresenterCallBack):base(signUpPresenterCallBack)
        {
            SignUpRequestObj = signUpRequestObj;
        }

        public override void Action()
        {
            _signUpManager.SignUpUserAsync(SignUpRequestObj, new SignUpUseCaseCallBack(this));
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
            _signUpUseCase?.PresenterCallBack?.OnSuccess(response);
        }

        public void OnError(Exception ex)
        {
            _signUpUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    //Request obj
    public class SignUpRequestObj
    {
        public string UserName { get; }
        public string Email { get; }
        public string Password { get; }
        public string RetypePassword { get; }

        public SignUpRequestObj(string userName, string email, string password, string retypePassword)
        {
            UserName = userName;
            Email = email;
            Password = password;
            RetypePassword = retypePassword;
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
