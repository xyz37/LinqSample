using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace System
{
	/// <summary>
	/// 타입에 적용되는 확장 기능을 지원합니다.
	/// </summary>
	public static class TypeExtensions
	{
		#region Nullable
		/// <summary>
		/// Null이나 0일 경우 defaultValue로 설정합니다.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int NullOrZeroToValue(this int? value, int defaultValue)
		{
			int result = defaultValue;

			if (value == null || value == 0)
			{
				result = defaultValue;
			}

			return result;
		}

		/// <summary>
		/// Nullable type인지 검사합니다.
		/// </summary>
		/// <param name="checkedType">검사하려는 type</param>
		/// <returns></returns>
		public static bool IsNullableType(this Type checkedType)
		{
			return checkedType.IsGenericType
				&& checkedType.GetGenericTypeDefinition() == (typeof(Nullable<>));
		}

		/// <summary>
		/// <see cref="NullableConverter"/>를 이용하여 NullableType의 내부 형식을 가져옵니다.
		/// </summary>
		/// <param name="nullableType">Nullable type</param>
		/// <returns>Nullable type이면 내부 형식을 반환하고, 그렇지 않으면 null을 반환 합니다.</returns>
		public static Type GetNullableUnderlyingType<T>(this T? nullableType)
			where T : struct
		{
			return GetNullableUnderlyingType(nullableType.GetType());
		}

		/// <summary>
		/// <see cref="NullableConverter"/>를 이용하여 NullableType의 내부 형식을 가져옵니다.
		/// </summary>
		/// <param name="nullableType">Nullable type</param>
		/// <returns>Nullable type이면 내부 형식을 반환하고, 그렇지 않으면 null을 반환 합니다.</returns>
		public static Type GetNullableUnderlyingType(this Type nullableType)
		{
			if (nullableType != null && nullableType.IsNullableType() == true)
			{
				NullableConverter nullableConverter = new NullableConverter(nullableType);

				return nullableConverter.UnderlyingType;
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region IsDerivedBaseType
		/// <summary>
		/// Type이 지정된 base type에서 상속 받았는지를 <see cref="Type.FullName"/> 일부로 검사합니다.
		/// </summary>
		/// <param name="checkedType">검사하려는 type</param>
		/// <param name="partOfBaseTypeFullName">base type <see cref="Type.FullName"/>의 일부 단어</param>
		/// <returns>지정된 Type으로 부터 상속 파생되었으면 true를 반환합니다.</returns>
		/// <seealso cref="IsDerivedBaseType(Type, string, bool)"/>
		public static bool IsDerivedBaseType(this Type checkedType, string partOfBaseTypeFullName)
		{
			bool exactlyMatchFullName = false;

			return IsDerivedBaseType(checkedType, partOfBaseTypeFullName, exactlyMatchFullName);
		}

		/// <summary>
		/// Type이 지정된 base type에서 상속 받았는지를 검사합니다.
		/// </summary>
		/// <param name="checkedType">검사하려는 type</param>
		/// <param name="baseTypeFullName">base type의 <see cref="Type.FullName"/></param>
		/// <param name="exactlyMatchFullName">baseTypeFullName을 정확하게 일치하는 단어로 검사할지 여부</param>
		/// <returns>지정된 Type으로 부터 상속 파생되었으면 true를 반환합니다.</returns>
		/// <seealso cref="IsDerivedBaseType(Type, string)"/>
		public static bool IsDerivedBaseType(this Type checkedType, string baseTypeFullName, bool exactlyMatchFullName)
		{
			bool checkBaseTypeResult = false;

			if (checkedType == null || baseTypeFullName == string.Empty)
			{
				return checkBaseTypeResult;
			}

			Queue<Type> checkTypeQueue = new Queue<Type>();

			checkTypeQueue.Enqueue(checkedType.BaseType);

			while (checkTypeQueue.Count > 0)
			{
				Type baseType = checkTypeQueue.Dequeue();

				if (baseType == null)
				{
					continue;
				}

				if (exactlyMatchFullName == false)
				{
					if (baseType.FullName.Contains(baseTypeFullName) == true)
					{
						return true;
					}
				}
				else
				{
					if (baseType.FullName == baseTypeFullName)
					{
						return true;
					}
				}

				checkTypeQueue.Enqueue(baseType.BaseType);
			}

			return checkBaseTypeResult;
		}
		#endregion

		#region DefaultValue
		/// <summary>
		/// 지정한 타입의 기본값을 구합니다.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>지정한 타입의 기본값을 반환 합니다.</returns>
		public static dynamic DefaultValue(this Type type)
		{
			if (type == null)
			{
				return null;
			}

			if (type.IsValueType == true)
			{
				if (Nullable.GetUnderlyingType(type) != null)
				{
					return Activator.CreateInstance(Nullable.GetUnderlyingType(type));
				}
				else
				{
					return Activator.CreateInstance(type);
				}
			}
			else
			{
				return null;
			}
		}
		#endregion

		/// <summary>
		/// Invokes the property.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="ArgumentException">
		/// InvokeProperty: target of type Object must not be null.
		/// or
		/// InvokeProperty: propertyName of type String must not be null.
		/// or
		/// InvokeProperty: value of type object must not be null.
		/// </exception>
		public static void InvokeSetProperty(this object target, string propertyName, object value)
		{
			#region Check Parameters
			if (target == null)
			{
				throw new ArgumentException("InvokeProperty: target of type Object must not be null.");
			}
			if (propertyName == null)
			{
				throw new ArgumentException("InvokeProperty: propertyName of type String must not be null.");
			}
			if (value == null)
			{
				throw new ArgumentException("InvokeProperty: value of type object must not be null.");
			}
			#endregion

			var propertyDescriptor = TypeDescriptor.GetProperties(target).Cast<PropertyDescriptor>().FirstOrDefault(x => x.Name == propertyName);

			if (propertyDescriptor != null)
			{
				propertyDescriptor.SetValue(target, value);
			}
		}
	}
}
