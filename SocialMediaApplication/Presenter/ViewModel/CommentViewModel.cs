using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using SocialMediaApplication.Models.BusinessModels;
using System.Collections.ObjectModel;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Util;
using System.ComponentModel.Design;
using SocialMediaApplication.Presenter.View.CommentView;
using static SocialMediaApplication.Presenter.ViewModel.PostControlViewModel;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class CommentViewModel : ObservableObject
    {
        public ObservableCollection<CommentBObj> PostComments = new ObservableCollection<CommentBObj>();
        public List<CommentBObj> CommentsList;
        public List<Reaction> Reactions;
        //public ObservableCollection<string> UserFollowerList;
        //public ObservableCollection<string> UserFollowingList;
        //public User User { get; set; }

        private Reaction _reaction;

        public Reaction Reaction
        {
            get => _reaction;
            set => SetProperty(ref _reaction, value);
        }
        public EventHandler CheckAnyComments;
        public EventHandler ParentCommentInserted;
        public Action<CommentBObj,int> CommentInserted;
        public Action<List<String>> CommentRemoved;

        public CommentViewModel()
        {
            CommentsList = new List<CommentBObj>();
            Reactions = new List<Reaction>();
            //UserFollowerList = new ObservableCollection<string>();
            //UserFollowingList = new ObservableCollection<string>();
        }

        private string _reactionIcon;

        public string ReactionIcon
        {
            get => _reactionIcon;
            set => SetProperty(ref _reactionIcon, value);
        }


        public void SendCommentButtonClicked(string content, string parentId, string commentOnPostId,int depth)
        {
            var insertCommentRequest =
                new InsertCommentRequest(content, parentId, commentOnPostId, depth, new InsertCommentPresenterCallBack(this));
            var insertCommentUseCase = new InsertCommentUseCase(insertCommentRequest);
            insertCommentUseCase.Execute();
        }

        public void RemoveSelectedComment(CommentBObj comment)
        {
            var removeCommentRequest = new RemoveCommentRequest(comment,new RemoveCommentPresenterCallBack(this));
            var removeCommentUseCase = new RemoveCommentUseCase(removeCommentRequest);
            removeCommentUseCase.Execute();
        }

        //public Action GetUserMiniDetailSuccess;
        //public void GetMiniProfileDetails(string commentByUser)
        //{
        //    var userMiniDetailRequest = new UserMiniDetailRequest(commentByUser, new UserMiniDetailPresenterCallBack(this));
        //    var userMiniDetailUseCase = new UserMiniDetailUseCase(userMiniDetailRequest);
        //    userMiniDetailUseCase.Execute();
        //}

        public string PostId;
        public string CommentId;

        public void GetCommentReaction()
        {
            if (Reactions == null)
            {

            }
            else
            {
                var react = Reactions.SingleOrDefault(r => r.ReactionOnId == CommentId && r.ReactedBy == AppSettings.UserId);
                Reaction = react;
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
                }

            }
        }

        public void GetComments()
        {
            var getCommentRequest = new GetCommentRequest(PostId, new GetCommentsPresenterCallBack(this));
            var getCommentUseCase = new GetCommentUseCase(getCommentRequest);
            getCommentUseCase.Execute();
        }

        public void ClearAndUpdate()
        {
            PostComments.Clear();
            foreach (var comment in CommentsList)
            {
                PostComments.Add(comment);
            }
        }

        public void SetStackPanelDepth(int depth)
        {
            StackDepth = depth+45;
        }

        private int _stackDepth;

        public int StackDepth
        {
            get => _stackDepth;
            set => SetProperty(ref _stackDepth, value);
        }
        private Visibility _removeButtonVisibility;

        public Visibility RemoveButtonVisibility
        {
            get => _removeButtonVisibility;
            set => SetProperty(ref _removeButtonVisibility, value);
        }

        public void SuccessfullyCommented(CommentBObj commentBObj, int insertedIndex)
        {
            if (CheckAnyComments != null)
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
                ParentCommentInserted?.Invoke(this, EventArgs.Empty);
            }
            CommentInserted?.Invoke(commentBObj,insertedIndex);
            CheckAnyComments?.Invoke(this, EventArgs.Empty);

        }
      
        public void SuccessfullyRemovedComments(List<string> removedCommentIds)
        {

            CommentRemoved?.Invoke(removedCommentIds);
        }
        public void GetCommentSuccess(List<CommentBObj> comments)
        {
            CommentsList = comments;
            PostComments.Clear();
            foreach (var comment in comments)
            {
                PostComments.Add(comment);
            }
            CheckAnyComments?.Invoke(this, EventArgs.Empty);

        }

        //public class UserMiniDetailPresenterCallBack : IPresenterCallBack<UserMiniDetailResponse>
        //{
        //    private readonly CommentViewModel _commentViewModel;
        //    public UserMiniDetailPresenterCallBack(CommentViewModel commentViewModel)
        //    {
        //        _commentViewModel = commentViewModel;
        //    }

        //    public void OnSuccess(UserMiniDetailResponse response)
        //    {
        //        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //            () =>
        //            {
        //                _commentViewModel.User = response.User;
        //                _commentViewModel.UserFollowerList.Clear();
        //                _commentViewModel.UserFollowingList.Clear();
        //                foreach (var responseFollowerUserId in response.FollowerUserIds)
        //                {
        //                    _commentViewModel.UserFollowerList.Add(responseFollowerUserId);
        //                }

        //                foreach (var responseFollowingUserId in response.FollowingUserIds)
        //                {
        //                    _commentViewModel.UserFollowingList.Add(responseFollowingUserId);
        //                }

        //                _commentViewModel.GetUserMiniDetailSuccess?.Invoke();
        //            }
        //        );
        //    }

        //    public void OnError(Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public class FollowUnFollowPresenterCallBack : IPresenterCallBack<FollowUnFollowSearchedUserResponse>
        //{
        //    private readonly CommentViewModel _commentViewModel;

        //    public FollowUnFollowPresenterCallBack(CommentViewModel commentViewModel)
        //    {
        //        _commentViewModel = commentViewModel;
        //    }
        //    public void OnSuccess(FollowUnFollowSearchedUserResponse followUnFollowSearchedUserResponse)
        //    {
        //        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //            () =>
        //            {
        //                if (followUnFollowSearchedUserResponse.FollowingSuccess)
        //                {
        //                    _commentViewModel.UserFollowingList.Add(AppSettings.UserId);
        //                }
        //                else
        //                {
        //                    _commentViewModel.UserFollowingList.Remove(AppSettings.UserId);
        //                }
        //            }
        //        );
        //    }

        //    public void OnError(Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        public class InsertCommentPresenterCallBack : IPresenterCallBack<InsertCommentResponse>
        {
            private readonly CommentViewModel _commentViewModel;

            public InsertCommentPresenterCallBack(CommentViewModel commentViewModel)
            {
                _commentViewModel = commentViewModel;
            }

            public void OnSuccess(InsertCommentResponse insertCommentResponse)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _commentViewModel.SuccessfullyCommented(insertCommentResponse.CommentBObj, insertCommentResponse.InsertedIndex);
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
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        //_commentViewModel.CheckAnyComments?.Invoke(this,EventArgs.Empty);
                        _commentViewModel.SuccessfullyRemovedComments(removeCommentResponse.RemovedCommentIds);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
            }
        }



        public class GetCommentsPresenterCallBack : IPresenterCallBack<GetCommentResponse>
        {
            private readonly CommentViewModel _commentViewModel;

            public GetCommentsPresenterCallBack(CommentViewModel commentViewModel)
            {
                _commentViewModel = commentViewModel;
            }

            public void OnSuccess(GetCommentResponse getCommentResponse)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _commentViewModel.GetCommentSuccess(getCommentResponse.Comments);
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
