using SocialMediaApplication.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Domain.UseCase;
using Windows.UI.Xaml.Controls;
using Microsoft.VisualStudio.PlatformUI;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class FeedPageViewModel : ObservableObject
    {
        public ObservableCollection<TextPostBObj> TextPosts;
        public ObservableCollection<PollPostBObj> PollPosts;

        public FeedPageViewModel()
        {
            TextPosts = new ObservableCollection<TextPostBObj>();
            PollPosts = new ObservableCollection<PollPostBObj>();
        }
        private double _scrollPosition;

        public double ScrollPosition
        {
            get => _scrollPosition;
            set => SetProperty(ref _scrollPosition,value);
        }


        public async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            ScrollPosition = scrollViewer.VerticalOffset;
            var maxOffset = scrollViewer.ScrollableHeight;
            if (maxOffset > 0 && ScrollPosition >= maxOffset - 100)
            { 
                GetFeeds(5,5,5,5);
            }
        }
        public void GetFeeds(int pollAmountToBeFetched, int pollAmountToBeSkipped,int textAmountToBeFetched,int textAmountToBeSkipped)
        {

            //int pollPostAmountToBeFetched ;
            //int pollPostAmountToBeSkipped = 0;

            //int textPostAmountToBeFetched = 0;
            //int textPostAmountToBeSkipped = 0;

            var fetchFeedRequest = new FetchFeedRequest(pollAmountToBeFetched, pollAmountToBeSkipped, textAmountToBeFetched, textAmountToBeSkipped, new FeedPageViewModelPresenterCallBack(this));
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

            public void OnSuccess(FetchFeedResponse logInResponse)
            {
            }

            public void OnError(Exception ex)
            {
            }
        }
    }
}
