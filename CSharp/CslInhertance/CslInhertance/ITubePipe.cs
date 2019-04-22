using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CslInhertance
{
	public interface ITubePipe
	{
		void PipeDiamensions(double diaOD, double thickness, double length);
		void FlangeDiamensions(double diaOD, double diaID, double thickness, double PCD, int NumberOfHoles, double HoleDia, string FlangeType);
		double GetPipeDiaID();
	}
}