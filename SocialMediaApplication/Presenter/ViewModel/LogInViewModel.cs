using System;
using System.Text.RegularExpressions;
using System.Threading;
using SocialMediaApplication.Domain.UseCase;
using Windows.UI.Core;
using SocialMediaApplication.Presenter.View;
using SocialMediaApplication.Presenter.View.MainPageView.LogInView;
using SocialMediaApplication.Util;
using ObservableObject = Microsoft.VisualStudio.PlatformUI.ObservableObject;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class LogInViewModel : ObservableObject
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public ILogInView LogInView { get; set; }

        private bool _isPasswordReveal =true;
        public bool IsPasswordReveal
        {
            get => _isPasswordReveal;
            set => SetProperty(ref _isPasswordReveal, value);
        } 


        public string ErrorMessage { get; set; }

        private bool _isMailErrorVisible = false;
        public bool IsMailErrorVisible
        {
            get => _isMailErrorVisible;
            set => SetProperty(ref _isMailErrorVisible, value);
        }

        private string _passwordErrorMessage;
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set => SetProperty(ref _passwordErrorMessage, value);
        }

        
        private bool _isPasswordErrorVisible = false;
        public bool IsPasswordErrorVisible
        {
            get => _isPasswordErrorVisible;
            set => SetProperty(ref _isPasswordErrorVisible, value);
        }

        private string _mailErrorMessage;
        public string MailErrorMessage
        {
            get => _mailErrorMessage;
            set => SetProperty(ref _mailErrorMessage,value);

        }

        public void RevealPassword()
        {
            IsPasswordReveal = !IsPasswordReveal;
        }

        private static readonly CancellationTokenSource Cts = new CancellationTokenSource();


        public void LoginButtonOnClick()
        {

            if (string.IsNullOrEmpty(Email))
            {
                IsMailErrorVisible = true;
                MailErrorMessage = "mail cant be empty";

                return;
            }
            else
            {
                if (!Regex.IsMatch(Email,
                        @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                        RegexOptions.IgnoreCase))
                {
                    // invalid email visibility
                    IsMailErrorVisible = true;
                    LogInView.MailErrorMessage("Invalid mail id ");
                    return;
                }
            }

            if (string.IsNullOrEmpty(Password))
            {
                //password cant be null visible
                IsPasswordErrorVisible = true;
                LogInView.PasswordErrorMessageNotification("Password can't be empty");
                return;
            }

            if (Password.Length < 8)
            {
                // invalid password visible
                IsPasswordErrorVisible = true;
                LogInView.PasswordErrorMessageNotification("Password must contain at least 8 characters");
                return;
            }

            var loginRequestObj = new LoginRequest(Email,Password);

            var loginUseCase= new LoginUseCase(loginRequestObj, new LogInViewModelPresenterCallBack(this));
            loginUseCase.Execute();
            Email = string.Empty;
            Password = string.Empty;
        }
        
        public class LogInViewModelPresenterCallBack : IPresenterCallBack<LoginResponse>
        {
            private readonly LogInViewModel _loginViewModel;
            public LogInViewModelPresenterCallBack(LogInViewModel loginViewModel)
            {
                _loginViewModel = loginViewModel;
            }

            public void OnSuccess(LoginResponse logInResponse)
            {
                AppSettings.LocalSettings.Values["user"] = logInResponse.User.Id;
                AppSettings.UserId = AppSettings.LocalSettings.Values["user"]?.ToString();

                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _loginViewModel.LogInView.GoToHomePage();
                    }
                );
            }
            public void OnError(Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _loginViewModel.LogInView.ErrorMessageNotification(ex);
                    }
                );
            }
        }
    }
}
