
using System;
using System.Threading;
using System.Threading.Tasks;
using SocialMediaApplication.Presenter.ViewModel;

namespace SocialMediaApplication.Domain.UseCase
{
    public abstract class UseCaseBase<T>
    {
        //public IPresenterCallBack<T> PresenterCallBack { get; }
        protected UseCaseBase() { }
        public abstract void Action();

        public void Execute()
        {
            if (GetIfAvailableCache())
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    Action();
                }
                catch (Exception ex)
                {
                    //should have my own error obj
                    //ZError errObj = new ZError(ex,ex.Message);
                    //PresenterCallBack?.OnError(errObj);
                }
            });
        }

        public virtual bool GetIfAvailableCache()
        {
            return false;
        }
    }

    public interface IUseCaseCallBack<T>
    {
        void OnSuccess(T responseObj);
        void OnError(Exception ex);
    }
}
