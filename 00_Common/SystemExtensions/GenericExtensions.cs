using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.ComponentModel;
using System.Linq.Expressions;

namespace System.Collections.Generic
{
	/// <summary>
	/// Generic collection의 확장 기능을 제공합니다.
	/// </summary>
	public static class GenericExtensions
	{
		#region In
		/// <summary>
		/// 사용한 인스턴스가 컬렉션에 포함이 되었는지 여부를 구합니다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="TSource">검색하려는 인스턴스</param>
		/// <param name="collections"><seealso cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>을 구현한 컬렉션</param>
		/// <returns>컬렉션에 항목이 포함되면 true이고, 그렇지 않으면 false 입니다.</returns>
		/// <exception cref="System.ArgumentNullException">collection 또는 predicate가 null인 경우</exception>
		public static bool In<T>(this T TSource, IEnumerable<T> collections)
		{
			return collections.Contains(TSource);
		}

		/// <summary>
		/// 사용한 인스턴스가 컬렉션에 포함이 되었는지 여부를 테스트하는 함수를 통해서 구합니다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="TSource">검색하려는 인스턴스</param>
		/// <param name="collections"><seealso cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>을 구현한 컬렉션</param>
		/// <param name="predicate">각 요소를 조건에 대해 테스트하는 함수</param>
		/// <returns>컬렉션에 항목이 포함되면 true이고, 그렇지 않으면 false 입니다.</returns>
		/// <exception cref="System.ArgumentNullException">collection 또는 predicate가 null인 경우</exception>
		public static bool In<T>(this T TSource, IEnumerable<T> collections, Func<T, bool> predicate)
		{
			return collections.Where(predicate).Count() > 0;
		}
		#endregion

		#region ToDictionary<T>
		/// <summary>
		/// 속성 = 값 형태의 익명 타입에서 IDictionary 형태의 값으로 변환 합니다.
		/// <remarks>키에 해당하는 문자열은 <see cref="System.StringComparer.OrdinalIgnoreCase" />로 지정되어 비교됩니다.</remarks>
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="exceptDefaultValue">기본값이 있는 속성은 제외하고 구할지 여부를 결정합니다.</param>
		/// <returns>IDictionary{System.String, dynamic}.</returns>
		/// <exception cref="System.ArgumentNullException">target</exception>
		/// <example>
		///		<code>
		/// object anonymous = new
		/// {
		///		Id = 1234,
		///		Password = "pwd",
		/// };
		/// var dictionary = anonymous.ToDictionary();
		///		</code>
		/// </example>
		public static IDictionary<string, dynamic> ToDictionary<T>(this T target, bool exceptDefaultValue = false)
			where T : class
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			IDictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>(System.StringComparer.OrdinalIgnoreCase);

			foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(target))
			{
				object value = propertyDescriptor.GetValue(target);

				if (exceptDefaultValue == true)
				{
					if (value == null || value.Equals(propertyDescriptor.PropertyType.DefaultValue()) == true)
					{
						continue;
					}
				}

				dictionary.Add(propertyDescriptor.Name, value);
			}

