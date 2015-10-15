using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.Exceptions
{
    [Serializable]
    public class CriticalWrongDataException : WrongDataException
    {
        public CriticalWrongDataException(string message)
            : base(message)
        {}
    }
}
