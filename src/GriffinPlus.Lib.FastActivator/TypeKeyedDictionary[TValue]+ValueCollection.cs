///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
//
// This file incorporates work covered by the following copyright and permission notice:
//
//    The MIT License (MIT)
//
//    Copyright (c) Microsoft Corporation
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy 
//    of this software and associated documentation files (the "Software"), to deal 
//    in the Software without restriction, including without limitation the rights 
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
//    copies of the Software, and to permit persons to whom the Software is 
//    furnished to do so, subject to the following conditions: 
//
//    The above copyright notice and this permission notice shall be included in all 
//    copies or substantial portions of the Software. 
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
//    SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GriffinPlus.Lib
{

	partial class TypeKeyedDictionary<TValue>
	{
		/// <summary>
		/// A collection of values in a <see cref="TypeKeyedDictionary{TValue}"/>.
		/// </summary>
		[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
		[Serializable]
		public sealed partial class ValueCollection : ICollection<TValue>
		{
			private readonly TypeKeyedDictionary<TValue> mDictionary;

			/// <summary>
			/// Initializes a new instance of the <see cref="TypeKeyedDictionary{TValue}"/>.
			/// </summary>
			/// <param name="dictionary">The <see cref="TypeKeyedDictionary{TValue}"/> the collection belongs to.</param>
			/// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
			public ValueCollection(TypeKeyedDictionary<TValue> dictionary)
			{
				mDictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
			}

			/// <summary>
			/// Gets the number of elements contained in the collection.
			/// </summary>
			public int Count => mDictionary.Count;

			/// <summary>
			/// Returns an enumerator that iterates through the collection.
			/// </summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			public Enumerator GetEnumerator()
			{
				return new Enumerator(mDictionary);
			}

			/// <summary>
			/// Copies the elements of the collection to an array, starting at a particular array index.
			/// </summary>
			/// <param name="array">
			/// The one-dimensional array that is the destination of the elements copied from collection.
			/// The array must have zero-based indexing.
			/// </param>
			/// <param name="index">The zero-based index in array at which copying begins.</param>
			/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
			/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> exceeds the bounds of the array.</exception>
			/// <exception cref="ArgumentException">
			/// The number of elements in the source collection is greater than the available space from <paramref name="index"/>
			/// to the end of the destination array.
			/// </exception>
			public void CopyTo(TValue[] array, int index)
			{
				if (array == null)
					throw new ArgumentNullException(nameof(array));

				if (index < 0 || index > array.Length)
					throw new ArgumentOutOfRangeException(nameof(index), index, "The index exceeds the array bounds.");

				if (array.Length - index < mDictionary.Count)
					throw new ArgumentException("The destination array is too small.");

				int count = mDictionary.mCount;
				var entries = mDictionary.mEntries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].Next >= -1) array[index++] = entries[i].Value;
				}
			}

			#region Explicit Implementation of IEnumerable<T>

			/// <summary>
			/// Returns an enumerator that iterates through the collection.
			/// </summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return new Enumerator(mDictionary);
			}

			#endregion

			#region Explicit Implementation of IEnumerable

			/// <summary>
			/// Returns an enumerator that iterates through the collection.
			/// </summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(mDictionary);
			}

			#endregion

			#region Explicit Implementation of ICollection<T>

			/// <summary>
			/// Gets a value indicating whether the collection is read-only.
			/// </summary>
			/// <value>Always <c>true</c>.</value>
			bool ICollection<TValue>.IsReadOnly => true;

			/// <summary>
			/// Adds an item to the collection (not supported).
			/// </summary>
			/// <param name="item">The item to add to the collection.</param>
			/// <exception cref="NotSupportedException">The collection is read-only.</exception>
			void ICollection<TValue>.Add(TValue item)
			{
				throw new NotSupportedException("The collection is read-only.");
			}

			/// <summary>
			/// Removes the first occurrence of a specific object from the collection (not supported).
			/// </summary>
			/// <param name="item">The object to remove from the collection.</param>
			/// <returns>
			/// <c>true</c> if item was successfully removed from the collection; otherwise <c>false</c>.
			/// This method also returns false if item is not found in the original collection.
			/// </returns>
			/// <exception cref="NotSupportedException">The collection is read-only.</exception>
			bool ICollection<TValue>.Remove(TValue item)
			{
				throw new NotSupportedException("The collection is read-only.");
			}

			/// <summary>
			/// Removes all items from the collection (not supported).
			/// </summary>
			/// <exception cref="NotSupportedException">The collection is read-only.</exception>
			void ICollection<TValue>.Clear()
			{
				throw new NotSupportedException("The collection is read-only.");
			}

			/// <summary>
			/// Determines whether the collection contains a specific value.
			/// </summary>
			/// <param name="item">The item to locate in the collection.</param>
			/// <returns>
			/// <c>true</c> if item is found in the collection; otherwise <c>false</c>.
			/// </returns>
			bool ICollection<TValue>.Contains(TValue item)
			{
				return mDictionary.ContainsValue(item);
			}

			#endregion
		}
	}

}
