using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.DataManager.CustomException;
using SocialMediaApplication.Presenter.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View.MainPageView.SignUp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : UserControl, ISignUpView
    {
        private SignUpViewModel _signUpViewModel;
        public SignUpPage()
        {
            this.InitializeComponent();
            _signUpViewModel = new SignUpViewModel();
            Loaded += SignUpPage_Loaded;
        }

        private void SignUpPage_Loaded(object sender, RoutedEventArgs e)
        {
            _signUpViewModel.SignUpView = this;
            ResourceLoader = ResourceLoader.GetForCurrentView();
        }
        public ResourceLoader ResourceLoader;

        public void ErrorMessageNotification(Exception exception)
        {

            switch (exception)
            {
                case UserNameAlreadyExistException userNameAlreadyExistException:
                    UserName.Text = string.Empty;
                    Email.Text = _signUpViewModel.Email;
                    PasswordBox.Password = _signUpViewModel.Password;
                    RetypePasswordBox.Password = _signUpViewModel.RetypedPassword;
                    ExampleVsCodeInAppNotification.Show(ResourceLoader.GetString("UserNameExistNotification"), 5000);
                    break;
                case UserMailAlreadyExistException userMailAlreadyExistException:
                    Email.Text = string.Empty;
                    UserName.Text = _signUpViewModel.UserName;
                    PasswordBox.Password = _signUpViewModel.Password;
                    RetypePasswordBox.Password = _signUpViewModel.RetypedPassword;
                    ExampleVsCodeInAppNotification.Show(ResourceLoader.GetString("MailExistNotification"), 5000);
                    break;
            }
         
            RetypePasswordError.Visibility = Visibility.Collapsed;
            PasswordError.Visibility = Visibility.Collapsed;
            MailErrorTextBox.Visibility = Visibility.Collapsed;
        }

        public void MailErrorMessage(string message)
        {
            if (message == "Invalid Mail")
            {
                _signUpViewModel.MailErrorMessage = ResourceLoader.GetString("InvalidMail");
            }
            else if (message == "Mail id Cannot be empty")
            {
                _signUpViewModel.MailErrorMessage = ResourceLoader.GetString("MailEmpty");
            }
            PasswordError.Visibility = Visibility.Collapsed;
            RetypePasswordError.Visibility = Visibility.Collapsed;
        }

        public void PasswordErrorMessageNotification(string message)
        {

            if (message == "Invalid password")
            {
                _signUpViewModel.PasswordErrorMessage = ResourceLoader.GetString("InvalidPassword");
            }
            else if (message == "Password can't be empty")
            {
                _signUpViewModel.PasswordErrorMessage = ResourceLoader.GetString("PasswordEmpty");
            }
            else if (message == "This slot can't be empty")
            {
                _signUpViewModel.ReTypePasswordErrorMessage = ResourceLoader.GetString("PasswordEmpty");
            }
            else if (message == "retype password is not same as password")
            {
                _signUpViewModel.ReTypePasswordErrorMessage = ResourceLoader.GetString("InvalidRetypePassword");
            }

            _signUpViewModel.IsMailErrorVisible = false;
        }

        public event Action GoToLogInControl;

        public void GotoSignInPageClick(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = string.Empty;
            RetypePasswordBox.Password = string.Empty;
            UserName.Text = string.Empty;
            Email.Text = string.Empty;
            RetypePasswordError.Visibility = Visibility.Collapsed;
            PasswordError.Visibility = Visibility.Collapsed;
            MailErrorTextBox.Visibility = Visibility.Collapsed;
            GoToLogInControl?.Invoke();
        }

        public void GoToLogInPage()
        {
            GoToLogInControl?.Invoke();
        }

        private void RevealPassword(object sender, TappedRoutedEventArgs e)
        {
            var fontIcon = sender as FontIcon;
            if (fontIcon != null && fontIcon.Name == "PasswordRevealEyeIcon")
            {
                PasswordBox.PasswordRevealMode = _signUpViewModel.IsPasswordVisible
                    ? PasswordRevealMode.Visible
                    : PasswordRevealMode.Hidden;
                _signUpViewModel.IsPasswordVisible = !_signUpViewModel.IsPasswordVisible;
            }
            else if (fontIcon != null && fontIcon.Name == "RetypePasswordRevealEyeIcon")
            {
                RetypePasswordBox.PasswordRevealMode = _signUpViewModel.IsRetypePasswordVisible
                    ? PasswordRevealMode.Visible
                    : PasswordRevealMode.Hidden;
                _signUpViewModel.IsRetypePasswordVisible = !_signUpViewModel.IsRetypePasswordVisible;
            }
        }

        private void UserName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Email.Text.Length > 0 && PasswordBox.Password.Length > 0 && RetypePasswordBox.Password.Length > 0 && UserName.Text.Length > 0)
            {
                SignUp.IsEnabled = true;
            }
            else
            {
                SignUp.IsEnabled = false;
            }
        }

        private void Email_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Email.Text.Length > 0 && PasswordBox.Password.Length > 0 && RetypePasswordBox.Password.Length > 0 && UserName.Text.Length > 0)
            {
                SignUp.IsEnabled = true;
            }
            else
            {
                SignUp.IsEnabled = false;
            }
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Email.Text.Length > 0 && PasswordBox.Password.Length > 0 && RetypePasswordBox.Password.Length > 0 && UserName.Text.Length > 0)
            {
                SignUp.IsEnabled = true;
            }
            else
            {
                SignUp.IsEnabled = false;
            }
        }

        private void RetypePasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Email.Text.Length > 0 && PasswordBox.Password.Length > 0 && RetypePasswordBox.Password.Length > 0 && UserName.Text.Length > 0)
            {
                SignUp.IsEnabled = true;
            }
            else
            {
                SignUp.IsEnabled = false;
            }
        }
    }

    public interface ISignUpView
    {
        void GoToLogInPage();
        void ErrorMessageNotification(Exception exception);
        void MailErrorMessage(string message);
        void PasswordErrorMessageNotification(string message);
    }
}
