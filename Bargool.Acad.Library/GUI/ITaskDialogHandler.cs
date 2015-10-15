using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Windows;

namespace Bargool.Acad.Library.GUI
{
    public interface ITaskDialogHandler
    {
        bool ProcessTaskDialog(ActiveTaskDialog atd, TaskDialogCallbackArgs e, object sender);
    }
}
