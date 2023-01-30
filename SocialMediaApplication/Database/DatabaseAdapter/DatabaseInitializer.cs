using System;
using System.IO;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;
using SQLite;

namespace SocialMediaApplication.Database.DatabaseAdapter
{
    public sealed class DatabaseInitializer
    {
        private static DatabaseInitializer _dbInstance;
        private static object _padLock = new object();
        
        private DatabaseInitializer()
        {
        }

        public static DatabaseInitializer Instance
        {
            get
            {
                if(_dbInstance == null)
                {
                    lock (_padLock)
                    {
                        if(_dbInstance == null)
                        {
                            _dbInstance = new DatabaseInitializer();
                        }
                    }
                }
                return _dbInstance;
            }
        }
        public SQLiteAsyncConnection _db { get; set; }

        public async Task InitializeDatabase()
        {
            if(_db == null)
            {
                var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SocialMediaDatabase.db3");
                _db = new SQLiteAsyncConnection(databasePath);
                await _db.CreateTableAsync<User>();
                await _db.CreateTableAsync<UserCredential>();
                await _db.CreateTableAsync<UserPollChoiceSelection>();
                await _db.CreateTableAsync<TextPost>();
                await _db.CreateTableAsync<PollPost>();
                await _db.CreateTableAsync<Reaction>();
                await _db.CreateTableAsync<Comment>();
                await _db.CreateTableAsync<PollChoice>();
                await _db.CreateTableAsync<Label>();
                await _db.CreateTableAsync<Follower>();
            }
        }
    }
}
