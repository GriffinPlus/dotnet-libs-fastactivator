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

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance<T>()                                        | 8226 allocs/ms               | 10453 allocs/ms              |
| T FastActivator<T>.CreateInstance()                                    | 15525 allocs/ms (1.89x)      | 27719 allocs/ms (2.65x)      |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance<T>()                                        | 9475 allocs/ms               | 10839 allocs/ms              |
| T FastActivator<T>.CreateInstance()                                    | 15123 allocs/ms (1.60x)      | 26276 allocs/ms (2.42x)      |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance<T>()                                        | 13600 allocs/ms              | 43376 allocs/ms              |
| T FastActivator<T>.CreateInstance()                                    | 22691 allocs/ms (1.67x)      | 41568 allocs/ms (0.96x)      |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance<T>()                                        | 12665 allocs/ms              | 45250 allocs/ms              |
| T FastActivator<T>.CreateInstance()                                    | 23261 allocs/ms (1.84x)      | 42829 allocs/ms (0.95x)      |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance<T>()                                        | 22232 allocs/ms              | 46052 allocs/ms              |
| T FastActivator<T>.CreateInstance()                                    | 22130 allocs/ms (1.00x)      | 43666 allocs/ms (0.95x)      |


### No Constructor Parameters (Non-Generic)

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                  | 8721 allocs/ms               | 10604 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 9507 allocs/ms (1.09x)       | 10625 allocs/ms (1.00x)      |
| object FastActivator.CreateInstance(Type)                              | 10214 allocs/ms (1.17x)      | 11414 allocs/ms (1.08x)      |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                  | 9523 allocs/ms               | 11055 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 11128 allocs/ms (1.17x)      | 11763 allocs/ms (1.06x)      |
| object FastActivator.CreateInstance(Type)                              | 12588 allocs/ms (1.32x)      | 13466 allocs/ms (1.22x)      |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                  | 13703 allocs/ms              | 14971 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 15122 allocs/ms (1.10x)      | 15641 allocs/ms (1.04x)      |
| object FastActivator.CreateInstance(Type)                              | 14973 allocs/ms (1.09x)      | 15631 allocs/ms (1.04x)      |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                  | 12334 allocs/ms              | 15340 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 15290 allocs/ms (1.24x)      | 16389 allocs/ms (1.07x)      |
| object FastActivator.CreateInstance(Type)                              | 14184 allocs/ms (1.15x)      | 15827 allocs/ms (1.03x)      |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                  | 23352 allocs/ms              | 26868 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 15671 allocs/ms (0.67x)      | 16435 allocs/ms (0.61x)      |
| object FastActivator.CreateInstance(Type)                              | 15124 allocs/ms (0.65x)      | 15327 allocs/ms (0.57x)      |


