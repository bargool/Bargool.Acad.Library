using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

using Bargool.Acad.Extensions;

namespace Bargool.Acad.Library.Blocks
{
    public class DynamicParameterReader : IValueReader
    {
        private readonly RXClass brefClass = RXClass.GetClass(typeof(BlockReference));

        DynamicBlockReferencePropertyCollection _pc;

        public DynamicParameterReader(DynamicBlockReferencePropertyCollection pc)
        {
            this.Initialize(pc);
        }

        public DynamicParameterReader(ObjectId brefId)
        {
            this.Initialize(brefId);
        }

        public DynamicParameterReader(BlockReference bref)
        {
            this.Initialize(bref);
        }


        private void Initialize(ObjectId brefId)
        {
            if (brefId.IsNull || brefId.IsErased || !brefId.IsValid || brefId.IsEffectivelyErased)
                throw new ArgumentException("brefId is broken");

            if (!brefId.ObjectClass.IsDerivedFrom(brefClass))
                throw new ArgumentException("brefId not BlockReference");

            BlockReference bref = brefId.GetObject<BlockReference>();
            this.Initialize(bref);
        }

        private void Initialize(BlockReference bref)
        {
            if (!bref.IsDynamicBlock)
                throw new InvalidOperationException("Not dynamic block");

            this.Initialize(bref.DynamicBlockReferencePropertyCollection);
        }

        private void Initialize(DynamicBlockReferencePropertyCollection pc)
        {
            if (pc == null)
                throw new ArgumentNullException("pc");

            this._pc = pc;
        }

        public IBlockParameter ReadParameter(IBlockParameter template, string name)
        {
            DynamicBlockReferenceProperty prop = _pc.Cast<DynamicBlockReferenceProperty>()
                .FirstOrDefault(p => p.PropertyName.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (prop == null)
                return null;
            IBlockParameter result = template.Create(prop.PropertyName, prop.Value.ToString());

            return result;
        }

        public IEnumerable<IBlockParameter> ReadValues(IBlockParameter template)
        {
            foreach (var par in _pc.Cast<DynamicBlockReferenceProperty>().Where(p => p.VisibleInCurrentVisibilityState && p.Show))
            {
                yield return template.Create(par.PropertyName, par.Value.ToString());
            }
        }
    }
}
