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
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Services.Contract;

namespace SocialMediaApplication.Domain.UseCase
{
    public class TextPostCreationUseCase : UseCaseBase<TextPostCreationResponse>
    {
        private readonly ICreatePostManager _createPostManager = CreatePostManager.GetInstance;
        public readonly TextPostCreationRequest TextPostCreationRequest;

        public TextPostCreationUseCase(TextPostCreationRequest textPostCreationRequest, IPresenterCallBack<TextPostCreationResponse> textPostCreationPresenterCallBack):base(textPostCreationPresenterCallBack)
        {
            TextPostCreationRequest = textPostCreationRequest;
        }

        public override void Action()
        {
            _createPostManager.CreateTextPostAsync(TextPostCreationRequest, new TextPostCreationUseCaseCallBack(this));
        }
    }

    //req obj
    public class TextPostCreationRequest
    {
        public TextPost TextPost;

        public TextPostCreationRequest(TextPost textPost)
        {
            TextPost = textPost;
        }
    }



    public class TextPostCreationUseCaseCallBack : IUseCaseCallBack<TextPostCreationResponse>
    {
        private readonly TextPostCreationUseCase _textPostCreationUseCase;

        public TextPostCreationUseCaseCallBack(TextPostCreationUseCase textPostCreationUseCase)
        {
            _textPostCreationUseCase = textPostCreationUseCase;
        }

        public void OnSuccess(TextPostCreationResponse responseObj)
        {
            _textPostCreationUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _textPostCreationUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    public class TextPostCreationResponse
    {
        public bool Success;

        public TextPostCreationResponse(bool success)
        {
            Success = success;
        }
    }
}
