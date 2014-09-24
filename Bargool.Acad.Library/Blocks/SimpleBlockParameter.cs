using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.Blocks
{
    public class SimpleBlockParameter : IBlockParameter
    {
        string name;
        string value;

        public string Name
        {
            get { return name; }
        }

        public string Value
        {
            get { return value; }
        }

        public IBlockParameter Create(string name, string value)
        {
            return new SimpleBlockParameter() { name = name, value = value };
        }

        public void WriteValue(IValueWriter writer)
        {
            throw new NotImplementedException();
        }

        public void ReadValue(IBlockVariablesReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
