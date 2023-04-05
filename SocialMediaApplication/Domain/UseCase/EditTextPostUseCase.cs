using SocialMediaApplication.DataManager;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;

namespace SocialMediaApplication.Domain.UseCase
{
    public class EditTextPostUseCase : UseCaseBase<EditTextPostResponse>
    {
        private readonly IEditTextPostManager _editTextPostManager = EditTextPostManager.GetInstance;
        public EditTextPostRequest EditTextPostRequest;

        public EditTextPostUseCase(EditTextPostRequest editTextPostRequest, IPresenterCallBack<EditTextPostResponse> editTextPostPresenterCallBack) : base(editTextPostPresenterCallBack)
        {
            EditTextPostRequest = editTextPostRequest;  
        }

        public override void Action()
        {
            _editTextPostManager.EditTextPostAsync(EditTextPostRequest,
                new EditTextPostUseCaseCallBack(this));
        }
    }

    //req obj
    public class EditTextPostRequest
    {
        public TextPostBObj TextPost;

        public EditTextPostRequest(TextPostBObj textPost)
        {
            TextPost = textPost;
        }
    }

    //response obj
    public class EditTextPostResponse
    {
        public bool Success { get; }
        public EditTextPostResponse(bool success)
        {
            Success = success;
        }
    }

    public class EditTextPostUseCaseCallBack : IUseCaseCallBack<EditTextPostResponse>
    {
        private readonly EditTextPostUseCase _editTextPostUseCase;

        public EditTextPostUseCaseCallBack(EditTextPostUseCase editTextPostUseCase)
        {
            _editTextPostUseCase = editTextPostUseCase;
        }

        public void OnSuccess(EditTextPostResponse responseObj)
        {
            _editTextPostUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _editTextPostUseCase?.PresenterCallBack?.OnError(ex);
        }
    }
}
