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
		/// Creates instances of the specified type using a constructor with 12 parameters
		/// (generic version providing best performance gain for value types).
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor parameter.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor parameter.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor parameter.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor parameter.</typeparam>
		/// <typeparam name="TArg5">Type of the fifth constructor parameter.</typeparam>
		/// <typeparam name="TArg6">Type of the sixth constructor parameter.</typeparam>
		/// <typeparam name="TArg7">Type of the seventh constructor parameter.</typeparam>
		/// <typeparam name="TArg8">Type of the eighth constructor parameter.</typeparam>
		/// <typeparam name="TArg9">Type of the ninth constructor parameter.</typeparam>
		/// <typeparam name="TArg10">Type of the tenth constructor parameter.</typeparam>
		/// <typeparam name="TArg11">Type of the eleventh constructor parameter.</typeparam>
		/// <typeparam name="TArg12">Type of the twelfth constructor parameter.</typeparam>
		private static class Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
		{
			private static readonly object                                                                                         sInitSync = new object();
			private static          Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, T> sCreator  = null;

			/// <summary>
			/// Creates an instance of <typeparamref name="T"/> using its constructor with parameters specified by <typeparamref name="TArg1"/> to
			/// <typeparamref name="TArg12"/>.
			/// </summary>
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
			public static T CreateInstance(
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
				Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, T> creator = sCreator ?? InitCreator();
				return creator(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			}

			/// <summary>
			/// Crafts a creator delegate that creates an instance of <typeparamref name="T"/> using its
			/// constructor with the parameters specified by <typeparamref name="TArg1"/> to <typeparamref name="TArg12"/>.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, T> InitCreator()
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
					Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, T> creator = MakeCreator();
					Thread.MemoryBarrier();
					sCreator = creator;
					return creator;
				}
			}

			/// <summary>
			/// Crafts a creator delegate that creates an instance of <typeparamref name="T"/> using the
			/// constructor defined by <typeparamref name="TArg1"/> to <typeparamref name="TArg12"/>.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, T> MakeCreator()
			{
				// prepare list of constructor parameter types
				Type[] parameterTypes =
				{
					typeof(TArg1), typeof(TArg2), typeof(TArg3), typeof(TArg4), typeof(TArg5),
					typeof(TArg6), typeof(TArg7), typeof(TArg8), typeof(TArg9), typeof(TArg10),
					typeof(TArg11), typeof(TArg12)
				};

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
				return (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, T>)Expression.Lambda(
						typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, T>),
						Expression.New(constructor, parameterExpressions),
						parameterExpressions)
					.Compile();
			}
		}
	}

}
