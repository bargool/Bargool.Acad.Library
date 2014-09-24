/*
 * User: aleksey.nakoryakov
 * Date: 23.10.12
 * Time: 10:32
 */
using System;
using System.Diagnostics;
using Autodesk.AutoCAD.ApplicationServices;

namespace Bargool.Acad.Library.Diagnostics
{
    /// <summary>
    /// Timer class
    /// <see cref="@Reference@">http://www.theswamp.org/index.php?topic=43040.0</see>
    /// </summary>
    /// <example>
    /// <code>using (TestTimer timer = new TestTimer("Creating 1000 random lines", n => log.AppendLine(n)))
    /// {
    /// ...
    /// }
    /// </code>
    /// </example>
    public class TestTimer : IDisposable
    {
        private readonly string _name;
        private readonly Stopwatch _watch;
        private readonly Action<string> _action;
        
        public TestTimer(string name)
        {
            _name = name;
            _watch = Stopwatch.StartNew();
        }
        
        public TestTimer(string name, Action<string> action)
            : this(name)
        {
            _action = action;
        }
        
        public void Dispose()
        {
            _watch.Stop();
            string message = string.Format("На {0} ушло {1}", _name, _watch.Elapsed);
            if (_action == null)
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\n"+message + "\n");
            else
                _action(message);
        }
    }
}
