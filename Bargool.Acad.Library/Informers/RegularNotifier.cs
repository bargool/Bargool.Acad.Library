using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

namespace Bargool.Acad.Library.Informers
{
    public class RegularNotifier : IInformer
    {
        public void Notify(string message)
        {
            this.Warning(message);
        }

        public void Alert(string message)
        {
            Application.ShowAlertDialog(message);
        }

        public void Warning(string message)
        {
            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\n{0}", message);
        }

        public void Log(string message)
        {
            this.Warning(message);
        }
    }
}
