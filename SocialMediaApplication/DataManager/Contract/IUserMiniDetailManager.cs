using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Domain.UseCase;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface IUserMiniDetailManager
    {
        Task GetUserMiniDetails(UserMiniDetailRequest userMiniDetailRequest,
            UserMiniDetailUseCaseCallBack userMiniDetailUseCaseCallBack);
    }
}
