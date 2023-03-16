﻿using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Domain.UseCase
{
    public class EditProfileImageUseCase : UseCaseBase<EditProfileImageResponse>
    {
        private readonly IEditProfileImageManager _editProfileImageManager = EditProfileImageManager.GetInstance;
        public EditProfileImageRequest EditProfileImageRequest;

        public EditProfileImageUseCase(EditProfileImageRequest editProfileImageRequest)
        {
            EditProfileImageRequest = editProfileImageRequest;
        }

        public override void Action()
        {
                _editProfileImageManager.EditProfileImage(EditProfileImageRequest,
                new EditProfileImageUseCaseCallBack(this));
        }
    }

   
    //req obj
    public class EditProfileImageRequest
    {
        public EditProfileImagePresenterCallBack EditProfileImagePresenterCallBack { get; }
        public string UserId { get; }
        public string ImagePath { get; }

        public EditProfileImageRequest(string userId, string imagePath ,EditProfileImagePresenterCallBack editProfileImagePresenterCallBack)
        {
            UserId = userId;
            ImagePath = imagePath;
            EditProfileImagePresenterCallBack = editProfileImagePresenterCallBack;
        }
    }

    //response obj
    public class EditProfileImageResponse
    {
        public bool Success { get; }
        public EditProfileImageResponse(bool success)
        {
            Success = success;
        }
    }

    public class EditProfileImageUseCaseCallBack : IUseCaseCallBack<EditProfileImageResponse>
    {
        private readonly EditProfileImageUseCase _editProfileImageUseCase;

        public EditProfileImageUseCaseCallBack(EditProfileImageUseCase editProfileImageUseCase)
        {
            _editProfileImageUseCase = editProfileImageUseCase;
        }

        public void OnSuccess(EditProfileImageResponse responseObj)
        {
            _editProfileImageUseCase?.EditProfileImageRequest.EditProfileImagePresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _editProfileImageUseCase?.EditProfileImageRequest.EditProfileImagePresenterCallBack?.OnError(ex);
        }
    }
}
