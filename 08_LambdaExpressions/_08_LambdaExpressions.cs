using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _08_LambdaExpressions
{
	class _08_LambdaExpressions
	{
		static void Main(string[] args)
		{
			_08_LambdaExpressions program = new _08_LambdaExpressions();

			Console.WriteLine("*** Lambda basic ***");
			program.TestLambdaBasic();

			Console.WriteLine();
			Console.WriteLine("*** Variable scope ***");
			program.TestScope();

			Console.WriteLine();
			Console.WriteLine("*** Lambda parameter ***");
			program.TestLambdaParameter();

			Console.ReadLine();
		}

		private void TestLambdaBasic()
		{
			Action bar = () => Console.WriteLine(string.Empty.PadLeft(20, '-'));
			Action<string> logger = msg => Console.WriteLine(msg);
			logger("log message");
			bar();

			Func<int, int> square = x => x * x;
			Console.WriteLine("square: {0}", square(3));
			bar();

			Func<int, bool> isOdd = x => (x % 2) == 1;
			Console.WriteLine("isOdd(11): {0}", isOdd(11));
			Console.WriteLine("isOdd(20): {0}", isOdd(20));
			bar();

			Func<int, int, bool> isEqual = (x, y) => x == y;
			Console.WriteLine("isEqual(5 % 2, 5 & 1): {0}", isEqual(5 % 2, 5 & 1));
			bar();
		}


		private void TestScope()
		{
			int baseNum = 1;        // extern variable
			Func<int, int> pow = num =>
			{
				Console.WriteLine("외부 변수(baseNum): {0}", baseNum);
				return baseNum << num;
			};
			// Func<int, int> pow = num => baseNum << num;

			Console.WriteLine("pow(10)    : {0}", pow(10));

			// Console.WriteLine("내부 변수(num): {0}", num);		// 외부에서 참조 안됨 Compiler error

			// Func<int, int, int> pow2 = (baseNum, num) => baseNum << num;		// baseNum이 현재 local 변수로 사용하고 있어서 Compiler error
			Func<int, int, int> pow2 = (baseNum2, num) => baseNum2 << num;
			Console.WriteLine("pow2({0}, 10): {1}", baseNum, pow2(baseNum, 10));

		}


		private void TestLambdaParameter()
		{
			Func<int, int, int> adder = (x, y) => x + y;

			Calculator(20, 10, adder);
			Calculator(20, 10, (x, y) => x - y);
		}

		private void Calculator(int x, int y, Func<int, int, int> @operator)
		{
			Console.WriteLine("x: {0}, y: {1} = {2}", x, y, @operator(x, y));
		}
	}
}
