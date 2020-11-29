///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite.
// Project URL: https://github.com/griffinplus/dotnet-libs-fastactivator
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;

using GriffinPlus.Lib;

using Microsoft.Win32;

// ReSharper disable AccessToModifiedClosure
// ReSharper disable ConvertClosureToMethodGroup

namespace GriffinPlus.Benchmark
{
	internal class Program
	{
		private const int TestedAllocationCount  = 50000000;
		private const int MethodColumnWidth      = 70;
		private const int MeasurementColumnWidth = 22;

		private static void Main()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			var framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
			Console.WriteLine("Target Framework:  {0}", framework);
			Console.WriteLine("Runtime Version:   {0}", GetRuntimeDescription());

			Console.WriteLine();
			Console.WriteLine("Activator Benchmark:");
			Console.WriteLine();

			RunBenchmark<TestClass<int>, TestStruct<int>>();

			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}


		private static void RunBenchmark<TClass, TStruct>()
		{
			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### No Constructor Parameters (Generic)");
			Console.WriteLine();
			WriteHeader();
			Measure(
				("T Activator.CreateInstance<T>()",
					() => Activator.CreateInstance<TClass>(),
					() => Activator.CreateInstance<TStruct>()),
				("T FastActivator<T>.CreateInstance()",
					() => FastActivator<TClass>.CreateInstance(),
					() => FastActivator<TStruct>.CreateInstance()));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### No Constructor Parameters (Non-Generic)");
			Console.WriteLine();
			WriteHeader();
			Measure(
				("object Activator.CreateInstance(Type)",
					() => (TClass) Activator.CreateInstance(typeof(TClass)),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct))),
				("object FastActivator.CreateInstance(Type)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass)),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct))));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			var    constructorArgumentTypesList = new List<Type>();
			Type[] constructorArgumentTypes;

			Console.WriteLine("### 1 Constructor Parameter");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			var constructorArgumentsTyped =
				new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			var constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1),
					() => FastActivator<TStruct>.CreateInstance(1)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 2 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2),
					() => FastActivator<TStruct>.CreateInstance(1, 2)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 3 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 4 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 5 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 6 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 7 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 8 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 9 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 10 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 11 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)),
				("T      FastActivator.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 12 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 13 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 14 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 15 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------

			Console.WriteLine("### 16 Constructor Parameters");
			Console.WriteLine();
			WriteHeader();
			constructorArgumentTypesList.Add(typeof(int));
			constructorArgumentTypes = constructorArgumentTypesList.ToArray();
			constructorArgumentsTyped = new int[constructorArgumentTypesList.Count]; // always 0, but does not matter...
			constructorArgumentsUntyped = new object[constructorArgumentTypesList.Count];
			Array.Copy(constructorArgumentsTyped, constructorArgumentsUntyped, constructorArgumentTypesList.Count);
			Measure(
				("object Activator.CreateInstance(Type, object[])",
					() => (TClass) Activator.CreateInstance(typeof(TClass), constructorArgumentsUntyped),
					() => (TStruct) Activator.CreateInstance(typeof(TStruct), constructorArgumentsUntyped)),
				("object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
					() => (TClass) FastActivator.CreateInstanceDynamically(typeof(TClass), constructorArgumentTypes, constructorArgumentsUntyped),
					() => (TStruct) FastActivator.CreateInstanceDynamically(typeof(TStruct), constructorArgumentTypes, constructorArgumentsUntyped)),
				("object FastActivator.CreateInstance<...>(...)",
					() => (TClass) FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
					() => (TStruct) FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)),
				("T      FastActivator<T>.CreateInstance<...>(...)",
					() => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
					() => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)));
			Console.WriteLine();
			Console.WriteLine();

			// -----------------------------------------------------------------------------------------------------------------
		}

		private static void Measure<TClass, TStruct>(
			ValueTuple<string, Func<TClass>, Func<TStruct>>          reference,
			params ValueTuple<string, Func<TClass>, Func<TStruct>>[] compares)
		{
			var row = 0;

			// measure the reference (.NET Activator)
			var allocateClassReferenceTimeMs  = MeasureAllocation(reference.Item2);
			var allocateStructReferenceTimeMs = MeasureAllocation(reference.Item3);
			WriteResultLine(
				row++,
				reference.Item1,
				allocateClassReferenceTimeMs,
				allocateStructReferenceTimeMs);

			// measure comparisons
			var watch = new Stopwatch();
			foreach (var compare in compares)
			{
				// warm up
				compare.Item2();
				compare.Item3();

				// clean up and calm down
				GC.Collect();
				watch.Reset();
				Thread.Sleep(5000);

				// measure allocating instances of a class
				watch.Start();
				for (var i = 0; i < TestedAllocationCount; i++) compare.Item2();
				watch.Stop();
				var allocateClassTimeMs = watch.ElapsedMilliseconds;

				// clean up and calm down
				GC.Collect();
				watch.Reset();
				Thread.Sleep(5000);

				// measure allocating instances of a struct
				watch.Start();
				for (var i = 0; i < TestedAllocationCount; i++) compare.Item3();
				watch.Stop();
				var allocateStructTimeMs = watch.ElapsedMilliseconds;

				// calculate the speed gain and print the result
				var speedGainClass  = (double) allocateClassReferenceTimeMs  / allocateClassTimeMs;
				var speedGainStruct = (double) allocateStructReferenceTimeMs / allocateStructTimeMs;
				WriteResultLine(
					row++,
					compare.Item1,
					allocateClassTimeMs,
					allocateStructTimeMs,
					speedGainClass,
					speedGainStruct);
			}
		}

		private static long MeasureAllocation<T>(Func<T> action)
		{
			var watch = new Stopwatch();

			// warm up
			GC.Collect();
			action();

			// calm down
			Thread.Sleep(5000);

			// measure
			watch.Start();
			for (var i = 0; i < TestedAllocationCount; i++) action();
			watch.Stop();
			return watch.ElapsedMilliseconds;
		}

		private static void WriteHeader()
		{
			Console.WriteLine("#### {0}", GetRuntimeDescription());
			Console.WriteLine();

			Console.WriteLine(
				$"| {{0,-{MethodColumnWidth}}} | {{1,-{MeasurementColumnWidth}}} | {{2,-{MeasurementColumnWidth}}} |",
				$"Method",
				"Class",
				"Struct");

			Console.WriteLine(
				"|{0}|{1}|{2}|",
				new string('-', MethodColumnWidth      + 2),
				new string('-', MeasurementColumnWidth + 2),
				new string('-', MeasurementColumnWidth + 2));
		}

		private static void WriteResultLine(int row, string method, double classTimeMs, double structTimeMs)
		{
			var format =
				$"| {{0,-{MethodColumnWidth}}} | {{1,-{MeasurementColumnWidth}}} | {{2,-{MeasurementColumnWidth}}} |";
			Console.WriteLine(
				format,
				method,
				$"{classTimeMs} ms",
				$"{structTimeMs} ms");
		}

		private static void WriteResultLine(
			int    row,
			string method,
			double classTimeMs,
			double structTimeMs,
			double classSpeedGain,
			double structSpeedGain)
		{
			var format =
				$"| {{0,-{MethodColumnWidth}}} | {{1,-{MeasurementColumnWidth}}} | {{2,-{MeasurementColumnWidth}}} |";
			Console.WriteLine(
				format,
				method,
				$"{classTimeMs} ms ({classSpeedGain:0.00}x)",
				$"{structTimeMs} ms ({structSpeedGain:0.00}x)");
		}

		private static string GetRuntimeDescription()
		{
#if NETFRAMEWORK
			using (var key = RegistryKey
				.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
				.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
			{
				if (key != null)
				{
					var releaseKey = Convert.ToInt32(key.GetValue("Release"));
					if (releaseKey >= 528040) return ".NET Framework 4.8";
					if (releaseKey >= 461808) return ".NET Framework 4.7.2";
					if (releaseKey >= 461308) return ".NET Framework 4.7.1";
					if (releaseKey >= 460798) return ".NET Framework 4.7";
					if (releaseKey >= 394802) return ".NET Framework 4.6.2";
					if (releaseKey >= 394254) return ".NET Framework 4.6.1";
					if (releaseKey >= 393295) return ".NET Framework 4.6";
					if (releaseKey >= 393273) return ".NET Framework 4.6 RC";
					if (releaseKey >= 379893) return ".NET Framework 4.5.2";
					if (releaseKey >= 378675) return ".NET Framework 4.5.1";
					if (releaseKey >= 378389) return ".NET Framework 4.5";
				}

				return "Unknown .NET Framework Version";
			}
#endif

#if NETCOREAPP
			var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
			var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
			int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
			if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2) {
				var version = assemblyPath[netCoreAppIndex + 1];
				if (version.StartsWith("5.")) return ".NET " + version;
				return ".NET Core " + version;
			}
			return null;
#endif
		}
	}
}
