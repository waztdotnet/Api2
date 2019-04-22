using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InheritanceClsAppl
{
	public class CDog : CAnimal, IAnimal
	{
		//private string szType;

		public CDog(string Animaltype) : base(Animaltype)
		{
			szType = Animaltype;
		}

		public override void HelloAnimal()
		{
			

			Console.WriteLine("My Type is " + szType);
		}

		public void SetAnimal()
		{
			Console.WriteLine("My Type i");
		}

		public new void SetAnimal(string SetAnimal) 
		{
			szType = SetAnimal;
			Console.WriteLine("Iam Really " + szType);
		}
	}

	public class CCat : CAnimal, IAnimal
	{


		public CCat(string Animaltype) : base(Animaltype)
		{
			szType = Animaltype;
		}

		public override void HelloAnimal()
		{
			//base.HelloAnimal();

			Console.WriteLine("My Type is " + szType);
		}

		public override void SetAnimal(string SetAnimal)
		{
			szType = SetAnimal;
			Console.WriteLine("Iam Really REA " + szType);
		}
	}
}