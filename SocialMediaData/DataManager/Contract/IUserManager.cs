using System.Threading.Tasks;
using SocialMediaApplication.Domain.UseCase;
//using SocialMediaApplication.Presenter.ViewModel.RequestObj;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface IUserLoginManager
    {
        Task LoginUserAsync(LoginRequest loginRequest, LogInUseCaseCallBack loginUseCaseCallBack);
    }

    public interface IUserSignUpManager
    {
        Task SignUpUserAsync(SignUpRequestObj signUpRequestObj, SignUpUseCaseCallBack signUpUseCaseCallBack);
    }

    public interface IUserManager : IUserLoginManager, IUserSignUpManager
    {

    }
}
