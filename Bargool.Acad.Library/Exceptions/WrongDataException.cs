using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.Exceptions
{
    public class WrongDataException : Exception
    {
        public WrongDataException(string message)
            :base(message)
        { }
    }
}
