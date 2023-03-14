using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Util;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginInPage : Page
    {
        private readonly LogInViewModel _loginViewModel;
        public LoginInPage()
        {
            this.InitializeComponent();
            _loginViewModel = new LogInViewModel();
            _loginViewModel.GoToHomePageEventHandler += GoToHomePage;
        }
        


        private void GoToHomePage(object sender, EventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }
        
        private void GoToSignUpPageClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignUpPage));
        }

        private void RevealPassword(object sender, TappedRoutedEventArgs e)
        {
            PasswordBox.PasswordRevealMode = _loginViewModel.IsPasswordReveal
                ? PasswordRevealMode.Visible
                : PasswordRevealMode.Hidden;
            _loginViewModel.IsPasswordReveal = !_loginViewModel.IsPasswordReveal;
        }

    }
}
