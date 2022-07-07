# Griffin+ Fast Activator

[![Azure DevOps builds (branch)](https://img.shields.io/azure-devops/build/griffinplus/2f589a5e-e2ab-4c08-bee5-5356db2b2aeb/33/master?label=Build)](https://dev.azure.com/griffinplus/DotNET%20Libraries/_build/latest?definitionId=33&branchName=master)
[![Tests (master)](https://img.shields.io/azure-devops/tests/griffinplus/DotNET%20Libraries/33/master?label=Tests)](https://dev.azure.com/griffinplus/DotNET%20Libraries/_build/latest?definitionId=33&branchName=master)
[![NuGet Version](https://img.shields.io/nuget/v/GriffinPlus.Lib.FastActivator.svg)](https://www.nuget.org/packages/GriffinPlus.Lib.FastActivator)
[![NuGet Downloads](https://img.shields.io/nuget/dt/GriffinPlus.Lib.FastActivator.svg)](https://www.nuget.org/packages/GriffinPlus.Lib.FastActivator)

## Overview

The *Fast Activator* is part of the Griffin+ Common Library Suite. It replaces some of the functions of the .NET `System.Activator` class. `System.Activator` is the standard tool for dynamic instantiation of types (classes/structures) under .NET. It is used, if you do not know which specific type must be instantiated at the time of compilation. Instantiating classes/structures in the same process is part of this functionality, but an immensely important part. All serialization mechanisms require this functionality to generate instances of certain types when deserializing objects.

The standard activator of .NET decides which constructor to call from concrete constructor arguments. This procedure is very flexible, but also time-consuming. In most cases, you know which constructor you need. So you do not have to have it search again with every instantiation. In contrast *Fast Activator* requires the explicit specification of the parameter types of the constructor to be used. It then dynamically generates functions that only do what you would do in the code to instantiate the type in question, namely the creation ala `new MyType(arg1, arg2,...)`. These dynamically generated functions are cached and are immediately available for the next use. The performance increase achieved is enormous (see benchmarks below).

## Supported Platforms

The library is entirely written in C# using .NET Standard 2.0.

Therefore it should work on the following platforms (or higher):
- .NET Framework 4.6.1
- .NET Core 2.0
- .NET 5.0
- .NET 6.0
- Mono 5.4
- Xamarin iOS 10.14
- Xamarin Mac 3.8
- Xamarin Android 8.0
- Universal Windows Platform (UWP) 10.0.16299

The library is tested automatically on the following frameworks and operating systems:
- .NET Framework 4.8 (Windows Server 2019)
- .NET Core 3.1 (Windows Server 2019 and Ubuntu 20.04)
- .NET 5.0 (Windows Server 2019 and Ubuntu 20.04)
- .NET 6.0 (Windows Server 2019 and Ubuntu 20.04)

## Using

The *Fast Activator* consists of the two classes `FastActivator` and `FastActivator<T>`. Both classes create instances of classes and structures and support constructors with up to 16 parameters. The generation methods of the `FastActivator` class always have the return type `System.Object`. The generic `FastActivator<T>` class provides generation methods whose return type corresponds to the type to be instantiated. `FastActivator<T>` is particularly recommended for instantiating value types (structs) since it does not box the returned values. Omitting the boxing operation speeds up the application in general reducing the load on the garbage collector.

### Class: GriffinPlus.Lib.FastActivator

If you need to create an instance of a certain type that is not known at compile time, you can use the following methods:

```csharp
public static object CreateInstance(Type type);
public static object CreateInstanceDynamically(Type type, Type[] constructorParameterTypes, params object[] args);
```

If you need to create an instance of a certain type that is not known at compile time, but whose constructor parameter types are known, you can use one of the following methods:

```csharp
public static object CreateInstance<TArg>(Type type, TArg arg);
public static object CreateInstance<TArg1, TArg2>(Type type, TArg1 arg1, TArg2 arg2);
public static object CreateInstance<TArg1, TArg2, TArg3>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15);
public static object CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, TArg16 arg16);
```

If you need to create a one-dimensional array of a certain type that is not known at compile time, you can use:

```csharp
public static Array CreateArray(Type type, int length);
```

### Class: GriffinPlus.Lib.FastActivator&lt;T&gt;

If you need to create an instance of a certain type that is known at compile time or available via generic type arguments, you can use:

```csharp
public static T CreateInstance();
public static T CreateInstance<TArg>(TArg arg);
public static T CreateInstance<TArg1, TArg2>(TArg1 arg1, TArg2 arg2);
public static T CreateInstance<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15);
public static T CreateInstance<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TArg16>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, TArg16 arg16);
```

If you need to create a one-dimensional array of a certain type that is not known at compile time, you can use:

```csharp
public static T[] CreateArray(int length);
```

### Class: GriffinPlus.Lib.FastCreator&lt;TCreator&gt;

If you need to create an instance of a certain type that is not known at compile time, you can use the following method to obtain a delegate that creates an instance of the specified type passing the delegate's arguments to the invoked constructor. The creator delegate `TCreator` must therefore have return type `System.Object` and parameters that exactly match the parameter types of the constructor to invoke. To create an instance of the specified type, you simply need to invoke the delegate and pass the appropriate constructor parameters.

```csharp
public static TCreator GetCreator(Type type);
```

In contrast to `FastActivator` and `FastActivator<T>` the `FastCreator<TCreator>` class allows to dynamically instantiate types that use `ref struct` types as constructor parameters. These types can only exist on the stack and are therefore not usable as generic arguments to methods of the `FastActivator` and `FastActivator<T>` class.

## Benchmark

To compare the performance of the *Fast Activator* with .NET's default activator a benchmark application is part of the repository. The benchmark creates instances of a test class/struct using the default activator over 30 seconds. Then it invokes the different *Fast Activator* methods to allocate the same amount of instances of the test class/struct and compares the time it takes with the reference time (30 seconds).

The executing runtimes are *.NET Framework 4.8*, *.NET Core 2.1.30*, *.NET Core 3.1.26*, *.NET 5.0.17* and *.NET Core 6.0.6*.

### No Constructor Parameters (Generic)

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| T Activator.CreateInstance<T>()                                        | 30000 ms (248406563 allocs)    | 30000 ms (309027688 allocs)    |
| T FastActivator<T>.CreateInstance()                                    | 6324 ms (4.74x)                | 1815 ms (16.53x)               |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| T Activator.CreateInstance<T>()                                        | 30000 ms (279140400 allocs)    | 30000 ms (322085316 allocs)    |
| T FastActivator<T>.CreateInstance()                                    | 5423 ms (5.53x)                | 3105 ms (9.66x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| T Activator.CreateInstance<T>()                                        | 30000 ms (409009105 allocs)    | 30000 ms (1301563602 allocs)   |
| T FastActivator<T>.CreateInstance()                                    | 6808 ms (4.41x)                | 6547 ms (4.58x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| T Activator.CreateInstance<T>()                                        | 30000 ms (383669160 allocs)    | 30000 ms (1357887883 allocs)   |
| T FastActivator<T>.CreateInstance()                                    | 6271 ms (4.78x)                | 6319 ms (4.75x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| T Activator.CreateInstance<T>()                                        | 30000 ms (661720275 allocs)    | 30000 ms (1380911145 allocs)   |
| T FastActivator<T>.CreateInstance()                                    | 11222 ms (2.67x)               | 5877 ms (5.10x)                |


### No Constructor Parameters (Non-Generic)

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type)                                  | 30000 ms (258305496 allocs)    | 30000 ms (312221890 allocs)    |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 17751 ms (1.69x)               | 16426 ms (1.83x)               |
| object FastActivator.CreateInstance(Type)                              | 17203 ms (1.74x)               | 13475 ms (2.23x)               |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type)                                  | 30000 ms (281129482 allocs)    | 30000 ms (331151320 allocs)    |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 14077 ms (2.13x)               | 15523 ms (1.93x)               |
| object FastActivator.CreateInstance(Type)                              | 10893 ms (2.75x)               | 11909 ms (2.52x)               |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type)                                  | 30000 ms (396196349 allocs)    | 30000 ms (453777261 allocs)    |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 15598 ms (1.92x)               | 16297 ms (1.84x)               |
| object FastActivator.CreateInstance(Type)                              | 15356 ms (1.95x)               | 17602 ms (1.70x)               |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type)                                  | 30000 ms (366347798 allocs)    | 30000 ms (452848452 allocs)    |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 15036 ms (2.00x)               | 15905 ms (1.89x)               |
| object FastActivator.CreateInstance(Type)                              | 14526 ms (2.07x)               | 17540 ms (1.71x)               |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type)                                  | 30000 ms (694311549 allocs)    | 30000 ms (808396763 allocs)    |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 27068 ms (1.11x)               | 28213 ms (1.06x)               |
| object FastActivator.CreateInstance(Type)                              | 26913 ms (1.11x)               | 28359 ms (1.06x)               |


### 1 Constructor Parameter

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (19610647 allocs)     | 30000 ms (20068434 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2109 ms (14.22x)               | 2044 ms (14.68x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1488 ms (20.16x)               | 1384 ms (21.68x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 729 ms (41.15x)                | 449 ms (66.82x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (22409851 allocs)     | 30000 ms (23047043 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1829 ms (16.40x)               | 1938 ms (15.48x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1163 ms (25.80x)               | 1095 ms (27.40x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 709 ms (42.31x)                | 402 ms (74.63x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (24112583 allocs)     | 30000 ms (24960225 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1631 ms (18.39x)               | 1606 ms (18.68x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1245 ms (24.10x)               | 1239 ms (24.21x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 666 ms (45.05x)                | 337 ms (89.02x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (26304892 allocs)     | 30000 ms (27403443 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1678 ms (17.88x)               | 1749 ms (17.15x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1438 ms (20.86x)               | 1365 ms (21.98x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 781 ms (38.41x)                | 330 ms (90.91x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (28352638 allocs)     | 30000 ms (29188619 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1800 ms (16.67x)               | 1764 ms (17.01x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1486 ms (20.19x)               | 1381 ms (21.72x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 808 ms (37.13x)                | 318 ms (94.34x)                |


### 2 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (17600678 allocs)     | 30000 ms (18168135 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2376 ms (12.63x)               | 2331 ms (12.87x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1311 ms (22.88x)               | 1251 ms (23.98x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 672 ms (44.64x)                | 431 ms (69.61x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (20276906 allocs)     | 30000 ms (20727750 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1934 ms (15.51x)               | 1915 ms (15.67x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1149 ms (26.11x)               | 1042 ms (28.79x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 654 ms (45.87x)                | 386 ms (77.72x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (21832859 allocs)     | 30000 ms (22509691 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1785 ms (16.81x)               | 1792 ms (16.74x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1165 ms (25.75x)               | 1121 ms (26.76x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 697 ms (43.04x)                | 308 ms (97.40x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (23981345 allocs)     | 30000 ms (24703444 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1913 ms (15.68x)               | 1891 ms (15.86x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1377 ms (21.79x)               | 1235 ms (24.29x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 747 ms (40.16x)                | 308 ms (97.40x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (25914906 allocs)     | 30000 ms (26520887 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2053 ms (14.61x)               | 1988 ms (15.09x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1443 ms (20.79x)               | 1332 ms (22.52x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 762 ms (39.37x)                | 302 ms (99.34x)                |


### 3 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (16139621 allocs)     | 30000 ms (16283357 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2569 ms (11.68x)               | 2535 ms (11.83x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1263 ms (23.75x)               | 1161 ms (25.84x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 636 ms (47.17x)                | 379 ms (79.16x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (18450051 allocs)     | 30000 ms (18744339 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2118 ms (14.16x)               | 2075 ms (14.46x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1024 ms (29.30x)               | 916 ms (32.75x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 631 ms (47.54x)                | 349 ms (85.96x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (20078984 allocs)     | 30000 ms (20620725 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1949 ms (15.39x)               | 1950 ms (15.38x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1072 ms (27.99x)               | 1033 ms (29.04x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 618 ms (48.54x)                | 322 ms (93.17x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (22050722 allocs)     | 30000 ms (22490895 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2044 ms (14.68x)               | 2073 ms (14.47x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1284 ms (23.36x)               | 1149 ms (26.11x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 733 ms (40.93x)                | 315 ms (95.24x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (23565196 allocs)     | 30000 ms (24205097 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2208 ms (13.59x)               | 2297 ms (13.06x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1325 ms (22.64x)               | 1195 ms (25.10x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 746 ms (40.21x)                | 297 ms (101.01x)               |


### 4 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (14917385 allocs)     | 30000 ms (15255299 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2789 ms (10.76x)               | 2752 ms (10.90x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1152 ms (26.04x)               | 1057 ms (28.38x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 621 ms (48.31x)                | 413 ms (72.64x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (16855472 allocs)     | 30000 ms (17189367 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2289 ms (13.11x)               | 2283 ms (13.14x)               |
| object FastActivator.CreateInstance<...>(...)                          | 953 ms (31.48x)                | 864 ms (34.72x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 594 ms (50.51x)                | 357 ms (84.03x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (18651058 allocs)     | 30000 ms (19220768 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2181 ms (13.76x)               | 2240 ms (13.39x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1057 ms (28.38x)               | 1013 ms (29.62x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 636 ms (47.17x)                | 344 ms (87.21x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (20088163 allocs)     | 30000 ms (20778953 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2235 ms (13.42x)               | 2227 ms (13.47x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1177 ms (25.49x)               | 1034 ms (29.01x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 729 ms (41.15x)                | 310 ms (96.77x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (22068594 allocs)     | 30000 ms (22356283 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2375 ms (12.63x)               | 2376 ms (12.63x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1283 ms (23.38x)               | 1108 ms (27.08x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 797 ms (37.64x)                | 325 ms (92.31x)                |


### 5 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (13652167 allocs)     | 30000 ms (13886464 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3030 ms (9.90x)                | 2901 ms (10.34x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1066 ms (28.14x)               | 980 ms (30.61x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 586 ms (51.19x)                | 370 ms (81.08x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (15328028 allocs)     | 30000 ms (15574879 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2351 ms (12.76x)               | 2357 ms (12.73x)               |
| object FastActivator.CreateInstance<...>(...)                          | 906 ms (33.11x)                | 798 ms (37.59x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 559 ms (53.67x)                | 345 ms (86.96x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (17286859 allocs)     | 30000 ms (17910211 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2363 ms (12.70x)               | 2366 ms (12.68x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1034 ms (29.01x)               | 953 ms (31.48x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 609 ms (49.26x)                | 328 ms (91.46x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (18619360 allocs)     | 30000 ms (19173151 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2362 ms (12.70x)               | 2429 ms (12.35x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1130 ms (26.55x)               | 968 ms (30.99x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 682 ms (43.99x)                | 306 ms (98.04x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (20359619 allocs)     | 30000 ms (20705912 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2421 ms (12.39x)               | 2477 ms (12.11x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1077 ms (27.86x)               | 1059 ms (28.33x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 691 ms (43.42x)                | 314 ms (95.54x)                |


### 6 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (12698507 allocs)     | 30000 ms (12858127 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3142 ms (9.55x)                | 3053 ms (9.83x)                |
| object FastActivator.CreateInstance<...>(...)                          | 1050 ms (28.57x)               | 916 ms (32.75x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 595 ms (50.42x)                | 382 ms (78.53x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (13953987 allocs)     | 30000 ms (14401422 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2428 ms (12.36x)               | 2415 ms (12.42x)               |
| object FastActivator.CreateInstance<...>(...)                          | 903 ms (33.22x)                | 774 ms (38.76x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 613 ms (48.94x)                | 324 ms (92.59x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (15912914 allocs)     | 30000 ms (16267661 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2481 ms (12.09x)               | 2379 ms (12.61x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1025 ms (29.27x)               | 897 ms (33.44x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 653 ms (45.94x)                | 305 ms (98.36x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (17238636 allocs)     | 30000 ms (17589125 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2511 ms (11.95x)               | 2368 ms (12.67x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1139 ms (26.34x)               | 881 ms (34.05x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 714 ms (42.02x)                | 245 ms (122.45x)               |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (18771454 allocs)     | 30000 ms (19073505 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2514 ms (11.93x)               | 2515 ms (11.93x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1141 ms (26.29x)               | 1018 ms (29.47x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 680 ms (44.12x)                | 293 ms (102.39x)               |


### 7 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11969034 allocs)     | 30000 ms (12027153 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3303 ms (9.08x)                | 3309 ms (9.07x)                |
| object FastActivator.CreateInstance<...>(...)                          | 978 ms (30.67x)                | 1013 ms (29.62x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 654 ms (45.87x)                | 460 ms (65.22x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (13265446 allocs)     | 30000 ms (13435643 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2643 ms (11.35x)               | 2610 ms (11.49x)               |
| object FastActivator.CreateInstance<...>(...)                          | 852 ms (35.21x)                | 903 ms (33.22x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 573 ms (52.36x)                | 457 ms (65.65x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (14822129 allocs)     | 30000 ms (15098284 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2549 ms (11.77x)               | 2594 ms (11.57x)               |
| object FastActivator.CreateInstance<...>(...)                          | 939 ms (31.95x)                | 1043 ms (28.76x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 622 ms (48.23x)                | 456 ms (65.79x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (16161074 allocs)     | 30000 ms (16632029 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2601 ms (11.53x)               | 2646 ms (11.34x)               |
| object FastActivator.CreateInstance<...>(...)                          | 1021 ms (29.38x)               | 1037 ms (28.93x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 748 ms (40.11x)                | 404 ms (74.26x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (17207381 allocs)     | 30000 ms (17451487 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2561 ms (11.71x)               | 2523 ms (11.89x)               |
| object FastActivator.CreateInstance<...>(...)                          | 970 ms (30.93x)                | 865 ms (34.68x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 628 ms (47.77x)                | 288 ms (104.17x)               |


### 8 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11339894 allocs)     | 30000 ms (11454775 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3442 ms (8.72x)                | 3507 ms (8.55x)                |
| object FastActivator.CreateInstance<...>(...)                          | 943 ms (31.81x)                | 978 ms (30.67x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 610 ms (49.18x)                | 506 ms (59.29x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (12606652 allocs)     | 30000 ms (12728375 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2741 ms (10.94x)               | 2710 ms (11.07x)               |
| object FastActivator.CreateInstance<...>(...)                          | 831 ms (36.10x)                | 907 ms (33.08x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 580 ms (51.72x)                | 510 ms (58.82x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (14164219 allocs)     | 30000 ms (14382902 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2653 ms (11.31x)               | 2669 ms (11.24x)               |
| object FastActivator.CreateInstance<...>(...)                          | 916 ms (32.75x)                | 1007 ms (29.79x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 603 ms (49.75x)                | 476 ms (63.03x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (15234415 allocs)     | 30000 ms (15535339 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2621 ms (11.45x)               | 2697 ms (11.12x)               |
| object FastActivator.CreateInstance<...>(...)                          | 940 ms (31.91x)                | 968 ms (30.99x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 691 ms (43.42x)                | 378 ms (79.37x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (16230232 allocs)     | 30000 ms (16523130 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2788 ms (10.76x)               | 2616 ms (11.47x)               |
| object FastActivator.CreateInstance<...>(...)                          | 961 ms (31.22x)                | 821 ms (36.54x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 708 ms (42.37x)                | 275 ms (109.09x)               |


### 9 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (10649894 allocs)     | 30000 ms (10820239 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3544 ms (8.47x)                | 3564 ms (8.42x)                |
| object FastActivator.CreateInstance<...>(...)                          | 916 ms (32.75x)                | 921 ms (32.57x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 595 ms (50.42x)                | 488 ms (61.48x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11913462 allocs)     | 30000 ms (12057875 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2912 ms (10.30x)               | 2922 ms (10.27x)               |
| object FastActivator.CreateInstance<...>(...)                          | 783 ms (38.31x)                | 830 ms (36.14x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 560 ms (53.57x)                | 518 ms (57.92x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (13436613 allocs)     | 30000 ms (13694763 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2733 ms (10.98x)               | 2785 ms (10.77x)               |
| object FastActivator.CreateInstance<...>(...)                          | 850 ms (35.29x)                | 926 ms (32.40x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 590 ms (50.85x)                | 508 ms (59.06x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (14230578 allocs)     | 30000 ms (14469241 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2691 ms (11.15x)               | 2686 ms (11.17x)               |
| object FastActivator.CreateInstance<...>(...)                          | 860 ms (34.88x)                | 867 ms (34.60x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 665 ms (45.11x)                | 394 ms (76.14x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (15543078 allocs)     | 30000 ms (15622342 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2907 ms (10.32x)               | 2838 ms (10.57x)               |
| object FastActivator.CreateInstance<...>(...)                          | 924 ms (32.47x)                | 984 ms (30.49x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 689 ms (43.54x)                | 409 ms (73.35x)                |


### 10 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (10187587 allocs)     | 30000 ms (10375926 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3689 ms (8.13x)                | 3764 ms (7.97x)                |
| object FastActivator.CreateInstance<...>(...)                          | 890 ms (33.71x)                | 891 ms (33.67x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 615 ms (48.78x)                | 480 ms (62.50x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30014 ms (11185227 allocs)     | 30000 ms (11372751 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2922 ms (10.27x)               | 2953 ms (10.16x)               |
| object FastActivator.CreateInstance<...>(...)                          | 798 ms (37.61x)                | 818 ms (36.67x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 526 ms (57.06x)                | 565 ms (53.10x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (12813530 allocs)     | 30000 ms (13045291 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2921 ms (10.27x)               | 2925 ms (10.26x)               |
| object FastActivator.CreateInstance<...>(...)                          | 810 ms (37.04x)                | 885 ms (33.90x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 581 ms (51.64x)                | 491 ms (61.10x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (13605061 allocs)     | 30000 ms (13895595 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2794 ms (10.74x)               | 2774 ms (10.81x)               |
| object FastActivator.CreateInstance<...>(...)                          | 849 ms (35.34x)                | 877 ms (34.21x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 647 ms (46.37x)                | 403 ms (74.44x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (14916592 allocs)     | 30000 ms (14997479 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2970 ms (10.10x)               | 2932 ms (10.23x)               |
| object FastActivator.CreateInstance<...>(...)                          | 905 ms (33.15x)                | 967 ms (31.02x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 716 ms (41.90x)                | 445 ms (67.42x)                |


### 11 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (9690267 allocs)      | 30000 ms (9797410 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3726 ms (8.05x)                | 3794 ms (7.91x)                |
| object FastActivator.CreateInstance<...>(...)                          | 866 ms (34.64x)                | 889 ms (33.75x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 590 ms (50.85x)                | 478 ms (62.76x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (10752784 allocs)     | 30000 ms (10877519 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3058 ms (9.81x)                | 3104 ms (9.66x)                |
| object FastActivator.CreateInstance<...>(...)                          | 766 ms (39.16x)                | 776 ms (38.66x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 518 ms (57.92x)                | 496 ms (60.48x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (12216846 allocs)     | 30000 ms (12395269 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2969 ms (10.10x)               | 3037 ms (9.88x)                |
| object FastActivator.CreateInstance<...>(...)                          | 838 ms (35.80x)                | 849 ms (35.34x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 558 ms (53.76x)                | 507 ms (59.17x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (13066504 allocs)     | 30000 ms (13217065 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2896 ms (10.36x)               | 2835 ms (10.58x)               |
| object FastActivator.CreateInstance<...>(...)                          | 827 ms (36.28x)                | 827 ms (36.28x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 647 ms (46.37x)                | 413 ms (72.64x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (14075052 allocs)     | 30000 ms (14269091 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2953 ms (10.16x)               | 2952 ms (10.16x)               |
| object FastActivator.CreateInstance<...>(...)                          | 879 ms (34.13x)                | 921 ms (32.57x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 686 ms (43.73x)                | 431 ms (69.61x)                |


### 12 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (9283132 allocs)      | 30000 ms (9408756 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3838 ms (7.82x)                | 3871 ms (7.75x)                |
| object FastActivator.CreateInstance<...>(...)                          | 820 ms (36.59x)                | 837 ms (35.84x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 543 ms (55.25x)                | 456 ms (65.79x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (10300594 allocs)     | 30000 ms (10382381 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3124 ms (9.60x)                | 3114 ms (9.63x)                |
| object FastActivator.CreateInstance<...>(...)                          | 781 ms (38.41x)                | 843 ms (35.59x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 516 ms (58.14x)                | 525 ms (57.14x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11691225 allocs)     | 30000 ms (11900206 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3020 ms (9.93x)                | 3065 ms (9.79x)                |
| object FastActivator.CreateInstance<...>(...)                          | 812 ms (36.95x)                | 830 ms (36.14x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 600 ms (50.00x)                | 565 ms (53.10x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (12527509 allocs)     | 30000 ms (12683226 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2886 ms (10.40x)               | 2964 ms (10.12x)               |
| object FastActivator.CreateInstance<...>(...)                          | 809 ms (37.08x)                | 811 ms (36.99x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 640 ms (46.88x)                | 424 ms (70.75x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (13477361 allocs)     | 30000 ms (13754709 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3049 ms (9.84x)                | 3045 ms (9.85x)                |
| object FastActivator.CreateInstance<...>(...)                          | 845 ms (35.50x)                | 935 ms (32.09x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 673 ms (44.58x)                | 444 ms (67.57x)                |


### 13 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (8874777 allocs)      | 30000 ms (9027632 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4022 ms (7.46x)                | 4072 ms (7.37x)                |
| object FastActivator.CreateInstance<...>(...)                          | 810 ms (37.04x)                | 834 ms (35.97x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 534 ms (56.18x)                | 455 ms (65.93x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (9442391 allocs)      | 30000 ms (9796135 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2964 ms (10.12x)               | 3054 ms (9.82x)                |
| object FastActivator.CreateInstance<...>(...)                          | 687 ms (43.67x)                | 739 ms (40.60x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 503 ms (59.64x)                | 441 ms (68.03x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11216173 allocs)     | 30000 ms (11382814 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3212 ms (9.34x)                | 3230 ms (9.29x)                |
| object FastActivator.CreateInstance<...>(...)                          | 806 ms (37.22x)                | 848 ms (35.38x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 562 ms (53.38x)                | 487 ms (61.60x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11991210 allocs)     | 30000 ms (12126098 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2934 ms (10.22x)               | 3052 ms (9.83x)                |
| object FastActivator.CreateInstance<...>(...)                          | 868 ms (34.56x)                | 1130 ms (26.55x)               |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 635 ms (47.24x)                | 431 ms (69.61x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (12924017 allocs)     | 30000 ms (13111889 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3126 ms (9.60x)                | 3178 ms (9.44x)                |
| object FastActivator.CreateInstance<...>(...)                          | 861 ms (34.84x)                | 877 ms (34.21x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 658 ms (45.59x)                | 434 ms (69.12x)                |


### 14 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (8504772 allocs)      | 30000 ms (8598534 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4057 ms (7.39x)                | 4112 ms (7.30x)                |
| object FastActivator.CreateInstance<...>(...)                          | 859 ms (34.92x)                | 862 ms (34.80x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 531 ms (56.50x)                | 442 ms (67.87x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (9322866 allocs)      | 30000 ms (9372127 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3180 ms (9.43x)                | 3167 ms (9.47x)                |
| object FastActivator.CreateInstance<...>(...)                          | 703 ms (42.67x)                | 699 ms (42.92x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 502 ms (59.76x)                | 433 ms (69.28x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (10750809 allocs)     | 30000 ms (10865700 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3270 ms (9.17x)                | 3241 ms (9.26x)                |
| object FastActivator.CreateInstance<...>(...)                          | 779 ms (38.51x)                | 817 ms (36.72x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 548 ms (54.74x)                | 475 ms (63.16x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11479900 allocs)     | 30000 ms (11590854 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2973 ms (10.09x)               | 3041 ms (9.87x)                |
| object FastActivator.CreateInstance<...>(...)                          | 785 ms (38.22x)                | 780 ms (38.46x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 647 ms (46.37x)                | 401 ms (74.81x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (12336696 allocs)     | 30000 ms (12529943 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3187 ms (9.41x)                | 3249 ms (9.23x)                |
| object FastActivator.CreateInstance<...>(...)                          | 834 ms (35.97x)                | 916 ms (32.75x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 665 ms (45.11x)                | 417 ms (71.94x)                |


### 15 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (8175038 allocs)      | 30000 ms (8185260 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4244 ms (7.07x)                | 4211 ms (7.12x)                |
| object FastActivator.CreateInstance<...>(...)                          | 769 ms (39.01x)                | 768 ms (39.06x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 519 ms (57.80x)                | 440 ms (68.18x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (9060290 allocs)      | 30000 ms (9098544 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3220 ms (9.32x)                | 3262 ms (9.20x)                |
| object FastActivator.CreateInstance<...>(...)                          | 692 ms (43.35x)                | 696 ms (43.10x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 523 ms (57.36x)                | 442 ms (67.87x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (10214468 allocs)     | 30000 ms (10457252 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3368 ms (8.91x)                | 3395 ms (8.84x)                |
| object FastActivator.CreateInstance<...>(...)                          | 754 ms (39.79x)                | 809 ms (37.08x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 545 ms (55.05x)                | 493 ms (60.85x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11039596 allocs)     | 30000 ms (11177193 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3136 ms (9.57x)                | 3184 ms (9.42x)                |
| object FastActivator.CreateInstance<...>(...)                          | 771 ms (38.91x)                | 762 ms (39.37x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 641 ms (46.80x)                | 419 ms (71.60x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11938048 allocs)     | 30000 ms (12084904 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3197 ms (9.38x)                | 3256 ms (9.21x)                |
| object FastActivator.CreateInstance<...>(...)                          | 814 ms (36.86x)                | 833 ms (36.01x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 649 ms (46.22x)                | 418 ms (71.77x)                |


### 16 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (7853410 allocs)      | 30000 ms (8001635 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4256 ms (7.05x)                | 4315 ms (6.95x)                |
| object FastActivator.CreateInstance<...>(...)                          | 814 ms (36.86x)                | 825 ms (36.36x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 528 ms (56.82x)                | 442 ms (67.87x)                |

#### .NET Core 2.1.30

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (8709439 allocs)      | 30000 ms (8731000 allocs)      |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3325 ms (9.02x)                | 3280 ms (9.15x)                |
| object FastActivator.CreateInstance<...>(...)                          | 676 ms (44.38x)                | 671 ms (44.71x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 493 ms (60.85x)                | 446 ms (67.26x)                |

#### .NET Core 3.1.26

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (9974437 allocs)      | 30000 ms (10053379 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3431 ms (8.74x)                | 3433 ms (8.74x)                |
| object FastActivator.CreateInstance<...>(...)                          | 750 ms (40.00x)                | 776 ms (38.66x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 562 ms (53.38x)                | 479 ms (62.63x)                |

#### .NET 5.0.17

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (10659969 allocs)     | 30000 ms (10810006 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3140 ms (9.55x)                | 3241 ms (9.26x)                |
| object FastActivator.CreateInstance<...>(...)                          | 815 ms (36.81x)                | 842 ms (35.63x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 634 ms (47.32x)                | 482 ms (62.24x)                |

#### .NET Core 6.0.6

| Method                                                                 | Class                          | Struct                         |
|------------------------------------------------------------------------|--------------------------------|--------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 30000 ms (11501519 allocs)     | 30000 ms (11708069 allocs)     |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3262 ms (9.20x)                | 3317 ms (9.04x)                |
| object FastActivator.CreateInstance<...>(...)                          | 848 ms (35.38x)                | 895 ms (33.52x)                |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 655 ms (45.80x)                | 473 ms (63.42x)                |

