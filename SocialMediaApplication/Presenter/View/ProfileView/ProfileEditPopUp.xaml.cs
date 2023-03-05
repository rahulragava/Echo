using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.ProfileView
{
    public sealed partial class ProfileEditPopUp : UserControl
    {
        //public UserBObj UserBObj => DataContext as UserBObj;
        public ProfileEditPopUp()
        {
            this.InitializeComponent();
            //DataContextChanged += (s, e) => Bindings.Update();
        }

        
    }
}
