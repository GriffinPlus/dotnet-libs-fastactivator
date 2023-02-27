///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace GriffinPlus.Lib
{

	partial class TypeKeyedDictionaryTests_Base<TValue>
	{
		#region KeyCollection # GetEnumerator() - incl. all enumerator functionality

		/// <summary>
		/// Tests the <see cref="GetEnumerator"/> method of the <see cref="TypeKeyedDictionary{TValue}.Keys"/> collection.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void KeyCollection_GetEnumerator(int count)
		{
			// get test data and create a new dictionary with it
			IDictionary<Type, TValue> data = GetTestData(count);
			var dict = new TypeKeyedDictionary<TValue>(data);
			TypeKeyedDictionary<TValue>.KeyCollection collection = dict.Keys;

			// get an enumerator
			TypeKeyedDictionary<TValue>.KeyCollection.Enumerator enumerator = collection.GetEnumerator();

			// the enumerator should point to the position before the first valid element,
			// but the 'Current' property should not throw an exception
			Type _ = enumerator.Current;

			// enumerate the keys in the collection
			var enumerated = new List<Type>();
			while (enumerator.MoveNext()) enumerated.Add(enumerator.Current);

			// the order of keys should be the same as returned by the dictionary enumerator
			Assert.Equal(
				dict.Select(x => x.Key),
				enumerated,
				KeyEqualityComparer);

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

		#region KeyCollection # ICollection<T>.Count

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Count"/> property of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void KeyCollection_ICollectionT_Count_Get(int count)
		{
			IDictionary<Type, TValue> data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;
			Assert.Equal(data.Count, collection.Count);
		}

		#endregion

		#region KeyCollection # ICollection<T>.IsReadOnly

		/// <summary>
		/// Tests the <see cref="ICollection{T}.IsReadOnly"/> property of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// </summary>
		[Fact]
		public void KeyCollection_ICollectionT_IsReadOnly()
		{
			var dict = GetDictionary() as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;
			Assert.True(collection.IsReadOnly);
		}

		#endregion

		#region KeyCollection # ICollection<T>.Add(T)

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Add"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// The implementation is expected to throw a <see cref="NotSupportedException"/>.
		/// </summary>
		[Fact]
		public void KeyCollection_ICollectionT_Add_NotSupported()
		{
			var dict = GetDictionary() as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;
			Assert.Throws<NotSupportedException>(() => collection.Add(default));
		}

		#endregion

		#region KeyCollection # ICollection<T>.Clear()

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Add"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// The implementation is expected to throw a <see cref="NotSupportedException"/>.
		/// </summary>
		[Fact]
		public void KeyCollection_ICollectionT_Clear_NotSupported()
		{
			var dict = GetDictionary() as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;
			Assert.Throws<NotSupportedException>(() => collection.Clear());
		}

		#endregion

		#region KeyCollection # ICollection<T>.Contains(T)

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Contains"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// The key is in the collection.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void KeyCollection_ICollectionT_Contains(int count)
		{
			// get test data and create a new dictionary with it
			IDictionary<Type, TValue> data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;

			// test whether keys of test data are reported to be in the collection
			foreach (KeyValuePair<Type, TValue> kvp in data)
			{
				bool contains = collection.Contains(kvp.Key);
				Assert.True(contains);
			}
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Contains"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// The key is not in the collection.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes_WithoutZero))]
		public void KeyCollection_ICollectionT_Contains_KeyNotFound(int count)
		{
			// get test data and create a new dictionary with it
			IDictionary<Type, TValue> data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;

			// test whether some other key is reported to be not in the collection
			bool contains = collection.Contains(KeyNotInTestData);
			Assert.False(contains);
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Contains"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// The method should return <c>false</c>, if the passed key is <c>null</c> (only for reference types).
		/// </summary>
		[Fact]
		public void KeyCollection_ICollectionT_Contains_KeyNull()
		{
			var dict = GetDictionary() as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;
			bool contains = collection.Contains(default);
			Assert.False(contains);
		}

		#endregion

		#region KeyCollection # ICollection<T>.CopyTo(T[],int)

		/// <summary>
		/// Tests the <see cref="ICollection{T}.CopyTo"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		/// <param name="index">Index in the destination array to start copying to.</param>
		[Theory]
		[MemberData(nameof(CopyTo_TestData))]
		public void KeyCollection_ICollectionT_CopyTo(int count, int index)
		{
			// get test data and create a new dictionary with it
			IDictionary<Type, TValue> data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;

			// copy the collection into an array
			var destination = new Type[count + index];
			collection.CopyTo(destination, index);

			// compare collection elements with the expected keys
			// (the order should be the same as returned by the dictionary enumerator) 
			Assert.Equal(
				dict.Select(x => x.Key),
				destination.Skip(index),
				KeyEqualityComparer);
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.CopyTo"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection
		/// passing <c>null</c> for the destination array.
		/// </summary>
		[Fact]
		public void KeyCollection_ICollectionT_CopyTo_ArrayNull()
		{
			var dict = GetDictionary() as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;
			// ReSharper disable once AssignNullToNotNullAttribute
			Assert.Throws<ArgumentNullException>(() => collection.CopyTo(null, 0));
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.CopyTo"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection
		/// passing an array index that is out of range.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		/// <param name="index">Index in the destination array to start copying to.</param>
		[Theory]
		[MemberData(nameof(CopyTo_TestData_IndexOutOfBounds))]
		public void KeyCollection_ICollectionT_CopyTo_IndexOutOfRange(int count, int index)
		{
			IDictionary<Type, TValue> data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;
			var destination = new Type[count];
			var exception = Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(destination, index));
			Assert.Equal("index", exception.ParamName);
		}

		/// <summary>
		/// Tests the <see cref="ICollection{T}.CopyTo"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection
		/// passing an destination array that is too small to store all.
		/// elements.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		/// <param name="arraySize">Size of the destination array.</param>
		/// <param name="index">Index in the destination array to start copying to.</param>
		[Theory]
		[MemberData(nameof(CopyTo_TestData_ArrayTooSmall))]
		public void KeyCollection_ICollectionT_CopyTo_ArrayTooSmall(int count, int arraySize, int index)
		{
			IDictionary<Type, TValue> data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;
			var destination = new Type[arraySize];
			Assert.Throws<ArgumentException>(() => collection.CopyTo(destination, index));
		}

		#endregion

		#region KeyCollection # ICollection<T>.Remove(T)

		/// <summary>
		/// Tests the <see cref="ICollection{T}.Remove"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// The implementation is expected to throw a <see cref="NotSupportedException"/>.
		/// </summary>
		[Fact]
		public void KeyCollection_ICollectionT_Remove_NotSupported()
		{
			var dict = GetDictionary() as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;
			Assert.Throws<NotSupportedException>(() => collection.Remove(default));
		}

		#endregion

		#region KeyCollection # IEnumerable.GetEnumerator() - incl. all enumerator functionality

		/// <summary>
		/// Tests the <see cref="IEnumerable.GetEnumerator"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void KeyCollection_IEnumerable_GetEnumerator(int count)
		{
			// get test data and create a new dictionary with it
			IDictionary<Type, TValue> data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;

			// get an enumerator
			var enumerable = (IEnumerable)collection;
			IEnumerator enumerator = enumerable.GetEnumerator();

			// the enumerator should point to the position before the first valid element,
			// but the 'Current' property should not throw an exception
			object _ = enumerator.Current;

			// enumerate the keys in the collection
			var enumerated = new List<Type>();
			while (enumerator.MoveNext()) enumerated.Add((Type)enumerator.Current);

			// the order of keys should be the same as returned by the dictionary enumerator
			Assert.Equal(
				dict.Select(x => x.Key),
				enumerated,
				KeyEqualityComparer);

			// the enumerator should point to the position after the last valid element now,
			// but the 'Current' property should not throw an exception
			// ReSharper disable once RedundantAssignment
			_ = enumerator.Current;

			// reset the enumerator and try again
			enumerator.Reset();
			enumerated = new List<Type>();
			while (enumerator.MoveNext()) enumerated.Add((Type)enumerator.Current);

			// the order of keys should be the same as returned by the dictionary enumerator
			Assert.Equal(
				dict.Select(x => x.Key),
				enumerated,
				KeyEqualityComparer);

			// modify the collection, the enumerator should recognize this
			dict[KeyNotInTestData] = ValueNotInTestData;
			Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
			Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
		}

		#endregion

		#region KeyCollection # IEnumerable<T>.GetEnumerator() - incl. all enumerator functionality

		/// <summary>
		/// Tests the <see cref="IEnumerable{T}.GetEnumerator"/> method of the <see cref="IDictionary{TKey,TValue}.Keys"/> collection.
		/// </summary>
		/// <param name="count">Number of elements to populate the dictionary with before running the test.</param>
		[Theory]
		[MemberData(nameof(TestDataSetSizes))]
		public void KeyCollection_IEnumerableT_GetEnumerator(int count)
		{
			// get test data and create a new dictionary with it
			IDictionary<Type, TValue> data = GetTestData(count);
			var dict = GetDictionary(data) as IDictionary<Type, TValue>;
			ICollection<Type> collection = dict.Keys;

			// get an enumerator
			IEnumerator<Type> enumerator = collection.GetEnumerator();

			// the enumerator should point to the position before the first valid element,
			// but the 'Current' property should not throw an exception
			Type _ = enumerator.Current;

			// enumerate the keys in the collection
			var enumerated = new List<Type>();
			while (enumerator.MoveNext()) enumerated.Add(enumerator.Current);

			// the order of keys should be the same as returned by the dictionary enumerator
			Assert.Equal(
				dict.Select(x => x.Key),
				enumerated,
				KeyEqualityComparer);

			// the enumerator should point to the position after the last valid element now,
			// but the 'Current' property should not throw an exception
			// ReSharper disable once RedundantAssignment
			_ = enumerator.Current;

			// reset the enumerator and try again
			enumerator.Reset();
			enumerated = new List<Type>();
			while (enumerator.MoveNext()) enumerated.Add(enumerator.Current);

			// the order of keys should be the same as returned by the dictionary enumerator
			Assert.Equal(
				dict.Select(x => x.Key),
				enumerated,
				KeyEqualityComparer);

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
