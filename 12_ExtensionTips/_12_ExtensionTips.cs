using _00_Common;
using System;
using System.Animals;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace _12_ExtensionTips
{
	class _12_ExtensionTips
	{
		static void Main(string[] args)
		{
			// 00_Common.SystemExtensions folder 참고
			_12_ExtensionTips program = new _12_ExtensionTips();

			Console.WriteLine("*** Enum Extension Tips ***");
			program.TestEnumExtensions();

			Console.WriteLine("\n*** Generic Extension Tips ***");
			program.TestGenericExtensions();

			Console.WriteLine("\n*** String Extension Tips ***");
			program.TestStringExtensions();

			Console.WriteLine("\n*** Type Extension Tips ***");
			program.TestTypeExtensions();

			Console.WriteLine("\n*** KeyEqualityComparer<T> Tips ***");
			program.TestKeyEqualityComparerExtensions();

			Console.ReadLine();
		}


		private void TestEnumExtensions()
		{
			var gender = Genders.Male;

			Console.WriteLine("{0}: {1}", gender, gender.GetDescription());

			var actions = MammalActions.Eating;

			actions.ForEach<MammalActions>(@enum =>
			{
				Console.WriteLine("{0} => {1}", @enum.ToString(), @enum.GetDescription());
			});
		}


		private List<Robot> Robots1 = new List<Robot>
		{
			new Robot { Name = "Robot1", LegCount= 4, BatteryLife = 4 },
			new Robot { Name = "Robot2", LegCount= 2, BatteryLife = 8 },
			new Robot { Name = "Robot3", LegCount= 2, BatteryLife = 12 },
			new Robot { Name = "Robot4", LegCount= 2, BatteryLife = 8 },
			new Robot { Name = "Robot5", LegCount= 4, BatteryLife = 6 },
		};
		private List<Robot> Robots2 = new List<Robot>
		{
			new Robot { Name = "Robot11", LegCount= 4, BatteryLife = 4 },
			new Robot { Name = "Robot12", LegCount= 2, BatteryLife = 8 },
			new Robot { Name = "Robot13", LegCount= 2, BatteryLife = 12 },
		};


		private void TestGenericExtensions()
		{
			var robot3 = new Robot { Name = "Robot3" };

			var containRobot3 = robot3.In(Robots1);
			Console.WriteLine("Contain robot3:  {0}", containRobot3);

			var robot13 = Robots2.SingleOrDefault(x => x.Name == "Robot13");
			var containRobot13 = robot13.In(Robots2);
			Console.WriteLine("Contain robot13: {0}", containRobot13);

			containRobot3 = robot3.In(Robots1, p => p.Name == robot3.Name);
			Console.WriteLine("Contain robot3:  {0}", containRobot3);

			var drones = Robots2.ConvertToList<Robot, Drone>();
			foreach (var drone in drones)
			{
				Console.WriteLine(drone);
			}

			var dronString = drones.MergeDelimiter(" | ");
			Console.WriteLine("Drone string: {0}", dronString);

			var droneTable = new DataTable();

			droneTable.Columns.Add("Name");
			droneTable.Columns.Add("FlyCount", typeof(int));
			droneTable.Columns.Add("BatteryLife", typeof(int));

			droneTable.Rows.Add("Drone1", 4, 6);
			droneTable.Rows.Add("Drone2", 8, 6);
			droneTable.Rows.Add("Drone3", 6, 12);

			Console.WriteLine("Drone List");
			var droneList = droneTable.ConvertToList<Drone>();
			foreach (var drone in droneList)
			{
				Console.WriteLine(drone);
			}
		}


		private void TestStringExtensions()
		{
			var stringCommaDelimiter = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,,,,";
			var stringDelimiter1 = stringCommaDelimiter.SplitDelimiter(",");
			var stringDelimiter2 = stringCommaDelimiter.SplitDelimiter<string>(",", StringSplitOptions.None);
			var stringDelimiter3 = stringCommaDelimiter.SplitDelimiter<int>(",", StringSplitOptions.None);

			Console.WriteLine(stringCommaDelimiter);
			PrintHelper.PrintCollections("String delimiter1", stringDelimiter1, "_");
			PrintHelper.PrintCollections("String delimiter2", stringDelimiter2, "_");
			PrintHelper.PrintCollections("String delimiter3", stringDelimiter3, "_");

			var stringMerged = stringDelimiter1.MergeDelimiter("*");
			Console.WriteLine("String merged: {0}", stringMerged);
		}


		private void TestTypeExtensions()
		{
			var person = new Person();

			Console.WriteLine("Person: {0}", person);
			person.InvokeSetProperty("Name", "홍길동");
			Console.WriteLine("Person: {0}", person);

			var personType = typeof(Person);

			Console.WriteLine("Derived Animal: {0}", personType.IsDerivedBaseType("animal"));
			Console.WriteLine("Derived Mammal: {0}", personType.IsDerivedBaseType("Ma"));
		}


		private class RobotComparer : IEqualityComparer<Robot>
		{
			public bool Equals(Robot x, Robot y)
			{
				if (Object.ReferenceEquals(x, y))
				{
					return true;
				}

				if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				{
					return false;
				}

				return x.LegCount.Equals(y.LegCount);
			}

			public int GetHashCode(Robot obj)
			{
				if (Object.ReferenceEquals(obj, null))
				{
					return 0;
				}

				int hashProductName = obj.Name == null ? 0 : obj.Name.GetHashCode();
				int hashProductCode = obj.Name.GetHashCode();

				return hashProductName ^ hashProductCode;
			}
		}

		private void TestKeyEqualityComparerExtensions()
		{
			Console.WriteLine("With IEqualityComparer");
			var distinct1 = Robots1.Distinct(new RobotComparer());

			foreach (var robot in distinct1)
			{
				Console.WriteLine(robot);
			}

			Console.WriteLine("With KeyEqualityComparer");
			var distinct2 = Robots1.Distinct(new KeyEqualityComparer<Robot>(k => k.LegCount));

			foreach (var robot in distinct2)
			{
				Console.WriteLine(robot);
			}
		}
	}
}
