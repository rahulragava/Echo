using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using SocialMediaApplication.Services.Contract;

namespace SocialMediaApplication.Domain.UseCase
{
    public class FetchFeedUseCase : UseCaseBase<FetchFeedResponse>
    {
        public FeedManager FeedManager = FeedManager.GetInstance;
        public FetchFeedRequest FetchFeedRequest;
        public FetchFeedUseCase(FetchFeedRequest fetchFeedRequest)
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
            _fetchFeedUseCase?.FetchFeedRequest?.FetchFeedPresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            //_fetchFeedUseCase?.FetchFeedRequest?.FetchFeedPresenterCallBack?.OnSuccess(responseObj);
        }
    }

    public class FetchFeedRequest
    {
        public IPresenterCallBack<FetchFeedResponse> FetchFeedPresenterCallBack;
        public int PollPostFeedsFetched;
        public int PollPostFeedsSkipped;
        public int TextPostFeedsFetched;
        public int TextPostFeedsSkipped;

        public FetchFeedRequest(int pollPostFeedsFetched, int pollPostFeedsSkipped, int textPostFeedsFetched, int textPostFeedsSkipped, IPresenterCallBack<FetchFeedResponse> fetchFeedPresenterCallBack)
        {
            FetchFeedPresenterCallBack = fetchFeedPresenterCallBack;
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
