using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Presenter.View.PostView.TextPostView
{
    public interface ITextPostUserControl
    {
        void GetUserMiniDetailSuccess();
        void FollowUnFollowActionDone();
        void RemovedPost(string postId);
        void SetReactionVisibility();
    }
}
