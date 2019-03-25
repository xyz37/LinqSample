using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections;

namespace System.Collections.Generic
{
	/// <summary>
	/// IEnumerable&lt;T&gt;를 구현한 컬렉션과 <see cref="System.Data.Linq.Table&lt;T&gt;"/> 컬렉션을
	/// <see cref="System.Data.DataTable"/>로 변환하는 확장 메서드를 지원합니다.
	/// </summary>
	public static class DataTableExtensions
	{
		/// <summary>
		/// <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>를 사용하여 <see cref="System.Data.DataTable"/>를 만듭니다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sources">DataTable로 변환하려는 컬렉션</param>
		/// <param name="dataTableName">DataTable 이름</param>
		/// <returns>DataTable 이라는 이름을 가진 <see cref="System.Data.DataTable"/>을 반환합니다.</returns>
		public static DataTable ToDataTable<T>(this IEnumerable<T> sources, string dataTableName = "DataTable")
		{
			DataTable dtResult = new DataTable(dataTableName);

			if (sources == null)
			{
				return dtResult;
			}

			PropertyInfo[] propInfos = null;

			foreach (T source in sources)
			{
				// 최초 1 회만 테이블 생성을 위해 처리 한다.
				if (propInfos == null)
				{
					propInfos = source.GetType().GetProperties();

					if (string.IsNullOrEmpty(dataTableName) == true)
					{
						dtResult.TableName = source.GetType().Name;
					}

					foreach (PropertyInfo pi in propInfos)
					{
						Type columnType = pi.PropertyType;

						if (SkipPropertyInfoCondition(pi) == true)
						{
							continue;
						}

						if (columnType.IsGenericType
								&& columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
						{
							columnType = columnType.GetGenericArguments()[0];
						}

						if (columnType == typeof(DateTime))     // 날짜는 Long 형으로 변환
						{
							dtResult.Columns.Add(new DataColumn(pi.Name, typeof(long)));
						}
						else if (columnType == typeof(DateTime?))       // 날짜는 Long 형으로 변환
						{
							dtResult.Columns.Add(new DataColumn(pi.Name, typeof(long?)));
						}
						else if (columnType.IsEnum == true)
						{
							dtResult.Columns.Add(new DataColumn(pi.Name, typeof(int)));
						}
						else
						{
							dtResult.Columns.Add(new DataColumn(pi.Name, columnType));
						}
					}
				}

				DataRow dataRow = dtResult.NewRow();

				dataRow.BeginEdit();

				foreach (PropertyInfo pi in propInfos)
				{
					if (SkipPropertyInfoCondition(pi) == true
						|| pi.Name.Equals("UniqueKey"))
					{
						continue;
					}

					object piValue = pi.GetValue(source, null);

					dataRow[pi.Name] = piValue ?? pi.PropertyType.DefaultValue();
				}

				dataRow.EndEdit();
				dtResult.Rows.Add(dataRow);
			}

			return (dtResult);
		}

		/// <summary>
		///	연관 테이블의 컬렉션, Eumn이 아니고 Built-In type이 아니면 포함하지 않는다.
		/// </summary>
		/// <param name="pi">The pi.</param>
		/// <returns>true if XXXX, false otherwise.</returns>
		private static bool SkipPropertyInfoCondition(PropertyInfo pi)
		{
#if NET40 || NET35
			var attrs = pi.GetCustomAttributes(true);

			if (attrs.Length > 0
				&& attrs.Count(x => x.ToString() == "System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute") > 0)
			{
				return true;
			}

			return (pi.PropertyType.IsEnum == false && pi.PropertyType.Namespace != "System")
					|| pi.GetCustomAttributes(typeof(System.Data.Linq.Mapping.AssociationAttribute), false).Count() > 0;
#else
			return (pi.PropertyType.IsEnum == false && pi.PropertyType.Namespace != "System")
					|| pi.GetCustomAttributes(typeof(System.Data.Linq.Mapping.AssociationAttribute), false).Count() > 0
					|| pi.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute), false).Count() > 0;
#endif
		}

		/// <summary>
		/// Dates the time to string.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>System.String.</returns>
		public static string DateTimeToString(DateTime? dateTime)
		{
			if (dateTime.HasValue == true)
			{
				return DateTimeToString(dateTime.Value);
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Dates the time to string.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>System.String.</returns>
		public static string DateTimeToString(DateTime dateTime)
		{
			//return dt.ToString("yyyy-MM-dd") + " " + dt.Hour.ToString("00") + ":" + dt.Minute.ToString("00") + ":" + dt.Second.ToString("00");
			return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
		}

		/// <summary>
		/// <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>를 사용하여 <see cref="System.Data.DataTable"/>이 하나 있는 <see cref="System.Data.DataSet"/>을 만듭니다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sources">DataTable로 변환하려는 컬렉션</param>
		/// <param name="dataSetName">DataSet 이름</param>
		/// <param name="dataTableName">DataTable 이름</param>
		/// <returns>DataTable 이라는 이름을 가진 <see cref="System.Data.DataTable"/>을 포함하는 <see cref="System.Data.DataSet"/>을 반환합니다.</returns>
		public static DataSet ToDataSet<T>(this IEnumerable<T> sources, string dataSetName = "DataSet", string dataTableName = "DataTable")
		{
			DataSet result = new DataSet(dataSetName);

			result.Tables.Add(ToDataTable<T>(sources, dataTableName));

			return result;
		}

		/// <summary>
		/// <see cref="System.Data.Linq.Table&lt;T&gt;"/>를 사용하여 <see cref="System.Data.DataTable"/>를 만듭니다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sources">DataTable로 변환하려는 컬렉션</param>
		/// <param name="dataTableName">DataTable 이름</param>
		/// <returns>DataTable 이라는 이름을 가진 <see cref="System.Data.DataTable"/>을 반환합니다.</returns>
		/// <remarks>성능 향상을 위해 많은 양의 데이터를 static의 DynamicMethod로 생성 합니다.</remarks>
		public static DataTable ToDataTable<T>(this Table<T> sources, string dataTableName = "DataTable")
			where T : class
		{
			DataTable resultTable = LinqToDataTableHelper<T>.ToDataTable(sources);

			resultTable.TableName = dataTableName;

			return resultTable;
		}

		/// <summary>
		/// <see cref="System.Data.Linq.Table&lt;T&gt;"/>를 사용하여 <see cref="System.Data.DataTable"/>이 하나 있는 <see cref="System.Data.DataSet"/>을 만듭니다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sources">DataTable로 변환하려는 컬렉션</param>
		/// <param name="dataSetName">DataSet 이름</param>
		/// <param name="dataTableName">DataTable 이름</param>
		/// <returns>DataTable 이라는 이름을 가진 <see cref="System.Data.DataTable"/>을 포함하는 <see cref="System.Data.DataSet"/>을 반환합니다.</returns>
		/// <remarks>성능 향상을 위해 많은 양의 데이터를 static의 DynamicMethod로 생성 합니다.</remarks>
		public static DataSet ToDataSet<T>(this Table<T> sources, string dataSetName = "DataSet", string dataTableName = "DataTable")
			where T : class
		{
			DataSet result = new DataSet(dataSetName);

			result.Tables.Add(ToDataTable<T>(sources, dataTableName));

			return result;
		}
	}
}
