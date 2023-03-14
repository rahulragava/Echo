using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.PostView.PollPostView
{
    public sealed partial class PollChoiceSelectedUser : UserControl
    {
        
        public PollChoiceSelectedUser()
        {
            this.InitializeComponent();
        }


        public static readonly DependencyProperty UserPollChoiceSelectionListProperty = DependencyProperty.Register(
            nameof(UserPollChoiceSelectionList), typeof(List<UserPollChoiceSelection>), typeof(PollChoiceSelectedUser), new PropertyMetadata(default(List<UserPollChoiceSelection>)));

        public List<UserPollChoiceSelection> UserPollChoiceSelectionList
        {
            get => (List<UserPollChoiceSelection>)GetValue(UserPollChoiceSelectionListProperty);
            set => SetValue(UserPollChoiceSelectionListProperty, value);
        }


    }
}
