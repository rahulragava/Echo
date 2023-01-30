using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;

namespace SocialMediaApplication.Services
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
            var pollChoiceBobjs = new List<PollChoiceBObj>();
            var pollChoices = (await Task.Run(()=> _pollChoiceDbHandler.GetAllPollChoiceAsync()).ConfigureAwait(false)).ToList();
            var userPollChoiceSelections = (await Task.Run(() => _userPollChoiceSelectionDbHandler.GetAllUserPollChoiceSelectionAsync()).ConfigureAwait(false)).ToList();

            foreach (var pollChoice in pollChoices)
            {
                var pollChoiceBobj = new PollChoiceBObj();
                var pollChoiceSelectedUsers = userPollChoiceSelections
                    .Where(userPollChoiceSelection => userPollChoiceSelection.ChoiceId == pollChoice.Id).ToList();


                pollChoiceBobj.Id = pollChoice.Id;
                pollChoiceBobj.Choice = pollChoice.Choice;
                pollChoiceBobj.PostId = pollChoice.PostId;
                pollChoiceBobj.ChoiceSelectedUsers = pollChoiceSelectedUsers;

                pollChoiceBobjs.Add(pollChoiceBobj);
            }

            return pollChoiceBobjs;
        }

        public async Task AddPollChoiceAsync(PollChoiceBObj pollChoice)
        {
            var pollChoiceEntityModel = ConvertPollChoiceBObjToEntityModel(pollChoice);
            await Task.Run(() => _pollChoiceDbHandler.InsertPollChoiceAsync(pollChoiceEntityModel));
        }

        public PollChoice ConvertPollChoiceBObjToEntityModel(PollChoiceBObj pollChoiceBobj)
        {
            PollChoice pollChoice = new PollChoice();
            pollChoice.Id = pollChoiceBobj.Id;
            pollChoice.Choice = pollChoiceBobj.Choice;
            pollChoice.PostId = pollChoiceBobj.PostId;

            return pollChoice;
        }

        public async Task RemovePollChoiceAsync(PollChoice pollChoice)
        {
            await Task.Run(() => _pollChoiceDbHandler.RemovePollChoiceAsync(pollChoice.Id));
        }

        public async Task AddPollChoicesAsync(List<PollChoiceBObj> pollChoiceBobjList)
        {
            foreach (var pollChoice in pollChoiceBobjList)
            {
                if (pollChoice.ChoiceSelectedUsers != null && pollChoice.ChoiceSelectedUsers.Any())
                {
                    await Task.Run(() => _userPollChoiceSelectionDbHandler.InsertUserPollChoiceSelectionsAsync(pollChoice.ChoiceSelectedUsers)).ConfigureAwait(false);
                }
                await AddPollChoiceAsync(pollChoice);
            }
        }

        public async Task RemovePollChoicesAsync(List<PollChoiceBObj> choices)
        {
            foreach (var pollChoice in choices)
            {
                if (pollChoice.ChoiceSelectedUsers != null && pollChoice.ChoiceSelectedUsers.Count > 0)
                    await Task.Run(() => _userPollChoiceSelectionDbHandler.RemoveUserPollChoiceSelectionsAsync(pollChoice.ChoiceSelectedUsers));
                await RemovePollChoiceAsync(ConvertPollChoiceBObjToEntityModel(pollChoice));
            }
        }

        public async Task AddChoiceSelectedUser(UserPollChoiceSelection userSelectionPollChoice)
        {
            if (userSelectionPollChoice != null)
            {
                await Task.Run(() => _userPollChoiceSelectionDbHandler.InsertUserPollChoiceSelectionAsync(userSelectionPollChoice)).ConfigureAwait(false);
            }
        }

        public async Task<List<PollChoice>> GetPollChoiceAsync() => (await Task.Run(() => _pollChoiceDbHandler.GetAllPollChoiceAsync()).ConfigureAwait(false)).ToList();
        
    }
}