			return dictionary;
		}
		#endregion

		#region ConvertTo<F, T>
		/// <summary>
		/// F type을 T type으로 변경합니다.
		/// </summary>
		/// <remarks>F와 T type은 이름과 타입이 같은 속성(Property)이 있어야 합니다.</remarks>
		/// <typeparam name="F">변환 하고자 하는 type</typeparam>
		/// <typeparam name="T">변환 할 type</typeparam>
		/// <param name="fromType">변환 하고자 하는 type</param>
		/// <returns>성공 적으로 변환하면 변환된 T type을 반환하고, 그렇지 않으면 빈 인스턴스를 반환합니다.</returns>
		public static T ConvertTo<F, T>(this F fromType)
			where T : class, new()
			where F : class, new()
		{
			List<F> fromTypes = new List<F>();

			fromTypes.Add(fromType);

			List<T> toTypes = ConvertToList<F, T>(fromTypes);

			return toTypes != null && toTypes.Count == 1 ? toTypes[0] : new T();
		}
		#endregion

		#region ConvertToList<T>
		/// <summary>
		/// DataTable을 T type의 List 컬렉션으로 변경합니다.
		/// <remarks>컬럼의 대소문자를 구별하지 않습니다.(변환 속도가 느립니다.)</remarks>
		/// </summary>
		/// <typeparam name="T">DataTable을 변환하고자 하는 type</typeparam>
		/// <param name="dataTable">변환 시킬 DataTable</param>
		/// <returns>
		/// 성공적으로 변환하면 변환된 List&lt;T&gt; 컬렉션을 반환하고, 그렇지 않으면 Count가 0인 빈 컬렉션을 반환합니다.
		/// </returns>
		/// <seealso cref="ConvertToList(DataTable, bool)"/>
		public static List<T> ConvertToList<T>(this DataTable dataTable)
			where T : class, new()
		{
			bool columnNameCaseSensitive = false;

			return ConvertToList<T>(dataTable, columnNameCaseSensitive);
		}

		/// <summary>
		/// DataTable을 T type의 List 컬렉션으로 변경합니다.
		/// </summary>
		/// <typeparam name="T">DataTable을 변환하고자 하는 type</typeparam>
		/// <param name="dataTable">변환 시킬 DataTable</param>
		/// <param name="columnNameCaseSensitive">column의 대소문자를 구별할지 여부(구별하지 않을 경우 변환 속도가 조금 향상 됩니다.)</param>
		/// <returns>
		/// 성공적으로 변환하면 변환된 List&lt;T&gt; 컬렉션을 반환하고, 그렇지 않으면 Count가 0인 빈 컬렉션을 반환합니다.
		/// </returns>
		public static List<T> ConvertToList<T>(this DataTable dataTable, bool columnNameCaseSensitive)
			where T : class, new()
		{
			List<T> returnCollection = new List<T>();

			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				List<string> columns = new List<string>();
				Dictionary<string, Type> columms = new Dictionary<string, Type>();      // 해당 테이블의 컬럼 리스트를 미리 구한다.

				if (columnNameCaseSensitive == true)
				{
					foreach (DataColumn dataColumn in dataTable.Columns)
					{
						columms.Add(dataColumn.ColumnName, dataColumn.DataType);
					}
				}

				// Column 이름으로 사용할 Property 구하기
				foreach (DataRow row in dataTable.Rows)
				{
					T t = new T();
					Type type = t.GetType();

					foreach (PropertyInfo pi in type.GetProperties())
					{
						try
						{
							// 컬럼 대소문자를 구별하고
							// 해당 컬럼이 DB에서 없을 경우는 skip
							if (columnNameCaseSensitive == true && columms.ContainsKey(pi.Name) == false)
							{
								continue;
							}

							object value = row[pi.Name];

							if (Convert.IsDBNull(value) == true)
							{
								value = null;
							}

							if (dataTable.Columns[pi.Name].DataType.Equals(typeof(decimal)) == true)
							{
								// Oracle Number/Integer type을 Decimal로 변환해서 Return 하기 때문에 처리한다						
								// 변환 객체의 Type이 bool 이면 Database Type이 Decimal 이라도 bool로 변환 한다.
								if (pi.PropertyType == typeof(System.Boolean))
								{
									value = Convert.ToBoolean(value);
								}
								else if (pi.PropertyType == typeof(System.Int32))
								{
									value = Convert.ToInt32(value);
								}
								else if (pi.PropertyType == typeof(System.Double))
								{
									value = Convert.ToDouble(value);
								}
							}

							pi.SetValue(t, value, null);
						}
						catch (ArgumentException)
						{
							// 해당 컬럼이 DB에서 없을 경우는 skip(오류날 경우 대비해서 이중 체크 한다)
							continue;
						}
					}

					returnCollection.Add(t);
				}
			}

			return returnCollection;
		}

		/// <summary>
		/// F type의 List 컬렉션을 T type의 List 컬렉션으로 변경합니다.
		/// </summary>
		/// <remarks>F와 T type은 이름과 타입이 같은 속성(Property)이 있어야 합니다.</remarks>
		/// <typeparam name="F">변환 하고자 하는 type</typeparam>
		/// <typeparam name="T">변환 할 type</typeparam>
		/// <param name="fromTypes">변환 하고자 하는 type의 컬렉션</param>
		/// <returns>
		/// 성공적으로 변환하면 변환된 List&lt;T&gt; 컬렉션을 반환하고, 그렇지 않으면 Count가 0인 빈 컬렉션을 반환합니다.
		/// </returns>
		public static List<T> ConvertToList<F, T>(this List<F> fromTypes)
			where T : class, new()
			where F : class, new()
		{
			List<T> returnCollection = new List<T>();

			if (fromTypes != null && fromTypes.Count > 0)
			{
				foreach (F f in fromTypes)
				{
					T t = new T();
					Type toType = t.GetType();
					Type fromType = f.GetType();

					foreach (PropertyInfo fPi in fromType.GetProperties())
					{
						if (fPi.CanRead == false)
						{
							continue;
						}

						PropertyInfo tPi = toType.GetProperty(fPi.Name);

						if (tPi != null
							&& tPi.CanWrite == true)
						{
							Type fromPropType = fPi.PropertyType.GetNullableUnderlyingType() ?? fPi.PropertyType;
							Type toPropType = tPi.PropertyType.GetNullableUnderlyingType() ?? tPi.PropertyType;
							object fPiValue = fPi.GetValue(f, BindingFlags.GetProperty, null, null, null);

							// 타입이 같으면 값을 할당한다.
							if (fromPropType == toPropType)
							{
								tPi.SetValue(t, fPiValue, null);
							}
							else if (TypeDescriptor.GetConverter(toPropType).IsValid(fPiValue) == true)
							{
								tPi.SetValue(
									t,
									TypeDescriptor.GetConverter(toPropType).ConvertFrom(fPiValue),
									null);
							}
						}
					}

					returnCollection.Add(t);
				}
			}

			return returnCollection;
		}

		/// <summary>
		/// F type의 List 컬렉션을 T type의 List 컬렉션으로 변경합니다.(같은 Property와 PropertyType이 있어야 합니다.)
		/// </summary>
		/// <typeparam name="F">변환 하고자 하는 type</typeparam>
		/// <typeparam name="T">변환 할 type</typeparam>
		/// <param name="fromTypes">변환 하고자 하는 type의 컬렉션</param>
		/// <returns>
		/// 성공적으로 변환하면 변환된 List&lt;T&gt; 컬렉션을 반환하고, 그렇지 않으면 Count가 0인 빈 컬렉션을 반환합니다.
		/// </returns>
		public static List<T> ConvertToList<F, T>(this IEnumerable<F> fromTypes)
			where T : class, new()
			where F : class, new()
		{
			if (fromTypes == null)
			{
				return new List<T>();
			}

			// fromTypes이 LINQ 컬렉션(ISingleResult, IQueryable...) 일 경우 
			// 데이터를 구하기 위해 쿼리를 여러번 할 경우 exception이 발생한다.
			return ConvertToList<F, T>(fromTypes.ToList());
		}
		#endregion

		#region Minus
		/// <summary>
		/// outer 컬렉션에 없는 inner 컬렉션의 항목만을 구합니다.
		/// </summary>
		/// <typeparam name="T">null이 가능한 구조체 타입입니다.</typeparam>
		/// <param name="inner">기준이 되는 컬렉션입니다.</param>
		/// <param name="outer">비교하려는 컬렉션입니다.</param>
		/// <returns>inner 컬렉션과 작거나 같은 값이 나옵니다.</returns>
		public static IEnumerable<T> Minus<T>(
			this IEnumerable<T> inner,
			IEnumerable<T> outer)
			where T : struct
		{
			return from i in inner
				   from o in
					   (from o in outer
						where o.Equals(i)
						select (T?)o
					   ).DefaultIfEmpty()
				   where o == null
				   select i;
		}

		/// <summary>
		/// outer 컬렉션에 없는 inner 컬렉션의 항목만을 구합니다.
		/// </summary>
		/// <typeparam name="T">클래스 타입입니다.</typeparam>
		/// <typeparam name="TKey">키가되는 타입입니다.</typeparam>
		/// <param name="inner">기준이 되는 컬렉션입니다.</param>
		/// <param name="outer">비교하려는 컬렉션입니다.</param>
		/// <param name="compareKeyExpression">T 타입에서 비교하려는 key의 표현식입니다.</param>
		/// <returns>inner 컬렉션과 작거나 같은 값이 나옵니다.</returns>
		public static IEnumerable<T> Minus<T, TKey>(
			this IEnumerable<T> inner,
			IEnumerable<T> outer,
			Expression<Func<T, TKey>> compareKeyExpression)
			where T : class
		{
			var compiledCondition = compareKeyExpression.Compile();
			return from i in inner
				   from o in
					   (from o in outer
						where compiledCondition(o).Equals(compiledCondition(i))
						select o
					   ).DefaultIfEmpty()
				   where o == null
				   select i;
		}

		/// <summary>
		/// outer 컬렉션에 없는 inner 컬렉션의 항목만을 구합니다.
		/// </summary>
		/// <param name="inner">기준이 되는 문자열 컬렉션입니다.</param>
		/// <param name="outer">비교하려는 문자열 컬렉션입니다.</param>
		/// <returns>inner 컬렉션과 작거나 같은 값이 나옵니다.</returns>
		public static IEnumerable<string> Minus(
			this IEnumerable<string> inner,
			IEnumerable<string> outer)
		{
			return inner.Minus(outer, x => x);
		}
		#endregion

		#region Duplicates<T>
		/// <summary>
		/// source 컬렉션에서 중복 또는 별개 항목을 반환합니다.
		/// </summary>
		/// <typeparam name="T">The type of the collection.</typeparam>
		/// <param name="source">중복 항목을 검사하려는 컬렉션</param>
		/// <param name="distinct">Specify <b>true</b> to only return distinct elements.</param>
		/// <returns>source 컬렉션에서 중복된 항목을 모두 반환하려면 false를, 중복된 항목만 구하려면 true를 설정합니다.</returns>
		/// <exception cref="System.ArgumentNullException">source</exception>
		public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> source, bool distinct = true)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			IEnumerable<T> result = source
				.GroupBy(a => a)                // 일치하는 아이템별로 Group
				.Where(g => g.Skip(1).Any())    // 1개 이상 포함된 항목 검색
				.SelectMany(g => g);            // 1개 이상인 항목을 재 확장
												// Where, Select를 한번에 => .SelectMany(x => x.Skip(1));

			if (distinct == true)
			{
				result = result.Distinct();
			}

			return result;
		}

		/// <summary>
		/// 중복되는 항목(Item1)과 그 항목의 인덱스 목록(Item2)를 반환합니다.
		/// <remarks>항목은 처음 발견되는 항목을 먼저 반환 합니다.</remarks>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns>항목(Name)과 IEnumerable&lt;T&gt; 인덱스 목록(Indices)를 속성으로 갖는 Turple을 반환합니다.</returns>
		/// <example>
		/// <code>
		/// var names = new[] { "a", "a", "c", "b", "a", "b" };
		/// var dups = names.DuplicatesWithIndices();
		/// 
		/// foreach (var g in dups)
		/// {
		/// 	Console.WriteLine("Have duplicate " + g.Item1 + " with indices " + string.Join(",", g.Item2.ToArray()));
		/// }
		/// </code>
		/// </example>
		public static IEnumerable<Tuple<T, IEnumerable<int>>> DuplicatesWithIndices<T>(this IEnumerable<T> source)
		{
			return from x in source.Select((Name, Index) => new
			{
				Name,
				Index
			})
				   group x by x.Name into g
				   let r = Tuple.Create(g.Key, g.Select(gx => gx.Index))
				   where r.Item2.Count() > 1
				   select r;
			//return source.Select((Name, Index) => new
			//		{
			//			Name,
			//			Index
			//		})
			//		.GroupBy(x => x.Name)
			//		.Select(xg => new
			//		{
			//			Name = xg.Key,
			//			Indices = xg.Select(x => x.Index)
			//		})
			//		.Where(x => x.Indices.Count() > 1)
			//		.Select(x => new
			//		{
			//			Name = x.Name,
			//			Indices = x.Indices,
			//		});
		}
		#endregion

		#region MissingElements<TSource, TResult>
		/// <summary>
		/// source 항목을 오름차순 정렬하여 빠져있는 요소를 구한 뒤 selector로 변환하여 반환 합니다.
		/// </summary>
		/// <typeparam name="TSource">source 요소의 형식입니다.</typeparam>
		/// <typeparam name="TResult">selector에서 반환하는 값의 형식입니다.</typeparam>
		/// <param name="source">빠져있는 값을 구할 컬렉션 입니다.</param>
		/// <param name="integerParser">각 요소를 int 형으로 적용할 변환 함수입니다.</param>
		/// <param name="selector">빠진 항목을 int와 조합해서 각 요소에 적용할 변환 함수입니다.</param>
		/// <returns>해당 요소가 source의 각 요소에 대해 변형 함수를 호출한 뒤 selector로 결과를 구한 <seealso cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;" /> 입니다.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// source
		/// 또는
		/// integerParser
		/// 또는
		/// selector
		/// </exception>
		/// <example>
		/// <code>
		/// var list = new List&lt;string&gt;
		/// {
		/// 	"11019-8611-7423",
		/// 	"11019-8611-7416",
		/// 	"11019-8611-7421",
		/// 	"11019-8611-7418",
		/// 	"11019-8611-7415",
		/// };
		/// var missing = list.MissingElements(
		/// 					x =&gt; int.Parse(x.Split('-')[2]),
		/// 					m =&gt; string.Format("11019-8611-{0}", m)).ToList();
		/// 
		/// Assert.AreEqual("11019-8611-7417", missing[0]);
		/// Assert.AreEqual("11019-8611-7419", missing[1]);
		/// Assert.AreEqual("11019-8611-7420", missing[2]);
		/// Assert.AreEqual("11019-8611-7422", missing[3]);
		/// </code>
		/// </example>
		public static IEnumerable<TResult> MissingElements<TSource, TResult>(
			this IEnumerable<TSource> source,
			Func<TSource, int> integerParser,
			Func<int, TResult> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			if (integerParser == null)
			{
				throw new ArgumentNullException("integerParser");
			}

			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}

			var query = source
				.Select(integerParser)
				.OrderBy(x => x)
				.ToArray();
			var queryLength = query.Length;
			var min = 0;
			var max = 0;

			if (queryLength > 0)
			{
				min = query[0];
				max = query[queryLength - 1];
			}

			var result = Enumerable.Range(min, max - min).Except(query);

			return result.Select(selector);
		}
		#endregion

		#region GetInsertUpdate<T, TKey>
		/// <summary>
		/// inner 컬렉션과 outer 컬렉션을 비교하여 같은 항목은 updateCollection에 outer에 없는 항목은 insertCollection에 저장합니다.
		/// </summary>
		/// <typeparam name="T">null이 가능한 구조체 타입입니다.</typeparam>
		/// <param name="inner">기준이 되는 컬렉션입니다.</param>
		/// <param name="outer">비교하려는 컬렉션입니다.</param>
		/// <param name="insertCollection">추가할 항목을 반환합니다.</param>
		/// <param name="updateCollection">수정할 항목을 반환합니다.</param>
		public static void GetInsertUpdate<T>(
			this IEnumerable<T> inner,
			IEnumerable<T> outer,
			out IEnumerable<T> insertCollection,
			out IEnumerable<T> updateCollection)
			where T : struct
		{
			insertCollection = outer.Minus(inner);
			updateCollection = inner.Intersect(outer);
		}

		/// <summary>
		/// inner 컬렉션과 outer 컬렉션을 비교하여 같은 항목은 updateCollection에 outer에 없는 항목은 insertCollection에 저장합니다.
		/// </summary>
		/// <typeparam name="T">클래스 타입입니다.</typeparam>
		/// <typeparam name="TKey">키가 되는 타입입니다.</typeparam>
		/// <param name="inner">기준이 되는 컬렉션입니다.</param>
		/// <param name="outer">비교하려는 컬렉션입니다.</param>
		/// <param name="compareKeyExpression">T 타입에서 비교하려는 key의 표현식입니다.</param>
		/// <param name="insertCollection">추가할 항목을 반환합니다.</param>
		/// <param name="updateCollection">수정할 항목을 반환합니다.</param>
		public static void GetInsertUpdate<T, TKey>(
			this IEnumerable<T> inner,
			IEnumerable<T> outer,
			Expression<Func<T, TKey>> compareKeyExpression,
			out IEnumerable<T> insertCollection,
			out IEnumerable<T> updateCollection)
			where T : class
		{
			insertCollection = outer.Minus(inner, compareKeyExpression);
			//updateCollection = inner.AsQueryable().
			//	Join(outer.AsQueryable(), compareKeyExpression, compareKeyExpression, (x, y) => x);
			var compiledCondition = compareKeyExpression.Compile();

			updateCollection = from i in inner
							   join o in outer on compiledCondition(i) equals compiledCondition(o)
							   select o;
		}

		/// <summary>
		/// inner 컬렉션과 outer 컬렉션을 비교하여 같은 항목은 updateCollection에 outer에 없는 항목은 insertCollection에 저장합니다.
		/// </summary>
		/// <param name="inner">기준이 되는 문자열 컬렉션입니다.</param>
		/// <param name="outer">비교하려는 문자열 컬렉션입니다.</param>
		/// <param name="insertCollection">추가할 문자열 항목을 반환합니다.</param>
		/// <param name="updateCollection">수정할 문자열 항목을 반환합니다.</param>
		public static void GetInsertUpdate(
			this IEnumerable<string> inner,
			IEnumerable<string> outer,
			out IEnumerable<string> insertCollection,
			out IEnumerable<string> updateCollection)
		{
			insertCollection = outer.Minus(inner);
			updateCollection = inner.Intersect(outer);
		}
		#endregion

		#region MergeDelimiter<T>
		/// <summary>
		/// delimiter를 이용하여 merge(<see cref="System.Linq.Enumerable.Aggregate&lt;TSource, TAccumulate, TResult&gt;(IEnumerable&lt;TSource&gt;, TAccumulate, Func&lt;TAccumulate, TSource, TAccumulate&gt;, Func&lt;TAccumulate, TResult&gt;)"/>) 시킨 뒤 T 타입으로 반환합니다.
		/// </summary>
		/// <typeparam name="T">string으로 변환이 가능한 타입만 가능합니다.</typeparam>
		/// <param name="source">merge 시킬 컬렉션입니다.</param>
		/// <param name="delimiter">기본 구분자는 (:) 입니다.</param>
		/// <returns></returns>
		public static string MergeDelimiter<T>(this IEnumerable<T> source, string delimiter = ":")
		{
			return source.Aggregate<T, string>(
					string.Empty,
					(acc, next) =>
					{
						string nextValue = next.ToString();

						if (acc == string.Empty)
						{
							return nextValue;
						}

						return string.Format("{0}{1}{2}", acc, delimiter, nextValue);
					});
		}
		#endregion

		#region ForEach<T>
		/// <summary>
		/// source의 각 요소에서 지정한 동작을 수행합니다.
		/// </summary>
		/// <typeparam name="T">요소의 형식입니다.</typeparam>
		/// <param name="source">해당 요소에서 동작이 수행되는 컬렉션</param>
		/// <param name="action">각 요소에서 수행할 <seealso cref="System.Action&lt;T&gt;"/>입니다.</param>
		/// <exception cref="System.ArgumentNullException">source나 action이 null일 경우</exception>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null || action == null)
			{
				throw new ArgumentNullException("source");
			}

			foreach (T item in source)
			{
				action(item);
			}
		}
		#endregion

		#region SkipArray<T>
		/// <summary>
		/// 배열에서 지정된 수의 요소를 건너뛴 다음 나머지 요소를 반환합니다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">요소를 반환할 배열 소스</param>
		/// <param name="count">나머지 요소를 반환하기 전에 건너뛸 요소 수</param>
		/// <param name="length">The length.</param>
		/// <returns>``0[][].</returns>
		public static T[] SkipArray<T>(this T[] source, int count, int length)
		{
			T[] result = new T[length];

			Array.Copy(source, count, result, 0, length);

			return result;
		}
		#endregion
	}
}
