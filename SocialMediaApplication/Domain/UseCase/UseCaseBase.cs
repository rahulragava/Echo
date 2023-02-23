
using System;
using System.Threading;
using System.Threading.Tasks;
using SocialMediaApplication.Presenter.ViewModel;

namespace SocialMediaApplication.Domain.UseCase
{
    public abstract class UseCaseBase<T>
    {
        public CancellationToken CancellationToken { get; }
        //public IPresenterCallBack<T> PresenterCallBack { get; }

        protected UseCaseBase(CancellationToken cancellationToken)
        {
            //PresenterCallBack = presenterCallBack;
            CancellationToken = cancellationToken;
        }

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
            }, CancellationToken);
        }

        public virtual bool GetIfAvailableCache()
        {
            return false;
        }
    }

    public interface IUseCaseCallBack<in T>
    {
        void OnSuccess(T responseObj);
        void OnError(Exception ex);
    }
}
