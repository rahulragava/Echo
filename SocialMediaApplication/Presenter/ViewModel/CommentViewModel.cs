using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.View.CommentView;
using SocialMediaApplication.Util;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class CommentViewModel : ObservableObject
    {
        public ObservableCollection<CommentBObj> PostComments = new ObservableCollection<CommentBObj>();
        public List<CommentBObj> CommentsList;
        public ObservableCollection<Reaction> Reactions;
        public ICommentsViewUserControlView CommentViewUserControlView { get; set; }
        public ICommentUserControlView CommentUserControlView { get; set; }

        private Reaction _reaction;

        public Reaction Reaction
        {
            get => _reaction;
            set => SetProperty(ref _reaction, value);
        }

        public CommentViewModel()
        {
            CommentsList = new List<CommentBObj>();
            Reactions = new ObservableCollection<Reaction>();
        }

        private string _reactionIcon;

        public string ReactionIcon
        {
            get => _reactionIcon;
            set => SetProperty(ref _reactionIcon, value);
        }

        private string _formattedDateTime;

        public string FormattedDateTime
        {
            get => _formattedDateTime;
            set => SetProperty(ref _formattedDateTime, value);
        }

        private string _reactionText = "React";

        public string ReactionText
        {
            get => _reactionText;
            set => SetProperty(ref _reactionText, value);
        }

        private string _commentedBy;

        public string CommentedBy
        {
            get => _commentedBy;
            set => SetProperty(ref _commentedBy, value);
        }

        public void SendCommentButtonClicked(string content, string parentId, string commentOnPostId, int depth)
        {
            var insertCommentRequest =
                new InsertCommentRequest(content, parentId, commentOnPostId, depth);
            var insertCommentUseCase =
                new InsertCommentUseCase(insertCommentRequest, new InsertCommentPresenterCallBack(this));
            insertCommentUseCase.Execute();
        }

        public void RemoveSelectedComment(CommentBObj comment)
        {
            var removeCommentRequest = new RemoveCommentRequest(comment);
            var removeCommentUseCase =
                new RemoveCommentUseCase(removeCommentRequest, new RemoveCommentPresenterCallBack(this));
            removeCommentUseCase.Execute();
        }

        public void ChangeInReaction(Reaction commentReaction)
        {
            var reaction = Reactions.SingleOrDefault(r => r.ReactedBy == commentReaction.ReactedBy);
            if (reaction != null)
            {
                Reactions.Remove(reaction);
                Reactions.Add(commentReaction);
            }
            else
            {
                Reactions.Add(commentReaction);
            }
        }

        public void SetCommentReactions(List<Reaction> reactions)
        {
            Reactions.Clear();
            foreach (var reaction in reactions) Reactions.Add(reaction);
        }

        public void GetUser()
        {
            var getUserRequest = new GetUserRequestObj(new List<string> { CommentedBy });
            var getUserUseCase = new GetUserUseCase(getUserRequest, new GetUserDetailViewModelPresenterCallBack(this));
            getUserUseCase.Execute();
        }

        public string PostId;
        public string CommentId;

        public void GetCommentReaction()
        {
            if (Reactions != null)
            {
                var react = Reactions.SingleOrDefault(r =>
                    r.ReactionOnId == CommentId && r.ReactedBy == AppSettings.UserId);
                if (react != null)
                {
                    Reaction = react;
                    switch (Reaction.ReactionType)
                    {
                        case ReactionType.Heart:
                            ReactionIcon = "♥";
                            ReactionText = "Heart";
                            break;
                        case ReactionType.ThumbsDown:
                            ReactionIcon = "👎";
                            ReactionText = "Thumbs down";

                            break;
                        case ReactionType.ThumbsUp:
                            ReactionIcon = "👍";
                            ReactionText = "Thumbs up";
                            break;
                        case ReactionType.Happy:
                            ReactionIcon = "😁";
                            ReactionText = "Happy";
                            break;
                        case ReactionType.Mad:
                            ReactionIcon = "😠";
                            ReactionText = "Mad";
                            break;
                        case ReactionType.HeartBreak:
                            ReactionIcon = "💔";
                            ReactionText = "Heart break";
                            break;
                        case ReactionType.Sad:
                            ReactionIcon = "😕";
                            ReactionText = "Sad";
                            break;
                    }
                }
                else
                {
                    Reaction = null;
                }
            }
        }

        private BitmapImage _profileIcon;

        public BitmapImage ProfileIcon
        {
            get => _profileIcon;
            set => SetProperty(ref _profileIcon, value);
        }


        public void ClearAndUpdate()
        {
            PostComments.Clear();
            foreach (var comment in CommentsList) PostComments.Add(comment);
        }

        public async Task SetProfileIconAsync(string imagePath)
        {
            var imageConversion = new StringToImageUtil();
            var profileIcon = await imageConversion.GetImageFromStringAsync(imagePath);
            ProfileIcon = profileIcon;
        }

        private Visibility _removeButtonVisibility;

        public Visibility RemoveButtonVisibility
        {
            get => _removeButtonVisibility;
            set => SetProperty(ref _removeButtonVisibility, value);
        }


        public void SuccessfullyCommented(CommentBObj commentBObj, int insertedIndex)
        {
            if (CommentViewUserControlView != null)
            {
                if (CommentsList.Count == 0)
                {
                    CommentsList.Add(commentBObj);
                    PostComments.Add(commentBObj);
                }
                else
                {
                    CommentsList.Insert(insertedIndex, commentBObj);
                    PostComments.Insert(insertedIndex, commentBObj);
                }

                CommentViewUserControlView.ParentCommentInserted();
                CommentViewUserControlView.CommentExist();
            }

            if (CommentUserControlView != null) CommentUserControlView.InsertComment(commentBObj, insertedIndex);
        }

        public class InsertCommentPresenterCallBack : IPresenterCallBack<InsertCommentResponse>
        {
            private readonly CommentViewModel _commentViewModel;

            public InsertCommentPresenterCallBack(CommentViewModel commentViewModel)
            {
                _commentViewModel = commentViewModel;
            }

            public void OnSuccess(InsertCommentResponse insertCommentResponse)
            {
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _commentViewModel.SuccessfullyCommented(insertCommentResponse.CommentBObj,
                            insertCommentResponse.InsertedIndex);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public class RemoveCommentPresenterCallBack : IPresenterCallBack<RemoveCommentResponse>
        {
            private readonly CommentViewModel _commentViewModel;

            public RemoveCommentPresenterCallBack(CommentViewModel commentViewModel)
            {
                _commentViewModel = commentViewModel;
            }

            public void OnSuccess(RemoveCommentResponse removeCommentResponse)
            {
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _commentViewModel.CommentUserControlView.RemoveComments(removeCommentResponse
                            .RemovedCommentIds);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                //throw new NotImplementedException();
            }
        }


        public class GetUserDetailViewModelPresenterCallBack : IPresenterCallBack<GetUserResponseObj>
        {
            private readonly CommentViewModel _commentViewModel;

            public GetUserDetailViewModelPresenterCallBack(CommentViewModel commentViewModel)
            {
                _commentViewModel = commentViewModel;
            }


            public void OnSuccess(GetUserResponseObj getUserResponseObj)
            {
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _commentViewModel?.SetProfileIconAsync(getUserResponseObj.Users[0].ProfileIcon);
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