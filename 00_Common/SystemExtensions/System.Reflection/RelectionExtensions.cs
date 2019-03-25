/**********************************************************************************************************************/
/*	Domain		:	System.Reflection.RelectionExtensions
/*	Creator		:	KIMKIWON\xyz37(김기원)
/*	Create		:	2011년 7월 12일 화요일 오후 9:17
/*	Purpose		:	Reflection 적용되는 확장 기능을 지원합니다.
/*--------------------------------------------------------------------------------------------------------------------*/
/*	Modifier	:	
/*	Update		:	
/*	Changes		:	
/*--------------------------------------------------------------------------------------------------------------------*/
/*	Comment		:	
/*--------------------------------------------------------------------------------------------------------------------*/
/*	Reviewer	:	Kim Ki Won
/*	Rev. Date	:	2011년 7월 12일 화요일 오후 9:21
/**********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Resources;

namespace System.Reflection
{
	/// <summary>
	/// Reflection 적용되는 확장 기능을 지원합니다.
	/// </summary>
	public static class RelectionExtensions
	{
		/// <summary>
		/// 이 멤버에 정의되어 있고 형식 또는 해당 형식의 사용자 지정 특성이 없는 경우 빈 배열로 식별되는 사용자 지정 특성의 배열을 반환합니다.
		/// </summary>
		/// <typeparam name="T">사용자 지정 특성을 지원하는 reflection 개체에 사용자 지정 특성 형식</typeparam>
		/// <param name="provider">사용자 지정 특성을 지원하는 reflection 개체에 사용자 지정 특성</param>
		/// <param name="inherit">true인 경우 상속된 사용자 지정 특성의 계층 구조 체인을 검색합니다.</param>
		/// <returns>사용자 지정 특성을 나타내는 개체 배열 또는 빈 배열을 반환합니다.</returns>
		/// <exception cref="System.TypeLoadException">사용자 지정 특성 형식을 로드할 수 없는 경우</exception>
		/// <exception cref="System.Reflection.AmbiguousMatchException">이 멤버에 정의된 attributeType 형식의 특성이 여러 개인 경우</exception>
		public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider provider, bool inherit = false)
			where T : Attribute
		{
			return provider.GetCustomAttributes(typeof(T), inherit).OfType<T>();
		}

		/// <summary>
		/// Gets the attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pi">The pi.</param>
		/// <returns>``0.</returns>
		public static T GetAttribute<T>(this PropertyInfo pi)
			where T : Attribute
		{
			object[] attributes = pi.GetCustomAttributes(typeof(T), true);

			if (attributes.Length == 0)
			{
				return null;
			}

			return attributes[0] as T;
		}

		#region Assembly
		/// <summary>
		/// 어셈블리에 대한 텍스트 설명을 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리의 텍스트 설명을 반환합니다.</returns>
		public static string GetAssemblyDescription(this Assembly assembly)
		{
			AssemblyDescriptionAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyDescriptionAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.Description;

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리 버전에 대해 특정 버전 번호를 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리 버전에 대해 특정 버전 번호를 반환합니다.</returns>
		public static string GetAssemblyVersion(this Assembly assembly)
		{
			AssemblyVersionAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyVersionAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
			{
				var version = assemblyGetCustomAttributes.Version;

				if (version == string.Empty)
					version = assembly.GetName().Version.ToString();

				return version;
			}

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리 파일 버전에 대해 특정 버전 번호를 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리 파일 버전에 대해 특정 버전 번호를 반환합니다.</returns>
		public static string GetAssemblyFileVersion(this Assembly assembly)
		{
			AssemblyFileVersionAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyFileVersionAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.Version;

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리에 대한 설명을 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리의 설명을 반환합니다.</returns>
		public static string GetAssemblyTitle(this Assembly assembly)
		{
			AssemblyTitleAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyTitleAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.Title;

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리의 빌드 구성(예: 정식 버전 또는 디버그 버전)을 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리의 빌드 구성(예: 정식 버전 또는 디버그 버전)을 반환합니다.</returns>
		public static string GetAssemblyConfiguration(this Assembly assembly)
		{
			AssemblyConfigurationAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyConfigurationAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.Configuration;

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리의 회사 이름에 대한 정보를 가져옵니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리의 회사 이름에 대한 정보를 반환합니다.</returns>
		public static string GetAssemblyCompany(this Assembly assembly)
		{
			AssemblyCompanyAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyCompanyAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.Company;

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리의 제품 이름에 대한 정보를 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리의 제품 이름에 대한 정보를 반환합니다.</returns>
		public static string GetAssemblyProduct(this Assembly assembly)
		{
			AssemblyProductAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyProductAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.Product;

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리의 저작권 정보를 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리의 저작권 정보를 반환합니다.</returns>
		public static string GetAssemblyCopyright(this Assembly assembly)
		{
			AssemblyCopyrightAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyCopyrightAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.Copyright;

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리의 상표 정보를 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리의 상표 정보를 반환합니다.</returns>
		public static string GetAssemblyTrademark(this Assembly assembly)
		{
			AssemblyTrademarkAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyTrademarkAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.Trademark;

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리의 특성 사용 어셈블리에서 지원하는 문화권을 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리의 특성 사용 어셈블리에서 지원하는 문화권을 반환합니다.</returns>
		public static string GetAssemblyCulture(this Assembly assembly)
		{
			AssemblyCultureAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<AssemblyCultureAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.Culture;

			return string.Empty;
		}

		/// <summary>
		/// 어셈블리의 주 어셈블리에 대한 기본 culture의 이름을 구합니다.
		/// </summary>
		/// <param name="assembly">구하려는 어셈블리</param>
		/// <returns>어셈블리의 주 어셈블리에 대한 기본 culture의 이름을 반환합니다.</returns>
		public static string GetAssemblyCultureName(this Assembly assembly)
		{
			NeutralResourcesLanguageAttribute assemblyGetCustomAttributes = assembly.GetCustomAttributes<NeutralResourcesLanguageAttribute>().FirstOrDefault();

			if (assemblyGetCustomAttributes != null)
				return assemblyGetCustomAttributes.CultureName;

			return string.Empty;
		}

		/// <summary>
		/// Gets the entry location.
		/// </summary>
		/// <returns></returns>
		public static string GetEntryLocation()
		{
			var entry = System.Reflection.Assembly.GetEntryAssembly();
			var location = string.Empty;

			if (entry != null)
			{
				location = entry.Location;
			}
			else
			{
				location = System.Reflection.Assembly.GetCallingAssembly().Location;
			}

			return location;
		}
		#endregion
	}
}
