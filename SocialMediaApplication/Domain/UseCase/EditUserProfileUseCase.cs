using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Presenter.ViewModel;

namespace SocialMediaApplication.Domain.UseCase
{
    //use case
    public class EditUserProfileUseCase : UseCaseBase<EditUserProfileResponseObj>
    {
        private readonly IUserManager _userManager = UserManager.GetInstance;
        public readonly EditUserProfileRequestObj EditUserProfileRequestObj;

        public EditUserProfileUseCase(EditUserProfileRequestObj editUserProfileRequestObj)
        {
            EditUserProfileRequestObj = editUserProfileRequestObj;
        }

        public override void Action()
        {
            _userManager.EditUserBObjAsync(EditUserProfileRequestObj, new EditUserProfileUseCaseCallBack(this));
        }

    }

    //req obj
    public class EditUserProfileRequestObj
    {
        public string UserName { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public Gender Gender { get; }
        public MaritalStatus MaritalStatus { get; }
        public string Education { get; }
        public string Occupation { get; }
        public string Place { get; }
        public ProfilePageViewModel.EditUserProfilePresenterCallBack EditUserProfilePresenterCallBack { get; }


        public EditUserProfileRequestObj(string userName, string firstName, string lastName, Gender gender, MaritalStatus maritalStatus, string education, string occupation, string place, ProfilePageViewModel.EditUserProfilePresenterCallBack editUserProfilePresenterCallBack)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            MaritalStatus = maritalStatus;
            Education = education;
            Occupation = occupation;
            Place = place;
            EditUserProfilePresenterCallBack = editUserProfilePresenterCallBack;
        }
    }

    //response obj
    public class EditUserProfileResponseObj
    {
        public UserBObj User { get; }

        public EditUserProfileResponseObj(UserBObj user)
        {
            User = user;
        }
    }

    public class EditUserProfileUseCaseCallBack : IUseCaseCallBack<EditUserProfileResponseObj>
    {
        private readonly EditUserProfileUseCase _editUserProfileUseCase;

        public EditUserProfileUseCaseCallBack(EditUserProfileUseCase editUserProfileUseCase)
        {
            this._editUserProfileUseCase = editUserProfileUseCase;
        }

        public void OnSuccess(EditUserProfileResponseObj responseObj)
        {
            _editUserProfileUseCase?.EditUserProfileRequestObj.EditUserProfilePresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _editUserProfileUseCase?.EditUserProfileRequestObj.EditUserProfilePresenterCallBack?.OnError(ex);
        }
    }
}
