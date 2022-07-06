///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;

using GriffinPlus.Lib;

using Microsoft.Win32;

// ReSharper disable AccessToModifiedClosure
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable UnusedParameter.Local

namespace GriffinPlus.Benchmark
{

	class Program
	{
		private const long MeasureDurationMs      = 30000;
		private const int  MethodColumnWidth      = 70;
		private const int  MeasurementColumnWidth = 30;

		private static void Main()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			string framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
			Console.WriteLine("Target Framework:  {0}", framework);
			Console.WriteLine("Runtime Version:   {0}", GetRuntimeDescription());

			Console.WriteLine();
			Console.WriteLine("Activator Benchmark:");
			Console.WriteLine();

			RunBenchmark<TestClass<int>, TestStruct<int>>();

			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}

		private class MeasureBlock<TClass, TStruct>
		{
			public string                             Description;
			public MeasureItem<TClass, TStruct>       Reference;
			public List<MeasureItem<TClass, TStruct>> Comparisons;
		}

		private class MeasureItem<TClass, TStruct>
		{
			public string        Description;
			public Func<TClass>  ClassAction;
			public Func<TStruct> StructAction;
		}

		private class RunData
		{
			public readonly Type[]   ConstructorArgumentTypes;
			public readonly int[]    ConstructorArgumentsTyped;
			public readonly object[] ConstructorArgumentsUntyped;

			public RunData(int parameterCount)
			{
				ConstructorArgumentTypes = Enumerable.Repeat(typeof(int), parameterCount).ToArray();
				ConstructorArgumentsTyped = Enumerable.Range(1, parameterCount).ToArray();
				ConstructorArgumentsUntyped = new object[ConstructorArgumentsTyped.Length];
				Array.Copy(ConstructorArgumentsTyped, ConstructorArgumentsUntyped, ConstructorArgumentsTyped.Length);
			}
		}

