/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 13.03.2014
 * Time: 9:42
 */
using System;

using Autodesk.AutoCAD.DatabaseServices;

namespace Bargool.Acad.Library
{
    /// <summary>
    /// Description of LayerUnlocker.
    /// </summary>
    public class LayerUnlocker : IDisposable
    {
        bool isLayerLocked = false;
        bool isLayerFrozen = false;

        ObjectId layerId = ObjectId.Null;
        string layerName = null;

        public LayerUnlocker(ObjectId layerId)
        {
            this.layerId = layerId;

            UnlockLayer();
        }

        public LayerUnlocker(string layerName)
        {
            this.layerName = layerName;

            UnlockLayer();
        }

        private void UnlockLayer()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (var tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                if (layerId == ObjectId.Null)
                {
                    if (string.IsNullOrWhiteSpace(this.layerName))
                        throw new ArgumentNullException("layerName");

                    LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                    if (!lt.Has(this.layerName))
                        throw new ArgumentException("Нет слоя с указанным именем: " + this.layerName);

                    this.layerId = lt[this.layerName];
                }

                LayerTableRecord ltr = tr.GetObject(this.layerId, OpenMode.ForRead, true) as LayerTableRecord;
                if (ltr == null)
                    throw new ArgumentException("Указанный ObjectId не соответствует слою");

                this.isLayerLocked = ltr.IsLocked;
                this.isLayerFrozen = ltr.IsFrozen;

                if (this.isLayerFrozen || this.isLayerLocked)
                {
                    ltr.UpgradeOpen();
                    ltr.IsLocked = false;
                    if (db.Clayer != ltr.ObjectId)
                        ltr.IsFrozen = false;
                }
                tr.Commit();
            }
        }

        public void Dispose()
        {
            if (this.isLayerFrozen || this.isLayerLocked)
            {
                Database db = HostApplicationServices.WorkingDatabase;
                using (var tr = db.TransactionManager.StartOpenCloseTransaction())
                {
                    LayerTableRecord ltr = (LayerTableRecord)tr.GetObject(this.layerId, OpenMode.ForWrite);
                    ltr.IsLocked = this.isLayerLocked;
                    if (db.Clayer != ltr.ObjectId)
                        ltr.IsFrozen = this.isLayerFrozen;
                    tr.Commit();
                }
            }
        }
    }
}
