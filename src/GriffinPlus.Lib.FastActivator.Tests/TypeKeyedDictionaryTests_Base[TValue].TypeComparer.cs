///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Unit tests targeting the <see cref="TypeKeyedDictionary{TValue}"/> class.
	/// </summary>
	abstract partial class TypeKeyedDictionaryTests_Base<TValue>
	{
		/// <summary>
		/// A comparer for <see cref="System.Type"/> taking only the assembly-qualified type name into account.
		/// </summary>
		private class TypeComparer : IComparer<Type>
		{
			/// <summary>
			/// An instance of the <see cref="TypeComparer"/> class.
			/// </summary>
			public static readonly TypeComparer Instance = new TypeComparer();

			/// <summary>
			/// Compares the assembly qualified name of the specified types with each other.
			/// </summary>
			/// <param name="x">First type to compare.</param>
			/// <param name="y">Second type to compare.</param>
			/// <returns>
			/// -1, if the assembly qualified name of <paramref name="x"/> is less than the assembly qualified name of <paramref name="y"/>.<br/>
			/// 1, if the assembly qualified name of <paramref name="x"/> is greater than the assembly qualified name of <paramref name="y"/>.<br/>
			/// 0, if the assembly qualified name of both types is the same.
			/// </returns>
			public int Compare(Type x, Type y)
			{
				if (x == null && y == null) return 0;
				if (x == null) return -1;
				return y != null ? StringComparer.Ordinal.Compare(x.AssemblyQualifiedName, y.AssemblyQualifiedName) : 1;
			}
		}
	}

}
