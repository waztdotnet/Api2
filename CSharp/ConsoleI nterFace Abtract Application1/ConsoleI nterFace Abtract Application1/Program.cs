using System;


namespace ConsoleI_nterFace_Abtract_Application1
{

	public class Customer1
	{
		private string  Customer1_ID;

		public Customer1()
		{
			Customer1_ID = "Customer 1";
		}

		public void Print()
		{
			Console.WriteLine("Customer 1");
		}

		public string GetCustomer1_ID()
		{
			return Customer1_ID;
		}
	}

	public class Customer2 : ICustomer2
	{
		private string Customer2_ID;

		public Customer2()
		{
			Customer2_ID = "Customer 1";
		}

		public void Print()
		{
			Console.WriteLine("Customer 2");
		}
		public string GetCustomer2_ID()
		{
			return Customer2_ID;
		}
	}

	public abstract  class abstractCustomer1
	{
		private string abstractCustomer1_ID;
		public void Print()
		{
			Console.WriteLine("Abstract Customer 1");
		} 

	}

	public abstract class abstractCustomer2
	{
		private string abstractCustomer2_ID;
		public void Print()
		{
			Console.WriteLine("Abstract Customer 2");
		}
	}

	public interface  ICustomer1
	{
		void Print();
		string GetCustomer1_ID();
	}

	public interface ICustomer2
	{
		void Print();
		string GetCustomer2_ID();
	}


	public class Program
	{
		static void Main(string[] args)
		{

			Console.ReadLine();  
		}
	}
}
