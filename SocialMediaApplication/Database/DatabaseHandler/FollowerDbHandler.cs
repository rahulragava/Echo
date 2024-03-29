﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseAdapter;
using SocialMediaApplication.Database.DatabaseAdapter.Contract;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Database.DatabaseHandler
{
    public sealed  class FollowerDbHandler : IFollowerDbHandler
    {

        private static FollowerDbHandler Instance { get; set; }
        private static readonly object PadLock = new object();
        private readonly IDbAdapter _dbAdapter = DbAdapter.GetInstance;

        private FollowerDbHandler() { }

        public static FollowerDbHandler GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new FollowerDbHandler();
                        }
                    }
                }
                return Instance;
            }
        }

        //insert to db
        public async Task InsertFollowerAsync(Follower follower)
        {
            await _dbAdapter.InsertInTableAsync(follower);
        }

        //remove from db
        public async Task RemoveFollowerAsync(string followerFollowingId)
        {
            await _dbAdapter.RemoveObjectFromTableAsync<Follower>(followerFollowingId);
        }

        //edit follower field or fields
        public async Task UpdateFollowerAsync(Follower follower)
        {
            await _dbAdapter.UpdateObjectInTableAsync(follower);
        }

        //get all Followers
        public async Task<IEnumerable<Follower>> GetAllFollowerAsync()
        {
            return await _dbAdapter.GetAllObjectsInTableAsync<Follower>();
        }

        //get a particular Follower/Followee
        public async Task<Follower> GetFollowerAsync(string viewingUserId, string searchedUserId)
        {
            return (await _dbAdapter.GetAllObjectsInTableAsync<Follower>()).FirstOrDefault(f => f.FollowerId == viewingUserId && f.FollowingId == searchedUserId);
        }

        public async Task<IEnumerable<string>> GetUserFollowerIdsAsync(string userId)
        {
            return (await _dbAdapter.GetAllObjectsInTableAsync<Follower>())
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingId);
        }

        public async Task<IEnumerable<string>> GetUserFollowingIdsAsync(string userId)
        {
            return (await _dbAdapter.GetAllObjectsInTableAsync<Follower>())
                .Where(f => f.FollowingId == userId)
                .Select(f => f.FollowerId);
        }
    }
}
