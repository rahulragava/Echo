using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Util;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using SocialMediaApplication.Presenter.View.PostView.PollPostView;
using SocialMediaApplication.Presenter.View.PostView.TextPostView;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class PostControlViewModel : ObservableObject
    {
        public readonly ObservableCollection<Reaction> Reactions = new ObservableCollection<Reaction>();
        public List<CommentBObj> CommentsList;
        public readonly ObservableCollection<CommentBObj> Comments = new ObservableCollection<CommentBObj>();
        public ObservableCollection<PollChoiceBObj> PollChoices = new ObservableCollection<PollChoiceBObj>();
        public ITextPostUserControl TextPostUserControl;
        public IPollPostUserControl PollPostUserControl;

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

        private string _postedById;

        public string PostedById
        {
            get => _postedById;
            set => SetProperty(ref _postedById, value);
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

        private BitmapImage _profileIcon;

        public BitmapImage ProfileIcon
        {
            get => _profileIcon;
            set => SetProperty(ref _profileIcon, value);
        }

        private string _totalComments;

        public string TotalComments
        {
            get => _totalComments;
            set => SetProperty(ref _totalComments, value);
        }

        private int _totalReaction;

        public int TotalReaction
        {
            get => _totalReaction;
            set => SetProperty(ref _totalReaction, value);
        }

        public async Task SetProfileIconAsync(string imagePath)
        {
            var imageConversion = new StringToImageUtil();
            var profileIcon = await imageConversion.GetImageFromStringAsync(imagePath);
            ProfileIcon = profileIcon;
        }

        public void SetObservableCollection(List<Reaction> reactionList, List<CommentBObj> commentList, string postId)
        {
            if (reactionList != null)
            {
                Reactions.Clear();
                foreach (var reaction in reactionList)
                {
                    Reactions.Add(reaction);
                }
            }
            ReactionType maxReaction = ReactionType.None;
            try
            {
                maxReaction = Reactions.Max(reaction => reaction.ReactionType);
                SetReactionIcon(maxReaction, false, true);
            }
            catch
            {
                MaxReactionIcon = string.Empty;
                maxReaction = ReactionType.None;
            }
            try
            {
                if (maxReaction == ReactionType.None)
                {
                    SecondMaxReactionIcon = string.Empty;
                }
                else
                {
                    var secondMax = Reactions.Where(r => r.ReactionType != maxReaction).Max(r => r.ReactionType);
                    SetReactionIcon(secondMax, false, false);
                }
            }
            catch
            {
                SecondMaxReactionIcon = string.Empty;
                // there is no second max reaction
            }
            
            if (commentList != null)
            {
                CommentsList = commentList;
                Comments.Clear();
                foreach (var comment in commentList)
                {
                    Comments.Add(comment);
                }
            }

            var numberOfComments = Comments.Count;
            if (numberOfComments > 0)
            {
                TotalComments = Comments.Count.ToString();
            }
            else
            {
                TotalComments = string.Empty;
            }

            var react = Reactions.SingleOrDefault(r => r.ReactionOnId == postId && r.ReactedBy == AppSettings.UserId);
            if (react != null)
            {
                Reaction = react;
                SetReactionIcon(Reaction.ReactionType,true,false);
            }
            else
            {
                Reaction = null;
                ReactionIcon = string.Empty;
            }
            if (ReactionIcon == MaxReactionIcon)
            {
                MaxReactionIcon = string.Empty;
            }
            if (ReactionIcon == SecondMaxReactionIcon)
            {
                SecondMaxReactionIcon = string.Empty;
            }
            if (TextPostUserControl != null)
            {
                TextPostUserControl.SetReactionVisibility();
            }
            else
            {
                PollPostUserControl.SetReactionVisibility();
            }
            TotalReaction = Reactions.Count - 1;
        }
        
        public void SetReactionIcon(ReactionType reactionType,bool isReactionIcon,bool isMaxReactionIcon)
        {
            switch (reactionType)
            {
                case ReactionType.Heart:
                    if (isReactionIcon)
                    {
                        ReactionIcon = "♥";
                        ReactionText = "Heart";
                        break;
                    }
                    if(isMaxReactionIcon)
                    {
                        MaxReactionIcon = "♥";
                        break;
                    }
                    SecondMaxReactionIcon = "♥";
                    break;
                case ReactionType.ThumbsDown:
                    if (isReactionIcon)
                    {
                        ReactionIcon = "👎";
                        ReactionText = "Dislike";
                        break;
                    }
                    if (isMaxReactionIcon)
                    {
                        MaxReactionIcon = "👎";
                        break;
                    }
                    SecondMaxReactionIcon = "👎";
                    break;
                case ReactionType.ThumbsUp:
                    if (isReactionIcon)
                    {
                        ReactionIcon = "👍";
                        ReactionText = "Like";
                        break;
                    }
                    if (isMaxReactionIcon)
                    {
                        MaxReactionIcon = "👍";
                        break;
                    }
                    SecondMaxReactionIcon = "👍";
                    break;
                case ReactionType.Happy:
                    if (isReactionIcon)
                    {
                        ReactionIcon = "😁";
                        ReactionText = "Happy";
                        break;
                    }
                    if (isMaxReactionIcon)
                    {
                        MaxReactionIcon = "😁";
                        break;
                    }
                    SecondMaxReactionIcon = "😁";
                    break;
                case ReactionType.Mad:
                    if (isReactionIcon)
                    {
                        ReactionIcon = "😠";
                        ReactionText = "Mad";
                        break;
                    }
                    if (isMaxReactionIcon)
                    {
                        MaxReactionIcon = "😠";
                        break;
                    }
                    SecondMaxReactionIcon = "😠";
                    break;
                case ReactionType.HeartBreak:
                    if (isReactionIcon)
                    {
                        ReactionIcon = "💔";
                        ReactionText = "HeartBreak";
                        break;
                    }
                    if (isMaxReactionIcon)
                    {
                        MaxReactionIcon = "💔";
                        break;
                    }
                    SecondMaxReactionIcon = "💔";
                    break;
                case ReactionType.Sad:
                    if (isReactionIcon)
                    {
                        ReactionIcon = "😕";
                        ReactionText = "Sad";
                        break;
                    }
                    if (isMaxReactionIcon)
                    {
                        MaxReactionIcon = "😕";
                        break;
                    }
                    SecondMaxReactionIcon = "😕";
                    break;
                default:
                    ReactionIcon = null;
                    ReactionText = "React";
                    break;
            }
        }

        public void GetMiniProfileDetails(string postedByUser)
        {
            var userMiniDetailRequest =
                new UserMiniDetailRequest(postedByUser, new UserMiniDetailPresenterCallBack(this));
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
                    choice.ChoiceSelectionPercent = choice.ChoiceSelectedUsers.Count * 100 / TotalVotes;
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
            var insertUserSelectionChoiceUseCase =
                new InsertUserSelectionChoiceUseCase(insertUserSelectionChoiceRequest);
            insertUserSelectionChoiceUseCase.Execute();
        }

        public void SuccessfullyUserChoiceSelectionInserted(
            InsertUserChoiceSelectionResponse insertUserChoiceSelectionResponse)
        {
            TotalVotes = 0;
            foreach (var pollChoice in PollChoices)
            {
                if (pollChoice.Id != PollChoiceBObj.Id)
                {
                    var choiceSelectedUser =
                        pollChoice.ChoiceSelectedUsers.SingleOrDefault(c => c.SelectedBy == AppSettings.UserId);
                    if (choiceSelectedUser != null)
                    {
                        pollChoice.ChoiceSelectedUsers.Remove(choiceSelectedUser);
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
                pollChoiceBObj.ChoiceSelectionPercent =
                    (int)pollChoiceBObj.ChoiceSelectedUsers.Count * 100 / TotalVotes;
            }
        }

        public void RemovePost(PostBObj postBObj)
        {
            var removePostRequest = new RemovePostRequest(postBObj, new RemovePostPresenterCallBack(this));
            var removePostUseCase = new RemovePostUseCase(removePostRequest);
            removePostUseCase.Execute();
        }

        public void FollowUnFollowSearchedUser(string userId)
        {

            var followUnFollowSearchedUserRequest = new FollowUnFollowSearchedUserRequest(userId, AppSettings.UserId,
                new FollowUnFollowPresenterCallBack(this));
            var followUnFollowSearchedUserUseCase =
                new FollowUnFollowSearchedUserUseCase(followUnFollowSearchedUserRequest);
            followUnFollowSearchedUserUseCase.Execute();
        }

        public void GetUser()
        {
            var getUserRequest = new GetUserRequestObj(new List<string>() { PostedById },
                new GetUserDetailViewModelPresenterCallBack(this));
            var getUserUseCase = new GetUserUseCase(getUserRequest);
            getUserUseCase.Execute();
        }

        private string _reactionIcon;

        public string ReactionIcon
        {
            get => _reactionIcon;
            set => SetProperty(ref _reactionIcon, value);
        }

        private string _maxReactionIcon;

        public string MaxReactionIcon
        {
            get => _maxReactionIcon;
            set => SetProperty(ref _maxReactionIcon, value);
        }

        private string _secondMaxReactionIcon;

        public string SecondMaxReactionIcon
        {
            get => _secondMaxReactionIcon;
            set => SetProperty(ref _secondMaxReactionIcon, value);
        }

        private string _reactionText = "React";

        public string ReactionText
        {
            get => _reactionText;
            set => SetProperty(ref _reactionText, value);
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

                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _postControlViewModel.SuccessfullyUserChoiceSelectionInserted(
                            insertUserChoiceSelectionResponse);
                    }
                );
            }

            public void OnError(Exception ex)
            {
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
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        if (_postControlViewModel.TextPostUserControl != null)
                        {
                            _postControlViewModel.TextPostUserControl.RemovedPost(response.PostId);
                        }
                        else
                        {
                            _postControlViewModel.PollPostUserControl.RemovedPost(response.PostId);
                        }
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
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
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

                        if (_postControlViewModel.TextPostUserControl != null)
                        {
                            _postControlViewModel.TextPostUserControl.GetUserMiniDetailSuccess();
                        }
                        else
                        {
                            _postControlViewModel.PollPostUserControl.GetUserMiniDetailSuccess();
                        }
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
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
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
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
                        if (_postControlViewModel.TextPostUserControl != null)
                        {
                            _postControlViewModel.TextPostUserControl.FollowUnFollowActionDone();
                        }
                        else
                        {
                            _postControlViewModel.PollPostUserControl.FollowUnFollowActionDone();

                        }
                    }
                );
            }

            public void OnError(Exception ex)
            {
            }
        }

        public class GetUserDetailViewModelPresenterCallBack : IPresenterCallBack<GetUserResponseObj>
        {
            private readonly PostControlViewModel _postControlViewModel;

            public GetUserDetailViewModelPresenterCallBack(PostControlViewModel postControlViewModel)
            {
                _postControlViewModel = postControlViewModel;
            }

            public void OnSuccess(GetUserResponseObj getUserResponseObj)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _postControlViewModel?.SetProfileIconAsync(getUserResponseObj.Users[0].ProfileIcon);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }
    }
}
