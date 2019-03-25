using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _06_ImplicitlyTypedLocalVariables
{
	class _06_ImplicitlyTypedLocalVariables
	{
		static void Main(string[] args)
		{
			_06_ImplicitlyTypedLocalVariables program = new _06_ImplicitlyTypedLocalVariables();

			Console.WriteLine("*** Implicitly typed local variables ***");
			program.TestVarLongNamedClass();

			Console.WriteLine();
			Console.WriteLine("*** 암시적 배열 ***");
			program.TestAnonymousType();

			Console.ReadLine();
		}

		private class 김수한무거북이와두루미삼천갑자동방삭치치카포사리사리센타워리워리세브리깡무두셀라구름이허리케인에담벼락담벼락에서생원서생원에고양이고양이엔바둑이바둑이는돌돌이
		{
			public override string ToString()
			{
				return GetType().Name;
			}
		}

		private void TestVarLongNamedClass()
		{
			김수한무거북이와두루미삼천갑자동방삭치치카포사리사리센타워리워리세브리깡무두셀라구름이허리케인에담벼락담벼락에서생원서생원에고양이고양이엔바둑이바둑이는돌돌이 longNamedClass1 = new 김수한무거북이와두루미삼천갑자동방삭치치카포사리사리센타워리워리세브리깡무두셀라구름이허리케인에담벼락담벼락에서생원서생원에고양이고양이엔바둑이바둑이는돌돌이();
			var longNamedClass = new 김수한무거북이와두루미삼천갑자동방삭치치카포사리사리센타워리워리세브리깡무두셀라구름이허리케인에담벼락담벼락에서생원서생원에고양이고양이엔바둑이바둑이는돌돌이();

			Console.WriteLine("longNamedClass name length: {0}", longNamedClass.ToString().Length);

			var button = new Button
			{
				Text = "Hint 버튼",       // object initializer
			};
		}


		private void TestAnonymousType()
		{
			var family = new[]
			{
				new { Name = "Robbin", Age = 46, Gender = "Male", },
				new { Name = "Anne", Age = 45, Gender = "Female", },
				new { Name = "Sam", Age = 13, Gender = "Male", },
				new { Name = "Olivia", Age = 11, Gender = "Female", },
			};

			Console.WriteLine(family);

			foreach (var person in family)
			{
				Console.WriteLine(person);
			}
		}
	}
}
