using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace Bargool.Acad.Library
{
    // Idea is from CP4471 AU 2012 "Programming AutoCAD with C#: Best Practices"
    /// <summary>
    /// Provides easy access to several "active" objects in the AutoCAD
    /// runtime environment
    /// </summary>
    public static class Active
    {
        public static Document Document
        {
            get { return Application.DocumentManager.MdiActiveDocument; }
        }

        public static Editor Editor
        {
            get
            {
                if (Document != null)
                    return Document.Editor;
                else
                    return null;
            }
        }

        public static Database Database
        {
            get
            {
                if (Document != null)
                    return Document.Database;
                else
                    return null;
            }
        }
    }
}
