//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using SocialMediaApplication.Database.DatabaseHandler;
//using SocialMediaApplication.Database.DatabaseHandler.Contract;
//using SocialMediaApplication.Models.EntityModels;
//using SocialMediaApplication.Services;

//namespace SocialMediaApplication.DataManager
//{
//    public sealed class LabelManager
//    {
//        private static LabelManager Instance { get; set; }
//        private static readonly object PadLock = new object();

//        LabelManager() { }

//        public static LabelManager GetInstance
//        {
//            get
//            {
//                if (Instance == null)
//                {
//                    lock (PadLock)
//                    {
//                        if (Instance == null)
//                        {
//                            Instance = new LabelManager();
//                        }
//                    }
//                }
//                return Instance;
//            }
//        }

//        private readonly ILabelDbHandler _labelDbHandler= LabelDbHandler.GetInstance;

//        public async Task AddLabelAsync(Label label)
//        {
//            if (label == null)
//            {
//                return;
//            }
//            await Task.Run(() => _labelDbHandler.InsertLabelAsync(label)).ConfigureAwait(false);
//        }

//        public async Task<List<Label>> GetLabelsAsync() => (await Task.Run(() => _labelDbHandler.GetAllLabelAsync()).ConfigureAwait(false)).ToList();

//        public async Task<List<Label>> GetUserLabelsAsync(string userId)
//        {
//            var labels = await GetLabelsAsync();
//            var userLabels = labels.Where(label => FetchPostManager.GetInstance.GetUserIdAsync(label.PostId).Result == userId).ToList();

//            return userLabels;
//        }

//        public async Task RemoveLabelAsync(Label label)
//        {
//            if (label == null)
//            {
//                return;
//            }
//            await Task.Run(()=> _labelDbHandler.RemoveLabelAsync(label.Id));
//        }

//        public async Task RemoveLabelsAsync(string postId)
//        {
//            var labels = await GetLabelsAsync();
//            var removableLabels = labels.Where(label => label.PostId == postId).ToList();
//            if (removableLabels.Count <= 0 || !(removableLabels.Any())) return;

//            while (true)
//            {
//                for (int i = 0; i < removableLabels.Count; i++)
//                {
//                    await RemoveLabelAsync(removableLabels[i]).ConfigureAwait(false);
//                    break;
//                }
//                if (labels.Count == 0) break;
//            }
//        }
//    }
//}
