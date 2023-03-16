﻿using System.Collections.Generic;
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



        //public async Task AddPostAsync(PostBObj postBObj)
        //{
        //    var post = ConvertBObjToEntityModel(postBObj);
        //    if (postBObj is TextPostBObj textPostBObj)
        //    {
        //        var textPost = post as TextPost;
        //        await Task.Run(() => _textPostDbHandler.InsertTextPostAsync(textPost)).ConfigureAwait(false);
        //        PostBobjs?.Add(textPostBObj);
        //    }
        //    else if (postBObj is PollPostBObj pollPostBobj)
        //    {
        //        var pollPost = post as PollPost;
        //        await Task.Run(() => _pollPostDbHandler.InsertPollPostAsync(pollPost));

        //        if (pollPostBobj.Choices != null)
        //        {
        //            await _pollChoiceManager.AddPollChoicesAsync(pollPostBobj.Choices);
        //        }
        //        PostBobjs?.Add(pollPostBobj);
        //    }
        //}

        //private Post ConvertBObjToEntityModel(PostBObj postBObj)
        //{
        //    Post post;
        //    if(postBObj is TextPostBObj)
        //    {
        //        post = new TextPost();
        //    }
        //    else
        //    {
        //        post = new PollPost();  
        //    }

        //    post.Id = postBObj.Id;
        //    post.Title = postBObj.Title;
        //    post.PostedBy = postBObj.PostedBy;
        //    post.CreatedAt = postBObj.CreatedAt;
        //    post.LastModifiedAt = postBObj.LastModifiedAt;

        //    if (postBObj is TextPostBObj textPostBObj)
        //    {
        //        var textPost = (TextPost)post;
        //        textPost.Content = textPostBObj.Content;

        //        return textPost;
        //    }
        //    else if (postBObj is PollPostBObj pollPostBObj)
        //    {
        //        var pollPost = post as PollPost;
        //        pollPost.Question = pollPostBObj.Question;

        //        return pollPost;
        //    }
        //    return post;
        //}

        //private async Task<PostBObj> ConvertEntityToBObj(Post post, List<CommentBObj> CommentCacheList, List<Reaction> reactions)
        //{
        //    PostBObj postBObj;

        //    if (post is TextPost)
        //    {
        //        postBObj = new TextPostBObj();
        //    }
        //    else
        //    {
        //        postBObj = new PollPostBObj();
        //    }

        //    postBObj.Id = post.Id;
        //    postBObj.PostedBy = post.PostedBy;
        //    postBObj.ReactionList = reactions;
        //    postBObj.Comments = CommentCacheList;
        //    postBObj.Title = post.Title;
        //    postBObj.FormattedCreatedTime = post.CreatedAt.ToString("dddd, dd MMMM yyyy");
        //    postBObj.CreatedAt = post.CreatedAt;
        //    postBObj.LastModifiedAt = post.LastModifiedAt;

        //    if (postBObj is TextPostBObj textPostBObj)
        //    {
        //        var textPost = post as TextPost;
        //        textPostBObj.Content = textPost.Content;

        //        return textPostBObj;
        //    }
        //    else if (postBObj is PollPostBObj pollPostBObj)
        //    {
        //        var choices = (await _pollChoiceManager.GetPollChoicesBObjAsync()).Where(choice => choice.ReactionOnId == post.Id).ToList();
        //        PollPost pollPost = post as PollPost;
        //        if (pollPost != null) pollPostBObj.Question = pollPost.Question;
        //        pollPostBObj.Choices = choices;

        //        return pollPostBObj;
        //    }

        //    return postBObj;
        //}

        //public async Task RemovePostAsync(PostBObj postBObj)
        //{
        //    var post = ConvertBObjToEntityModel(postBObj);
        //    if (post is PollPost)
        //    {
        //        var pollPostBObj = postBObj as PollPostBObj;
        //        var pollPost = post as PollPost;
        //        if (pollPostBObj != null)
        //        {
        //            await _pollChoiceManager.RemovePollChoicesAsync(pollPostBObj.Choices);
        //            await Task.Run(() => _pollPostDbHandler.RemovePollPostAsync(pollPost.Id)).ConfigureAwait(false);
        //            PostBobjs?.Remove(pollPostBObj);
        //        }
        //    }
        //    else if (post is TextPost)
        //    {
        //        var textPost = post as TextPost;
        //        await Task.Run(() => _textPostDbHandler.RemoveTextPostAsync(textPost.Id)).ConfigureAwait(false);
        //        PostBobjs?.Remove(postBObj as TextPostBObj);
        //    }
        //    //if (postBObj.ReactionList != null && postBObj.ReactionList.Any())
        //    //    await _reactionManager.RemoveReactionsAsync(postBObj.ReactionList);
        //    //if (postBObj.Comments != null && postBObj.Comments.Any())
        //    //    await _addCommentManager.RemoveCommentsAsync(postBObj.Comments);
        //}

        //public async Task EditPostAsync(PostBObj postBObj)
        //{
        //    var editedPost = ConvertBObjToEntityModel(postBObj);
        //    if (editedPost is PollPost)
        //    {
        //        await Task.Run(() => _pollPostDbHandler.UpdatePollPostAsync(editedPost as PollPost)).ConfigureAwait(false);
        //        if (PostBobjs != null)
        //        {
        //            PostBobjs[PostBobjs.IndexOf(PostBobjs.Single(p => p.Id == postBObj.Id))] = postBObj as PollPostBObj;
        //        }
        //    }
        //    else
        //    {
        //        await Task.Run(() => _textPostDbHandler.UpdateTextPostAsync(editedPost as TextPost)).ConfigureAwait(false);
        //        if (PostBobjs != null)
        //        {
        //            PostBobjs[PostBobjs.IndexOf(PostBobjs.Single(p => p.Id == postBObj.Id))] = postBObj as TextPostBObj;
        //        }
        //    }
        //}

        //public async Task<TextPostBObj> GetTextPostBObjAsync(string postId)
        //{
        //    if (PostBobjs != null)
        //    {
        //        return PostBobjs.OfType<TextPostBObj>().Single(p => p.Id == postId);
        //    }
            
        //    var textPost = (await Task.Run(() => _textPostDbHandler.GetAllTextPostAsync()).ConfigureAwait(false)).Single(tp => tp.Id == postId);
        //    var CommentCacheList = (await _addCommentManager.GetCommentBObjsAsync()).Where(comment => comment.ReactionOnId == postId).ToList();
        //    var reactions = (await _reactionManager.GetReactionAsync()).Where(reaction => reaction.ReactionOnId == postId).ToList();
        //    var textPostBObj = await ConvertEntityToBObj(textPost, CommentCacheList, reactions);
            
        //    return textPostBObj as TextPostBObj;
        //}

        //public async Task<PollPostBObj> GetPollPostBObjAsync(string postId)
        //{
        //    if (PostBobjs != null)
        //    {
        //        return PostBobjs.OfType<PollPostBObj>().Single(p => p.Id == postId);
        //    }
        //    var pollPost = (await Task.Run(() => _pollPostDbHandler.GetAllPollPostAsync()).ConfigureAwait(false)).Single(pp => pp.Id == postId);
        //    var CommentCacheList = (await _addCommentManager.GetCommentBObjsAsync()).Where(comment => comment.ReactionOnId == postId).ToList();
        //    var reactions = (await _reactionManager.GetReactionAsync()).Where(reaction => reaction.ReactionOnId == postId).ToList();
        //    var pollPostBObj = await ConvertEntityToBObj(pollPost, CommentCacheList, reactions);

        //    return pollPostBObj as PollPostBObj;
        //}

        //public async Task<string> GetUserIdAsync(string postId)
        //{
        //    if (PostBobjs != null)
        //    {
        //        return PostBobjs.SingleOrDefault(post => post.Id == postId)?.PostedBy;
        //    }
        //    var textPosts = (await Task.Run(() => _textPostDbHandler.GetAllTextPostAsync()).ConfigureAwait(false)).ToList();
        //    var pollPosts = (await Task.Run(() => _pollPostDbHandler.GetAllPollPostAsync()).ConfigureAwait(false)).ToList();
        //    var userId = textPosts.Single(post => post.Id == postId).PostedBy;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        userId = pollPosts.Single(post => post.Id == postId).PostedBy;
        //    }

        //    return userId;
        //}


        //public async Task<List<PostBObj>> GetUserPostBObjsAsync(string userId)
        //{
        //    //if (PostBobjs != null)
        //    //{
        //    //    return PostBobjs;
        //    //}

        //    var textPostBObjs = await GetUserTextPostBObjsAsync(userId);
        //    var pollPostBObjs = await GetUserPollPostBObjsAsync(userId);
        //    PostBobjs = new List<PostBObj>();
        //    PostBobjs.AddRange(textPostBObjs);
        //    PostBobjs.AddRange(pollPostBObjs);
            
        //    return PostBobjs;
        //}

        public async Task<List<TextPostBObj>> GetUserTextPostBObjsAsync(string userId)
        {
            var reactions = (await _reactionManager.GetReactionAsync()).ToList();
            var commentBObjs = (await _addCommentManager.GetCommentBObjsAsync()).ToList();
            var textPosts = (await _textPostDbHandler.GetAllTextPostAsync()).Where(tp => tp.PostedBy == userId);
            var textPostBObjs = new List<TextPostBObj>();

            foreach (var textPost in textPosts)
            {
                var postCommentBObjs = commentBObjs.Where((commentBobj) => commentBobj.PostId == textPost.Id).ToList();
                var sortedCommentBobjs = GetSortedComments(postCommentBObjs);
                var postReactions = reactions.Where((reaction) => (reaction.ReactionOnId == textPost.Id)).ToList();

                var textPostBObj = new TextPostBObj
                {
                    Id = textPost.Id,
                    Title = textPost.Title,
                    PostedBy = textPost.PostedBy,
                    CreatedAt = textPost.CreatedAt,
                    FormattedCreatedTime = textPost.CreatedAt.ToString("dddd, dd MMMM yyyy"),
                    LastModifiedAt = textPost.LastModifiedAt,
                    Comments = sortedCommentBobjs,
                    FontStyle = textPost.FontStyle,
                    Reactions = postReactions,
                    Content = textPost.Content
                };
                textPostBObjs.Add(textPostBObj);
            }
            return textPostBObjs;
        }

        public async Task<List<PollPostBObj>> GetUserPollPostBObjsAsync(string userId)
        {
            var reactions = (await _reactionManager.GetReactionAsync()).ToList();
            var commentBObjs = (await _addCommentManager.GetCommentBObjsAsync()).ToList();
            var pollPosts = (await _pollPostDbHandler.GetAllPollPostAsync()).Where(pp => pp.PostedBy == userId).ToList();
            //var pollPostsTesting = (await _pollPostDbHandler.GetAllPollPostAsync()).ToList();

            var pollChoices = (await _pollChoiceManager.GetPollChoicesBObjAsync()).ToList();

            var pollPostBObjs = new List<PollPostBObj>();
            
            foreach (var pollPost in pollPosts)
            {
                var postCommentBObjs = commentBObjs.Where((commentBobj) => commentBobj.PostId == pollPost.Id).ToList();
                var sortedCommentBobjs = GetSortedComments(postCommentBObjs);
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
                    Comments = sortedCommentBobjs,
                    Reactions = postReactions,
                    Question = pollPost.Question,
                    FontStyle = pollPost.FontStyle,
                    Choices = choices
                };
                pollPostBObjs.Add(pollPostBObj);
            }
            return pollPostBObjs;
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
