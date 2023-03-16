using SocialMediaApplication.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using SocialMediaApplication.Domain.UseCase;
using Windows.UI.Xaml.Controls;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class FeedPageViewModel : ObservableObject
    {
        public ObservableCollection<TextPostBObj> TextPosts;
        public ObservableCollection<PollPostBObj> PollPosts;
        public ObservableCollection<PostBObj> PostBObjs;
        public ObservableCollection<Reaction> Reactions;
        public bool Success = true;

        public FeedPageViewModel()
        {
            TextPosts = new ObservableCollection<TextPostBObj>();
            PollPosts = new ObservableCollection<PollPostBObj>();
            Reactions = new ObservableCollection<Reaction>();
            PostBObjs = new ObservableCollection<PostBObj>();
        }
        private double _scrollPosition;

        public double ScrollPosition
        {
            get => _scrollPosition;
            set => SetProperty(ref _scrollPosition,value);
        }

        public void SetReactions(List<Reaction> reactions)
        {
            Reactions.Clear();
            foreach (var reaction in reactions)
            {
                Reactions.Add(reaction);
            }
        }

        private Reaction _reaction;

        public Reaction Reaction
        {
            get => _reaction;
            set => SetProperty(ref _reaction, value);
        }

        public void ChangeInReactions(List<Reaction> reactions)
        {
            var flag = false;
            foreach (var reaction in reactions)
            {
                if (reaction.ReactedBy == Reaction.ReactedBy)
                {
                    Reactions.Remove(reaction);
                    Reactions.Add(Reaction);
                    flag = true;
                }
            }
            if (!flag)
            {
                Reactions.Add(Reaction);
            }
            flag = false;
        }

        public int PollAmountToBeFetched = 5;
        public int PollAmountToBeSkipped = 0;
        public int TextAmountToBeFetched = 5;
        public int TextAmountToBeSkipped = 0;

        public void GetFeeds()
        {

            //int pollPostAmountToBeFetched ;
            //int pollPostAmountToBeSkipped = 0;

            //int textPostAmountToBeFetched = 0;
            //int textPostAmountToBeSkipped = 0;
            Success = false;
            var fetchFeedRequest = new FetchFeedRequest(PollAmountToBeFetched, PollAmountToBeSkipped, TextAmountToBeFetched, TextAmountToBeSkipped, new FeedPageViewModelPresenterCallBack(this));
            var fetchFeedUseCase = new FetchFeedUseCase(fetchFeedRequest);
            fetchFeedUseCase.Execute();

        }

        public class FeedPageViewModelPresenterCallBack : IPresenterCallBack<FetchFeedResponse>
        {
            private readonly FeedPageViewModel _feedPageViewModel;
            public FeedPageViewModelPresenterCallBack(FeedPageViewModel feedPageViewModel)
            {
                _feedPageViewModel = feedPageViewModel;
            }

            public void OnSuccess(FetchFeedResponse fetchFeedResponse)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        if (fetchFeedResponse.TextPosts.Count == 0 && fetchFeedResponse.PollPosts.Count == 0)
                        {
                            return;
                        }
                        else
                        {
                            foreach (var textPost in fetchFeedResponse.TextPosts)
                            {
                                _feedPageViewModel.TextPosts.Add(textPost);
                                _feedPageViewModel.PostBObjs.Add(textPost);
                            }
                            foreach (var pollPost in fetchFeedResponse.PollPosts)
                            {
                                _feedPageViewModel.PollPosts.Add(pollPost);
                                _feedPageViewModel.PostBObjs.Add(pollPost);

                            }

                            _feedPageViewModel.PollAmountToBeSkipped += 5;
                            _feedPageViewModel.TextAmountToBeSkipped += 5;
                        }

                        _feedPageViewModel.Success = true;
                    }
                );
            }

            public void OnError(Exception ex)
            {
            }
        }
    }
}
