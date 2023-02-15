using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.Presenter.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginInPage : Page
    {
        public readonly LogInViewModel LoginViewModel;

        //public string abc;// = LoginViewModel.PasswordErrorMessage;
        
        public LoginInPage()
        {
            this.InitializeComponent();
            LoginViewModel = new LogInViewModel();
            //abc = LoginViewModel.PasswordErrorMessage;
        }

        //private Visibility _visibility = Visibility.Collapsed;
        //public Visibility UserNameTextVisibility
        //{
        //    get => _visibility;
        //    set => SetField(ref _visibility, value);
        //}
        //private void PasswordRevealHideCheckBox_OnChecked(object sender, RoutedEventArgs e)
        //{
        //    UserPasswordBox.PasswordRevealMode = PasswordRevealHideCheckBox.IsChecked == true
        //        ? PasswordRevealMode.Visible
        //        : PasswordRevealMode.Hidden;
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        //    field = value;
        //    OnPropertyChanged(propertyName);
        //    return true;
        //}

       

        private void GoToSignUpPageClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignUpPage));
        }

        private void RevealPassword(object sender, TappedRoutedEventArgs e)
        {
            PasswordBox.PasswordRevealMode = LoginViewModel.IsPasswordReveal
                ? PasswordRevealMode.Visible
                : PasswordRevealMode.Hidden;
            LoginViewModel.IsPasswordReveal = !LoginViewModel.IsPasswordReveal;
        }

        public Visibility SetVisibility(Visibility value)
        {
            return value == Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
