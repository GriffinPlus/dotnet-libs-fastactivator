///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://griffin.plus)
//
// Copyright 2018 Sascha Falk <sascha@falk-online.eu>
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance
// with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed
// on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for
// the specific language governing permissions and limitations under the License.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace GriffinPlus.Lib
{
	internal interface IFastActivatorFuncTypeMap
	{
		/// <summary>
		/// Trys to get the creator type corresponding to the specified constructor parameter types.
		/// </summary>
		/// <param name="constructorParameterTypes">Constructor parameter types to get the corresponding creator type for.</param>
		/// <param name="creatorFuncType">Receives the requested creator type.</param>
		/// <returns>true, if the creator type was found; otherwise false.</returns>
		bool TryGet(Type[] constructorParameterTypes, out Type creatorFuncType);

		/// <summary>
		/// Gets or creates the creator function type for the specified constructor parameter types.
		/// </summary>
		/// <param name="constructorParameterTypes">Constructor parameter types to get/create the corresponding creator type for.</param>
		/// <returns>The requested creator function type.</returns>
		Type Set(Type[] constructorParameterTypes);
	}

	/// <summary>
	/// Helper class mapping constructor parameter types to the corresponding function type that
	/// is used as a key when accessing creators.
	/// </summary>
	internal class FastActivatorFuncTypeMap<RESULT> : IFastActivatorFuncTypeMap
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
		/// Initializes a new instance of the <see cref="FastActivatorFuncTypeMap{RESULT}"/> class.
		/// </summary>
		public FastActivatorFuncTypeMap()
		{
				
		}

		/// <summary>
		/// Trys to get the creator type corresponding to the specified constructor parameter types.
		/// </summary>
		/// <param name="constructorParameterTypes">Constructor parameter types to get the corresponding creator type for.</param>
		/// <param name="creatorFuncType">Receives the requested creator type.</param>
		/// <returns>true, if the creator type was found; otherwise false.</returns>
		public bool TryGet(Type[] constructorParameterTypes, out Type creatorFuncType)
		{
			if (constructorParameterTypes.Length == 0) {
				creatorFuncType = typeof(Func<RESULT>);
				return true;
			}

			Node node = new Node();
			node.ConstructorParameterTypes = constructorParameterTypes;
			int index = mData.BinarySearch(node, sComparer);
			if (index >= 0)
			{
				creatorFuncType = mData[index].FuncType;
				return true;
			}
			else
			{
				creatorFuncType = null;
				return false;
			}
		}

		/// <summary>
		/// Gets or creates the creator function type for the specified constructor parameter types.
		/// </summary>
		/// <param name="constructorParameterTypes">Constructor parameter types to get/create the corresponding creator type for.</param>
		/// <returns>The requested creator function type.</returns>
		public Type Set(Type[] constructorParameterTypes)
		{
			if (constructorParameterTypes.Length == 0) {
				return typeof(Func<RESULT>);
			}

			Node node = new Node();
			node.ConstructorParameterTypes = constructorParameterTypes;
			int index = mData.BinarySearch(node, sComparer);
			if (index >= 0)
			{
				return mData[index].FuncType;
			}
			else
			{
				node = new Node();
				node.ConstructorParameterTypes = constructorParameterTypes;
				node.FuncType = FastActivator.MakeGenericCreatorFuncType(typeof(RESULT), constructorParameterTypes, constructorParameterTypes.Length);
				mData.Insert(~index, node);
				return node.FuncType;
			}
		}
	}
}
