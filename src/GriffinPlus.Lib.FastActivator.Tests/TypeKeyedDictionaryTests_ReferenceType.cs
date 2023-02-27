///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Unit tests targeting the <see cref="TypeKeyedDictionary{TValue}"/> class (for reference types).
	/// </summary>
	public class TypeKeyedDictionaryTests_ReferenceType : TypeKeyedDictionaryTests_Base<string>
	{
		/// <summary>
		/// Gets an instance of the dictionary to test, populated with the specified data.
		/// </summary>
		/// <param name="data">Data to populate the dictionary with.</param>
		/// <returns>A new instance of the dictionary to test, populated with the specified data.</returns>
		internal override TypeKeyedDictionary<string> GetDictionary(IDictionary<Type, string> data = null)
		{
			return data != null ? new TypeKeyedDictionary<string>(data) : new TypeKeyedDictionary<string>();
		}

		/// <summary>
		/// Gets a dictionary containing some test data.
		/// </summary>
		/// <param name="count">Number of entries in the dictionary.</param>
		/// <returns>A test data dictionary.</returns>
		protected override IDictionary<Type, string> GetTestData(int count)
		{
			// generate random test data
			var dict = new Dictionary<Type, string>(EqualityComparer<Type>.Default);
			Type[] types = GetTypes();
			for (int i = 0; i < count; i++)
			{
				Type key = types[i];
				dict[key] = types[i].AssemblyQualifiedName;
			}

			return dict;
		}

		/// <summary>
		/// Gets a value that is guaranteed to be not in the generated test data set.
		/// Must not be the default value of <see cref="System.String"/>.
		/// </summary>
		protected override string ValueNotInTestData => "xxx"; // not an assembly qualified type name
	}

}
