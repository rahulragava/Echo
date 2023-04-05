using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.DataManager.CustomException;
using SocialMediaApplication.Presenter.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View.MainPageView.LogInView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginInPage : UserControl,ILogInView
    {
        private readonly LogInViewModel _loginViewModel;
        public LoginInPage()
        {
            _loginViewModel = new LogInViewModel
            {
                LogInView = this
            };
            this.InitializeComponent();

        }
        public event Action GoToHome;
        public event Action GoToSignUpControl;
        public ResourceLoader ResourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
        public void GoToHomePage()
        {
            GoToHome?.Invoke();
            //this.Frame.Navigate(typeof(HomePage));
        }

        public void MailErrorMessage(string message)
        {
            if (message == "Invalid mail id ")
            {
                _loginViewModel.MailErrorMessage = ResourceLoader.GetString("InvalidMail");
            }
            else if (message == "mail cant be empty")
            {
                _loginViewModel.MailErrorMessage = ResourceLoader.GetString("MailEmpty");
            }
            _loginViewModel.IsPasswordErrorVisible = false;

        }

        public void PasswordErrorMessageNotification(string message)
        {

            if (message == "Password must contain at least 8 characters")
            {
                _loginViewModel.PasswordErrorMessage = ResourceLoader.GetString("InvalidPassword");
            }
            else if (message == "Password can't be empty")
            {
                _loginViewModel.PasswordErrorMessage= ResourceLoader.GetString("PasswordEmpty");
            }

            _loginViewModel.IsMailErrorVisible = false;
        }



        private void GoToSignUpPageClick(object sender, RoutedEventArgs e)
        {
            Email.Text = String.Empty;
            PasswordBox.Password = string.Empty;
            EmailErrorMessage.Visibility = Visibility.Collapsed;
            PasswordErrorMessage.Visibility = Visibility.Collapsed;
            GoToSignUpControl?.Invoke();
            //this.Frame.Navigate(typeof(SignUpPage));
        }

        private void RevealPassword(object sender, TappedRoutedEventArgs e)
        {
            PasswordBox.PasswordRevealMode = _loginViewModel.IsPasswordReveal
                ? PasswordRevealMode.Visible
                : PasswordRevealMode.Hidden;
            _loginViewModel.IsPasswordReveal = !_loginViewModel.IsPasswordReveal;
        }

        public event Action<string> NotifyError;
        public void ErrorMessageNotification(Exception exception)
        {
            switch (exception)
            {
                case NoSuchUserException noSuchUserException:
                    NotifyError?.Invoke(noSuchUserException.Message);
                    break;
                default:
                    NotifyError?.Invoke(exception.Message);
                    break;
            }
        }


        private void Email_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Email.Text.Length > 0 && PasswordBox.Password.Length > 0)
            {
                LogInButton.IsEnabled = true;
            }
            else
            {
                LogInButton.IsEnabled = false;
            }
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Email.Text.Length > 0 && PasswordBox.Password.Length > 0)
            {
                LogInButton.IsEnabled = true;
            }
            else
            {
                LogInButton.IsEnabled = false;
            }
        }
    }

    public interface ILogInView
    {
        void GoToHomePage();
        void MailErrorMessage(string message);
        void PasswordErrorMessageNotification(string message);
        void ErrorMessageNotification(Exception exception);
    }
}
