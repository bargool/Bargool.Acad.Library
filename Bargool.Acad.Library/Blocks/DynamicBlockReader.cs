using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

using Bargool.Acad.Extensions;

namespace Bargool.Acad.Library.Blocks
{
    public class DynamicBlockReader : IBlockVariablesReader
    {
        private readonly RXClass brefClass = RXClass.GetClass(typeof(BlockReference));

        DynamicBlockReferencePropertyCollection _pc;
        AttributeCollection _ac;
        BlockReference bref;

        public string BlockName
        {
            get { return bref.GetEffectiveName(); }
        }

        //public DynamicBlockReader(DynamicBlockReferencePropertyCollection pc)
        //{
        //    this.Initialize(pc);
        //}

        public DynamicBlockReader(ObjectId brefId)
        {
            this.Initialize(brefId);
        }

        public DynamicBlockReader(BlockReference bref)
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

            this.bref = bref;
        }

        private void Initialize(DynamicBlockReferencePropertyCollection pc)
        {
            if (pc == null)
                throw new ArgumentNullException("pc");

            this._pc = pc;
        }

        private void Initialize(AttributeCollection ac)
        {
            if (ac == null)
                throw new ArgumentNullException("ac");

            this._ac = ac;
        }

        public IBlockParameter ReadDynamicParameter(IBlockParameter template, string name)
        {
            if (_pc == null)
                Initialize(bref.DynamicBlockReferencePropertyCollection);

            DynamicBlockReferenceProperty prop = _pc.Cast<DynamicBlockReferenceProperty>()
                .FirstOrDefault(p => p.PropertyName.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (prop == null)
                return null;
            IBlockParameter result = template.Create(prop.PropertyName, prop.Value.ToString());

            return result;
        }

        public IEnumerable<IBlockParameter> ReadDynamicParameters(IBlockParameter template)
        {
            if (_pc == null)
                Initialize(bref.DynamicBlockReferencePropertyCollection);

            return
                _pc.Cast<DynamicBlockReferenceProperty>()
                .Select(prop => template.Create(prop.PropertyName, prop.Value.ToString()));
        }

        public IBlockParameter ReadAttribute(IBlockParameter template, string name)
        {
            if (this._ac == null)
                Initialize(bref.AttributeCollection);
            var att = this._ac.Cast<ObjectId>()
                    .Select(id => id.GetObject<AttributeReference>())
                    .FirstOrDefault(a => a.Tag.Equals(name, StringComparison.OrdinalIgnoreCase));
            return att != null ? template.Create(att.Tag, att.TextString) : null;
        }

        public IEnumerable<IBlockParameter> ReadAttributes(IBlockParameter template)
        {
            if (this._ac == null)
                Initialize(bref.AttributeCollection);
            return
                this._ac.Cast<ObjectId>()
                .Select(id => id.GetObject<AttributeReference>())
                .Select(att => template.Create(att.Tag, att.TextString));
        }
    }
}
