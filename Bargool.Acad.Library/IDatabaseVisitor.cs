/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 14.01.2014
 * Time: 12:37
 */
using System;
using Autodesk.AutoCAD.DatabaseServices;

namespace Bargool.Acad.Library
{
	/// <summary>
	/// Description of IDatabaseVisitor.
	/// </summary>
	public interface IDatabaseVisitor
	{
		void Accept(Database db);
	}
}
