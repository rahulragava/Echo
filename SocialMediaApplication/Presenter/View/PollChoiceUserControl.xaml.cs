using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View
{
    public sealed partial class PollChoiceUserControl : UserControl
    {
        public readonly PostControlViewModel PostControlViewModel;
        public PollChoiceUserControl()
        {
            PostControlViewModel = new PostControlViewModel();
            this.InitializeComponent();
            Loaded += PollChoiceUserControl_Loaded;

        }

        private void PollChoiceUserControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
            if (TotalVotesProperty != null)
            {
                PostControlViewModel.GetCount(ChoiceSelectedUserList.Count, TotalVotes);
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

        public static readonly DependencyProperty TotalVotesProperty = DependencyProperty.Register(
            nameof(TotalVotes), typeof(int), typeof(PollChoiceUserControl), new PropertyMetadata(default(int)));

        public int TotalVotes
        {
            get => (int)GetValue(TotalVotesProperty);
            set => SetValue(TotalVotesProperty, value);
        }

        
    }
}
