using System;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public interface IPresenterCallBack<in T>
    {
        void OnSuccess(T logInResponse);
        void OnError(Exception ex);
    }
}
