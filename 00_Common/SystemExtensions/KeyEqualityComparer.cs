using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
	/// <summary>
	/// Class KeyEqualityComparer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class KeyEqualityComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> _comparer;
		private readonly Func<T, object> _keyExtractor;

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyEqualityComparer{T}"/> class.
		/// </summary>
		/// <param name="keyExtractor">The key extractor.</param>
		public KeyEqualityComparer(Func<T, object> keyExtractor)
			: this(keyExtractor, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyEqualityComparer{T}"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public KeyEqualityComparer(Func<T, T, bool> comparer)
			: this(null, comparer)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyEqualityComparer{T}"/> class.
		/// </summary>
		/// <param name="keyExtractor">The key extractor.</param>
		/// <param name="comparer">The comparer.</param>
		public KeyEqualityComparer(Func<T, object> keyExtractor, Func<T, T, bool> comparer)
		{
			_keyExtractor = keyExtractor;
			_comparer = comparer;
		}

		/// <summary>
		/// 지정한 개체가 같은지 여부를 확인합니다.
		/// </summary>
		/// <param name="x">비교할 T 형식의 첫 번째 개체입니다.</param>
		/// <param name="y">비교할 T 형식의 두 번째 개체입니다.</param>
		/// <returns>지정한 개체가 같으면 true이고, 그렇지 않으면 false입니다.</returns>
		public bool Equals(T x, T y)
		{
			if (_comparer != null)
			{
				return _comparer(x, y);
			}
			else
			{
				var valX = _keyExtractor(x);

				if (valX is IEnumerable<object>) // The special case where we pass a list of keys
				{
					return ((IEnumerable<object>)valX).SequenceEqual((IEnumerable<object>)_keyExtractor(y));
				}

				return valX.Equals(_keyExtractor(y));
			}
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <param name="obj">해시 코드가 반환될 <see cref="T:System.Object" />입니다.</param>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public int GetHashCode(T obj)
		{
			if (_keyExtractor == null)
			{
				return obj.ToString().ToLower().GetHashCode();
			}
			else
			{
				var val = _keyExtractor(obj);

				if (val is IEnumerable<object>) // The special case where we pass a list of keys
				{
					return (int)((IEnumerable<object>)val).Aggregate((x, y) => x.GetHashCode() ^ y.GetHashCode());
				}

				return val.GetHashCode();
			}
		}
	}
}
