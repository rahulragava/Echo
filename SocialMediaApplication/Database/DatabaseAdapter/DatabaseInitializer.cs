using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Audio;
using SocialMediaApplication.Models.EntityModels;
using SQLite;

namespace SocialMediaApplication.Database.DatabaseAdapter
{
    public sealed class DatabaseInitializer
    {
        private static DatabaseInitializer _dbInstance;
        private static readonly object PadLock = new object();
        
        private DatabaseInitializer()
        {
        }

        public static DatabaseInitializer Instance
        {
            get
            {
                if(_dbInstance == null)
                {
                    lock (PadLock)
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
        public SQLiteAsyncConnection Db { get; set; }

        public void InitializeDatabase()
        {
            if(Db == null)
            {
                var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SocialMediaDatabase.db3");
                Db = new SQLiteAsyncConnection(databasePath);
                Debug.WriteLine(databasePath);
                //await CreateAllTablesAsync();
            }
        }

        public async Task CreateAllTablesAsync()
        {
            await Db.CreateTableAsync<User>();
            await Db.CreateTableAsync<UserCredential>();
            await Db.CreateTableAsync<UserPollChoiceSelection>();
            await Db.CreateTableAsync<TextPost>();
            await Db.CreateTableAsync<PollPost>();
            await Db.CreateTableAsync<Reaction>();
            await Db.CreateTableAsync<Comment>();
            await Db.CreateTableAsync<PollChoice>();
            await Db.CreateTableAsync<Label>();
            await Db.CreateTableAsync<Follower>();
        }
    }
}
