using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Animals
{
	public enum MammalActions
	{
		[Description("resting")]
		Resting = 0,
		[Description("walking")]
		Walking,
		[Description("running")]
		Running,
		[Description("sleeping")]
		Sleeping,
		[Description("eating")]
		Eating,

	}
}
