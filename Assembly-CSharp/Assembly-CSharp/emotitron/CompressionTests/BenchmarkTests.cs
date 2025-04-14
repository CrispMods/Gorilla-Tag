using System;
using System.Diagnostics;
using emotitron.Compression;
using UnityEngine;

namespace emotitron.CompressionTests
{
	// Token: 0x02000C7A RID: 3194
	public class BenchmarkTests : MonoBehaviour
	{
		// Token: 0x0600509F RID: 20639 RVA: 0x00187EA5 File Offset: 0x001860A5
		private void Start()
		{
			BenchmarkTests.TestWriterIntegrity();
			BenchmarkTests.ArrayCopy();
			BenchmarkTests.ArrayCopySafe();
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x00187EB8 File Offset: 0x001860B8
		public static void TestWriterIntegrity()
		{
			int num = 1;
			int num2 = 1;
			BenchmarkTests.ubuffer.Write(ulong.MaxValue, ref num, 64);
			if (BenchmarkTests.ubuffer.Read(ref num2, 64) != 18446744073709551615UL)
			{
				Debug.Log("Error writing with maxulong");
			}
			for (int i = 0; i < 3000; i++)
			{
				num = Random.Range(0, 200);
				num2 = num;
				int num3 = num;
				int num4 = Random.Range(1, 64);
				int num5 = Random.Range(-(1 << num4 - 1), (1 << num4 - 1) - 1);
				BenchmarkTests.ubuffer.WriteSigned(num5, ref num, num4);
				BenchmarkTests.ubuffer.WriteSigned(num5, ref num, num4);
				BenchmarkTests.ubuffer.WriteSigned(num5, ref num, num4);
				if (BenchmarkTests.ubuffer.ReadSigned(ref num2, num4) != num5)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Error writing ",
						num5.ToString(),
						" to pos ",
						num3.ToString(),
						" with size ",
						num4.ToString()
					}));
				}
				if (BenchmarkTests.ubuffer.ReadSigned(ref num2, num4) != num5)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Error writing ",
						num5.ToString(),
						" to pos ",
						num3.ToString(),
						" with size ",
						num4.ToString()
					}));
				}
				if (BenchmarkTests.ubuffer.ReadSigned(ref num2, num4) != num5)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Error writing ",
						num5.ToString(),
						" to pos ",
						num3.ToString(),
						" with size ",
						num4.ToString()
					}));
				}
				ulong num6 = (ulong)Random.Range(0f, (1L << num4) - 1L);
				BenchmarkTests.ubuffer.Write(num6, ref num, num4);
				BenchmarkTests.ubuffer.Write(num6, ref num, num4);
				BenchmarkTests.ubuffer.Write(num6, ref num, num4);
				if (BenchmarkTests.ubuffer.Read(ref num2, num4) != num6)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Error writing ",
						num6.ToString(),
						" to pos ",
						num3.ToString(),
						" with size ",
						num4.ToString()
					}));
				}
				if (BenchmarkTests.ubuffer.Read(ref num2, num4) != num6)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Error writing ",
						num6.ToString(),
						" to pos ",
						num3.ToString(),
						" with size ",
						num4.ToString()
					}));
				}
				if (BenchmarkTests.ubuffer.Read(ref num2, num4) != num6)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Error writing ",
						num6.ToString(),
						" to pos ",
						num3.ToString(),
						" with size ",
						num4.ToString()
					}));
				}
			}
			Debug.Log("Integrity check complete.");
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x001881D8 File Offset: 0x001863D8
		private static void TestLog2()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (uint num = 0U; num <= 4294967295U; num += 3000U)
			{
				num.UsedBitCount();
				num.UsedBitCount();
				num.UsedBitCount();
				num.UsedBitCount();
				num.UsedBitCount();
				if (4294967295U - num < 4000U)
				{
					break;
				}
			}
			stopwatch.Stop();
			Debug.Log("Log2 nifty: time=" + stopwatch.ElapsedMilliseconds.ToString() + " ms");
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x00188250 File Offset: 0x00186450
		private static void ArrayCopy()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < 1000000; i++)
			{
				int num = 0;
				BenchmarkTests.ubuffer.ReadOutUnsafe(0, BenchmarkTests.buffer, ref num, 960);
			}
			stopwatch.Stop();
			Debug.Log("Array Copy Unsafe: time=" + stopwatch.ElapsedMilliseconds.ToString() + " ms");
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x001882B4 File Offset: 0x001864B4
		private static void ArrayCopySafe()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < 1000000; i++)
			{
				int num = 0;
				BenchmarkTests.ubuffer.ReadOutSafe(0, BenchmarkTests.buffer, ref num, 960);
			}
			stopwatch.Stop();
			Debug.Log("Array Copy Safe: time=" + stopwatch.ElapsedMilliseconds.ToString() + " ms");
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x00188318 File Offset: 0x00186518
		public static void ByteForByteWrite()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < 1000000; i++)
			{
				BasicWriter.Reset();
				for (int j = 0; j < 128; j++)
				{
					BasicWriter.BasicWrite(BenchmarkTests.buffer, byte.MaxValue);
				}
				BasicWriter.Reset();
				for (int k = 0; k < 128; k++)
				{
					BasicWriter.BasicRead(BenchmarkTests.buffer);
				}
			}
			stopwatch.Stop();
			Debug.Log("Byte For Byte: time=" + stopwatch.ElapsedMilliseconds.ToString() + " ms");
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x001883AC File Offset: 0x001865AC
		public static void BitpackBytesEven()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < 1000000; i++)
			{
				int num = 0;
				for (int j = 0; j < 128; j++)
				{
					BenchmarkTests.buffer.Write(255UL, ref num, 8);
				}
				num = 0;
				for (int k = 0; k < 127; k++)
				{
					BenchmarkTests.buffer.Read(ref num, 8);
				}
			}
			stopwatch.Stop();
			Debug.Log("Even Bitpack byte: time=" + stopwatch.ElapsedMilliseconds.ToString() + " ms");
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x00188440 File Offset: 0x00186640
		public static void BitpackBytesToULongUneven()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < 1000000; i++)
			{
				int num = 0;
				BenchmarkTests.ubuffer.Write(1UL, ref num, 1);
				for (int j = 0; j < 127; j++)
				{
					BenchmarkTests.ubuffer.Write(255UL, ref num, 33);
				}
				num = 0;
				BenchmarkTests.ubuffer.Read(ref num, 1);
				for (int k = 0; k < 127; k++)
				{
					BenchmarkTests.ubuffer.Read(ref num, 33);
				}
			}
			stopwatch.Stop();
			Debug.Log("Uneven Bitpack ulong[]: time=" + stopwatch.ElapsedMilliseconds.ToString() + " ms");
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x001884F0 File Offset: 0x001866F0
		public static void BitpackBytesUnEven()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < 1000000; i++)
			{
				int num = 0;
				BenchmarkTests.buffer.Write(1UL, ref num, 1);
				for (int j = 0; j < 127; j++)
				{
					BenchmarkTests.buffer.Write(255UL, ref num, 8);
				}
				num = 0;
				BenchmarkTests.buffer.Read(ref num, 1);
				for (int k = 0; k < 127; k++)
				{
					BenchmarkTests.buffer.Read(ref num, 8);
				}
			}
			stopwatch.Stop();
			Debug.Log("Uneven Bitpack byte: time=" + stopwatch.ElapsedMilliseconds.ToString() + " ms");
		}

		// Token: 0x04005324 RID: 21284
		public const int BYTE_CNT = 128;

		// Token: 0x04005325 RID: 21285
		public const int LOOP = 1000000;

		// Token: 0x04005326 RID: 21286
		public static byte[] buffer = new byte[4800];

		// Token: 0x04005327 RID: 21287
		public static uint[] ibuffer = new uint[128];

		// Token: 0x04005328 RID: 21288
		public static ulong[] ubuffer = new ulong[128];

		// Token: 0x04005329 RID: 21289
		public static ulong[] ubuffer2 = new ulong[128];
	}
}
