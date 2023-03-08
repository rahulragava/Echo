using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Util;
using SocialMediaApplication.Presenter.View;
using static System.Collections.Specialized.BitVector32;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class PostControlViewModel : ObservableObject
    {
        public readonly ObservableCollection<Reaction> Reactions = new ObservableCollection<Reaction>();
        public List<CommentBObj> CommentsList;
        public readonly ObservableCollection<CommentBObj> Comments = new ObservableCollection<CommentBObj>();
        public readonly ObservableCollection<PollChoiceBObj> PollChoices = new ObservableCollection<PollChoiceBObj>();
        public Dictionary<ReactionType, int> ReactionCountData = new Dictionary<ReactionType, int>();

        private Reaction _reaction;

        public Reaction Reaction
        {
            get => _reaction;
            set => SetProperty(ref _reaction, value);
        }

        public string PostId { get; set; }

        private int _totalVotes = 0;

        public int TotalVotes
        {
            get => _totalVotes;
            set => SetProperty(ref _totalVotes, value);
        }

        public void SetObservableCollection(List<Reaction> reactionList, List<CommentBObj> commentList, string postId)
        {
            Reactions.Clear();
            foreach (var reaction in reactionList)
            {
                Reactions.Add(reaction);
            }

            CommentsList = commentList;
            Comments.Clear();
            foreach (var comment in commentList)
            {
                Comments.Add(comment);
            }

            var react = Reactions.SingleOrDefault(r => r.ReactionOnId == postId && r.ReactedBy == AppSettings.UserId);
            if (react != null)
            {
                Reaction = react;
                switch (Reaction.ReactionType)
                {
                    case ReactionType.Heart:
                        ReactionIcon = "♥";
                        break;
                    case ReactionType.ThumbsDown:
                        ReactionIcon = "👎";
                        break;
                    case ReactionType.ThumbsUp:
                        ReactionIcon = "👍";
                        break;
                    case ReactionType.Happy:
                        ReactionIcon = "😁";
                        break;
                    case ReactionType.Mad:
                        ReactionIcon = "😡";
                        break;
                    case ReactionType.HeartBreak:
                        ReactionIcon = "💔";
                        break;
                    case ReactionType.Sad:
                        ReactionIcon = "😕";
                        break;
                }
            }
            else
            {
                Reaction = null;
                ReactionIcon = "👍";

            }

        }

        public void SetPollChoiceCollection(List<PollChoiceBObj> pollChoiceList)
        {
            PollChoices.Clear();
            foreach (var choice in pollChoiceList)
            {
                TotalVotes += choice.ChoiceSelectedUsers.Count;
                PollChoices.Add(choice);
            }

            foreach (var choice in pollChoiceList)
            {
                choice.TotalVotes = TotalVotes;
            }
        }

        public void InsertUserSelectedChoice(string postId, UserPollChoiceSelection userPollChoiceSelection)
        {
            var insertUserSelectionChoiceRequest =
                new InsertUserChoiceSelectionRequest(postId, userPollChoiceSelection,
                    new PostControlPresenterCallBack(this));
            var insertUserSelectionChoiceUseCase =
                new InsertUserSelectionChoiceUseCase(insertUserSelectionChoiceRequest);
            insertUserSelectionChoiceUseCase.Execute();
        }

        public void ClearAndUpdate()
        {
            Comments.Clear();
            foreach (var comment in CommentsList)
            {
                Comments.Add(comment);
            }
        }

        //public void GetUserReaction()
        //{
        //    var userId = AppSettings.UserId;
        //    var postId = PostId;
        //    var getReactionRequest = new GetUserReactionRequest(userId, postId, new GetReactionPresenterCallBack(this));
        //    var getReactionUseCase = new GetUserReactionUseCase(getReactionRequest);
        //    getReactionUseCase.Execute();

        //}

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

        public int GetCount(int choiceSelectedUserListCount, int totalVotes)
        {
            if (totalVotes > 0)
            {
                return ((int)((choiceSelectedUserListCount * 100) / totalVotes));

            }
            else
            {
                return 0;
            }
        }

        public void GetComments()
        {
            var getCommentRequest = new GetCommentRequest(PostId, new CommentsPresenterCallBack(this));
            var getCommentUseCase = new GetCommentUseCase(getCommentRequest);
            getCommentUseCase.Execute();
        }

        private string _reactionIcon;

        public string ReactionIcon
        {
            get => _reactionIcon;
            set => SetProperty(ref _reactionIcon, value);
        }

        public void GetCommentSuccess(List<CommentBObj> commentBObjs)
        {
            CommentsList = commentBObjs;
            ClearAndUpdate();
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

        public class PostControlPresenterCallBack : IPresenterCallBack<InsertUserChoiceSelectionResponse>
        {
            private readonly PostControlViewModel _postControlViewModel;

            public PostControlPresenterCallBack(PostControlViewModel postControlViewModel)
            {
                _postControlViewModel = postControlViewModel;
            }

            public void OnSuccess(InsertUserChoiceSelectionResponse logInResponse)
            {
            }

            public void OnError(Exception ex)
            {
            }
        }

        public class GetReactionPresenterCallBack : IPresenterCallBack<GetUserReactionResponse>
        {
            private readonly PostControlViewModel _postControlViewModel;

            public GetReactionPresenterCallBack(PostControlViewModel postControlViewModel)
            {
                _postControlViewModel = postControlViewModel;
            }

            public void OnSuccess(GetUserReactionResponse getUserReactionResponse)
            {
                //_postControlViewModel.Reaction = getUserReactionResponse.Reaction;
                //_postControlViewModel.SetGlyph();
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public class CommentsPresenterCallBack : IPresenterCallBack<GetCommentResponse>
        {
            private readonly PostControlViewModel _postControlViewModel;

            public CommentsPresenterCallBack(PostControlViewModel postControlViewModel)
            {
                _postControlViewModel = postControlViewModel;
            }

            public void OnSuccess(GetCommentResponse getCommentResponse)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _postControlViewModel.GetCommentSuccess(getCommentResponse.Comments);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }
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

    }
}
