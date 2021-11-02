///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable CoVariantArrayConversion

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Creates instances of a certain class using dynamically compiled creation methods
	/// (Generic version providing best performance gain for value types).
	/// </summary>
	public sealed class FastActivator<T>
	{
		// ReSharper disable once StaticMemberInGenericType
		private static readonly Dictionary<Type, Delegate> sCreators;
		private static readonly Func<T>                    sParameterlessCreator;

		/// <summary>
		/// Initializes the <see cref="FastActivator"/> class.
		/// </summary>
		static FastActivator()
		{
			sCreators = GetCreators();

			// init parameterless creator
			sCreators.TryGetValue(typeof(Func<T>), out var creator);
			sParameterlessCreator = creator != null
				                        ? (Func<T>)creator
				                        : () => throw new ArgumentException(
					                                $"The specified type ({typeof(T).FullName}) does not have the required constructor.",
					                                nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its default constructor.
		/// </summary>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance() => sParameterlessCreator();

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg">Type of the constructor argument.</typeparam>
		/// <param name="arg">The constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TArg>(TArg arg)
		{
			// create an instance of the type
			Type creatorType = typeof(Func<TArg, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg, T>)creator)(arg);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
		{
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, T>)creator)(arg1, arg2);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
		{
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, T>)creator)(arg1, arg2, arg3);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4>(
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4)
		{
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, T>)creator)(arg1, arg2, arg3, arg4);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5>(
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5)
		{
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, T>)creator)(arg1, arg2, arg3, arg4, arg5);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5,
			TArg6 arg6)
		{
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5,
			TArg6 arg6,
			TArg7 arg7)
		{
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5,
			TArg6 arg6,
			TArg7 arg7,
			TArg8 arg8)
		{
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
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
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(
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
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(
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
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(
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
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(
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
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(
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
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(
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
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
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
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(
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
			// create an instance of the type
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, T>);
			if (sCreators.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, T>)creator)(
					arg1,
					arg2,
					arg3,
					arg4,
					arg5,
					arg6,
					arg7,
					arg8,
					arg9,
					arg10,
					arg11,
					arg12,
					arg13,
					arg14,
					arg15,
					arg16);
			}

			// constructor not found
			string error = $"The specified type ({typeof(T).FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Initializes the creator cache.
		/// </summary>
		private static Dictionary<Type, Delegate> GetCreators()
		{
			Dictionary<Type, Delegate> creatorTypeToCreatorMap = new Dictionary<Type, Delegate>();

			// add default constructor, if the type is a value type
			// (the default constructor will not occur in the enumeration below...)
			if (typeof(T).IsValueType)
			{
				ParameterExpression[] parameterExpressions = new ParameterExpression[0];
				Expression body = Expression.New(typeof(T));

				// compile creator
				Type creatorType = FastActivatorFuncTypeMap.MakeGenericCreatorFuncType(typeof(T), Type.EmptyTypes, 0);
				LambdaExpression lambda = Expression.Lambda(creatorType, body, parameterExpressions);
				Delegate creator = lambda.Compile();
				creatorTypeToCreatorMap.Add(creatorType, creator);
			}

			// add create for all constructors
			foreach (var constructor in typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance))
			{
				var constructorParameterTypes = constructor.GetParameters().Select(x => x.ParameterType).ToArray();
				if (constructorParameterTypes.Length > 16) continue;

				ParameterExpression[] parameterExpressions = constructorParameterTypes.Select(Expression.Parameter).ToArray();
				Expression body;

				if (typeof(T).IsValueType)
				{
					body = parameterExpressions.Length > 0
						       ? Expression.New(constructor, parameterExpressions)
						       : Expression.New(typeof(T));
				}
				else
				{
					body = Expression.New(constructor, parameterExpressions);
				}

				// compile creator
				Type creatorType = FastActivatorFuncTypeMap.MakeGenericCreatorFuncType(typeof(T), constructorParameterTypes, constructorParameterTypes.Length);
				LambdaExpression lambda = Expression.Lambda(creatorType, body, parameterExpressions);
				Delegate creator = lambda.Compile();
				creatorTypeToCreatorMap.Add(creatorType, creator);
			}

			return creatorTypeToCreatorMap;
		}
	}

}
