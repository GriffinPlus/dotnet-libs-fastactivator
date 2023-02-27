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

// Some source for the implementation of this specialized dictionary was taken from the original implementation of the
// System.Collections.Generic.Dictionary<TKey,TValue> class:
// https://github.com/dotnet/runtime/blob/bbcb6b7707e55056a59b704f27af2db0c740da86/src/libraries/System.Private.CoreLib/src/System/Collections/Generic/Dictionary.cs

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// ReSharper disable InvertIf
// ReSharper disable ConvertToAutoProperty

namespace GriffinPlus.Lib
{

	/// <summary>
	/// A generic dictionary using <see cref="System.Type"/> as key.
	/// </summary>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	sealed partial class TypeKeyedDictionary<TValue> : IDictionary<Type, TValue>
	{
		private struct Entry
		{
			/// <summary>
			/// The hash code of the key of the entry.
			/// </summary>
			public uint HashCode;

			/// <summary>
			/// 0-based index of next entry in chain: -1 means end of chain
			/// also encodes whether this entry _itself_ is part of the free list by changing sign and subtracting 3,
			/// so -2 means end of free list, -3 means index 0 but on free list, -4 means index 1 but on free list, etc.
			/// </summary>
			public int Next;

			/// <summary>
			/// Key of the entry.
			/// </summary>
			public Type Key;

			/// <summary>
			/// Value of the entry.
			/// </summary>
			public TValue Value;
		}

		private int[]           mBuckets;
		private Entry[]         mEntries;
		private int             mCount;
		private int             mVersion;
		private int             mFreeList;
		private int             mFreeCount;
		private KeyCollection   mKeys;
		private ValueCollection mValues;

		private const int StartOfFreeList = -3;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeKeyedDictionary{TValue}"/> class that is empty
		/// and has the default initial capacity.
		/// </summary>
		public TypeKeyedDictionary() : this(0) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeKeyedDictionary{TValue}"/> class that is empty and
		/// has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">The initial number of elements that the dictionary can contain.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
		public TypeKeyedDictionary(int capacity)
		{
			if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "The capacity must be positive.");
			if (capacity > 0) Initialize(capacity);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeKeyedDictionary{TValue}"/> class that contains elements copied
		/// from the specified dictionary.
		/// </summary>
		/// <param name="dictionary">The dictionary whose elements are copied to the new dictionary.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="dictionary"/> contains one or more duplicate keys.</exception>
		public TypeKeyedDictionary(IDictionary<Type, TValue> dictionary) :
			this(dictionary?.Count ?? 0)
		{
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary), "The specified dictionary must not be null.");

