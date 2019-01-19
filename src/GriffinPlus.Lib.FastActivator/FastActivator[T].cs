///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator).
//
// Copyright 2018 Sascha Falk <sascha@falk-online.eu>
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance
// with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed
// on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for
// the specific language governing permissions and limitations under the License.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace GriffinPlus.Lib
{
	/// <summary>
	/// Creates instances of a certain class using dynamically compiled creation methods
	/// (Generic version providing best performance gain for value types).
	/// </summary>
	public sealed class FastActivator<T>
	{
		private static readonly Dictionary<Type, Delegate> sCreators;
		private static readonly Func<T> sParameterlessCreator;

		/// <summary>
		/// Initializes the <see cref="FastActivator"/> class.
		/// </summary>
		static FastActivator()
		{
			sCreators = GetCreators();
			
			// init parameterless creator
			Delegate creator;
			sCreators.TryGetValue(typeof(Func<T>), out creator);
			sParameterlessCreator = (Func<T>)creator;
		}

		/// <summary>
		/// Creates an instance of the specified type using its default constructor.
		/// </summary>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance()
		{
			// create an instance of the type
			if (sParameterlessCreator != null) {
				return sParameterlessCreator();
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG">Type of the constructor argument.</typeparam>
		/// <param name="arg">The constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TARG>(TARG arg)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG, T>)creator)(arg);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TARG1, TARG2>(TARG1 arg1, TARG2 arg2)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, T>)creator)(arg1, arg2);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TARG1, TARG2, TARG3>(TARG1 arg1, TARG2 arg2, TARG3 arg3)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, T>)creator)(arg1, arg2, arg3);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, T>)creator)(arg1, arg2, arg3, arg4);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, T>)creator)(arg1, arg2, arg3, arg4, arg5);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TARG8">Type of the eighth constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <param name="arg8">The eighth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TARG8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TARG9">Type of the ninth constructor argument.</typeparam>
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
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TARG8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TARG9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TARG10">Type of the tenth constructor argument.</typeparam>
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
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TARG8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TARG9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TARG10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TARG11">Type of the eleventh constructor argument.</typeparam>
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
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TARG8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TARG9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TARG10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TARG11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TARG12">Type of the twelfth constructor argument.</typeparam>
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
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TARG8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TARG9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TARG10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TARG11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TARG12">Type of the twelfth constructor argument.</typeparam>
		/// <typeparam name="TARG13">Type of the thirteenth constructor argument.</typeparam>
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
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TARG8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TARG9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TARG10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TARG11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TARG12">Type of the twelfth constructor argument.</typeparam>
		/// <typeparam name="TARG13">Type of the thirteenth constructor argument.</typeparam>
		/// <typeparam name="TARG14">Type of the fourteenth constructor argument.</typeparam>
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
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TARG8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TARG9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TARG10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TARG11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TARG12">Type of the twelfth constructor argument.</typeparam>
		/// <typeparam name="TARG13">Type of the thirteenth constructor argument.</typeparam>
		/// <typeparam name="TARG14">Type of the fourteenth constructor argument.</typeparam>
		/// <typeparam name="TARG15">Type of the fifteenth constructor argument.</typeparam>
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
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14, TARG15 arg15)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
			throw new ArgumentException(error, nameof(T));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <typeparam name="TARG6">Type of the sixth constructor argument.</typeparam>
		/// <typeparam name="TARG7">Type of the seventh constructor argument.</typeparam>
		/// <typeparam name="TARG8">Type of the eighth constructor argument.</typeparam>
		/// <typeparam name="TARG9">Type of the ninth constructor argument.</typeparam>
		/// <typeparam name="TARG10">Type of the tenth constructor argument.</typeparam>
		/// <typeparam name="TARG11">Type of the eleventh constructor argument.</typeparam>
		/// <typeparam name="TARG12">Type of the twelfth constructor argument.</typeparam>
		/// <typeparam name="TARG13">Type of the thirteenth constructor argument.</typeparam>
		/// <typeparam name="TARG14">Type of the fourteenth constructor argument.</typeparam>
		/// <typeparam name="TARG15">Type of the fifteenth constructor argument.</typeparam>
		/// <typeparam name="TARG16">Type of the sixteenth constructor argument.</typeparam>
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
		public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, TARG16>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14, TARG15 arg15, TARG16 arg16)
		{
			// create an instance of the type
			Delegate creator;
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, TARG16, T>);
			if (sCreators.TryGetValue(creatorType, out creator)) {
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, TARG16, T>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", typeof(T).FullName);
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

				ParameterExpression[] parameterExpressions = constructorParameterTypes.Select(x => Expression.Parameter(x)).ToArray();
				Expression body;

				if (typeof(T).IsValueType)
				{
					if (parameterExpressions.Length > 0)
					{
						body = Expression.New(constructor, parameterExpressions);
					}
					else
					{
						body = Expression.New(typeof(T));
					}
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
