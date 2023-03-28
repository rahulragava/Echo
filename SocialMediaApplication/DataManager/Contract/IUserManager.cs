using System.Threading.Tasks;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;

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

    public interface IUserLogOut
    {
        //Task RemoveUserAsync();
    }

    public interface IGetUserBObj
    {
        Task GetUserBObjAsync(GetUserProfileRequestObj getUserProfileRequestObj, GetUserProfileUseCaseCallBack getUserProfileUseCaseCallBack);
    }

    public interface IEditUserBObj
    {
        Task EditUserBObjAsync(EditUserProfileRequestObj editUserProfileRequestObj, EditUserProfileUseCaseCallBack editUserProfileUseCaseCallBack);
    }

    public interface IGetUserNames
    {
        Task GetUserNamesAsync(GetUserNamesRequestObj getUserNameRequestObj, GetUserNamesUseCaseCallBack getUserNamesUseCaseCallBack);
    }

    public interface IUserManager : IUserLoginManager, IUserSignUpManager, IUserLogOut, IGetUserBObj, IEditUserBObj
    {

    }
}
