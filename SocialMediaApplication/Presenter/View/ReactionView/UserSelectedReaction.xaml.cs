﻿using SocialMediaApplication.Presenter.View.FeedView;
using SocialMediaApplication.Presenter.View.ProfileView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SocialMediaApplication.Presenter.View.ReactionView
{
    public sealed partial class UserSelectedReaction : UserControl
    {
        public UserSelectionReactionsVM UserSelectionReactionsViewModel;
        public UserSelectedReaction()
        {
            UserSelectionReactionsViewModel = new UserSelectionReactionsVM();
            this.InitializeComponent();
            Loaded += UserSelectedReaction_Loaded;
        }

        private void UserSelectedReaction_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ReactionList != null)
            {
                UserSelectionReactionsViewModel.Reactions = ReactionList;
            }
            Happy.Visibility = Visibility.Collapsed;
            Sad.Visibility = Visibility.Collapsed;
            Mad.Visibility = Visibility.Collapsed;
            Like.Visibility = Visibility.Collapsed;
            DisLike.Visibility = Visibility.Collapsed;
            HeartBreak.Visibility = Visibility.Collapsed;
            Heart.Visibility = Visibility.Collapsed;
            foreach (var reaction in UserSelectionReactionsViewModel.Reactions)
            {
                switch (reaction.ReactionType)
                {
                    case ReactionType.Happy:
                        Happy.Visibility = Visibility.Visible;
                        break;
                    case ReactionType.Heart:
                        Heart.Visibility = Visibility.Visible;
                        break;
                    case ReactionType.HeartBreak:
                        HeartBreak.Visibility = Visibility.Visible;
                        break;
                    case ReactionType.Mad:
                        Mad.Visibility = Visibility.Visible;
                        break;
                    case ReactionType.Sad:
                        Sad.Visibility = Visibility.Visible;
                        break;
                    case ReactionType.ThumbsDown:
                        DisLike.Visibility = Visibility.Visible;
                        break;
                    case ReactionType.ThumbsUp:
                        Like.Visibility = Visibility.Visible;
                        break;
                }
            }
            NavigationViewItem itemContent = ReactionNavigationView.MenuItems.ElementAt(0) as NavigationViewItem;
            ReactionNavigationView.SelectedItem = itemContent;
        }

        public static readonly DependencyProperty ReactionListProperty = DependencyProperty.Register(
            nameof(ReactionList), typeof(ObservableCollection<Reaction>), typeof(UserSelectedReaction), new PropertyMetadata(default(List<Reaction>)));

        public ObservableCollection<Reaction> ReactionList
        {
            get => (ObservableCollection<Reaction>)GetValue(ReactionListProperty);
            set => SetValue(ReactionListProperty, value);
        }

        private void ReactionNavigationView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedTag = args.SelectedItemContainer.Tag.ToString();

            switch (selectedTag)
            {
                case "AllReactionTag":
                    UserSelectionReactionsViewModel.SetAllReactedUser();
                    break;

                case "HeartReactionTag":
                    //this.Frame.Navigate(typeof(HomePage));
                    UserSelectionReactionsViewModel.GetAndSetReactionChosenUsers(ReactionType.Heart);
                    break;
                case "LikeReactionTag":
                    UserSelectionReactionsViewModel.GetAndSetReactionChosenUsers(ReactionType.ThumbsUp);
                    break;
                case "DisLikeReactionTag":
                    UserSelectionReactionsViewModel.GetAndSetReactionChosenUsers(ReactionType.ThumbsDown);
                    break;
                case "HappyReactionTag":
                    UserSelectionReactionsViewModel.GetAndSetReactionChosenUsers(ReactionType.Happy);
                    break;
                case "SadReactionTag":
                    UserSelectionReactionsViewModel.GetAndSetReactionChosenUsers(ReactionType.Sad);
                    break;
                case "HeartBreakReactionTag":
                    UserSelectionReactionsViewModel.GetAndSetReactionChosenUsers(ReactionType.HeartBreak);
                    break;
                case "MadReactionTag":
                    UserSelectionReactionsViewModel.GetAndSetReactionChosenUsers(ReactionType.Mad);
                    break;

            }
            ReactionFrame.Navigate(typeof(UserListPage), UserSelectionReactionsViewModel.UserIds);

        }
    }
}
