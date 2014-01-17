/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 17.01.2014
 * Time: 10:00
 */
using System;

namespace Bargool.Acad.Library.Configuration
{
	/// <summary>
	/// Description of GlobalConfigAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public sealed class GlobalConfigAttribute : Attribute
	{
		public GlobalConfigAttribute() { }
	}
}
