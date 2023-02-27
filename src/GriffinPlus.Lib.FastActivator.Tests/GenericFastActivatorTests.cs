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
	/// Tests around the <see cref="FastActivator{T}"/> class.
	/// </summary>
	public class GenericFastActivatorTests
	{
		#region CreateArray()

		/// <summary>
		/// Gets test data for testing the <see cref="FastActivator{T}.CreateArray"/> method.
		/// </summary>
		public static IEnumerable<object[]> CreateArrayTestData
		{
			get
			{
				foreach (Type type in new[] { typeof(TestStruct<int>), typeof(TestClass<int>) })
				{
					yield return new object[] { type, 0 };
					yield return new object[] { type, 1 };
					yield return new object[] { type, 5 };
				}
			}
		}

		/// <summary>
		/// Checks whether <see cref="FastActivator{T}.CreateArray"/> works as expected.
		/// </summary>
		[Theory]
		[MemberData(nameof(CreateArrayTestData))]
		public void CreateArray(Type type, int count)
		{
			var expected = Array.CreateInstance(type, count);
			MethodInfo method = typeof(FastActivator<>).MakeGenericType(type).GetMethod("CreateArray");
			Assert.NotNull(method);
			var actual = (Array)method.Invoke(null, new object[] { count });
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
				foreach (Type type in new[] { typeof(TestStruct<int>), typeof(TestClass<int>) })
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
		/// - <see cref="FastActivator{T}.CreateInstance"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12,TArg13}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12,TArg13,TArg14}"/><br/>
		/// - <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12,TArg13,TArg14,TArg15}"/><br/>
		/// -
		/// <see cref="FastActivator{T}.CreateInstance{TArg1,TArg2,TArg3,TArg4,TArg5,TArg6,TArg7,TArg8,TArg9,TArg10,TArg11,TArg12,TArg13,TArg14,TArg15,TArg16}"/>
		/// </summary>
		[Theory]
		[MemberData(nameof(CreateInstanceTestData))]
		public void CreateInstance(Type type, int parameterCount)
		{
			// get method to call
			MethodInfo method = typeof(FastActivator<>)
				.MakeGenericType(type)
				.GetMethods()
				.Single(mi => mi.Name == "CreateInstance" && mi.GetParameters().Length == parameterCount);
			if (parameterCount > 0)
			{
				var parameterTypes = new Type[parameterCount];
				for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);
				method = method.MakeGenericMethod(parameterTypes);
			}

			if (parameterCount == 0)
			{
				// call method
				object obj = method.Invoke(null, Array.Empty<object>());
				Assert.IsType(type, obj);
			}
			else
			{
				for (int i = 0; i < parameterCount; i++)
				{
					// call method
					object[] parameters = new object[parameterCount];
					for (int j = 0; j < parameterCount; j++) parameters[j] = i == j ? 1 : 0;
					object obj = method.Invoke(null, parameters);

					// check result
					Assert.IsType(type, obj);
					var data = (ITestData<int>)obj;
					for (int j = 0; j < parameterCount; j++)
					{
						Assert.Equal(parameters[j], data.Values[j]);
					}
				}
			}
		}

		#endregion
	}

}
