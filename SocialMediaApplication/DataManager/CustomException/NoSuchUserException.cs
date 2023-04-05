using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.DataManager.CustomException
{
    public class NoSuchUserException : Exception
    {
        public NoSuchUserException()
        { }

        public NoSuchUserException(string message, Exception inner)
            : base(message, inner) { }

        public NoSuchUserException(string message) : base(message)
        {
        }
    }
}
