using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
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
using MessagePack.Resolvers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View
{
    public sealed partial class PollPostUserControl : UserControl
    {
        public PostControlViewModel PostControlViewModel;
        public PollPostBObj PollPost => this.DataContext as PollPostBObj;
        public ObservableCollection<PollChoiceBObj> PollChoices;

        public PollPostUserControl()
        {
            PostControlViewModel = new PostControlViewModel();
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
            Loaded += PollPostControl_Loaded;
        }

        private void PollPostControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PostControlViewModel.GetUserName(PollPost.PostedBy);
            PollChoices = new ObservableCollection<PollChoiceBObj>();
            ClearAndUpdate(PollChoices);
            Bindings.Update();
        }

        private void ClearAndUpdate(ObservableCollection<PollChoiceBObj> pollChoices)
        {
            pollChoices.Clear();
            foreach (var choice in PollPost.Choices)
            {
                pollChoices.Add(choice);
            }
        }

        //public string GetTotalVotes()
        //{
        //    return PostControlViewModel.GetTotalVotes(PollPost);
        //}

    }
}
