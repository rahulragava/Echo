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
using Microsoft.VisualStudio.PlatformUI;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class CommentViewModel : ObservableObject
    {
        public ObservableCollection<CommentBObj> PostComments = new ObservableCollection<CommentBObj>();
        public List<CommentBObj> CommentsList;

        public EventHandler CheckAnyComments;


        public void SendCommentButtonClicked(string content, string parentId, string commentOnPostId,int depth)
        {
            var insertCommentRequest =
                new InsertCommentRequest(content, parentId, commentOnPostId, depth, new InsertCommentPresenterCallBack(this));
            var insertCommentUseCase = new InsertCommentUseCase(insertCommentRequest);
            insertCommentUseCase.Execute();
        }

        public void RemoveSelectedComment(CommentBObj comment, List<CommentBObj> comments)
        {
            var removeCommentRequest = new RemoveCommentRequest(comment,comments,new RemoveCommentPresenterCallBack(this));
            var removeCommentUseCase = new RemoveCommentUseCase(removeCommentRequest);
            removeCommentUseCase.Execute();
        }
        public void ClearAndUpdate()
        {
            PostComments.Clear();
            foreach (var comment in CommentsList)
            {
                PostComments.Add(comment);
            }
        }

        public void SuccessfullyCommented(CommentBObj commentBObj)
        {
            if (CommentsList.Count == 0)
            {
                CommentsList.Add(commentBObj);

            }
            else
            {
                int parentIndex = -1;
                int siblingCount = 0;
                int currentIndex = CommentsList.Count;
                if (commentBObj.ParentCommentId == null)
                {
                    currentIndex = CommentsList.Count;
                }
                else
                {
                    for (int i = 0; i < CommentsList.Count; i++)
                    {
                        if (CommentsList[i].Id == commentBObj.ParentCommentId)
                        {
                            parentIndex = i;
                        }

                        if (CommentsList[i].ParentCommentId == commentBObj.ParentCommentId)
                        {
                            siblingCount++;
                        }
                    }
                    currentIndex = parentIndex + siblingCount + 1;
                }
                CommentsList.Insert(currentIndex, commentBObj);
            } 
            ClearAndUpdate();
            CheckAnyComments?.Invoke(this, EventArgs.Empty);

        }

        public void SuccessfullyRemovedComments(List<string> removedCommentIds)
        {
            CommentsList.RemoveAll(c => removedCommentIds.Contains(c.Id));
            ClearAndUpdate();
            CheckAnyComments?.Invoke(this, EventArgs.Empty);

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
                    _commentViewModel.SuccessfullyCommented(insertCommentResponse.CommentBObj);
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
                    _commentViewModel.SuccessfullyRemovedComments(removeCommentResponse.RemovedCommentIds);
                }
            );
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
