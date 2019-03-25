using System;
using System.Collections.Generic;
using System.Linq;

namespace _12_ExtensionTips
{
	public class Drone
	{
		public string Name { get; set; }
		public int FlyCount { get; set; }
		public int BatteryLife { get; set; }

		public override string ToString()
		{
			return string.Format("Name:{0}, FlyCount:{1}, BatteryLife:{2}", Name, FlyCount, BatteryLife);
		}
	}
}
