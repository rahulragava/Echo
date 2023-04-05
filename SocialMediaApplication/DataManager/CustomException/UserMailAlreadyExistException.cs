using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.DataManager.CustomException
{
    public class UserMailAlreadyExistException : Exception
    {
        public UserMailAlreadyExistException()
        { }

        public UserMailAlreadyExistException(string message, Exception inner)
            : base(message, inner) { }

        public UserMailAlreadyExistException(string message) : base(message)
        {

        }
    }
}
