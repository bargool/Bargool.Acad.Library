/*
 * User: aleksey.nakoryakov
 * Date: 03.08.12
 * Time: 11:11
 */
using System;
using Autodesk.AutoCAD.ApplicationServices;

namespace Bargool.Acad.Library
{
	/// <summary>
	/// Class is using to set AutoCAD system variable and return it to previous value.
	/// First it sets given Acad system variable to given value and while disposing it sets
	/// this variable to previous value
	/// </summary>
	/// <example>
	/// Example how-to
	/// <code>
	/// using (AcadSystemVariableSwitcher varSw = new AcadSystemVariableSwitcher(variableName, variableValue))
	/// {
	/// 	// Some code here
	/// }
	/// </code>
	/// </example>
	public sealed class AcadSystemVariableSwitcher:IDisposable
	{
		string variableName = string.Empty;
		object oldVariableValue;
		
		/// <summary>
		/// Constructor sets given Acad system variable to given value
		/// </summary>
		/// <param name="variableName">System variable name to change</param>
		/// <param name="variableValue">New value for given system variable</param>
		public AcadSystemVariableSwitcher(string variableName, object variableValue)
		{
			this.variableName = variableName;
			this.oldVariableValue = Application.GetSystemVariable(variableName);
			Application.SetSystemVariable(variableName, variableValue);
		}
		
		public void Dispose()
		{
			Application.SetSystemVariable(this.variableName, this.oldVariableValue);
		}
	}
}
