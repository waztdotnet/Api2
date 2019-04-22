using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CslInhertance
{
	public class CPipe : ITubePipe
	{
		private double DiaOD;
		private double DiaID;
		private double Length;
		protected double Thickness;

		public CPipe(double diaOD,  double thickness,double length)
		{
			DiaOD = diaOD;
			Length = length;
			Thickness = thickness;
		}

		public CPipe(double diaOD, double thickness)
		{
			DiaOD = diaOD;
			Thickness = thickness;
		}

		public CPipe()
		{
			
		}

		public void PipeDiamensions(double diaOD, double thickness, double length)
		{
			DiaOD = diaOD;
			Length = length;
			Thickness = thickness;
		}

		public void FlangeDiamensions(double diaOD, double diaID, double thickness, double PCD, int NumberOfHoles, double HoleDia, string FlangeType)
		{
			
		}

		public double GetPipeDiaID()
		{
			return DiaOD-(2*Thickness ); 
		}
	}
}