using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceClsAppl
{
	class Program
	{
		static void Main(string[] args)
		{
			string holdprograme = "";
			CDog dg = new CDog("Doggie");
			dg.HelloAnimal();
			dg.SetAnimal("dog that shat on the mat");
			dg.SetAnimal(); 
			CCat ct = new CCat("Cat");
			ct.HelloAnimal();
			ct.SetAnimal("Cat that sat on the mat");
			holdprograme = Console.ReadLine();
			 
			
		}
	}
}
