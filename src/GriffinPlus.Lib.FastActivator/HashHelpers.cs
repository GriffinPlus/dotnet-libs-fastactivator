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

// Some source for the implementation of this class was taken the official reference implementation of the .NET framework internal HashHelper class:
// https://github.com/dotnet/runtime/blob/57bfe474518ab5b7cfe6bf7424a79ce3af9d6657/src/libraries/System.Private.CoreLib/src/System/Collections/HashHelpers.cs

using System;
using System.Diagnostics;

namespace GriffinPlus.Lib
{

	/// <summary>
	/// A set of methods that come in handy when writing hash functions.
	/// </summary>
	static class HashHelpers
	{
		internal const uint HashCollisionThreshold = 100;
		internal const int  HashPrime              = 101;

		/// <summary>
		/// Table of prime numbers to use as hash table sizes.
		/// </summary>
		internal static readonly int[] Primes =
		{
			3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
			1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
			17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
			187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
			1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
		};

		/// <summary>
		/// The smallest positive prime.
		/// </summary>
		public const int MinPrime = 3;

		/// <summary>
		/// This is the maximum prime smaller than Array.MaxLength.
		/// </summary>
		public const int MaxPrimeArrayLength = 0x7FFFFFC3;

		/// <summary>
		/// Checks whether the specified number is prime.
		/// </summary>
		/// <param name="candidate">Number to check.</param>
		/// <returns>
		/// <c>true</c> if the specified number is prime;
		/// otherwise <c>false</c>.
		/// </returns>
		public static bool IsPrime(int candidate)
		{
			if ((candidate & 1) != 0)
			{
				int limit = (int)Math.Sqrt(candidate);
				for (int divisor = 3; divisor <= limit; divisor += 2)
				{
					if (candidate % divisor == 0)
						return false;
				}

				return true;
			}

			return candidate == 2;
		}

		/// <summary>
		/// Gets the next prime that is greater than or equal to the specified number.
		/// </summary>
		/// <param name="min">The lower bound of the prime to get.</param>
		/// <returns>A prime that is greater than or equal to the specified number.</returns>
		public static int GetPrime(int min)
		{
			if (min < 0)
				throw new ArgumentOutOfRangeException(nameof(min), min, "The minimum must be positive.");

			foreach (int prime in Primes)
			{
				if (prime >= min)
					return prime;
			}

			// outside of our predefined table
			// => compute the hard way
			for (int i = min | 1; i < int.MaxValue; i += 2)
			{
				if (IsPrime(i) && (i - 1) % HashPrime != 0)
					return i;
			}

			return min;
		}

		/// <summary>
		/// Gets the size of the hashtable to grow to.
		/// </summary>
		/// <param name="oldSize">Old size of the hash table.</param>
		/// <returns>New size of the hash table (always prime).</returns>
		public static int ExpandPrime(int oldSize)
		{
			int newSize = 2 * oldSize;

			// Allow the hashtable to grow to maximum possible size (~2G elements) before encountering capacity overflow.
			// Note that this check works even when _items.Length overflowed thanks to the (uint) cast
			if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
			{
				Debug.Assert(MaxPrimeArrayLength == GetPrime(MaxPrimeArrayLength), "Invalid MaxPrimeArrayLength");
				return MaxPrimeArrayLength;
			}

			return GetPrime(newSize);
		}
	}

}
