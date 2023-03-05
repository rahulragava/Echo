using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed  class TextPostDbHandler: ITextPostDbHandler
    {

        private static TextPostDbHandler Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;

        TextPostDbHandler() { }

        public static TextPostDbHandler GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new TextPostDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert text post to db
        public async Task InsertTextPostAsync(TextPost textPost)
        {
            await _dbAdapter.InsertInTableAsync(textPost);
        }

        //remove a text post from db
        public async Task RemoveTextPostAsync(string textPostId)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<TextPost>(textPostId);

        }

        //edit a text post field or fields
        public async Task UpdateTextPostAsync(TextPost textPost)
        {
            await _dbAdapter.UpdateObjectInTableAsync(textPost);
        }

        //get all text posts
        public async Task<IEnumerable<TextPost>> GetAllTextPostAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<TextPost>();
        }

        //get a particular text post
        public async Task<TextPost> GetTextPostAsync(string id)
        {
            return await _dbAdapter.GetObjectFromTableAsync<TextPost>(id);
        }

        public async Task<IEnumerable<TextPost>> GetUserTextPostsAsync(string userId)
        {
            DatabaseInitializer.Instance.InitializeDatabase();
            return await DatabaseInitializer.Instance.Db.Table<TextPost>().Where(tp => tp.PostedBy == userId).ToListAsync();
        }

        public async Task<IEnumerable<TextPost>> GetSpecificPostAsync(int takeAmount, int skipAmount)
        {
            return (await _dbAdapter.GetSpecificObjectsInTableAsync<TextPost>(takeAmount, skipAmount)).ToList();
        }
    }
}
