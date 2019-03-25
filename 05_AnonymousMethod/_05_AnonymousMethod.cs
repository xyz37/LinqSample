using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _05_AnonymousMethod
{
	class _05_AnonymousMethod
	{
		static void Main(string[] args)
		{
			_05_AnonymousMethod program = new _05_AnonymousMethod();

			Console.WriteLine("*** Anonymous methods ***");
			program.TestAnonymousMethod();

			Console.ReadLine();
		}
		private void TestAnonymousMethod()
		{
			Button button = new Button();

			button.Click += OnButtonClick;
			button.Click += new EventHandler(OnButtonClick);
			button.Click += delegate (object sender, EventArgs e)
			{
				Console.WriteLine("Button clicked by anonymous method 1(무명 메서드)");
			};
			button.Click += (EventHandler)delegate (object sender, EventArgs e)
			{
				Console.WriteLine("Button clicked by anonymous method 2(무명 메서드)");
			};
			button.Click += delegate
			{
				Console.WriteLine("Button clicked by anonymous method 3(무명 메서드)");
			};

			button.Click += (sender, e) =>
			{
				Console.WriteLine("Button clicked by lambda expressions(람다 식)");
			};

			button.PerformClick();
		}

		private void OnButtonClick(object sender, EventArgs e)
		{
			Console.WriteLine("Button clicked by named method(명명 메서드)");
		}
	}
}
