using System.Threading.Tasks;
using SocialMediaApplication.Domain.UseCase;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface ICreatePostManager
    {
        Task CreateTextPostAsync(TextPostCreationRequest textPostCreationRequest,
            TextPostCreationUseCaseCallBack textPostCreationUseCaseCallBack);

        Task CreatePollPostAsync(PollPostCreationRequest pollPostCreationRequest,
            PollPostCreationUseCaseCallBack pollPostCreationUseCaseCallBack);
    }
}
