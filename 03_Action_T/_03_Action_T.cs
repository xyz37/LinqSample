using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _03_Action_T
{
	class _03_Action_T
	{
		static void Main(string[] args)
		{
			_03_Action_T program = new _03_Action_T();

			Console.WriteLine("*** Action<int, int> ***");
			program.TestBasicAction();

			Console.WriteLine();
			Console.WriteLine("*** Func<int, int, string> ***");
			program.TestBasicFunc();

			Console.ReadLine();
		}

		private void TestBasicAction()
		{
			Action<int, int> adder = AdderAction;

			adder(10, 20);
			adder.Invoke(30, 40);
		}

		private void AdderAction(int arg1, int arg2)
		{
			Console.WriteLine("{0} + {1} = {2}", arg1, arg2, arg1 + arg2);
		}


		private void TestBasicFunc()
		{
			Func<int, int, string> adder = AdderFunc;

			Console.WriteLine("adder(10, 20)       : {0}", adder(10, 20));
			Console.WriteLine("adder.Invoke(30, 40): {0}", adder.Invoke(30, 40));
		}

		private string AdderFunc(int arg1, int arg2)
		{
			return string.Format("{0} + {1} = {2}", arg1, arg2, arg1 + arg2);
		}
	}
}
