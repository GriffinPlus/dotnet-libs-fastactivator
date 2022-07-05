///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xunit;

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Tests around the <see cref="FastActivator"/> class.
	/// </summary>
	public class NonGenericFastActivatorTests
	{
		#region CreateArray()

		/// <summary>
		/// Gets test data for testing the <see cref="FastActivator{T}.CreateArray"/> method.
		/// </summary>
		public static IEnumerable<object[]> CreateArrayTestData
		{
			get
			{
				foreach (var type in new[] { typeof(TestStruct<int>), typeof(TestClass<int>) })
				{
					yield return new object[] { type, 0 };
					yield return new object[] { type, 1 };
					yield return new object[] { type, 5 };
				}
			}
		}

		/// <summary>
		/// Checks whether <see cref="FastActivator.CreateArray"/> works as expected.
		/// </summary>
		[Theory]
		[MemberData(nameof(CreateArrayTestData))]
		public void CreateArray(Type type, int count)
		{
			Array expected = Array.CreateInstance(type, count);
			Array actual = FastActivator.CreateArray(type, count);
			Assert.Equal(expected, actual);
		}

		#endregion

		#region CreateInstance()

		/// <summary>
		/// Gets test data for test methods targeting the CreateInstance() overloads.
		/// </summary>
		public static IEnumerable<object[]> CreateInstanceTestData
		{
			get
			{
				foreach (var type in new[] { typeof(TestStruct<int>), typeof(TestClass<int>) })
				{
					for (int i = 0; i <= 16; i++)
					{
						yield return new object[] { type, i };
					}
				}
			}
		}

		/// <summary>
		/// Checks whether the following methods work as expected:
		/// - <see cref="FastActivator.CreateInstance"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12,TArg13}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12,TArg13,TArg14}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12,TArg13,TArg14,TArg15}"/><br/>
		/// - <see cref="FastActivator.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12,TArg13,TArg14,TArg15,TArg16}"/>
		/// </summary>
		[Theory]
		[MemberData(nameof(CreateInstanceTestData))]
		public void CreateInstance(Type type, int parameterCount)
		{
			// get method to call
			MethodInfo method = typeof(FastActivator)
				.GetMethods()
				.Single(mi => mi.Name == "CreateInstance" && mi.GetParameters().Length == parameterCount + 1);
			if (parameterCount > 0)
			{
				Type[] parameterTypes = new Type[parameterCount];
				for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);
				method = method.MakeGenericMethod(parameterTypes);
			}

			if (parameterCount == 0)
			{
				// call method
				object obj = method.Invoke(null, new object[] { type });
				Assert.IsType(type, obj);
			}
			else
			{
				for (int i = 0; i < parameterCount; i++)
				{
					// call method
					object[] parameters = new object[1 + parameterCount];
					parameters[0] = type;
					for (int j = 1; j < parameterCount + 1; j++) parameters[j] = i + 1 == j ? 1 : 0;
					object obj = method.Invoke(null, parameters);

					// check result
					Assert.IsType(type, obj);
					ITestData<int> data = (ITestData<int>)obj;
					for (int j = 0; j < parameterCount; j++)
					{
						Assert.Equal(parameters[j + 1], data.Values[j]);
					}
				}
			}
		}

		#endregion

		#region CreateInstanceDynamically()

		/// <summary>
		/// Checks whether <see cref="FastActivator.CreateInstanceDynamically"/> works as expected.
		/// </summary>
		[Theory]
		[MemberData(nameof(CreateInstanceTestData))]
		public void CreateInstanceDynamically(Type type, int parameterCount)
		{
			// get method to call
			Type[] parameterTypes = new Type[parameterCount];
			for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);

			if (parameterCount == 0)
			{
				// call method
				object obj = FastActivator.CreateInstanceDynamically(type, parameterTypes, Array.Empty<object>());
				Assert.IsType(type, obj);
			}
			else
			{
				for (int i = 0; i < parameterCount; i++)
				{
					// call method
					object[] arguments = new object[parameterCount];
					for (int j = 0; j < parameterCount; j++) arguments[j] = i == j ? 1 : 0;
					object obj = FastActivator.CreateInstanceDynamically(type, parameterTypes, arguments);

					// check result
					Assert.IsType(type, obj);
					ITestData<int> data = (ITestData<int>)obj;
					for (int j = 0; j < parameterCount; j++)
					{
						Assert.Equal(arguments[j], data.Values[j]);
					}
				}
			}
		}

		#endregion

		#region IsCreatable()

		/// <summary>
		/// Gets test data for test methods targeting the CreateInstance() overloads.
		/// </summary>
		public static IEnumerable<object[]> IsCreatableTestData
		{
			get
			{
				// constructor exists
				yield return new object[] { typeof(TestStruct<int>), new Type[] { }, true }; // implicit constructor
				yield return new object[] { typeof(TestStruct<int>), new[] { typeof(int) }, true };
				yield return new object[] { typeof(TestStruct<int>), new[] { typeof(int), typeof(int) }, true };
				yield return new object[] { typeof(TestClass<int>), new Type[] { }, true }; // explicit constructor
				yield return new object[] { typeof(TestClass<int>), new[] { typeof(int) }, true };
				yield return new object[] { typeof(TestClass<int>), new[] { typeof(int), typeof(int) }, true };

				// constructor does not exist
				yield return new object[] { typeof(TestStruct<int>), new[] { typeof(string) }, false };
				yield return new object[] { typeof(TestStruct<int>), new[] { typeof(string), typeof(string) }, false };
				yield return new object[] { typeof(TestClass<int>), new[] { typeof(string) }, false };
				yield return new object[] { typeof(TestClass<int>), new[] { typeof(string), typeof(string) }, false };
			}
		}

		/// <summary>
		/// Checks whether <see cref="FastActivator.IsCreatable"/> works as expected.
		/// </summary>
		[Theory]
		[MemberData(nameof(IsCreatableTestData))]
		public void IsCreatable(Type type, Type[] constructorParameters, bool expected)
		{
			bool actual = FastActivator.IsCreatable(type, constructorParameters);
			Assert.Equal(expected, actual);
		}

		#endregion
	}

}
