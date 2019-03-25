using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _00_Common
{
	public class PrintHelper
	{
		public static void PrintCollections<T>(string comment, IEnumerable<T> source, string delimiter = " ")
		{
			var plainString = source.Aggregate<T, string>(
					string.Empty,
					(acc, next) =>
					{
						string nextValue = next.ToString();

						if (acc == string.Empty)
						{
							return nextValue;
						}

						return string.Format("{0}{1}{2}", acc, delimiter, nextValue);
					}
			);

			PrintValue(comment, plainString);
		}

		public static void PrintValue(string comment, object value)
		{
			Console.WriteLine("\n{0}\n{1}", comment, value);
		}
	}
}
