using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Models.EntityModels;
using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.ReactionView
{
    public sealed partial class ReactionUserControl : UserControl
    {
        public ReactionViewModel ReactionViewModel;
        public ReactionUserControl()
        {
            ReactionViewModel = new ReactionViewModel();
            this.InitializeComponent();
        }

        public event Action<Reaction> GetReaction;

        public static readonly DependencyProperty ReactionOnIdProperty = DependencyProperty.Register(
            nameof(ReactionOnId), typeof(string), typeof(ReactionUserControl), new PropertyMetadata(default(string)));

        public string ReactionOnId
        {
            get => (string)GetValue(ReactionOnIdProperty);
            set => SetValue(ReactionOnIdProperty, value);
        }


        private void HeartReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ReactionViewModel.ReactionToPost(ReactionType.Heart,ReactionOnId);
            GetReaction?.Invoke(ReactionViewModel.Reaction);
           
        }

        private void DislikeReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ReactionViewModel.ReactionToPost(ReactionType.ThumbsDown, ReactionOnId);
            GetReaction?.Invoke(ReactionViewModel.Reaction);
        }

        private void LikeReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ReactionViewModel.ReactionToPost(ReactionType.ThumbsUp, ReactionOnId);
            GetReaction?.Invoke(ReactionViewModel.Reaction);
        }


        private void HeartBreakReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ReactionViewModel.ReactionToPost(ReactionType.HeartBreak, ReactionOnId);
            GetReaction?.Invoke(ReactionViewModel.Reaction);
        }

        private void HappyReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ReactionViewModel.ReactionToPost(ReactionType.Happy, ReactionOnId);
            GetReaction?.Invoke(ReactionViewModel.Reaction);

        }

        private void SadReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ReactionViewModel.ReactionToPost(ReactionType.Sad, ReactionOnId);
            GetReaction?.Invoke(ReactionViewModel.Reaction);
        }

        private void MadReaction_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ReactionViewModel.ReactionToPost(ReactionType.Mad, ReactionOnId);
            GetReaction?.Invoke(ReactionViewModel.Reaction);
        }

    }
}
