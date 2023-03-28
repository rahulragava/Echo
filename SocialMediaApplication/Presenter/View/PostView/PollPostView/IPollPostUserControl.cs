using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Presenter.View.PostView.PollPostView
{
    public interface IPollPostUserControl
    {
        void GetUserMiniDetailSuccess();
        void FollowUnFollowActionDone();
        void RemovedPost(string postId);
        void SetReactionVisibility();
    }
}
