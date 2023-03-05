using SocialMediaApplication.Domain.UseCase;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface ICreatePostManager
    {
        void CreateTextPostAsync(TextPostCreationRequest textPostCreationRequest,
            TextPostCreationUseCaseCallBack textPostCreationUseCaseCallBack);

        void CreatePollPostAsync(PollPostCreationRequest pollPostCreationRequest,
            PollPostCreationUseCaseCallBack pollPostCreationUseCaseCallBack);
    }
}
