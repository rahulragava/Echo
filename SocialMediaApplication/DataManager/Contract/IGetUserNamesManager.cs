using SocialMediaApplication.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface IGetUserNamesManager
    {
        Task GetUserNamesAsync(GetUserNamesRequestObj getUserNameRequestObj, GetUserNamesUseCaseCallBack getUserNamesUseCaseCallBack);
    }
}
