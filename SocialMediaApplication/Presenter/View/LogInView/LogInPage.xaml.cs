using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.Presenter.View.SignUp;
using SocialMediaApplication.Presenter.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View.LogInView
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
        public void GoToHomePage()
        {
            GoToHome?.Invoke();
            //this.Frame.Navigate(typeof(HomePage));
        }
        
        private void GoToSignUpPageClick(object sender, RoutedEventArgs e)
        {
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
    }

    public interface ILogInView
    {
        void GoToHomePage();
    }
}
