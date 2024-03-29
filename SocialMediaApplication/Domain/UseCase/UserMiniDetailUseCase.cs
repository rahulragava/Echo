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
using SocialMediaApplication.Presenter.View.CommentView;
using static SocialMediaApplication.Presenter.ViewModel.PostControlViewModel;

namespace SocialMediaApplication.Domain.UseCase
{
    public class UserMiniDetailUseCase : UseCaseBase<UserMiniDetailResponse>
    {
        private readonly IUserMiniDetailManager _userManager = UserMiniDetailManager.GetInstance;
        public readonly UserMiniDetailRequest UserMiniDetailRequest;

        public UserMiniDetailUseCase(UserMiniDetailRequest userMiniDetailRequest, IPresenterCallBack<UserMiniDetailResponse> userMiniDetailPresenterCallBack) :base(userMiniDetailPresenterCallBack)
        {
            UserMiniDetailRequest = userMiniDetailRequest;
        }

        public override void Action()
        {
            _userManager.GetUserMiniDetailsAsync(UserMiniDetailRequest, new UserMiniDetailUseCaseCallBack(this));
        }

    }

    //req obj
    public class UserMiniDetailRequest
    {
        public string UserId { get; }

        public UserMiniDetailRequest(string userId)
        {
            UserId = userId;
        }
    }

    //response obj
    public class UserMiniDetailResponse
    {
        public User User { get; }
        public List<string> FollowerUserIds{ get; set; }
        public List<string> FollowingUserIds{ get; set; }

        public UserMiniDetailResponse(User user, List<string> followerUserIds, List<string> followingUserIds)
        {
            User = user;
            FollowerUserIds = followerUserIds;
            FollowingUserIds = followingUserIds;
        }
    }

    public class UserMiniDetailUseCaseCallBack : IUseCaseCallBack<UserMiniDetailResponse>
    {
        private readonly UserMiniDetailUseCase _userMiniDetailUseCase;

        public UserMiniDetailUseCaseCallBack(UserMiniDetailUseCase userMiniDetailUseCase)
        {
            _userMiniDetailUseCase = userMiniDetailUseCase;
        }

        public void OnSuccess(UserMiniDetailResponse responseObj)
        {
            _userMiniDetailUseCase?.PresenterCallBack?.OnSuccess(responseObj);
        }


        public void OnError(Exception ex)
        {
            _userMiniDetailUseCase?.PresenterCallBack?.OnError(ex);
        }
    }
}
