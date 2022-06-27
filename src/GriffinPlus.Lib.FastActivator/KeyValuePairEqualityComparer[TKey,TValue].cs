///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace GriffinPlus.Lib
{

	/// <summary>
	/// An equality comparer for comparing the key/value pairs (thread-safe).
	/// </summary>
	class KeyValuePairEqualityComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
	{
		private readonly IEqualityComparer<TKey>   mKeyEqualityComparer;
		private readonly IEqualityComparer<TValue> mValueEqualityComparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyValuePairEqualityComparer{TKey,TValue}"/> class using default equality comparers
		/// for keys and values.
		/// </summary>
		public KeyValuePairEqualityComparer() : this(null, null) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyValuePairEqualityComparer{TKey,TValue}"/> class with specific equality comparers
		/// for keys and values.
		/// </summary>
		/// <param name="keyEqualityComparer">Comparer to use for comparing keys (may be <c>null</c> to use the default comparer).</param>
		/// <param name="valueEqualityComparer">Comparer to use for comparing values (may be <c>null</c> to use the default comparer)</param>
		public KeyValuePairEqualityComparer(IEqualityComparer<TKey> keyEqualityComparer, IEqualityComparer<TValue> valueEqualityComparer)
		{
			mKeyEqualityComparer = keyEqualityComparer ?? EqualityComparer<TKey>.Default;
			mValueEqualityComparer = valueEqualityComparer ?? EqualityComparer<TValue>.Default;
		}

		/// <summary>
		/// Determines whether the specified key/value pairs are equal.
		/// </summary>
		/// <param name="x">The first key/value pair to compare.</param>
		/// <param name="y">The second key/value pair to compare.</param>
		/// <returns>
		/// <c>true</c>true if the specified key/value pairs equal;
		/// otherwise <c>false</c>false.
		/// </returns>
		public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
		{
			return mKeyEqualityComparer.Equals(x.Key, y.Key) && mValueEqualityComparer.Equals(x.Value, y.Value);
		}

		/// <summary>
		/// Returns a hash code for the specified key/value pair.
		/// </summary>
		/// <param name="keyValuePair">The key/value pair to calculate a hash code for.</param>
		/// <returns>The hash code for the specified key/value pair.</returns>
		/// <exception cref="ArgumentException">
		/// <paramref name="keyValuePair.Key"/> is null.
		/// -or-
		/// <paramref name="keyValuePair.Value"/> is null.
		/// </exception>
		public int GetHashCode(KeyValuePair<TKey, TValue> keyValuePair)
		{
			if (keyValuePair.Key == null) throw new ArgumentException("The key of the key/value pair is null.", nameof(keyValuePair));
			if (keyValuePair.Value == null) throw new ArgumentException("The value key/value pair is null.", nameof(keyValuePair));
			return mKeyEqualityComparer.GetHashCode(keyValuePair.Key) ^ mValueEqualityComparer.GetHashCode(keyValuePair.Value);
		}
	}

}
