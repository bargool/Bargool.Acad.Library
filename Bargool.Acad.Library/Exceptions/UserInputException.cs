using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.Exceptions
{
    public class UserInputException : Exception
    {
        public UserInputException(string message)
            : base(message)
        {}
    }
}
