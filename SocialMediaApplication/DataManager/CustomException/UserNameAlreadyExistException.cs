using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.DataManager.CustomException
{
    public class UserNameAlreadyExistException : Exception
    {
        public UserNameAlreadyExistException()
        { }

        public UserNameAlreadyExistException(string message, Exception inner)
            : base(message, inner) { }

        public UserNameAlreadyExistException(string message) :base(message)
        {
        }
    }
}
