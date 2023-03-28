using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.DataManager
{
    public sealed class PollChoiceManager
    {
        private static PollChoiceManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private PollChoiceManager() { }

        public static PollChoiceManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new PollChoiceManager();
                        }
                    }
                }
                return Instance;
            }
        }

        private readonly IPollChoiceDbHandler _pollChoiceDbHandler = PollChoiceDbHandler.GetInstance;
        private readonly IUserPollChoiceSelectionDbHandler _userPollChoiceSelectionDbHandler = UserPollChoiceSelectionDbHandler.GetInstance;

        public async Task<List<PollChoiceBObj>> GetPollChoicesBObjAsync()
        {
            var pollChoiceBObjList = new List<PollChoiceBObj>();
            var pollChoices = (await _pollChoiceDbHandler.GetAllPollChoiceAsync()).ToList();
            var userPollChoiceSelections = (await _userPollChoiceSelectionDbHandler.GetAllUserPollChoiceSelectionAsync()).ToList();

            foreach (var pollChoice in pollChoices)
            {
                var pollChoiceBObj = new PollChoiceBObj();
                var pollChoiceSelectedUsers = userPollChoiceSelections
                    .Where(userPollChoiceSelection => userPollChoiceSelection.ChoiceId == pollChoice.Id).ToList();

                pollChoiceBObj.Id = pollChoice.Id;
                pollChoiceBObj.Choice = pollChoice.Choice;
                pollChoiceBObj.PostId = pollChoice.PostId;
                pollChoiceBObj.PostFontStyle = pollChoice.PostFontStyle;
                pollChoiceBObj.ChoiceSelectedUsers = pollChoiceSelectedUsers;

                pollChoiceBObjList.Add(pollChoiceBObj);
            }

            return pollChoiceBObjList;
        }
    }
}
