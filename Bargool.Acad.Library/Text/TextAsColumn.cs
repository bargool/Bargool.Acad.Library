/*
 * User: aleksey.nakoryakov
 * Date: 15.03.12
 * Time: 16:19
 */
using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace Bargool.Acad.Library.Text
{
	/// <summary>
	/// Класс для работы со столбцом, сформированным из отдельных текстов
	/// </summary>
	public class TextAsColumn<T> where T : Entity
	{
		public List<T> cells { get; private set; }
		public TextAsColumn()
		{
			cells = new List<T>();
		}
		
		public TextAsColumn(T header, T[] texts)
		{
			cells = new List<T>();
			cells.Add(header);
			cells.AddRange(FindCol(texts));
		}
		
		List<T> FindCol(T[] texts)
		{
			Point3d min = cells.First().GeometricExtents.MinPoint;
			Point3d max = cells.First().GeometricExtents.MaxPoint;
			return texts
				.Where(n =>{
				       	Point3d nmin = n.GeometricExtents.MinPoint;
				       	Point3d nmax = n.GeometricExtents.MaxPoint;
				       	if (((min.X>=nmin.X)&&(min.X<=nmax.X))||
				       	    ((max.X>=nmin.X)&&(max.X<=nmax.X))||
				       	    ((min.X<=nmin.X)&&(max.X>=nmax.X)))
				       	{
				       		if (min.Y>nmax.Y)
				       			return true;
				       	}
				       	return false;
				       })
				.Select(n => n)
				.OrderByDescending(n => n.GeometricExtents.MinPoint.Y)
				.ToList();
		}
	}
}
