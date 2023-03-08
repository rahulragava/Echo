using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;

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

        public async void InsertPollChoiceSelection(InsertUserChoiceSelectionRequest insertUserChoiceSelectionRequest,
            InsertUserChoiceSelectionUseCaseCallBack insertUserChoiceSelectionUseCaseCallBack)
        {
            try
            {
                var userPollChoiceSelectionList =
                    (await _userPollChoiceSelectionDbHandler.GetAllUserPollChoiceSelectionAsync());
                
                var selectionChoice = userPollChoiceSelectionList.SingleOrDefault(choiceSelection =>
                    choiceSelection.SelectedBy == insertUserChoiceSelectionRequest.UserPollChoiceSelection.SelectedBy &&
                    (_pollChoiceDbHandler.GetPollChoiceAsync(choiceSelection.ChoiceId).Result).PostId ==
                    insertUserChoiceSelectionRequest.PostId);

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
