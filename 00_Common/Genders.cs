using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Animals
{
	public enum Genders
	{
		[Description("남자")]
		Male = 0,
		[Description("여자")]
		Female,
	}
}
