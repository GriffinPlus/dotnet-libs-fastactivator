///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This file is part of the Griffin+ common library suite (https://github.com/griffinplus/dotnet-libs-fastactivator)
// The source code is licensed under the MIT license.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// ReSharper disable UnusedParameter.Local

namespace GriffinPlus.Lib
{

	public struct TestStruct<T> : ITestData<T>
	{
		public T[] Values { get; set; }

		public TestStruct(RefStruct test)
		{
			// just for testing whether ref structs now work as constructor parameters
			Values = new T[16];
		}

		public TestStruct(T test0)
		{
			Values = new T[16];
			Values[0] = test0;
		}

		public TestStruct(T test0, T test1)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
		}

		public TestStruct(T test0, T test1, T test2)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6,
			T test7)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
			Values[7] = test7;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6,
			T test7,
			T test8)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
			Values[7] = test7;
			Values[8] = test8;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6,
			T test7,
			T test8,
			T test9)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
			Values[7] = test7;
			Values[8] = test8;
			Values[9] = test9;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6,
			T test7,
			T test8,
			T test9,
			T test10)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
			Values[7] = test7;
			Values[8] = test8;
			Values[9] = test9;
			Values[10] = test10;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6,
			T test7,
			T test8,
			T test9,
			T test10,
			T test11)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
			Values[7] = test7;
			Values[8] = test8;
			Values[9] = test9;
			Values[10] = test10;
			Values[11] = test11;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6,
			T test7,
			T test8,
			T test9,
			T test10,
			T test11,
			T test12)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
			Values[7] = test7;
			Values[8] = test8;
			Values[9] = test9;
			Values[10] = test10;
			Values[11] = test11;
			Values[12] = test12;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6,
			T test7,
			T test8,
			T test9,
			T test10,
			T test11,
			T test12,
			T test13)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
			Values[7] = test7;
			Values[8] = test8;
			Values[9] = test9;
			Values[10] = test10;
			Values[11] = test11;
			Values[12] = test12;
			Values[13] = test13;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6,
			T test7,
			T test8,
			T test9,
			T test10,
			T test11,
			T test12,
			T test13,
			T test14)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
			Values[7] = test7;
			Values[8] = test8;
			Values[9] = test9;
			Values[10] = test10;
			Values[11] = test11;
			Values[12] = test12;
			Values[13] = test13;
			Values[14] = test14;
		}

		public TestStruct(
			T test0,
			T test1,
			T test2,
			T test3,
			T test4,
			T test5,
			T test6,
			T test7,
			T test8,
			T test9,
			T test10,
			T test11,
			T test12,
			T test13,
			T test14,
			T test15)
		{
			Values = new T[16];
			Values[0] = test0;
			Values[1] = test1;
			Values[2] = test2;
			Values[3] = test3;
			Values[4] = test4;
			Values[5] = test5;
			Values[6] = test6;
			Values[7] = test7;
			Values[8] = test8;
			Values[9] = test9;
			Values[10] = test10;
			Values[11] = test11;
			Values[12] = test12;
			Values[13] = test13;
			Values[14] = test14;
			Values[15] = test15;
		}
	}

}
