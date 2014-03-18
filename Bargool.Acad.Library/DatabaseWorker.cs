/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 14.01.2014
 * Time: 12:17
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace Bargool.Acad.Library
{
	/// <summary>
	/// Description of DatabaseWorker.
	/// </summary>
	public class DatabaseWorker : IDisposable
	{
		string path;
		Database previousDatabase;
		bool isAlreadyOpened;
		bool doSave;
		
		Database currentDb = null;
		DocumentLock docLock = null;
		
		private DatabaseWorker()
		{
			previousDatabase = HostApplicationServices.WorkingDatabase;
		}
		
		public DatabaseWorker(string path)
			: this()
		{
			this.path = path;
			OpenDatabase();
		}
		
		public DatabaseWorker(Database workDatabase)
			: this()
		{
			this.path = workDatabase.OriginalFileName;
			OpenDatabase();
		}
		
		private void OpenDatabase()
		{
			Document alreadyOpenedDocument =
				Application
				.DocumentManager
				.Cast<Document>()
				.FirstOrDefault(d => d.Name.Equals(this.path, StringComparison.InvariantCulture));
			this.isAlreadyOpened = alreadyOpenedDocument != null;
			currentDb = null;
			docLock = null;
			if (isAlreadyOpened)
			{
				docLock = alreadyOpenedDocument.LockDocument();
				HostApplicationServices.WorkingDatabase = alreadyOpenedDocument.Database;
				currentDb = alreadyOpenedDocument.Database;
			}
			else
			{
				if (!System.IO.File.Exists(this.path))
					throw new System.IO.FileNotFoundException(this.path);
				
				try
				{
					currentDb = new Database(false, true);
					currentDb.ReadDwgFile(this.path, System.IO.FileShare.ReadWrite, true, null);
					currentDb.CloseInput(true);
				}
				catch (System.Exception ex)
				{
					string message = "При открытии файла " + this.path + " возникла ошибка: ";
					ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ex, message + ex.Message);
					throw ex;
				}
			}
		}
		
		private void CloseDatabase()
		{
			if (isAlreadyOpened)
			{
				if (docLock != null)
					docLock.Dispose();
			}
			else
			{
				if (currentDb != null)
				{
					string tempPath = Path.GetTempFileName();
					if (doSave)
						currentDb.SaveAs(tempPath, DwgVersion.Current);
					currentDb.Dispose();
					
					if (doSave)
					{
						File.Delete(this.path);
						File.Move(tempPath, this.path);
					}
				}
			}
		}
		
		public void Visit(IDatabaseVisitor visitor)
		{
			Visit(visitor, false);
		}
		
		public void Visit(IDatabaseVisitor visitor, bool doSave)
		{
			this.doSave = doSave;
			try
			{
				visitor.Accept(currentDb);
			}
			catch (System.Exception)
			{
				throw;
			}
		}
		
		public void Dispose()
		{
			CloseDatabase();
			HostApplicationServices.WorkingDatabase = previousDatabase;
		}
	}
}
