﻿using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Domain.UseCase
{
    public class InsertUserSelectionChoiceUseCase : UseCaseBase<InsertUserChoiceSelectionResponse>
    {
        private readonly IUserPollChoiceSelectionManager _userSelectionPollChoiceManager = UserPollChoiceSelectionManager.GetInstance;
        public readonly InsertUserChoiceSelectionRequest InsertUserChoiceSelectionRequest;

        public InsertUserSelectionChoiceUseCase(InsertUserChoiceSelectionRequest insertUserChoiceSelectionRequest, IPresenterCallBack<InsertUserChoiceSelectionResponse> insertUserChoiceSelectionPresenterCallBack) : base(insertUserChoiceSelectionPresenterCallBack)
        {
            InsertUserChoiceSelectionRequest = insertUserChoiceSelectionRequest;
        }

        public override void Action()
        {
            _userSelectionPollChoiceManager.InsertPollChoiceSelectionAsync(InsertUserChoiceSelectionRequest, new InsertUserChoiceSelectionUseCaseCallBack(this));
        }
    }

    public class InsertUserChoiceSelectionResponse
    {
        public InsertUserChoiceSelectionResponse(bool success)
        {
            Success = success;
        }

        //public UserPollChoiceSelection UserPollChoiceSelection;
        public bool Success;
    }

    public class InsertUserChoiceSelectionRequest
    {
        public InsertUserChoiceSelectionRequest(string postId, UserPollChoiceSelection userPollChoiceSelection)
        {
            PostId = postId;
            UserPollChoiceSelection = userPollChoiceSelection;
        }

        public string PostId { get; }
        public UserPollChoiceSelection UserPollChoiceSelection { get; }
    }
    
    public class InsertUserChoiceSelectionUseCaseCallBack : IUseCaseCallBack<InsertUserChoiceSelectionResponse>
    {
        private readonly InsertUserSelectionChoiceUseCase _insertUserChoiceSelectionUseCase;

        public InsertUserChoiceSelectionUseCaseCallBack(InsertUserSelectionChoiceUseCase insertUserChoiceSelectionUseCase)
        {
            _insertUserChoiceSelectionUseCase = insertUserChoiceSelectionUseCase;
        }

        public void OnSuccess(InsertUserChoiceSelectionResponse responseObj)
        {
            _insertUserChoiceSelectionUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _insertUserChoiceSelectionUseCase?.PresenterCallBack?.OnError(ex);
        }
    }
}
