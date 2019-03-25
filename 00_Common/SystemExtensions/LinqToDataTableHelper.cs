using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Collections.Generic
{
	/// <summary>
	/// 성능 향상을 위해 많은 양의 데이터를 static의 Dynamic Method로 생성하여 DataTable로 변환하는 기능을 제공합니다.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class LinqToDataTableHelper<T>
		where T : class
	{
		private static Func<Table<T>, DataTable> _generatedMethodDelegate;

		/// <summary>
		/// System.Data.Linq.Table&lt;T&gt;를 사용하여 <see cref="System.Data.DataTable"/>를 만듭니다.
		/// </summary>
		/// <param name="linqTable"></param>
		/// <returns></returns>
		public static DataTable ToDataTable(Table<T> linqTable)
		{
			if (_generatedMethodDelegate == null)
			{
				_generatedMethodDelegate = GenerateTableConverter().CreateDelegate(typeof(Func<Table<T>, DataTable>)) as Func<Table<T>, DataTable>;
			}

			return _generatedMethodDelegate(linqTable);
		}

		/// <summary>
		/// 테이블을 동적 method로 생성합니다.
		/// </summary>
		/// <returns></returns>
		public static DynamicMethod GenerateTableConverter()
		{
			var dynamicMethod = new DynamicMethod("Convert", typeof(DataTable), new Type[] { typeof(Table<T>) });
			var convertIlGenerator = dynamicMethod.GetILGenerator();

			convertIlGenerator.DeclareLocal(typeof(DataTable));
			convertIlGenerator.DeclareLocal(typeof(IEnumerator<T>));
			convertIlGenerator.DeclareLocal(typeof(T));
			convertIlGenerator.DeclareLocal(typeof(DataRow));

			var propertyValue = convertIlGenerator.DeclareLocal(typeof(object));

			convertIlGenerator.Emit(OpCodes.Ldarg_0);

			var convertIlGeneratorArgOkLabel = convertIlGenerator.DefineLabel();

			convertIlGenerator.Emit(OpCodes.Brtrue_S, convertIlGeneratorArgOkLabel);
			convertIlGenerator.Emit(OpCodes.Newobj, typeof(ArgumentException).GetConstructor(new Type[0]));
			convertIlGenerator.Emit(OpCodes.Throw);
			convertIlGenerator.MarkLabel(convertIlGeneratorArgOkLabel);

			convertIlGenerator.Emit(OpCodes.Newobj, typeof(DataTable).GetConstructor(new Type[0]));
			convertIlGenerator.Emit(OpCodes.Stloc_0);

			var properties = typeof(T).GetProperties().Where(p => p.GetAttribute<ColumnAttribute>() != null);

			foreach (var pi in properties)
			{
				convertIlGenerator.Emit(OpCodes.Ldloc_0);
				convertIlGenerator.Emit(OpCodes.Callvirt, typeof(DataTable).GetMethod("get_Columns"));
				convertIlGenerator.Emit(OpCodes.Ldstr, pi.Name);

				if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					convertIlGenerator.Emit(OpCodes.Ldtoken, pi.PropertyType.GetGenericArguments()[0]);
				}
				else
				{
					convertIlGenerator.Emit(OpCodes.Ldtoken, pi.PropertyType);
				}

				convertIlGenerator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }));
				convertIlGenerator.Emit(OpCodes.Callvirt, typeof(DataColumnCollection).GetMethod("Add", new Type[] { typeof(string), typeof(Type) }));
				convertIlGenerator.Emit(OpCodes.Pop);
			}

			var convertIlGeneratorEntitiesLoopLabel = convertIlGenerator.DefineLabel();

			convertIlGenerator.Emit(OpCodes.Ldarg_0);
			convertIlGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerable<T>).GetMethod("GetEnumerator"));
			convertIlGenerator.Emit(OpCodes.Stloc_1);
			convertIlGenerator.MarkLabel(convertIlGeneratorEntitiesLoopLabel);
			convertIlGenerator.Emit(OpCodes.Ldloc_1);
			convertIlGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"));

			var convertIlGeneratorEntitiesEndLoopLable = convertIlGenerator.DefineLabel();

			convertIlGenerator.Emit(OpCodes.Brfalse, convertIlGeneratorEntitiesEndLoopLable);
			convertIlGenerator.Emit(OpCodes.Ldloc_1);
			convertIlGenerator.Emit(OpCodes.Callvirt, typeof(IEnumerator<T>).GetMethod("get_Current"));
			convertIlGenerator.Emit(OpCodes.Stloc_2);
			convertIlGenerator.Emit(OpCodes.Ldloc_0);
			convertIlGenerator.Emit(OpCodes.Callvirt, typeof(DataTable).GetMethod("get_Rows"));
			convertIlGenerator.Emit(OpCodes.Ldloc_0);
			convertIlGenerator.Emit(OpCodes.Callvirt, typeof(DataTable).GetMethod("NewRow"));
			convertIlGenerator.Emit(OpCodes.Stloc_3);

			foreach (var pi in properties)
			{
				convertIlGenerator.Emit(OpCodes.Ldloc_2);
				convertIlGenerator.Emit(OpCodes.Callvirt, typeof(T).GetMethod("get_" + pi.Name, new Type[0]));

				if (pi.PropertyType.IsValueType)
				{
					convertIlGenerator.Emit(OpCodes.Box, pi.PropertyType);
				}

				convertIlGenerator.Emit(OpCodes.Stloc, propertyValue);
				convertIlGenerator.Emit(OpCodes.Ldloc, propertyValue);

				var convertIlGeneratorNextPropertyLabel = convertIlGenerator.DefineLabel();

				convertIlGenerator.Emit(OpCodes.Brfalse_S, convertIlGeneratorNextPropertyLabel);
				convertIlGenerator.Emit(OpCodes.Ldloc_3);
				convertIlGenerator.Emit(OpCodes.Ldstr, pi.Name);
				convertIlGenerator.Emit(OpCodes.Ldloc, propertyValue);
				convertIlGenerator.Emit(OpCodes.Callvirt, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
				convertIlGenerator.MarkLabel(convertIlGeneratorNextPropertyLabel);
			}

			convertIlGenerator.Emit(OpCodes.Ldloc_3);
			convertIlGenerator.Emit(OpCodes.Callvirt, typeof(DataRowCollection).GetMethod("Add", new Type[] { typeof(DataRow) }));
			convertIlGenerator.Emit(OpCodes.Br, convertIlGeneratorEntitiesLoopLabel);
			convertIlGenerator.MarkLabel(convertIlGeneratorEntitiesEndLoopLable);
			convertIlGenerator.Emit(OpCodes.Ldloc_0);
			convertIlGenerator.Emit(OpCodes.Ret);

			return dynamicMethod;
		}
	}
}

/*
namespace System.Reflection
{
	/// <summary>
	/// 
	/// </summary>
	public static class PropertyInfoExtension
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pi"></param>
		/// <returns></returns>
		public static T GetAttribute<T>(this PropertyInfo pi) where T : Attribute
		{
			object[] attributes = pi.GetCustomAttributes(typeof(T), true);

			if (attributes.Length == 0)
				return null;

			return attributes[0] as T;
		}
	}
}

*/
