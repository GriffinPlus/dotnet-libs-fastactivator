///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
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
	public class FastActivator
	{
		private delegate Array ArrayCreatorDelegate(int length);

		private static          TypeKeyedDictionary<TypeKeyedDictionary<Delegate>> sCreatorsByCreatorTypeMap           = new TypeKeyedDictionary<TypeKeyedDictionary<Delegate>>();
		private static          TypeKeyedDictionary<Delegate>                      sParameterlessCreatorsByCreatorType = new TypeKeyedDictionary<Delegate>();
		private static          TypeKeyedDictionary<ArrayCreatorDelegate>          sArrayCreatorsByTypeMap             = new TypeKeyedDictionary<ArrayCreatorDelegate>();
		private static readonly FastActivatorFuncTypeMap                           sObjectResultCreatorFuncMap         = new FastActivatorFuncTypeMap();
		private static readonly object                                             sSync                               = new object();

		/// <summary>
		/// Initializes the <see cref="FastActivator"/> class.
		/// </summary>
		static FastActivator() { }

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
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// check whether the specified constructor is available
			return creatorsByCreatorType.TryGetValue(creatorType, out _);
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
			if (!sParameterlessCreatorsByCreatorType.TryGetValue(type, out var creator))
			{
				InitCreatorCache(type);
				sParameterlessCreatorsByCreatorType.TryGetValue(type, out creator);
			}

			if (creator != null)
				return ((Func<object>)creator)();

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
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
		/// <exception cref="ArgumentException">
		/// The 'args' argument does not contain the same number of objects as the 'constructorParameterTypes' argument
		/// contains types.
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
		/// Creates an instance of the specified type using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg">Type of the constructor argument.</typeparam>
		/// <param name="type">Type of the object to create.</param>
		/// <param name="arg">The constructor argument.</param>
		/// <returns>An instance of the specified type.</returns>
		public static object CreateInstance<TArg>(Type type, TArg arg)
		{
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg, object>)creator)(arg);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, object>)creator)(arg1, arg2);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, object>)creator)(arg1, arg2, arg3);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, object>)creator)(arg1, arg2, arg3, arg4);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, object>)creator)(arg1, arg2, arg3, arg4, arg5);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, object>)creator)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, object>)creator)(
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
					arg15);
			}

			// constructor not found
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
			throw new ArgumentException(error, nameof(type));
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
			// create the key that is used to access the second level of cache
			Type creatorType = typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, object>);

			// query the creator cache
			if (!sCreatorsByCreatorTypeMap.TryGetValue(type, out var creatorsByCreatorType))
			{
				InitCreatorCache(type);
				creatorsByCreatorType = sCreatorsByCreatorTypeMap[type];
			}

			// try to create an instance of the specified class/struct using the requested constructor
			if (creatorsByCreatorType.TryGetValue(creatorType, out var creator))
			{
				return ((Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16, object>)creator)(
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
			string error = $"The specified type ({type.FullName}) does not have the required constructor.";
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
			// query the creator cache
			if (!sArrayCreatorsByTypeMap.TryGetValue(type, out var creator))
			{
				InitArrayCreatorCache(type);
				creator = sArrayCreatorsByTypeMap[type];
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
				if (sCreatorsByCreatorTypeMap.ContainsKey(type))
					return;

				TypeKeyedDictionary<Delegate> creatorTypeToCreatorMap = new TypeKeyedDictionary<Delegate>();
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

					ParameterExpression[] parameterExpressions = constructorParameterTypes.Select(Expression.Parameter).ToArray();
					Expression body;

					if (type.IsValueType)
					{
						body = Expression.Convert(
							parameterExpressions.Length > 0
								? Expression.New(constructor, parameterExpressions)
								: Expression.New(type),
							typeof(object));
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
					if (constructorParameterTypes.Length == 0)
						parameterlessCreator = creator;
				}

				TypeKeyedDictionary<TypeKeyedDictionary<Delegate>> untypedCreatorsByCreatorTypeMap = new TypeKeyedDictionary<TypeKeyedDictionary<Delegate>>(sCreatorsByCreatorTypeMap);
				untypedCreatorsByCreatorTypeMap.Add(type, creatorTypeToCreatorMap);
				TypeKeyedDictionary<Delegate> parameterlessCreatorsByCreatorType = new TypeKeyedDictionary<Delegate>(sParameterlessCreatorsByCreatorType);
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
					return;

				Type[] parameterTypes = { typeof(int) };
				ParameterExpression[] parameterExpressions = parameterTypes.Select(Expression.Parameter).ToArray();
				Expression body = Expression.NewArrayBounds(type, parameterExpressions);
				LambdaExpression lambda = Expression.Lambda(typeof(ArrayCreatorDelegate), body, parameterExpressions);
				var creator = (ArrayCreatorDelegate)lambda.Compile();

				TypeKeyedDictionary<ArrayCreatorDelegate> arrayCreatorsByTypeMap = new TypeKeyedDictionary<ArrayCreatorDelegate>(sArrayCreatorsByTypeMap);
				arrayCreatorsByTypeMap.Add(type, creator);
				Thread.MemoryBarrier(); // ensures everything up to this point has been actually written to memory
				sArrayCreatorsByTypeMap = arrayCreatorsByTypeMap;
			}
		}

		#endregion
	}

}
