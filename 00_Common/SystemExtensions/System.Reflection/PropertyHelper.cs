using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GS.Common.SystemExtensions.System.Reflection
{
	/// <summary>
	/// Property 속성에 확장 기능을 제공합니다.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class PropertyHelper<T>
	{
		/// <summary>
		/// 문자열 기반이 아닌 lambda 기반으로 PropertyInfo 속성을 구합니다.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="selector">The selector.</param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static PropertyInfo GetProperty<TValue>(Expression<Func<T, TValue>> selector)
		{
			Expression body = selector;

			if (body is LambdaExpression)
			{
				body = ((LambdaExpression)body).Body;
			}

			switch (body.NodeType)
			{
				case ExpressionType.MemberAccess:

					return (PropertyInfo)((MemberExpression)body).Member;
				default:

					throw new InvalidOperationException();
			}
		}
	}
}
