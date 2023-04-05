using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Presenter.ViewModel;

namespace SocialMediaApplication.Domain.UseCase
{
    //use case
    public class EditUserProfileUseCase : UseCaseBase<EditUserProfileResponseObj>
    {
        private readonly IEditUserBObj _editUserManager = EditUserManager.GetInstance;
        public readonly EditUserProfileRequestObj EditUserProfileRequestObj;

        public EditUserProfileUseCase(EditUserProfileRequestObj editUserProfileRequestObj, IPresenterCallBack<EditUserProfileResponseObj> editUserProfilePresenterCallBack) : base(editUserProfilePresenterCallBack)
        {
            EditUserProfileRequestObj = editUserProfileRequestObj;
        }

        public override void Action()
        {
            _editUserManager.EditUserBObjAsync(EditUserProfileRequestObj, new EditUserProfileUseCaseCallBack(this));
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
        public string UserId { get; }

        public EditUserProfileRequestObj(string userId, string userName, string firstName, string lastName, Gender gender, MaritalStatus maritalStatus, string education, string occupation, string place)
        {
            UserId = userId;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            MaritalStatus = maritalStatus;
            Education = education;
            Occupation = occupation;
            Place = place;
        }
    }

    //response obj
    public class EditUserProfileResponseObj
    {
        public User User { get; }

        public EditUserProfileResponseObj(User user)
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
            _editUserProfileUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _editUserProfileUseCase?.PresenterCallBack?.OnError(ex);
        }
    }
}
