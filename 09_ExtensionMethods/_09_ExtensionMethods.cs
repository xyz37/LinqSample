using _00_Common;
using System;
using System.Animals;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _09_ExtensionMethods
{
	class _09_ExtensionMethods
	{
		static void Main(string[] args)
		{
			_09_ExtensionMethods program = new _09_ExtensionMethods();

			Console.WriteLine("*** Basic Extension method ***");
			program.TestBasicExtensionMethod();

			Console.WriteLine();
			Console.WriteLine("*** Collection extensions ***");
			program.TestCollectionExtension();

			Console.WriteLine();
			Console.WriteLine("*** Object Collection extensions ***");
			program.TestObjectCollectionExtension();

			Console.ReadLine();
		}

		private void TestBasicExtensionMethod()
		{
			Person person = new Person
			{
				Name = "Robbin",
				Age = 46,
				Gender = Genders.Male,
			};

			person.Rest();      // 3 Extension methods
			person.Eat();
			person.Sleep();
			person.Rest();
			person.Eat();
			person.Sleep();

			IMammal mammal = new Mammal { Gender = Genders.Female };
			mammal.Rest();      // 1 Extension method
		}


		private void TestCollectionExtension()
		{
			IEnumerable<int> source = Enumerable.Range(0, 26);

			PrintHelper.PrintCollections("Original", source);

			var alphabet = source.Select(x => (char)('a' + x));
			PrintHelper.PrintCollections("Select: convert to char", alphabet);

			var countAlpha = alphabet.Count();
			PrintHelper.PrintValue("Alphabet count", countAlpha);

			var vowel = new[] { 'a', 'e', 'i', 'o', 'u' };
			var vowelCount = alphabet.Count(x => vowel.Contains(x) == true);
			PrintHelper.PrintValue("Vowel count", vowelCount);

			var vowelCountWhere = alphabet.Where(x => vowel.Contains(x) == true).Count();
			PrintHelper.PrintValue("Vowel count(Where)", vowelCount);

			var skip5 = alphabet.Skip(5);
			PrintHelper.PrintCollections("Skip 5", skip5);

			var take7 = skip5.Take(7);
			PrintHelper.PrintCollections("Take 7", take7);

			var orderByDesc = alphabet.OrderByDescending(x => x);
			PrintHelper.PrintCollections("Order by descending", orderByDesc);
		}


		private void TestObjectCollectionExtension()
		{
			var family = new[]
			{
				new Person{ Name = "Robbin", Age = 46, Gender =  Genders.Male, },
				new Person{ Name = "Anne", Age = 45, Gender = Genders.Female, },
				new Person{ Name = "Sam", Age = 13, Gender =  Genders.Male, },
				new Person{ Name = "Olivia", Age = 11, Gender = Genders.Female, },
			};

			var males = family.Where(person => person.Gender == Genders.Male);
			PrintHelper.PrintCollections("남성", males, "\n");

			var orderByName = family.OrderBy(person => person.Name);
			PrintHelper.PrintCollections("이름순 정렬", orderByName, "\n");

			var avgOfAges = family.Average(person => person.Age);
			PrintHelper.PrintValue("평균 나이", avgOfAges);

			var groupByGender = family.GroupBy(key => key.Gender)
				.ToDictionary(x => x.Key);
			PrintHelper.PrintCollections("성별 그룹", groupByGender, "\n");
			foreach (var key in groupByGender.Keys)
			{
				Console.WriteLine(key);

				foreach (Person person in groupByGender[key])
				{
					Console.WriteLine("\t{0}", person);
				}
			}
		}
	}
}