			foreach (KeyValuePair<Type, TValue> pair in dictionary)
			{
				Add(pair.Key, pair.Value);
			}
		}

		/// <summary>
		/// Initializes the dictionary with the specified capacity.
		/// </summary>
		/// <param name="capacity">Requested capacity of the dictionary.</param>
		private void Initialize(int capacity)
		{
			int size = HashHelpers.GetPrime(capacity);
			int[] buckets = new int[size];
			var entries = new Entry[size];

			// assign member variables after both arrays allocated to guard against corruption from OOM if second fails
			Capacity = size;
			mFreeList = -1;
			mBuckets = buckets;
			mEntries = entries;
		}

		/// <summary>
		/// Gets the capacity of the dictionary.
		/// </summary>
		public int Capacity { get; private set; }

		/// <summary>
		/// Gets the number of elements in the dictionary.
		/// </summary>
		public int Count => mCount - mFreeCount;

		/// <summary>
		/// Gets a collection containing the keys in the dictionary.
		/// </summary>
		public KeyCollection Keys => mKeys ?? (mKeys = new KeyCollection(this));

		/// <summary>
		/// Gets a collection containing the values in the dictionary.
		/// </summary>
		public ValueCollection Values => mValues ?? (mValues = new ValueCollection(this));

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get or set.</param>
		/// <returns>
		/// The value associated with the specified key.
		/// If the specified key is not found, a get operation throws a <see cref="KeyNotFoundException"/>,
		/// and a set operation creates a new element with the specified key.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
		/// <exception cref="KeyNotFoundException">The property is retrieved and key does not exist in the collection.</exception>
		public TValue this[Type key]
		{
			get
			{
				if (TryGetValue(key, out TValue value)) return value;
				throw new KeyNotFoundException();
			}
			set => TryInsert(key, value, InsertionBehavior.OverwriteExisting);
		}

		/// <summary>
		/// Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be <c>null</c> for reference types.</param>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
		public void Add(Type key, TValue value)
		{
			TryInsert(key, value, InsertionBehavior.ThrowOnExisting);
		}

		/// <summary>
		/// Tries to add the specified key and value to dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be <c>null</c> for reference types.</param>
		/// <returns>
		/// <c>true</c> if the element was successfully added to the dictionary;
		/// <c>false</c> if the dictionary already contains an element with the specified key.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
		public bool TryAdd(Type key, TValue value)
		{
			return TryInsert(key, value, InsertionBehavior.None);
		}

		/// <summary>
		/// Removes the value with the specified key from the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		/// <c>true</c>true if the element is successfully found and removed; otherwise <c>false</c>.
		/// This method returns false if key is not found in the dictionary.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
		public bool Remove(Type key)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			if (mBuckets != null)
			{
				Debug.Assert(mEntries != null, "entries should be non-null");
				uint collisionCount = 0;
				uint hashCode = (uint)key.GetHashCode();
				ref int bucket = ref GetBucket(hashCode);
				Entry[] entries = mEntries;
				int last = -1;
				int i = bucket - 1; // Value in buckets is 1-based
				while (i >= 0)
				{
					ref Entry entry = ref entries[i];

					if (entry.HashCode == hashCode && entry.Key == key)
					{
						if (last < 0)
						{
							bucket = entry.Next + 1; // Value in buckets is 1-based
						}
						else
						{
							entries[last].Next = entry.Next;
						}

						Debug.Assert(
							StartOfFreeList - mFreeList < 0,
							// ReSharper disable once StringLiteralTypo
							"shouldn't underflow because max hashtable length is MaxPrimeArrayLength = 0x7FEFFFFD(2146435069) mFreeList underflow threshold 2147483646");

						entry.Next = StartOfFreeList - mFreeList;
						entry.Key = default;
						entry.Value = default;

						mFreeList = i;
						mFreeCount++;
						return true;
					}

					last = i;
					i = entry.Next;

					collisionCount++;
					if (collisionCount > (uint)entries.Length)
					{
						// The chain of entries forms a loop; which means a concurrent update has happened.
						// Break out of the loop and throw, rather than looping forever.
						throw new InvalidOperationException("Concurrent operations are not supported.");
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Removes all keys and values from the dictionary.
		/// </summary>
		public void Clear()
		{
			int count = mCount;

			if (count > 0)
			{
				Debug.Assert(mBuckets != null, "mBuckets should be non-null");
				Debug.Assert(mEntries != null, "mEntries should be non-null");

				Array.Clear(mBuckets, 0, mBuckets.Length);

				mCount = 0;
				mFreeList = -1;
				mFreeCount = 0;
				Array.Clear(mEntries, 0, count);
				mVersion++;
			}
		}

		/// <summary>
		/// Determines whether the dictionary contains the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the dictionary.</param>
		/// <returns>
		/// <c>true</c>, if the dictionary contains an element with the specified key;
		/// otherwise <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
		public bool ContainsKey(Type key)
		{
			return TryGetValue(key, out TValue _);
		}

		/// <summary>
		/// Determines whether the dictionary contains the specified value.
		/// </summary>
		/// <param name="value">
		/// The value to locate in the dictionary.
		/// The value can be <c>null</c> for reference types.
		/// </param>
		/// <returns>
		/// <c>true</c> if the dictionary contains an element with the specified value;
		/// otherwise <c>false</c>.
		/// </returns>
		public bool ContainsValue(TValue value)
		{
			Entry[] entries = mEntries;

			if (value == null)
			{
				for (int i = 0; i < mCount; i++)
				{
					if (entries[i].Next >= -1 && entries[i].Value == null)
					{
						return true;
					}
				}
			}
			else if (typeof(TValue).IsValueType)
			{
				// Value type: Devirtualize with EqualityComparer<TValue>.Default intrinsic
				for (int i = 0; i < mCount; i++)
				{
					if (entries[i].Next >= -1 && EqualityComparer<TValue>.Default.Equals(entries[i].Value, value))
					{
						return true;
					}
				}
			}
			else
			{
				// Object type: Shared Generic, EqualityComparer<TValue>.Default won't devirtualize
				// https://github.com/dotnet/runtime/issues/10050
				// So cache in a local rather than get EqualityComparer per loop iteration
				var defaultComparer = EqualityComparer<TValue>.Default;
				for (int i = 0; i < mCount; i++)
				{
					if (entries[i].Next >= -1 && defaultComparer.Equals(entries[i].Value, value))
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key whose value to get.</param>
		/// <param name="value">
		/// When this method returns, the value associated with the specified key, if the key is found;
		/// otherwise, the default value for the type of the value parameter.
		/// </param>
		/// <returns>
		/// <c>true</c> if the dictionary contains an item with the specified key;
		/// otherwise <c>false</c>.
		/// </returns>
		public bool TryGetValue(Type key, out TValue value)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			value = default;
			if (mBuckets != null)
			{
				Debug.Assert(mEntries != null, "expected entries to be != null");

				uint hashCode = (uint)key.GetHashCode();
				int i = GetBucket(hashCode);
				Entry[] entries = mEntries;
				uint collisionCount = 0;
				i--; // Value in mBuckets is 1-based; subtract 1 from i. We do it here so it fuses with the following conditional.
				do
				{
					// Should be a while loop https://github.com/dotnet/runtime/issues/9422
					// Test in if to drop range check for following array access
					if ((uint)i >= (uint)entries.Length)
						return false;

					ref Entry entry = ref entries[i];
					if (entry.HashCode == hashCode && entry.Key == key)
					{
						value = entry.Value;
						return true;
					}

					i = entry.Next;

					collisionCount++;
				} while (collisionCount <= (uint)entries.Length);

				// The chain of entries forms a loop; which means a concurrent update has happened.
				// Break out of the loop and throw, rather than looping forever.
				throw new InvalidOperationException("Concurrent operations are not supported.");
			}

			return false;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the dictionary.</returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this, Enumerator.KeyValuePair);
		}

		#region Explicit Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the dictionary.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this, Enumerator.KeyValuePair);
		}

		#endregion

		#region Explicit Implementation of IEnumerable<KeyValuePair<Type, TValue>>

		/// <summary>
		/// Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the dictionary.</returns>
		IEnumerator<KeyValuePair<Type, TValue>> IEnumerable<KeyValuePair<Type, TValue>>.GetEnumerator()
		{
			return new Enumerator(this, Enumerator.KeyValuePair);
		}

		#endregion

		#region Explicit Implementation of ICollection<T>

		/// <summary>
		/// Gets a value indicating whether the collection is read-only.
		/// </summary>
		/// <value>Always <c>true</c>.</value>
		bool ICollection<KeyValuePair<Type, TValue>>.IsReadOnly => false;

		/// <summary>
		/// Adds the specified key/value pair to the dictionary.
		/// </summary>
		/// <param name="item">Key/value pair to add.</param>
		/// <exception cref="ArgumentNullException"><paramref name="item.Key"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">An element with the same key already exists in the dictionary.</exception>
		void ICollection<KeyValuePair<Type, TValue>>.Add(KeyValuePair<Type, TValue> item)
		{
			TryInsert(item.Key, item.Value, InsertionBehavior.ThrowOnExisting);
		}

		/// <summary>
		/// Determines whether the dictionary contains the specified key/value pair.
		/// </summary>
		/// <param name="item">Key/value pair to locate in the dictionary.</param>
		/// <returns>
		/// <c>true</c>, if the dictionary contains the specified key/value pair;
		/// otherwise <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="item.Key"/> is <c>null</c>.</exception>
		bool ICollection<KeyValuePair<Type, TValue>>.Contains(KeyValuePair<Type, TValue> item)
		{
			return TryGetValue(item.Key, out TValue value) && EqualityComparer<TValue>.Default.Equals(value, item.Value);
		}

		/// <summary>
		/// Removes the specified key/value pair from the dictionary.
		/// </summary>
		/// <param name="item">Key/value pair to remove from the dictionary.</param>
		/// <returns>
		/// <c>true</c>, if the specified key/value pair was removed from the dictionary;
		/// <c>false</c> if the specified key/value pair was not found in the dictionary.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="item.Key"/> is <c>null</c>.</exception>
		bool ICollection<KeyValuePair<Type, TValue>>.Remove(KeyValuePair<Type, TValue> item)
		{
			if (TryGetValue(item.Key, out TValue value) && EqualityComparer<TValue>.Default.Equals(value, item.Value))
			{
				Remove(item.Key);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Copies the key/value pairs of the dictionary into the specified array.
		/// </summary>
		/// <param name="array">Destination array to copy the key/value pairs into.</param>
		/// <param name="index">Index in the destination array to start at.</param>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The specified index exceeds the bounds of the array.</exception>
		/// <exception cref="ArgumentException">The destination array is too small.</exception>
		void ICollection<KeyValuePair<Type, TValue>>.CopyTo(KeyValuePair<Type, TValue>[] array, int index)
		{
			CopyTo(array, index);
		}

		#endregion

		#region Explicit Implementation of IDictionary<TKey,TValue>

		/// <summary>
		/// Gets a collection containing the keys in the dictionary.
		/// </summary>
		ICollection<Type> IDictionary<Type, TValue>.Keys => mKeys ?? (mKeys = new KeyCollection(this));

		/// <summary>
		/// Gets a collection containing the values in the dictionary.
		/// </summary>
		ICollection<TValue> IDictionary<Type, TValue>.Values => mValues ?? (mValues = new ValueCollection(this));

		#endregion

		#region Helpers

		private enum InsertionBehavior
		{
			None,
			OverwriteExisting,
			ThrowOnExisting
		}

		/// <summary>
		/// Gets the number of the bucket storing objects with the specified hash code.
		/// </summary>
		/// <param name="hashCode">Hash code to get the bucket for.</param>
		/// <returns>The number of the bucket storing objects with the specified hash code.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ref int GetBucket(uint hashCode)
		{
			return ref mBuckets[hashCode % (uint)mBuckets.Length];
		}

		/// <summary>
		/// Tries to insert an element into the dictionary.
		/// </summary>
		/// <param name="key">Key of the element to insert.</param>
		/// <param name="value">Value of the element to insert.</param>
		/// <param name="behavior">Behavior determining whether to overwrite or throw on existing elements.</param>
		/// <returns>
		/// <c>true</c> if the dictionary was modified (element was inserted or overwritten);
		/// otherwise <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
		private bool TryInsert(Type key, TValue value, InsertionBehavior behavior)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			// initialize the buckets, if necessary
			if (mBuckets == null)
				Initialize(0);

			Entry[] entries = mEntries;

			// calculate a hash code for the specified key
			uint hashCode = (uint)key.GetHashCode();

			// get the bucket the key belongs into
			uint collisionCount = 0;
			ref int bucket = ref GetBucket(hashCode);
			int i = bucket - 1; // Value in mBuckets is 1-based

			while (true)
			{
				// Should be a while loop https://github.com/dotnet/runtime/issues/9422
				// Test uint in if rather than loop condition to drop range check for following array access
				if ((uint)i >= (uint)entries.Length)
					break;

				if (entries[i].HashCode == hashCode && entries[i].Key == key)
				{
					// found element with the specified key

					if (behavior == InsertionBehavior.OverwriteExisting)
					{
						entries[i].Value = value;
						return true;
					}

					if (behavior == InsertionBehavior.ThrowOnExisting)
					{
						throw new ArgumentException("An item with the specified key exists already.");
					}

					Debug.Assert(behavior == InsertionBehavior.None);
					return false;
				}

				i = entries[i].Next;

				collisionCount++;
				if (collisionCount > (uint)entries.Length)
				{
					// The chain of entries forms a loop; which means a concurrent update has happened.
					// Break out of the loop and throw, rather than looping forever.
					throw new InvalidOperationException("Concurrent operations are not supported.");
				}
			}

			int index;
			if (mFreeCount > 0)
			{
				index = mFreeList;
				Debug.Assert(StartOfFreeList - entries[mFreeList].Next >= -1, "shouldn't overflow because `Next` cannot underflow");
				mFreeList = StartOfFreeList - entries[mFreeList].Next;
				mFreeCount--;
			}
			else
			{
				int count = mCount;
				if (count == entries.Length)
				{
					Resize();
					bucket = ref GetBucket(hashCode);
				}

				index = count;
				mCount = count + 1;
				entries = mEntries;
			}

			ref Entry entry = ref entries[index];
			entry.HashCode = hashCode;
			entry.Next = bucket - 1; // Value in mBuckets is 1-based
			entry.Key = key;
			entry.Value = value;
			bucket = index + 1; // Value in mBuckets is 1-based
			mVersion++;

			return true;
		}

		/// <summary>
		/// Copies the key/value pairs of the dictionary into the specified array.
		/// </summary>
		/// <param name="array">Destination array to copy the key/value pairs into.</param>
		/// <param name="index">Index in the destination array to start at.</param>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The specified index exceeds the bounds of the array.</exception>
		/// <exception cref="ArgumentException">The destination array is too small.</exception>
		private void CopyTo(KeyValuePair<Type, TValue>[] array, int index)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if ((uint)index > (uint)array.Length)
				throw new ArgumentOutOfRangeException(nameof(index), "The specified index is out of bounds.");

			if (array.Length - index < Count)
				throw new ArgumentException("The destination array is too small.");

			int count = mCount;
			Entry[] entries = mEntries;
			for (int i = 0; i < count; i++)
			{
				if (entries[i].Next >= -1)
				{
					array[index++] = new KeyValuePair<Type, TValue>(entries[i].Key, entries[i].Value);
				}
			}
		}

		/// <summary>
		/// Enlarges the dictionary (the size is doubled and rounded up to the next prime).
		/// </summary>
		private void Resize()
		{
			Resize(HashHelpers.ExpandPrime(mCount));
		}

		/// <summary>
		/// Resizes the dictionary to the specified size.
		/// </summary>
		/// <param name="newSize">New size of the dictionary.</param>
		private void Resize(int newSize)
		{
			Debug.Assert(mEntries != null, "mEntries should be non-null");
			Debug.Assert(newSize >= mEntries.Length);

			var entries = new Entry[newSize];
			int count = mCount;
			Array.Copy(mEntries, entries, count);

			// assign member variables after both arrays allocated to guard against corruption from OOM if second fails
			mBuckets = new int[newSize];
			for (int i = 0; i < count; i++)
			{
				if (entries[i].Next >= -1)
				{
					ref int bucket = ref GetBucket(entries[i].HashCode);
					entries[i].Next = bucket - 1; // Value in _buckets is 1-based
					bucket = i + 1;
				}
			}

			mEntries = entries;
			Capacity = newSize;
		}

		#endregion
	}

}
