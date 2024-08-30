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

A more specific build for .NET Framework 4.6.1 minimizes dependencies to framework components.

Therefore it should work on the following platforms (or higher):
- .NET Framework 4.6.1
- .NET Core 2/3
- .NET 5/6/7
- Mono 5.4
- Xamarin iOS 10.14
- Xamarin Mac 3.8
- Xamarin Android 8.0
- Universal Windows Platform (UWP) 10.0.16299

The library is tested automatically on the following frameworks and operating systems:
- .NET Framework 4.6.1: Tests with library built for .NET Framework 4.6.1 (Windows Server 2022)
- .NET Framework 4.8: Tests with library built for .NET Framework 4.6.1 (Windows Server 2022)
- .NET Core 2.2: Tests with library built for .NET Standard 2.0 (Windows Server 2022 and Ubuntu 22.04)
- .NET Core 3.1: Tests with library built for .NET Standard 2.0 (Windows Server 2022 and Ubuntu 22.04)
- .NET 5.0/6.0/7.0/8.0: Tests with library built for .NET Standard 2.0 (Windows Server 2022 and Ubuntu 22.04)

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

The executing runtimes are *.NET Framework 4.8*, *.NET Core 2.2.8*, *.NET Core 3.1.32*, *.NET 5.0.17*, *.NET 6.0.14* and *.NET 7.0.3*.

### No Constructor Parameters (Generic)

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance&lt;T&gt;()                                        | 7493 allocs/ms               | 10144 allocs/ms              |
| T FastActivator&lt;T&gt;.CreateInstance()                                    | 15691 allocs/ms (2.09x)      | 25712 allocs/ms (2.53x)      |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance&lt;T&gt;()                                        | 9370 allocs/ms               | 10841 allocs/ms              |
| T FastActivator&lt;T&gt;.CreateInstance()                                    | 17641 allocs/ms (1.88x)      | 24907 allocs/ms (2.30x)      |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance&lt;T&gt;()                                        | 13694 allocs/ms              | 43570 allocs/ms              |
| T FastActivator&lt;T&gt;.CreateInstance()                                    | 22868 allocs/ms (1.67x)      | 41000 allocs/ms (0.94x)      |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance&lt;T&gt;()                                        | 12865 allocs/ms              | 44799 allocs/ms              |
| T FastActivator&lt;T&gt;.CreateInstance()                                    | 22741 allocs/ms (1.77x)      | 42938 allocs/ms (0.96x)      |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance&lt;T&gt;()                                        | 21581 allocs/ms              | 46034 allocs/ms              |
| T FastActivator&lt;T&gt;.CreateInstance()                                    | 22961 allocs/ms (1.06x)      | 42554 allocs/ms (0.92x)      |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| T Activator.CreateInstance&lt;T&gt;()                                        | 18455 allocs/ms              | 46114 allocs/ms              |
| T FastActivator&lt;T&gt;.CreateInstance()                                    | 21515 allocs/ms (1.17x)      | 43785 allocs/ms (0.95x)      |


### No Constructor Parameters (Non-Generic)

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                        | 8260 allocs/ms               | 10200 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 9237 allocs/ms (1.12x)       | 10491 allocs/ms (1.03x)      |
| object FastActivator.CreateInstance(Type)                                    | 10131 allocs/ms (1.23x)      | 11380 allocs/ms (1.12x)      |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                        | 9873 allocs/ms               | 10946 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 11046 allocs/ms (1.12x)      | 11597 allocs/ms (1.06x)      |
| object FastActivator.CreateInstance(Type)                                    | 12323 allocs/ms (1.25x)      | 12808 allocs/ms (1.17x)      |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                        | 13047 allocs/ms              | 14303 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 14362 allocs/ms (1.10x)      | 15404 allocs/ms (1.08x)      |
| object FastActivator.CreateInstance(Type)                                    | 15267 allocs/ms (1.17x)      | 15767 allocs/ms (1.10x)      |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                        | 13057 allocs/ms              | 15451 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 14679 allocs/ms (1.12x)      | 15292 allocs/ms (0.99x)      |
| object FastActivator.CreateInstance(Type)                                    | 13169 allocs/ms (1.01x)      | 14990 allocs/ms (0.97x)      |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                        | 22821 allocs/ms              | 26731 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 14895 allocs/ms (0.65x)      | 15977 allocs/ms (0.60x)      |
| object FastActivator.CreateInstance(Type)                                    | 14869 allocs/ms (0.65x)      | 14508 allocs/ms (0.54x)      |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type)                                        | 20830 allocs/ms              | 25669 allocs/ms              |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 13518 allocs/ms (0.65x)      | 15666 allocs/ms (0.61x)      |
| object FastActivator.CreateInstance(Type)                                    | 15260 allocs/ms (0.73x)      | 16106 allocs/ms (0.63x)      |


