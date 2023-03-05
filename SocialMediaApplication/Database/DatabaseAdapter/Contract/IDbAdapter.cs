using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace SocialMediaApplication.Database.DatabaseAdapter.Contract
{
    public interface IDbAdapter
    {
        Task InsertInTableAsync<T>(T obj) where T : new();
        Task InsertMultipleObjectInTableAsync<T>(List<T> objList) where T : new();
        Task RemoveObjectFromTableAsync<T>(string id) where T : new();
        Task UpdateObjectInTableAsync<T>(T obj) where T : new();
        Task<T> GetObjectFromTableAsync<T>(string id) where T : new();
        Task<IEnumerable<T>> GetAllObjectsInTableAsync<T>() where T : new ();
        Task<IEnumerable<T>> GetSpecificObjectsInTableAsync<T>(int takeAmount, int skipAmount) where T : new ();
    }
}
