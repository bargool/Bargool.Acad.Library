/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 14.01.2014
 * Time: 12:17
 */
using System;
using System.Collections.Generic;
using System.Linq;

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
		
		public DatabaseWorker(string path)
		{
			this.path = path;
			previousDatabase = HostApplicationServices.WorkingDatabase;
			
		}
		
		public DatabaseWorker(Database workDatabase)
		{
			previousDatabase = HostApplicationServices.WorkingDatabase;
			HostApplicationServices.WorkingDatabase = workDatabase;
		}
		
		private void OpenDatabase()
		{
			
		}
		
		public void Visit(IDatabaseVisitor visitor)
		{
			Document alreadyOpenedDocument =
				Application
				.DocumentManager
				.Cast<Document>()
				.FirstOrDefault(d => d.Name.Equals(this.path, StringComparison.InvariantCulture));
			this.isAlreadyOpened = alreadyOpenedDocument != null;
			
			Database db = null;
			DocumentLock doclock = null;
			
			try
			{
				if (isAlreadyOpened)
				{
					doclock = alreadyOpenedDocument.LockDocument();
					HostApplicationServices.WorkingDatabase = alreadyOpenedDocument.Database;
					db = alreadyOpenedDocument.Database;
				}
				else
				{
					if (!System.IO.File.Exists(this.path))
						throw new System.IO.FileNotFoundException(this.path);
					
					db = new Database(false, true);
					db.ReadDwgFile(this.path, System.IO.FileShare.ReadWrite, true, null);
				}
				
				visitor.Accept(db);
			}
			catch (System.Exception)
			{
				throw;
			}
			finally
			{
				if (isAlreadyOpened)
				{
					if (doclock != null)
						doclock.Dispose();
				}
				else
				{
					if (db != null)
						db.Dispose();
				}
			}
		}
		
		public void Dispose()
		{
			HostApplicationServices.WorkingDatabase = previousDatabase;
		}
	}
}
