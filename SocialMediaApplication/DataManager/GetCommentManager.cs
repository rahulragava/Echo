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


        public async Task GetPostComments(GetCommentRequest getCommentRequest, GetCommentUseCaseCallBack getCommentUseCaseCallBack)
        {
            try
            {
                var comments = (await _commentDbHandler.GetPostCommentsAsync(getCommentRequest.PostId)).ToList();
                List<CommentBObj> commentBObjs = new List<CommentBObj>();
                for (int i = 0; i < comments.Count; i++)
                {
                    //var commentReactions = reactions.Where((reaction) => reaction.ReactionOnId == CommentCacheList[i].Id).ToList();
                    var commentReactions = (await _reactionDbHandler.GetReactionsAsync(comments[i].Id)).ToList();
                    var userName = (await _userDbHandler.GetUserAsync(comments[i].CommentedBy)).UserName;
                    var commentBobj = new CommentBObj
                    {
                        Id = comments[i].Id,
                        PostId = comments[i].PostId,
                        ParentCommentId = comments[i].ParentCommentId,
                        CommentedUserName = userName,
                        CommentedBy = comments[i].CommentedBy,
                        CommentedAt = comments[i].CommentedAt,
                        FormattedCommentDate = comments[i].CommentedAt.ToString("dddd, dd MMMM yyyy"),
                        Content = comments[i].Content,
                        Reactions = commentReactions
                    };

                    commentBObjs.Add(commentBobj);
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
