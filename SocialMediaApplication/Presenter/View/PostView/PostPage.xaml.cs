using System;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;
using Windows.ApplicationModel.Resources;
using System.Resources;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View.PostView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PostPage : Page
    {
        public PostCreationPageViewModel PostCreationPageViewModel;
        public PostPage()
        {
            PostCreationPageViewModel = new PostCreationPageViewModel();
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            PostCreationPageViewModel.GetUser();
        }

        private void TextPostCreation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            SplitView.IsPaneOpen = true;
            FontStyleComboBox.SetValue(Grid.ColumnSpanProperty, 2);
            PostContentTextBox.SetValue(TextBox.MaxLengthProperty, 200);
            PostContentTextBox.SetValue(HeightProperty, 150);
            TextPostPreview.Visibility = Visibility.Visible;
            PollPostPreview.Visibility = Visibility.Collapsed;
            TextPostCreation.Visibility = Visibility.Collapsed;
            PollPostCreation.Visibility = Visibility.Collapsed;
            PollChoiceCount.Visibility = Visibility.Collapsed;
            CreateTextPostButton.Visibility = Visibility.Visible;
            CreatePollPostButton.Visibility = Visibility.Collapsed;
            PollChoiceContents.Visibility = Visibility.Collapsed;
        }

        private void PollPostCreation_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            SplitView.IsPaneOpen = true;
            FontStyleComboBox.SetValue(Grid.ColumnSpanProperty,1);
            PostContentTextBox.SetValue(TextBox.MaxLengthProperty,92);
            PostContentTextBox.SetValue(TextBox.HeightProperty,75);
            TextPostPreview.Visibility = Visibility.Collapsed;
            PollPostPreview.Visibility = Visibility.Visible;
            TextPostCreation.Visibility = Visibility.Collapsed;
            PollPostCreation.Visibility = Visibility.Collapsed;
            CreateTextPostButton.Visibility = Visibility.Collapsed;
            CreatePollPostButton.Visibility = Visibility.Visible;
            PollChoiceCount.Visibility = Visibility.Visible;
            PollChoiceContents.Visibility = Visibility.Visible;
            ThirdChoice.Visibility = Visibility.Collapsed;
            FourthChoice.Visibility = Visibility.Collapsed;
            //if (Window.Current.Bounds.Width < 800)
            //{
            //    PollPostPreview.Visibility = Visibility.Collapsed;
            //    SplitView.HorizontalAlignment = HorizontalAlignment.Center;
            //}

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
                    FourthChoice.Text = string.Empty;
                    ThirdChoice.Text = string.Empty;
                    ChoiceGrid3.Visibility = Visibility.Collapsed;
                    ChoiceGrid4.Visibility = Visibility.Collapsed;
                    Choice4.Text = string.Empty;
                    Choice3.Text = string.Empty;

                    break;
                case 1:
                    ChoiceGrid4.Visibility = Visibility.Collapsed;
                    Choice4.Text = string.Empty;
                    ChoiceGrid3.Visibility = Visibility.Visible;
                    ThirdChoice.Visibility = Visibility.Visible;
                    FourthChoice.Visibility = Visibility.Collapsed;
                    FourthChoice.Text = string.Empty;
                    break;

                case 2:
                    ChoiceGrid3.Visibility = Visibility.Visible;
                    ThirdChoice.Visibility = Visibility.Visible;
                    ChoiceGrid4.Visibility = Visibility.Visible;
                    FourthChoice.Visibility = Visibility.Visible; 
                    break;

            }
        }

        private void PollChoiceCount_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox) comboBox.SelectedIndex = 0;
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
            TextPostCreation.Visibility = Visibility.Visible;
            PollPostCreation.Visibility = Visibility.Visible;
            PollPostPreview.Visibility = Visibility.Collapsed;
            PostCreationPageViewModel.DiscardButtonClicked();
        }

        private void CreateTextPostButton_OnClick(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            // if the Popup is open, then close it 
            if (string.IsNullOrEmpty(PostContentTextBox.Text))
            {
                ExampleVsCodeInAppNotification.Show(resourceLoader.GetString("PostContentEmptyNotification"), 5000);
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
            ExampleVsCodeInAppNotification.Show(resourceLoader.GetString("TextPostCreatedNotification"), 2000);


        }

        private void CreatePollPostButton_OnClick(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            // if the Popup is open, then close it 
            if (string.IsNullOrEmpty(PostContentTextBox.Text))
            {
                ExampleVsCodeInAppNotification.Show(resourceLoader.GetString("PostQuestionEmptyNotification"), 5000);
                return;

            }

            var choiceEmpty = resourceLoader.GetString("ChoiceEmptyNotification");
            if (FirstChoice.Visibility == Visibility.Visible && string.IsNullOrEmpty(FirstChoice.Text))
            {
                ExampleVsCodeInAppNotification.Show(choiceEmpty, 5000);
                return;
            }
            if (SecondChoice.Visibility == Visibility.Visible && string.IsNullOrEmpty(SecondChoice.Text))
            {
                ExampleVsCodeInAppNotification.Show(choiceEmpty, 5000);
                return;
            }
            if (ThirdChoice.Visibility == Visibility.Visible && string.IsNullOrEmpty(ThirdChoice.Text))
            {
                ExampleVsCodeInAppNotification.Show(choiceEmpty, 5000);
                return;
            }
            if (FourthChoice.Visibility == Visibility.Visible && string.IsNullOrEmpty(FourthChoice.Text))
            {
                ExampleVsCodeInAppNotification.Show(choiceEmpty, 5000);
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
            ExampleVsCodeInAppNotification.Show(resourceLoader.GetString("PollPostCreatedNotification"), 2000);
            TextPostPreview.Visibility = Visibility.Collapsed;
            PollPostPreview.Visibility = Visibility.Collapsed;
            TextPostCreation.Visibility = Visibility.Visible;
            PollPostCreation.Visibility = Visibility.Visible;
        }

        private void PostPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is Page page && page.ActualWidth < 800)
            {
                FontStyleComboBox.SetValue(Grid.ColumnSpanProperty, 2);
                FontStyleComboBox.SetValue(Grid.RowProperty, 4);
                PollChoiceCount.SetValue(Grid.RowProperty, 5);
                PollChoiceCount.SetValue(Grid.ColumnProperty, 0);
                PollChoiceCount.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else
            {
                FontStyleComboBox.SetValue(Grid.ColumnSpanProperty, 1);
                FontStyleComboBox.SetValue(Grid.RowProperty, 4);
                PollChoiceCount.SetValue(Grid.RowProperty, 4);
                PollChoiceCount.SetValue(Grid.ColumnProperty, 1);
                PollChoiceCount.SetValue(Grid.ColumnSpanProperty, 1);
            }
        }
    }
}
