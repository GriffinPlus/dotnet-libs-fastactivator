///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
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
		/// <c>true</c> if <paramref name="type"/>has a constructor that takes the specified parameters;
		/// otherwise <c>false</c>.
		/// </returns>
		public static bool IsCreatable(Type type, Type[] constructorParameterTypes)
		{
			return CreateInstanceDynamically_GetCreator(type, constructorParameterTypes) != null;
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

		private class DynamicCreatorNode
		{
			public readonly Type                   Type; // null = root node
			public          DynamicCreatorNode[]   NextNodes = Array.Empty<DynamicCreatorNode>();
			public          Func<object[], object> Creator;

			public DynamicCreatorNode(Type type)
			{
				Type = type;
			}

			public DynamicCreatorNode GetChild(Type[] types, int index)
			{
				if (types.Length == index)
					return this;

				// try to find the node for the current type
				Type type = types[index];
				foreach (var next in NextNodes)
				{
					if (next.Type == type)
						return next.GetChild(types, index + 1);
				}

				// the node for the current type does not exist, yet
				// => add it...
				DynamicCreatorNode node;
				lock (sSync)
				{
					foreach (var next in NextNodes)
					{
						if (next.Type == type)
							return next.GetChild(types, index + 1);
					}

					node = new DynamicCreatorNode(type);
					var newNextNodesByType = new DynamicCreatorNode[NextNodes.Length + 1];
					Array.Copy(NextNodes, newNextNodesByType, NextNodes.Length);
					newNextNodesByType[NextNodes.Length] = node;
					Thread.MemoryBarrier();
					NextNodes = newNextNodesByType;
				}

				return node.GetChild(types, index + 1);
			}
		}

		private static TypeKeyedDictionary<DynamicCreatorNode> sDynamicCreatorNodesByType = new TypeKeyedDictionary<DynamicCreatorNode>();

		/// <summary>
		/// Creates an instance of the specified type.
		/// </summary>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="constructorParameterTypes">Parameters of the constructor to call (may be <c>null</c> for parameterless constructors).</param>
		/// <param name="args">Arguments to pass to the constructor.</param>
		/// <returns>An instance of the specified type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">
		/// The <paramref name="args"/> do not contain the same number of objects as <paramref name="constructorParameterTypes"/> contains types,
		/// or the specified arguments cannot be assigned to the corresponding constructor parameter types.
		/// </exception>
		/// <remarks>
		/// Due to use of reflection this method is rather expensive.
		/// Please use the generic implementations of the <see cref="CreateInstance"/> method instead, if possible.
		/// </remarks>
		public static object CreateInstanceDynamically(Type type, Type[] constructorParameterTypes, params object[] args)
		{
			// check arguments
			if (type == null) throw new ArgumentNullException(nameof(type));

			// default to use the parameterless constructor
			if (constructorParameterTypes == null) constructorParameterTypes = Type.EmptyTypes;

			// check consistency of constructor parameters and arguments
			int constructorParameterTypeCount = constructorParameterTypes.Length;
			if (constructorParameterTypeCount > 0)
			{
				if (args == null || args.Length != constructorParameterTypeCount)
					throw new ArgumentException("The number of constructor arguments does not match the the number of constructor parameter types.");

				for (int i = 0; i < constructorParameterTypeCount; i++)
				{
					if (!constructorParameterTypes[i].IsInstanceOfType(args[i]))
						throw new ArgumentException($"Argument {i} (zero-based) is not assignable to the corresponding constructor parameter type '{constructorParameterTypes[i].FullName}'.");
				}
			}

			// create an instance of the specified type using the specified constructor
			var creator = CreateInstanceDynamically_GetCreator(type, constructorParameterTypes);
			return creator(args);
		}

		/// <summary>
		/// Gets a creator delegate that invokes the constructor with the specified parameters of the specified type.
		/// </summary>
		/// <param name="type">Type to get the creator delegate for.</param>
		/// <param name="constructorParameterTypes">Types of constructor parameters of <paramref name="type"/>.</param>
		/// <returns>
		/// Delegate that invokes the constructor of <paramref name="type"/> with the specified constructor parameters.
		/// <c>null</c> if <paramref name="type"/> does not have a constructor with the specified parameters.
		/// </returns>
		private static Func<object[], object> CreateInstanceDynamically_GetCreator(Type type, Type[] constructorParameterTypes)
		{
			// get the node of the type to create...
			if (!sDynamicCreatorNodesByType.TryGetValue(type, out var typeToInstantiateNode))
			{
				// create node for the type if it does not exist, yet
				lock (sSync)
				{
					if (!sDynamicCreatorNodesByType.TryGetValue(type, out typeToInstantiateNode))
					{
						typeToInstantiateNode = new DynamicCreatorNode(type);
						var newDynamicCreatorNodesByType = new TypeKeyedDictionary<DynamicCreatorNode>(sDynamicCreatorNodesByType) { { type, typeToInstantiateNode } };
						Thread.MemoryBarrier();
						sDynamicCreatorNodesByType = newDynamicCreatorNodesByType;
					}
				}
			}

			// try to look up creator node, create it, if necessary
			var node = typeToInstantiateNode.GetChild(constructorParameterTypes, 0);
			if (node.Creator != null) return node.Creator;

			// the creator is not initialized, yet
			// => generate and cache it
			lock (sSync)
			{
				if (node.Creator != null) return node.Creator;
				var creator = CreateInstanceDynamically_MakeCreator(type, constructorParameterTypes);
				Thread.MemoryBarrier();
				node.Creator = creator;
				return creator;
			}
		}

		/// <summary>
		/// Generates a creator delegate for the specified type using the constructor with the specified parameters.
		/// </summary>
		/// <param name="typeToInstantiate">Type to get the creator delegate for.</param>
		/// <param name="constructorParameterTypes">Parameters of the constructor of <paramref name="typeToInstantiate"/>.</param>
		/// <returns>
		/// Delegate that invokes the constructor of <paramref name="typeToInstantiate"/> with the specified constructor parameters.
		/// <c>null</c> if <paramref name="typeToInstantiate"/> does not have a constructor with the specified parameters.
		/// </returns>
		private static Func<object[], object> CreateInstanceDynamically_MakeCreator(Type typeToInstantiate, Type[] constructorParameterTypes)
		{
			// handle parameterless constructor of value types
			// (value types cannot have an explicit parameterless constructor)
			var parameterExpression = Expression.Parameter(typeof(object[]));
			if (typeToInstantiate.IsValueType && constructorParameterTypes.Length == 0)
			{
				return (Func<object[], object>)Expression.Lambda(
						typeof(Func<object[], object>),
						Expression.Convert(Expression.New(typeToInstantiate), typeof(object)),
						parameterExpression)
					.Compile();
			}

			// try to find the constructor with the specified parameter types
			var constructor = typeToInstantiate.GetConstructor(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				Type.DefaultBinder,
				CallingConventions.Any,
				constructorParameterTypes,
				null);

			// abort if the type does not contain a constructor with the specified arguments
			if (constructor == null) return null;

			// handle regular constructors of value types (structs) and all constructors of reference types (classes)
			var constructorArgumentExpressions = new List<Expression>();
			for (int i = 0; i < constructorParameterTypes.Length; i++)
			{
				Expression expression = Expression.ArrayIndex(parameterExpression, Expression.Constant(i));
				if (constructorParameterTypes[i] != typeof(object)) expression = Expression.Convert(expression, constructorParameterTypes[i]);
				constructorArgumentExpressions.Add(expression);
			}

			Expression body = Expression.Block(Expression.New(constructor, constructorArgumentExpressions));
			if (typeToInstantiate.IsValueType) body = Expression.Convert(body, typeof(object));
			return (Func<object[], object>)Expression.Lambda(
					typeof(Func<object[], object>),
					body,
					parameterExpression)
				.Compile();
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
