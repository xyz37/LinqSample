using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _02_Delegate
{
	public class EventClass
	{
		public event EventHandler<StartedEventArgs> Started;

		protected virtual void OnStarted(StartedEventArgs e)
		{
			if (Started != null)
			{
				Started(this, e);
			}
		}

		[System.Diagnostics.DebuggerStepThrough]
		public class StartedEventArgs : EventArgs
		{
			public StartedEventArgs(object tag)
			{
				Tag = tag;
			}

			public object Tag { get; set; }
		}

		public void Start()
		{
			OnStarted(new StartedEventArgs(DateTime.Now));
		}
	}
}
