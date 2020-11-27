///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading;

namespace GriffinPlus.Lib
{
	/// <summary>
	/// Helper class mapping constructor parameter types to the corresponding function type that
	/// is used as a key when accessing creators.
	/// </summary>
	internal class FastActivatorFuncTypeMap
	{
		private struct Node
		{
			public Type FuncType;
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
					if (xcpt != ycpt) {
						int result = xcpt.FullName.CompareTo(ycpt.FullName);
						if (result != 0) return result;
					}
				}

				return 0;
			}
		}

		private static NodeComparer sComparer = new NodeComparer();
		private List<Node> mData = new List<Node>();

		/// <summary>
		/// Initializes a new instance of the <see cref="FastActivatorFuncTypeMap"/> class.
		/// </summary>
		public FastActivatorFuncTypeMap()
		{

		}

		/// <summary>
		/// Gets or creates the creator function type for the specified constructor parameter types.
		/// </summary>
		/// <param name="constructorParameterTypes">Constructor parameter types to get/create the corresponding creator type for.</param>
		/// <returns>The requested creator function type.</returns>
		public Type Get(Type[] constructorParameterTypes)
		{
			if (constructorParameterTypes.Length == 0) {
				return typeof(Func<object>);
			}

			// get snapshot of function type map (is replaced atomically when changed)
			while (true)
			{
				// get current version of the map
				var data = mData;
				Thread.MemoryBarrier();

				// query for the appropriate type; create new type, if not found
				Node node = new Node();
				node.ConstructorParameterTypes = constructorParameterTypes;
				int index = data.BinarySearch(node, sComparer);
				if (index >= 0)
				{
					return data[index].FuncType;
				}
				else
				{
					var newData = new List<Node>(data);
					node = new Node();
					node.ConstructorParameterTypes = constructorParameterTypes;
					node.FuncType = MakeGenericCreatorFuncType(typeof(object), constructorParameterTypes, constructorParameterTypes.Length);
					newData.Insert(~index, node);
					if (Interlocked.CompareExchange(ref mData, newData, data) == data) {
						return node.FuncType;
					}
				}
			}
		}

		/// <summary>
		/// Creates a generic function type from the specified parameter types.
		/// </summary>
		/// <param name="returnType">Return type of the creator function.</param>
		/// <param name="parameterTypes">Parameter types to create the function type from.</param>
		/// <param name="count">Number of parameters to consider.</param>
		/// <returns>The generated function type.</returns>
		public static Type MakeGenericCreatorFuncType(Type returnType, Type[] parameterTypes, int count)
		{
			Type funcType = null;
			Type[] pt = parameterTypes;
			switch (count)
			{
				case 0:
					funcType = typeof(Func<>).MakeGenericType(returnType);
					break;
				case 1:
					funcType = typeof(Func<,>).MakeGenericType(pt[0], returnType);
					break;
				case 2:
					funcType = typeof(Func<,,>).MakeGenericType(pt[0], pt[1], returnType);
					break;
				case 3:
					funcType = typeof(Func<,,,>).MakeGenericType(pt[0], pt[1], pt[2], returnType);
					break;
				case 4:
					funcType = typeof(Func<,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], returnType);
					break;
				case 5:
					funcType = typeof(Func<,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], returnType);
					break;
				case 6:
					funcType = typeof(Func<,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], returnType);
					break;
				case 7:
					funcType = typeof(Func<,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], returnType);
					break;
				case 8:
					funcType = typeof(Func<,,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], pt[7], returnType);
					break;
				case 9:
					funcType = typeof(Func<,,,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], pt[7], pt[8], returnType);
					break;
				case 10:
					funcType = typeof(Func<,,,,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], pt[7], pt[8], pt[9], returnType);
					break;
				case 11:
					funcType = typeof(Func<,,,,,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], pt[7], pt[8], pt[9], pt[10], returnType);
					break;
				case 12:
					funcType = typeof(Func<,,,,,,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], pt[7], pt[8], pt[9], pt[10], pt[11], returnType);
					break;
				case 13:
					funcType = typeof(Func<,,,,,,,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], pt[7], pt[8], pt[9], pt[10], pt[11], pt[12], returnType);
					break;
				case 14:
					funcType = typeof(Func<,,,,,,,,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], pt[7], pt[8], pt[9], pt[10], pt[11], pt[12], pt[13], returnType);
					break;
				case 15:
					funcType = typeof(Func<,,,,,,,,,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], pt[7], pt[8], pt[9], pt[10], pt[11], pt[12], pt[13], pt[14], returnType);
					break;
				case 16:
					funcType = typeof(Func<,,,,,,,,,,,,,,,,>).MakeGenericType(pt[0], pt[1], pt[2], pt[3], pt[4], pt[5], pt[6], pt[7], pt[8], pt[9], pt[10], pt[11], pt[12], pt[13], pt[14], pt[15], returnType);
					break;
			}

			return funcType;
		}
	}
}
