using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using SocialMediaApplication.Presenter.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        
        private readonly ProfilePageViewModel _profilePageViewModel;
        
        public ProfilePage()
        {
            _profilePageViewModel= new ProfilePageViewModel();
            this.InitializeComponent();
            Loaded += ProfilePage_Loaded;
        }

        private void ProfilePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _profilePageViewModel.GetUser();
            Bindings.Update();
        }


        // Handles the Click event on the Button on the page and opens the Popup. 
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!EditPopUp.IsOpen) { EditPopUp.IsOpen = true; }

        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (EditPopUp.IsOpen) { EditPopUp.IsOpen = false; }

            _profilePageViewModel.EditUserProfile();
        }

        private void PostTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems[0] is ComboBoxItem comboBoxItem)) return;
            var content = comboBoxItem.Content as string;
            if (content != null && content.Equals("TextPost"))
            {
                if ((PollPostListView is null) || (TextPostListView is null)) return;
                PollPostListView.Visibility = Visibility.Collapsed;
                TextPostListView.Visibility = Visibility.Visible;
            }
            else if (content != null && content.Equals("PollPost"))
            {
                if ((PollPostListView is null) || (TextPostListView is null)) return;
                TextPostListView.Visibility = Visibility.Collapsed;
                PollPostListView.Visibility = Visibility.Visible;
            }
           
        }
    }

}
