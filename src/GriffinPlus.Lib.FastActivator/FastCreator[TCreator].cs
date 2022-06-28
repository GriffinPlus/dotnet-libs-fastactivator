///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

// ReSharper disable CoVariantArrayConversion
// ReSharper disable StaticMemberInGenericType

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Provides a creator delegate for a certain type using a constructor with a specific set of parameters.
	/// </summary>
	/// <typeparam name="TCreator">
	/// The creator delegate type.
	/// Its return type must be <see cref="System.Object"/>.
	/// Its parameters must match the parameters of the constructor of the type to invoke.
	/// </typeparam>
	public sealed class FastCreator<TCreator> where TCreator : Delegate
	{
		private static          TypeKeyedDictionary<TCreator> sCreators = new TypeKeyedDictionary<TCreator>();
		private static readonly object                        sInitSync = new object();

		/// <summary>
		/// Gets the creator delegate instance for the specified type.
		/// </summary>
		/// <param name="type">Type to get the creator delegate for.</param>
		public static TCreator GetCreator(Type type)
		{
			// try to get a previously generated creator
			if (sCreators.TryGetValue(type, out var creator))
				return creator;

			// the creator does not exist, yet
			lock (sInitSync)
			{
				// check again...
				if (sCreators.TryGetValue(type, out creator))
					return creator;

				// generate creator and cache it
				creator = MakeCreator(type);
				var creators = new TypeKeyedDictionary<TCreator>(sCreators) { { type, creator } };
				Thread.MemoryBarrier();
				sCreators = creators;
				return creator;
			}
		}

		/// <summary>
		/// Crafts a creator delegate that creates an instance of the type returned by the creator delegate
		/// using a constructor defined by parameters of the delegate.
		/// </summary>
		/// <param name="typeOfObjectToCreate">The type to create.</param>
		/// <returns>The creator delegate.</returns>
		/// <exception cref="ArgumentException">
		/// The <typeparamref name="TCreator"/> type parameter is invalid if one of the following conditions applies:
		/// <br/>- Its return type is <see cref="System.Void"/>.
		/// <br/>- The type to create does not have a constructor with the same parameters as <typeparamref name="TCreator"/>.
		/// </exception>
		private static TCreator MakeCreator(Type typeOfObjectToCreate)
		{
			// get the return type and parameter types of the delegate
			MethodInfo method = typeof(TCreator).GetMethod("Invoke");
			Debug.Assert(method != null, nameof(method) + " != null");
			Type returnType = method.ReturnType;
			Type[] parameterTypes = method.GetParameters().Select(x => x.ParameterType).ToArray();

			// ensure that the specified creator delegate type returns System.Object
			if (returnType != typeof(object))
				throw new ArgumentException("The creator delegate must return System.Object.");

			// handle parameterless constructor of value types
			// (value types cannot have an explicit parameterless constructor)
			if (typeOfObjectToCreate.IsValueType && parameterTypes.Length == 0)
			{
				return (TCreator)Expression.Lambda(
						typeof(TCreator),
						Expression.Convert(
							Expression.New(typeOfObjectToCreate),
							typeof(object)),
						Array.Empty<ParameterExpression>())
					.Compile();
			}

			// try to find the constructor with the specified parameter types
			var constructor = typeOfObjectToCreate.GetConstructor(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				Type.DefaultBinder,
				CallingConventions.Any,
				parameterTypes,
				null);

			// abort if the type does not contain a constructor with the specified arguments
			if (constructor == null)
			{
				if (parameterTypes.Length == 0)
					throw new ArgumentException($"The type ({returnType.FullName}) does not have a parameterless constructor.");

				string message = $"The type ({returnType.FullName}) does not have a constructor with the specified parameters:";
				foreach (var parameterType in parameterTypes) message += Environment.NewLine + $"- {parameterType.FullName}";
				throw new ArgumentException(message);
			}

			// craft a creator delegate using the constructor defined by the parameters of the creator delegate
			if (typeOfObjectToCreate.IsValueType)
			{
				var parameterExpressions = parameterTypes.Select(Expression.Parameter).ToArray();
				return (TCreator)Expression.Lambda(
						typeof(TCreator),
						Expression.Convert(
							parameterTypes.Length > 0
								? Expression.New(constructor, parameterExpressions)
								: Expression.New(typeOfObjectToCreate),
							typeof(object)),
						parameterExpressions)
					.Compile();
			}
			else
			{
				var parameterExpressions = parameterTypes.Select(Expression.Parameter).ToArray();
				return (TCreator)Expression.Lambda(
						typeof(TCreator),
						Expression.New(constructor, parameterExpressions),
						parameterExpressions)
					.Compile();
			}
		}
	}

}
