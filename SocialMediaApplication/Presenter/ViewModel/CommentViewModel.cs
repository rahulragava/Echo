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

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class CommentViewModel : ObservableObject
    {
        public ObservableCollection<CommentBObj> PostComments = new ObservableCollection<CommentBObj>();
        public List<CommentBObj> CommentsList;

        public EventHandler CheckAnyComments;

        public CommentViewModel()
        {
            CommentsList = new List<CommentBObj>();
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

        public string PostId;

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

        public void SuccessfullyCommented()
        {
            CheckAnyComments?.Invoke(this, EventArgs.Empty);

            //if (CommentsList.Count == 0)
            //{
            //    CommentsList.Add(commentBObj);

            //}
            //else
            //{
            //    int parentIndex = -1;
            //    int siblingCount = 0;
            //    int currentIndex = CommentsList.Count;
            //    if (commentBObj.ParentCommentId == null)
            //    {
            //        currentIndex = CommentsList.Count;
            //    }
            //    else
            //    {
            //        for (int i = 0; i < CommentsList.Count; i++)
            //        {
            //            if (CommentsList[i].Id == commentBObj.ParentCommentId)
            //            {
            //                parentIndex = i;
            //            }

            //            if (CommentsList[i].ParentCommentId == commentBObj.ParentCommentId)
            //            {
            //                siblingCount++;
            //            }
            //        }
            //        currentIndex = parentIndex + siblingCount + 1;
            //    }
            //    CommentsList.Insert(currentIndex, commentBObj);
            //} 
            //CommentsList = commentBObjs;
            //ClearAndUpdate();


        }

        //public List<string> RemovedCommentIds;
        public void SuccessfullyRemovedComments()
        {
            CheckAnyComments?.Invoke(this, EventArgs.Empty);

            //CommentsList.RemoveAll(c => removedCommentIds.Contains(c.Id));
            //RemovedCommentIds = new List<string>();
            //RemovedCommentIds = removedCommentIds;
            //ClearAndUpdate();


        }

        //public void CommentRemoved()
        //{

        //    CheckAnyComments?.Invoke(this, EventArgs.Empty);
        //}
        public void GetCommentSuccess(List<CommentBObj> comments)
        {
            CommentsList = comments;
            PostComments.Clear();
            foreach (var comment in comments)
            {
                PostComments.Add(comment);
            }
        }

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
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _commentViewModel.SuccessfullyCommented();
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
                    _commentViewModel.SuccessfullyRemovedComments();
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
