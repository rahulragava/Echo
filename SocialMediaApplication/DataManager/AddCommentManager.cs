using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Services;
using SocialMediaApplication.Services.Contract;
using SocialMediaApplication.Util;
using Windows.System;

namespace SocialMediaApplication.DataManager
{
    public sealed class AddCommentManager : IAddCommentManager
    {
        private static AddCommentManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private AddCommentManager() { }

        //public List<CommentBObj> CommentCacheList;

        public static AddCommentManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new AddCommentManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly ICommentDbHandler _commentDbHandler = CommentDbHandler.GetInstance;
        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;

        private readonly ReactionManager _reactionManager = ReactionManager.GetInstance;

        public async Task<List<CommentBObj>> GetCommentBObjsAsync()
        {
            var commentBObjs = new List<CommentBObj>();
            var comments = (await _commentDbHandler.GetAllCommentAsync()).ToList();
            var reactions = await _reactionManager.GetReactionAsync();
            CommentBObj commentBobj;

            foreach (var comment in comments)
            {
                var commentReactions =
                    reactions.Where((reaction) => reaction.ReactionOnId == comment.Id).ToList();
                commentBobj = new CommentBObj
                {
                    Id = comment.Id,
                    PostId = comment.PostId,
                    ParentCommentId = comment.ParentCommentId,
                    CommentedBy = comment.CommentedBy,
                    CommentedAt = comment.CommentedAt,
                    FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                    Content = comment.Content,
                    Reactions = reactions
                };

                commentBObjs.Add(commentBobj);
            }

            return commentBObjs;
        
        }

        public static event Action CommentInserted;

        public async Task InsertCommentAsync(InsertCommentRequest insertCommentRequest,
            InsertCommentUseCaseCallBack insertCommentUseCaseCallBack)
        {
            try
            {
                var comment = new Comment()
                {
                    ParentCommentId = insertCommentRequest.ParentCommentId,
                    CommentedAt = DateTime.Now,
                    Content = insertCommentRequest.Content,
                    CommentedBy = AppSettings.LocalSettings.Values["user"].ToString(),
                    PostId = insertCommentRequest.CommentOnPostId
                };
                await _commentDbHandler.InsertCommentAsync(comment);
                //var user = await _userDbHandler.GetUserAsync(AppSettings.UserId);
                //var commentBObj = new CommentBObj()
                //{
                //    Id = comment.Id,
                //    PostId = comment.PostId,
                //    CommentedBy = user.Id,
                //    CommentedAt = comment.CommentedAt,
                //    FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                //    Content = comment.Content,
                //    CommentedUserName = user.UserName,
                //    ParentCommentId = insertCommentRequest.ParentCommentId,

                //};
                //var comments = (await _commentDbHandler.GetPostCommentsAsync(insertCommentRequest.CommentOnPostId)).ToList();
                //var commentBObjList = await GetSortedCommentBObjList(comments);

                //if (insertCommentRequest.Depth == 0 && insertCommentRequest.ParentCommentId == null)
                //{
                //    commentBObj.Depth = 0;
                //}
                //else
                //{
                //    commentBObj.Depth = insertCommentRequest.Depth + 10;

                //}


                //if (commentBObjList.Count == 0)
                //{
                //    commentBObjList.Add(commentBObj);

                //}
                //else
                //{
                //    int parentIndex = -1;
                //    int siblingCount = 0;
                //    int currentIndex = commentBObjList.Count;
                //    if (commentBObj.ParentCommentId == null)
                //    {
                //        currentIndex = commentBObjList.Count;
                //    }
                //    else
                //    {
                //        for (int i = 0; i < commentBObjList.Count; i++)
                //        {
                //            if (commentBObjList[i].Id == commentBObj.ParentCommentId)
                //            {
                //                parentIndex = i;
                //            }

                //            if (commentBObjList[i].ParentCommentId == commentBObj.ParentCommentId)
                //            {
                //                siblingCount++;
                //            }
                //        }
                //        currentIndex = parentIndex + siblingCount + 1;
                //    }
                //    commentBObjList.Insert(currentIndex, commentBObj);
                //}

                CommentInserted?.Invoke();
                //comment
                insertCommentUseCaseCallBack?.OnSuccess(new InsertCommentResponse());
            }
            catch (Exception ex)
            {
                insertCommentUseCaseCallBack?.OnError(ex);
            }
        }

        public async Task<List<CommentBObj>> GetSortedCommentBObjList(List<Comment> comments)
        {
            List<CommentBObj> sortedCommentBobjList;
            var commentBobjList = new List<CommentBObj>();
            foreach (var comment in comments)
            {
                var user = await _userDbHandler.GetUserAsync(comment.CommentedBy);
                var commentBObj = new CommentBObj()
                {
                    Id = comment.Id,
                    PostId = comment.PostId,
                    CommentedBy = user.Id,
                    CommentedAt = comment.CommentedAt,
                    FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                    Content = comment.Content,
                    CommentedUserName = user.UserName,
                    ParentCommentId = comment.ParentCommentId,
                };
                commentBobjList.Add(commentBObj);
            }

            sortedCommentBobjList = FetchPostManager.GetSortedComments(commentBobjList);
            return sortedCommentBobjList;
        }
        //public async Task AddCommentAsync(CommentBObj comment)
        //{
        //    await Task.Run(() => _commentDbHandler.InsertCommentAsync(ConvertCommentBObjToEntity(comment))).ConfigureAwait(false);
        //}

        //public async Task RemoveCommentAsync(CommentBObj comment)
        //{
        //    if (comment.Reactions != null && comment.Reactions.Any())
        //        await _reactionManager.RemoveReactionsAsync(comment.Reactions).ConfigureAwait(false);
        //    var entityComment = ConvertCommentBObjToEntity(comment);
        //    await Task.Run(() => _commentDbHandler.RemoveCommentAsync(entityComment.Id)).ConfigureAwait(false);
        //}



        //public async Task RemoveCommentsAsync(List<CommentBObj> CommentCacheList)
        //{
        //    if (CommentCacheList.Count <= 0 || !(CommentCacheList.Any())) return;

        //    while (true)
        //    {
        //        for (var i = 0; i < CommentCacheList.Count; i++)
        //        {
        //            await RemoveCommentAsync(CommentCacheList[i]).ConfigureAwait(false);
        //            break;
        //        }
        //        if (CommentCacheList.Count == 0) break;
        //    }
        //}


    }
}
