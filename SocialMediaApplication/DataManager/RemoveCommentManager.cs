using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
{
    public sealed class RemoveCommentManager : IRemoveCommentManager
    {
        private static RemoveCommentManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private RemoveCommentManager() { }

        public static RemoveCommentManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new RemoveCommentManager();
                        }
                    }
                }
                return Instance;
            }
        }
        private readonly ICommentDbHandler _commentDbHandler = CommentDbHandler.GetInstance;
        private readonly IReactionManager _reactionManager = ReactionManager.GetInstance;



        public async Task RemoveCommentAsync(RemoveCommentRequest removeCommentRequest,
            RemoveCommentUseCaseCallBack removeCommentUseCaseCallBack)
        {
            try
            {
                //if (removeCommentRequest.Comment.Reactions != null && removeCommentRequest.Comment.Reactions.Any())
                //    await _reactionManager.RemoveReactionsAsync(removeCommentRequest.Comment.Reactions).ConfigureAwait(false);
                //var removedCommentIds = new List<string>();
                //var childComments = removeCommentRequest.Comments.Where(c => c.ParentCommentId == removeCommentRequest.Comment.Id);
                //List<CommentBObj> commentBObjs = new List<CommentBObj>();
                //foreach (var comment in removeCommentRequest.Comments)
                //{
                //    if (comment.ParentCommentId == removeCommentRequest.Comment.Id)
                //    {
                //        commentBObjs.Add(comment);
                //    }

                //}


                var (childComments,removedCommentIdList) = RemovableCommentList(removeCommentRequest.Comments, removeCommentRequest.Comment);
                foreach (var comment in childComments)
                {
                    removedCommentIdList.Add(comment.Id);
                    await _commentDbHandler.RemoveCommentAsync(comment.Id);
                }


                removedCommentIdList.Add(removeCommentRequest.Comment.Id);
                await _commentDbHandler.RemoveCommentAsync(removeCommentRequest.Comment.Id);
                var entityComment = ConvertCommentBObjToEntity(removeCommentRequest.Comment);
                await _commentDbHandler.RemoveCommentAsync(entityComment.Id);

                removeCommentUseCaseCallBack?.OnSuccess(new RemoveCommentResponse(removedCommentIdList));
            }
            catch (Exception e)
            {
                removeCommentUseCaseCallBack?.OnError(e);
            }
        }

        private Tuple<List<CommentBObj>,List<String>> RemovableCommentList(List<CommentBObj> comments, CommentBObj comment)
        {
            List<CommentBObj> commentList = new List<CommentBObj>();
            List<string> commentIdList = new List<string>();

            foreach (var c in comments)
            {
                if (c.ParentCommentId == comment.Id)
                {
                    commentList.Add(c);
                    commentIdList.Add(c.Id);
                    var childOfChild = comments.Where(cc => cc.ParentCommentId == c.Id);
                    if (childOfChild.Any())
                    {
                        var (childComment,removedCommentListId) = RemovableCommentList(comments, c);
                        commentList.AddRange(childComment);
                        commentIdList.AddRange(removedCommentListId);
                    }
                }

            }
            return Tuple.Create(commentList,commentIdList);
        }

        public Comment ConvertCommentBObjToEntity(CommentBObj commentBobj)
        {
            Comment comment = new Comment();
            comment.Id = commentBobj.Id;
            comment.ParentCommentId = commentBobj.ParentCommentId;
            comment.CommentedBy = commentBobj.CommentedBy;
            comment.CommentedAt = commentBobj.CommentedAt;
            comment.Content = commentBobj.Content;
            comment.PostId = commentBobj.PostId;

            return comment;
        }
    }
}
