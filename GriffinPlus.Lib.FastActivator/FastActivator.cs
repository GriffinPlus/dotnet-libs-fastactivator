///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://griffin.plus)
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
	/// (Non-generic version providing best performance gain for reference types).
	/// </summary>
	public partial class FastActivator
	{
		private delegate Array ArrayCreatorDelegate(int length);
		private static Dictionary<Type, Dictionary<Type, Delegate>> sCreatorsByCreatorTypeMap = new Dictionary<Type, Dictionary<Type, Delegate>>();
		private static Dictionary<Type, Delegate> sParameterlessCreatorsByCreatorType = new Dictionary<Type, Delegate>();
		private static Dictionary<Type, ArrayCreatorDelegate> sArrayCreatorsByTypeMap = new Dictionary<Type, ArrayCreatorDelegate>();
		private static FastActivatorFuncTypeMap sObjectResultCreatorFuncMap = new FastActivatorFuncTypeMap();
		private static object sSync = new object();

		/// <summary>
		/// Initializes the <see cref="FastActivator"/> class.
		/// </summary>
		static FastActivator()
		{

		}

		#region Checking Type Constructor

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
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType)) {
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// check whether the specified constructor is available
			Delegate creator;
			return creatorsByCreatorType.TryGetValue(creatorType, out creator);
		}

		#endregion

		#region Creating Objects

		/// <summary>
		/// Creates an instance of the specified type using its default constructor.
		/// </summary>
		/// <param name="type">Type of the object to create.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance(Type type)
		{
			// get parameterless creator
			Delegate creator;
			if (!sParameterlessCreatorsByCreatorType.TryGetValue(type, out creator)) {
				InitCreatorCache(type);
				sParameterlessCreatorsByCreatorType.TryGetValue(type, out creator);
			}

			if (creator != null) {
				return ((Func<object>)creator)();
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
		}

		/// <summary>
		/// Creates an instance of the specified type.
		/// </summary>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="constructorParameterTypes">Parameters of the constructor to call (up to 16 types; may be null for parameterless constructors).</param>
		/// <param name="args">Arguments to pass to the constructor.</param>
		/// <returns>An instance of the specified type.</returns>
		/// <exception cref="ArgumentNullException">The 'type' argument is null.</exception>
		/// <exception cref="ArgumentException">The 'args' argument does not contain the same number of objects as the 'constructorParameterTypes' argument contains types.</exception>
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
					if (constructorParameterTypeCount > 16) {
						throw new ArgumentException("This method does not support constructors with more than 16 arguments.");
					}

					if (args == null || args.Length != constructorParameterTypeCount) {
						throw new ArgumentException("The number of constructor arguments does not match the the number of constructor parameter types.");
					}
				}
			}

			Type creatorType = sObjectResultCreatorFuncMap.Get(constructorParameterTypes);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return CallCreatorDynamically(creator, args);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
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
				case 0: return creator();
				case 1: return creator(args[0]);
				case 2: return creator(args[0], args[1]);
				case 3: return creator(args[0], args[1], args[2]);
				case 4: return creator(args[0], args[1], args[2], args[3]);
				case 5: return creator(args[0], args[1], args[2], args[3], args[4]);
				case 6: return creator(args[0], args[1], args[2], args[3], args[4], args[5]);
				case 7: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
				case 8: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
				case 9: return creator(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
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
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG">Type of the constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg">The constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TARG>(Type type, TARG arg)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG, object>)creator)(arg);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TARG1, TARG2>(Type type, TARG1 arg1, TARG2 arg2)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, object>)creator)(arg1, arg2);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TARG1, TARG2, TARG3>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, object>)creator)(arg1, arg2, arg3);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, object>)creator)(arg1, arg2, arg3, arg4);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
		}

		/// <summary>
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TARG1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TARG2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TARG3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TARG4">Type of the fourth constructor argument.</typeparam>
		/// <typeparam name="TARG5">Type of the fifth constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, object>)creator)(arg1, arg2, arg3, arg4, arg5);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <param name="arg5">The fifth constructor argument.</param>
		/// <param name="arg6">The sixth constructor argument.</param>
		/// <param name="arg7">The seventh constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14, TARG15 arg15)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
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
		public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, TARG16>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14, TARG15 arg15, TARG16 arg16)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, TARG16, object>);

			// query the creator cache
			Dictionary<Type, Delegate> creatorsByCreatorType;
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType))
			{
				InitCreatorCache(type);
				sCreatorsByCreatorTypeMap.TryGetValue(type, out creatorsByCreatorType);
			}

			// try to create an instance of the specified class/struct using the requested constructor
			Delegate creator;
			if (creatorsByCreatorType.TryGetValue(creatorType, out creator))
			{
				return ((Func<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, TARG16, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
			}

			// constructor not found
			string error = string.Format("The specified type ({0}) does not have the required constructor.", type.FullName);
			throw new ArgumentException(error, nameof(type));
		}

		#endregion

		#region Creating Arrays

		/// <summary>
		/// Creates an array of the specified type.
		/// </summary>
		/// <param name="type">Element type of the array to create.</param>
		/// <param name="length">Length of the array to create.</param>
		/// <returns>An instance of the specified type.</returns>
		public static Array CreateArray(Type type, int length)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<Array>);

			// query the creator cache
			ArrayCreatorDelegate creator;
			if (!sArrayCreatorsByTypeMap.TryGetValue(type, out creator))
			{
				InitArrayCreatorCache(type);
				sArrayCreatorsByTypeMap.TryGetValue(type, out creator);
			}

			return creator(length);
		}

		#endregion

		#region Building Creator Delegates

		/// <summary>
		/// Initializes the creator cache for the specified type.
		/// </summary>
		/// <param name="type">Type to generate creators for.</param>
		private static void InitCreatorCache(Type type)
		{
			lock (sSync)
			{
				// abort, if the type was already processed
				if (sCreatorsByCreatorTypeMap.ContainsKey(type)) {
					return;
				}

				Dictionary<Type, Delegate> creatorTypeToCreatorMap = new Dictionary<Type, Delegate>();
				Delegate parameterlessCreator = null;

				// add default constructor, if the type is a value type
				// (the default constructor will not occur in the enumeration below...)
				if (type.IsValueType)
				{
					ParameterExpression[] parameterExpressions = new ParameterExpression[0];
					Expression body = Expression.Convert(Expression.New(type), typeof(object));

					// get creator function type
					Type creatorType = sObjectResultCreatorFuncMap.Get(Type.EmptyTypes);

					// compile creator
					LambdaExpression lambda = Expression.Lambda(creatorType, body, parameterExpressions);
					Delegate creator = lambda.Compile();
					creatorTypeToCreatorMap.Add(creatorType, creator);
					parameterlessCreator = creator;
				}

				// add create for all constructors
				foreach (var constructor in type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance))
				{
					var constructorParameterTypes = constructor.GetParameters().Select(x => x.ParameterType).ToArray();
					if (constructorParameterTypes.Length > 16) continue;

					ParameterExpression[] parameterExpressions = constructorParameterTypes.Select(x => Expression.Parameter(x)).ToArray();
					Expression body;

					if (type.IsValueType)
					{
						if (parameterExpressions.Length > 0)
						{
							body = Expression.Convert(Expression.New(constructor, parameterExpressions), typeof(object));
						}
						else
						{
							body = Expression.Convert(Expression.New(type), typeof(object));
						}
					}
					else
					{
						body = Expression.New(constructor, parameterExpressions);
					}

					// get creator function type
					Type creatorType = sObjectResultCreatorFuncMap.Get(constructorParameterTypes);

					// compile creator
					LambdaExpression lambda = Expression.Lambda(creatorType, body, parameterExpressions);
					Delegate creator = lambda.Compile();
					creatorTypeToCreatorMap.Add(creatorType, creator);
					if (constructorParameterTypes.Length == 0) {
						parameterlessCreator = creator;
					}
				}

				Dictionary<Type, Dictionary<Type, Delegate>> untypedCreatorsByCreatorTypeMap = new Dictionary<Type, Dictionary<Type, Delegate>>(sCreatorsByCreatorTypeMap);
				untypedCreatorsByCreatorTypeMap.Add(type, creatorTypeToCreatorMap);
				Dictionary<Type, Delegate> parameterlessCreatorsByCreatorType = new Dictionary<Type, Delegate>(sParameterlessCreatorsByCreatorType);
				parameterlessCreatorsByCreatorType.Add(type, parameterlessCreator);
				Thread.MemoryBarrier(); // ensures everything up to this point has been actually written to memory
				sCreatorsByCreatorTypeMap = untypedCreatorsByCreatorTypeMap;
				sParameterlessCreatorsByCreatorType = parameterlessCreatorsByCreatorType;
			}
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
				{
					return;
				}

				Type returnType = typeof(Array);
				Type[] parameterTypes = new Type[] { typeof(int) };
				ParameterExpression[] parameterExpressions = parameterTypes.Select(x => Expression.Parameter(x)).ToArray();
				Expression body = Expression.NewArrayBounds(type, parameterExpressions);
				LambdaExpression lambda = Expression.Lambda(typeof(ArrayCreatorDelegate), body, parameterExpressions);
				var creator = (ArrayCreatorDelegate)lambda.Compile();

				Dictionary<Type, ArrayCreatorDelegate> arrayCreatorsByTypeMap = new Dictionary<Type, ArrayCreatorDelegate>(sArrayCreatorsByTypeMap);
				arrayCreatorsByTypeMap.Add(type, creator);
				Thread.MemoryBarrier(); // ensures everything up to this point has been actually written to memory
				sArrayCreatorsByTypeMap = arrayCreatorsByTypeMap;
			}
		}

		#endregion

	}
}
