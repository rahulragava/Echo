using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;
using Windows.System;

namespace SocialMediaApplication.Database.DatabaseAdapter
{
    public class DbAdapter : IDbAdapter
    {
        private static DbAdapter Instance { get; set; }
        private static readonly object PadLock = new object();

        DbAdapter() { }

        public static DbAdapter GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new DbAdapter();
                        }
                    }
                }
                return Instance;
            }
        }

        public async Task InsertInTableAsync<T>(T obj) where T : new()
        {
            await DatabaseInitializer.Instance.InitializeDatabase();
            await DatabaseInitializer.Instance._db.InsertAsync(obj);
        }

        public async Task InsertMultipleObjectInTableAsync<T>(List<T> objList) where T : new()
        {
            await DatabaseInitializer.Instance.InitializeDatabase();
            await DatabaseInitializer.Instance._db.InsertAllAsync(objList, true);
        }

        public async Task RemoveObjectFromTableAsync<T>(string id) where T : new()
        {
            await DatabaseInitializer.Instance.InitializeDatabase();
            await DatabaseInitializer.Instance._db.DeleteAsync<T>(id);
        }

        public async Task UpdateObjectInTableAsync<T>(T obj) where T : new()
        {
            await DatabaseInitializer.Instance.InitializeDatabase();
            await DatabaseInitializer.Instance._db.UpdateAsync(obj);
        }

        public async Task<T> GetObjectFromTableAsync<T>(string id) where T : new()
        {
            await DatabaseInitializer.Instance.InitializeDatabase();
            return await DatabaseInitializer.Instance._db.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAllObjectsInTableAsync<T>() where T : new()
        {
            await DatabaseInitializer.Instance.InitializeDatabase();
            return await DatabaseInitializer.Instance._db.Table<T>().ToListAsync();
        }

    }
}
