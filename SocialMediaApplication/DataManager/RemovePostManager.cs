using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;

namespace SocialMediaApplication.DataManager
{
    public sealed class RemovePostManager : IRemovePostManager
    {

        private static RemovePostManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private RemovePostManager() { }

        public static RemovePostManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new RemovePostManager();
                        }
                    }
                }
                return Instance;
            }
        }
        private readonly ICommentDbHandler _commentDbHandler = CommentDbHandler.GetInstance;
        private readonly ITextPostDbHandler _textPostDbHandler = TextPostDbHandler.GetInstance;
        private readonly IPollPostDbHandler _pollPostDbHandler= PollPostDbHandler.GetInstance;
        private readonly IReactionDbHandler _reactionDbHandler = ReactionDbHandler.GetInstance;
        //private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;
        private readonly IPollChoiceDbHandler _pollChoiceDbHandler = PollChoiceDbHandler.GetInstance;
        private readonly IUserPollChoiceSelectionDbHandler _userPollChoiceSelectionDbHandler = UserPollChoiceSelectionDbHandler.GetInstance;

        public async Task RemovePost(RemovePostRequest removePostRequest, RemovePostUseCaseCallBack removePostUseCaseCallBack)
        {
            try
            {
                if (removePostRequest.PostBObj is TextPostBObj TextPost)
                {
                    var comments = await _commentDbHandler.GetPostCommentsAsync(TextPost.Id);
                    var reactions = await _reactionDbHandler.GetReactionsAsync(TextPost.Id);
                    foreach (var comment in comments)
                    {
                        await _commentDbHandler.RemoveCommentAsync(comment.Id);
                        var commentReactions = await _reactionDbHandler.GetReactionsAsync(comment.Id);
                        foreach (var commentReaction in commentReactions)
                        {
                            await _reactionDbHandler.RemoveReactionAsync(commentReaction.Id);
                        }
                    }

                    foreach (var reaction in reactions)
                    {
                        await _reactionDbHandler.RemoveReactionAsync(reaction.Id);
                    }

                    await _textPostDbHandler.RemoveTextPostAsync(TextPost.Id);
                }

                else if (removePostRequest.PostBObj is PollPostBObj pollPost)
                {
                    var choices = await _pollChoiceDbHandler.GetPostPollChoicesAsync(pollPost.Id);
                    foreach (var choice in choices)
                    {
                        await _pollChoiceDbHandler.RemovePollChoiceAsync(choice.Id);
                        var choiceSelectedUser =
                            (await _userPollChoiceSelectionDbHandler.GetSelectiveUserPollChoicesSelectionAsync(
                                choice.Id)).ToList();
                        await _userPollChoiceSelectionDbHandler.RemoveUserPollChoiceSelectionsAsync(choiceSelectedUser);
                    }

                    var comments = await _commentDbHandler.GetPostCommentsAsync(pollPost.Id);
                    var reactions = await _reactionDbHandler.GetReactionsAsync(pollPost.Id);
                    foreach (var comment in comments)
                    {
                        await _commentDbHandler.RemoveCommentAsync(comment.Id);
                        var commentReactions = await _reactionDbHandler.GetReactionsAsync(comment.Id);
                        foreach (var commentReaction in commentReactions)
                        {
                            await _reactionDbHandler.RemoveReactionAsync(commentReaction.Id);
                        }
                    }

                    foreach (var reaction in reactions)
                    {
                        await _reactionDbHandler.RemoveReactionAsync(reaction.Id);
                    }

                    await _pollPostDbHandler.RemovePollPostAsync(pollPost.Id);
                }

                removePostUseCaseCallBack.OnSuccess(new RemovePostResponse(removePostRequest.PostBObj.Id));

            }
            catch (Exception e)
            {
                removePostUseCaseCallBack.OnError(e);

            }
        }
    }
}
