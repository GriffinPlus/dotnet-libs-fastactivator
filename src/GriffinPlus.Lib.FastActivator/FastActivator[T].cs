///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace GriffinPlus.Lib
{

	/// <summary>
	/// Creates instances of a certain class using dynamically compiled creation methods
	/// (Generic version providing best performance gain for value types).
	/// </summary>
	/// <typeparam name="T">Type to instantiate.</typeparam>
	public static partial class FastActivator<T>
	{
		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its parameterless constructor.
		/// </summary>
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
		public static T CreateInstance()
		{
			return Creator.CreateInstance();
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg">Type of the constructor argument.</typeparam>
		/// <param name="arg">The constructor argument.</param>
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
		public static T CreateInstance<TArg>(TArg arg)
		{
			return Creator<TArg>.CreateInstance(arg);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
		public static T CreateInstance<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
		{
			return Creator<TArg1, TArg2>.CreateInstance(arg1, arg2);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
		{
			return Creator<TArg1, TArg2, TArg3>.CreateInstance(arg1, arg2, arg3);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
		/// </summary>
		/// <typeparam name="TArg1">Type of the first constructor argument.</typeparam>
		/// <typeparam name="TArg2">Type of the second constructor argument.</typeparam>
		/// <typeparam name="TArg3">Type of the third constructor argument.</typeparam>
		/// <typeparam name="TArg4">Type of the fourth constructor argument.</typeparam>
		/// <param name="arg1">The first constructor argument.</param>
		/// <param name="arg2">The second constructor argument.</param>
		/// <param name="arg3">The third constructor argument.</param>
		/// <param name="arg4">The fourth constructor argument.</param>
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4>(
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4)
		{
			return Creator<TArg1, TArg2, TArg3, TArg4>.CreateInstance(arg1, arg2, arg3, arg4);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5>(
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5)
		{
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5>.CreateInstance(arg1, arg2, arg3, arg4, arg5);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5,
			TArg6 arg6)
		{
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
		public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
			TArg1 arg1,
			TArg2 arg2,
			TArg3 arg3,
			TArg4 arg4,
			TArg5 arg5,
			TArg6 arg6,
			TArg7 arg7)
		{
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
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
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
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
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
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
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
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
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
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
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
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
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
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
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
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
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
		}

		/// <summary>
		/// Creates an instance of <typeparamref name="T"/> using its constructor with the specified arguments.
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
		/// <returns>An instance of <typeparamref name="T"/>.</returns>
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
			return Creator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>
				.CreateInstance(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
		}

		/// <summary>
		/// Creates an one-dimensional array with zero-based indexing of <typeparamref name="T"/>.
		/// </summary>
		/// <param name="length">Length of the array to create.</param>
		/// <returns>An array of <typeparamref name="T"/> with the specified length.</returns>
		public static T[] CreateArray(int length)
		{
			return Creator.CreateArray(length);
		}
	}

}
