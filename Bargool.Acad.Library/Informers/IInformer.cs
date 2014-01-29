/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 29.01.2014
 * Time: 10:09
 */
using System;

namespace Bargool.Acad.Library.Informers
{
	/// <summary>
	/// Description of IInformer.
	/// </summary>
	public interface IInformer
	{
		void Notify(string message);
		void Alert(string message);
		void Warning(string message);
		void Log(string message);
	}
}
