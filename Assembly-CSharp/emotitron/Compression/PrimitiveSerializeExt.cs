using System;
using emotitron.Compression.HalfFloat;
using emotitron.Compression.Utilities;

namespace emotitron.Compression
{
	// Token: 0x02000C6B RID: 3179
	public static class PrimitiveSerializeExt
	{
		// Token: 0x06004FBF RID: 20415 RVA: 0x00185EE6 File Offset: 0x001840E6
		public static void Inject(this ByteConverter value, ref ulong buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x00185EF6 File Offset: 0x001840F6
		public static void Inject(this ByteConverter value, ref uint buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x00185F06 File Offset: 0x00184106
		public static void Inject(this ByteConverter value, ref ushort buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x00185F16 File Offset: 0x00184116
		public static void Inject(this ByteConverter value, ref byte buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x00185F28 File Offset: 0x00184128
		public static ulong WriteSigned(this ulong buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x00185F48 File Offset: 0x00184148
		public static void InjectSigned(this long value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x00185F5B File Offset: 0x0018415B
		public static void InjectSigned(this int value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FC6 RID: 20422 RVA: 0x00185F5B File Offset: 0x0018415B
		public static void InjectSigned(this short value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x00185F5B File Offset: 0x0018415B
		public static void InjectSigned(this sbyte value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x00185F70 File Offset: 0x00184170
		public static int ReadSigned(this ulong buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x00185F94 File Offset: 0x00184194
		public static uint WriteSigned(this uint buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x00185FB4 File Offset: 0x001841B4
		public static void InjectSigned(this long value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FCB RID: 20427 RVA: 0x00185FC7 File Offset: 0x001841C7
		public static void InjectSigned(this int value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x00185FC7 File Offset: 0x001841C7
		public static void InjectSigned(this short value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x00185FC7 File Offset: 0x001841C7
		public static void InjectSigned(this sbyte value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x00185FDC File Offset: 0x001841DC
		public static int ReadSigned(this uint buffer, ref int bitposition, int bits)
		{
			uint num = buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x00186000 File Offset: 0x00184200
		public static ushort WriteSigned(this ushort buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x00186020 File Offset: 0x00184220
		public static void InjectSigned(this long value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x00186033 File Offset: 0x00184233
		public static void InjectSigned(this int value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x00186033 File Offset: 0x00184233
		public static void InjectSigned(this short value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x00186033 File Offset: 0x00184233
		public static void InjectSigned(this sbyte value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x00186048 File Offset: 0x00184248
		public static int ReadSigned(this ushort buffer, ref int bitposition, int bits)
		{
			uint num = buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x0018606C File Offset: 0x0018426C
		public static byte WriteSigned(this byte buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x0018608C File Offset: 0x0018428C
		public static void InjectSigned(this long value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x0018609F File Offset: 0x0018429F
		public static void InjectSigned(this int value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x0018609F File Offset: 0x0018429F
		public static void InjectSigned(this short value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x0018609F File Offset: 0x0018429F
		public static void InjectSigned(this sbyte value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x001860B4 File Offset: 0x001842B4
		public static int ReadSigned(this byte buffer, ref int bitposition, int bits)
		{
			uint num = buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x001860D5 File Offset: 0x001842D5
		public static ulong WritetBool(this ulong buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x001860E7 File Offset: 0x001842E7
		public static uint WritetBool(this uint buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x001860F9 File Offset: 0x001842F9
		public static ushort WritetBool(this ushort buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x0018610B File Offset: 0x0018430B
		public static byte WritetBool(this byte buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x0018611D File Offset: 0x0018431D
		public static void Inject(this bool value, ref ulong buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x0018612F File Offset: 0x0018432F
		public static void Inject(this bool value, ref uint buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x00186141 File Offset: 0x00184341
		public static void Inject(this bool value, ref ushort buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x00186153 File Offset: 0x00184353
		public static void Inject(this bool value, ref byte buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x00186165 File Offset: 0x00184365
		public static bool ReadBool(this ulong buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0UL;
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x00186174 File Offset: 0x00184374
		public static bool ReadtBool(this uint buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0U;
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x00186183 File Offset: 0x00184383
		public static bool ReadBool(this ushort buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0U;
		}

		// Token: 0x06004FE6 RID: 20454 RVA: 0x00186192 File Offset: 0x00184392
		public static bool ReadBool(this byte buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0U;
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x001861A4 File Offset: 0x001843A4
		public static ulong Write(this ulong buffer, ulong value, ref int bitposition, int bits = 64)
		{
			ulong num = value << bitposition;
			ulong num2 = ulong.MaxValue >> 64 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x001861E0 File Offset: 0x001843E0
		public static uint Write(this uint buffer, ulong value, ref int bitposition, int bits = 64)
		{
			uint num = (uint)value << bitposition;
			uint num2 = uint.MaxValue >> 32 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x0018621C File Offset: 0x0018441C
		public static ushort Write(this ushort buffer, ulong value, ref int bitposition, int bits = 64)
		{
			uint num = (uint)value << bitposition;
			uint num2 = 65535U >> 16 - bits << bitposition;
			buffer = (ushort)(((uint)buffer & ~num2) | (num2 & num));
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x00186258 File Offset: 0x00184458
		public static byte Write(this byte buffer, ulong value, ref int bitposition, int bits = 64)
		{
			uint num = (uint)value << bitposition;
			uint num2 = 255U >> 8 - bits << bitposition;
			buffer = (byte)(((uint)buffer & ~num2) | (num2 & num));
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x00186293 File Offset: 0x00184493
		public static void Inject(this ulong value, ref ulong buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x001862A4 File Offset: 0x001844A4
		public static void Inject(this ulong value, ref ulong buffer, int bitposition, int bits = 64)
		{
			ulong num = value << bitposition;
			ulong num2 = ulong.MaxValue >> 64 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x001862D7 File Offset: 0x001844D7
		public static void Inject(this uint value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x001862E8 File Offset: 0x001844E8
		public static void Inject(this uint value, ref ulong buffer, int bitposition, int bits = 32)
		{
			ulong num = (ulong)value << bitposition;
			ulong num2 = ulong.MaxValue >> 64 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x001862D7 File Offset: 0x001844D7
		public static void Inject(this ushort value, ref ulong buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x0018631C File Offset: 0x0018451C
		public static void Inject(this ushort value, ref ulong buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001862D7 File Offset: 0x001844D7
		public static void Inject(this byte value, ref ulong buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x0018631C File Offset: 0x0018451C
		public static void Inject(this byte value, ref ulong buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x00186293 File Offset: 0x00184493
		public static void InjectUnsigned(this long value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x0018632C File Offset: 0x0018452C
		public static void InjectUnsigned(this int value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x0018632C File Offset: 0x0018452C
		public static void InjectUnsigned(this short value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x0018632C File Offset: 0x0018452C
		public static void InjectUnsigned(this sbyte value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x0018633B File Offset: 0x0018453B
		public static void Inject(this ulong value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x00186349 File Offset: 0x00184549
		public static void Inject(this ulong value, ref uint buffer, int bitposition, int bits = 64)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x00186358 File Offset: 0x00184558
		public static void Inject(this uint value, ref uint buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x00186367 File Offset: 0x00184567
		public static void Inject(this uint value, ref uint buffer, int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x00186358 File Offset: 0x00184558
		public static void Inject(this ushort value, ref uint buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x00186367 File Offset: 0x00184567
		public static void Inject(this ushort value, ref uint buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFD RID: 20477 RVA: 0x00186358 File Offset: 0x00184558
		public static void Inject(this byte value, ref uint buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x00186367 File Offset: 0x00184567
		public static void Inject(this byte value, ref uint buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x0018633B File Offset: 0x0018453B
		public static void InjectUnsigned(this long value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x00186377 File Offset: 0x00184577
		public static void InjectUnsigned(this int value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x00186377 File Offset: 0x00184577
		public static void InjectUnsigned(this short value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x00186377 File Offset: 0x00184577
		public static void InjectUnsigned(this sbyte value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x00186386 File Offset: 0x00184586
		public static void Inject(this ulong value, ref ushort buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x00186394 File Offset: 0x00184594
		public static void Inject(this ulong value, ref ushort buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005005 RID: 20485 RVA: 0x001863A3 File Offset: 0x001845A3
		public static void Inject(this uint value, ref ushort buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005006 RID: 20486 RVA: 0x001863B2 File Offset: 0x001845B2
		public static void Inject(this uint value, ref ushort buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x001863A3 File Offset: 0x001845A3
		public static void Inject(this ushort value, ref ushort buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x001863B2 File Offset: 0x001845B2
		public static void Inject(this ushort value, ref ushort buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x001863A3 File Offset: 0x001845A3
		public static void Inject(this byte value, ref ushort buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x001863B2 File Offset: 0x001845B2
		public static void Inject(this byte value, ref ushort buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x001863C2 File Offset: 0x001845C2
		public static void Inject(this ulong value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x001863D0 File Offset: 0x001845D0
		public static void Inject(this ulong value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x001863DF File Offset: 0x001845DF
		public static void Inject(this uint value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x001863EE File Offset: 0x001845EE
		public static void Inject(this uint value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x001863DF File Offset: 0x001845DF
		public static void Inject(this ushort value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x001863EE File Offset: 0x001845EE
		public static void Inject(this ushort value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x001863DF File Offset: 0x001845DF
		public static void Inject(this byte value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x001863EE File Offset: 0x001845EE
		public static void Inject(this byte value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x001863FE File Offset: 0x001845FE
		[Obsolete("Argument order changed")]
		public static ulong Extract(this ulong value, int bits, ref int bitposition)
		{
			return value.Extract(bits, ref bitposition);
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x00186408 File Offset: 0x00184608
		public static ulong Read(this ulong value, ref int bitposition, int bits)
		{
			ulong num = ulong.MaxValue >> 64 - bits;
			ulong result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x00186430 File Offset: 0x00184630
		[Obsolete("Use Read instead.")]
		public static ulong Extract(this ulong value, ref int bitposition, int bits)
		{
			ulong num = ulong.MaxValue >> 64 - bits;
			ulong result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x00186458 File Offset: 0x00184658
		[Obsolete("Always include the [ref int bitposition] argument. Extracting from position 0 would be better handled with a mask operation.")]
		public static ulong Extract(this ulong value, int bits)
		{
			ulong num = ulong.MaxValue >> 64 - bits;
			return value & num;
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x00186474 File Offset: 0x00184674
		public static uint Read(this uint value, ref int bitposition, int bits)
		{
			uint num = uint.MaxValue >> 32 - bits;
			uint result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x0018649C File Offset: 0x0018469C
		[Obsolete("Use Read instead.")]
		public static uint Extract(this uint value, ref int bitposition, int bits)
		{
			uint num = uint.MaxValue >> 32 - bits;
			uint result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x001864C4 File Offset: 0x001846C4
		[Obsolete("Always include the [ref int bitposition] argument. Extracting from position 0 would be better handled with a mask operation.")]
		public static uint Extract(this uint value, int bits)
		{
			uint num = uint.MaxValue >> 32 - bits;
			return value & num;
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x001864E0 File Offset: 0x001846E0
		public static uint Read(this ushort value, ref int bitposition, int bits)
		{
			uint num = 65535U >> 16 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x0018650C File Offset: 0x0018470C
		[Obsolete("Use Read instead.")]
		public static uint Extract(this ushort value, ref int bitposition, int bits)
		{
			uint num = 65535U >> 16 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x0600501C RID: 20508 RVA: 0x00186538 File Offset: 0x00184738
		public static uint Read(this byte value, ref int bitposition, int bits)
		{
			uint num = 255U >> 8 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x0600501D RID: 20509 RVA: 0x00186564 File Offset: 0x00184764
		[Obsolete("Use Read instead.")]
		public static uint Extract(this byte value, ref int bitposition, int bits)
		{
			uint num = 255U >> 8 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x0600501E RID: 20510 RVA: 0x00186590 File Offset: 0x00184790
		[Obsolete("Always include the [ref int bitposition] argument. Extracting from position 0 would be better handled with a mask operation.")]
		public static byte Extract(this byte value, int bits)
		{
			uint num = 255U >> 8 - bits;
			return (byte)((uint)value & num);
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x001865AE File Offset: 0x001847AE
		public static void Inject(this float f, ref ulong buffer, ref int bitposition)
		{
			buffer = buffer.Write(f, ref bitposition, 32);
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x001865C7 File Offset: 0x001847C7
		public static float ReadFloat(this ulong buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 32);
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x001865DC File Offset: 0x001847DC
		[Obsolete("Use Read instead.")]
		public static float ExtractFloat(this ulong buffer, ref int bitposition)
		{
			return buffer.Extract(ref bitposition, 32);
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x001865F4 File Offset: 0x001847F4
		public static ushort InjectAsHalfFloat(this float f, ref ulong buffer, ref int bitposition)
		{
			ushort num = HalfUtilities.Pack(f);
			buffer = buffer.Write((ulong)num, ref bitposition, 16);
			return num;
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x00186618 File Offset: 0x00184818
		public static ushort InjectAsHalfFloat(this float f, ref uint buffer, ref int bitposition)
		{
			ushort num = HalfUtilities.Pack(f);
			buffer = buffer.Write((ulong)num, ref bitposition, 16);
			return num;
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x0018663B File Offset: 0x0018483B
		public static float ReadHalfFloat(this ulong buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Read(ref bitposition, 16));
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x0018664C File Offset: 0x0018484C
		[Obsolete("Use Read instead.")]
		public static float ExtractHalfFloat(this ulong buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Extract(ref bitposition, 16));
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x0018665D File Offset: 0x0018485D
		public static float ReadHalfFloat(this uint buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Read(ref bitposition, 16));
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x0018666E File Offset: 0x0018486E
		[Obsolete("Use Read instead.")]
		public static float ExtractHalfFloat(this uint buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Extract(ref bitposition, 16));
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x0018667F File Offset: 0x0018487F
		[Obsolete("Argument order changed")]
		public static void Inject(this ulong value, ref uint buffer, int bits, ref int bitposition)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x0018668A File Offset: 0x0018488A
		[Obsolete("Argument order changed")]
		public static void Inject(this ulong value, ref ulong buffer, int bits, ref int bitposition)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x040052C9 RID: 21193
		private const string overrunerror = "Write buffer overrun. writepos + bits exceeds target length. Data loss will occur.";
	}
}
