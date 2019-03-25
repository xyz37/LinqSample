using System;
using System.Animals;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Animals
{
	public class Person : Mammal
	{
		public string Name { get; set; }
		public int Age { get; set; }

		public override bool IsTail { get { return false; } }

		public override string ToString()
		{
			return string.Format("{0} is {1}", Name, Action.ToString().ToLower());
		}
	}
}
