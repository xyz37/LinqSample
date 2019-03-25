using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Animals
{
	public class Mammal : IMammal
	{
		public virtual bool IsTail { get; protected set; }

		public MammalActions Action { get; set; }

		public Genders Gender { get; set; }

		public override string ToString()
		{
			return string.Format("{0} is {1}", GetType().Name, Action.ToString());
		}
	}
}
