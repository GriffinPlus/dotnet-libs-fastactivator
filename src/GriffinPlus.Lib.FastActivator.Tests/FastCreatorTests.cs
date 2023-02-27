///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Xunit;

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Tests targeting the <see cref="FastCreator{TCreator}"/> class.
	/// </summary>
	public class FastCreatorTests
	{
		/// <summary>
		/// Gets test data for tests targeting the <see cref="FastCreator{TCreator}.GetCreator"/> method.
		/// </summary>
		public static IEnumerable<object[]> GetCreatorTestData
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
		/// Checks whether the <see cref="FastCreator{TCreator}.GetCreator"/> method works as expected.
		/// </summary>
		[Theory]
		[MemberData(nameof(GetCreatorTestData))]
		public void GetCreator(Type type, int parameterCount)
		{
			// get the creator delegate
			var parameterTypes = new Type[parameterCount + 1];
			for (int j = 0; j < parameterCount; j++) parameterTypes[j] = typeof(int);
			parameterTypes[parameterCount] = typeof(object);
			Type delegateType = Expression.GetDelegateType(parameterTypes);
			Type fastCreatorType = typeof(FastCreator<>).MakeGenericType(delegateType);
			MethodInfo getCreatorMethod = fastCreatorType.GetMethod("GetCreator");
			Assert.NotNull(getCreatorMethod);
			var getCreatorDelegate = (Delegate)getCreatorMethod.Invoke(null, new object[] { type });

			// create instances of the type and check out whether arguments are properly passed in
			for (int i = 0; i < parameterCount; i++)
			{
				// call method
				object[] parameters = new object[parameterCount];
				for (int j = 0; j < parameterCount; j++) parameters[j] = i == j ? 1 : 0;
				object obj = getCreatorDelegate.DynamicInvoke(parameters);

				// check result
				Assert.IsType(type, obj);
				var testClass = (ITestData<int>)obj;
				for (int j = 0; j < parameterCount; j++)
				{
					Assert.Equal(parameters[j], testClass.Values[j]);
				}
			}
		}
	}

}
