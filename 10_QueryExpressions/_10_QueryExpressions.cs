using _00_Common;
using System;
using System.Animals;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _10_QueryExpressions
{
	class _10_QueryExpressions
	{
		static void Main(string[] args)
		{
			_10_QueryExpressions program = new _10_QueryExpressions();

			Console.WriteLine("*** Basic Query Expressions ***");
			program.TestBasicQueryExpressions();

			Console.WriteLine();
			Console.WriteLine("*** Advanced Query Expressions ***");
			program.TestAdvancedQueryExpressions();

			Console.WriteLine();
			Console.WriteLine("*** Join Query Expressions ***");
			program.TestJoinQueryExpressions();

			Console.ReadLine();
		}


		private Person[] family = new[]
		{
				new Person{ Name = "Robbin", Age = 46, Gender =  Genders.Male, },
				new Person{ Name = "Anne", Age = 45, Gender = Genders.Female, },
				new Person{ Name = "Sam", Age = 13, Gender =  Genders.Male, },
				new Person{ Name = "Olivia", Age = 11, Gender = Genders.Female, },
		};


		private void TestBasicQueryExpressions()
		{
			var persons = from p in family
						  where p.Age > 15
						  orderby p.Name
						  select p.Name;
			//select new { p.Name, p.Age, };
			PrintHelper.PrintCollections("15세 이상 가족(쿼리 식)", persons, Environment.NewLine);

			var personsExt = family.Where(p => p.Age > 15)
				.OrderBy(p => p.Name)
				//.Select(p => p.Name);
				.Select(p => string.Format("{0} ({1})", p.Name, p.Age));
			PrintHelper.PrintCollections("15세 이상 가족(확장 메서드)", personsExt, Environment.NewLine);
		}


		private void TestAdvancedQueryExpressions()
		{
			var groupByGender = family.GroupBy(key => key.Gender)
				.ToDictionary(x => x.Key);

			foreach (var key in groupByGender.Keys)
			{
				Console.WriteLine(key);

				foreach (Person person in groupByGender[key])
				{
					Console.WriteLine("\t{0}({1})", person.Name, person.Age);
				}
			}

			var byQueryExp = from person in family
							 where person.Age > 10
							 group person by person.Gender into genderGroup
							 orderby genderGroup.Key descending
							 select new
							 {
								 Gender = genderGroup.Key,
								 HighAge = (from gg in genderGroup
											select gg.Age).Max(),
								 LowAge = genderGroup.Min(gg => gg.Age),
								 Names = (from gg in genderGroup
										  select gg.Name).Aggregate((acc, next) => string.Format("{0},{1}", acc, next)),
							 };
			PrintHelper.PrintCollections("성별 그룹(쿼리 식)", byQueryExp, Environment.NewLine);

			var byExtMethod = family
							.Where(person => person.Age > 10)
							.GroupBy(person => person.Gender,
								(key, genderGroup) => new
								{
									Gender = key,
									HighAge = (from gg in genderGroup
											   select gg.Age).Max(),
									LowAge = genderGroup.Min(gg => gg.Age),
									Names = (from gg in genderGroup
											 select gg.Name).Aggregate((acc, next) => string.Format("{0},{1}", acc, next)),
								})
								.OrderByDescending(g => g.Gender);

			PrintHelper.PrintCollections("성별 그룹(확장 메서드)", byQueryExp, Environment.NewLine);
		}


		private enum Types
		{
			Dynamic,
			Static,
			Unkonwn,
		}

		private void TestJoinQueryExpressions()
		{
			var languages = new[]
			{
				new { Name = "C#", Type = Types.Static },
				new { Name = "Java", Type = Types.Static },
				new { Name = "Python", Type = Types.Dynamic },
				new { Name = "JavaScript", Type = Types.Dynamic },
				new { Name = "TypeScript", Type = Types.Static },
				new { Name = "NodeJS", Type = Types.Unkonwn },
			};
			var descriptions = new[]
			{
				new { Kind = Types.Dynamic, Desc = "동적 언어" },
				new { Kind = Types.Static, Desc = "정적 언어" },
			};

			var comments = from l in languages
						   join d in descriptions
						   on l.Type equals d.Kind
						   select string.Format("{0}\t{1}", l.Name, d.Desc);
			PrintHelper.PrintCollections("Join(쿼리 식 - inner join)", comments, Environment.NewLine);

			var commentsOut = from l in languages
							  join d in descriptions
							  on l.Type equals d.Kind
							  into results
							  from item in results.DefaultIfEmpty()
							  select string.Format("{0}\t{1}", l.Name, item != null ? item.Desc : "몰라");
			PrintHelper.PrintCollections("Join(쿼리 식 - outter join)", commentsOut, Environment.NewLine);


			var commentsEx = languages.Join(descriptions,
				o => o.Type,
				i => i.Kind,
				(l, d) => string.Format("{0}\t{1}", l.Name, d.Desc));
			PrintHelper.PrintCollections("Join(확장 메서드 - inner join)", comments, Environment.NewLine);


			var commentsOutEx = languages.GroupJoin(descriptions,
				o => o.Type,
				i => i.Kind,
				(lang, desc) => new
				{
					Languages = lang,
					Descriptions = desc.DefaultIfEmpty(),
				})
				.Select(x =>
				{
					var desc = x.Descriptions.FirstOrDefault();

					return string.Format("{0}\t{1}", x.Languages.Name, desc == null ? "몰라" : desc.Desc);
				});
			PrintHelper.PrintCollections("Join(확장 메서드 - outter join)", commentsOutEx, Environment.NewLine);
		}
	}
}
