///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Unit tests targeting the <see cref="TypeKeyedDictionary{TValue}"/> class.
	/// </summary>
	public abstract partial class TypeKeyedDictionaryTests_Base<TValue>
	{
		#region Test Data

		/// <summary>
		/// Test data for tests expecting the size of the test data set only.
		/// Contains: 0, 1, 10, 100, 1000, 10000.
		/// </summary>
		public static IEnumerable<object[]> TestDataSetSizes
		{
			get
			{
				yield return new object[] { 0 };
				yield return new object[] { 1 };
				yield return new object[] { 10 };
				yield return new object[] { 100 };
			}
		}

		/// <summary>
		/// Test data for tests expecting the size of the test data set only.
		/// Contains: 1, 10, 100, 1000, 10000.
		/// </summary>
		public static IEnumerable<object[]> TestDataSetSizes_WithoutZero
		{
			get
			{
				yield return new object[] { 1 };
				yield return new object[] { 10 };
				yield return new object[] { 100 };
			}
		}

		/// <summary>
		/// Test data for CopyTo() tests expecting the size of the test data set and an index in the destination array to start copying to.
		/// For tests that should succeed.
		/// </summary>
		public static IEnumerable<object[]> CopyTo_TestData
		{
			get
			{
				foreach (object[] data in TestDataSetSizes)
				{
					int count = (int)data[0];
					yield return new object[] { count, 0 };
					yield return new object[] { count, 1 };
					yield return new object[] { count, 5 };
				}
			}
		}

		/// <summary>
		/// Test data for CopyTo() tests expecting the size of the test data set and an index in the destination array to start copying to.
		/// For tests that check whether CopyTo() fails, if the index is out of bounds.
		/// </summary>
		public static IEnumerable<object[]> CopyTo_TestData_IndexOutOfBounds
		{
			get
			{
				foreach (object[] data in TestDataSetSizes)
				{
					int count = (int)data[0];
					yield return new object[] { count, -1 };        // before start of array
					yield return new object[] { count, count + 1 }; // after end of array (count is ok, if there are no elements to copy)
				}
			}
		}

		/// <summary>
		/// Test data for CopyTo() tests expecting the size of the test data set, the size of the destination array and an
		/// index in the destination array to start copying to.
		/// For tests that check whether CopyTo() fails, if the array is too small.
		/// </summary>
		public static IEnumerable<object[]> CopyTo_TestData_ArrayTooSmall
		{
			get
			{
				foreach (object[] data in TestDataSetSizes)
				{
					int count = (int)data[0];

					if (count > 0)
					{
						// destination array is way too small to store any elements
						yield return new object[] { count, 0, 0 };

						// destination array itself is large enough, but start index shifts the destination out
						// (the last element does not fit into the array)
						yield return new object[] { count, count, 1 };

						// destination array itself is large enough, but start index shifts the destination out
						// (no space left for any elements)
						yield return new object[] { count, count, count };
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Gets a dictionary containing some test data.
		/// </summary>
		/// <param name="count">Number of entries in the dictionary.</param>
		/// <returns>A test data dictionary.</returns>
		protected abstract IDictionary<Type, TValue> GetTestData(int count);

		/// <summary>
		/// Gets a key that is guaranteed to be not in the generated test data set.
		/// </summary>
		protected virtual Type KeyNotInTestData => typeof(TypeKeyedDictionary<>);

		/// <summary>
		/// Gets a value that is guaranteed to be not in the generated test data set.
		/// Must not be the default value of <see cref="TValue"/>.
		/// </summary>
		protected abstract TValue ValueNotInTestData { get; }

		/// <summary>
		/// Gets a comparer for comparing keys.
		/// </summary>
		protected virtual IComparer<Type> KeyComparer => TypeComparer.Instance;

		/// <summary>
		/// Gets an equality comparer for comparing keys.
		/// </summary>
		protected virtual IEqualityComparer<Type> KeyEqualityComparer => EqualityComparer<Type>.Default;

		/// <summary>
		/// Gets a comparer for comparing values.
		/// </summary>
		protected virtual IComparer<TValue> ValueComparer => Comparer<TValue>.Default;

		/// <summary>
		/// Gets an equality comparer for comparing values.
		/// </summary>
		protected virtual IEqualityComparer<TValue> ValueEqualityComparer => EqualityComparer<TValue>.Default;

		/// <summary>
		/// Gets an equality comparer for comparing key/value pairs returned by the dictionary
		/// </summary>
		private KeyValuePairEqualityComparer<Type, TValue> KeyValuePairEqualityComparer => new KeyValuePairEqualityComparer<Type, TValue>(KeyEqualityComparer, ValueEqualityComparer);

		/// <summary>
		/// Gets an instance of the dictionary to test, populated with the specified data.
		/// </summary>
		/// <param name="data">Data to populate the dictionary with.</param>
		/// <returns>A new instance of the dictionary to test, populated with the specified data.</returns>
		internal abstract TypeKeyedDictionary<TValue> GetDictionary(IDictionary<Type, TValue> data = null);

		/// <summary>
		/// Gets types defined in all assemblies loaded into the current application domain,
		/// except <see cref="KeyNotInTestData"/>.
		/// </summary>
		/// <returns>Types defined in assemblies loaded into the current application domain, except <see cref="KeyNotInTestData"/>.</returns>
		protected Type[] GetTypes()
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => !a.IsDynamic)
				.SelectMany(
					a =>
					{
						try
						{
							return a.GetTypes();
						}
						catch (ReflectionTypeLoadException ex)
						{
							return ex.Types;
						}
					})
				.Where(x => x != KeyNotInTestData)
				.ToArray();
		}

		#region Construction

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}()"/> constructor succeeds.
		/// </summary>
		[Fact]
		public void Create_Default()
		{
			var dict = new TypeKeyedDictionary<TValue>();
			Create_CheckEmptyDictionary(dict);
		}

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}(int)"/> constructor succeeds with a positive capacity.
		/// </summary>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Create_WithCapacity(int capacity)
		{
			var dict = new TypeKeyedDictionary<TValue>(capacity);
			Create_CheckEmptyDictionary(dict, capacity);
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}()"/> constructor throws a <see cref="ArgumentOutOfRangeException"/>
		/// if a negative capacity is passed.
		/// </summary>
		[Fact]
		public void Create_WithCapacity_NegativeCapacity()
		{
			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new TypeKeyedDictionary<TValue>(-1));
			Assert.Equal("capacity", exception.ParamName);
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}(IDictionary{Type, TValue})"/> constructor succeeds.
		/// </summary>
		/// <param name="count">Number of elements the data set to pass to the constructor should contain.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Create_WithDictionary(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = new TypeKeyedDictionary<TValue>(data);

			// check the dictionary itself
			// --------------------------------------------------------------------------------------------------------

			// the Count property should return the number of elements
			Assert.Equal(data.Count, dict.Count);

			// the number of elements should always be lower than the capacity
			Assert.True(dict.Count <= dict.Capacity);

			if (count > 0)
			{
				// dictionary should contain elements
				Assert.NotEmpty(dict);                           // using the enumerator
				Assert.True(HashHelpers.IsPrime(dict.Capacity)); // the capacity of a hash table should always be a prime
			}
			else
			{
				// dictionary should not contain any elements
				Assert.Empty(dict);             // using the enumerator
				Assert.Equal(0, dict.Capacity); // the capacity should also be 0 (no internal data buffer)
			}

			// check collection of keys in the dictionary
			// --------------------------------------------------------------------------------------------------------

			// the Count property should return the number of elements
			Assert.Equal(data.Count, dict.Keys.Count);

			if (count > 0)
			{
				// dictionary should contain elements
				Assert.NotEmpty(dict.Keys); // using the enumerator
			}
			else
			{
				// dictionary should not contain any elements
				Assert.Empty(dict.Keys); // using the enumerator
			}

			// compare collection elements with the expected values
			// (use array comparer to force the keys into the same order and check for equality)
			Assert.Equal(
				data.Keys
					.OrderBy(x => x, KeyComparer),
				dict.Keys
					.OrderBy(x => x, KeyComparer));

			// check collection of values in the dictionary
			// --------------------------------------------------------------------------------------------------------

			// the Count property should return the number of elements
			Assert.Equal(data.Count, dict.Values.Count);

			if (count > 0)
			{
				// dictionary should contain elements
				Assert.NotEmpty(dict.Values); // using the enumerator
			}
			else
			{
				// dictionary should not contain any elements
				Assert.Empty(dict.Values); // using the enumerator
			}

			// compare collection elements with the expected values
			Assert.Equal(data.Values, dict.Values);
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}(IDictionary{Type, TValue})"/> constructor
		/// throws a <see cref="ArgumentNullException"/> if the source dictionary is <c>null</c>.
		/// </summary>
		[Fact]
		public void Create_WithDictionary_DictionaryNull()
		{
			var exception = Assert.Throws<ArgumentNullException>(() => new TypeKeyedDictionary<TValue>(null));
			Assert.Equal("dictionary", exception.ParamName);
		}

		/// <summary>
		/// Checks whether the dictionary has the expected state after construction.
		/// </summary>
		/// <param name="dict">Dictionary to check.</param>
		/// <param name="capacity">Initial capacity of the dictionary (as specified at construction time).</param>
		private static void Create_CheckEmptyDictionary(TypeKeyedDictionary<TValue> dict, int capacity = 0)
		{
			// calculate the actual capacity of the dictionary
			// (empty dictionary: 0, non-empty dictionary: always the next prime greater than the specified capacity)
			int expectedCapacity = capacity > 0 ? HashHelpers.GetPrime(capacity) : 0;

			// check the dictionary itself
			Assert.Equal(expectedCapacity, dict.Capacity);
			Assert.Equal(0, dict.Count);
			Assert.Empty(dict);

			// check collection of keys in the dictionary
			Assert.Equal(0, dict.Keys.Count);
			Assert.Empty(dict.Keys);

			// check collection of values in the dictionary
			Assert.Equal(0, dict.Values.Count);
			Assert.Empty(dict.Values);
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.Capacity

		/// <summary>
		/// Tests getting the <see cref="TypeKeyedDictionary{TValue}.Capacity"/> property.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Capacity_Get(int count)
		{
			var data = GetTestData(count);
			var dict = new TypeKeyedDictionary<TValue>(data);
			int expectedCapacity = count > 0 ? HashHelpers.GetPrime(count) : 0;
			Assert.Equal(expectedCapacity, dict.Capacity); // the capacity should always be prime
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.Count

		/// <summary>
		/// Tests getting the <see cref="TypeKeyedDictionary{TValue}.Count"/> property.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Count_Get(int count)
		{
			var data = GetTestData(count);
			var dict = GetDictionary(data);
			Assert.Equal(data.Count, dict.Count);
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.Keys

		/// <summary>
		/// Tests accessing the key collection via <see cref="TypeKeyedDictionary{TValue}.Keys"/>.
		/// The key collection should present all keys in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Keys(int count)
		{
			// get test data and create a new dictionary with it
			var expected = GetTestData(count);
			var dict = GetDictionary(expected);

			// enumerate the keys in the dictionary
			var enumerated = dict.Keys.ToList();

			// compare collection elements with the expected values
			Assert.Equal(
				expected.Select(x => x.Key).OrderBy(x => x, KeyComparer),
				enumerated.OrderBy(x => x, KeyComparer),
				KeyEqualityComparer);
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.Values

		/// <summary>
		/// Tests accessing the value collection via <see cref="TypeKeyedDictionary{TValue}.Values"/>.
		/// The value collection should present all values in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Values(int count)
		{
			// get test data and create a new dictionary with it
			var expected = GetTestData(count);
			var dict = GetDictionary(expected);

			// enumerate the values in the dictionary
			var enumerated = dict.Values.ToList();

			// compare collection elements with the expected values
			Assert.Equal(
				expected.Select(x => x.Value).OrderBy(x => x, ValueComparer),
				enumerated.OrderBy(x => x, ValueComparer),
				ValueEqualityComparer);
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.this[Type]

		/// <summary>
		/// Tests accessing the key collection via <see cref="TypeKeyedDictionary{TValue}.this"/>.
		/// The key of the element is in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Indexer_Get_List(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// test whether keys of test data are reported to be in the dictionary
			foreach (var kvp in data)
			{
				Assert.Equal(kvp.Value, dict[kvp.Key]);
			}
		}

		/// <summary>
		/// Tests accessing the key collection via <see cref="TypeKeyedDictionary{TValue}.this"/>.
		/// The key of the element is not in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Indexer_Get_List_KeyNotFound(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// test whether some other key is reported to be not in the dictionary
			Assert.Throws<KeyNotFoundException>(() => dict[KeyNotInTestData]);
		}

		/// <summary>
		/// Tests whether <see cref="IDictionary{TKey,TValue}.this[TKey]"/> fails, if the passed key is <c>null</c>.
		/// Only for reference types.
		/// </summary>
		[Fact]
		public void Indexer_Get_List_KeyNull()
		{
			var dict = GetDictionary();
			// ReSharper disable once AssignNullToNotNullAttribute
			var exception = Assert.Throws<ArgumentNullException>(() => dict[default]);
			Assert.Equal("key", exception.ParamName);
		}

		/// <summary>
		/// Tests accessing the key collection via <see cref="TypeKeyedDictionary{TValue}.this"/>.
		/// The item is added to the dictionary, because there is no item with the specified key, yet.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Indexer_Set_List_NewItem(int count)
		{
			// get test data and create an empty dictionary
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// add data to the dictionary
			foreach (var kvp in data)
			{
				dict[kvp.Key] = kvp.Value;
			}

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			foreach (var kvp in dict) enumerated.Add(kvp);

			// compare collection elements with the expected key/value pairs
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		/// <summary>
		/// Tests accessing the key collection via <see cref="TypeKeyedDictionary{TValue}.this"/>.
		/// The item is overwritten, because there is already an item with the specified key in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes_WithoutZero))]
		public void Indexer_Set_List_OverwriteItem(int count)
		{
			// get test data and create an empty dictionary
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// add data to the dictionary
			foreach (var kvp in data)
			{
				dict[kvp.Key] = kvp.Value;
			}

			// overwrite an item
			var key = data.First().Key;
			data[key] = ValueNotInTestData;
			dict[key] = ValueNotInTestData;

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			foreach (var kvp in dict) enumerated.Add(kvp);

			// compare collection elements with the expected key/value pairs
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		/// <summary>
		/// Tests whether <see cref="TypeKeyedDictionary{TValue}.this"/> fails, if the passed key is <c>null</c>.
		/// Only for reference types.
		/// </summary>
		[Fact]
		public void Indexer_Set_List_KeyNull()
		{
			var dict = GetDictionary();
			// ReSharper disable once AssignNullToNotNullAttribute
			var exception = Assert.Throws<ArgumentNullException>(() => dict[default] = default);
			Assert.Equal("key", exception.ParamName);
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.Add(Type, TValue)

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.Add"/> method.
		/// </summary>
		/// <param name="count">Number of elements to add to the dictionary.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Add_List(int count)
		{
			// get test data and create an empty dictionary
			var data = GetTestData(count);
			var dict = GetDictionary();
			// add data to the dictionary
			foreach (var kvp in data)
			{
				dict.Add(kvp.Key, kvp.Value);
			}

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			foreach (var kvp in dict) enumerated.Add(kvp);

			// compare collection elements with the expected key/value pairs
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}.Add(Type,TValue)"/> method fails,
		/// if the key is already in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to add to the dictionary.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Add_List_DuplicateKey(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary();

			// add data to the dictionary
			KeyValuePair<Type, TValue>? first = null;
			KeyValuePair<Type, TValue>? last = null;
			foreach (var kvp in data)
			{
				if (first == null) first = kvp;
				last = kvp;
				dict.Add(kvp.Key, kvp.Value);
			}

			// try to add the first and the last element once again
			if (first != null) Assert.Throws<ArgumentException>(() => dict.Add(first.Value.Key, first.Value.Value));
			if (last != null) Assert.Throws<ArgumentException>(() => dict.Add(last.Value.Key, last.Value.Value));

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			foreach (var kvp in dict) enumerated.Add(kvp);

			// compare collection elements with the expected key/value pairs
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}.Add(Type,TValue)"/> method fails, if the key is <c>null</c>.
		/// Only for reference types.
		/// </summary>
		[Fact]
		public void Add_List_KeyNull()
		{
			var dict = GetDictionary();
			// ReSharper disable once AssignNullToNotNullAttribute
			var exception = Assert.Throws<ArgumentNullException>(() => dict.Add(default, default));
			Assert.Equal("key", exception.ParamName); // the 'key' is actually not the name of the method parameter
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.Clear()

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.Clear"/> method.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Clear(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// clear the dictionary
			dict.Clear();

			// the dictionary should be empty now
			Assert.Equal(0, dict.Count);
			Assert.Empty(dict);
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.ContainsKey(Type)

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.ContainsKey"/> method.
		/// The key of the element is in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void ContainsKey_List(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// test whether keys of test data are reported to be in the dictionary
			foreach (var kvp in data)
			{
				Assert.True(dict.ContainsKey(kvp.Key));
			}
		}

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.ContainsKey"/> method.
		/// The key of the element is not in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void ContainsKey_List_KeyNotFound(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// test whether some other key is reported to be not in the dictionary
			Assert.False(dict.ContainsKey(KeyNotInTestData));
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}.ContainsKey"/> method fails, if the passed key is <c>null</c>.
		/// Only for reference types.
		/// </summary>
		[Fact]
		public void ContainsKey_List_KeyNull()
		{
			var dict = GetDictionary();
			// ReSharper disable once AssignNullToNotNullAttribute
			var exception = Assert.Throws<ArgumentNullException>(() => dict.ContainsKey(default));
			Assert.Equal("key", exception.ParamName);
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.ContainsValue(TValue)

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.ContainsValue"/> method.
		/// The value of the element is in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void ContainsValue(int count)
		{
			// get test data and create a new dictionary with it,
			// replace one element with a null reference, if TValue is a reference type to check that too
			// (last element is better than the first one as it requires to iterate over all elements => better code coverage)
			var data = GetTestData(count);
			if (data.Count > 1 && !typeof(TValue).IsValueType) data[data.Last().Key] = default;
			var dict = GetDictionary(data);

			// test whether keys of test data are reported to be in the dictionary
			foreach (var kvp in data)
			{
				Assert.True(dict.ContainsValue(kvp.Value));
			}
		}

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.ContainsValue"/> method.
		/// The value of the element is not in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void ContainsValue_ValueNotFound(int count)
		{
			// get test data and create a new dictionary with it,
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// test whether some other value is reported to be not in the dictionary
			// (just take the default value of the value type, the test data does not contain the default value)
			Assert.False(dict.ContainsValue(ValueNotInTestData));
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.GetEnumerator() - incl. all enumerator functionality

		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void GetEnumerator(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = new TypeKeyedDictionary<TValue>(data);

			// get an enumerator
			var enumerator = dict.GetEnumerator();

			// the enumerator should point to the position before the first valid element,
			// but the 'Current' property should not throw an exception
			var _ = enumerator.Current;

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			while (enumerator.MoveNext())
			{
				Assert.IsType<KeyValuePair<Type, TValue>>(enumerator.Current);
				var current = enumerator.Current;
				enumerated.Add(current);
			}

			// compare collection elements with the expected values
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);

			// the enumerator should point to the position after the last valid element now,
			// but the 'Current' property should not throw an exception
			// ReSharper disable once RedundantAssignment
			_ = enumerator.Current;

			// modify the collection, the enumerator should recognize this
			dict[KeyNotInTestData] = ValueNotInTestData;
			Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());

			// dispose the enumerator
			enumerator.Dispose();
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.Remove(Type)

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.Remove"/> method.
		/// The key of the element to remove is in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Remove_List(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// remove elements in random order until the dictionary is empty
			var random = new Random();
			var remainingData = data.ToList();
			while (remainingData.Count > 0)
			{
				int index = random.Next(0, remainingData.Count - 1);
				dict.Remove(remainingData[index].Key);
				remainingData.RemoveAt(index);
				Assert.Equal(remainingData.Count, dict.Count);
			}

			// the dictionary should be empty now
			Assert.Equal(0, dict.Count);
			Assert.Empty(dict);
		}

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.Remove"/> method.
		/// The key of the element to remove is not in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void Remove_List_NotFound(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// try to remove an element that does not exist
			Assert.False(dict.Remove(KeyNotInTestData));
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}.Remove"/> method fails, if the passed key is <c>null</c>.
		/// Only for reference types.
		/// </summary>
		[Fact]
		public void Remove_List_KeyNull()
		{
			var dict = GetDictionary();
			// ReSharper disable once AssignNullToNotNullAttribute
			var exception = Assert.Throws<ArgumentNullException>(() => dict.Remove(default));
			Assert.Equal("key", exception.ParamName);
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.TryAdd(Type, TValue)

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.TryAdd"/> method.
		/// </summary>
		/// <param name="count">Number of elements to add to the dictionary.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void TryAdd_List(int count)
		{
			// get test data and create an empty dictionary
			var data = GetTestData(count);
			var dict = GetDictionary();

			// add data to the dictionary
			foreach (var kvp in data)
			{
				Assert.True(dict.TryAdd(kvp.Key, kvp.Value));
			}

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			var enumerable = (IEnumerable<KeyValuePair<Type, TValue>>)dict;
			foreach (var kvp in enumerable) enumerated.Add(kvp);

			// compare collection elements with the expected key/value pairs
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}.TryAdd"/> method fails,
		/// if the key is already in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to add to the dictionary.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void TryAdd_List_DuplicateKey(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary();

			// add data to the dictionary
			KeyValuePair<Type, TValue>? first = null;
			KeyValuePair<Type, TValue>? last = null;
			foreach (var kvp in data)
			{
				if (first == null) first = kvp;
				last = kvp;
				Assert.True(dict.TryAdd(kvp.Key, kvp.Value));
			}

			// try to add the first and the last element once again
			if (first != null) Assert.False(dict.TryAdd(first.Value.Key, first.Value.Value));
			if (last != null) Assert.False(dict.TryAdd(last.Value.Key, last.Value.Value));

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			var enumerable = (IEnumerable<KeyValuePair<Type, TValue>>)dict;
			foreach (var kvp in enumerable) enumerated.Add(kvp);

			// compare collection elements with the expected key/value pairs
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}.TryAdd"/> method fails, if the key is <c>null</c>.
		/// For reference types only.
		/// </summary>
		[Fact]
		public void TryAdd_List_KeyNull()
		{
			var dict = GetDictionary();
			// ReSharper disable once AssignNullToNotNullAttribute
			var exception = Assert.Throws<ArgumentNullException>(() => dict.Add(default, default));
			Assert.Equal("key", exception.ParamName); // the 'key' is actually not the name of the method parameter
		}

		#endregion

		#region ICollection<T>.IsReadOnly

		/// <summary>
		/// Tests getting the <see cref="ICollection{T}.IsReadOnly"/> property.
		/// </summary>
		[Fact]
		public void ICollectionT_IsReadOnly_Get()
		{
			var dict = GetDictionary() as ICollection<KeyValuePair<Type, TValue>>;
			Assert.False(dict.IsReadOnly);
		}

		#endregion

		#region ICollection<T>.Add(T)

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Add"/> method.
		/// </summary>
		/// <param name="count">Number of elements to add to the dictionary.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void ICollectionT_Add(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary() as ICollection<KeyValuePair<Type, TValue>>;

			// add data to the dictionary
			foreach (var kvp in data)
			{
				dict.Add(new KeyValuePair<Type, TValue>(kvp.Key, kvp.Value));
			}

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			foreach (var kvp in dict) enumerated.Add(kvp);

			// compare collection elements with the expected key/value pairs
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		/// <summary>
		/// Tests whether the <see cref="ICollection{T}.Add"/> method fails, if the key of the key/value pair is already in the dictionary.
		/// </summary>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void ICollectionT_Add_DuplicateKey(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary() as ICollection<KeyValuePair<Type, TValue>>;

			// add data to the dictionary
			KeyValuePair<Type, TValue>? first = null;
			KeyValuePair<Type, TValue>? last = null;
			foreach (var kvp in data)
			{
				if (first == null) first = kvp;
				last = kvp;
				dict.Add(new KeyValuePair<Type, TValue>(kvp.Key, kvp.Value));
			}

			// try to add the first and the last element once again
			if (first != null) Assert.Throws<ArgumentException>(() => dict.Add(first.Value));
			if (last != null) Assert.Throws<ArgumentException>(() => dict.Add(last.Value));

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			foreach (var kvp in dict) enumerated.Add(kvp);

			// compare collection elements with the expected key/value pairs
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		/// <summary>
		/// Tests whether the <see cref="ICollection{T}.Add"/> method fails, if the key of the passed key/value pair is <c>null</c>.
		/// </summary>
		[Fact]
		public void ICollectionT_Add_KeyNull()
		{
			var dict = GetDictionary() as ICollection<KeyValuePair<Type, TValue>>;
			var exception = Assert.Throws<ArgumentNullException>(() => dict.Add(new KeyValuePair<Type, TValue>(default, default)));
			Assert.Equal("key", exception.ParamName); // the 'key' is actually not the name of the method parameter
		}

		#endregion

		#region ICollection<T>.Contains(T)

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Contains"/> method.
		/// The element is in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void ICollectionT_Contains(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data) as ICollection<KeyValuePair<Type, TValue>>;

			// test whether keys of test data are reported to be in the dictionary
			foreach (var kvp in data)
			{
				Assert.True(dict.Contains(new KeyValuePair<Type, TValue>(kvp.Key, kvp.Value)));
			}
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Contains"/> method.
		/// The key of the element is not in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes_WithoutZero))]
		public void ICollectionT_Contains_KeyNotFound(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data) as ICollection<KeyValuePair<Type, TValue>>;

			// test whether some other key is reported to be not in the dictionary
			Assert.False(
				dict.Contains(
					new KeyValuePair<Type, TValue>(
						KeyNotInTestData,
						data.First().Value)));
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Contains"/> method.
		/// The key of the element is in the dictionary, but the value does not match.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes_WithoutZero))]
		public void ICollectionT_Contains_ValueMismatch(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data) as ICollection<KeyValuePair<Type, TValue>>;

			// test whether some other key is reported to be not in the dictionary
			Assert.False(
				dict.Contains(
					new KeyValuePair<Type, TValue>(
						data.First().Key,
						ValueNotInTestData)));
		}

		/// <summary>
		/// Tests whether the <see cref="ICollection{T}.Contains"/> method fails, if the key of the passed key/value pair is <c>null</c>.
		/// Only for reference types.
		/// </summary>
		[Fact]
		public void ICollectionT_Contains_KeyNull()
		{
			var dict = GetDictionary() as ICollection<KeyValuePair<Type, TValue>>;
			var exception = Assert.Throws<ArgumentNullException>(() => dict.Contains(new KeyValuePair<Type, TValue>(default, default)));
			Assert.Equal("key", exception.ParamName); // the 'key' is actually not the name of the method parameter
		}

		#endregion

		#region ICollection<T>.CopyTo(T[], int)

		/// <summary>
		/// Tests the <see cref="ICollection{T}.CopyTo"/> method.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		/// <param name="index">Index in the destination array to start copying to.</param>
		[Theory]
		[MemberData(nameof(CopyTo_TestData))]
		public void ICollectionT_CopyTo(int count, int index)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data) as ICollection<KeyValuePair<Type, TValue>>;

			// copy the dictionary into an array
			var destination = new KeyValuePair<Type, TValue>[count + index];
			dict.CopyTo(destination, index);

			// compare collection elements with the expected data set
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				destination.Skip(index).OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.CopyTo"/> method passing <c>null</c> for the destination array.
		/// </summary>
		[Fact]
		public void ICollectionT_CopyTo_ArrayNull()
		{
			var dict = GetDictionary() as ICollection<KeyValuePair<Type, TValue>>;
			// ReSharper disable once AssignNullToNotNullAttribute
			Assert.Throws<ArgumentNullException>(() => dict.CopyTo(null, 0));
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.CopyTo"/> method passing an array index that is out of range.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		/// <param name="index">Index in the destination array to start copying to.</param>
		[Theory]
		[MemberData(nameof(CopyTo_TestData_IndexOutOfBounds))]
		public void ICollectionT_CopyTo_IndexOutOfRange(int count, int index)
		{
			var data = GetTestData(count);
			var dict = GetDictionary(data) as ICollection<KeyValuePair<Type, TValue>>;
			var destination = new KeyValuePair<Type, TValue>[count];
			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => dict.CopyTo(destination, index));
			Assert.Equal("index", exception.ParamName);
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.CopyTo"/> method passing an destination array that is too small to store all elements.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		/// <param name="arraySize">Size of the destination array.</param>
		/// <param name="index">Index in the destination array to start copying to.</param>
		[Theory]
		[MemberData(nameof(CopyTo_TestData_ArrayTooSmall))]
		public void ICollectionT_CopyTo_ArrayTooSmall(int count, int arraySize, int index)
		{
			var data = GetTestData(count);
			var dict = GetDictionary(data) as ICollection<KeyValuePair<Type, TValue>>;
			var destination = new KeyValuePair<Type, TValue>[arraySize];
			Assert.Throws<ArgumentException>(() => dict.CopyTo(destination, index));
		}

		#endregion

		#region ICollection<T>.Remove(T)

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Remove"/> method.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void ICollectionT_Remove(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data) as ICollection<KeyValuePair<Type, TValue>>;

			// remove elements in random order until the dictionary is empty
			var random = new Random();
			var remainingData = data.ToList();
			while (remainingData.Count > 0)
			{
				int index = random.Next(0, remainingData.Count - 1);
				bool removed = dict.Remove(remainingData[index]);
				Assert.True(removed);
				remainingData.RemoveAt(index);
				Assert.Equal(remainingData.Count, dict.Count);
			}

			// the dictionary should be empty now
			Assert.Equal(0, dict.Count);
			Assert.Empty(dict);
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Remove"/> method.
		/// The key of the item to remove is not in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes_WithoutZero))]
		public void ICollectionT_Remove_KeyNotFound(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data) as ICollection<KeyValuePair<Type, TValue>>;

			// try to remove an element that does not exist
			var kvp = new KeyValuePair<Type, TValue>(KeyNotInTestData, data.First().Value);
			Assert.False(dict.Remove(kvp));
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Remove"/> method.
		/// The key of the item to remove is in the dictionary, but the value does not match.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes_WithoutZero))]
		public void ICollectionT_Remove_ValueMismatch(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data) as ICollection<KeyValuePair<Type, TValue>>;

			// try to remove an element that does not exist
			var kvp = new KeyValuePair<Type, TValue>(data.First().Key, ValueNotInTestData);
			Assert.False(dict.Remove(kvp));
		}

		/// <summary>
		/// Tests whether the <see cref="ICollection{T}.Remove"/> method fails, if the key of the passed key/value pair is <c>null</c>.
		/// Only for reference types.
		/// </summary>
		[Fact]
		public void ICollectionT_Remove_KeyNull()
		{
			var dict = GetDictionary() as ICollection<KeyValuePair<Type, TValue>>;
			var exception = Assert.Throws<ArgumentNullException>(() => dict.Remove(new KeyValuePair<Type, TValue>(default, default)));
			Assert.Equal("key", exception.ParamName); // the 'key' is actually not the name of the method parameter
		}

		#endregion

		#region TypeKeyedDictionary<TValue>.TryGetValue(Type, out TValue)

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.TryGetValue(Type,out TValue)"/> method.
		/// The key of the element to get is in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void TryGetValue_List(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// test whether keys of test data are reported to be in the dictionary
			foreach (var kvp in data)
			{
				Assert.True(dict.TryGetValue(kvp.Key, out var value));
				Assert.Equal(kvp.Value, value);
			}
		}

		/// <summary>
		/// Tests the <see cref="TypeKeyedDictionary{TValue}.TryGetValue(Type,out TValue)"/> method.
		/// The key of the element to get is not in the dictionary.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void TryGetValue_List_KeyNotFound(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data);

			// test whether some other key is reported to be not in the dictionary
			Assert.False(dict.TryGetValue(KeyNotInTestData, out _));
		}

		/// <summary>
		/// Tests whether the <see cref="TypeKeyedDictionary{TValue}.TryGetValue(Type,out TValue)"/> method fails, if the passed key is <c>null</c>.
		/// Only for reference types.
		/// </summary>
		[Fact]
		public void TryGetValue_List_KeyNull()
		{
			var dict = GetDictionary();
			// ReSharper disable once AssignNullToNotNullAttribute
			var exception = Assert.Throws<ArgumentNullException>(() => dict.TryGetValue(default, out _));
			Assert.Equal("key", exception.ParamName);
		}

		#endregion

		#region Add-Remove-Add, Entry Recycling

		/// <summary>
		/// Tests adding items using the <see cref="IDictionary{TKey,TValue}.Add(TKey,TValue)"/> method,
		/// then removing items using the <see cref="IDictionary{TKey,TValue}.Remove(TKey)"/> method,
		/// then adding the removed items again.
		/// This tests whether the free-list in the dictionary is used correctly.
		/// </summary>
		/// <param name="count">Number of elements to add to the dictionary.</param>
		[Theory]
		[InlineData(5000)]
		public void AddAfterRemove_List(int count)
		{
			// get test data and create an empty dictionary
			var data = GetTestData(count);
			var dict = GetDictionary();

			// add data to the dictionary
			foreach (var kvp in data)
			{
				dict.Add(kvp.Key, kvp.Value);
			}

			// compare collection elements with the expected key/value pairs
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			foreach (var kvp in dict) enumerated.Add(kvp);
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);

			// remove elements in random order until the dictionary is empty
			var random = new Random();
			var remainingData = data.ToList();
			while (remainingData.Count > 0)
			{
				int index = random.Next(0, remainingData.Count - 1);
				dict.Remove(remainingData[index].Key);
				remainingData.RemoveAt(index);
				Assert.Equal(remainingData.Count, dict.Count);
			}

			// the dictionary should be empty now
			Assert.Equal(0, dict.Count);
			Assert.Empty(dict);

			// add data to the dictionary
			foreach (var kvp in data)
			{
				dict.Add(kvp.Key, kvp.Value);
			}

			// the dictionary should now contain the expected key/value pairs
			enumerated = new List<KeyValuePair<Type, TValue>>();
			foreach (var kvp in dict) enumerated.Add(kvp);
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);
		}

		#endregion

		#region IEnumerable.GetEnumerator() - incl. all enumerator functionality

		/// <summary>
		/// Tests enumerating key/value pairs using <see cref="IEnumerable.GetEnumerator"/>.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void IEnumerable_GetEnumerator(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;

			// get an enumerator
			var enumerator = ((IEnumerable)dict).GetEnumerator();

			// the enumerator should point to the position before the first valid element,
			// but the 'Current' property should not throw an exception
			object _ = enumerator.Current;

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			while (enumerator.MoveNext())
			{
				Assert.IsType<KeyValuePair<Type, TValue>>(enumerator.Current);
				var current = (KeyValuePair<Type, TValue>)enumerator.Current;
				Assert.IsAssignableFrom<Type>(current.Key);
				Assert.IsAssignableFrom<TValue>(current.Value);
				enumerated.Add(current);
			}

			// compare collection elements with the expected values
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);

			// the enumerator should point to the position after the last valid element now,
			// but the 'Current' property should not throw an exception
			// ReSharper disable once RedundantAssignment
			_ = enumerator.Current;

			// reset the enumerator and try again
			enumerator.Reset();
			enumerated = new List<KeyValuePair<Type, TValue>>();
			while (enumerator.MoveNext())
			{
				Assert.IsType<KeyValuePair<Type, TValue>>(enumerator.Current);
				var current = (KeyValuePair<Type, TValue>)enumerator.Current;
				Assert.IsAssignableFrom<Type>(current.Key);
				Assert.IsAssignableFrom<TValue>(current.Value);
				enumerated.Add(current);
			}

			// compare collection elements with the expected values
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);

			// modify the collection, the enumerator should recognize this
			dict[KeyNotInTestData] = ValueNotInTestData;
			Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
			Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
		}

		#endregion

		#region IEnumerable<KeyValuePair<Type,TValue>>.GetEnumerator() - incl. all enumerator functionality

		/// <summary>
		/// Tests enumerating key/value pairs using <see cref="IEnumerable{T}.GetEnumerator"/>.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void IEnumerableT_GetEnumerator(int count)
		{
			// get test data and create a new dictionary with it
			var data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;

			// get an enumerator
			var enumerator = dict.GetEnumerator();

			// the enumerator should point to the position before the first valid element,
			// but the 'Current' property should not throw an exception
			var _ = enumerator.Current;

			// enumerate the key/value pairs in the dictionary
			var enumerated = new List<KeyValuePair<Type, TValue>>();
			while (enumerator.MoveNext())
			{
				Assert.IsType<KeyValuePair<Type, TValue>>(enumerator.Current);
				var current = enumerator.Current;
				enumerated.Add(current);
			}

			// compare collection elements with the expected values
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);

			// the enumerator should point to the position after the last valid element now,
			// but the 'Current' property should not throw an exception
			// ReSharper disable once RedundantAssignment
			_ = enumerator.Current;

			// reset the enumerator and try again
			enumerator.Reset();
			enumerated = new List<KeyValuePair<Type, TValue>>();
			while (enumerator.MoveNext())
			{
				Assert.IsType<KeyValuePair<Type, TValue>>(enumerator.Current);
				var current = enumerator.Current;
				enumerated.Add(current);
			}

			// compare collection elements with the expected values
			Assert.Equal(
				data.OrderBy(x => x.Key, KeyComparer),
				enumerated.OrderBy(x => x.Key, KeyComparer),
				KeyValuePairEqualityComparer);

			// modify the collection, the enumerator should recognize this
			dict[KeyNotInTestData] = ValueNotInTestData;
			Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
			Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());

			// dispose the enumerator
			enumerator.Dispose();
		}

		#endregion
	}

}
