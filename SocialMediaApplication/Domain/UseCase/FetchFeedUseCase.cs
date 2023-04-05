using System;
using System.Collections.Generic;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;

namespace SocialMediaApplication.Domain.UseCase
{
    public class FetchFeedUseCase : UseCaseBase<FetchFeedResponse>
    {
        public IFeedManager FeedManager = DataManager.FeedManager.GetInstance;
        public FetchFeedRequest FetchFeedRequest;
        public FetchFeedUseCase(FetchFeedRequest fetchFeedRequest, IPresenterCallBack<FetchFeedResponse> fetchFeedPresenterCallBack) : base(fetchFeedPresenterCallBack)
        {
            FetchFeedRequest = fetchFeedRequest;
        }
        public override void Action()
        {
            FeedManager.FetchFeedAsync(FetchFeedRequest, new FetchFeedUseCaseCallBack(this));
        }
    }

    public class FetchFeedUseCaseCallBack : IUseCaseCallBack<FetchFeedResponse>
    {
        private readonly FetchFeedUseCase _fetchFeedUseCase;

        public FetchFeedUseCaseCallBack(FetchFeedUseCase fetchFeedUseCase)
        {
            _fetchFeedUseCase = fetchFeedUseCase;
        }

        public void OnSuccess(FetchFeedResponse responseObj)
        {
            _fetchFeedUseCase?.PresenterCallBack.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _fetchFeedUseCase?.PresenterCallBack.OnError(ex);
        }
    }

    public class FetchFeedRequest
    {
        public int PollPostFeedsFetched;
        public int PollPostFeedsSkipped;
        public int TextPostFeedsFetched;
        public int TextPostFeedsSkipped;

        public FetchFeedRequest(int pollPostFeedsFetched, int pollPostFeedsSkipped, int textPostFeedsFetched, int textPostFeedsSkipped)
        {
            PollPostFeedsFetched = pollPostFeedsFetched;
            PollPostFeedsSkipped = pollPostFeedsSkipped;
            TextPostFeedsFetched = textPostFeedsFetched;
            TextPostFeedsSkipped = textPostFeedsSkipped;
        }
    }

    public class FetchFeedResponse
    {
        public List<TextPostBObj> TextPosts;
        public List<PollPostBObj> PollPosts;

        public FetchFeedResponse(List<TextPostBObj> textPosts, List<PollPostBObj> pollPosts)
        {
            TextPosts = textPosts;
            PollPosts = pollPosts;
        }
    }
}
