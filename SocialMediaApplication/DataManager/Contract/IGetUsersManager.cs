using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataTasks;
using SocialMediaApplication.Domain.UseCase;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface IGetUsersManager
    {
        Task GetUserAsync(GetUserRequestObj getUserRequestObj, GetUserUseCaseCallBack getUserUseCaseCallBack);
    }
}
