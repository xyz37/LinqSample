using System;
using System.Animals;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Animals
{
	public static class MammalExtensions
	{
		public static void Rest(this IMammal mammal)
		{
			mammal.Action = MammalActions.Resting;
			Console.WriteLine("{0} with IMammal", mammal);
		}

		public static void Rest(this Mammal mammal)
		{
			mammal.Action = MammalActions.Resting;
			Console.WriteLine("{0} with Mammal", mammal);
		}

		public static void Rest(this Person person)
		{
			person.Action = MammalActions.Resting;
			Console.WriteLine(person);
		}

		public static void Walk(this IMammal mammal)
		{
			mammal.Action = MammalActions.Walking;
			Console.WriteLine(mammal);
		}

		public static void Run(this IMammal mammal)
		{
			mammal.Action = MammalActions.Running;
			Console.WriteLine(mammal);
		}

		public static void Sleep(this IMammal mammal)
		{
			mammal.Action = MammalActions.Sleeping;
			Console.WriteLine(mammal);
		}

		public static void Eat(this IMammal mammal)
		{
			mammal.Action = MammalActions.Eating;
			Console.WriteLine(mammal);
		}
	}
}
