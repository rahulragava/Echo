using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Services;

namespace SocialMediaApplication.DataManager
{
    public sealed class FetchPostManager : IFetchPostManager
    {
        private static FetchPostManager Instance { get; set; }
        private static readonly object PadLock = new object();

        //public List<PostBObj> PostBobjs;
        private FetchPostManager() { }

        public static FetchPostManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new FetchPostManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly ITextPostDbHandler _textPostDbHandler = TextPostDbHandler.GetInstance;
        private readonly IPollPostDbHandler _pollPostDbHandler = PollPostDbHandler.GetInstance;
        private readonly PollChoiceManager _pollChoiceManager = PollChoiceManager.GetInstance;
        private readonly ReactionManager _reactionManager = ReactionManager.GetInstance;
        private readonly AddCommentManager _addCommentManager = AddCommentManager.GetInstance;


        public async Task<List<TextPostBObj>> GetUserTextPostBObjsAsync(string userId)
        {
            var reactions = (await _reactionManager.GetReactionAsync()).ToList();
            var commentBObjList = (await _addCommentManager.GetCommentBObjsAsync()).ToList();
            var textPosts = (await _textPostDbHandler.GetAllTextPostAsync()).Where(tp => tp.PostedBy == userId);
            var textPostBObjList = new List<TextPostBObj>();

            foreach (var textPost in textPosts)
            {
                var postCommentBObjList = commentBObjList.Where((commentBobj) => commentBobj.PostId == textPost.Id).ToList();
                var sortedCommentBObjList = GetSortedComments(postCommentBObjList);
                var postReactions = reactions.Where((reaction) => (reaction.ReactionOnId == textPost.Id)).ToList();

                var textPostBObj = new TextPostBObj
                {
                    Id = textPost.Id,
                    Title = textPost.Title,
                    PostedBy = textPost.PostedBy,
                    CreatedAt = textPost.CreatedAt,
                    FormattedCreatedTime = textPost.CreatedAt.ToString("dddd, dd MMMM yyyy"),
                    LastModifiedAt = textPost.LastModifiedAt,
                    Comments = sortedCommentBObjList,
                    FontStyle = textPost.FontStyle,
                    Reactions = postReactions,
                    Content = textPost.Content
                };
                textPostBObjList.Add(textPostBObj);
            }
            return textPostBObjList;
        }

        public async Task<List<PollPostBObj>> GetUserPollPostBObjsAsync(string userId)
        {
            var reactions = (await _reactionManager.GetReactionAsync()).ToList();
            var commentBObjList = (await _addCommentManager.GetCommentBObjsAsync()).ToList();
            var pollPosts = (await _pollPostDbHandler.GetAllPollPostAsync()).Where(pp => pp.PostedBy == userId).ToList();

            var pollChoices = (await _pollChoiceManager.GetPollChoicesBObjAsync()).ToList();

            var pollPostBObjList = new List<PollPostBObj>();
            
            foreach (var pollPost in pollPosts)
            {
                var postCommentBObjList = commentBObjList.Where((commentBobj) => commentBobj.PostId == pollPost.Id).ToList();
                var sortedCommentBObjList = GetSortedComments(postCommentBObjList);
                var postReactions = reactions.Where((reaction) => (reaction.ReactionOnId == pollPost.Id)).ToList();
                var choices = pollChoices.Where((choice) => choice.PostId == pollPost.Id).ToList();

                var pollPostBObj = new PollPostBObj()
                {
                    Id = pollPost.Id,
                    Title = pollPost.Title,
                    PostedBy = pollPost.PostedBy,
                    CreatedAt = pollPost.CreatedAt,
                    LastModifiedAt = pollPost.LastModifiedAt,
                    FormattedCreatedTime = pollPost.CreatedAt.ToString("dddd, dd MMMM yyyy"),
                    Comments = sortedCommentBObjList,
                    Reactions = postReactions,
                    Question = pollPost.Question,
                    FontStyle = pollPost.FontStyle,
                    Choices = choices
                };
                pollPostBObjList.Add(pollPostBObj);
            }
            return pollPostBObjList;
        }

        public static List<CommentBObj> GetSortedComments(List<CommentBObj> postCommentBObjs)
        {
            var comments = postCommentBObjs;
            var sortedComments = new List<CommentBObj>();

            var levelZeroComments = comments.Where(x => x.ParentCommentId == null).OrderBy(x => x.CommentedAt).ToList();
            foreach (var comment in levelZeroComments)
            {
                sortedComments.Add(comment);
                comment.Depth = 0;
                RecursiveSort(comment.Id, 1);
            }

            void RecursiveSort(string id, int depth)
            {
                var childComments = comments.Where(x => x.ParentCommentId == id).OrderBy(x => x.CommentedAt).ToList();

                foreach (var comment in childComments)
                {
                    sortedComments.Add(comment);
                    comment.Depth = depth*15;
                    //done change here
                    RecursiveSort(comment.Id, depth + 1);
                }
            }

            foreach (var sortedComment in sortedComments)
            {
                if (sortedComment.Depth > 60)
                {
                    sortedComment.Depth = 60;
                }
            }
            return sortedComments;
        }
    }
}
