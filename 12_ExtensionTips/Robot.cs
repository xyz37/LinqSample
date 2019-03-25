using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _12_ExtensionTips
{
	public class Robot
	{
		public string Name { get; set; }
		public int LegCount { get; set; }
		public int BatteryLife { get; set; }

		public override string ToString()
		{
			return string.Format("Name:{0}, LegCount:{1}, BatteryLife:{2}", Name, LegCount, BatteryLife);
		}
	}
}
