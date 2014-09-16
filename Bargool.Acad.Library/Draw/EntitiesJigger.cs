using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.GraphicsInterface;

namespace Bargool.Acad.Library.Draw
{
    /// <summary>
    /// Класс для вставки набора объектов с учетом UCS
    /// </summary>
    public class EntitiesJigger : DrawJig
    {
        Point3d startPoint;
        IEnumerable<Entity> entities;
        Point3d insertPoint;

        private readonly Matrix3d currentUCS;
        private readonly Tolerance tolerance = new Tolerance(0.0001, 0.0001);

        public EntitiesJigger(Point3d startPoint, IEnumerable<Entity> ents)
        {
            this.currentUCS = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.CurrentUserCoordinateSystem;
            this.startPoint = startPoint.TransformBy(currentUCS);
            this.entities = ents;
            this.insertPoint = Point3d.Origin;
        }

        protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        {
            Matrix3d mat = Matrix3d.Displacement(this.startPoint.GetVectorTo(this.insertPoint));

            mat = currentUCS.PreMultiplyBy(mat);

            WorldGeometry geo = draw.Geometry;
            if (geo != null)
            {
                geo.PushModelTransform(mat);
                foreach (var ent in this.entities)
                    geo.Draw(ent);

                geo.PopModelTransform();
            }

            return true;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions jppo = new JigPromptPointOptions("\nУкажите точку вставки изображения:");
            jppo.UserInputControls = UserInputControls.GovernedByUCSDetect | UserInputControls.Accept3dCoordinates;

            PromptPointResult res = prompts.AcquirePoint(jppo);

            if (res.Status != PromptStatus.OK)
                return SamplerStatus.Cancel;

            if (res.Value.IsEqualTo(this.insertPoint, tolerance))
                return SamplerStatus.NoChange;

            this.insertPoint = res.Value;

            return SamplerStatus.OK;
        }

        private void TransformEntities()
        {
            Matrix3d mat = Matrix3d.Displacement(this.startPoint.GetVectorTo(insertPoint));
            mat = currentUCS.PreMultiplyBy(mat);
            foreach (Entity ent in this.entities)
                ent.TransformBy(mat);
        }

        public static PromptResult Jig(IEnumerable<Entity> ents)
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            EntitiesJigger jigger = new EntitiesJigger(Point3d.Origin, ents);
            PromptResult res = ed.Drag(jigger);
            if (res.Status == PromptStatus.OK)
                jigger.TransformEntities();

            return res;
        }
    }
}
