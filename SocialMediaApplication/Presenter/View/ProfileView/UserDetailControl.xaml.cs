using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.Constant;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.ProfileView
{
    public sealed partial class UserDetailControl : UserControl
    {
        public UserDetailControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FirstNameProperty = DependencyProperty.Register(
            nameof(FirstName), typeof(string), typeof(UserDetailControl), new PropertyMetadata(default(string)));

        public string FirstName
        {
            get => (string)GetValue(FirstNameProperty);
            set => SetValue(FirstNameProperty, value);
        }

        public static readonly DependencyProperty LastNameProperty = DependencyProperty.Register(
            nameof(LastName), typeof(string), typeof(UserDetailControl), new PropertyMetadata(default(string)));

        public string LastName
        {
            get => (string)GetValue(LastNameProperty);
            set => SetValue(LastNameProperty, value);
        }

        public static readonly DependencyProperty PlaceProperty = DependencyProperty.Register(
            nameof(Place), typeof(string), typeof(UserDetailControl), new PropertyMetadata(default(string)));

        public string Place
        {
            get => (string)GetValue(PlaceProperty);
            set => SetValue(PlaceProperty, value);
        }

        public static readonly DependencyProperty OccupationProperty = DependencyProperty.Register(
            nameof(Occupation), typeof(string), typeof(UserDetailControl), new PropertyMetadata(default(string)));

        public string Occupation
        {
            get => (string)GetValue(OccupationProperty);
            set => SetValue(OccupationProperty, value);
        }

        public static readonly DependencyProperty EducationProperty = DependencyProperty.Register(
            nameof(Education), typeof(string), typeof(UserDetailControl), new PropertyMetadata(default(string)));

        public string Education
        {
            get => (string)GetValue(EducationProperty);
            set => SetValue(EducationProperty, value);
        }

        public static readonly DependencyProperty MarriageStatusProperty = DependencyProperty.Register(
            nameof(MarriageStatus), typeof(string), typeof(UserDetailControl),
            new PropertyMetadata(default(MaritalStatus)));

        public string MarriageStatus
        {
            get => (string)GetValue(MarriageStatusProperty);
            set => SetValue(MarriageStatusProperty, value);
        }

        public static readonly DependencyProperty UserCreatedAtProperty = DependencyProperty.Register(
            nameof(UserCreatedAt), typeof(string), typeof(UserDetailControl), new PropertyMetadata(default(string)));

        public string UserCreatedAt
        {
            get => (string)GetValue(UserCreatedAtProperty);
            set => SetValue(UserCreatedAtProperty, value);
        }
    }
}