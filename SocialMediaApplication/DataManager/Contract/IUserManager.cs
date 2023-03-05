using System.Threading.Tasks;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;

//using SocialMediaApplication.Presenter.ViewModel.RequestObj;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface IUserLoginManager
    {
        void LoginUserAsync(LoginRequest loginRequest, LogInUseCaseCallBack loginUseCaseCallBack);
    }

    public interface IUserSignUpManager
    {
        void SignUpUserAsync(SignUpRequestObj signUpRequestObj, SignUpUseCaseCallBack signUpUseCaseCallBack);
    }

    public interface IUserLogOut
    {
        //Task RemoveUserAsync();
    }

    public interface IGetUserBObj
    {
        void GetUserBObjAsync(GetUserProfileRequestObj getUserProfileRequestObj, GetUserProfileUseCaseCallBack getUserProfileUseCaseCallBack);
    }

    public interface IEditUserBObj
    {
        void EditUserBObjAsync(EditUserProfileRequestObj editUserProfileRequestObj, EditUserProfileUseCaseCallBack editUserProfileUseCaseCallBack);
    }

    public interface IGetUserNames
    {
        void GetUserNamesAsync(GetUserNamesRequestObj getUserNameRequestObj, GetUserNamesUseCaseCallBack getUserNamesUseCaseCallBack);
    }

    public interface IUserManager : IUserLoginManager, IUserSignUpManager, IUserLogOut, IGetUserBObj, IEditUserBObj
    {

    }
}
