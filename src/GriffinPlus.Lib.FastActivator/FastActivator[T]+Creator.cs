///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

// ReSharper disable CoVariantArrayConversion
// ReSharper disable StaticMemberInGenericType

namespace GriffinPlus.Lib
{

	partial class FastActivator<T>
	{
		/// <summary>
		/// Creates instances of <typeparamref name="T"/> using its parameterless constructor
		/// (generic version providing best performance gain for value types).
		/// </summary>
		private static class Creator
		{
			private static readonly object sInitSync = new object();

			#region CreateInstance()

			private static Func<T> sCreator = null;

			/// <summary>
			/// Creates an instance of <typeparamref name="T"/> using its parameterless constructor.
			/// </summary>
			/// <returns>An instance of the specified type.</returns>
			public static T CreateInstance()
			{
				var creator = sCreator ?? InitCreator();
				return creator();
			}

			/// <summary>
			/// Initializes the creator delegate <see cref="sCreator"/> used to instantiate <typeparamref name="T"/> using its parameterless constructor.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<T> InitCreator()
			{
				// try to get a previously generated creator
				if (sCreator != null)
					return sCreator;

				// the creator does not exist, yet
				lock (sInitSync)
				{
					// check again...
					if (sCreator != null)
						return sCreator;

					// generate creator and cache it
					var creator = MakeCreator();
					Thread.MemoryBarrier();
					sCreator = creator;
					return creator;
				}
			}

			/// <summary>
			/// Crafts a creator delegate that creates an instance of <typeparamref name="T"/> using its parameterless constructor.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<T> MakeCreator()
			{
				// handle parameterless constructor of value types
				// (value types cannot have an explicit parameterless constructor)
				if (typeof(T).IsValueType)
				{
					return (Func<T>)Expression.Lambda(
							typeof(Func<T>),
							Expression.New(typeof(T)),
							Array.Empty<ParameterExpression>())
						.Compile();
				}

				// try to find the constructor with the specified parameter types
				var constructor = typeof(T).GetConstructor(
					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
					Type.DefaultBinder,
					CallingConventions.Any,
					Type.EmptyTypes,
					null);

				// abort if the type does not contain a parameterless constructor
				if (constructor == null)
					throw new ArgumentException($"The type ({typeof(T).FullName}) does not have a parameterless constructor.");

				// craft a creator delegate using the parameterless constructor
				return (Func<T>)Expression.Lambda(
						typeof(Func<T>),
						Expression.New(typeof(T)),
						Array.Empty<ParameterExpression>())
					.Compile();
			}

			#endregion

			#region CreateArray()

			private static Func<int, T[]> sArrayCreator = null;

			/// <summary>
			/// Creates a one-dimensional array with zero-based indexing of <typeparamref name="T"/>.
			/// </summary>
			/// <param name="length">Length of the array to create.</param>
			/// <returns>An instance of the specified type.</returns>
			public static T[] CreateArray(int length)
			{
				var creator = sArrayCreator ?? InitArrayCreator();
				return creator(length);
			}

			/// <summary>
			/// Initializes the array creator delegate <see cref="sArrayCreator"/> used to instantiate one-dimensional
			/// zero-based arrays of <typeparamref name="T"/>.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<int, T[]> InitArrayCreator()
			{
				// try to get a previously generated creator
				if (sArrayCreator != null)
					return sArrayCreator;

				// the creator does not exist, yet
				lock (sInitSync)
				{
					// check again...
					if (sArrayCreator != null)
						return sArrayCreator;

					// generate creator and cache it
					var creator = MakeArrayCreator();
					Thread.MemoryBarrier();
					sArrayCreator = creator;
					return creator;
				}
			}

			/// <summary>
			/// Crafts a creator delegate that creates an instance of an one-dimensional zero-based array of <typeparamref name="T"/>.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<int, T[]> MakeArrayCreator()
			{
				Type[] parameterTypes = { typeof(int) };
				ParameterExpression[] parameterExpressions = parameterTypes.Select(Expression.Parameter).ToArray();
				LambdaExpression lambda = Expression.Lambda(
					typeof(Func<int, T[]>),
					Expression.NewArrayBounds(typeof(T), parameterExpressions),
					parameterExpressions);
				return (Func<int, T[]>)lambda.Compile();
			}

			#endregion
		}
	}

}
