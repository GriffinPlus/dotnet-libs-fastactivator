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

namespace GriffinPlus.Lib
{

	partial class TypeKeyedDictionary<TValue>
	{
		/// <summary>
		/// An enumerator for the <see cref="TypeKeyedDictionary{TValue}"/> class.
		/// </summary>
		[Serializable]
		public struct Enumerator : IEnumerator<KeyValuePair<Type, TValue>>
		{
			private readonly int                         mGetEnumeratorReturnType; // determines what Enumerator.Current should return
			private          TypeKeyedDictionary<TValue> mDictionary;
			private          int                         mVersion;
			private          int                         mIndex;
			private          KeyValuePair<Type, TValue>  mCurrent;

			internal const int DictEntry    = 1;
			internal const int KeyValuePair = 2;

			/// <summary>
			/// Initializes a new instance of the <see cref="Enumerator"/> class.
			/// </summary>
			/// <param name="dictionary">Dictionary the enumerator belongs to.</param>
			/// <param name="getEnumeratorReturnType">Item to return when enumerating.</param>
			internal Enumerator(TypeKeyedDictionary<TValue> dictionary, int getEnumeratorReturnType)
			{
				mDictionary = dictionary;
				mVersion = dictionary.mVersion;
				mIndex = 0;
				mGetEnumeratorReturnType = getEnumeratorReturnType;
				mCurrent = default;
			}

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>
			/// <c>true</c>, if the enumerator was successfully advanced to the next element;
			/// <c>false</c>, if the enumerator has passed the end of the collection.
			/// </returns>
			/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
			public bool MoveNext()
			{
				if (mVersion != mDictionary.mVersion)
					throw new InvalidOperationException("The collection was modified after the enumerator was created.");

				// Use unsigned comparison since we set index to dictionary.count+1 when the enumeration ends.
				// dictionary.count+1 could be negative if dictionary.count is Int32.MaxValue
				while ((uint)mIndex < (uint)mDictionary.mCount)
				{
					ref var entry = ref mDictionary.mEntries[mIndex++];
					if (entry.Next >= -1)
					{
						mCurrent = new KeyValuePair<Type, TValue>(entry.Key, entry.Value);
						return true;
					}
				}

				mIndex = mDictionary.mCount + 1;
				mCurrent = default;
				return false;
			}

			/// <summary>
			/// Disposes the enumerator.
			/// </summary>
			public void Dispose() { }

			/// <summary>
			/// Gets the current value of the enumerator.
			/// </summary>
			public KeyValuePair<Type, TValue> Current => mCurrent;

			/// <summary>
			/// Gets the element in the collection at the current position of the enumerator.
			/// </summary>
			/// <value>The element in the collection at the current position of the enumerator.</value>
			object IEnumerator.Current
			{
				get
				{
					if (mGetEnumeratorReturnType == DictEntry) return new DictionaryEntry(mCurrent.Key, mCurrent.Value);
					return new KeyValuePair<Type, TValue>(mCurrent.Key, mCurrent.Value);
				}
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
			void IEnumerator.Reset()
			{
				if (mVersion != mDictionary.mVersion)
					throw new InvalidOperationException("The collection was modified after the enumerator was created.");

				mIndex = 0;
				mCurrent = new KeyValuePair<Type, TValue>();
			}
		}
	}

}
