using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InheritanceClsAppl
{
	public abstract  class CAnimal
	{
		protected string szType;

		public CAnimal(string Animaltype)
		{
			szType = Animaltype;
		}

		public virtual void HelloAnimal()
		{
			Console.WriteLine("Animal Type " + szType);
		}

		public virtual void SetAnimal(string SetAnimal)
		{
			szType = SetAnimal;
			Console.WriteLine("Iam Really " + szType);
		}
	}



}