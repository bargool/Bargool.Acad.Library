/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 14.01.2014
 * Time: 12:17
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
		
		Database currentDb;
		DocumentLock docLock;
		
		public DatabaseWorker(string path)
		{
			this.path = path;
			previousDatabase = HostApplicationServices.WorkingDatabase;
			OpenDatabase();
		}
		
//		public DatabaseWorker(Database workDatabase)
//		{
//			previousDatabase = HostApplicationServices.WorkingDatabase;
//			HostApplicationServices.WorkingDatabase = workDatabase;
//		}
		
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
				
				currentDb = new Database(false, true);
				currentDb.ReadDwgFile(this.path, System.IO.FileShare.ReadWrite, true, null);
				currentDb.CloseInput(true);
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
