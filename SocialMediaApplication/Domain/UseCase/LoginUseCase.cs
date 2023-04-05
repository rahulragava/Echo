using System;
using System.Threading;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;


namespace SocialMediaApplication.Domain.UseCase
{
    //use case
    public class LoginUseCase : UseCaseBase<LoginResponse>
    {
        private readonly ILoginManager _loginManager = LogInManager.GetInstance;
        public readonly LoginRequest LoginRequest;
        public LoginUseCase(LoginRequest loginRequest, IPresenterCallBack<LoginResponse> loginPresenterCallBack) : base(loginPresenterCallBack) 
        {
            LoginRequest = loginRequest;
        }
        
        public override void Action()
        {
            _loginManager.LoginUserAsync(LoginRequest,new LogInUseCaseCallBack(this));
        }
    }

    //use case call back
    public class LogInUseCaseCallBack : IUseCaseCallBack<LoginResponse>
    {
        private readonly LoginUseCase _loginUseCase;

        public LogInUseCaseCallBack(LoginUseCase loginUseCase)
        {
            _loginUseCase = loginUseCase;
        }

        public void OnSuccess(LoginResponse response)
        {
            _loginUseCase?.PresenterCallBack?.OnSuccess(response);
        }

        public void OnError(Exception ex)
        {
            _loginUseCase?.PresenterCallBack?.OnError(ex);
        }
    }
    
    //request object
    public class LoginRequest
    {
        public string Email { get; }
        public string Password { get; }

        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    //response object
    public class LoginResponse
    {
        public UserBObj User { get; }

        public LoginResponse(UserBObj user)
        {
            User = user;
        }
    }
}
