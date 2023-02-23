using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View
{
    public sealed partial class TextPostUserControl : UserControl
    {
        public PostControlViewModel PostControlViewModel;
        public TextPostBObj TextPost => this.DataContext as TextPostBObj;

        public TextPostUserControl()
        {
            PostControlViewModel = new PostControlViewModel();
            this.InitializeComponent();
            this.DataContextChanged += (s,e) => Bindings.Update();
            Loaded += PostControl_Loaded;
        }

        private void PostControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PostControlViewModel.GetUserName(TextPost.PostedBy);
            Bindings.Update();
        }

        // Handles the Click event on the Button on the page and opens the Popup. 
        //private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        //{
        //    // open the Popup if it isn't open already 
        //    if (!CommentPopUp.IsOpen) { CommentPopUp.IsOpen = true; }
        //}

        //private void ClosePopupClicked(object sender, RoutedEventArgs e)
        //{
        //    // if the Popup is open, then close it 
        //    if (CommentPopUp.IsOpen) { CommentPopUp.IsOpen = false; }
        //}



    }
}
