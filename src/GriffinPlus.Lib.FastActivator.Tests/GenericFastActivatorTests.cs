///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using GriffinPlus.Lib;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace UnitTests
{
	public class GenericFastActivatorTests_Fixture
	{
		public MethodInfo[] TypedResultCreateInstanceMethodInfos_TestClass;
		public MethodInfo[] TypedResultCreateInstanceMethodInfos_TestStruct;

		public GenericFastActivatorTests_Fixture()
		{
			TypedResultCreateInstanceMethodInfos_TestClass = typeof(FastActivator<TestClass<int>>)
				.GetMethods()
				.Where(mi => mi.Name == "CreateInstance")
				.OrderBy(mi => mi.GetParameters().Length)
				.ToArray();

			TypedResultCreateInstanceMethodInfos_TestStruct = typeof(FastActivator<TestStruct<int>>)
				.GetMethods()
				.Where(mi => mi.Name == "CreateInstance")
				.OrderBy(mi => mi.GetParameters().Length)
				.ToArray();

			Assert.Equal(17, TypedResultCreateInstanceMethodInfos_TestClass.Length);
			Assert.Equal(17, TypedResultCreateInstanceMethodInfos_TestStruct.Length);
		}
	}

	/// <summary>
	/// Tests around the <see cref="FastActivator{T}"/> class.
	/// </summary>
	public class GenericFastActivatorTests : IClassFixture<GenericFastActivatorTests_Fixture>
	{
		private readonly GenericFastActivatorTests_Fixture mFixture;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericFastActivatorTests"/> class.
		/// </summary>
		/// <param name="fixture">Fixture containing common test data.</param>
		public GenericFastActivatorTests(GenericFastActivatorTests_Fixture fixture)
		{
			mFixture = fixture;
		}

		/// <summary>
		/// Checks whether the creation method taking no parameters returning a simple object is working (for structs).
		/// </summary>
		[Fact]
		public void CreateInstance_Struct_Without_Arguments_Returning_Specific_Type()
		{
			TestStruct<int> testStruct = FastActivator<TestStruct<int>>.CreateInstance();
			Assert.Null(testStruct.Values);
		}

		/// <summary>
		/// Checks whether the creation method taking no parameters returning a simple object is working (for classes).
		/// </summary>
		[Fact]
		public void CreateInstance_Class_Without_Arguments_Returning_Specific_Type()
		{
			TestClass<int> testClass = FastActivator<TestClass<int>>.CreateInstance();
			Assert.Null(testClass.Values);
		}

		/// <summary>
		/// Checks whether the creation methods taking 1 to 16 arguments returning a typed object pass constructor arguments properly (for structs).
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
		public void CreateInstance_Struct_With_Arguments_Returning_Specific_Type(int parameterCount)
		{
			// get method to call
			Type[] parameterTypes = new Type[parameterCount];
			for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);
			MethodInfo method = mFixture.TypedResultCreateInstanceMethodInfos_TestStruct[parameterCount].MakeGenericMethod(parameterTypes);

			for (int i = 0; i < parameterCount; i++)
			{
				// call method
				object[] parameters = new object[parameterCount];
				for (int j = 0; j < parameterCount; j++) parameters[j] = i == j ? 1 : 0;
				object obj = method.Invoke(null, parameters);

				// check result
				Assert.IsType<TestStruct<int>>(obj);
				TestStruct<int> testStruct = (TestStruct<int>)obj;
				for (int j = 0; j < parameterCount; j++) {
					Assert.Equal(parameters[j], testStruct.Values[j]);
				}
			}
		}

		/// <summary>
		/// Checks whether the creation methods taking 1 to 16 arguments returning a typed object pass constructor arguments properly (for classes).
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
		public void CreateInstance_Class_With_Arguments_Returning_Specific_Type(int parameterCount)
		{
			// get method to call
			Type[] parameterTypes = new Type[parameterCount];
			for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);
			MethodInfo method = mFixture.TypedResultCreateInstanceMethodInfos_TestClass[parameterCount].MakeGenericMethod(parameterTypes);

			for (int i = 0; i < parameterCount; i++)
			{
				// call method
				object[] parameters = new object[parameterCount];
				for (int j = 0; j < parameterCount; j++) parameters[j] = i == j ? 1 : 0;
				object obj = method.Invoke(null, parameters);

				// check result
				Assert.IsType<TestClass<int>>(obj);
				TestClass<int> testClass = (TestClass<int>)obj;
				for (int j = 0; j < parameterCount; j++) {
					Assert.Equal(parameters[j], testClass.Values[j]);
				}
			}
		}

	}
}
