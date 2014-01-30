/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 29.01.2014
 * Time: 11:22
 */
using System;
using System.Drawing;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Windows;

namespace Bargool.Acad.Library.Informers
{
	/// <summary>
	/// Description of BaloonNotifier.
	/// </summary>
	public class BaloonNotifier : IInformer
	{
		StringBuilder sb;
		
		EventHandler notifyHandler = null;
		
		string title;
		string toolTipText;
		Icon icon;
		
		public BaloonNotifier()
		{
			sb = new StringBuilder();
		}
		
		public void Notify(string message)
		{
			sb.AppendLine(message);
			if (notifyHandler == null)
			{
				notifyHandler = new EventHandler(Application_Idle);
				Application.Idle += notifyHandler;
			}
		}

		void Application_Idle(object sender, EventArgs e)
		{
			if (sb.Length != 0)
			{
				Document doc = Application.DocumentManager.MdiActiveDocument;
				TrayItem ti = new TrayItem();
				ti.ToolTipText = "Йо!";
				ti.Icon = doc.GetStatusBar().TrayItems[0].Icon;
				Application.StatusBar.TrayItems.Add(ti);
				Application.StatusBar.Update();
				ti.CloseBubbleWindows();
				TrayItemBubbleWindow bw = new TrayItemBubbleWindow();
				bw.Title = "Хей хо!";
				bw.Text = sb.ToString();
				bw.IconType = IconType.Information;
				ti.ShowBubbleWindow(bw);

				bw.Closed += delegate { CloseBw(ti); };
			}
			

			Application.Idle -= notifyHandler;
			sb.Clear();
			notifyHandler = null;
		}
		
		void CloseBw(TrayItem ti)
		{
			try
			{
				Application.StatusBar.TrayItems.Remove(ti);
				Application.StatusBar.Update();
			}
			catch
			{ }
		}

		public void Alert(string message)
		{
			throw new NotImplementedException();
		}
		
		public void Warning(string message)
		{
			throw new NotImplementedException();
		}
		
		public void Log(string message)
		{
			throw new NotImplementedException();
		}
	}
}
