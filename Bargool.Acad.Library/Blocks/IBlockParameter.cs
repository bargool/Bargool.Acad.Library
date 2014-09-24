using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.Blocks
{
    public interface IBlockParameter
    {
        string Name { get; }
        string Value { get; }
        IBlockParameter Create(string name, string value);
        void WriteValue(IValueWriter writer);
        void ReadValue(IBlockVariablesReader reader);
    }
}
