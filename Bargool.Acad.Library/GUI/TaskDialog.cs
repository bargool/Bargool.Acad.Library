using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bargool.Acad.Library.GUI
{
    public class TaskDialog<T> : Autodesk.Windows.TaskDialog where T : ITaskDialogHandler
    {
        T handler;

        public TaskDialog(T handler)
        {
            this.handler = handler;
            this.Callback = this.handler.ProcessTaskDialog;
        }
    }
}
