using System;
using System.Text.RegularExpressions;
using System.Threading;
using Windows.UI.Xaml;
using SocialMediaApplication.Domain.UseCase;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Windows.UI.Core;
using GalaSoft.MvvmLight;
using Microsoft.VisualStudio.PlatformUI;
using ObservableObject = Microsoft.VisualStudio.PlatformUI.ObservableObject;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class LogInViewModel : ObservableObject
    {
        public string Email { get; set; }
        public string Password { get; set; }

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

        public EventHandler GoToHomePageEventHandler;

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
                    MailErrorMessage = "Invalid mail id ";
                    return;
                }
            }

            if (string.IsNullOrEmpty(Password))
            {
                //password cant be null visible
                IsPasswordErrorVisible = true;
                PasswordErrorMessage = "Password can't be empty";
                return;
            }
            else
            {
                if (Password.Length < 8)
                {
                    // invalid password visible
                    IsPasswordErrorVisible = true;
                    PasswordErrorMessage = "Password must contain at least 8 characters";
                    return;
                }
            }
            
            var loginRequestObj = new LoginRequest(Email,Password,new LogInViewModelPresenterCallBack(this),Cts.Token);

            var loginUseCase= new LoginUseCase(loginRequestObj);
            loginUseCase.Execute();
            Email = string.Empty;
            Password = string.Empty;
        }

        public void SuccessfullyLoggedIn()
        {
            GoToHomePageEventHandler?.Invoke(this,EventArgs.Empty);
        }
    }


    public class LogInViewModelPresenterCallBack : IPresenterCallBack<LoginResponse>
    {
        private LogInViewModel _loginViewModel;
        public LogInViewModelPresenterCallBack(LogInViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
        }

        public void OnSuccess(LoginResponse logInResponse)
        {
            //var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //localSettings.Values["user"] = logInResponse.User.Id;
            App.LocalSettings.Values["user"] = logInResponse.User.Id;
            
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    // Your UI update code goes here!
                    _loginViewModel.SuccessfullyLoggedIn();
                }
            );
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }


}
