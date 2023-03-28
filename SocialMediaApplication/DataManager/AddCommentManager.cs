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
                var userName = (await _userDbHandler.GetUserAsync(comment.CommentedBy)).UserName;
                commentBobj = new CommentBObj
                {
                    Id = comment.Id,
                    PostId = comment.PostId,
                    ParentCommentId = comment.ParentCommentId,
                    CommentedBy = comment.CommentedBy,
                    CommentedAt = comment.CommentedAt,
                    FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                    Content = comment.Content,
                    CommentedUserName = userName,
                    Reactions = commentReactions
                };

                commentBObjs.Add(commentBobj);
            }

            return commentBObjs;
        
        }
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

                var comments = (await _commentDbHandler.GetPostCommentsAsync(comment.PostId)).ToList();
                var sortedComments = await GetSortedCommentBObjListAsync(comments);
                var commentBObj = sortedComments.SingleOrDefault(c => c.Id == comment.Id);
                var insertedIndex = sortedComments.IndexOf(commentBObj);
                
                insertCommentUseCaseCallBack?.OnSuccess(new InsertCommentResponse(insertedIndex,commentBObj));
            }
            catch (Exception ex)
            {
                insertCommentUseCaseCallBack?.OnError(ex);
            }
        }

        public async Task<List<CommentBObj>> GetSortedCommentBObjListAsync(List<Comment> comments)
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
    }
}
