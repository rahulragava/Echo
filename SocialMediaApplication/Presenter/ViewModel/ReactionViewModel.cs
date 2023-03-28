using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.PlatformUI;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.Constant;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public class ReactionViewModel : ObservableObject
    {
        public Reaction Reaction;
        public void ReactionToPost(ReactionType reactionType, string postId)
        {
            var reaction = new Reaction()
            {
                ReactionOnId = postId,
                ReactionType = reactionType,
                ReactedBy = AppSettings.UserId
            };
            Reaction = reaction;
            var reactionToPostRequest = new ReactionToPostRequestObj(reaction, new ReactionToPostPresenterCallBack(this));
            var reactionToPostUseCase = new ReactionToPostUseCase(reactionToPostRequest);
            reactionToPostUseCase.Execute();
        }

       

        public class ReactionToPostPresenterCallBack : IPresenterCallBack<ReactionToPostResponse>
        {
            private readonly ReactionViewModel _reactionViewModel;

            public ReactionToPostPresenterCallBack(ReactionViewModel reactionViewModel)
            {
                _reactionViewModel = reactionViewModel;
            }

            public void OnSuccess(ReactionToPostResponse reactionToPostResponse)
            {
                // success
            }

            public void OnError(Exception ex)
            {
                //error
            }
        }
    }
}
