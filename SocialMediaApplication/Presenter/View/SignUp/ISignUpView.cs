using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Presenter.View.SignUp
{
    public interface ISignUpView
    {
        void GoToLogInPage();
        void ErrorMessageNotification(string message);
    }
}
