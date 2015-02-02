using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.Blocks
{
    public interface IValueWriter : IDisposable
    {
        void WriteValue(IBlockParameter parameter);
    }
}
