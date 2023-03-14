using System;

namespace SocialMediaApplication.Presenter.ViewModel
{
    public interface IPresenterCallBack<T>
    {
        void OnSuccess(T response);
        void OnError(Exception ex);
    }
}
