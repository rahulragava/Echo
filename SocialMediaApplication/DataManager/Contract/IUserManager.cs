using System.Threading.Tasks;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;

//using SocialMediaApplication.Presenter.ViewModel.RequestObj;

namespace SocialMediaApplication.DataManager.Contract
{
    public interface IEditUserBObj
    {
        Task EditUserBObjAsync(EditUserProfileRequestObj editUserProfileRequestObj, EditUserProfileUseCaseCallBack editUserProfileUseCaseCallBack);
    }
}
