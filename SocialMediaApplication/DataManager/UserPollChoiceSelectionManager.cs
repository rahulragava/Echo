using System;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
{
    public sealed class UserPollChoiceSelectionManager : IUserPollChoiceSelectionManager
    {
        private static UserPollChoiceSelectionManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private UserPollChoiceSelectionManager() { }

        public static UserPollChoiceSelectionManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new UserPollChoiceSelectionManager();
                        }
                    }
                }
                return Instance;
            }
        }
        private readonly IUserPollChoiceSelectionDbHandler _userPollChoiceSelectionDbHandler = UserPollChoiceSelectionDbHandler.GetInstance;
        private readonly IPollChoiceDbHandler _pollChoiceDbHandler = PollChoiceDbHandler.GetInstance;

        public async Task InsertPollChoiceSelectionAsync(InsertUserChoiceSelectionRequest insertUserChoiceSelectionRequest,
            InsertUserChoiceSelectionUseCaseCallBack insertUserChoiceSelectionUseCaseCallBack)
        {
            try
            {
                var userPollChoiceSelectionList =
                    (await _userPollChoiceSelectionDbHandler.GetAllUserPollChoiceSelectionAsync());

                UserPollChoiceSelection selectionChoice = null;
                foreach (var userPollChoiceSelection in userPollChoiceSelectionList)
                {
                    if (userPollChoiceSelection.SelectedBy ==
                        insertUserChoiceSelectionRequest.UserPollChoiceSelection.SelectedBy)
                    {
                        var postId = (await _pollChoiceDbHandler.GetPollChoiceAsync(userPollChoiceSelection.ChoiceId)).PostId;
                        if (postId == insertUserChoiceSelectionRequest.PostId)
                        {
                            selectionChoice = userPollChoiceSelection;
                            break;
                        }
                    }
                }
                
                //if user already voted for different choice
                if (selectionChoice != null)
                {
                    await _userPollChoiceSelectionDbHandler.RemoveUserPollChoiceSelectionAsync(selectionChoice.Id);
                    await _userPollChoiceSelectionDbHandler.InsertUserPollChoiceSelectionAsync(
                        insertUserChoiceSelectionRequest.UserPollChoiceSelection);
                }
                else
                { 
                    await _userPollChoiceSelectionDbHandler.InsertUserPollChoiceSelectionAsync(
                        insertUserChoiceSelectionRequest.UserPollChoiceSelection);
                }
                insertUserChoiceSelectionUseCaseCallBack?.OnSuccess(new InsertUserChoiceSelectionResponse(true));
            }
            catch (Exception e)
            {
                insertUserChoiceSelectionUseCaseCallBack?.OnError(e);
            }
        }

    }
}
