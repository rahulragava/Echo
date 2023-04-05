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
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Presenter.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialMediaApplication.Presenter.View.PostView.TextPostView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditTextPostUserControl : UserControl, IEditTextPostUserControl
    {
        public EditTextPostViewModel EditTextPostViewModel;
        public EditTextPostUserControl()
        {
            EditTextPostViewModel = new EditTextPostViewModel();
            this.InitializeComponent();
            Loaded += EditTextPostUserControl_Loaded;
        }

        public event Action<TextPostBObj> CloseEdit;  

        private void EditTextPostUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            EditTextPostViewModel.EditTextPostUserControl = this;
            EditTextPostViewModel.TextPostBObj = TextPost;
            EditTextPostViewModel.Content = TextPost.Content;
            //EditTextPostViewModel.OriginalContent = TextPost.Content;
            //EditTextPostViewModel.OriginalFontStyle = TextPost.FontStyle;
            //EditTextPostViewModel.FontStyle = TextPost.FontStyle;

        }

        public static readonly DependencyProperty TextPostProperty = DependencyProperty.Register(
            nameof(TextPost), typeof(TextPostBObj), typeof(EditTextPostUserControl), new PropertyMetadata(default(TextPostBObj)));

        public TextPostBObj TextPost
        {
            get => (TextPostBObj)GetValue(TextPostProperty);
            set => SetValue(TextPostProperty, value);
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            //if (EditText.Text == EditTextPostViewModel.OriginalContent && EditTextPostViewModel.OriginalFontStyle == EditTextPostViewModel.FontStyle)
            //{
            //    Close();
            //}
            //else
            //{
                EditTextPostViewModel.Content = EditText.Text;
                //EditTextPostViewModel.FontStyle = FontStyle.SelectedItem is PostFontStyle fontStyle ? fontStyle : PostFontStyle.Simple;

                EditTextPostViewModel.EditText();
            //}
        }

        //private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    CloseEdit?.Invoke(EditTextPostViewModel.TextPostBObj);
        //}

        public void Close()
        {
            CloseEdit?.Invoke(EditTextPostViewModel.TextPostBObj);
        }

        private void EditText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            Save.IsEnabled = textBox?.Text != EditTextPostViewModel.Content;

            Save.IsEnabled = !string.IsNullOrEmpty(textBox?.Text);

            Save.IsEnabled = !string.IsNullOrWhiteSpace(textBox?.Text);

            //if (sender is TextBox textBox && (!(string.IsNullOrEmpty(textBox.Text)) && !(string.IsNullOrWhiteSpace(textBox.Text))))
            //{
            //    var a = textBox.Text;
            //    Save.IsEnabled = true;
            //}
        }
    }

    public interface IEditTextPostUserControl
    {
        void Close();
    }
}
