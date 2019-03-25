using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net.Mail;

namespace System
{
	/// <summary>
	/// 문자열에 적용되는 확장 기능을 지원합니다.
	/// </summary>
	public static class StringExtensions
	{
		#region String
		/// <summary>
		/// delimiter를 이용하여 split 시킨 뒤 IEnumerable&lt;T&gt;를 구합니다.
		/// </summary>
		/// <param name="value">구별할 값입니다.</param>
		/// <param name="delimiter">기본 구분자는 (:) 입니다.</param>
		/// <returns></returns>
		public static IEnumerable<string> SplitDelimiter(this string value, string delimiter = ":")
		{
			return value.SplitDelimiter<string>(delimiter);
		}

		/// <summary>
		/// delimiter를 이용하여 split 시킨 뒤 IEnumerable&lt;T&gt;를 구합니다.
		/// </summary>
		/// <typeparam name="T">string, int, long, double, decimal, float 타입만 가능합니다.</typeparam>
		/// <param name="value">구별할 값입니다.</param>
		/// <param name="delimiter">기본 구분자는 (:) 입니다.</param>
		/// <param name="stringSplitOptions">The string split options.</param>
		/// <returns>IEnumerable{``0}.</returns>
		/// <exception cref="System.ArgumentException">T의 타입이 string, int, long 형이여야 합니다.</exception>
		public static IEnumerable<T> SplitDelimiter<T>(this string value, string delimiter = ":", StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
		{
			if (string.IsNullOrEmpty(value) == true)
			{
				return new T[0];
			}

			if (typeof(T) != typeof(int)
				&& typeof(T) != typeof(long)
				&& typeof(T) != typeof(double)
				&& typeof(T) != typeof(decimal)
				&& typeof(T) != typeof(float)
				&& typeof(T) != typeof(string))
			{
				throw new ArgumentException("T의 타입이 string, int, long 형이여야 합니다.");
			}

			string[] splitValues = value.Split(new string[] { delimiter }, stringSplitOptions);

			if (typeof(T) == typeof(string))
			{
				return splitValues.Cast<T>();
			}
			else if (typeof(T) == typeof(int))
			{
				List<int> result = new List<int>();
				int oneValue = 0;

				foreach (string stringValue in splitValues)
				{
					if (int.TryParse(stringValue, out oneValue) == true)
					{
						result.Add(oneValue);
					}
				}

				return result.Cast<T>();
			}
			else if (typeof(T) == typeof(long))
			{
				List<long> result = new List<long>();
				long oneValue = 0L;

				foreach (string stringValue in splitValues)
				{
					if (long.TryParse(stringValue, out oneValue) == true)
					{
						result.Add(oneValue);
					}
				}

				return result.Cast<T>();
			}
			else if (typeof(T) == typeof(double))
			{
				List<double> result = new List<double>();
				double oneValue = 0d;

				foreach (string stringValue in splitValues)
				{
					if (double.TryParse(stringValue, out oneValue) == true)
					{
						result.Add(oneValue);
					}
				}

				return result.Cast<T>();
			}
			else if (typeof(T) == typeof(decimal))
			{
				List<decimal> result = new List<decimal>();
				decimal oneValue = decimal.Zero;

				foreach (string stringValue in splitValues)
				{
					if (decimal.TryParse(stringValue, out oneValue) == true)
					{
						result.Add(oneValue);
					}
				}

				return result.Cast<T>();
			}
			else if (typeof(T) == typeof(float))
			{
				List<float> result = new List<float>();
				float oneValue = 0f;

				foreach (string stringValue in splitValues)
				{
					if (float.TryParse(stringValue, out oneValue) == true)
					{
						result.Add(oneValue);
					}
				}

				return result.Cast<T>();
			}

			return new T[0];
		}

		/// <summary>
		/// delimiter를 이용하여 split 시킨 뒤 count 개수 만큼 합친 문자열을 구합니다.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="delimiter">The delimiter.</param>
		/// <param name="count">The count.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="System.InvalidOperationException">count는 1보다 커야 합니다.</exception>
		public static string Merge(this string value, string delimiter = ":", int count = 1)
		{
			if (count == 0)
			{
				throw new InvalidOperationException("count는 1보다 커야 합니다.");
			}

			return value.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
				.Take(count)
				.Aggregate((acc, next) => string.Format("{0}{1}{2}", acc, delimiter, next));
		}

		/// <summary>
		/// 지정된 문자열이 null 이거나 System.String.Empty 문자열인지 여부를 나타냅니다.
		/// </summary>
		/// <param name="value">테스트할 문자열입니다.</param>
		/// <returns>value 매개 변수가 null이거나 빈 문자열("")이면 true 이고, 그렇지 않으면 false입니다.</returns>
		public static bool IsNullOrEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		/// <summary>
		/// 지정된 문자열이 null이거나 비어 있거나 공백 문자로만 구성되어 있는지 여부를 나타냅니다.
		/// </summary>
		/// <param name="value">테스트할 문자열입니다.</param>
		/// <returns>value 매개 변수가 null 또는 System.String.Empty이거나, value가 모두 공백 문자로 구성되어 있으면 true입니다.</returns>
		public static bool IsNullOrWhiteSpace(this string value)
		{
			if (value == null)
			{
				return true;
			}

			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// 현재 인스턴스의 지정된 문자열이 지정된 다른 문자열로 첫번째만 바뀌는 새 문자열을 반환합니다.
		/// </summary>
		/// <param name="text">변경하려는 문자열 입니다.</param>
		/// <param name="oldValue">바꿀 문자열입니다.</param>
		/// <param name="newValue">oldValue를 바꿀 문자열입니다.</param>
		/// <returns>oldValue의 첫번째 인스턴스를 newValue로 바꾼다는 점을 제외하고 현재 문자열과 동일한 문자열입니다.</returns>
		/// <exception cref="System.ArgumentNullException">oldValue가 null인 경우</exception>
		/// <exception cref="System.ArgumentException">oldValue가 빈 문자열("")인 경우</exception>
		public static string ReplaceFirst(this string text, string oldValue, string newValue)
		{
			if (oldValue == null)
			{
				throw new System.ArgumentNullException(oldValue);
			}

			if (oldValue == string.Empty)
			{
				throw new System.ArgumentException(oldValue);
			}

			int pos = text.IndexOf(oldValue);

			if (pos < 0)
			{
				return text;
			}

			return string.Format("{0}{1}{2}", text.Substring(0, pos), newValue, text.Substring(pos + oldValue.Length));
		}

		/// <summary>
		/// oldText에 있는 문자들을 newText로 대체합니다.
		/// </summary>
		/// <param name="source">The source text.</param>
		/// <param name="newText">The old text.</param>
		/// <param name="oldText">The new text.</param>
		/// <returns>System.String.</returns>
		public static string ReplaceAll(this string source, string newText, params string[] oldText)
		{
			if (oldText == null || oldText.Length == 0)
			{
				return source;
			}

			foreach (string item in oldText)
			{
				if (item.Length == 0)
				{
					continue;
				}

				source = source.Replace(item, newText);
			}

			return source;
		}

		/// <summary>
		/// text 에서 공백을 모두 제거합니다
		/// </summary>
		/// <param name="text">공백을 제거하려는 문자열</param>
		/// <returns>모든 공백이 제거된 문자열</returns>
		/// <exception cref="System.ArgumentNullException">text</exception>
		public static string TrimAll(this string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			return text.ReplaceAll(string.Empty, " ");
		}

		/// <summary>
		/// 숫자의 문자열 표현을 해당하는 32비트 부호 있는 정수로 변환합니다.
		/// </summary>
		/// <param name="value">변환할 숫자가 들어 있는 문자열입니다.</param>
		/// <param name="defaultValue">변환에 실패 했을 때.</param>
		/// <returns>변환에 성공하면 32비트 부호 있는 정수를 반환하고, 그렇지 않으면 defaultValue를 반환합니다.</returns>
		public static int Parse(this string value, int defaultValue = int.MinValue)
		{
			int result = defaultValue;

			int.TryParse(value, out result);

			return result;
		}

		/// <summary>
		/// Checks the Email.
		/// </summary>
		/// <param name="emailAddress">The email address.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public static bool CheckEmail(this string emailAddress)
		{
			if (string.IsNullOrEmpty(emailAddress) == true)
			{
				return false;
			}

			try
			{
				MailAddress mailAddress = new MailAddress(emailAddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		/// <summary>
		/// Determines whether [contains] [the specified to check].
		/// </summary>
		/// <param name="source">The string source.</param>
		/// <param name="value">The string to seek.</param>
		/// <param name="stringComparison">The string comparison.</param>
		/// <returns><c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.</returns>
		public static bool Contains(this string source, string value, StringComparison stringComparison)
		{
			return source != null && value != null && source.IndexOf(value, stringComparison) >= 0;
		}
		#endregion

		#region StringBuilder
		/// <summary>
		/// 서식 항목이 0개 이상 들어 있는 복합 서식 문자열을 처리하여 반환된 문자열을 이 인스턴스에 추가합니다.<br/>
		/// 각 서식 항목이 매개 변수 배열에서 해당하는 인수의 문자열 표현으로 바뀝니다.<br/>
		/// 현재 System.Text.StringBuilder 개체의 끝에 지정한 문자열의 복사본과 기본 줄 종결자를 차례로 추가합니다.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="format">복합 서식 문자열입니다.</param>
		/// <param name="args">서식을 지정할 개체의 배열입니다.</param>
		/// <returns>format이 추가된 이 인스턴스에 대한 참조입니다. format의 각 서식 항목이 해당하는 개체 인수의 문자열 표현으로 바뀝니다.</returns>
		/// <exception cref="System.ArgumentNullException">format 또는 args가 null인 경우</exception>
		/// <exception cref="System.FormatException">format이 잘못된 경우 -또는-형식 항목의 인덱스가 0보다 작거나 args 배열의 길이보다 크거나 같은 경우</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">확장된 문자열 길이는 System.Text.StringBuilder.MaxCapacity를 초과합니다.</exception>
		public static StringBuilder AppendFormatLine(this StringBuilder builder, string format, params object[] args)
		{
			return builder.AppendFormat(format + Environment.NewLine, args);
		}

		/// <summary>
		/// Clears the specified builder.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns>StringBuilder.</returns>
		public static StringBuilder Clear(this StringBuilder builder)
		{
			builder.Length = 0;

			return builder;
		}
		#endregion
	}
}
