# Griffin+ Fast Activator

[![Build status](https://ci.appveyor.com/api/projects/status/1kdebht6cre074no/branch/master?svg=true)](https://ci.appveyor.com/project/ravenpride/dotnet-libs-fastactivator/branch/master)

## Overview

The *Fast Activator* is part of the Griffin+ Common Library Suite. It replaces some of the functions of the .NET `System.Activator` class. `System.Activator` is the standard tool for dynamic instantiation of types (classes/structures) under .NET. It is used, if you do not know which specific type must be instantiated at the time of compilation. Instantiating classes/structures in the same process is part of this functionality, but an immensely important part. All serialization mechanisms require this functionality to generate instances of certain types when deserializing objects.

The standard activator of .NET decides which constructor to call from concrete constructor arguments. This procedure is very flexible, but also time-consuming. In most cases, you know which constructor you need. So you do not have to have it search again with every instantiation. In contrast *Fast Activator* requires the explicit specification of the parameter types of the constructor to be used. It then dynamically generates functions that only do what you would do in the code to instantiate the type in question, namely the creation ala `new MyType(arg1, arg2,...)`. These dynamically generated functions are cached and are immediately available for the next use. The performance increase achieved is enormous (see benchmarks below).

## Supported Platforms

The *Fast Activator* is written in .NET Standard 2.0.

Therefore the library should work on the following platforms (or higher):
- .NET Framework 4.6.1
- .NET Core 2.0
- Mono 5.4
- Xamarin iOS 10.14
- Xamarin Mac 3.8
- Xamarin Android 8.0
- Universal Windows Platform (UWP) 10.0.16299

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
public static object CreateInstance<TARG>(Type type, TARG arg);
public static object CreateInstance<TARG1, TARG2>(Type type, TARG1 arg1, TARG2 arg2);
public static object CreateInstance<TARG1, TARG2, TARG3>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14, TARG15 arg15);
public static object CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, TARG16>(Type type, TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14, TARG15 arg15, TARG16 arg16);
```

If you need to create a one-dimensional array of a certain type that is not known at compile time, you can use:

```csharp
public static Array CreateArray(Type type, int length);
```

### Class: GriffinPlus.Lib.FastActivator&lt;T&gt;

If you need to create an instance of a certain type that is known at compile time or available via generic type arguments, you can use:

```csharp
public static T CreateInstance();
public static T CreateInstance<TARG>(TARG arg);
public static T CreateInstance<TARG1, TARG2>(TARG1 arg1, TARG2 arg2);
public static T CreateInstance<TARG1, TARG2, TARG3>(TARG1 arg1, TARG2 arg2, TARG3 arg3);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14, TARG15 arg15);
public static T CreateInstance<TARG1, TARG2, TARG3, TARG4, TARG5, TARG6, TARG7, TARG8, TARG9, TARG10, TARG11, TARG12, TARG13, TARG14, TARG15, TARG16>(TARG1 arg1, TARG2 arg2, TARG3 arg3, TARG4 arg4, TARG5 arg5, TARG6 arg6, TARG7 arg7, TARG8 arg8, TARG9 arg9, TARG10 arg10, TARG11 arg11, TARG12 arg12, TARG13 arg13, TARG14 arg14, TARG15 arg15, TARG16 arg16);
```

## Benchmark

To compare the performance of the *Fast Activator* with .NET's default activator a benchmark application is part of the repository. The benchmark creates 50 million instances of a test class/struct using the default activator and the different *Fast Activator* methods. It measures the time the instantiation needs to complete and compares the times measured for *Fast Activator* to the time the standard activator needed to complete.

The machine the benchmark runs on is a Virtual Box with Windows 10 (64 bit) installed. 4 virtual cores are configured, but only 1 core is actually used during the benchmark). As only relative measurements are required, that should not be an issue. The executing runtime is *.NET Core 2.0* (64 bit).

### No Constructor Parameters (Generic)

| Method                              | Class             | Struct
|-------------------------------------|-------------------|-------------------
| T Activator.CreateInstance<T>()     | 2233ms            | 1926ms
| T FastActivator<T>.CreateInstance() | **738ms (3.03x)** | **237ms (8.13x)**

### No Constructor Parameters (Non-Generic)

| Method                                    | Class              | Struct
|-------------------------------------------|--------------------|-------------------
| object Activator.CreateInstance(Type)     | 2188ms             | 1934ms
| object FastActivator.CreateInstance(Type) | **1512ms (1.45x)** | **1444ms (1.34x)**

### 1 Constructor Parameter

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 40506ms             | 39136ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 5822ms (6.96x)      | 5504ms (7.11x)
| object FastActivator.CreateInstance<...>(...)                          | **2438ms (16.61x)** | 2453ms (15.95x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4184ms (9.68x)      | **1543ms (25.36x)**

### 2 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 45330ms             | 43688ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 6153ms (7.37x)      | 5689ms (7.68x)
| object FastActivator.CreateInstance<...>(...)                          | **2468ms (18.37x)** | 2446ms (17.86x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4125ms (10.99x)     | **1449ms (30.15x)**
  
### 3 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 49857ms             | 48079ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 6777ms (7.36x)      | 6388ms (7.53x)
| object FastActivator.CreateInstance<...>(...)                          | **2590ms (19.25x)** | 2470ms (19.47x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4238ms (11.76x)     | **1629ms (29.51x)**
  
### 4 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 53605ms             | 52922ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 6741ms (7.95x)      | 6467ms (8.18x)
| object FastActivator.CreateInstance<...>(...)                          | **2688ms (19.94x)** | 2768ms (19.12x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4300ms (12.47x)     | **1626ms (32.55x)**
  
### 5 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 58465ms             | 57936ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 7510ms (7.78x)      | 7190ms (8.06x)
| object FastActivator.CreateInstance<...>(...)                          | **2648ms (22.08x)** | 2696ms (21.49x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4244ms (13.78x)     | **1501ms (38.60x)**
  
### 6 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 63606ms             | 63726ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 7805ms (8.15x)      | 7313ms (8.71x)
| object FastActivator.CreateInstance<...>(...)                          | **2629ms (24.19x)** | 2529ms (25.20x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4323ms (14.71x)     | **1536ms (41.49x)**
  
### 7 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 68450ms             | 67271ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 8734ms (7.84x)      | 8553ms (7.87x)
| object FastActivator.CreateInstance<...>(...)                          | **2731ms (25.06x)** | 2770ms (24.29x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4495ms (15.23x)     | **1778ms (37.84x)**

### 8 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 70927ms             | 70018ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 8309ms (8.54x)      | 8117ms (8.63x)
| object FastActivator.CreateInstance<...>(...)                          | **2675ms (26.51x)** | 2690ms (26.03x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4490ms (15.80x)     | **1807ms (38.75x)**

### 9 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 74254ms             | 73808ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 9479ms (7.83x)      | 9236ms (7.99x)
| object FastActivator.CreateInstance<...>(...)                          | **2655ms (27.97x)** | 2719ms (27.15x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4399ms (16.88x)     | **1729ms (42.69x)**

### 10 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 77811ms             | 77405ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 9613ms (8.09x)      | 9457ms (8.18x)
| object FastActivator.CreateInstance<...>(...)                          | **2690ms (28.93x)** | 2765ms (27.99x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4497ms (17.30x)     | **1767ms (43.81x)**

### 11 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 80367ms             | 80818ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 10330ms (7.78x)     | 10103ms (8.00x)
| object FastActivator.CreateInstance<...>(...)                          | **2898ms (27.73x)** | 2952ms (27.38x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4553ms (17.65x)     | **1822ms (44.36x)**

### 12 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 83998ms             | 84513ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 10168ms (8.26x)     | 9969ms (8.48x)
| object FastActivator.CreateInstance<...>(...)                          | **2869ms (29.28x)** | 2892ms (29.22x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 5250ms (16.00x)     | **1881ms (44.93x)**

### 13 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 87693ms             | 88327ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 11084ms (7.91x)     | 10874ms (8.12x)
| object FastActivator.CreateInstance<...>(...)                          | **2903ms (30.21x)** | 2804ms (31.50x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 5070ms (17.30x)     | **1951ms (45.27x)**
  
### 14 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 91378ms             | 92178ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 11238ms (8.13x)     | 11113ms (8.29x)
| object FastActivator.CreateInstance<...>(...)                          | **2949ms (30.99x)** | 2932ms (31.44x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4758ms (19.21x)     | **1951ms (47.25x)**
  
### 15 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 94903ms             | 96048ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 11887ms (7.98x)     | 11679ms (8.22x)
| object FastActivator.CreateInstance<...>(...)                          | **2953ms (32.14x)** | 3127ms (30.72x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4872ms (19.48x)     | **2023ms (47.48x)**
  
### 16 Constructor Parameters

| Method                                                                 | Class               | Struct
|------------------------------------------------------------------------|---------------------|-------------------
| object Activator.CreateInstance(Type, object[])                        | 98163ms             | 98860ms
| object FastActivator.CreateInstanceDynamically(Type, Type[], object[]) | 12508ms (7.85x)     | 12341ms (8.01x)
| object FastActivator.CreateInstance<...>(...)                          | **2994ms (32.79x)** | 3127ms (31.61x)
|      T FastActivator<T>.CreateInstance<...>(...)                       | 4876ms (20.13x)     | **2130mx (46.41x)**

