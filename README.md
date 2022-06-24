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
- .NET Core 2.1 (Windows Server 2019 and Ubuntu 20.04)
- .NET Core 3.1 (Windows Server 2019 and Ubuntu 20.04)
- .NET 5.0 (Windows Server 2019 and Ubuntu 20.04)
- .NET 6.0 (Windows Server 2019 and Ubuntu 20.04)

## Using

The *Fast Activator* consists of the two classes `FastActivator` and `FastActivator<T>`. Both classes create instances of classes and structures and support constructors with up to 16 parameters. The generation methods of the `FastActivator` class always have the return type `System.Object`. The generic `FastActivator<T>` class provides generation methods whose return type corresponds to the type to be instantiated. `FastActivator<T>` is particularly recommended for instantiating value types (structs) since it does not box the returned values. Omitting the boxing operation speeds up the application in general reducing the load on the garbage collector. The non-generic `FastActivator` delivers better results for reference types (classes).

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

## Benchmark

To compare the performance of the *Fast Activator* with .NET's default activator a benchmark application is part of the repository. The benchmark creates 50 million instances of a test class/struct using the default activator and the different *Fast Activator* methods to call the constructors of the test class/struct. The test class/struct contains a parameterless constructor and constructors taking up to sixteen integer arguments. The benchmark measures the time the instantiation needs to complete and compares the times measured for the *Fast Activator* to the time the standard activator needed to complete.

The executing runtimes are *.NET Framework 4.8*, *.NET Core 2.1.23*, *.NET Core 3.1.9* and *.NET 5.0.0*.

### No Constructor Parameters (Generic)

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| T Activator.CreateInstance<T>()                                        | 10162 ms               | 6869 ms                |
| T FastActivator<T>.CreateInstance()                                    | **3596 ms (2.83x)**    | **1268 ms (5.42x)**    |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| T Activator.CreateInstance<T>()                                        | 6793 ms                | 5872 ms                |
| T FastActivator<T>.CreateInstance()                                    | **2585 ms (2.63x)**    | **1669 ms (3.52x)**    |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| T Activator.CreateInstance<T>()                                        | 5889 ms                | **694 ms**             |
| T FastActivator<T>.CreateInstance()                                    | **2344 ms (2.51x)**    | 1240 ms (0.56x)        |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| T Activator.CreateInstance<T>()                                        | 5425 ms                | **625 ms**             |
| T FastActivator<T>.CreateInstance()                                    | **2556 ms (2.12x)**    | 919 ms (0.68x)         |


### No Constructor Parameters (Non-Generic)

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type)                                  | 9605 ms                | 6737 ms                |
| object FastActivator.CreateInstance(Type)                              | **7726 ms (1.24x)**    | **6736 ms (1.00x)**    |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type)                                  | 6832 ms                | **5429 ms**            |
| object FastActivator.CreateInstance(Type)                              | **6003 ms (1.14x)**    | 5683 ms (0.96x)        |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type)                                  | 6301 ms                | **5188 ms**            |
| object FastActivator.CreateInstance(Type)                              | **6157 ms (1.02x)**    | 6053 ms (0.86x)        |

### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type)                                  | 6411 ms                | **4871 ms**            |
| object FastActivator.CreateInstance(Type)                              | **5812 ms (1.10x)**    | 5530 ms (0.88x)        |


