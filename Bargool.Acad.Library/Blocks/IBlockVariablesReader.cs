using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.Blocks
{
    public interface IBlockVariablesReader
    {
        string BlockName { get; }

        IBlockParameter ReadAttribute(IBlockParameter template, string name);
        IEnumerable<IBlockParameter> ReadAttributes(IBlockParameter template);
        
        IBlockParameter ReadDynamicParameter(IBlockParameter template, string name);
        IEnumerable<IBlockParameter> ReadDynamicParameters(IBlockParameter template);
    }
}