### 1 Constructor Parameter

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 662 allocs/ms                | 679 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 6383 allocs/ms (9.64x)       | 6612 allocs/ms (9.74x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8932 allocs/ms (13.49x)      | 9393 allocs/ms (13.84x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13407 allocs/ms (20.25x)     | 16853 allocs/ms (24.84x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 695 allocs/ms                | 713 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 7815 allocs/ms (11.24x)      | 8194 allocs/ms (11.50x)      |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10626 allocs/ms (15.29x)     | 11502 allocs/ms (16.14x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 15244 allocs/ms (21.93x)     | 19910 allocs/ms (27.93x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 805 allocs/ms                | 833 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 10038 allocs/ms (12.47x)     | 10137 allocs/ms (12.17x)     |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12576 allocs/ms (15.62x)     | 13056 allocs/ms (15.67x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17050 allocs/ms (21.18x)     | 28327 allocs/ms (34.01x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 877 allocs/ms                | 916 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 10327 allocs/ms (11.78x)     | 10445 allocs/ms (11.40x)     |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 13163 allocs/ms (15.02x)     | 13378 allocs/ms (14.60x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17207 allocs/ms (19.63x)     | 31308 allocs/ms (34.16x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 909 allocs/ms                | 938 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 10387 allocs/ms (11.43x)     | 10819 allocs/ms (11.53x)     |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 13164 allocs/ms (14.49x)     | 13151 allocs/ms (14.02x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 18609 allocs/ms (20.48x)     | 32464 allocs/ms (34.60x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 1187 allocs/ms               | 1234 allocs/ms               |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 9649 allocs/ms (8.13x)       | 10848 allocs/ms (8.79x)      |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12619 allocs/ms (10.63x)     | 13081 allocs/ms (10.60x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17935 allocs/ms (15.11x)     | 30642 allocs/ms (24.82x)     |


### 2 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 593 allocs/ms                | 613 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 5193 allocs/ms (8.75x)       | 5380 allocs/ms (8.78x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8838 allocs/ms (14.90x)      | 9412 allocs/ms (15.36x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13583 allocs/ms (22.89x)     | 16610 allocs/ms (27.11x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 629 allocs/ms                | 647 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 6578 allocs/ms (10.46x)      | 6622 allocs/ms (10.24x)      |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10043 allocs/ms (15.96x)     | 11179 allocs/ms (17.29x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14765 allocs/ms (23.47x)     | 19338 allocs/ms (29.91x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 728 allocs/ms                | 756 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 8083 allocs/ms (11.10x)      | 8187 allocs/ms (10.83x)      |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12728 allocs/ms (17.48x)     | 13042 allocs/ms (17.25x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 18060 allocs/ms (24.80x)     | 27767 allocs/ms (36.72x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 790 allocs/ms                | 825 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 8238 allocs/ms (10.42x)      | 8461 allocs/ms (10.26x)      |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12744 allocs/ms (16.13x)     | 13669 allocs/ms (16.57x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 16880 allocs/ms (21.36x)     | 29605 allocs/ms (35.90x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 820 allocs/ms                | 847 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 8466 allocs/ms (10.33x)      | 8744 allocs/ms (10.32x)      |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12912 allocs/ms (15.75x)     | 12850 allocs/ms (15.17x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 18520 allocs/ms (22.59x)     | 31378 allocs/ms (37.03x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 1111 allocs/ms               | 1152 allocs/ms               |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 7988 allocs/ms (7.19x)       | 8513 allocs/ms (7.39x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12364 allocs/ms (11.12x)     | 13569 allocs/ms (11.78x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17735 allocs/ms (15.96x)     | 28942 allocs/ms (25.12x)     |


### 3 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 547 allocs/ms                | 553 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4465 allocs/ms (8.16x)       | 4564 allocs/ms (8.25x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8941 allocs/ms (16.34x)      | 9292 allocs/ms (16.80x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12938 allocs/ms (23.64x)     | 15685 allocs/ms (28.37x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 576 allocs/ms                | 599 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 5638 allocs/ms (9.80x)       | 5472 allocs/ms (9.14x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10232 allocs/ms (17.78x)     | 11439 allocs/ms (19.11x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14657 allocs/ms (25.47x)     | 18819 allocs/ms (31.44x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 671 allocs/ms                | 694 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 6754 allocs/ms (10.07x)      | 6799 allocs/ms (9.79x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12454 allocs/ms (18.57x)     | 12675 allocs/ms (18.25x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17070 allocs/ms (25.45x)     | 26207 allocs/ms (37.74x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 726 allocs/ms                | 750 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 6795 allocs/ms (9.36x)       | 6978 allocs/ms (9.30x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12889 allocs/ms (17.75x)     | 13547 allocs/ms (18.05x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 16347 allocs/ms (22.51x)     | 27811 allocs/ms (37.06x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 754 allocs/ms                | 777 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 7131 allocs/ms (9.46x)       | 7397 allocs/ms (9.52x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12445 allocs/ms (16.50x)     | 12050 allocs/ms (15.50x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17772 allocs/ms (23.57x)     | 29850 allocs/ms (38.40x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 1041 allocs/ms               | 1077 allocs/ms               |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 6805 allocs/ms (6.54x)       | 7241 allocs/ms (6.72x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12609 allocs/ms (12.12x)     | 13461 allocs/ms (12.50x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 16987 allocs/ms (16.32x)     | 28192 allocs/ms (26.18x)     |


### 4 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 502 allocs/ms                | 513 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3780 allocs/ms (7.54x)       | 3871 allocs/ms (7.54x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8758 allocs/ms (17.46x)      | 9335 allocs/ms (18.19x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12983 allocs/ms (25.88x)     | 16039 allocs/ms (31.26x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 508 allocs/ms                | 530 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4837 allocs/ms (9.52x)       | 4824 allocs/ms (9.10x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9394 allocs/ms (18.49x)      | 10319 allocs/ms (19.47x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13192 allocs/ms (25.97x)     | 17187 allocs/ms (32.42x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 615 allocs/ms                | 641 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 5725 allocs/ms (9.30x)       | 5845 allocs/ms (9.12x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12031 allocs/ms (19.55x)     | 12024 allocs/ms (18.77x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 16806 allocs/ms (27.31x)     | 23146 allocs/ms (36.13x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 667 allocs/ms                | 693 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 5831 allocs/ms (8.74x)       | 6009 allocs/ms (8.67x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 13050 allocs/ms (19.57x)     | 13456 allocs/ms (19.42x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14574 allocs/ms (21.85x)     | 27287 allocs/ms (39.38x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 700 allocs/ms                | 719 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 6189 allocs/ms (8.84x)       | 6358 allocs/ms (8.84x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12767 allocs/ms (18.23x)     | 13575 allocs/ms (18.88x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17892 allocs/ms (25.55x)     | 28205 allocs/ms (39.23x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 979 allocs/ms                | 1017 allocs/ms               |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 5809 allocs/ms (5.93x)       | 6226 allocs/ms (6.12x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11969 allocs/ms (12.23x)     | 13437 allocs/ms (13.22x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 15961 allocs/ms (16.31x)     | 27896 allocs/ms (27.44x)     |


### 5 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 455 allocs/ms                | 465 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3336 allocs/ms (7.33x)       | 3407 allocs/ms (7.33x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8797 allocs/ms (19.34x)      | 8811 allocs/ms (18.96x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12711 allocs/ms (27.95x)     | 15234 allocs/ms (32.78x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 486 allocs/ms                | 495 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4278 allocs/ms (8.81x)       | 4330 allocs/ms (8.75x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9819 allocs/ms (20.21x)      | 10814 allocs/ms (21.86x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13384 allocs/ms (27.55x)     | 17397 allocs/ms (35.16x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 572 allocs/ms                | 590 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4924 allocs/ms (8.61x)       | 4999 allocs/ms (8.48x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12215 allocs/ms (21.37x)     | 12447 allocs/ms (21.10x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 16615 allocs/ms (29.07x)     | 22401 allocs/ms (37.97x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 618 allocs/ms                | 641 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 5150 allocs/ms (8.34x)       | 5194 allocs/ms (8.11x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12532 allocs/ms (20.28x)     | 12653 allocs/ms (19.75x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 15965 allocs/ms (25.84x)     | 26290 allocs/ms (41.03x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 649 allocs/ms                | 665 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 5410 allocs/ms (8.33x)       | 5420 allocs/ms (8.16x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12728 allocs/ms (19.60x)     | 13607 allocs/ms (20.47x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17427 allocs/ms (26.84x)     | 28400 allocs/ms (42.73x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 903 allocs/ms                | 934 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 5147 allocs/ms (5.70x)       | 5230 allocs/ms (5.60x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11574 allocs/ms (12.82x)     | 12791 allocs/ms (13.70x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 16764 allocs/ms (18.57x)     | 27411 allocs/ms (29.35x)     |


### 6 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 428 allocs/ms                | 434 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2840 allocs/ms (6.64x)       | 2876 allocs/ms (6.62x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8640 allocs/ms (20.20x)      | 9130 allocs/ms (21.02x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12050 allocs/ms (28.17x)     | 15600 allocs/ms (35.91x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 444 allocs/ms                | 456 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3801 allocs/ms (8.56x)       | 3849 allocs/ms (8.44x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9448 allocs/ms (21.28x)      | 10299 allocs/ms (22.59x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13407 allocs/ms (30.19x)     | 17805 allocs/ms (39.05x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 530 allocs/ms                | 538 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4241 allocs/ms (8.00x)       | 4225 allocs/ms (7.85x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11687 allocs/ms (22.04x)     | 12528 allocs/ms (23.28x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 15473 allocs/ms (29.18x)     | 24087 allocs/ms (44.76x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 576 allocs/ms                | 582 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4358 allocs/ms (7.57x)       | 4398 allocs/ms (7.56x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11806 allocs/ms (20.51x)     | 13427 allocs/ms (23.07x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13043 allocs/ms (22.66x)     | 28503 allocs/ms (48.97x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 600 allocs/ms                | 619 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4572 allocs/ms (7.61x)       | 4699 allocs/ms (7.60x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11800 allocs/ms (19.65x)     | 12768 allocs/ms (20.64x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17631 allocs/ms (29.36x)     | 28293 allocs/ms (45.74x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 850 allocs/ms                | 896 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4413 allocs/ms (5.19x)       | 4572 allocs/ms (5.10x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12215 allocs/ms (14.38x)     | 13366 allocs/ms (14.91x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13125 allocs/ms (15.45x)     | 26703 allocs/ms (29.79x)     |


### 7 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 404 allocs/ms                | 408 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2588 allocs/ms (6.40x)       | 2596 allocs/ms (6.35x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8434 allocs/ms (20.86x)      | 8381 allocs/ms (20.52x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 11641 allocs/ms (28.79x)     | 13001 allocs/ms (31.83x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 423 allocs/ms                | 429 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3322 allocs/ms (7.85x)       | 3282 allocs/ms (7.65x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9457 allocs/ms (22.34x)      | 9682 allocs/ms (22.58x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12131 allocs/ms (28.66x)     | 15847 allocs/ms (36.96x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 491 allocs/ms                | 501 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3772 allocs/ms (7.68x)       | 3755 allocs/ms (7.50x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11580 allocs/ms (23.56x)     | 11443 allocs/ms (22.85x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 15422 allocs/ms (31.38x)     | 20803 allocs/ms (41.55x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 541 allocs/ms                | 553 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3962 allocs/ms (7.32x)       | 3953 allocs/ms (7.15x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11776 allocs/ms (21.76x)     | 12434 allocs/ms (22.50x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14860 allocs/ms (27.45x)     | 23310 allocs/ms (42.18x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 572 allocs/ms                | 583 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4197 allocs/ms (7.34x)       | 4227 allocs/ms (7.26x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12023 allocs/ms (21.01x)     | 13192 allocs/ms (22.64x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 17176 allocs/ms (30.02x)     | 27659 allocs/ms (47.48x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 826 allocs/ms                | 856 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 4108 allocs/ms (4.97x)       | 4207 allocs/ms (4.92x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 12133 allocs/ms (14.69x)     | 13185 allocs/ms (15.41x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 16692 allocs/ms (20.20x)     | 26000 allocs/ms (30.38x)     |


### 8 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 380 allocs/ms                | 386 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2374 allocs/ms (6.25x)       | 2347 allocs/ms (6.07x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8163 allocs/ms (21.47x)      | 8229 allocs/ms (21.30x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 11068 allocs/ms (29.11x)     | 11334 allocs/ms (29.33x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 400 allocs/ms                | 406 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3049 allocs/ms (7.62x)       | 3028 allocs/ms (7.45x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9485 allocs/ms (23.71x)      | 9514 allocs/ms (23.41x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12588 allocs/ms (31.46x)     | 14933 allocs/ms (36.74x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 469 allocs/ms                | 475 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3488 allocs/ms (7.43x)       | 3448 allocs/ms (7.26x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10452 allocs/ms (22.27x)     | 10611 allocs/ms (22.35x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 15774 allocs/ms (33.61x)     | 18974 allocs/ms (39.96x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 504 allocs/ms                | 519 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3613 allocs/ms (7.17x)       | 3597 allocs/ms (6.93x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11273 allocs/ms (22.37x)     | 11712 allocs/ms (22.56x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14892 allocs/ms (29.55x)     | 22140 allocs/ms (42.64x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 549 allocs/ms                | 552 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3777 allocs/ms (6.88x)       | 3854 allocs/ms (6.98x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11476 allocs/ms (20.92x)     | 13340 allocs/ms (24.17x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 16003 allocs/ms (29.17x)     | 27597 allocs/ms (50.01x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 794 allocs/ms                | 824 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3693 allocs/ms (4.65x)       | 3966 allocs/ms (4.81x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11257 allocs/ms (14.18x)     | 12634 allocs/ms (15.33x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14872 allocs/ms (18.73x)     | 25898 allocs/ms (31.42x)     |


### 9 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 360 allocs/ms                | 365 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2157 allocs/ms (5.99x)       | 2220 allocs/ms (6.08x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8266 allocs/ms (22.96x)      | 8151 allocs/ms (22.32x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 11278 allocs/ms (31.33x)     | 11182 allocs/ms (30.62x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 381 allocs/ms                | 387 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2835 allocs/ms (7.44x)       | 2767 allocs/ms (7.16x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9548 allocs/ms (25.06x)      | 9618 allocs/ms (24.88x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12054 allocs/ms (31.63x)     | 14338 allocs/ms (37.09x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 445 allocs/ms                | 454 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3230 allocs/ms (7.25x)       | 3141 allocs/ms (6.92x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11423 allocs/ms (25.65x)     | 11071 allocs/ms (24.39x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14612 allocs/ms (32.82x)     | 17702 allocs/ms (39.00x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 473 allocs/ms                | 480 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3351 allocs/ms (7.09x)       | 3311 allocs/ms (6.89x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11370 allocs/ms (24.05x)     | 11559 allocs/ms (24.07x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13561 allocs/ms (28.68x)     | 21857 allocs/ms (45.51x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 503 allocs/ms                | 513 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3455 allocs/ms (6.87x)       | 3415 allocs/ms (6.66x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11180 allocs/ms (22.24x)     | 11449 allocs/ms (22.34x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 15896 allocs/ms (31.62x)     | 21855 allocs/ms (42.64x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 747 allocs/ms                | 781 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3485 allocs/ms (4.67x)       | 3529 allocs/ms (4.52x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11316 allocs/ms (15.16x)     | 11382 allocs/ms (14.58x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14666 allocs/ms (19.64x)     | 21412 allocs/ms (27.43x)     |


### 10 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 342 allocs/ms                | 348 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1967 allocs/ms (5.74x)       | 2008 allocs/ms (5.77x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8226 allocs/ms (24.02x)      | 7985 allocs/ms (22.93x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 10930 allocs/ms (31.92x)     | 11024 allocs/ms (31.66x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 361 allocs/ms                | 368 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2625 allocs/ms (7.27x)       | 2596 allocs/ms (7.05x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9081 allocs/ms (25.14x)      | 9286 allocs/ms (25.21x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12297 allocs/ms (34.05x)     | 13628 allocs/ms (37.00x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 423 allocs/ms                | 431 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2982 allocs/ms (7.04x)       | 2881 allocs/ms (6.69x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10534 allocs/ms (24.88x)     | 10564 allocs/ms (24.52x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14902 allocs/ms (35.19x)     | 18272 allocs/ms (42.42x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 454 allocs/ms                | 462 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3111 allocs/ms (6.85x)       | 3050 allocs/ms (6.60x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11412 allocs/ms (25.13x)     | 11697 allocs/ms (25.32x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14258 allocs/ms (31.40x)     | 21915 allocs/ms (47.45x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 480 allocs/ms                | 491 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3177 allocs/ms (6.62x)       | 3150 allocs/ms (6.42x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11102 allocs/ms (23.13x)     | 11402 allocs/ms (23.22x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14900 allocs/ms (31.04x)     | 21581 allocs/ms (43.96x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 727 allocs/ms                | 753 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3215 allocs/ms (4.42x)       | 3193 allocs/ms (4.24x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11218 allocs/ms (15.43x)     | 11730 allocs/ms (15.58x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14499 allocs/ms (19.94x)     | 21042 allocs/ms (27.95x)     |


### 11 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 327 allocs/ms                | 331 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1844 allocs/ms (5.63x)       | 1872 allocs/ms (5.66x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 7805 allocs/ms (23.83x)      | 7195 allocs/ms (21.76x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 10224 allocs/ms (31.22x)     | 11189 allocs/ms (33.84x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 344 allocs/ms                | 350 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2470 allocs/ms (7.18x)       | 2400 allocs/ms (6.86x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9071 allocs/ms (26.35x)      | 9128 allocs/ms (26.10x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12211 allocs/ms (35.47x)     | 13781 allocs/ms (39.40x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 403 allocs/ms                | 409 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2744 allocs/ms (6.81x)       | 2645 allocs/ms (6.46x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10668 allocs/ms (26.46x)     | 10024 allocs/ms (24.50x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14984 allocs/ms (37.16x)     | 17432 allocs/ms (42.60x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 432 allocs/ms                | 437 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2853 allocs/ms (6.60x)       | 2805 allocs/ms (6.42x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11434 allocs/ms (26.45x)     | 11818 allocs/ms (27.05x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13674 allocs/ms (31.63x)     | 20477 allocs/ms (46.88x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 460 allocs/ms                | 470 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2982 allocs/ms (6.48x)       | 2941 allocs/ms (6.26x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11209 allocs/ms (24.34x)     | 11183 allocs/ms (23.80x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14399 allocs/ms (31.27x)     | 20990 allocs/ms (44.66x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 700 allocs/ms                | 726 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 3001 allocs/ms (4.29x)       | 2979 allocs/ms (4.10x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11384 allocs/ms (16.27x)     | 11566 allocs/ms (15.93x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14463 allocs/ms (20.67x)     | 20742 allocs/ms (28.56x)     |


### 12 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 311 allocs/ms                | 317 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1719 allocs/ms (5.53x)       | 1766 allocs/ms (5.57x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8104 allocs/ms (26.10x)      | 7560 allocs/ms (23.85x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 10469 allocs/ms (33.71x)     | 10938 allocs/ms (34.50x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 328 allocs/ms                | 334 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2269 allocs/ms (6.93x)       | 2217 allocs/ms (6.64x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9241 allocs/ms (28.21x)      | 9260 allocs/ms (27.74x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 11801 allocs/ms (36.03x)     | 12836 allocs/ms (38.46x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 386 allocs/ms                | 392 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2553 allocs/ms (6.60x)       | 2409 allocs/ms (6.14x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10754 allocs/ms (27.83x)     | 10473 allocs/ms (26.71x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14482 allocs/ms (37.47x)     | 17469 allocs/ms (44.54x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 413 allocs/ms                | 420 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2667 allocs/ms (6.45x)       | 2629 allocs/ms (6.25x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11214 allocs/ms (27.12x)     | 11613 allocs/ms (27.63x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14011 allocs/ms (33.89x)     | 20608 allocs/ms (49.02x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 440 allocs/ms                | 450 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2780 allocs/ms (6.32x)       | 2755 allocs/ms (6.12x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11118 allocs/ms (25.28x)     | 11118 allocs/ms (24.68x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13568 allocs/ms (30.85x)     | 20808 allocs/ms (46.20x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 675 allocs/ms                | 699 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2822 allocs/ms (4.18x)       | 2825 allocs/ms (4.04x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10922 allocs/ms (16.18x)     | 11390 allocs/ms (16.30x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14004 allocs/ms (20.74x)     | 19970 allocs/ms (28.57x)     |


### 13 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 298 allocs/ms                | 302 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1633 allocs/ms (5.47x)       | 1654 allocs/ms (5.48x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 7588 allocs/ms (25.43x)      | 7686 allocs/ms (25.47x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 10046 allocs/ms (33.67x)     | 10853 allocs/ms (35.96x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 313 allocs/ms                | 317 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2111 allocs/ms (6.75x)       | 2086 allocs/ms (6.58x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8725 allocs/ms (27.90x)      | 8638 allocs/ms (27.25x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 11747 allocs/ms (37.57x)     | 12910 allocs/ms (40.72x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 370 allocs/ms                | 378 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2332 allocs/ms (6.30x)       | 2301 allocs/ms (6.09x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9081 allocs/ms (24.54x)      | 9746 allocs/ms (25.80x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14295 allocs/ms (38.62x)     | 16941 allocs/ms (44.85x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 396 allocs/ms                | 400 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2539 allocs/ms (6.41x)       | 2477 allocs/ms (6.20x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11060 allocs/ms (27.93x)     | 11221 allocs/ms (28.08x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12962 allocs/ms (32.74x)     | 20274 allocs/ms (50.73x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 422 allocs/ms                | 430 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2622 allocs/ms (6.22x)       | 2577 allocs/ms (5.99x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10971 allocs/ms (26.02x)     | 11255 allocs/ms (26.16x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14112 allocs/ms (33.47x)     | 20113 allocs/ms (46.74x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 659 allocs/ms                | 673 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2655 allocs/ms (4.03x)       | 2616 allocs/ms (3.89x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10805 allocs/ms (16.39x)     | 11158 allocs/ms (16.58x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13849 allocs/ms (21.00x)     | 19510 allocs/ms (28.99x)     |


### 14 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 286 allocs/ms                | 287 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1543 allocs/ms (5.41x)       | 1551 allocs/ms (5.39x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 7576 allocs/ms (26.54x)      | 7492 allocs/ms (26.06x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 9867 allocs/ms (34.56x)      | 10887 allocs/ms (37.87x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 297 allocs/ms                | 304 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1990 allocs/ms (6.70x)       | 1994 allocs/ms (6.55x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8048 allocs/ms (27.10x)      | 8342 allocs/ms (27.40x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 11647 allocs/ms (39.22x)     | 12015 allocs/ms (39.47x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 353 allocs/ms                | 358 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2193 allocs/ms (6.21x)       | 2167 allocs/ms (6.06x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9014 allocs/ms (25.54x)      | 9090 allocs/ms (25.41x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 12906 allocs/ms (36.56x)     | 16426 allocs/ms (45.91x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 381 allocs/ms                | 385 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2397 allocs/ms (6.30x)       | 2343 allocs/ms (6.09x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11044 allocs/ms (29.01x)     | 10863 allocs/ms (28.22x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13092 allocs/ms (34.39x)     | 19874 allocs/ms (51.62x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 408 allocs/ms                | 413 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2460 allocs/ms (6.02x)       | 2410 allocs/ms (5.84x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10756 allocs/ms (26.34x)     | 10695 allocs/ms (25.92x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 14241 allocs/ms (34.87x)     | 20230 allocs/ms (49.02x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 639 allocs/ms                | 654 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2516 allocs/ms (3.94x)       | 2466 allocs/ms (3.77x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 11227 allocs/ms (17.57x)     | 11005 allocs/ms (16.84x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13993 allocs/ms (21.90x)     | 19108 allocs/ms (29.24x)     |


### 15 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 274 allocs/ms                | 277 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1436 allocs/ms (5.25x)       | 1468 allocs/ms (5.30x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 7818 allocs/ms (28.56x)      | 7498 allocs/ms (27.09x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 9994 allocs/ms (36.52x)      | 10658 allocs/ms (38.50x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 288 allocs/ms                | 292 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1921 allocs/ms (6.67x)       | 1882 allocs/ms (6.43x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8960 allocs/ms (31.11x)      | 8982 allocs/ms (30.71x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 11521 allocs/ms (40.00x)     | 12355 allocs/ms (42.24x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 341 allocs/ms                | 346 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2094 allocs/ms (6.14x)       | 2061 allocs/ms (5.96x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10689 allocs/ms (31.37x)     | 10317 allocs/ms (29.84x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13705 allocs/ms (40.22x)     | 15936 allocs/ms (46.09x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 365 allocs/ms                | 370 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2269 allocs/ms (6.21x)       | 2212 allocs/ms (5.98x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10677 allocs/ms (29.22x)     | 10660 allocs/ms (28.80x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13466 allocs/ms (36.85x)     | 19224 allocs/ms (51.95x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 392 allocs/ms                | 399 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2341 allocs/ms (5.97x)       | 2306 allocs/ms (5.79x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10506 allocs/ms (26.80x)     | 11011 allocs/ms (27.62x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13903 allocs/ms (35.47x)     | 19999 allocs/ms (50.17x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 619 allocs/ms                | 627 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2401 allocs/ms (3.88x)       | 2369 allocs/ms (3.78x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9881 allocs/ms (15.97x)      | 9879 allocs/ms (15.76x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13968 allocs/ms (22.57x)     | 17572 allocs/ms (28.03x)     |


### 16 Constructor Parameters

#### .NET Framework 4.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 265 allocs/ms                | 268 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1361 allocs/ms (5.13x)       | 1381 allocs/ms (5.14x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 7768 allocs/ms (29.29x)      | 7140 allocs/ms (26.60x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 10004 allocs/ms (37.73x)     | 10580 allocs/ms (39.42x)     |

#### .NET Core 2.2.8

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 280 allocs/ms                | 282 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1833 allocs/ms (6.56x)       | 1804 allocs/ms (6.39x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 8322 allocs/ms (29.76x)      | 8316 allocs/ms (29.47x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 11590 allocs/ms (41.45x)     | 11681 allocs/ms (41.40x)     |

#### .NET Core 3.1.32

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 330 allocs/ms                | 333 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 1972 allocs/ms (5.98x)       | 1956 allocs/ms (5.88x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10059 allocs/ms (30.52x)     | 9811 allocs/ms (29.48x)      |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13312 allocs/ms (40.39x)     | 15348 allocs/ms (46.12x)     |

#### .NET 5.0.17

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 354 allocs/ms                | 358 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2137 allocs/ms (6.04x)       | 2105 allocs/ms (5.88x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9908 allocs/ms (28.01x)      | 10043 allocs/ms (28.07x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13315 allocs/ms (37.65x)     | 16109 allocs/ms (45.03x)     |

#### .NET 6.0.14

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 380 allocs/ms                | 386 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2214 allocs/ms (5.83x)       | 2210 allocs/ms (5.73x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 10285 allocs/ms (27.08x)     | 10142 allocs/ms (26.27x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13912 allocs/ms (36.62x)     | 16675 allocs/ms (43.19x)     |

#### .NET 7.0.3

| Method                                                                       | Class                        | Struct                       |
|------------------------------------------------------------------------------|------------------------------|------------------------------|
| object Activator.CreateInstance(Type, object[])                              | 595 allocs/ms                | 615 allocs/ms                |
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[])       | 2241 allocs/ms (3.77x)       | 2240 allocs/ms (3.64x)       |
| object FastActivator.CreateInstance&lt;...&gt;(...)                          | 9648 allocs/ms (16.22x)      | 10019 allocs/ms (16.29x)     |
| T      FastActivator&lt;T&gt;.CreateInstance&lt;...&gt;(...)                 | 13098 allocs/ms (22.03x)     | 14842 allocs/ms (24.13x)     |
