using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

using Bargool.Acad.Extensions;

namespace Bargool.Acad.Library.Blocks
{
    public class DynamicBlockWriter : IValueWriter
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

        public DynamicBlockWriter(ObjectId brefId)
        {
            this.Initialize(brefId);
        }

        public DynamicBlockWriter(BlockReference bref)
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
            this.Initialize(bref.DynamicBlockReferencePropertyCollection);
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

        public void WriteValue(IBlockParameter parameter, bool isRequired = true)
        {
            var para = _pc.Cast<DynamicBlockReferenceProperty>().FirstOrDefault(n => n.PropertyName.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
            if (para == null)
                if (isRequired)
                    throw new Bargool.Acad.Library.Exceptions.WrongDataException("Нет тут параметра " + parameter.Name);
                else
                    return;

            double value = double.Parse(parameter.Value);
            if (value < 0)
                throw new Bargool.Acad.Library.Exceptions.WrongDataException("Такая геометрия невозможна");

            if ((double)para.Value != value)
            {
                para.Value = value;
            }
        }

        public void Dispose()
        {
            if (this._pc != null)
                this._pc.Dispose();
        }
    }
}
