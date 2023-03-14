using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class PostPage : Page
    {
        public PostCreationPageViewModel PostCreationPageViewModel;
        public PostPage()
        {
            this.InitializeComponent();
            PostCreationPageViewModel = new PostCreationPageViewModel();
        }

        private void TextPostCreation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            SplitView.IsPaneOpen = true;
            TextPostPreview.Visibility = Visibility.Visible;
            PollPostPreview.Visibility = Visibility.Collapsed;
            TextPostCreation.Visibility = Visibility.Visible;
            PollPostCreation.Visibility = Visibility.Visible;
            PollChoiceCount.Visibility = Visibility.Collapsed;
            CreateTextPostButton.Visibility = Visibility.Visible;
            CreatePollPostButton.Visibility = Visibility.Collapsed;
            PollChoiceContents.Visibility = Visibility.Collapsed;
        }

        private void PollPostCreation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            SplitView.IsPaneOpen = true;
            TextPostPreview.Visibility = Visibility.Collapsed;
            PollPostPreview.Visibility = Visibility.Visible;
            TextPostCreation.Visibility = Visibility.Visible;
            PollPostCreation.Visibility = Visibility.Visible;
            CreateTextPostButton.Visibility = Visibility.Collapsed;
            CreatePollPostButton.Visibility = Visibility.Visible;
            PollChoiceCount.Visibility = Visibility.Visible;
            PollChoiceContents.Visibility = Visibility.Visible;
            ThirdChoice.Visibility = Visibility.Collapsed;
            FourthChoice.Visibility = Visibility.Collapsed;
            
        }

        private void PollChoiceCount_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int pollChoiceCount = 0;
            if (PollChoiceCount.SelectedItem != null)
            {
                pollChoiceCount = PollChoiceCount.SelectedIndex;
            }

            switch (pollChoiceCount)
            {
                case 0:
                    FirstChoice.Visibility = Visibility.Visible;
                    SecondChoice.Visibility = Visibility.Visible;
                    ThirdChoice.Visibility = Visibility.Collapsed;
                    FourthChoice.Visibility = Visibility.Collapsed;
                    FourthChoice.Text = String.Empty;
                    ThirdChoice.Text = String.Empty;
                    ChoiceGrid3.Visibility = Visibility.Collapsed;
                    ChoiceGrid4.Visibility = Visibility.Collapsed;
                    choice4.Text = string.Empty;
                    choice3.Text = string.Empty;

                    break;
                case 1:
                    ChoiceGrid4.Visibility = Visibility.Collapsed;
                    choice4.Text = string.Empty;
                    ChoiceGrid3.Visibility = Visibility.Visible;
                    ThirdChoice.Visibility = Visibility.Visible;
                    FourthChoice.Visibility = Visibility.Collapsed;
                    FourthChoice.Text = String.Empty;
                    break;

                case 2:
                    ChoiceGrid4.Visibility = Visibility.Visible;
                    FourthChoice.Visibility = Visibility.Visible; 
                    break;

            }
        }

        private void PollChoiceCount_OnLoaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.SelectedIndex = 0;
            FirstChoice.Visibility = Visibility.Visible;
            SecondChoice.Visibility = Visibility.Visible;
            ThirdChoice.Visibility = Visibility.Collapsed;
            FourthChoice.Visibility = Visibility.Collapsed;
        }

        

        private void DiscardButton_OnClick(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = false;
            ThirdChoice.Visibility = Visibility.Collapsed;
            FourthChoice.Visibility = Visibility.Collapsed;
            TextPostCreation.Visibility = Visibility.Visible;
            PollPostCreation.Visibility = Visibility.Visible;
            TextPostPreview.Visibility = Visibility.Collapsed;
            PollPostPreview.Visibility = Visibility.Collapsed;
            PostCreationPageViewModel.DiscardButtonClicked();

        }

        private void CreateTextPostButton_OnClick(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (string.IsNullOrEmpty(PostTitleTextBox.Text))
            {
                ExampleVsCodeInAppNotification.Show("Post title cannot be empty!", 5000);
                return;

            }
            if (string.IsNullOrEmpty(PostContentTextBox.Text))
            {
                ExampleVsCodeInAppNotification.Show("Post content cannot be empty!", 5000);
                return;

            }


            SplitView.IsPaneOpen = false;
            ThirdChoice.Visibility = Visibility.Collapsed;
            FourthChoice.Visibility = Visibility.Collapsed;
            TextPostCreation.Visibility = Visibility.Visible;
            PollPostCreation.Visibility = Visibility.Visible;
            TextPostPreview.Visibility = Visibility.Collapsed;
            PollPostPreview.Visibility = Visibility.Collapsed;
            PostCreationPageViewModel.CreateTextPost();
            ExampleVsCodeInAppNotification.Show("Text post is Successfully Created!", 2000);


        }

        private void CreatePollPostButton_OnClick(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (string.IsNullOrEmpty(PostTitleTextBox.Text))
            {
                ExampleVsCodeInAppNotification.Show("Post title cannot be empty!", 5000);
                return;

            }
            if (string.IsNullOrEmpty(PostContentTextBox.Text))
            {
                ExampleVsCodeInAppNotification.Show("Poll question cannot be empty!", 5000);
                return;

            }

            if (FirstChoice.Visibility == Visibility.Visible && string.IsNullOrEmpty(FirstChoice.Text))
            {
                ExampleVsCodeInAppNotification.Show("Choice field cannot be empty!", 5000);
                return;
            }
            if (SecondChoice.Visibility == Visibility.Visible && string.IsNullOrEmpty(SecondChoice.Text))
            {
                ExampleVsCodeInAppNotification.Show("Choice field cannot be empty!", 5000);
                return;
            }
            if (ThirdChoice.Visibility == Visibility.Visible && string.IsNullOrEmpty(ThirdChoice.Text))
            {
                ExampleVsCodeInAppNotification.Show("Choice field cannot be empty!", 5000);
                return;
            }
            if (FourthChoice.Visibility == Visibility.Visible && string.IsNullOrEmpty(FourthChoice.Text))
            {
                ExampleVsCodeInAppNotification.Show("Choice field cannot be empty!", 5000);
                return;
            }

            int index = 0;
            int pollChoiceCount = 2;
            if (PollChoiceCount.SelectedItem != null)
            {
                index= PollChoiceCount.SelectedIndex;
            }

            switch (index)
            {
                case 0:
                    pollChoiceCount = 2;
                    break;
                case 1:
                    pollChoiceCount = 3;
                    break;

                case 2:
                    pollChoiceCount = 4;
                    break;
            }

            SplitView.IsPaneOpen = false;
            ThirdChoice.Visibility = Visibility.Collapsed;
            FourthChoice.Visibility = Visibility.Collapsed;
            PostCreationPageViewModel.CreatePollPost(pollChoiceCount);

            //ForegroundColor = new SolidColorBrush(Colors.GreenYellow);
            ExampleVsCodeInAppNotification.Show("Poll  is Successfully Created!", 2000);
            //ForegroundColor = new SolidColorBrush(Colors.Red)
            TextPostPreview.Visibility = Visibility.Collapsed;
            PollPostPreview.Visibility = Visibility.Collapsed;
            TextPostCreation.Visibility = Visibility.Visible;
            PollPostCreation.Visibility = Visibility.Visible;
        }

        //    private SolidColorBrush _foregroundColor;

        //    public SolidColorBrush ForegroundColor
        //    {
        //        get => _foregroundColor;
        //        set => SetField(ref _foregroundColor, value);
        //    }
        //    public event PropertyChangedEventHandler PropertyChanged;

        //    private void OnPropertyChanged([CallerMemberName] string FollowingIds = null)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(FollowingIds));
        //    }

        //    private bool SetField<T>(ref T field, T value, [CallerMemberName] string FollowingIds = null)
        //    {
        //        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        //        field = value;
        //        OnPropertyChanged(FollowingIds);
        //        return true;
        //    }
    }
}
