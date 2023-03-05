using System;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public interface IPresenterCallBack<T>
    {
        void OnSuccess(T logInResponse);
        void OnError(Exception ex);
    }
}
