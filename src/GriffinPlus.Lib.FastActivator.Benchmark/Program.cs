///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using GriffinPlus.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

// ReSharper disable ConvertClosureToMethodGroup

namespace GriffinPlus.Benchmark
{
	class Program
	{
		const int RUNS = 50000000;
		const int METHOD_FIELD_LENGTH = 70;

		static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			Console.WriteLine("Activator Benchmark:");
			Console.WriteLine();

			RunBenchmark<TestClass<int>>();
			RunBenchmark<TestStruct<int>>();

			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}


		static void RunBenchmark<T>()
		{
			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (generic, default constructor):", typeof(T));
			Measure<T>(
				("T Activator.CreateInstance<T>()",     () => Activator.CreateInstance<T>()),
				("T FastActivator<T>.CreateInstance()", () => FastActivator<T>.CreateInstance())
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (non-generic, default constructor):", typeof(T));
			Measure<T>(
				("object Activator.CreateInstance(Type)",     () => (T)Activator.CreateInstance(typeof(T))),
				("object FastActivator.CreateInstance(Type)", () => (T)FastActivator.CreateInstance(typeof(T)))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			List<Type> constructorArgumentTypesList = new List<Type>();
			Type[] constructorArgumentTypes;

			Console.WriteLine("Instantiating {0} (1 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			int[] constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			object[] constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int>(typeof(T), 1)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int>(1))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (2 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int>(typeof(T), 1, 2)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int>(1, 2))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (3 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int>(typeof(T), 1, 2, 3)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int>(1, 2, 3))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (4 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int>(typeof(T), 1, 2, 3, 4)),
				("T      FastActivator<T>.CreateInstance<...>(...)", () => FastActivator<T>.CreateInstance<int, int, int, int>(1, 2, 3, 4))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (5 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5)),
				("T      FastActivator<T>.CreateInstance<...>(...)", () => FastActivator<T>.CreateInstance<int, int, int, int, int>(1, 2, 3, 4, 5))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (6 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int, int, int, int>(1, 2, 3, 4, 5, 6))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (7 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (8 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7, 8)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (9 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7, 8, 9)),
				("T      FastActivator<T>.CreateInstance<...>(...)", () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8, 9))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (10 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8, 9, 10))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (11 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)),
				("T      FastActivator.CreateInstance<...>(...)", () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (12 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (13 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (14 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (15 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("Instantiating {0} (16 constructor parameters):", typeof(T));
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure<T>(
				("object Activator.CreateInstance(Type, object[])",                        () => (T)Activator.CreateInstance(typeof(T), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])", () => (T)FastActivator.CreateInstanceDynamically(typeof(T), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",                          () => (T)FastActivator.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(typeof(T), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
				("T      FastActivator<T>.CreateInstance<...>(...)",                       () => FastActivator<T>.CreateInstance<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16))
			);
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------
		}

		static void Measure<T>(ValueTuple<string,Func<T>> reference, params ValueTuple<string,Func<T>>[] compares)
		{
			Stopwatch watch = new Stopwatch();

			// warm up
			GC.Collect();
			reference.Item2();

			// calm down
			Thread.Sleep(5000);

			// measure reference
			watch.Start();
			for (int i = 0; i < RUNS; i++) {
				reference.Item2();
			}
			watch.Stop();
			long referenceTimeMs = watch.ElapsedMilliseconds;
			WriteResultLine(reference.Item1, watch.ElapsedMilliseconds);

			// measure test object
			foreach (var compare in compares)
			{
				// clean up
				GC.Collect();
				watch.Reset();

				// warm up
				compare.Item2();

				// calm down
				Thread.Sleep(5000);

				// measure
				watch.Start();
				for (int i = 0; i < RUNS; i++) {
					compare.Item2();
				}
				watch.Stop();

				double speedGain = (double)(referenceTimeMs) / (double)watch.ElapsedMilliseconds;
				WriteResultLine(compare.Item1, watch.ElapsedMilliseconds, speedGain);
			}

		}

		static void WriteResultLine(string method, double timeMs)
		{
			string format = $"{{0,-{METHOD_FIELD_LENGTH}}}: {{1}} ms";
			Console.WriteLine(format, method, timeMs);
		}

		static void WriteResultLine(string method, double timeMs, double speedGain)
		{
			string format = $"{{0,-{METHOD_FIELD_LENGTH}}}: {{1}} ms ({{2:0.00}}x)";
			Console.WriteLine(format, method, timeMs, speedGain);
		}
	}
}
