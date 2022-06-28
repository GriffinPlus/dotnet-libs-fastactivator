///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

using Xunit;

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Tests targeting the <see cref="FastCreator{TCreator}"/> class.
	/// </summary>
	public class FastCreatorTests
	{
		/// <summary>
		/// Checks whether the creation method taking no parameters returning a simple object is working (for structs).
		/// </summary>
		[Fact]
		public void GetCreator_Struct_Without_Arguments()
		{
			TestStruct<int> testStruct = (TestStruct<int>)FastCreator<Func<object>>.GetCreator(typeof(TestStruct<int>))();
			Assert.Null(testStruct.Values);
		}

		/// <summary>
		/// Checks whether the creation method taking no parameters returning a simple object is working (for classes).
		/// </summary>
		[Fact]
		public void GetCreator_Class_Without_Arguments()
		{
			TestClass<int> testClass = (TestClass<int>)FastCreator<Func<object>>.GetCreator(typeof(TestClass<int>))();
			Assert.Null(testClass.Values);
		}

		/// <summary>
		/// Gets test data for test methods taking the number of parameters to test with (1..16).
		/// </summary>
		public static IEnumerable<object[]> ParameterCountTestData
		{
			get
			{
				for (int i = 1; i <= 16; i++)
				{
					yield return new object[] { i };
				}
			}
		}

		/// <summary>
		/// Checks whether the creation methods taking 1 to 16 arguments returning a typed object pass constructor arguments properly (for structs).
		/// </summary>
		[Theory]
		[MemberData(nameof(ParameterCountTestData))]
		public void GetCreator_Struct_With_Arguments(int parameterCount)
		{
			// get the creator delegate
			Type[] parameterTypes = new Type[parameterCount + 1];
			for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);
			parameterTypes[parameterCount] = typeof(object);
			var delegateType = Expression.GetDelegateType(parameterTypes);
			var fastCreatorType = typeof(FastCreator<>).MakeGenericType(delegateType);
			var getCreatorMethod = fastCreatorType.GetMethod("GetCreator");
			Debug.Assert(getCreatorMethod != null, nameof(getCreatorMethod) + " != null");
			var getCreatorDelegate = (Delegate)getCreatorMethod.Invoke(null, new object[] { typeof(TestStruct<int>) });

			// create instances of the type and check out whether arguments are properly passed in
			for (int i = 0; i < parameterCount; i++)
			{
				// call method
				object[] parameters = new object[parameterCount];
				for (int j = 0; j < parameterCount; j++) parameters[j] = i == j ? 1 : 0;
				var obj = getCreatorDelegate.DynamicInvoke(parameters);

				// check result
				Assert.IsType<TestStruct<int>>(obj);
				TestStruct<int> testClass = (TestStruct<int>)obj;
				for (int j = 0; j < parameterCount; j++)
				{
					Assert.Equal(parameters[j], testClass.Values[j]);
				}
			}
		}

		/// <summary>
		/// Checks whether the creation methods taking 1 to 16 arguments returning a typed object pass constructor arguments properly (for classes).
		/// </summary>
		[Theory]
		[MemberData(nameof(ParameterCountTestData))]
		public void GetCreator_Class_With_Arguments(int parameterCount)
		{
			// get the creator delegate
			Type[] parameterTypes = new Type[parameterCount + 1];
			for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);
			parameterTypes[parameterCount] = typeof(object);
			var delegateType = Expression.GetDelegateType(parameterTypes);
			var fastCreatorType = typeof(FastCreator<>).MakeGenericType(delegateType);
			var getCreatorMethod = fastCreatorType.GetMethod("GetCreator");
			Debug.Assert(getCreatorMethod != null, nameof(getCreatorMethod) + " != null");
			var getCreatorDelegate = (Delegate)getCreatorMethod.Invoke(null, new object[] { typeof(TestClass<int>) });

			// create instances of the type and check out whether arguments are properly passed in
			for (int i = 0; i < parameterCount; i++)
			{
				// call method
				object[] parameters = new object[parameterCount];
				for (int j = 0; j < parameterCount; j++) parameters[j] = i == j ? 1 : 0;
				var obj = getCreatorDelegate.DynamicInvoke(parameters);

				// check result
				Assert.IsType<TestClass<int>>(obj);
				TestClass<int> testClass = (TestClass<int>)obj;
				for (int j = 0; j < parameterCount; j++)
				{
					Assert.Equal(parameters[j], testClass.Values[j]);
				}
			}
		}
	}

}
