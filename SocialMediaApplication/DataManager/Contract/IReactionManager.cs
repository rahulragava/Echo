using SocialMediaApplication.Domain.UseCase;
using System.Threading.Tasks;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface IReactionManager
    {
        Task AddReactionAsync(ReactionToPostRequestObj reactionToPostRequestObj,
            ReactionToPostUseCaseCallBack reactionToPostUseCaseCallBack);

        Task GetUserReaction(GetUserReactionRequest getUserReactionRequest,
            GetUserReactionUseCaseCallBack getUserReactionUseCallBack);

    }
}
