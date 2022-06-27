///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Reflection;

using Xunit;

namespace GriffinPlus.Lib
{

	public class NonGenericFastActivatorTests_Fixture
	{
		public MethodInfo[] UntypedResultCreateInstanceMethodInfos;

		public NonGenericFastActivatorTests_Fixture()
		{
			UntypedResultCreateInstanceMethodInfos = typeof(FastActivator)
				.GetMethods()
				.Where(mi => mi.Name == "CreateInstance" && mi.ReturnType == typeof(object))
				.OrderBy(mi => mi.GetParameters().Length)
				.ToArray();

			Assert.Equal(17, UntypedResultCreateInstanceMethodInfos.Length);
		}
	}

	/// <summary>
	/// Tests around the <see cref="FastActivator"/> class.
	/// </summary>
	public class NonGenericFastActivatorTests : IClassFixture<NonGenericFastActivatorTests_Fixture>
	{
		#region Member Variables

		private readonly NonGenericFastActivatorTests_Fixture mFixture;

		#endregion

		#region Initialization

		/// <summary>
		/// Initializes a new instance of the <see cref="NonGenericFastActivatorTests"/> class.
		/// </summary>
		/// <param name="fixture">Fixture containing common test data.</param>
		public NonGenericFastActivatorTests(NonGenericFastActivatorTests_Fixture fixture)
		{
			mFixture = fixture;
		}

		#endregion

		#region Testing Creators Returning Object

		/// <summary>
		/// Checks whether the creation method taking no parameters returning a simple object is working (for structs).
		/// </summary>
		[Fact]
		public void CreateInstance_Struct_Without_Arguments_Returning_Object()
		{
			object obj = FastActivator.CreateInstance(typeof(TestStruct<int>));
			Assert.IsType<TestStruct<int>>(obj);
			TestStruct<int> testStruct = (TestStruct<int>)obj;
			Assert.Null(testStruct.Values);
		}

		/// <summary>
		/// Checks whether the creation method taking no parameters returning a simple object is working (for classes).
		/// </summary>
		[Fact]
		public void CreateInstance_Class_Without_Arguments_Returning_Object()
		{
			object obj = FastActivator.CreateInstance(typeof(TestClass<int>));
			Assert.IsType<TestClass<int>>(obj);
			TestClass<int> testClass = (TestClass<int>)obj;
			Assert.Null(testClass.Values);
		}

		/// <summary>
		/// Checks whether the creation methods taking 1 to 16 arguments returning a simple object pass constructor arguments properly (for structs).
		/// </summary>
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		[InlineData(9)]
		[InlineData(10)]
		[InlineData(11)]
		[InlineData(12)]
		[InlineData(13)]
		[InlineData(14)]
		[InlineData(15)]
		[InlineData(16)]
		public void CreateInstance_Struct_With_Arguments_Returning_Object(int parameterCount)
		{
			// get method to call
			Type[] parameterTypes = new Type[parameterCount];
			for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);
			MethodInfo method = mFixture.UntypedResultCreateInstanceMethodInfos[parameterCount].MakeGenericMethod(parameterTypes);

			for (int i = 0; i < parameterCount; i++)
			{
				// call method
				object[] parameters = new object[1 + parameterCount];
				parameters[0] = typeof(TestStruct<int>);
				for (int j = 1; j < parameterCount + 1; j++) parameters[j] = i + 1 == j ? 1 : 0;
				object obj = method.Invoke(null, parameters);

				// check result
				Assert.IsType<TestStruct<int>>(obj);
				TestStruct<int> testStruct = (TestStruct<int>)obj;
				for (int j = 0; j < parameterCount; j++)
				{
					Assert.Equal(parameters[j + 1], testStruct.Values[j]);
				}
			}
		}

		/// <summary>
		/// Checks whether the creation methods taking 1 to 16 arguments returning a simple object pass constructor arguments properly (for classes).
		/// </summary>
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		[InlineData(9)]
		[InlineData(10)]
		[InlineData(11)]
		[InlineData(12)]
		[InlineData(13)]
		[InlineData(14)]
		[InlineData(15)]
		[InlineData(16)]
		public void CreateInstance_Class_With_Arguments_Returning_Object(int parameterCount)
		{
			// get method to call
			Type[] parameterTypes = new Type[parameterCount];
			for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);
			MethodInfo method = mFixture.UntypedResultCreateInstanceMethodInfos[parameterCount].MakeGenericMethod(parameterTypes);

			for (int i = 0; i < parameterCount; i++)
			{
				// call method
				object[] parameters = new object[1 + parameterCount];
				parameters[0] = typeof(TestClass<int>);
				for (int j = 1; j < parameterCount + 1; j++) parameters[j] = i + 1 == j ? 1 : 0;
				object obj = method.Invoke(null, parameters);

				// check result
				Assert.IsType<TestClass<int>>(obj);
				TestClass<int> testStruct = (TestClass<int>)obj;
				for (int j = 0; j < parameterCount; j++)
				{
					Assert.Equal(parameters[j + 1], testStruct.Values[j]);
				}
			}
		}

		#endregion

		#region Testing Creators Returning specific Type

		/// <summary>
		/// Checks whether the creation method taking no parameters returning a typed object is working (for structs).
		/// </summary>
		[Fact]
		public void CreateInstance_Struct_Without_Arguments_Returning_Specific_Type()
		{
			TestStruct<int> testStruct = (TestStruct<int>)FastActivator.CreateInstance(typeof(TestStruct<int>));
			Assert.Null(testStruct.Values);
		}

		/// <summary>
		/// Checks whether the creation method taking no parameters returning a typed object is working (for classes).
		/// </summary>
		[Fact]
		public void CreateInstance_Class_Without_Arguments_Returning_Specific_Type()
		{
			TestClass<int> testClass = (TestClass<int>)FastActivator.CreateInstance(typeof(TestClass<int>));
			Assert.Null(testClass.Values);
		}

		#endregion

		#region Testing Array Creators

		/// <summary>
		/// Checks whether the array creation method returning a simple array is working (for structs).
		/// </summary>
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(5)]
		public void CreateArray_Struct(int count)
		{
			TestStruct<int>[] expected = new TestStruct<int>[count];
			TestStruct<int>[] actual = (TestStruct<int>[])FastActivator.CreateArray(typeof(TestStruct<int>), count);
			Assert.Equal(expected, actual);
		}

		/// <summary>
		/// Checks whether the array creation method returning a simple array is working (for classes).
		/// </summary>
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(5)]
		public void CreateArray_Class(int count)
		{
			TestClass<int>[] expected = new TestClass<int>[count];
			TestClass<int>[] actual = (TestClass<int>[])FastActivator.CreateArray(typeof(TestClass<int>), count);
			Assert.Equal(expected, actual);
		}

		#endregion
	}

}
