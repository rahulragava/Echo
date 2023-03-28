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
    public sealed class GetCommentManager : IGetCommentManager
    {
        private static GetCommentManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private GetCommentManager() { }

        public static GetCommentManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new GetCommentManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly ICommentDbHandler _commentDbHandler = CommentDbHandler.GetInstance;
        private readonly IReactionDbHandler _reactionDbHandler = ReactionDbHandler.GetInstance;
        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;


        public async Task GetPostCommentsAsync(GetCommentRequest getCommentRequest, GetCommentUseCaseCallBack getCommentUseCaseCallBack)
        {
            try
            {
                var comments = (await _commentDbHandler.GetPostCommentsAsync(getCommentRequest.PostId)).ToList();
                List<CommentBObj> commentBObjs = new List<CommentBObj>();
                foreach (var comment in comments)
                {
                    //var commentReactions = reactions.Where((reaction) => reaction.ReactionOnId == CommentCacheList[i].Id).ToList();
                    var commentReactions = (await _reactionDbHandler.GetReactionsAsync(comment.Id)).ToList();
                    var userName = (await _userDbHandler.GetUserAsync(comment.CommentedBy)).UserName;
                    var commentBObj = new CommentBObj
                    {
                        Id = comment.Id,
                        PostId = comment.PostId,
                        ParentCommentId = comment.ParentCommentId,
                        CommentedUserName = userName,
                        CommentedBy = comment.CommentedBy,
                        CommentedAt = comment.CommentedAt,
                        FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                        Content = comment.Content,
                        Reactions = commentReactions
                    };

                    commentBObjs.Add(commentBObj);
                }
                var commentList = FetchPostManager.GetSortedComments(commentBObjs);
                getCommentUseCaseCallBack?.OnSuccess(new GetCommentResponse(commentList));
            }
            catch (Exception e)
            {
                getCommentUseCaseCallBack?.OnError(e);

            }
        }
    }
}
