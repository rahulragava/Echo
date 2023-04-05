using SocialMediaApplication.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface ILoginManager
    {
        Task LoginUserAsync(LoginRequest loginRequest, LogInUseCaseCallBack loginUseCaseCallBack);
    }
}
