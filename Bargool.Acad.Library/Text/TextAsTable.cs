/*
 * User: aleksey.nakoryakov
 * Date: 15.03.12
 * Time: 15:59
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.DatabaseServices;

namespace Bargool.Acad.Library.Text
{
	/// <summary>
	/// Класс для работы с набором столбцов, сформированных из отдельных текстов
	/// </summary>
	public class TextAsTable<T>:IEnumerable<TextAsColumn<T>> where T: Entity
	{
		public List<TextAsColumn<T>>  columns { get; private set; }
		
//		public List<string> Titles{
//			get {
//				return cols
//					.Select(n => n.cells.First().TextString)
//					.ToList();
//			}
//		}
		
		public TextAsTable()
		{
			columns = new List<TextAsColumn<T>>();
		}
		
		public TextAsTable(T[] textEntities, Func<T, bool> IsHeader)
			: this(textEntities, IsHeader, (n => true)) {}
		
		public TextAsTable(T[] textEntities, Func<T, bool> IsHeader, Func<T, bool> IsColText)
			: this()
		{
			IEnumerable<T> texts = textEntities
				.Where(n => IsColText(n));
			foreach (T txt in texts
			         .Where(n => IsHeader(n)))
			{
				columns.Add(new TextAsColumn<T>(txt, texts.ToArray()));
			}
		}
		
		public TextAsTable(T[] textEntities, T[] headers)
			:this()
		{
			foreach (T txt in headers)
			{
				columns.Add(new TextAsColumn<T>(txt, textEntities));
			}
		}
		
//		public void ChangeTableValues(Dictionary<string, string[]> Table)
//		{
//			foreach (KeyValuePair<string, string[]> kvp in Table)
//			{
//				acad.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\nМеняем {0}", kvp.Key);
//				cols.Find(n=>n.cells.First().TextString==kvp.Key).ChangeCellsValues(kvp.Value);
//			}
//		}
		
//		public override string ToString()
//		{
//			StringBuilder sb = new StringBuilder();
//			foreach (SLTableColumn row in cols)
//				sb.AppendLine(row.ToString());
//			return sb.ToString();
//		}
		
		
		public IEnumerator<TextAsColumn<T>> GetEnumerator()
		{
			return columns.GetEnumerator();
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return columns.GetEnumerator();
		}
	}
}
