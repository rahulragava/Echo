using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.PostView.PollPostView
{
    public sealed partial class PollChoiceUserControl : UserControl
    {
        //public readonly PostControlViewModel PostControlViewModel;
        public readonly UserSelectionChoiceViewModel UserSelectionChoiceViewModel;
        public PollChoiceUserControl()
        {
            UserSelectionChoiceViewModel = new UserSelectionChoiceViewModel();
            //PostControlViewModel = new PostControlViewModel();
            this.InitializeComponent();
            Loaded += PollChoiceUserControl_Loaded;
        }

        private void PollChoiceUserControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
            if (ChoicePercentageProperty != null)
            {
                //PostControlViewModel.GetCount(ChoiceSelectedUserList.Count, ChoicePercentage);
            }
        }

        //public string CalculateUserSelectionPercentage()
        //{
        //    return " ";
        //    //(PollChoice.ChoiceSelectedUsers * 100) / /*totalVotes*/;
        //}

        public static readonly DependencyProperty ChoiceProperty = DependencyProperty.Register(
            nameof(Choice), typeof(string), typeof(PollChoiceUserControl), new PropertyMetadata(default(string)));

        public string Choice
        {
            get => (string)GetValue(ChoiceProperty);
            set => SetValue(ChoiceProperty, value);
        }

        public static readonly DependencyProperty ChoiceSelectedUserListProperty = DependencyProperty.Register(
            nameof(ChoiceSelectedUserList), typeof(List<UserPollChoiceSelection>), typeof(PollChoiceUserControl), new PropertyMetadata(default(List<UserPollChoiceSelection>)));

        public List<UserPollChoiceSelection> ChoiceSelectedUserList
        {
            get => (List<UserPollChoiceSelection>)GetValue(ChoiceSelectedUserListProperty);
            set => SetValue(ChoiceSelectedUserListProperty, value);
        }

        public static readonly DependencyProperty ChoicePercentageProperty = DependencyProperty.Register(
            nameof(ChoicePercentage), typeof(int), typeof(PollChoiceUserControl), new PropertyMetadata(default(int)));

        public int ChoicePercentage
        {
            get => (int)GetValue(ChoicePercentageProperty);
            set => SetValue(ChoicePercentageProperty, value);
        }
    }
}
