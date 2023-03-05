using System;
using System.Collections.Generic;
using System.Linq;
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
            var comments = (await Task.Run(() => _commentDbHandler.GetAllCommentAsync()).ConfigureAwait(false)).ToList();
            var reactions = await _reactionManager.GetReactionAsync();
            CommentBObj commentBobj;

            for (int i = 0; i < comments.Count; i++)
            {
                var commentReactions = reactions.Where((reaction) => reaction.ReactionOnId == comments[i].Id).ToList();
                commentBobj = new CommentBObj
                {
                    Id = comments[i].Id,
                    PostId = comments[i].PostId,
                    ParentCommentId = comments[i].ParentCommentId,
                    CommentedBy = comments[i].CommentedBy,
                    CommentedAt = comments[i].CommentedAt,
                    FormattedCommentDate = comments[i].CommentedAt.ToString("dddd, dd MMMM yyyy"),
                    Content = comments[i].Content,
                    Reactions = reactions
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
                var user = await _userDbHandler.GetUserAsync(AppSettings.LocalSettings.Values["user"].ToString());
                var commentBObj = new CommentBObj()
                {
                    Id = comment.Id,
                    PostId = comment.PostId,
                    CommentedBy = user.Id,
                    CommentedAt = comment.CommentedAt,
                    FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                    Content = comment.Content,
                    CommentedUserName = user.UserName,
                    ParentCommentId = insertCommentRequest.ParentCommentId,

                };
                if (insertCommentRequest.Depth == 0 && insertCommentRequest.ParentCommentId == null)
                {
                    commentBObj.Depth = 0;
                }
                else
                {
                    commentBObj.Depth = insertCommentRequest.Depth + 10;

                }
                insertCommentUseCaseCallBack?.OnSuccess(new InsertCommentResponse(commentBObj));
            }
            catch (Exception ex)
            {
                insertCommentUseCaseCallBack?.OnError(ex);
            }
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



        //public async Task RemoveCommentsAsync(List<CommentBObj> comments)
        //{
        //    if (comments.Count <= 0 || !(comments.Any())) return;

        //    while (true)
        //    {
        //        for (var i = 0; i < comments.Count; i++)
        //        {
        //            await RemoveCommentAsync(comments[i]).ConfigureAwait(false);
        //            break;
        //        }
        //        if (comments.Count == 0) break;
        //    }
        //}

      
    }
}
