using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.DataManager;

namespace SocialMediaApplication.Util
{
    public class Validation
    {
        private readonly IUserDbHandler _userDbHandler = UserDbHandler.GetInstance;

        public async Task<bool> IsUserNameAlreadyExistAsync(string userName)
        {
            var userNames = await GetUserNamesAsync().ConfigureAwait(false);
            return userNames.Any() && userNames.Contains(userName);
        }

        public async Task<bool> IsUserMailAlreadyExistAsync(string userMail)
        {
            var userMailIds= await GetUserMailIdsAsync().ConfigureAwait(false);
            return userMailIds.Any() && userMailIds.Contains(userMail);
        }

        public async Task<List<string>> GetUserMailIdsAsync()
        {
            return (await _userDbHandler.GetAllUserAsync().ConfigureAwait(false)).Select(user => user.MailId).ToList();
        }

        public async Task<List<string>> GetUserNamesAsync()
        {
            return (await _userDbHandler.GetAllUserAsync().ConfigureAwait(false)).Select(user => user.UserName).ToList();
        }
    }
}
