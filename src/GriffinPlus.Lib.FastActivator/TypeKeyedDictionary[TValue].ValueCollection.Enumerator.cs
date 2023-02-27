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

// ReSharper disable MemberHidesStaticFromOuterClass

namespace GriffinPlus.Lib
{

	partial class TypeKeyedDictionary<TValue>
	{
		partial class ValueCollection
		{
			/// <summary>
			/// An enumerator for the <see cref="TypeKeyedDictionary{TValue}.ValueCollection"/> class.
			/// </summary>
			[Serializable]
			public struct Enumerator : IEnumerator<TValue>
			{
				private TypeKeyedDictionary<TValue> mDictionary;
				private int                         mIndex;
				private int                         mVersion;

				/// <summary>
				/// Initializes a new instance of the <see cref="TypeKeyedDictionary{TValue}.ValueCollection.Enumerator"/> class.
				/// </summary>
				/// <param name="dictionary">Dictionary the enumerator belongs to.</param>
				internal Enumerator(TypeKeyedDictionary<TValue> dictionary)
				{
					mDictionary = dictionary;
					mVersion = dictionary.mVersion;
					mIndex = 0;
					Current = default;
				}

				/// <summary>
				/// Disposes the enumerator.
				/// </summary>
				public void Dispose() { }

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

					while ((uint)mIndex < (uint)mDictionary.mCount)
					{
						ref Entry entry = ref mDictionary.mEntries[mIndex++];
						if (entry.Next >= -1)
						{
							Current = entry.Value;
							return true;
						}
					}

					mIndex = mDictionary.mCount + 1;
					Current = default;
					return false;
				}

				/// <summary>
				/// Gets the current value of the enumerator.
				/// </summary>
				public TValue Current { get; private set; }

				/// <summary>
				/// Gets the element in the collection at the current position of the enumerator.
				/// </summary>
				/// <value>The element in the collection at the current position of the enumerator.</value>
				object IEnumerator.Current => Current;

				/// <summary>
				/// Sets the enumerator to its initial position, which is before the first element in the collection.
				/// </summary>
				/// <exception cref="InvalidOperationException">The collection was modified after the enumerator was created.</exception>
				void IEnumerator.Reset()
				{
					if (mVersion != mDictionary.mVersion)
						throw new InvalidOperationException("The collection was modified after the enumerator was created.");

					mIndex = 0;
					Current = default;
				}
			}
		}
	}

}
