using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed class LabelDbHandler : ILabelDbHandler
    {

        private static LabelDbHandler Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;


        LabelDbHandler() { }

        public static LabelDbHandler GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new LabelDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert a label to db
        public async Task InsertLabelAsync(Label label)
        {
            await _dbAdapter.InsertInTableAsync(label);
        }

        //remove a label from db
        public async Task RemoveLabelAsync(string labelId)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<Label>(labelId);
        }

        //edit a label field or fields
        public async Task UpdateLabelAsync(Label label)
        {
            await _dbAdapter.UpdateObjectInTableAsync(label);
        }

        //get all labels
        public async Task<IEnumerable<Label>> GetAllLabelAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<Label>();
        }

        //get a particular Label
        public async Task<Label> GetLabelAsync(string labelId)
        {
            return await _dbAdapter.GetObjectFromTableAsync<Label>(labelId);
        }

       
    }
}