### 1 Constructor Parameter

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 139812 ms              | 137978 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 23591 ms (5.93x)       | 23264 ms (5.93x)       |
| object FastActivator.CreateInstance<...>(...)                          | **13709 ms (10.20x)**  | 13094 ms (10.54x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16400 ms (8.53x)       | **7912 ms (17.44x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 98191 ms               | 93786 ms               |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 20240 ms (4.85x)       | 20094 ms (4.67x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10366 ms (9.47x)**   | 10010 ms (9.37x)       |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11865 ms (8.28x)       | **6018 ms (15.58x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 93679 ms               | 90530 ms               |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 21465 ms (4.36x)       | 21125 ms (4.29x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11781 ms (7.95x)**   | 11279 ms (8.03x)       |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12261 ms (7.64x)       | **6734 ms (13.44x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 89894 ms               | 86626 ms               |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 19725 ms (4.56x)       | 19392 ms (4.47x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10568 ms (8.51x)**   | 10174 ms (8.51x)       |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11360 ms (7.91x)       | **5661 ms (15.30x)**   |


### 2 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 157332 ms              | 154482 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 24859 ms (6.33x)       | 24476 ms (6.31x)       |
| object FastActivator.CreateInstance<...>(...)                          | **13515 ms (11.64x)**  | 13094 ms (11.80x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16402 ms (9.59x)       | **7694 ms (20.08x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 109501 ms              | 106579 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 21697 ms (5.05x)       | 21470 ms (4.96x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10426 ms (10.50x)**  | 10045 ms (10.61x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11949 ms (9.16x)       | **6003 ms (17.75x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 103837 ms              | 101103 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 22334 ms (4.65x)       | 21998 ms (4.60x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11710 ms (8.87x)**   | 11245 ms (8.99x)       |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12215 ms (8.50x)       | **6592 ms (15.34x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 100361 ms              | 97561 ms               |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 20069 ms (5.00x)       | 19866 ms (4.91x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10886 ms (9.22x)**   | 10499 ms (9.29x)       |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11610 ms (8.64x)       | **5982 ms (16.31x)**   |


### 3 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 172644 ms              | 170940 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 28552 ms (6.05x)       | 27968 ms (6.11x)       |
| object FastActivator.CreateInstance<...>(...)                          | **13630 ms (12.67x)**  | 13414 ms (12.74x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16455 ms (10.49x)      | **7859 ms (21.75x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 120758 ms              | 117670 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 24812 ms (4.87x)       | 24520 ms (4.80x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10387 ms (11.63x)**  | 9999 ms (11.77x)       |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11998 ms (10.06x)      | **6011 ms (19.58x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 114270 ms              | 110485 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 25262 ms (4.52x)       | 25101 ms (4.40x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11596 ms (9.85x)**   | 11372 ms (9.72x)       |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12046 ms (9.49x)       | **6608 ms (16.72x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 110181 ms              | 106357 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 22497 ms (4.90x)       | 22054 ms (4.82x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10539 ms (10.45x)**  | 10136 ms (10.49x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11371 ms (9.69x)       | **5596 ms (19.01x)**   |


### 4 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 190764 ms              | 187864 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 28662 ms (6.66x)       | 28149 ms (6.67x)       |
| object FastActivator.CreateInstance<...>(...)                          | **13525 ms (14.10x)**  | 13301 ms (14.12x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16670 ms (11.44x)      | **7839 ms (23.97x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 133264 ms              | 130298 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 25004 ms (5.33x)       | 24577 ms (5.30x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10408 ms (12.80x)**  | 10209 ms (12.76x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11786 ms (11.31x)      | **6043 ms (21.56x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 124769 ms              | 121006 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 25849 ms (4.83x)       | 25613 ms (4.72x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11740 ms (10.63x)**  | 11766 ms (10.28x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12316 ms (10.13x)      | **6665 ms (18.16x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 120587 ms              | 117191 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 22185 ms (5.44x)       | 21627 ms (5.42x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10557 ms (11.42x)**  | 10179 ms (11.51x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11342 ms (10.63x)      | **5688 ms (20.60x)**   |


### 5 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 208609 ms              | 206811 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 32744 ms (6.37x)       | 32422 ms (6.38x)       |
| object FastActivator.CreateInstance<...>(...)                          | **13920 ms (14.99x)**  | 13143 ms (15.74x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16359 ms (12.75x)      | **7601 ms (27.21x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 147406 ms              | 142732 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 29435 ms (5.01x)       | 29045 ms (4.91x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10539 ms (13.99x)**  | 10089 ms (14.15x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11722 ms (12.58x)      | **5982 ms (23.86x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 135989 ms              | 135111 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 29688 ms (4.58x)       | 29271 ms (4.62x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11738 ms (11.59x)**  | 11536 ms (11.71x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12433 ms (10.94x)      | **6493 ms (20.81x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 131387 ms              | 127939 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 25655 ms (5.12x)       | 24980 ms (5.12x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10844 ms (12.12x)**  | 10260 ms (12.47x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11588 ms (11.34x)      | **5814 ms (22.01x)**   |


### 6 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 228160 ms              | 224688 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 34275 ms (6.66x)       | 33771 ms (6.65x)       |
| object FastActivator.CreateInstance<...>(...)                          | **13905 ms (16.41x)**  | 13405 ms (16.76x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16592 ms (13.75x)      | **7809 ms (28.77x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 159463 ms              | 156846 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 30855 ms (5.17x)       | 30177 ms (5.20x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11046 ms (14.44x)**  | 10353 ms (15.15x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12138 ms (13.14x)      | **5951 ms (26.36x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 147241 ms              | 142798 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 31343 ms (4.70x)       | 30514 ms (4.68x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12102 ms (12.17x)**  | 11966 ms (11.93x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12859 ms (11.45x)      | **7144 ms (19.99x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 141102 ms              | 137820 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 26432 ms (5.34x)       | 25861 ms (5.33x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10890 ms (12.96x)**  | 10280 ms (13.41x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11842 ms (11.92x)      | **5869 ms (23.48x)**   |


### 7 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 243418 ms              | 240924 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 37001 ms (6.58x)       | 37005 ms (6.51x)       |
| object FastActivator.CreateInstance<...>(...)                          | **13787 ms (17.66x)**  | 13748 ms (17.52x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16462 ms (14.79x)      | **8382 ms (28.74x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 171904 ms              | 169656 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 33975 ms (5.06x)       | 33786 ms (5.02x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10883 ms (15.80x)**  | 10724 ms (15.82x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12412 ms (13.85x)      | **6403 ms (26.50x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 159018 ms              | 155952 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 34292 ms (4.64x)       | 33863 ms (4.61x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12065 ms (13.18x)**  | 11701 ms (13.33x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12532 ms (12.69x)      | **7090 ms (22.00x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 150237 ms              | 147776 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 29089 ms (5.16x)       | 28535 ms (5.18x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10869 ms (13.82x)**  | 10853 ms (13.62x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12104 ms (12.41x)      | **6338 ms (23.32x)**   |


### 8 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 261125 ms              | 258583 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 36137 ms (7.23x)       | 36146 ms (7.15x)       |
| object FastActivator.CreateInstance<...>(...)                          | 13894 ms (18.79x)      | 13821 ms (18.71x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16786 ms (15.56x)      | 8303 ms (31.14x)       |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 184737 ms              | 182180 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 33013 ms (5.60x)       | 32652 ms (5.58x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10931 ms (16.90x)      | 10722 ms (16.99x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12608 ms (14.65x)      | 6506 ms (28.00x)       |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 168542 ms              | 165743 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 33500 ms (5.03x)       | 32975 ms (5.03x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12263 ms (13.74x)**  | 11892 ms (13.94x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12628 ms (13.35x)      | **7316 ms (22.65x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 161534 ms              | 158248 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 27688 ms (5.83x)       | 28406 ms (5.57x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10982 ms (14.71x)**  | 10829 ms (14.61x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12101 ms (13.35x)      | **6450 ms (24.53x)**   |


### 9 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 278321 ms              | 274821 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 42071 ms (6.62x)       | 41720 ms (6.59x)       |
| object FastActivator.CreateInstance<...>(...)                          | **13971 ms (19.92x)**  | 13683 ms (20.08x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16669 ms (16.70x)      | **8339 ms (32.96x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 195128 ms              | 194222 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 38735 ms (5.04x)       | 38440 ms (5.05x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10940 ms (17.84x)**  | 10780 ms (18.02x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12505 ms (15.60x)      | **6600 ms (29.43x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 179775 ms              | 175953 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 38589 ms (4.66x)       | 38251 ms (4.60x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12634 ms (14.23x)**  | 12433 ms (14.15x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13040 ms (13.79x)      | **7714 ms (22.81x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 173671 ms              | 167615 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 32228 ms (5.39x)       | 31845 ms (5.26x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11055 ms (15.71x)**  | 10676 ms (15.70x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11936 ms (14.55x)      | **6404 ms (26.17x)**   |


### 10 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 294717 ms              | 293476 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 43293 ms (6.81x)       | 42893 ms (6.84x)       |
| object FastActivator.CreateInstance<...>(...)                          | **13998 ms (21.05x)**  | 13662 ms (21.48x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16771 ms (17.57x)      | **8336 ms (35.21x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 209147 ms              | 206479 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 39852 ms (5.25x)       | 39724 ms (5.20x)       |
| object FastActivator.CreateInstance<...>(...)                          | **10935 ms (19.13x)**  | 10728 ms (19.25x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12184 ms (17.17x)      | **6503 ms (31.75x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 189191 ms              | 186678 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 39182 ms (4.83x)       | 38983 ms (4.79x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12560 ms (15.06x)**  | 12370 ms (15.09x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12964 ms (14.59x)      | **7405 ms (25.21x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 180611 ms              | 177641 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 33262 ms (5.43x)       | 32551 ms (5.46x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11011 ms (16.40x)**  | 10588 ms (16.78x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11981 ms (15.07x)      | **6294 ms (28.22x)**   |


### 11 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 313106 ms              | 309749 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 46680 ms (6.71x)       | 46872 ms (6.61x)       |
| object FastActivator.CreateInstance<...>(...)                          | **14215 ms (22.03x)**  | 14089 ms (21.99x)      |
| T      FastActivator.CreateInstance<...>(...)                          | 16762 ms (18.68x)      | **8677 ms (35.70x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 220311 ms              | 219065 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 43409 ms (5.08x)       | 43052 ms (5.09x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11056 ms (19.93x)**  | 10831 ms (20.23x)      |
| T      FastActivator.CreateInstance<...>(...)                          | 12347 ms (17.84x)      | **6488 ms (33.76x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 198462 ms              | 195201 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 42308 ms (4.69x)       | 42669 ms (4.57x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12065 ms (16.45x)**  | 11748 ms (16.62x)      |
| T      FastActivator.CreateInstance<...>(...)                          | 12644 ms (15.70x)      | **7147 ms (27.31x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 190343 ms              | 186719 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 35941 ms (5.30x)       | 35585 ms (5.25x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11250 ms (16.92x)**  | 10894 ms (17.14x)      |
| T      FastActivator.CreateInstance<...>(...)                          | 12004 ms (15.86x)      | **6364 ms (29.34x)**   |


### 12 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 330390 ms              | 327760 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 47413 ms (6.97x)       | 46979 ms (6.98x)       |
| object FastActivator.CreateInstance<...>(...)                          | **14045 ms (23.52x)**  | 13962 ms (23.48x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16539 ms (19.98x)      | **8616 ms (38.04x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 232568 ms              | 234205 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 43625 ms (5.33x)       | 43104 ms (5.43x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11146 ms (20.87x)**  | 10903 ms (21.48x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12312 ms (18.89x)      | **6587 ms (35.56x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 209737 ms              | 209212 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 42675 ms (4.91x)       | 42348 ms (4.94x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12228 ms (17.15x)**  | 11758 ms (17.79x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12496 ms (16.78x)      | **7091 ms (29.50x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 202024 ms              | 199018 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 35700 ms (5.66x)       | 36006 ms (5.53x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11578 ms (17.45x)**  | 10960 ms (18.16x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12241 ms (16.50x)      | **6470 ms (30.76x)**   |


### 13 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 348232 ms              | 343267 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 51513 ms (6.76x)       | 50657 ms (6.78x)       |
| object FastActivator.CreateInstance<...>(...)                          | **14341 ms (24.28x)**  | 14275 ms (24.05x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16936 ms (20.56x)      | **8474 ms (40.51x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 243861 ms              | 246572 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 48491 ms (5.03x)       | 48241 ms (5.11x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11117 ms (21.94x)**  | 11010 ms (22.40x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12511 ms (19.49x)      | **6551 ms (37.64x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 220073 ms              | 218285 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 46780 ms (4.70x)       | 46761 ms (4.67x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12557 ms (17.53x)**  | 12007 ms (18.18x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12780 ms (17.22x)      | **7201 ms (30.31x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 212703 ms              | 208158 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 39216 ms (5.42x)       | 38930 ms (5.35x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11199 ms (18.99x)**  | 11450 ms (18.18x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12460 ms (17.07x)      | **6622 ms (31.43x)**   |


### 14 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 366621 ms              | 366719 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 52592 ms (6.97x)       | 54018 ms (6.79x)       |
| object FastActivator.CreateInstance<...>(...)                          | **14562 ms (25.18x)**  | 14699 ms (24.95x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17272 ms (21.23x)      | **8729 ms (42.01x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 259824 ms              | 255525 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 48245 ms (5.39x)       | 48658 ms (5.25x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11097 ms (23.41x)**  | 10861 ms (23.53x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12412 ms (20.93x)      | **6528 ms (39.14x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 231347 ms              | 229211 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 48503 ms (4.77x)       | 48448 ms (4.73x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12380 ms (18.69x)**  | 11956 ms (19.17x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12693 ms (18.23x)      | **7293 ms (31.43x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 220772 ms              | 217857 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 40047 ms (5.51x)       | 39012 ms (5.58x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11677 ms (18.91x)**  | 11321 ms (19.24x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12245 ms (18.03x)      | **6636 ms (32.83x)**   |


### 15 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 384381 ms              | 385173 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 56713 ms (6.78x)       | 56281 ms (6.84x)       |
| object FastActivator.CreateInstance<...>(...)                          | **14508 ms (26.49x)**  | 14015 ms (27.48x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16839 ms (22.83x)      | **8520 ms (45.21x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 269883 ms              | 267743 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 52100 ms (5.18x)       | 51918 ms (5.16x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11218 ms (24.06x)**  | 10965 ms (24.42x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12429 ms (21.71x)      | **6712 ms (39.89x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 241309 ms              | 239067 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 51708 ms (4.67x)       | 51576 ms (4.64x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12591 ms (19.17x)**  | 12113 ms (19.74x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13022 ms (18.53x)      | **7569 ms (31.59x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 231409 ms              | 228794 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 42501 ms (5.44x)       | 42112 ms (5.43x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11207 ms (20.65x)**  | 10975 ms (20.85x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11978 ms (19.32x)      | **6636 ms (34.48x)**   |


### 16 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 401630 ms              | 400764 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 59946 ms (6.70x)       | 59217 ms (6.77x)       |
| object FastActivator.CreateInstance<...>(...)                          | **14653 ms (27.41x)**  | 14058 ms (28.51x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17134 ms (23.44x)      | **8615 ms (46.52x)**   |

#### .NET Core 2.1.23

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 283546 ms              | 277944 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 55453 ms (5.11x)       | 55197 ms (5.04x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11395 ms (24.88x)**  | 11003 ms (25.26x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12534 ms (22.62x)      | **6827 ms (40.71x)**   |

#### .NET Core 3.1.9

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 250466 ms              | 247545 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 54462 ms (4.60x)       | 54194 ms (4.57x)       |
| object FastActivator.CreateInstance<...>(...)                          | **12827 ms (19.53x)**  | 12572 ms (19.69x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13233 ms (18.93x)      | **7587 ms (32.63x)**   |

#### .NET 5.0.0

| Method                                                                 | Class                  | Struct                 |
|------------------------------------------------------------------------|------------------------|------------------------|
| object Activator.CreateInstance(Type, object[])                        | 238737 ms              | 235958 ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 44784 ms (5.33x)       | 44272 ms (5.33x)       |
| object FastActivator.CreateInstance<...>(...)                          | **11404 ms (20.93x)**  | 11361 ms (20.77x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12508 ms (19.09x)      | **6627 ms (35.61x)**   |

