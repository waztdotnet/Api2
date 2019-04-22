using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CslInhertance
{
	class Program
	{
		static void Main(string[] args)
		{
			CPipe pipe = new CPipe(120, 12);
			Console.WriteLine(pipe.GetPipeDiaID());
			ITubePipe Tpipe = pipe;
			Tpipe.PipeDiamensions(788, 25, 3000);


			Console.WriteLine(Tpipe.GetPipeDiaID());
			
			Console.ReadLine();  
			//Console.WriteLine("{0}{1}", var,var);  
		}
	}
}
