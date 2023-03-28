using System;
using SocialMediaApplication.Domain.UseCase;
using System.Text.RegularExpressions;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Threading;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.View;
using SocialMediaApplication.Presenter.View.SignUp;
using ObservableObject = Microsoft.VisualStudio.PlatformUI.ObservableObject;


namespace SocialMediaApplication.Presenter.ViewModel
{
    public class SignUpViewModel : ObservableObject
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsPasswordVisible { get; set; } = true;
        public string RetypedPassword { get; set; }
        public bool IsRetypePasswordVisible { get; set; } = true;
        public ISignUpView SignUpView { get; set; }

        private string _userNameErrorMessage;
        public string UserNameErrorMessage
        {
            get => _userNameErrorMessage;
            set => SetProperty(ref _userNameErrorMessage, value);
        }

        private string _mailErrorMessage;
        public string MailErrorMessage
        {
            get => _mailErrorMessage;
            set => SetProperty(ref _mailErrorMessage, value);
        }

        private string _passwordErrorMessage;
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set => SetProperty(ref _passwordErrorMessage, value);
        }

        private string _reTypePasswordErrorMessage;
        public string ReTypePasswordErrorMessage
        {
            get => _reTypePasswordErrorMessage;
            set => SetProperty(ref _reTypePasswordErrorMessage, value);
        }

        private bool _isUserNameErrorVisible = false;
        public bool IsTextErrorVisible
        {
            get => _isUserNameErrorVisible;
            set => SetProperty(ref _isUserNameErrorVisible, value);
        }

        private bool _isMailErrorVisible = false;
        public bool IsMailErrorVisible
        {
            get => _isMailErrorVisible;
            set => SetProperty(ref _isMailErrorVisible, value);
        }

        private bool _isRetypePasswordErrorVisible = false;
        public bool IsRetypePasswordErrorVisible
        {
            get => _isRetypePasswordErrorVisible;
            set => SetProperty(ref _isRetypePasswordErrorVisible, value);
        }

        private bool _isPasswordErrorVisible = false;
        public bool IsPasswordErrorVisible
        {
            get => _isPasswordErrorVisible;
            set => SetProperty(ref _isPasswordErrorVisible, value);
        }



        public void SignUpButtonClicked()
        {
            if (string.IsNullOrEmpty(Email))
            {
                IsMailErrorVisible = true;
                MailErrorMessage = "Mail id Cannot be empty";
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
                    MailErrorMessage = "Invalid Mail";
                    return;
                }
                else
                {
                    IsMailErrorVisible = false;
                }
            }

            if (string.IsNullOrEmpty(Password))
            {
                //password cant be null visible
                IsPasswordErrorVisible = true;
                PasswordErrorMessage = "Password cannot be empty";
                return;
            }
            else
            {
                if (Password.Length < 8)
                {
                    // invalid password visible
                    IsPasswordErrorVisible = true;
                    PasswordErrorMessage = "Invalid password";
                    return;
                }
                else
                {
                    IsPasswordErrorVisible = false;
                }
            }

            if (string.IsNullOrEmpty(RetypedPassword))
            {
                IsRetypePasswordErrorVisible = true;
                ReTypePasswordErrorMessage = "this slot cannot be empty";
                return;
            }
            else
            {
                if (Password != RetypedPassword)
                {
                    //error visible 
                    IsRetypePasswordErrorVisible = true;
                    ReTypePasswordErrorMessage = "retype password is not same as password";
                    return;
                }
                else
                {
                    IsRetypePasswordErrorVisible = false;
                }
            }

            if (string.IsNullOrEmpty(UserName))
            {
                _isUserNameErrorVisible = true;
                UserNameErrorMessage = "userName cannot be empty";
                return;
            }

            _isUserNameErrorVisible = false;

            var signUpRequestObj = new SignUpRequestObj(UserName, Email, Password, RetypedPassword, new SignUpViewModelPresenterCallBack(this));
            var signUpUseCase= new SignUpUseCase(signUpRequestObj);
            signUpUseCase.Execute();
            UserName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            RetypedPassword = string.Empty;
        }

        public class SignUpViewModelPresenterCallBack : IPresenterCallBack<SignUpResponse>
        {
            private readonly SignUpViewModel _signUpViewModel;
            public SignUpViewModelPresenterCallBack(SignUpViewModel signUpViewModel)
            {
                _signUpViewModel = signUpViewModel;
            }
            //public event GoToLogInPageAfterCreateAccountEvent  
            public void OnSuccess(SignUpResponse logInResponse)
            {
                //get to login page once sign in gets successful
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _signUpViewModel.SignUpView.GoToLogInPage();
                    }
                );
            }

            public void OnError(Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _signUpViewModel.SignUpView.ErrorMessageNotification(ex.Message);
                    }
                );
            }
        }
    }
}
