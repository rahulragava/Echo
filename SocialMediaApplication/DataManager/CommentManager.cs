using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Services;

namespace SocialMediaApplication.DataManager
{
    public sealed class CommentManager
    {
        private static CommentManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private CommentManager() { }

        public static CommentManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new CommentManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly ICommentDbHandler _commentDbHandler = CommentDbHandler.GetInstance;
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
                    Content = comments[i].Content,
                    Reactions = reactions
                };

                commentBObjs.Add(commentBobj);
            }
            return commentBObjs;
        }

        public async Task AddCommentAsync(CommentBObj comment)
        {
            await Task.Run(() => _commentDbHandler.InsertCommentAsync(ConvertCommentBObjToEntity(comment))).ConfigureAwait(false);
        }

        public async Task RemoveCommentAsync(CommentBObj comment)
        {
            if (comment.Reactions != null && comment.Reactions.Any())
                await _reactionManager.RemoveReactionsAsync(comment.Reactions).ConfigureAwait(false);
            var entityComment = ConvertCommentBObjToEntity(comment);
            await Task.Run(() => _commentDbHandler.RemoveCommentAsync(entityComment.Id)).ConfigureAwait(false);
        }

        private Comment ConvertCommentBObjToEntity(CommentBObj commentBobj)
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

        public async Task RemoveCommentsAsync(List<CommentBObj> comments)
        {
            if (comments.Count <= 0 || !(comments.Any())) return;

            while (true)
            {
                for (var i = 0; i < comments.Count; i++)
                {
                    await RemoveCommentAsync(comments[i]).ConfigureAwait(false);
                    break;
                }
                if (comments.Count == 0) break;
            }
        }
        
    }
}
