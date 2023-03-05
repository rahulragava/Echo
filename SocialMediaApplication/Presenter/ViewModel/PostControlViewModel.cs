using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Models.Constant;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class PostControlViewModel : ObservableObject
    {
        public readonly ObservableCollection<Reaction> Reactions = new ObservableCollection<Reaction>();
        public readonly ObservableCollection<CommentBObj> Comments = new ObservableCollection<CommentBObj>();
        public readonly ObservableCollection<PollChoiceBObj> PollChoices= new ObservableCollection<PollChoiceBObj>();
        public Dictionary<ReactionType, int> ReactionCountData = new Dictionary<ReactionType, int>();
        
        public void SetObservableCollection(List<Reaction> reactionList, List<CommentBObj> commentList)
        {
            Reactions.Clear();
            foreach (var reaction in reactionList)
            {
                Reactions.Add(reaction);
            }
            Comments.Clear();
            foreach (var comment in commentList)
            {
                Comments.Add(comment);
            }
            InitializeReactionCount();
        }

        public void SetPollChoiceCollection(List<PollChoiceBObj> pollChoiceList)
        {
            PollChoices.Clear();
            foreach (var choice in pollChoiceList)
            {
                PollChoices.Add(choice);
            }

        }

        public void InitializeReactionCount()
        {
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                var reactionCount = Reactions.Count(r => r.ReactionType == reactionType);
                ReactionCountData.Add(reactionType, reactionCount);
            }

            HappyCount = ReactionCountData[ReactionType.Happy];
            SadCount = ReactionCountData[ReactionType.Sad];
            HeartCount = ReactionCountData[ReactionType.Heart];
            HeartBreakCount = ReactionCountData[ReactionType.HeartBreak];
            ThumbsDownCount = ReactionCountData[ReactionType.ThumbsDown];
            ThumbsUpCount = ReactionCountData[ReactionType.ThumbsUp];
            MadCount = ReactionCountData[ReactionType.Mad];
            ConfusedCount = ReactionCountData[ReactionType.Confused];
        }

        public void SetReactionCount()
        {
            foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
            {
                var reactionCount = Reactions.Count(r => r.ReactionType == reactionType);
                ReactionCountData[reactionType] = reactionCount;
            }

            HappyCount = ReactionCountData[ReactionType.Happy];
            SadCount = ReactionCountData[ReactionType.Sad];
            HeartCount = ReactionCountData[ReactionType.Heart];
            HeartBreakCount = ReactionCountData[ReactionType.HeartBreak];
            ThumbsDownCount = ReactionCountData[ReactionType.ThumbsDown];
            ThumbsUpCount = ReactionCountData[ReactionType.ThumbsUp];
            MadCount = ReactionCountData[ReactionType.Mad];
            ConfusedCount = ReactionCountData[ReactionType.Confused];
        }

        //public void ReactionToPost(ToggleButton toggleButton, string PostId, ReactionType reactionType)
        //{
        //    foreach (ReactionType reactionType in Enum.GetValues(typeof(ReactionType)))
        //    {
        //        switch (reactionType)
        //        {
        //            case ReactionType.Heart:
        //                if (HeartReaction.IsChecked != null && (bool)HeartReaction.IsChecked)
        //                {
        //                    ReactedToPost(ReactionType.Heart, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
        //                    HeartReaction.IsChecked = false;
        //                    return;
        //                }
        //                break;
        //            case ReactionType.Happy:
        //                if (HappyReaction.IsChecked != null && (bool)HappyReaction.IsChecked)
        //                {
        //                    ReactedToPost(ReactionType.Happy, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
        //                    HappyReaction.IsChecked = false;
        //                    return;
        //                }
        //                break;
        //            case ReactionType.HeartBreak:
        //                if (HeartBreakReaction.IsChecked != null && (bool)HeartBreakReaction.IsChecked)
        //                {
        //                    ReactedToPost(ReactionType.HeartBreak, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
        //                    HeartBreakReaction.IsChecked = false;
        //                    return;
        //                }
        //                break;
        //            case ReactionType.Mad:
        //                if (MadReaction.IsChecked != null && (bool)MadReaction.IsChecked)
        //                {
        //                    ReactedToPost(ReactionType.Mad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
        //                    MadReaction.IsChecked = false;
        //                    return;
        //                }
        //                break;
        //            case ReactionType.Sad:
        //                if (SadReaction.IsChecked != null && (bool)SadReaction.IsChecked)
        //                {
        //                    ReactedToPost(ReactionType.Sad, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
        //                    SadReaction.IsChecked = false;
        //                    return;
        //                }
        //                break;
        //            case ReactionType.ThumbsUp:
        //                if (LikeReaction.IsChecked != null && (bool)LikeReaction.IsChecked)
        //                {
        //                    ReactedToPost(ReactionType.ThumbsUp, new Reaction() { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown }, PostId);
        //                    LikeReaction.IsChecked = false;
        //                    return;
        //                }
        //                break;
        //        }
        //    }

        //    //just new reaction is added
        //    if (DisLikeReaction.IsChecked != null && (bool)DisLikeReaction.IsChecked)
        //    {
        //        PostControlViewModel.ReactedToPost(ReactionType.None,
        //            new Reaction()
        //            { ReactedBy = App.UserId, ReactionOnId = PostId, ReactionType = ReactionType.ThumbsDown },
        //            PostId);
        //    }

        //    //reaction by user is removed
        //    else
        //    {
        //        PostControlViewModel.ReactedToPost(ReactionType.None, null, PostId);
        //    }

        //}
        private int _happyCount;

        public int HappyCount
        {
            get => _happyCount;
            set => SetProperty(ref _happyCount, value);
        }

        private int _sadCount;

        public int SadCount
        {
            get => _sadCount;
            set => SetProperty(ref _sadCount, value);
        }

        private int _madCount;

        public int MadCount
        {
            get => _madCount;
            set => SetProperty(ref _madCount, value);
        }

        private int _heartCount;

        public int HeartCount
        {
            get => _heartCount;
            set => SetProperty(ref _heartCount, value);
        }

        private int _heartBreakCount;

        public int HeartBreakCount
        {
            get => _heartBreakCount;
            set => SetProperty(ref _heartBreakCount, value);
        }

        private int _thumbsUpCount;

        public int ThumbsUpCount
        {
            get => _thumbsUpCount;
            set => SetProperty(ref _thumbsUpCount, value);
        }

        private int _thumbsDownCount;

        public int ThumbsDownCount
        {
            get => _thumbsDownCount;
            set => SetProperty(ref _thumbsDownCount, value);
        }

        private int _confusedCount;

        public int ConfusedCount
        {
            get => _confusedCount;
            set => SetProperty(ref _confusedCount, value);
        }

        public void ReactedSuccessfully(List<Reaction> reactionList)
        {
            Reactions.Clear();
            foreach (var reaction in reactionList)
            {
                Reactions.Add(reaction);
            }
            SetReactionCount();
        }
       
        public void ReactedToPost(ReactionType reactionType, Reaction reaction, string reactionOnId)
        {

            var reactionToPostRequest =
                new ReactionToPostRequestObj(reactionType ,reaction, reactionOnId, new ReactionToPostPresenterCallBack(this));
            var reactionToPostUseCase = new ReactionToPostUseCase(reactionToPostRequest);
            reactionToPostUseCase.Execute();
        }
    }

    //    private string _userName;

    //    public string UserName
    //    {
    //        get => _userName;
    //        set => SetProperty(ref _userName, value);
    //    }

    //    private PollChoiceBObj _pollChoiceBObj;

    //    public PollChoiceBObj PollChoiceBObj
    //    {
    //        get => _pollChoiceBObj;
    //        set => SetProperty(ref _pollChoiceBObj, value);
    //    }

    //    private double _totalVotes;

    //    public double TotalVotes
    //    {
    //        get => _totalVotes;
    //        set => SetProperty(ref _totalVotes, value);
    //    }

    //    //public string GetTotalVotes(PollPostBObj pollPostBObj)
    //    //{
    //    //    var totalVotes = pollPostBObj.Choices.Aggregate(0.0, (current, choice) => current + choice.ChoiceSelectedUsers.Count);

    //    //    return totalVotes.ToString();
    //    //}

    //    public void GetUserName(string userId)
    //    {
    //        var cts = new CancellationTokenSource();

    //        var getUserNameRequestObj = new GetUserNameRequestObj(userId, new GetUserNamePresenterCallBack(this), cts.Token);
    //        var getUserNameUseCase = new GetUserNameUseCase(getUserNameRequestObj);
    //        getUserNameUseCase.Execute();
    //    }

    //    public void GetUserNameSucceed(GetUserNameResponseObj getUserNameResponseObj)
    //    {
    //        UserName = getUserNameResponseObj.UserName;
    //    }
    //}

    //public class GetUserNamePresenterCallBack : IPresenterCallBack<GetUserNameResponseObj>
    //{
    //    private readonly PostControlViewModel _postControlViewModel;

    //    public GetUserNamePresenterCallBack(PostControlViewModel postControlViewModel)
    //    {
    //        _postControlViewModel = postControlViewModel;
    //    }

    //    public void OnSuccess(GetUserNameResponseObj getUserNameResponse)
    //    {
    //        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
    //            () =>
    //            {
    //                _postControlViewModel.GetUserNameSucceed(getUserNameResponse);
    //            }
    //        );
    //    }

    //    public void OnError(Exception ex)
    //    {
    //        throw new NotImplementedException();
    //    }
    public class ReactionToPostPresenterCallBack: IPresenterCallBack<ReactionToPostResponse>
    {
        private readonly PostControlViewModel _postControlViewModel;

        public ReactionToPostPresenterCallBack(PostControlViewModel postControlViewModel)
        {
            _postControlViewModel = postControlViewModel;
        }

        public void OnSuccess(ReactionToPostResponse reactionToPostResponse)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _postControlViewModel.ReactedSuccessfully(reactionToPostResponse.Reactions);
                }
            );
        }

        public void OnError(Exception ex)
        {
            // Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            //    () =>
            //    {
            //        _postControlViewModel.ReactedSuccessfully(reactionToPostResponse.Reactions);
            //    }
            //);
            throw new NotImplementedException();
        }
    }
}
