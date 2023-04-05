using System;
using SocialMediaApplication.Domain.UseCase;
using System.Text.RegularExpressions;
using Windows.UI.Core;
using SocialMediaApplication.Presenter.View.MainPageView.SignUp;
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
                SignUpView.MailErrorMessage("Mail id Cannot be empty");
                return;
            }

            if (!Regex.IsMatch(Email,
                    @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                    RegexOptions.IgnoreCase))
            {
                // invalid email visibility
                IsMailErrorVisible = true;
                SignUpView.MailErrorMessage("Invalid Mail");
                return;
            }

            IsMailErrorVisible = false;

            if (string.IsNullOrEmpty(Password))
            {
                //password cant be null visible
                IsPasswordErrorVisible = true;
                SignUpView.PasswordErrorMessageNotification("Password cannot be empty");
                return;
            }

            if (Password.Length < 8)
            {
                // invalid password visible
                IsPasswordErrorVisible = true;
                SignUpView.PasswordErrorMessageNotification("Invalid password");
                return;
            }

            IsPasswordErrorVisible = false;

            if (string.IsNullOrEmpty(RetypedPassword))
            {
                IsRetypePasswordErrorVisible = true;
                SignUpView.PasswordErrorMessageNotification("This slot can't be empty");
                return;
            }

            if (Password != RetypedPassword)
            {
                //error visible 
                IsRetypePasswordErrorVisible = true;
                SignUpView.PasswordErrorMessageNotification("retype password is not same as password");
                return;
            }

            IsRetypePasswordErrorVisible = false;

            if (string.IsNullOrEmpty(UserName))
            {
                _isUserNameErrorVisible = true;
                UserNameErrorMessage = "userName cannot be empty";
                return;
            }

            _isUserNameErrorVisible = false;

            var signUpRequestObj = new SignUpRequestObj(UserName, Email, Password, RetypedPassword);
            var signUpUseCase= new SignUpUseCase(signUpRequestObj, new SignUpViewModelPresenterCallBack(this));
            signUpUseCase.Execute();
            //UserName = string.Empty;
            //Email = string.Empty;
            //Password = string.Empty;
            //RetypedPassword = string.Empty;
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
                        _signUpViewModel.SignUpView.ErrorMessageNotification(ex);
                    }
                );
            }
        }
    }
}
