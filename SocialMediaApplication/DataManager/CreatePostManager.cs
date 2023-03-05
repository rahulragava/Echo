using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;

namespace SocialMediaApplication.DataManager
{
    public class CreatePostManager : ICreatePostManager
    {
        private static CreatePostManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private CreatePostManager() { }

        public static CreatePostManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new CreatePostManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly ITextPostDbHandler _textPostDbHandler = TextPostDbHandler.GetInstance;
        private readonly IPollPostDbHandler _pollPostDbHandler = PollPostDbHandler.GetInstance;
        private readonly IPollChoiceDbHandler _pollChoiceDbHandler = PollChoiceDbHandler.GetInstance;

        public async void CreateTextPostAsync(TextPostCreationRequest textPostCreationRequest, TextPostCreationUseCaseCallBack textPostCreationUseCaseCallBack)
        {
            try
            {
                await _textPostDbHandler.InsertTextPostAsync(textPostCreationRequest.TextPost);
                textPostCreationUseCaseCallBack?.OnSuccess(new TextPostCreationResponse(true));
            }
            catch (Exception e)
            {
                textPostCreationUseCaseCallBack?.OnError(e);

            }
        }

        public async void CreatePollPostAsync(PollPostCreationRequest pollPostCreationRequest, PollPostCreationUseCaseCallBack pollPostCreationUseCaseCallBack)
        {
            try
            {
                await _pollPostDbHandler.InsertPollPostAsync(pollPostCreationRequest.PollPost);
                await _pollChoiceDbHandler.InsertPollChoicesAsync(pollPostCreationRequest.Choices);
                pollPostCreationUseCaseCallBack?.OnSuccess(new PollPostCreationResponse(true));
            }
            catch (Exception e)
            {
                pollPostCreationUseCaseCallBack?.OnError(e);
            }

        }
    }
}
