using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorAutoSpool
{
	public interface IProcessPart
	{
		string DiaID(string DiaOD, string Thickness);
		string DiaOD(string Thickness);
		void SetExtents(string Height,string Width,string Length,string Thickness);

		void NumberOfHoles();
		void Radius();
	}
}