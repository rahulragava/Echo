using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SocialMediaApplication.Models.EntityModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.CommentView
{
    public sealed partial class CommentUserControl : UserControl
    {
        public CommentUserControl()
        {
            this.InitializeComponent();
        }

        //public Thickness Thickness

        public static readonly DependencyProperty CommentedByUserNameProperty = DependencyProperty.Register(
            nameof(CommentedByUserName), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string CommentedByUserName
        {
            get => (string)GetValue(CommentedByUserNameProperty);
            set => SetValue(CommentedByUserNameProperty, value);
        }

        public static readonly DependencyProperty CommentContentProperty = DependencyProperty.Register(
            nameof(CommentContent), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string CommentContent
        {
            get => (string)GetValue(CommentContentProperty);
            set => SetValue(CommentContentProperty, value);
        }

        public static readonly DependencyProperty CommentedAtProperty = DependencyProperty.Register(
            nameof(CommentedAt), typeof(string), typeof(CommentUserControl), new PropertyMetadata(default(string)));

        public string CommentedAt
        {
            get => (string)GetValue(CommentedAtProperty);
            set => SetValue(CommentedAtProperty, value);
        }

        public static readonly DependencyProperty CommentDepthProperty = DependencyProperty.Register(
            nameof(CommentDepth), typeof(int), typeof(CommentUserControl), new PropertyMetadata(default(int)));

        public int CommentDepth
        {
            get => (int)GetValue(CommentDepthProperty);
            set => SetValue(CommentDepthProperty, value);
        }

        public static readonly DependencyProperty CommentReactionsProperty = DependencyProperty.Register(
            nameof(CommentReactions), typeof(List<Reaction>), typeof(CommentUserControl), new PropertyMetadata(default(List<Reaction>)));

        public List<Reaction> CommentReactions
        {
            get => (List<Reaction>)GetValue(CommentReactionsProperty);
            set => SetValue(CommentReactionsProperty, value);
        }
    }
}
