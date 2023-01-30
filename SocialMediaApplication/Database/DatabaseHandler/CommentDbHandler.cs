using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed class CommentDbHandler : ICommentDbHandler
    {

        private static CommentDbHandler Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;


        private CommentDbHandler() { }

        public static CommentDbHandler GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new CommentDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert comment to db
        public async Task InsertCommentAsync(Comment comment)
        {
            await _dbAdapter.InsertInTableAsync(comment);
        }

        //remove a comment from db
        public async Task RemoveCommentAsync(string commentId)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<Comment>(commentId);
        }

        //edit a comment field or fields
        public async Task UpdateCommentAsync(Comment comment)
        {
            await _dbAdapter.UpdateObjectInTableAsync(comment);
        }

        //get all comments
        public async Task<IEnumerable<Comment>> GetAllCommentAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<Comment>();
        }

        //get a particular comment
        public async Task<Comment> GetCommentAsync(string id)
        {
            return await _dbAdapter.GetObjectFromTableAsync<Comment>(id);
        }
    }
}
