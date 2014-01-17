/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 17.01.2014
 * Time: 9:51
 */
using System;

namespace Bargool.Acad.Library.Configuration
{
	/// <summary>
	/// Description of LocalConfigAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public sealed class LocalConfigAttribute : Attribute
	{
		public LocalConfigAttribute() { }
	}
}
