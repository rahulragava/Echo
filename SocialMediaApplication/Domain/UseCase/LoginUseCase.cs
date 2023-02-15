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
        private readonly IUserManager _userManager = UserManager.GetInstance;
        public readonly LoginRequest LoginRequest;
        public LoginUseCase(LoginRequest loginRequest) : base(loginRequest.CancellationToken)
        {
            LoginRequest = loginRequest;
        }

        
        public override void Action()
        {
            _userManager.LoginUserAsync(LoginRequest,new LogInUseCaseCallBack(this));
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
            _loginUseCase?.LoginRequest.LoginPresenterCallBack?.OnSuccess(response);
        }

        public void OnError(Exception ex)
        {
            _loginUseCase?.LoginRequest.LoginPresenterCallBack?.OnError(ex);
        }
    }
    
    //request object
    public class LoginRequest
    {
        public string Email { get; }
        public string Password { get; }
        public CancellationToken CancellationToken { get; }
        public IPresenterCallBack<LoginResponse> LoginPresenterCallBack { get; }

        public LoginRequest(string email, string password, IPresenterCallBack<LoginResponse> loginPresenterCallBack, CancellationToken cancellationToken)
        {
            Email = email;
            Password = password;
            LoginPresenterCallBack = loginPresenterCallBack;
            CancellationToken = cancellationToken;
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
