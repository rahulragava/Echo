using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts.DataProvider;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Services.Contract;

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
        private static Dictionary<string,string> _userNames = new Dictionary<string,string>();

        public async Task FetchFeedAsync(FetchFeedRequest fetchFeedRequest, FetchFeedUseCaseCallBack fetchFeedUseCaseCallBack)
        {
            try
            {
                var textPosts = await _textPostDbHandler.GetSpecificPostAsync(fetchFeedRequest.TextPostFeedsFetched,
                    fetchFeedRequest.TextPostFeedsSkipped);
                var pollPosts = await _pollPostDbHandler.GetSpecificPostAsync(fetchFeedRequest.PollPostFeedsFetched,
                    fetchFeedRequest.PollPostFeedsSkipped);
                if (!_userNames.Any())
                {
                    var users = await _userDbHandler.GetAllUserAsync();
                    foreach (var user in users)
                    {
                        _userNames.Add(user.Id,user.UserName);
                    }
                    //var userNames = users.Select(u => u.UserName).ToList();
                }
                var pollPostBObjs = new List<PollPostBObj>();
                var textPostBObjs = new List<TextPostBObj>();
                foreach (var pollPost in pollPosts)
                {
                    var commentBObjs = new List<CommentBObj>();
                    var comments = (await _commentDbHandler.GetPostCommentsAsync(pollPost.Id)).ToList();
                    var reactions = (await _reactionDbHandler.GetReactionsAsync(pollPost.Id)).ToList();
                    var choices = (await _pollChoiceDbHandler.GetPostPollChoicesAsync(pollPost.Id)).ToList();
                    var choiceBObjs = new List<PollChoiceBObj>();
                    foreach (var choice in choices)   
                    {
                        var userPollChoiceSelections = (await _userPollChoiceSelectionDbHandler.GetSelectiveUserPollChoicesSelectionAsync(choice.Id)).ToList();
                        var pollChoiceBobj = new PollChoiceBObj();
                        
                        pollChoiceBobj.Id = choice.Id;
                        pollChoiceBobj.Choice = choice.Choice;
                        pollChoiceBobj.PostId = choice.PostId;
                        pollChoiceBobj.ChoiceSelectedUsers = userPollChoiceSelections;

                        choiceBObjs.Add(pollChoiceBobj);
                    }
                    foreach (var comment in comments)
                    {
                        var commentsReactions = (await _reactionDbHandler.GetReactionsAsync(comment.Id)).ToList();
                        var commentBobj = new CommentBObj
                        {
                            Id = comment.Id,
                            PostId = comment.PostId,
                            CommentedUserName = _userNames[comment.CommentedBy],
                            ParentCommentId = comment.ParentCommentId,
                            CommentedBy = comment.CommentedBy,
                            CommentedAt = comment.CommentedAt,
                            FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                            Content = comment.Content,
                            Reactions = commentsReactions,
                        };
                        commentBObjs.Add(commentBobj);
                    }
                    var sortedCommentBobjs = FetchPostManager.GetSortedComments(commentBObjs);
                    
                    var pollPostBObj = new PollPostBObj()
                    {
                        Id = pollPost.Id,
                        Title = pollPost.Title,
                        PostedBy = pollPost.PostedBy,
                        UserName = _userNames[pollPost.PostedBy],
                        CreatedAt = pollPost.CreatedAt,
                        LastModifiedAt = pollPost.LastModifiedAt,
                        FormattedCreatedTime = pollPost.CreatedAt.ToString("dddd, dd MMMM yyyy"),
                        Comments = sortedCommentBobjs,
                        Reactions = reactions,
                        Question = pollPost.Question,
                        Choices = choiceBObjs,
                    };
                    pollPostBObjs.Add(pollPostBObj);
                }
                foreach (var textPost in textPosts)
                {
                    var comments = (await _commentDbHandler.GetPostCommentsAsync(textPost.Id)).ToList();
                    var commentBObjs = new List<CommentBObj>();
                    var reactions = (await _reactionDbHandler.GetReactionsAsync(textPost.Id)).ToList();
                    
                    foreach (var comment in comments)
                    {
                        var commentsReactions = (await _reactionDbHandler.GetReactionsAsync(comment.Id)).ToList();
                        var commentBobj = new CommentBObj
                        {
                            Id = comment.Id,
                            PostId = comment.PostId,
                            ParentCommentId = comment.ParentCommentId,
                            CommentedBy = comment.CommentedBy,
                            CommentedAt = comment.CommentedAt,
                            FormattedCommentDate = comment.CommentedAt.ToString("dddd, dd MMMM yyyy"),
                            Content = comment.Content,
                            CommentedUserName = _userNames[comment.CommentedBy],
                            Reactions = commentsReactions,
                        };
                        commentBObjs.Add(commentBobj);
                    }
                    var sortedCommentBobjs = FetchPostManager.GetSortedComments(commentBObjs);
                    var textPostBObj = new TextPostBObj()
                    {
                        Id = textPost.Id,
                        Title = textPost.Title,
                        PostedBy = textPost.PostedBy,
                        CreatedAt = textPost.CreatedAt,
                        UserName = _userNames[textPost.PostedBy], 
                        LastModifiedAt = textPost.LastModifiedAt,
                        FormattedCreatedTime = textPost.CreatedAt.ToString("dddd, dd MMMM yyyy"),
                        Comments = sortedCommentBobjs,
                        Reactions = reactions,
                        Content= textPost.Content,
                    };
                    textPostBObjs.Add(textPostBObj);
                }


                fetchFeedUseCaseCallBack?.OnSuccess(new FetchFeedResponse(textPostBObjs, pollPostBObjs));
            }
            catch (Exception e)
            {
                fetchFeedUseCaseCallBack?.OnError(e);
            }
        }
    }
}