		private static void RunBenchmark<TClass, TStruct>()
		{
			// -----------------------------------------------------------------------------------------------------------------

			// prepare run data
			var runData1 = new RunData(1);
			var runData2 = new RunData(2);
			var runData3 = new RunData(3);
			var runData4 = new RunData(4);
			var runData5 = new RunData(5);
			var runData6 = new RunData(6);
			var runData7 = new RunData(7);
			var runData8 = new RunData(8);
			var runData9 = new RunData(9);
			var runData10 = new RunData(10);
			var runData11 = new RunData(11);
			var runData12 = new RunData(12);
			var runData13 = new RunData(13);
			var runData14 = new RunData(14);
			var runData15 = new RunData(15);
			var runData16 = new RunData(16);

			// -----------------------------------------------------------------------------------------------------------------

			// define the tests to run
			List<MeasureBlock<TClass, TStruct>> blocks = new List<MeasureBlock<TClass, TStruct>>
			{
				new MeasureBlock<TClass, TStruct>
				{
					Description = "No Constructor Parameters (Generic)",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "T Activator.CreateInstance<T>()",
						ClassAction = () => Activator.CreateInstance<TClass>(),
						StructAction = () => Activator.CreateInstance<TStruct>()
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "T FastActivator<T>.CreateInstance()",
							ClassAction = () => FastActivator<TClass>.CreateInstance(),
							StructAction = () => FastActivator<TStruct>.CreateInstance()
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "No Constructor Parameters (Non-Generic)",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type)",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass)),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct))
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), null),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), null)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance(Type)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass)),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct))
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "1 Constructor Parameter",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData1.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData1.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData1.ConstructorArgumentTypes, runData1.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData1.ConstructorArgumentTypes, runData1.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "2 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData2.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData2.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData2.ConstructorArgumentTypes, runData2.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData2.ConstructorArgumentTypes, runData2.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "3 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData3.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData3.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData3.ConstructorArgumentTypes, runData3.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData3.ConstructorArgumentTypes, runData3.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "4 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData4.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData4.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData4.ConstructorArgumentTypes, runData4.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData4.ConstructorArgumentTypes, runData4.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "5 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData5.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData5.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData5.ConstructorArgumentTypes, runData5.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData5.ConstructorArgumentTypes, runData5.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "6 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData6.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData6.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData6.ConstructorArgumentTypes, runData6.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData6.ConstructorArgumentTypes, runData6.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "7 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData7.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData7.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData7.ConstructorArgumentTypes, runData7.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData7.ConstructorArgumentTypes, runData7.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "8 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData8.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData8.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData8.ConstructorArgumentTypes, runData8.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData8.ConstructorArgumentTypes, runData8.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "9 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData9.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData9.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData9.ConstructorArgumentTypes, runData9.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData9.ConstructorArgumentTypes, runData9.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "10 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData10.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData10.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData10.ConstructorArgumentTypes, runData10.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData10.ConstructorArgumentTypes, runData10.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "11 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData11.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData11.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData11.ConstructorArgumentTypes, runData11.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData11.ConstructorArgumentTypes, runData11.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "12 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData12.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData12.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData12.ConstructorArgumentTypes, runData12.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData12.ConstructorArgumentTypes, runData12.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "13 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData13.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData13.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData13.ConstructorArgumentTypes, runData13.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData13.ConstructorArgumentTypes, runData13.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "14 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData14.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData14.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData14.ConstructorArgumentTypes, runData14.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData14.ConstructorArgumentTypes, runData14.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "15 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData15.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData15.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData15.ConstructorArgumentTypes, runData15.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData15.ConstructorArgumentTypes, runData15.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)
						}
					}
				},
				new MeasureBlock<TClass, TStruct>
				{
					Description = "16 Constructor Parameters",
					Reference = new MeasureItem<TClass, TStruct>
					{
						Description = "object Activator.CreateInstance(Type, object[])",
						ClassAction = () => (TClass)Activator.CreateInstance(typeof(TClass), runData16.ConstructorArgumentsUntyped),
						StructAction = () => (TStruct)Activator.CreateInstance(typeof(TStruct), runData16.ConstructorArgumentsUntyped)
					},
					Comparisons = new List<MeasureItem<TClass, TStruct>>
					{
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstanceDynamically(Type, Type[], object[])",
							ClassAction = () => (TClass)FastActivator.CreateInstanceDynamically(typeof(TClass), runData16.ConstructorArgumentTypes, runData16.ConstructorArgumentsUntyped),
							StructAction = () => (TStruct)FastActivator.CreateInstanceDynamically(typeof(TStruct), runData16.ConstructorArgumentTypes, runData16.ConstructorArgumentsUntyped)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "object FastActivator.CreateInstance<...>(...)",
							ClassAction = () => (TClass)FastActivator.CreateInstance(typeof(TClass), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
							StructAction = () => (TStruct)FastActivator.CreateInstance(typeof(TStruct), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)
						},
						new MeasureItem<TClass, TStruct>
						{
							Description = "T      FastActivator<T>.CreateInstance<...>(...)",
							ClassAction = () => FastActivator<TClass>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
							StructAction = () => FastActivator<TStruct>.CreateInstance(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)
						}
					}
				}
			};

			// -----------------------------------------------------------------------------------------------------------------

			// run all creator functions that will be used in the benchmark to set up the lookup tables
			foreach (var block in blocks)
			{
				block.Reference.ClassAction();
				block.Reference.StructAction();
				foreach (var comparison in block.Comparisons)
				{
					comparison.ClassAction();
					comparison.StructAction();
				}
			}

			// -----------------------------------------------------------------------------------------------------------------

			// run the benchmark
			foreach (var block in blocks)
			{
				int row = 0;

				Console.WriteLine($"### {block.Description}");
				Console.WriteLine();
				WriteHeader();

				// measure the reference (.NET Activator)
				long allocateClassReferenceIterationsPerDuration = MeasureAllocation(block.Reference.ClassAction, out long allocateClassReferenceDuration);
				long allocateStructReferenceIterationsPerDuration = MeasureAllocation(block.Reference.StructAction, out long allocateStructReferenceDuration);
				WriteResultLine(
					row++,
					block.Reference.Description,
					allocateClassReferenceDuration,
					allocateClassReferenceIterationsPerDuration,
					allocateStructReferenceDuration,
					allocateStructReferenceIterationsPerDuration);

				// measure comparisons
				var watch = new Stopwatch();
				foreach (var comparison in block.Comparisons)
				{
					// warm up
					comparison.ClassAction();
					comparison.StructAction();

					// clean up and calm down
					GC.Collect();
					watch.Reset();
					Thread.Sleep(5000);

					// measure allocating instances of a class
					watch.Start();
					for (int i = 0; i < allocateClassReferenceIterationsPerDuration; i++) comparison.ClassAction();
					watch.Stop();
					long allocateClassTimeMs = watch.ElapsedMilliseconds;

					// clean up and calm down
					GC.Collect();
					watch.Reset();
					Thread.Sleep(5000);

					// measure allocating instances of a struct
					watch.Start();
					for (int i = 0; i < allocateStructReferenceIterationsPerDuration; i++) comparison.StructAction();
					watch.Stop();
					long allocateStructTimeMs = watch.ElapsedMilliseconds;

					// calculate the speed gain and print the result
					double speedGainClass = (double)allocateClassReferenceDuration / allocateClassTimeMs;
					double speedGainStruct = (double)allocateStructReferenceDuration / allocateStructTimeMs;
					WriteResultLine(
						row++,
						comparison.Description,
						allocateClassTimeMs,
						allocateStructTimeMs,
						speedGainClass,
						speedGainStruct);
				}

				Console.WriteLine();
				Console.WriteLine();
			}
		}

		private static long MeasureAllocation<T>(Func<T> action, out long measuredTimeMs)
		{
			measuredTimeMs = 0;

			// warm up
			GC.Collect();
			action();

			// calm down
			Thread.Sleep(5000);

			// measure
			long startTicks = Stopwatch.GetTimestamp();
			long measureDurationTicks = MeasureDurationMs * Stopwatch.Frequency / 1000;
			long iterations = 0;
			while (true)
			{
				long elapsedTicks = Stopwatch.GetTimestamp() - startTicks;
				if (elapsedTicks > measureDurationTicks)
				{
					measuredTimeMs = 1000 * elapsedTicks / Stopwatch.Frequency;
					break;
				}

				action();
				iterations++;
			}

			return iterations;
		}

		private static void WriteHeader()
		{
			Console.WriteLine("#### {0}", GetRuntimeDescription());
			Console.WriteLine();

			Console.WriteLine(
				$"| {{0,-{MethodColumnWidth}}} | {{1,-{MeasurementColumnWidth}}} | {{2,-{MeasurementColumnWidth}}} |",
				"Method",
				"Class",
				"Struct");

			Console.WriteLine(
				"|{0}|{1}|{2}|",
				new string('-', MethodColumnWidth + 2),
				new string('-', MeasurementColumnWidth + 2),
				new string('-', MeasurementColumnWidth + 2));
		}

		private static void WriteResultLine(
			int    row,
			string method,
			double classTimeMs,
			long   allocateClassReferenceIterationsPerDuration,
			double structTimeMs,
			long   allocateStructReferenceIterationsPerDuration)
		{
			string format =
				$"| {{0,-{MethodColumnWidth}}} | {{1,-{MeasurementColumnWidth}}} | {{2,-{MeasurementColumnWidth}}} |";
			Console.WriteLine(
				format,
				method,
				$"{classTimeMs} ms ({allocateClassReferenceIterationsPerDuration} allocs)",
				$"{structTimeMs} ms ({allocateStructReferenceIterationsPerDuration} allocs)");
		}

		private static void WriteResultLine(
			int    row,
			string method,
			double classTimeMs,
			double structTimeMs,
			double classSpeedGain,
			double structSpeedGain)
		{
			string format =
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
					int releaseKey = Convert.ToInt32(key.GetValue("Release"));
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
			string[] assemblyPath = assembly.Location.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
			int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
			if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
			{
				string version = assemblyPath[netCoreAppIndex + 1];
				if (version.StartsWith("5.")) return ".NET " + version;
				return ".NET Core " + version;
			}

			return null;
#endif
		}
	}

}
