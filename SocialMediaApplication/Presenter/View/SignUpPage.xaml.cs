using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.Presenter.ViewModel;
using System.ServiceModel.Channels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : Page
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
            _signUpViewModel.NavigateToLogInPage += GoToLogInPage;
            _signUpViewModel.ErrorMessageNotification += ErrorMessageNotification;
        }

        private void ErrorMessageNotification(string message)
        {
            ExampleVsCodeInAppNotification.Show(message, 5000);
            return;
        }


        private void GotoSignInPageClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginInPage));

        }

        private void GoToLogInPage(object sender, EventArgs e)
        {   
            _signUpViewModel.NavigateToLogInPage -= GoToLogInPage;
            this.Frame.Navigate(typeof(LoginInPage));
        }

        //private void ViewModelSaidDoSomething()
        //{
        //    var vm = (SignUpViewModel)DataContext;
        //    vm.
        //}

        private void RevealPassword(object sender, TappedRoutedEventArgs e)
        {
            var fontIcon = sender as FontIcon;
            if (fontIcon.Name == "PasswordRevealEyeIcon")
            {
                PasswordBox.PasswordRevealMode = _signUpViewModel.IsPasswordVisible
                    ? PasswordRevealMode.Visible
                    : PasswordRevealMode.Hidden;
                _signUpViewModel.IsPasswordVisible = !_signUpViewModel.IsPasswordVisible;
            }
            else if (fontIcon.Name == "RetypePasswordRevealEyeIcon")
            {
                RetypePasswordBox.PasswordRevealMode = _signUpViewModel.IsRetypePasswordVisible
                    ? PasswordRevealMode.Visible
                    : PasswordRevealMode.Hidden;
                _signUpViewModel.IsRetypePasswordVisible = !_signUpViewModel.IsRetypePasswordVisible;
            }
        }
    }
}
