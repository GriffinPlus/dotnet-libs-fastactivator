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
		/// Creates instances of the specified type using a constructor with 2 parameters
		/// (generic version providing best performance gain for value types).
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor parameter.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor parameter.</typeparam>
		private static class Creator<TArg1, TArg2>
		{
			private static readonly object                sInitSync = new object();
			private static          Func<TArg1, TArg2, T> sCreator  = null;

			/// <summary>
			/// Creates an instance of <typeparamref name="T"/> using its constructor with parameters specified by <typeparamref name="TArg1"/> to
			/// <typeparamref name="TArg2"/>.
			/// </summary>
			/// <param name="arg1">The first constructor argument.</param>
			/// <param name="arg2">The second constructor argument.</param>
			/// <returns>An instance of the specified type.</returns>
			public static T CreateInstance(TArg1 arg1, TArg2 arg2)
			{
				Func<TArg1, TArg2, T> creator = sCreator ?? InitCreator();
				return creator(arg1, arg2);
			}

			/// <summary>
			/// Crafts a creator delegate that creates an instance of <typeparamref name="T"/> using its
			/// constructor with the parameters specified by <typeparamref name="TArg1"/> to <typeparamref name="TArg2"/>.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<TArg1, TArg2, T> InitCreator()
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
					Func<TArg1, TArg2, T> creator = MakeCreator();
					Thread.MemoryBarrier();
					sCreator = creator;
					return creator;
				}
			}

			/// <summary>
			/// Crafts a creator delegate that creates an instance of <typeparamref name="T"/> using the
			/// constructor defined by <typeparamref name="TArg1"/> to <typeparamref name="TArg2"/>.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<TArg1, TArg2, T> MakeCreator()
			{
				// prepare list of constructor parameter types
				Type[] parameterTypes = { typeof(TArg1), typeof(TArg2) };

				// try to find the constructor with the specified parameter types
				ConstructorInfo constructor = typeof(T).GetConstructor(
					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
					Type.DefaultBinder,
					CallingConventions.Any,
					parameterTypes,
					null);

				// abort if the type does not contain a constructor with the specified arguments
				if (constructor == null)
				{
					string message = $"The type ({typeof(T).FullName}) does not have a constructor with the specified parameters:";
					foreach (Type parameterType in parameterTypes) message += Environment.NewLine + $"- {parameterType.FullName}";
					throw new ArgumentException(message);
				}

				// craft a creator delegate using the constructor defined by the parameters of the creator delegate
				ParameterExpression[] parameterExpressions = parameterTypes.Select(Expression.Parameter).ToArray();
				return (Func<TArg1, TArg2, T>)Expression.Lambda(
						typeof(Func<TArg1, TArg2, T>),
						Expression.New(constructor, parameterExpressions),
						parameterExpressions)
					.Compile();
			}
		}
	}

}