### 1 Constructor Parameter

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 658 allocs/ms                | 678 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 7054 allocs/ms (10.72x)      | 7339 allocs/ms (10.82x)      |
| object FastActivator.CreateInstance<...>(...)                          | 9246 allocs/ms (14.05x)      | 9529 allocs/ms (14.05x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13605 allocs/ms (20.68x)     | 18229 allocs/ms (26.88x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 748 allocs/ms                | 752 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 8487 allocs/ms (11.35x)      | 8776 allocs/ms (11.68x)      |
| object FastActivator.CreateInstance<...>(...)                          | 11160 allocs/ms (14.92x)     | 11871 allocs/ms (15.80x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14846 allocs/ms (19.85x)     | 19684 allocs/ms (26.19x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 821 allocs/ms                | 850 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 10647 allocs/ms (12.97x)     | 11306 allocs/ms (13.30x)     |
| object FastActivator.CreateInstance<...>(...)                          | 13010 allocs/ms (15.84x)     | 13337 allocs/ms (15.69x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 18383 allocs/ms (22.39x)     | 26694 allocs/ms (31.41x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 884 allocs/ms                | 919 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 11333 allocs/ms (12.82x)     | 11688 allocs/ms (12.72x)     |
| object FastActivator.CreateInstance<...>(...)                          | 13257 allocs/ms (14.99x)     | 13850 allocs/ms (15.07x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16934 allocs/ms (19.15x)     | 31157 allocs/ms (33.90x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 962 allocs/ms                | 994 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 11640 allocs/ms (12.10x)     | 12025 allocs/ms (12.10x)     |
| object FastActivator.CreateInstance<...>(...)                          | 13068 allocs/ms (13.58x)     | 13787 allocs/ms (13.87x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 18511 allocs/ms (19.24x)     | 32597 allocs/ms (32.80x)     |


### 2 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 596 allocs/ms                | 609 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5975 allocs/ms (10.02x)      | 6074 allocs/ms (9.97x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9020 allocs/ms (15.13x)      | 9507 allocs/ms (15.60x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13665 allocs/ms (22.92x)     | 17801 allocs/ms (29.21x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 675 allocs/ms                | 681 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 7565 allocs/ms (11.20x)      | 7661 allocs/ms (11.26x)      |
| object FastActivator.CreateInstance<...>(...)                          | 10777 allocs/ms (15.96x)     | 11327 allocs/ms (16.64x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14606 allocs/ms (21.63x)     | 19335 allocs/ms (28.41x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 741 allocs/ms                | 769 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 9446 allocs/ms (12.75x)      | 9800 allocs/ms (12.74x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12800 allocs/ms (17.27x)     | 13124 allocs/ms (17.06x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17988 allocs/ms (24.27x)     | 27612 allocs/ms (35.90x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 794 allocs/ms                | 824 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 9954 allocs/ms (12.53x)      | 10195 allocs/ms (12.37x)     |
| object FastActivator.CreateInstance<...>(...)                          | 12854 allocs/ms (16.19x)     | 13512 allocs/ms (16.40x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16875 allocs/ms (21.25x)     | 30027 allocs/ms (36.44x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 881 allocs/ms                | 903 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 10155 allocs/ms (11.53x)     | 9908 allocs/ms (10.98x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12951 allocs/ms (14.70x)     | 13466 allocs/ms (14.92x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 18384 allocs/ms (20.87x)     | 30477 allocs/ms (33.76x)     |


### 3 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 549 allocs/ms                | 557 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5177 allocs/ms (9.44x)       | 5290 allocs/ms (9.50x)       |
| object FastActivator.CreateInstance<...>(...)                          | 8996 allocs/ms (16.40x)      | 9461 allocs/ms (17.00x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13255 allocs/ms (24.16x)     | 17780 allocs/ms (31.94x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 615 allocs/ms                | 615 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 6657 allocs/ms (10.83x)      | 6828 allocs/ms (11.11x)      |
| object FastActivator.CreateInstance<...>(...)                          | 10449 allocs/ms (16.99x)     | 11121 allocs/ms (18.09x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14478 allocs/ms (23.54x)     | 18903 allocs/ms (30.75x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 680 allocs/ms                | 704 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 8097 allocs/ms (11.91x)      | 8269 allocs/ms (11.74x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12644 allocs/ms (18.60x)     | 12955 allocs/ms (18.40x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17468 allocs/ms (25.70x)     | 26428 allocs/ms (37.53x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 739 allocs/ms                | 757 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 8760 allocs/ms (11.86x)      | 8949 allocs/ms (11.82x)      |
| object FastActivator.CreateInstance<...>(...)                          | 13026 allocs/ms (17.64x)     | 13649 allocs/ms (18.02x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15658 allocs/ms (21.20x)     | 27907 allocs/ms (36.85x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 809 allocs/ms                | 827 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 8759 allocs/ms (10.82x)      | 8974 allocs/ms (10.85x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12856 allocs/ms (15.89x)     | 13386 allocs/ms (16.19x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17674 allocs/ms (21.84x)     | 30001 allocs/ms (36.29x)     |


### 4 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 501 allocs/ms                | 516 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4506 allocs/ms (9.00x)       | 4593 allocs/ms (8.90x)       |
| object FastActivator.CreateInstance<...>(...)                          | 8824 allocs/ms (17.62x)      | 9328 allocs/ms (18.08x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12536 allocs/ms (25.04x)     | 17069 allocs/ms (33.09x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 560 allocs/ms                | 566 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5882 allocs/ms (10.51x)      | 5964 allocs/ms (10.53x)      |
| object FastActivator.CreateInstance<...>(...)                          | 10559 allocs/ms (18.87x)     | 10962 allocs/ms (19.36x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12990 allocs/ms (23.22x)     | 18241 allocs/ms (32.21x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 629 allocs/ms                | 648 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 7131 allocs/ms (11.33x)      | 7125 allocs/ms (11.00x)      |
| object FastActivator.CreateInstance<...>(...)                          | 11968 allocs/ms (19.02x)     | 12218 allocs/ms (18.87x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17358 allocs/ms (27.58x)     | 25584 allocs/ms (39.50x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 679 allocs/ms                | 695 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 7521 allocs/ms (11.08x)      | 7667 allocs/ms (11.03x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12834 allocs/ms (18.91x)     | 13603 allocs/ms (19.57x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16315 allocs/ms (24.04x)     | 27230 allocs/ms (39.18x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 753 allocs/ms                | 763 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 7798 allocs/ms (10.36x)      | 7888 allocs/ms (10.34x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12698 allocs/ms (16.86x)     | 13287 allocs/ms (17.41x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17042 allocs/ms (22.63x)     | 29056 allocs/ms (38.08x)     |


### 5 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 460 allocs/ms                | 470 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3980 allocs/ms (8.66x)       | 4115 allocs/ms (8.76x)       |
| object FastActivator.CreateInstance<...>(...)                          | 8777 allocs/ms (19.10x)      | 9245 allocs/ms (19.69x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12864 allocs/ms (27.99x)     | 16742 allocs/ms (35.66x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 503 allocs/ms                | 510 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5299 allocs/ms (10.53x)      | 5386 allocs/ms (10.57x)      |
| object FastActivator.CreateInstance<...>(...)                          | 10760 allocs/ms (21.39x)     | 11483 allocs/ms (22.54x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13775 allocs/ms (27.38x)     | 17914 allocs/ms (35.16x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 583 allocs/ms                | 602 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 6090 allocs/ms (10.44x)      | 6128 allocs/ms (10.18x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12087 allocs/ms (20.72x)     | 12243 allocs/ms (20.34x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17121 allocs/ms (29.35x)     | 23011 allocs/ms (38.24x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 621 allocs/ms                | 644 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 6756 allocs/ms (10.89x)      | 6825 allocs/ms (10.59x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12297 allocs/ms (19.82x)     | 12868 allocs/ms (19.98x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15732 allocs/ms (25.35x)     | 25763 allocs/ms (40.00x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 692 allocs/ms                | 703 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 7048 allocs/ms (10.18x)      | 7120 allocs/ms (10.12x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12679 allocs/ms (18.31x)     | 13361 allocs/ms (18.99x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17307 allocs/ms (25.00x)     | 28453 allocs/ms (40.45x)     |


### 6 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 429 allocs/ms                | 436 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3605 allocs/ms (8.40x)       | 3707 allocs/ms (8.51x)       |
| object FastActivator.CreateInstance<...>(...)                          | 8621 allocs/ms (20.09x)      | 9317 allocs/ms (21.39x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12409 allocs/ms (28.92x)     | 16115 allocs/ms (37.00x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 472 allocs/ms                | 474 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4782 allocs/ms (10.12x)      | 4858 allocs/ms (10.26x)      |
| object FastActivator.CreateInstance<...>(...)                          | 9766 allocs/ms (20.68x)      | 10522 allocs/ms (22.21x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12621 allocs/ms (26.72x)     | 17436 allocs/ms (36.81x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 538 allocs/ms                | 547 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5670 allocs/ms (10.54x)      | 5629 allocs/ms (10.29x)      |
| object FastActivator.CreateInstance<...>(...)                          | 11422 allocs/ms (21.23x)     | 12196 allocs/ms (22.30x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 16049 allocs/ms (29.84x)     | 24149 allocs/ms (44.15x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 579 allocs/ms                | 592 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5791 allocs/ms (10.00x)      | 6264 allocs/ms (10.58x)      |
| object FastActivator.CreateInstance<...>(...)                          | 11842 allocs/ms (20.45x)     | 13591 allocs/ms (22.95x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15007 allocs/ms (25.91x)     | 28399 allocs/ms (47.95x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 637 allocs/ms                | 646 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 6369 allocs/ms (10.00x)      | 6399 allocs/ms (9.90x)       |
| object FastActivator.CreateInstance<...>(...)                          | 12714 allocs/ms (19.96x)     | 13370 allocs/ms (20.69x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17588 allocs/ms (27.61x)     | 28284 allocs/ms (43.77x)     |


### 7 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 401 allocs/ms                | 408 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3181 allocs/ms (7.93x)       | 3203 allocs/ms (7.86x)       |
| object FastActivator.CreateInstance<...>(...)                          | 8485 allocs/ms (21.15x)      | 8611 allocs/ms (21.12x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11767 allocs/ms (29.33x)     | 13798 allocs/ms (33.84x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 441 allocs/ms                | 444 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4296 allocs/ms (9.74x)       | 4406 allocs/ms (9.91x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9524 allocs/ms (21.59x)      | 9980 allocs/ms (22.45x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12804 allocs/ms (29.03x)     | 15851 allocs/ms (35.66x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 497 allocs/ms                | 508 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5134 allocs/ms (10.32x)      | 5051 allocs/ms (9.94x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11110 allocs/ms (22.33x)     | 11136 allocs/ms (21.92x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15847 allocs/ms (31.86x)     | 21473 allocs/ms (42.26x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 547 allocs/ms                | 557 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5440 allocs/ms (9.94x)       | 5547 allocs/ms (9.96x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11640 allocs/ms (21.26x)     | 12390 allocs/ms (22.26x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15068 allocs/ms (27.52x)     | 23571 allocs/ms (42.34x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 578 allocs/ms                | 586 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5918 allocs/ms (10.23x)      | 5983 allocs/ms (10.21x)      |
| object FastActivator.CreateInstance<...>(...)                          | 12439 allocs/ms (21.51x)     | 13110 allocs/ms (22.38x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 17014 allocs/ms (29.42x)     | 27297 allocs/ms (46.60x)     |


### 8 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 381 allocs/ms                | 387 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2898 allocs/ms (7.61x)       | 2936 allocs/ms (7.59x)       |
| object FastActivator.CreateInstance<...>(...)                          | 8420 allocs/ms (22.13x)      | 8646 allocs/ms (22.35x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11391 allocs/ms (29.93x)     | 13247 allocs/ms (34.24x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 420 allocs/ms                | 420 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4015 allocs/ms (9.57x)       | 4057 allocs/ms (9.66x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9679 allocs/ms (23.07x)      | 9830 allocs/ms (23.40x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12276 allocs/ms (29.26x)     | 14942 allocs/ms (35.57x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 473 allocs/ms                | 484 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4785 allocs/ms (10.11x)      | 4663 allocs/ms (9.64x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11080 allocs/ms (23.40x)     | 11409 allocs/ms (23.57x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15431 allocs/ms (32.59x)     | 19253 allocs/ms (39.79x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 510 allocs/ms                | 524 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5079 allocs/ms (9.96x)       | 5125 allocs/ms (9.78x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11009 allocs/ms (21.58x)     | 11553 allocs/ms (22.06x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14802 allocs/ms (29.01x)     | 21977 allocs/ms (41.96x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 548 allocs/ms                | 557 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5357 allocs/ms (9.77x)       | 5447 allocs/ms (9.79x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11537 allocs/ms (21.05x)     | 13043 allocs/ms (23.44x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15739 allocs/ms (28.71x)     | 27323 allocs/ms (49.10x)     |


### 9 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 359 allocs/ms                | 367 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2743 allocs/ms (7.64x)       | 2753 allocs/ms (7.50x)       |
| object FastActivator.CreateInstance<...>(...)                          | 7778 allocs/ms (21.67x)      | 8496 allocs/ms (23.14x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12001 allocs/ms (33.43x)     | 13006 allocs/ms (35.43x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 396 allocs/ms                | 398 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3595 allocs/ms (9.08x)       | 3642 allocs/ms (9.14x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9579 allocs/ms (24.20x)      | 9778 allocs/ms (24.55x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11584 allocs/ms (29.26x)     | 14380 allocs/ms (36.10x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 451 allocs/ms                | 460 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4436 allocs/ms (9.84x)       | 4272 allocs/ms (9.30x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11091 allocs/ms (24.61x)     | 11058 allocs/ms (24.06x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15540 allocs/ms (34.49x)     | 18448 allocs/ms (40.15x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 480 allocs/ms                | 485 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4734 allocs/ms (9.87x)       | 4764 allocs/ms (9.82x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11569 allocs/ms (24.12x)     | 12089 allocs/ms (24.92x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14367 allocs/ms (29.95x)     | 21977 allocs/ms (45.31x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 525 allocs/ms                | 529 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5023 allocs/ms (9.57x)       | 4905 allocs/ms (9.27x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11311 allocs/ms (21.54x)     | 10790 allocs/ms (20.39x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15801 allocs/ms (30.10x)     | 22846 allocs/ms (43.17x)     |


### 10 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 343 allocs/ms                | 348 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2522 allocs/ms (7.35x)       | 2514 allocs/ms (7.22x)       |
| object FastActivator.CreateInstance<...>(...)                          | 7845 allocs/ms (22.87x)      | 8423 allocs/ms (24.18x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11825 allocs/ms (34.47x)     | 12848 allocs/ms (36.88x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 374 allocs/ms                | 376 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3254 allocs/ms (8.71x)       | 3276 allocs/ms (8.71x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9528 allocs/ms (25.51x)      | 9603 allocs/ms (25.53x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11300 allocs/ms (30.25x)     | 13580 allocs/ms (36.11x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 427 allocs/ms                | 436 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3989 allocs/ms (9.34x)       | 3956 allocs/ms (9.07x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11246 allocs/ms (26.33x)     | 10864 allocs/ms (24.91x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14809 allocs/ms (34.67x)     | 18357 allocs/ms (42.09x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 458 allocs/ms                | 465 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4466 allocs/ms (9.76x)       | 4359 allocs/ms (9.38x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11295 allocs/ms (24.68x)     | 11278 allocs/ms (24.27x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14216 allocs/ms (31.06x)     | 21031 allocs/ms (45.26x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 504 allocs/ms                | 508 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4650 allocs/ms (9.23x)       | 4587 allocs/ms (9.03x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11267 allocs/ms (22.35x)     | 11274 allocs/ms (22.18x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15363 allocs/ms (30.48x)     | 20970 allocs/ms (41.26x)     |


### 11 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 326 allocs/ms                | 330 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2363 allocs/ms (7.24x)       | 2344 allocs/ms (7.10x)       |
| object FastActivator.CreateInstance<...>(...)                          | 7713 allocs/ms (23.64x)      | 7869 allocs/ms (23.82x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11411 allocs/ms (34.98x)     | 12758 allocs/ms (38.62x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 359 allocs/ms                | 360 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3045 allocs/ms (8.49x)       | 3112 allocs/ms (8.64x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9336 allocs/ms (26.02x)      | 9428 allocs/ms (26.17x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11794 allocs/ms (32.87x)     | 13686 allocs/ms (37.99x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 408 allocs/ms                | 414 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3788 allocs/ms (9.28x)       | 3712 allocs/ms (8.96x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10843 allocs/ms (26.57x)     | 10756 allocs/ms (25.96x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13955 allocs/ms (34.19x)     | 17574 allocs/ms (42.41x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 438 allocs/ms                | 438 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4165 allocs/ms (9.52x)       | 4085 allocs/ms (9.32x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11259 allocs/ms (25.73x)     | 11645 allocs/ms (26.57x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13951 allocs/ms (31.88x)     | 20733 allocs/ms (47.31x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 478 allocs/ms                | 481 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4377 allocs/ms (9.16x)       | 4347 allocs/ms (9.04x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11480 allocs/ms (24.03x)     | 11409 allocs/ms (23.72x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 15023 allocs/ms (31.44x)     | 21962 allocs/ms (45.67x)     |


### 12 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 312 allocs/ms                | 318 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2226 allocs/ms (7.14x)       | 2256 allocs/ms (7.10x)       |
| object FastActivator.CreateInstance<...>(...)                          | 8190 allocs/ms (26.28x)      | 8264 allocs/ms (26.01x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 10864 allocs/ms (34.85x)     | 12462 allocs/ms (39.23x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 343 allocs/ms                | 345 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2888 allocs/ms (8.41x)       | 2924 allocs/ms (8.48x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9244 allocs/ms (26.93x)      | 9389 allocs/ms (27.24x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 12180 allocs/ms (35.48x)     | 12732 allocs/ms (36.95x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 388 allocs/ms                | 397 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3558 allocs/ms (9.18x)       | 3497 allocs/ms (8.81x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11021 allocs/ms (28.44x)     | 10563 allocs/ms (26.60x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14739 allocs/ms (38.03x)     | 17213 allocs/ms (43.35x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 418 allocs/ms                | 423 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3925 allocs/ms (9.38x)       | 3831 allocs/ms (9.05x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11171 allocs/ms (26.71x)     | 11608 allocs/ms (27.43x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13964 allocs/ms (33.38x)     | 19240 allocs/ms (45.46x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 457 allocs/ms                | 464 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 4164 allocs/ms (9.11x)       | 4052 allocs/ms (8.74x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11457 allocs/ms (25.07x)     | 11467 allocs/ms (24.72x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14416 allocs/ms (31.54x)     | 21089 allocs/ms (45.46x)     |


### 13 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 299 allocs/ms                | 301 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2054 allocs/ms (6.88x)       | 2068 allocs/ms (6.88x)       |
| object FastActivator.CreateInstance<...>(...)                          | 8109 allocs/ms (27.14x)      | 8096 allocs/ms (26.94x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 10492 allocs/ms (35.11x)     | 8787 allocs/ms (29.24x)      |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 325 allocs/ms                | 327 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2795 allocs/ms (8.59x)       | 2749 allocs/ms (8.40x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9339 allocs/ms (28.71x)      | 8993 allocs/ms (27.50x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11647 allocs/ms (35.80x)     | 12949 allocs/ms (39.59x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 374 allocs/ms                | 380 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3251 allocs/ms (8.69x)       | 3146 allocs/ms (8.29x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10830 allocs/ms (28.96x)     | 10076 allocs/ms (26.55x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14444 allocs/ms (38.63x)     | 16790 allocs/ms (44.24x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 400 allocs/ms                | 406 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3725 allocs/ms (9.31x)       | 3585 allocs/ms (8.83x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10614 allocs/ms (26.54x)     | 11219 allocs/ms (27.62x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13748 allocs/ms (34.37x)     | 19274 allocs/ms (47.44x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 438 allocs/ms                | 443 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3819 allocs/ms (8.71x)       | 3756 allocs/ms (8.47x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10879 allocs/ms (24.83x)     | 10668 allocs/ms (24.06x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14882 allocs/ms (33.96x)     | 19670 allocs/ms (44.36x)     |


### 14 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 286 allocs/ms                | 290 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1955 allocs/ms (6.83x)       | 1935 allocs/ms (6.68x)       |
| object FastActivator.CreateInstance<...>(...)                          | 8036 allocs/ms (28.07x)      | 8126 allocs/ms (28.04x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11163 allocs/ms (38.99x)     | 12088 allocs/ms (41.72x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 311 allocs/ms                | 313 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2641 allocs/ms (8.48x)       | 2657 allocs/ms (8.48x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9269 allocs/ms (29.78x)      | 9276 allocs/ms (29.61x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11308 allocs/ms (36.32x)     | 12145 allocs/ms (38.77x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 360 allocs/ms                | 359 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3059 allocs/ms (8.49x)       | 3001 allocs/ms (8.36x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10743 allocs/ms (29.81x)     | 10327 allocs/ms (28.77x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14058 allocs/ms (39.01x)     | 16499 allocs/ms (45.96x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 382 allocs/ms                | 385 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3379 allocs/ms (8.84x)       | 3384 allocs/ms (8.79x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11100 allocs/ms (29.05x)     | 11463 allocs/ms (29.77x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13347 allocs/ms (34.93x)     | 19432 allocs/ms (50.46x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 420 allocs/ms                | 422 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3631 allocs/ms (8.65x)       | 3601 allocs/ms (8.53x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11067 allocs/ms (26.36x)     | 10400 allocs/ms (24.63x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 14342 allocs/ms (34.17x)     | 19285 allocs/ms (45.67x)     |


### 15 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 274 allocs/ms                | 277 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1809 allocs/ms (6.62x)       | 1830 allocs/ms (6.61x)       |
| object FastActivator.CreateInstance<...>(...)                          | 7912 allocs/ms (28.93x)      | 7992 allocs/ms (28.85x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 10148 allocs/ms (37.10x)     | 11206 allocs/ms (40.46x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 302 allocs/ms                | 302 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2495 allocs/ms (8.27x)       | 2495 allocs/ms (8.26x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9178 allocs/ms (30.42x)      | 9103 allocs/ms (30.13x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 11138 allocs/ms (36.91x)     | 12536 allocs/ms (41.49x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 342 allocs/ms                | 349 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2829 allocs/ms (8.27x)       | 2809 allocs/ms (8.05x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10714 allocs/ms (31.33x)     | 10279 allocs/ms (29.48x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13825 allocs/ms (40.43x)     | 15893 allocs/ms (45.57x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 368 allocs/ms                | 373 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3170 allocs/ms (8.61x)       | 3134 allocs/ms (8.40x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10990 allocs/ms (29.86x)     | 11323 allocs/ms (30.33x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13469 allocs/ms (36.60x)     | 18854 allocs/ms (50.51x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 406 allocs/ms                | 409 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3452 allocs/ms (8.50x)       | 3435 allocs/ms (8.39x)       |
| object FastActivator.CreateInstance<...>(...)                          | 11176 allocs/ms (27.51x)     | 11127 allocs/ms (27.17x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13412 allocs/ms (33.01x)     | 18094 allocs/ms (44.19x)     |


### 16 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 265 allocs/ms                | 268 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 1715 allocs/ms (6.48x)       | 1736 allocs/ms (6.47x)       |
| object FastActivator.CreateInstance<...>(...)                          | 7912 allocs/ms (29.91x)      | 7672 allocs/ms (28.57x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 10534 allocs/ms (39.82x)     | 11582 allocs/ms (43.14x)     |

#### .NET Core 2.1.30

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 292 allocs/ms                | 291 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2341 allocs/ms (8.02x)       | 2407 allocs/ms (8.27x)       |
| object FastActivator.CreateInstance<...>(...)                          | 9068 allocs/ms (31.07x)      | 8933 allocs/ms (30.70x)      |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 10766 allocs/ms (36.89x)     | 11942 allocs/ms (41.04x)     |

#### .NET Core 3.1.26

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 334 allocs/ms                | 338 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 2728 allocs/ms (8.16x)       | 2708 allocs/ms (8.01x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10072 allocs/ms (30.12x)     | 10169 allocs/ms (30.07x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13664 allocs/ms (40.86x)     | 15627 allocs/ms (46.22x)     |

#### .NET 5.0.17

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 354 allocs/ms                | 361 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3083 allocs/ms (8.71x)       | 2973 allocs/ms (8.23x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10212 allocs/ms (28.84x)     | 10260 allocs/ms (28.41x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13158 allocs/ms (37.16x)     | 14948 allocs/ms (41.40x)     |

#### .NET Core 6.0.6

| Method                                                                 | Class                        | Struct                       |
|------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                        | 390 allocs/ms                | 395 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 3282 allocs/ms (8.42x)       | 3272 allocs/ms (8.29x)       |
| object FastActivator.CreateInstance<...>(...)                          | 10497 allocs/ms (26.93x)     | 10348 allocs/ms (26.20x)     |
| T      FastActivator<T>.CreateInstance<...>(...)                       | 13419 allocs/ms (34.42x)     | 17221 allocs/ms (43.60x)     |
