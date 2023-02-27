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
		/// Creates instances of the specified type using a constructor with 1 parameter
		/// (generic version providing best performance gain for value types).
		/// </summary>
		/// <typeparam name="TArg">Type of the constructor parameter.</typeparam>
		private static class Creator<TArg>
		{
			private static readonly object        sInitSync = new object();
			private static          Func<TArg, T> sCreator  = null;

			/// <summary>
			/// Creates an instance of <typeparamref name="T"/> using its constructor with the parameter specified by <typeparamref name="TArg"/>.
			/// </summary>
			/// <param name="arg">The constructor argument.</param>
			/// <returns>An instance of the specified type.</returns>
			public static T CreateInstance(TArg arg)
			{
				Func<TArg, T> creator = sCreator ?? InitCreator();
				return creator(arg);
			}

			/// <summary>
			/// Crafts a creator delegate that creates an instance of <typeparamref name="T"/> using its
			/// constructor with the parameter specified by <typeparamref name="TArg"/>.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<TArg, T> InitCreator()
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
					Func<TArg, T> creator = MakeCreator();
					Thread.MemoryBarrier();
					sCreator = creator;
					return creator;
				}
			}

			/// <summary>
			/// Crafts a creator delegate that creates an instance of <typeparamref name="T"/> using the
			/// constructor with the parameter defined by <typeparamref name="TArg"/>.
			/// </summary>
			/// <returns>The creator delegate.</returns>
			private static Func<TArg, T> MakeCreator()
			{
				// prepare list of constructor parameter types
				Type[] parameterTypes = { typeof(TArg) };

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
				return (Func<TArg, T>)Expression.Lambda(
						typeof(Func<TArg, T>),
						Expression.New(constructor, parameterExpressions),
						parameterExpressions)
					.Compile();
			}
		}
	}

}
