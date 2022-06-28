///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

// ReSharper disable EmptyConstructor

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Helper class mapping constructor parameter types to the corresponding function type that
	/// is used as a key when accessing creators.
	/// </summary>
	class FastActivatorFuncTypeMap
	{
		private struct Node
		{
			public Type   FuncType;
			public Type[] ConstructorParameterTypes;
		}

		/// <summary>
		/// A comparer for the <see cref="Node"/> struct.
		/// </summary>
		private class NodeComparer : IComparer<Node>
		{
			public int Compare(Node x, Node y)
			{
				Type[] xcpts = x.ConstructorParameterTypes;
				Type[] ycpts = y.ConstructorParameterTypes;
				if (xcpts.Length < ycpts.Length) return -1;
				if (xcpts.Length > ycpts.Length) return 1;
				for (int i = 0; i < xcpts.Length; i++)
				{
					// sort ascendingly by hash code
					Type xcpt = xcpts[i];
					Type ycpt = ycpts[i];
					int xcpth = xcpt.GetHashCode();
					int ycpth = ycpt.GetHashCode();
					if (xcpth < ycpth) return -1;
					if (xcpth > ycpth) return 1;

					// the types should be equal at this point, but two types MAY have the same hash code
					// => check type name to be sure...
					if (xcpt != ycpt)
					{
						int result = StringComparer.Ordinal.Compare(xcpt.FullName, ycpt.FullName);
						if (result != 0) return result;
					}
				}

				return 0;
			}
		}

		private static readonly NodeComparer sComparer = new NodeComparer();
		private                 List<Node>   mData     = new List<Node>();

		/// <summary>
		/// Initializes a new instance of the <see cref="FastActivatorFuncTypeMap"/> class.
		/// </summary>
		public FastActivatorFuncTypeMap() { }

		/// <summary>
		/// Gets or creates the creator function type for the specified constructor parameter types.
		/// </summary>
		/// <param name="constructorParameterTypes">Constructor parameter types to get/create the corresponding creator type for.</param>
		/// <returns>The requested creator function type.</returns>
		public Type Get(Type[] constructorParameterTypes)
		{
			if (constructorParameterTypes.Length == 0)
				return typeof(Func<object>);

			// get snapshot of function type map (is replaced atomically when changed)
			while (true)
			{
				// get current version of the map
				var data = mData;
				Thread.MemoryBarrier();

				// query for the appropriate type; create new type, if not found
				Node node = new Node { ConstructorParameterTypes = constructorParameterTypes };
				int index = data.BinarySearch(node, sComparer);
				if (index >= 0)
				{
					return data[index].FuncType;
				}

				var newData = new List<Node>(data);
				node = new Node
				{
					ConstructorParameterTypes = constructorParameterTypes,
					FuncType = Expression.GetDelegateType(new List<Type>(constructorParameterTypes) { typeof(object) }.ToArray())
				};
				newData.Insert(~index, node);
				if (Interlocked.CompareExchange(ref mData, newData, data) == data)
					return node.FuncType;
			}
		}
	}

}
