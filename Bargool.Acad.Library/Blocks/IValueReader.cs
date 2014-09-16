using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.Blocks
{
    public interface IValueReader
    {
        IBlockParameter ReadValue(IBlockParameter template, string name);
        IEnumerable<IBlockParameter> ReadValues(IBlockParameter template);
    }
}
