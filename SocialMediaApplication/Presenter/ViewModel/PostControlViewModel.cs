using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
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
using Windows.UI.Xaml;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class PostControlViewModel : ObservableObject
    {
        public readonly ObservableCollection<Reaction> Reactions = new ObservableCollection<Reaction>();
        public List<CommentBObj> CommentsList;
        public readonly ObservableCollection<CommentBObj> Comments = new ObservableCollection<CommentBObj>();
        public  ObservableCollection<PollChoiceBObj> PollChoices = new ObservableCollection<PollChoiceBObj>();

        public Action<string> PostRemoved;
        private PollChoiceBObj _pollChoiceBObj;

        public PollChoiceBObj PollChoiceBObj
        {
            get => _pollChoiceBObj;
            set => SetProperty(ref _pollChoiceBObj, value);
        }
        public UserPollChoiceSelection UserPollChoiceSelection;
        private Reaction _reaction;
        public ObservableCollection<string> UserFollowerList;
        public ObservableCollection<string> UserFollowingList;
        public User User;
        public Reaction Reaction
        {
            get => _reaction;
            set => SetProperty(ref _reaction, value);
        }

        private Visibility _removeButtonVisibility;

        public Visibility RemoveButtonVisibility
        {
            get => _removeButtonVisibility;
            set => SetProperty(ref _removeButtonVisibility, value);
        }
        public string PostId { get; set; }

        public PostControlViewModel()
        {
            UserFollowerList = new ObservableCollection<string>();
            UserFollowingList = new ObservableCollection<string>();
        }
        private int _totalVotes = 0;

        public int TotalVotes
        {
            get => _totalVotes;
            set => SetProperty(ref _totalVotes, value);
        }

        private PostFontStyle _postFontStyle = PostFontStyle.Simple;

        public PostFontStyle PostFontStyle
        {
            get => _postFontStyle;

            set => SetProperty(ref _postFontStyle, value);
        }


        private int _totalComments;
        public int TotalComments
        {
            get => _totalComments;
            set => SetProperty(ref _totalComments, value);
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
            TotalComments = Comments.Count;

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

        public Action GetUserMiniDetailSuccess;
        public void GetMiniProfileDetails(string postedByUser)
        {
            var userMiniDetailRequest = new UserMiniDetailRequest(postedByUser,new UserMiniDetailPresenterCallBack(this)); 
            var userMiniDetailUseCase = new UserMiniDetailUseCase(userMiniDetailRequest);
            userMiniDetailUseCase.Execute();
        }

        public void SetPollChoiceCollection(List<PollChoiceBObj> pollChoiceList)
        {
            PollChoices.Clear();
            foreach (var choice in pollChoiceList)
            {
                TotalVotes += choice.ChoiceSelectedUsers.Count;
                PollChoices.Add(choice);
                foreach (var userSelectionPollChoice in choice.ChoiceSelectedUsers)
                {
                    if (userSelectionPollChoice.SelectedBy == AppSettings.UserId)
                    {
                        PollChoiceBObj = choice;
                    }
                }
            }


            foreach (var choice in PollChoices)
            {
                if (TotalVotes == 0)
                {
                    choice.ChoiceSelectionPercent = 0;
                }
                else
                {
                    choice.ChoiceSelectionPercent = choice.ChoiceSelectedUsers.Count*100/TotalVotes;
                }
            }
        }

       

        public void InsertUserSelectedChoice(string postId, UserPollChoiceSelection userPollChoiceSelection)
        {
            foreach (var pollChoiceSelection in PollChoiceBObj.ChoiceSelectedUsers)
            {
                if (pollChoiceSelection.SelectedBy == AppSettings.UserId)
                {
                    return;
                }
            }
            UserPollChoiceSelection = userPollChoiceSelection;
            var insertUserSelectionChoiceRequest =
                new InsertUserChoiceSelectionRequest(postId, userPollChoiceSelection,
                    new InsertUserSelectionPostChoicePresenterCallBack(this));
            var insertUserSelectionChoiceUseCase = new InsertUserSelectionChoiceUseCase(insertUserSelectionChoiceRequest);
            insertUserSelectionChoiceUseCase.Execute();
        }

        public EventHandler UserSelectionChoiceInserted;
        public void SuccessfullyUserChoiceSelectionInserted(InsertUserChoiceSelectionResponse insertUserChoiceSelectionResponse)
        {
            TotalVotes = 0;
            foreach (var pollChoice in PollChoices)
            {
                if (pollChoice.Id != PollChoiceBObj.Id)
                {
                    var choiceSelectedUser = pollChoice.ChoiceSelectedUsers.SingleOrDefault(c => c.SelectedBy == AppSettings.UserId);
                    if (choiceSelectedUser != null)
                    {
                        pollChoice.ChoiceSelectedUsers.Remove(choiceSelectedUser);
                        //PollChoices[PollChoices.IndexOf(pollChoiceBObj)].ChoiceSelectedUsers.Remove(choiceSelectedUser);
                        PollChoiceBObj.ChoiceSelectedUsers.Remove(choiceSelectedUser);
                    }
                }
                else
                {
                    pollChoice.ChoiceSelectedUsers.Add(UserPollChoiceSelection);
                }

                TotalVotes += pollChoice.ChoiceSelectedUsers.Count;
            }

            foreach (var pollChoiceBObj in PollChoices)
            {
                pollChoiceBObj.ChoiceSelectionPercent = (int)pollChoiceBObj.ChoiceSelectedUsers.Count * 100 / TotalVotes;
            }

            //UserSelectionChoiceInserted?.Invoke(this, EventArgs.Empty);
        }

        public void ClearAndUpdate()
        {
            Comments.Clear();
            foreach (var comment in CommentsList)
            {
                Comments.Add(comment);
            }
        }

        
        public int GetCount(int choiceSelectedUserListCount)
        {
            if (TotalVotes > 0)
            {
                return ((int)((choiceSelectedUserListCount * 100) / TotalVotes));

            }
            else
            {
                return 0;
            }
        }


        public Action FollowUnFollowActionDone;
        public void GetComments()
        {
            var getCommentRequest = new GetCommentRequest(PostId, new CommentsPresenterCallBack(this));
            var getCommentUseCase = new GetCommentUseCase(getCommentRequest);
            getCommentUseCase.Execute();
        }

        public void RemovePost(PostBObj postBObj)
        {
            var removePostRequest = new RemovePostRequest(postBObj,new RemovePostPresenterCallBack(this));
            var removePostUseCase = new RemovePostUseCase(removePostRequest);
            removePostUseCase.Execute();
        }

        public void FollowUnFollowSearchedUser(string userId)
        {

            var followUnFollowSearchedUserRequest = new FollowUnFollowSearchedUserRequest(userId, AppSettings.UserId, new FollowUnFollowPresenterCallBack(this));
            var followUnFollowSearchedUserUseCase =
                new FollowUnFollowSearchedUserUseCase(followUnFollowSearchedUserRequest);
            followUnFollowSearchedUserUseCase.Execute();
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

        public class InsertUserSelectionPostChoicePresenterCallBack : IPresenterCallBack<InsertUserChoiceSelectionResponse>
        {
            private readonly PostControlViewModel _postControlViewModel;

            public InsertUserSelectionPostChoicePresenterCallBack(PostControlViewModel postControlViewModel)
            {
                _postControlViewModel = postControlViewModel;
            }

            public void OnSuccess(InsertUserChoiceSelectionResponse insertUserChoiceSelectionResponse)
            {

                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _postControlViewModel.SuccessfullyUserChoiceSelectionInserted(insertUserChoiceSelectionResponse);
                    }
                );
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

        public class RemovePostPresenterCallBack : IPresenterCallBack<RemovePostResponse>
        {
            private readonly PostControlViewModel _postControlViewModel;

            public RemovePostPresenterCallBack(PostControlViewModel postControlViewModel)
            {
                _postControlViewModel = postControlViewModel;
            }

            public void OnSuccess(RemovePostResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                   () =>
                   {
                       _postControlViewModel.PostRemoved?.Invoke(response.PostId);
                   }
               );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public class UserMiniDetailPresenterCallBack : IPresenterCallBack<UserMiniDetailResponse>
        {
            private readonly PostControlViewModel _postControlViewModel;

            public UserMiniDetailPresenterCallBack(PostControlViewModel postControlViewModel)
            {
                _postControlViewModel = postControlViewModel;
            }

            public void OnSuccess(UserMiniDetailResponse response)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _postControlViewModel.User = response.User;
                        _postControlViewModel.UserFollowerList.Clear();
                        _postControlViewModel.UserFollowingList.Clear();
                        foreach (var responseFollowerUserId in response.FollowerUserIds)
                        {
                            _postControlViewModel.UserFollowerList.Add(responseFollowerUserId);
                        }

                        foreach (var responseFollowingUserId in response.FollowingUserIds)
                        {
                            _postControlViewModel.UserFollowingList.Add(responseFollowingUserId);
                        }

                        _postControlViewModel.GetUserMiniDetailSuccess?.Invoke();
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public class FollowUnFollowPresenterCallBack : IPresenterCallBack<FollowUnFollowSearchedUserResponse>
        {
            private readonly PostControlViewModel _postControlViewModel;

            public FollowUnFollowPresenterCallBack(PostControlViewModel postControlViewModel)
            {
                _postControlViewModel = postControlViewModel;
            }
            public void OnSuccess(FollowUnFollowSearchedUserResponse followUnFollowSearchedUserResponse)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        if (followUnFollowSearchedUserResponse.FollowingSuccess)
                        {
                            _postControlViewModel.UserFollowingList.Add(AppSettings.UserId);
                        }
                        else
                        {
                            _postControlViewModel.UserFollowingList.Remove(AppSettings.UserId);
                        }
                        _postControlViewModel.FollowUnFollowActionDone?.Invoke();
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}
