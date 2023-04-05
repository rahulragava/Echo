using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;

namespace SocialMediaApplication.DataManager
{
    public sealed class FeedManager : IFeedManager
    {
        private static FeedManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private FeedManager()
        {
        }

        public static FeedManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new FeedManager();
                        }
                    }
                }

                return Instance;
            }
        }

        private readonly IPollPostDbHandler _pollPostDbHandler = PollPostDbHandler.GetInstance;
        private readonly ITextPostDbHandler _textPostDbHandler = TextPostDbHandler.GetInstance;
        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;
        private readonly ICommentDbHandler _commentDbHandler = CommentDbHandler.GetInstance;
        private readonly IReactionDbHandler _reactionDbHandler = ReactionDbHandler.GetInstance;
        private readonly IPollChoiceDbHandler _pollChoiceDbHandler = PollChoiceDbHandler.GetInstance;
        private readonly IUserPollChoiceSelectionDbHandler _userPollChoiceSelectionDbHandler = UserPollChoiceSelectionDbHandler.GetInstance;

        public async Task FetchFeedAsync(FetchFeedRequest fetchFeedRequest, FetchFeedUseCaseCallBack fetchFeedUseCaseCallBack)
        {
            try
            {
                var textPosts = await _textPostDbHandler.GetSpecificPostAsync(fetchFeedRequest.TextPostFeedsFetched,
                    fetchFeedRequest.TextPostFeedsSkipped);
                var pollPosts = await _pollPostDbHandler.GetSpecificPostAsync(fetchFeedRequest.PollPostFeedsFetched,
                    fetchFeedRequest.PollPostFeedsSkipped);
                var userNames = new Dictionary<string,string>();
                if (!userNames.Any())
                {
                    var users = await _userDbHandler.GetAllUserAsync();
                    foreach (var user in users)
                    {
                        userNames.Add(user.Id,user.UserName);
                    }
                    //var userNames = users.Select(u => u.UserName).ToList();
                }
                var pollPostBObjList = new List<PollPostBObj>();
                var textPostBObjList = new List<TextPostBObj>();
                foreach (var pollPost in pollPosts)
                {
                    var commentBObjList = new List<CommentBObj>();
                    var comments = (await _commentDbHandler.GetPostCommentsAsync(pollPost.Id)).ToList();
                    var reactions = (await _reactionDbHandler.GetReactionsAsync(pollPost.Id)).ToList();
                    var choices = (await _pollChoiceDbHandler.GetPostPollChoicesAsync(pollPost.Id)).ToList();
                    var choiceBObjList = new List<PollChoiceBObj>();
                    foreach (var choice in choices)   
                    {
                        var userPollChoiceSelections = (await _userPollChoiceSelectionDbHandler.GetSelectiveUserPollChoicesSelectionAsync(choice.Id)).ToList();
                        var pollChoiceBObj = new PollChoiceBObj
                        {
                            Id = choice.Id,
                            Choice = choice.Choice,
                            PostId = choice.PostId,
                            ChoiceSelectedUsers = userPollChoiceSelections
                        };

                        choiceBObjList.Add(pollChoiceBObj);
                    }
                    foreach (var comment in comments)
                    {
                        var commentsReactions = (await _reactionDbHandler.GetReactionsAsync(comment.Id)).ToList();
                        var commentBObj = new CommentBObj
                        {
                            Id = comment.Id,
                            PostId = comment.PostId,
                            CommentedUserName = userNames[comment.CommentedBy],
                            ParentCommentId = comment.ParentCommentId,
                            CommentedBy = comment.CommentedBy,
                            CommentedAt = comment.CommentedAt,
                            FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                            Content = comment.Content,
                            Reactions = commentsReactions,
                        };
                        commentBObjList.Add(commentBObj);
                    }
                    var sortedCommentBObjList = FetchPostManager.GetSortedComments(commentBObjList);
                    
                    var pollPostBObj = new PollPostBObj()
                    {
                        Id = pollPost.Id,
                        PostedBy = pollPost.PostedBy,
                        UserName = userNames[pollPost.PostedBy],
                        CreatedAt = pollPost.CreatedAt,
                        LastModifiedAt = pollPost.LastModifiedAt,
                        FormattedCreatedTime = pollPost.CreatedAt.ToString("dddd, dd MMMM yyyy"),
                        Comments = sortedCommentBObjList,
                        Reactions = reactions,
                        Question = pollPost.Question,
                        Choices = choiceBObjList,
                    };
                    pollPostBObjList.Add(pollPostBObj);
                }
                foreach (var textPost in textPosts)
                {
                    var comments = (await _commentDbHandler.GetPostCommentsAsync(textPost.Id)).ToList();
                    var commentBObjList = new List<CommentBObj>();
                    var reactions = (await _reactionDbHandler.GetReactionsAsync(textPost.Id)).ToList();
                    
                    foreach (var comment in comments)
                    {
                        var commentsReactions = (await _reactionDbHandler.GetReactionsAsync(comment.Id)).ToList();
                        var commentBObj = new CommentBObj
                        {
                            Id = comment.Id,
                            PostId = comment.PostId,
                            ParentCommentId = comment.ParentCommentId,
                            CommentedBy = comment.CommentedBy,
                            CommentedAt = comment.CommentedAt,
                            FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                            Content = comment.Content,
                            CommentedUserName = userNames[comment.CommentedBy],
                            Reactions = commentsReactions,
                        };
                        commentBObjList.Add(commentBObj);
                    }
                    var sortedCommentBObjList = FetchPostManager.GetSortedComments(commentBObjList);
                    var textPostBObj = new TextPostBObj()
                    {
                        Id = textPost.Id,
                        PostedBy = textPost.PostedBy,
                        CreatedAt = textPost.CreatedAt,
                        UserName = userNames[textPost.PostedBy], 
                        LastModifiedAt = textPost.LastModifiedAt,
                        FormattedCreatedTime = textPost.CreatedAt.ToString("dddd, dd MMMM yyyy"),
                        Comments = sortedCommentBObjList,
                        Reactions = reactions,
                        Content= textPost.Content,
                    };
                    textPostBObjList.Add(textPostBObj);
                }


                fetchFeedUseCaseCallBack?.OnSuccess(new FetchFeedResponse(textPostBObjList, pollPostBObjList));
            }
            catch (Exception e)
            {
                fetchFeedUseCaseCallBack?.OnError(e);
            }
        }
    }
}
