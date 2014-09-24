using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.Blocks
{
    class AttributeReferenceReader : IValueReader
    {
        public IBlockParameter ReadParameter(IBlockParameter template, string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBlockParameter> ReadValues(IBlockParameter template)
        {
            throw new NotImplementedException();
        }
    }
}
