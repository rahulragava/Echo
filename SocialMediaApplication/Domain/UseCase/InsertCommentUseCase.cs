﻿using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Services.Contract;

namespace SocialMediaApplication.Domain.UseCase
{

    public class InsertCommentUseCase : UseCaseBase<InsertCommentResponse>
    {
        private readonly IAddCommentManager _addCommentManager = AddCommentManager.GetInstance;
        public readonly InsertCommentRequest InsertCommentRequest;

        public InsertCommentUseCase(InsertCommentRequest insertCommentRequest, IPresenterCallBack<InsertCommentResponse> insertCommentPresenterCallBack) : base(insertCommentPresenterCallBack) 
        {
            InsertCommentRequest = insertCommentRequest;
        }

        public override void Action()
        {
            _addCommentManager.InsertCommentAsync(InsertCommentRequest, new InsertCommentUseCaseCallBack(this));
            //_getUserManager.LoginUserAsync(LoginRequest, new LogInUseCaseCallBack(this));

        }
    }

    public class InsertCommentUseCaseCallBack : IUseCaseCallBack<InsertCommentResponse>
    {
        private readonly InsertCommentUseCase _insertCommentUseCase;

        public InsertCommentUseCaseCallBack(InsertCommentUseCase insertCommentUseCase)
        {
            _insertCommentUseCase = insertCommentUseCase;
        }


        public void OnSuccess(InsertCommentResponse responseObj)
        {
            _insertCommentUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _insertCommentUseCase?.PresenterCallBack?.OnError(ex);
        }
    }

    //request object
    public class InsertCommentRequest
    {
        public InsertCommentRequest(string content, string parentCommentId,string postId,int depth)
        {
            ParentCommentId = parentCommentId;
            Content = content;
            CommentOnPostId = postId;
            Depth = depth;
        }

        public string Content { get; }
        public string ParentCommentId { get; }
        public string CommentOnPostId { get; }
        public int Depth { get; }

    }

    public class InsertCommentResponse
    {
        public int InsertedIndex;
        public CommentBObj CommentBObj;
        public InsertCommentResponse(int insertedIndex, CommentBObj commentBObj)
        {
            InsertedIndex = insertedIndex;
            CommentBObj = commentBObj;
        }
    }
}
