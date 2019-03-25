using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	/// <summary>
	/// Enum에 적용되는 확장 기능을 지원합니다.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// T type의 Enum을 반복하며 action을 처리 합니다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enum">The enum.</param>
		/// <param name="action">The action.</param>
		/// <exception cref="System.ArgumentException">T 형식은 enum 이어야만 합니다.</exception>
		public static void ForEach<T>(this Enum @enum, Action<T> action)
		{
			if (typeof(T).IsEnum == false)
			{
				throw new ArgumentException("T 형식은 enum 이어야만 합니다.");
			}

			foreach (T t in (T[])Enum.GetValues(typeof(T)))
			{
				action(t);
			}
		}

		/// <summary>
		/// Gets the custom attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enum">The enum.</param>
		/// <returns>not assigned custom attribute default&lt;T&gt;</returns>
		/// <exception cref="ArgumentException">enumValue must be of Enum type - @enum</exception>
		public static T GetCustomAttribute<T>(this Enum @enum)
		{
			Type type = @enum.GetType();

			if (type.IsEnum == false)
			{
				throw new ArgumentException("enumValue must be of Enum type", "@enum");
			}

			var mis = type.GetMember(@enum.ToString());

			if (mis != null && mis.Length > 0)
			{
				object[] attrs = mis[0].GetCustomAttributes(typeof(T), false);

				if (attrs != null && attrs.Length > 0)
				{
					return ((T)attrs[0]);
				}
			}

			return default(T);
		}

		/// <summary>
		/// Gets the description of enum value that attached <see cref="System.ComponentModel.DescriptionAttribute" />.
		/// </summary>
		/// <param name="enum">The enum.</param>
		/// <returns>enum string if not assigned.</returns>
		/// <exception cref="System.ArgumentException">enumValue must be of Enum type;@enum</exception>
		public static string GetDescription(this Enum @enum)
		{
			var desc = @enum.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();

			if (desc == null)
			{
				return @enum.ToString();
			}
			else
			{
				return desc.Description;
			}
		}
	}
}
