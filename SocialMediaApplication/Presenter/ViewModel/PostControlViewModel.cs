using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class PostControlViewModel : ObservableObject
    {
        private string _userName;

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private PollChoiceBObj _pollChoiceBObj;

        public PollChoiceBObj PollChoiceBObj
        {
            get => _pollChoiceBObj;
            set => SetProperty(ref _pollChoiceBObj, value);
        }

        private double _totalVotes;

        public double TotalVotes
        {
            get => _totalVotes;
            set => SetProperty(ref _totalVotes, value);
        }

        //public string GetTotalVotes(PollPostBObj pollPostBObj)
        //{
        //    var totalVotes = pollPostBObj.Choices.Aggregate(0.0, (current, choice) => current + choice.ChoiceSelectedUsers.Count);

        //    return totalVotes.ToString();
        //}

        public void GetUserName(string userId)
        {
            var cts = new CancellationTokenSource();

            var getUserNameRequestObj = new GetUserNameRequestObj(userId, new GetUserNamePresenterCallBack(this), cts.Token);
            var getUserNameUseCase = new GetUserNameUseCase(getUserNameRequestObj);
            getUserNameUseCase.Execute();
        }

        public void GetUserNameSucceed(GetUserNameResponseObj getUserNameResponseObj)
        {
            UserName = getUserNameResponseObj.UserName;
        }
    }

    public class GetUserNamePresenterCallBack : IPresenterCallBack<GetUserNameResponseObj>
    {
        private readonly PostControlViewModel _postControlViewModel;

        public GetUserNamePresenterCallBack(PostControlViewModel postControlViewModel)
        {
            _postControlViewModel = postControlViewModel;
        }

        public void OnSuccess(GetUserNameResponseObj getUserNameResponse)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _postControlViewModel.GetUserNameSucceed(getUserNameResponse);
                }
            );
        }

        public void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
