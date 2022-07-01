///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable CoVariantArrayConversion

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Creates instances of a certain class using dynamically compiled creation methods
	/// (Non-generic version providing best performance gain for reference types).
	/// </summary>
	public static class FastActivator
	{
		private delegate Array ArrayCreatorDelegate(int length);

		private static readonly object sSync = new object();

		#region IsCreatable(...)

		/// <summary>
		/// Checks whether the specified type can be created using the specified specified parameters.
		/// </summary>
		/// <param name="type">Type to check.</param>
		/// <param name="constructorParameterTypes">Types of constructor parameters to check for.</param>
		/// <returns>
		/// true, if the specified type has a constructor that takes the specified parameters;
		/// otherwise false.
		/// </returns>
		public static bool IsCreatable(Type type, Type[] constructorParameterTypes)
		{
			// convert constructor parameters into the corresponding creator function type
			Type creatorType = sObjectResultCreatorFuncMap.Get(constructorParameterTypes);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// check whether the specified constructor is available
			return creatorsByCreatorType.TryGetValue(creatorType, out _);
		}

		#endregion

		#region CreateInstance(...)

		/// <summary>
		/// Creates an instance of the specified type using its default constructor.
		/// </summary>
		/// <param name="type">Type of the object to create.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance(Type type)
		{
			return FastCreator<Func<object>>
				.GetCreator(type)
				.Invoke();
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg">Type of the constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg">The constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg>(Type type, TArg arg)
		{
			return FastCreator<Func<TArg, object>>
				.GetCreator(type)
				.Invoke(arg);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2>(Type type, TArg1 arg1, TArg2 arg2)
		{
			return FastCreator<Func<TArg1, TArg2, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3>(
			Type  type,
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4>(
			Type  type,
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5>(
			Type  type,
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
			Type  type,
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5,
			TArg6 arg6)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
			Type  type,
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5,
			TArg6 arg6,
			TArg7 arg7)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
			Type  type,
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5,
			TArg6 arg6,
			TArg7 arg7,
			TArg8 arg8)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TArg9">Type of the ninth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <param name="arg9">The ninth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
			Type  type,
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5,
			TArg6 arg6,
			TArg7 arg7,
			TArg8 arg8,
			TArg9 arg9)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TArg9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TArg10">Type of the tenth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <param name="arg9">The ninth constructor argument.</param>
		/// <param name="arg10">The tenth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(
			Type   type,
			TArg1  arg1,
			TArg2  arg2,
			TArg3  arg3,
			TArg4  arg4,
			TArg5  arg5,
			TArg6  arg6,
			TArg7  arg7,
			TArg8  arg8,
			TArg9  arg9,
			TArg10 arg10)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TArg9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TArg10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TArg11">Type of the eleventh constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <param name="arg9">The ninth constructor argument.</param>
		/// <param name="arg10">The tenth constructor argument.</param>
		/// <param name="arg11">The eleventh constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(
			Type   type,
			TArg1  arg1,
			TArg2  arg2,
			TArg3  arg3,
			TArg4  arg4,
			TArg5  arg5,
			TArg6  arg6,
			TArg7  arg7,
			TArg8  arg8,
			TArg9  arg9,
			TArg10 arg10,
			TArg11 arg11)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TArg9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TArg10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TArg11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TArg12">Type of the twelfth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <param name="arg9">The ninth constructor argument.</param>
		/// <param name="arg10">The tenth constructor argument.</param>
		/// <param name="arg11">The eleventh constructor argument.</param>
		/// <param name="arg12">The twelfth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(
			Type   type,
			TArg1  arg1,
			TArg2  arg2,
			TArg3  arg3,
			TArg4  arg4,
			TArg5  arg5,
			TArg6  arg6,
			TArg7  arg7,
			TArg8  arg8,
			TArg9  arg9,
			TArg10 arg10,
			TArg11 arg11,
			TArg12 arg12)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TArg9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TArg10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TArg11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TArg12">Type of the twelfth constructor argument.</typeparam>
		/// <typeparam name="TArg13">Type of the thirteenth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <param name="arg9">The ninth constructor argument.</param>
		/// <param name="arg10">The tenth constructor argument.</param>
		/// <param name="arg11">The eleventh constructor argument.</param>
		/// <param name="arg12">The twelfth constructor argument.</param>
		/// <param name="arg13">The thirteenth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(
			Type   type,
			TArg1  arg1,
			TArg2  arg2,
			TArg3  arg3,
			TArg4  arg4,
			TArg5  arg5,
			TArg6  arg6,
			TArg7  arg7,
			TArg8  arg8,
			TArg9  arg9,
			TArg10 arg10,
			TArg11 arg11,
			TArg12 arg12,
			TArg13 arg13)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TArg9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TArg10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TArg11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TArg12">Type of the twelfth constructor argument.</typeparam>
		/// <typeparam name="TArg13">Type of the thirteenth constructor argument.</typeparam>
		/// <typeparam name="TArg14">Type of the fourteenth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <param name="arg9">The ninth constructor argument.</param>
		/// <param name="arg10">The tenth constructor argument.</param>
		/// <param name="arg11">The eleventh constructor argument.</param>
		/// <param name="arg12">The twelfth constructor argument.</param>
		/// <param name="arg13">The thirteenth constructor argument.</param>
		/// <param name="arg14">The fourteenth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(
			Type   type,
			TArg1  arg1,
			TArg2  arg2,
			TArg3  arg3,
			TArg4  arg4,
			TArg5  arg5,
			TArg6  arg6,
			TArg7  arg7,
			TArg8  arg8,
			TArg9  arg9,
			TArg10 arg10,
			TArg11 arg11,
			TArg12 arg12,
			TArg13 arg13,
			TArg14 arg14)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TArg9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TArg10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TArg11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TArg12">Type of the twelfth constructor argument.</typeparam>
		/// <typeparam name="TArg13">Type of the thirteenth constructor argument.</typeparam>
		/// <typeparam name="TArg14">Type of the fourteenth constructor argument.</typeparam>
		/// <typeparam name="TArg15">Type of the fifteenth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <param name="arg9">The ninth constructor argument.</param>
		/// <param name="arg10">The tenth constructor argument.</param>
		/// <param name="arg11">The eleventh constructor argument.</param>
		/// <param name="arg12">The twelfth constructor argument.</param>
		/// <param name="arg13">The thirteenth constructor argument.</param>
		/// <param name="arg14">The fourteenth constructor argument.</param>
		/// <param name="arg15">The fifteenth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(
			Type   type,
			TArg1  arg1,
			TArg2  arg2,
			TArg3  arg3,
			TArg4  arg4,
			TArg5  arg5,
			TArg6  arg6,
			TArg7  arg7,
			TArg8  arg8,
			TArg9  arg9,
			TArg10 arg10,
			TArg11 arg11,
			TArg12 arg12,
			TArg13 arg13,
			TArg14 arg14,
			TArg15 arg15)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TArg9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TArg10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TArg11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TArg12">Type of the twelfth constructor argument.</typeparam>
		/// <typeparam name="TArg13">Type of the thirteenth constructor argument.</typeparam>
		/// <typeparam name="TArg14">Type of the fourteenth constructor argument.</typeparam>
		/// <typeparam name="TArg15">Type of the fifteenth constructor argument.</typeparam>
		/// <typeparam name="TArg16">Type of the sixteenth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <param name="arg9">The ninth constructor argument.</param>
		/// <param name="arg10">The tenth constructor argument.</param>
		/// <param name="arg11">The eleventh constructor argument.</param>
		/// <param name="arg12">The twelfth constructor argument.</param>
		/// <param name="arg13">The thirteenth constructor argument.</param>
		/// <param name="arg14">The fourteenth constructor argument.</param>
		/// <param name="arg15">The fifteenth constructor argument.</param>
		/// <param name="arg16">The sixteenth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(
			Type   type,
			TArg1  arg1,
			TArg2  arg2,
			TArg3  arg3,
			TArg4  arg4,
			TArg5  arg5,
			TArg6  arg6,
			TArg7  arg7,
			TArg8  arg8,
			TArg9  arg9,
			TArg10 arg10,
			TArg11 arg11,
			TArg12 arg12,
			TArg13 arg13,
			TArg14 arg14,
			TArg15 arg15,
			TArg16 arg16)
		{
			return FastCreator<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, object>>
				.GetCreator(type)
				.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
		}

		#endregion

		#region CreateInstanceDynamically(...)

		private static          TypeKeyedDictionary<TypeKeyedDictionary<Delegate>> sCreatorsByCreatorTypeMap   = new TypeKeyedDictionary<TypeKeyedDictionary<Delegate>>();
		private static readonly FastActivatorFuncTypeMap                           sObjectResultCreatorFuncMap = new FastActivatorFuncTypeMap();

		/// <summary>
		/// Creates an instance of the specified type.
		/// </summary>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="constructorParameterTypes">Parameters of the constructor to call (up to 16 types; may be <c>null</c> for parameterless constructors).</param>
		/// <param name="args">Arguments to pass to the constructor.</param>
		/// <returns>An instance of the specified type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">
		/// The <paramref name="args"/> do not contain the same number of objects as <paramref name="constructorParameterTypes"/> contains types.
		/// </exception>
		/// <remarks>
		/// Due to use of reflection this method is rather expensive.
		/// Please use the generic implementations of the <see cref="CreateInstance"/> method instead, if possible.
		/// </remarks>
		public static object CreateInstanceDynamically(Type type, Type[] constructorParameterTypes, params object[] args)
		{
			// check arguments
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (constructorParameterTypes != null)
			{
				int constructorParameterTypeCount = constructorParameterTypes.Length;
				if (constructorParameterTypeCount > 0)
				{
					if (constructorParameterTypeCount > 16)
						throw new ArgumentException("This method does not support constructors with more than 16 arguments.");

					if (args == null || args.Length != constructorParameterTypeCount)
						throw new ArgumentException("The number of constructor arguments does not match the the number of constructor parameter types.");
				}
			}

			Type creatorType = sObjectResultCreatorFuncMap.Get(constructorParameterTypes);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return CallCreatorDynamically(creator, args);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
		}

		/// <summary>
		/// Calls the specified creator delegate dynamically passing the specified arguments to the constructor.
		/// </summary>
		/// <param name="creator">Creator delegate to call.</param>
		/// <param name="args">Arguments to pass to the constructor of the object to create.</param>
		/// <returns>An object created by the creator delegate.</returns>
		private static object CallCreatorDynamically(dynamic creator, dynamic[] args)
		{
			if (args == null) return creator();

			switch (args.Length)
			{
				case 0:  return creator();
				case 1:  return creator(args[0]);
				case 2:  return creator(args[0], args[1]);
				case 3:  return creator(args[0], args[1], args[2]);
				case 4:  return creator(args[0], args[1], args[2], args[3]);
				case 5:  return creator(args[0], args[1], args[2], args[3], args[4]);
				case 6:  return creator(args[0], args[1], args[2], args[3], args[4], args[5]);
				case 7:  return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
				case 8:  return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
				case 9:  return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
				case 10: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
				case 11: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
				case 12: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
				case 13: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
				case 14: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
				case 15: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
				case 16: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
				default: return null;
			}
		}

		/// <summary>
		/// Initializes the creator cache for the specified type.
		/// </summary>
		/// <param name="type">Type to generate creators for.</param>
		private static void InitCreatorCache(Type type)
		{
			lock (sSync)
			{
				// abort, if the type was already processed
				if (sCreatorsByCreatorTypeMap.ContainsKey(type))
					return;

				TypeKeyedDictionary<Delegate> creatorTypeToCreatorMap = new TypeKeyedDictionary<Delegate>();

				// add default constructor, if the type is a value type
				// (the default constructor will not occur in the enumeration below...)
				if (type.IsValueType)
				{
					// get creator and cache it
					Type creatorType = sObjectResultCreatorFuncMap.Get(Type.EmptyTypes);
					Type fastCreatorType = typeof(FastCreator<>).MakeGenericType(creatorType);
					MethodInfo getCreatorMethod = fastCreatorType.GetMethod("GetCreator");
					Debug.Assert(getCreatorMethod != null, nameof(getCreatorMethod) + " != null");
					Delegate creator = (Delegate)getCreatorMethod.Invoke(null, new object[] { type });
					creatorTypeToCreatorMap.Add(creatorType, creator);
				}

				// add create for all constructors
				foreach (var constructor in type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					var constructorParameterTypes = constructor.GetParameters().Select(x => x.ParameterType).ToArray();
					if (constructorParameterTypes.Length > 16) continue;

					// get creator and cache it
					Type creatorType = sObjectResultCreatorFuncMap.Get(constructorParameterTypes);
					Type fastCreatorType = typeof(FastCreator<>).MakeGenericType(creatorType);
					MethodInfo getCreatorMethod = fastCreatorType.GetMethod("GetCreator");
					Debug.Assert(getCreatorMethod != null, nameof(getCreatorMethod) + " != null");
					Delegate creator = (Delegate)getCreatorMethod.Invoke(null, new object[] { type });
					creatorTypeToCreatorMap.Add(creatorType, creator);
				}

				TypeKeyedDictionary<TypeKeyedDictionary<Delegate>> creatorsByCreatorTypeMap = new TypeKeyedDictionary<TypeKeyedDictionary<Delegate>>(sCreatorsByCreatorTypeMap);
				creatorsByCreatorTypeMap.Add(type, creatorTypeToCreatorMap);
				Thread.MemoryBarrier(); // ensures everything up to this point has been actually written to memory
				sCreatorsByCreatorTypeMap = creatorsByCreatorTypeMap;
			}
		}

		#endregion

		#region CreateArray(...)

		private static TypeKeyedDictionary<ArrayCreatorDelegate> sArrayCreatorsByTypeMap = new TypeKeyedDictionary<ArrayCreatorDelegate>();

		/// <summary>
		/// Creates an one-dimensional array with zero-based indexing of the specified type.
		/// </summary>
		/// <param name="type">Type of the array's elements.</param>
		/// <param name="length">Length of the array to create.</param>
		/// <returns>An array of the <paramref name="type"/> the specified length.</returns>
		public static Array CreateArray(Type type, int length)
		{
			// query the creator cache
			if (!sArrayCreatorsByTypeMap.TryGetValue(type, out var creator))
			{
				InitArrayCreatorCache(type);
				creator = sArrayCreatorsByTypeMap[type];
			}

			return creator(length);
		}

		/// <summary>
		/// Initializes the array creator cache for the specified type.
		/// </summary>
		/// <param name="type">Type to generate creators for.</param>
		private static void InitArrayCreatorCache(Type type)
		{
			lock (sSync)
			{
				// abort, if the type was already processed
				if (sArrayCreatorsByTypeMap.ContainsKey(type))
					return;

				Type[] parameterTypes = { typeof(int) };
				ParameterExpression[] parameterExpressions = parameterTypes.Select(Expression.Parameter).ToArray();
				LambdaExpression lambda = Expression.Lambda(
					typeof(ArrayCreatorDelegate),
					Expression.NewArrayBounds(type, parameterExpressions),
					parameterExpressions);
				var creator = (ArrayCreatorDelegate)lambda.Compile();

				TypeKeyedDictionary<ArrayCreatorDelegate> arrayCreatorsByTypeMap = new TypeKeyedDictionary<ArrayCreatorDelegate>(sArrayCreatorsByTypeMap) { { type, creator } };
				Thread.MemoryBarrier(); // ensures everything up to this point has been actually written to memory
				sArrayCreatorsByTypeMap = arrayCreatorsByTypeMap;
			}
		}

		#endregion
	}

}
