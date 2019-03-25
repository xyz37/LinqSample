using System;

namespace System.Animals
{
	public interface IMammal
	{
		MammalActions Action { get; set; }
		Genders Gender { get; set; }
	}
}
