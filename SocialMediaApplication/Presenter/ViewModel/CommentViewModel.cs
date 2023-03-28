using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using SocialMediaApplication.Models.BusinessModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Util;
using SocialMediaApplication.Presenter.View.CommentView;
using Windows.UI.Xaml.Media.Imaging;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class CommentViewModel : ObservableObject
    {
        public ObservableCollection<CommentBObj> PostComments = new ObservableCollection<CommentBObj>();
        public List<CommentBObj> CommentsList;
        public List<Reaction> Reactions;
        public ICommentsViewUserControlView CommentViewUserControlView { get; set; }
        public ICommentUserControlView CommentUserControlView { get; set; }

        private Reaction _reaction;
        public Reaction Reaction
        {
            get => _reaction;
            set => SetProperty(ref _reaction, value);
        }
        //public Action<CommentBObj,int> CommentInserted;
        //public Action<List<String>> CommentRemoved;

        public CommentViewModel()
        {
            CommentsList = new List<CommentBObj>();
            Reactions = new List<Reaction>();
        }

        private string _reactionIcon;

        public string ReactionIcon
        {
            get => _reactionIcon;
            set => SetProperty(ref _reactionIcon, value);
        }

        private string _commentedBy;

        public string CommentedBy
        {
            get => _commentedBy;
            set => SetProperty(ref _commentedBy, value);
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

        public void GetUser()
        {
            var getUserRequest = new GetUserRequestObj(new List<string>() { CommentedBy },
                new GetUserDetailViewModelPresenterCallBack(this));
            var getUserUseCase = new GetUserUseCase(getUserRequest);
            getUserUseCase.Execute();
        }

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

        private BitmapImage _profileIcon;

        public BitmapImage ProfileIcon
        {
            get => _profileIcon;
            set => SetProperty(ref _profileIcon, value);
        }


        public void ClearAndUpdate()
        {
            PostComments.Clear();
            foreach (var comment in CommentsList)
            {
                PostComments.Add(comment);
            }
        }

        public async Task SetProfileIconAsync(string imagePath)
        {
            var imageConversion = new StringToImageUtil();
            var profileIcon = await imageConversion.GetImageFromStringAsync(imagePath);
            ProfileIcon = profileIcon;
        }

        //public void SetStackPanelDepth(int depth)
        //{
        //    StackDepth = 45;
        //}

        //private int _stackDepth;

        //public int StackDepth
        //{
        //    get => _stackDepth;
        //    set => SetProperty(ref _stackDepth, value);
        //}
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

            if (CommentUserControlView != null)
            {

                CommentUserControlView.InsertComment(commentBObj, insertedIndex);
            }
            
        }
      
        //public void GetCommentSuccess(List<CommentBObj> comments)
        //{
        //    CommentsList = comments;
        //    PostComments.Clear();
        //    foreach (var comment in comments)
        //    {
        //        PostComments.Add(comment);
        //    }
        //    if (CommentViewUserControlView != null)
        //    {
        //        CommentViewUserControlView.CommentExist();
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
                        _commentViewModel.CommentUserControlView.RemoveComments(removeCommentResponse.RemovedCommentIds);
                    }
                );
            }

            public void OnError(Exception ex)
            {
                throw new NotImplementedException();
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
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
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

        //public class GetCommentsPresenterCallBack : IPresenterCallBack<GetCommentResponse>
        //{
        //    private readonly CommentViewModel _commentViewModel;

        //    public GetCommentsPresenterCallBack(CommentViewModel commentViewModel)
        //    {
        //        _commentViewModel = commentViewModel;
        //    }

        //    public void OnSuccess(GetCommentResponse getCommentResponse)
        //    {
        //        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //            () =>
        //            {
        //                _commentViewModel.GetCommentSuccess(getCommentResponse.Comments);
        //            }
        //        );
        //    }

        //    public void OnError(Exception ex)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
