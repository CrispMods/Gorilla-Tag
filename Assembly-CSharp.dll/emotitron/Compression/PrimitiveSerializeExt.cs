using System;
using emotitron.Compression.HalfFloat;
using emotitron.Compression.Utilities;

namespace emotitron.Compression
{
	// Token: 0x02000C6E RID: 3182
	public static class PrimitiveSerializeExt
	{
		// Token: 0x06004FCB RID: 20427 RVA: 0x0006310E File Offset: 0x0006130E
		public static void Inject(this ByteConverter value, ref ulong buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x0006311E File Offset: 0x0006131E
		public static void Inject(this ByteConverter value, ref uint buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x0006312E File Offset: 0x0006132E
		public static void Inject(this ByteConverter value, ref ushort buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x0006313E File Offset: 0x0006133E
		public static void Inject(this ByteConverter value, ref byte buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x001B5868 File Offset: 0x001B3A68
		public static ulong WriteSigned(this ulong buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x0006314E File Offset: 0x0006134E
		public static void InjectSigned(this long value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x00063161 File Offset: 0x00061361
		public static void InjectSigned(this int value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x00063161 File Offset: 0x00061361
		public static void InjectSigned(this short value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x00063161 File Offset: 0x00061361
		public static void InjectSigned(this sbyte value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x001B5888 File Offset: 0x001B3A88
		public static int ReadSigned(this ulong buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x001B58AC File Offset: 0x001B3AAC
		public static uint WriteSigned(this uint buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x00063173 File Offset: 0x00061373
		public static void InjectSigned(this long value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x00063186 File Offset: 0x00061386
		public static void InjectSigned(this int value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x00063186 File Offset: 0x00061386
		public static void InjectSigned(this short value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x00063186 File Offset: 0x00061386
		public static void InjectSigned(this sbyte value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x001B58CC File Offset: 0x001B3ACC
		public static int ReadSigned(this uint buffer, ref int bitposition, int bits)
		{
			uint num = buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x001B58F0 File Offset: 0x001B3AF0
		public static ushort WriteSigned(this ushort buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x00063198 File Offset: 0x00061398
		public static void InjectSigned(this long value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x000631AB File Offset: 0x000613AB
		public static void InjectSigned(this int value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x000631AB File Offset: 0x000613AB
		public static void InjectSigned(this short value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x000631AB File Offset: 0x000613AB
		public static void InjectSigned(this sbyte value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x001B5910 File Offset: 0x001B3B10
		public static int ReadSigned(this ushort buffer, ref int bitposition, int bits)
		{
			uint num = buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x001B5934 File Offset: 0x001B3B34
		public static byte WriteSigned(this byte buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x000631BD File Offset: 0x000613BD
		public static void InjectSigned(this long value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x000631D0 File Offset: 0x000613D0
		public static void InjectSigned(this int value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x000631D0 File Offset: 0x000613D0
		public static void InjectSigned(this short value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x000631D0 File Offset: 0x000613D0
		public static void InjectSigned(this sbyte value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06004FE6 RID: 20454 RVA: 0x001B5954 File Offset: 0x001B3B54
		public static int ReadSigned(this byte buffer, ref int bitposition, int bits)
		{
			uint num = buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x000631E2 File Offset: 0x000613E2
		public static ulong WritetBool(this ulong buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x000631F4 File Offset: 0x000613F4
		public static uint WritetBool(this uint buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x00063206 File Offset: 0x00061406
		public static ushort WritetBool(this ushort buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x00063218 File Offset: 0x00061418
		public static byte WritetBool(this byte buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x0006322A File Offset: 0x0006142A
		public static void Inject(this bool value, ref ulong buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x0006323C File Offset: 0x0006143C
		public static void Inject(this bool value, ref uint buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x0006324E File Offset: 0x0006144E
		public static void Inject(this bool value, ref ushort buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x00063260 File Offset: 0x00061460
		public static void Inject(this bool value, ref byte buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x00063272 File Offset: 0x00061472
		public static bool ReadBool(this ulong buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0UL;
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x00063281 File Offset: 0x00061481
		public static bool ReadtBool(this uint buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0U;
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x00063290 File Offset: 0x00061490
		public static bool ReadBool(this ushort buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0U;
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x0006329F File Offset: 0x0006149F
		public static bool ReadBool(this byte buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0U;
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x001B5978 File Offset: 0x001B3B78
		public static ulong Write(this ulong buffer, ulong value, ref int bitposition, int bits = 64)
		{
			ulong num = value << bitposition;
			ulong num2 = ulong.MaxValue >> 64 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x001B59B4 File Offset: 0x001B3BB4
		public static uint Write(this uint buffer, ulong value, ref int bitposition, int bits = 64)
		{
			uint num = (uint)value << bitposition;
			uint num2 = uint.MaxValue >> 32 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x001B59F0 File Offset: 0x001B3BF0
		public static ushort Write(this ushort buffer, ulong value, ref int bitposition, int bits = 64)
		{
			uint num = (uint)value << bitposition;
			uint num2 = 65535U >> 16 - bits << bitposition;
			buffer = (ushort)(((uint)buffer & ~num2) | (num2 & num));
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x001B5A2C File Offset: 0x001B3C2C
		public static byte Write(this byte buffer, ulong value, ref int bitposition, int bits = 64)
		{
			uint num = (uint)value << bitposition;
			uint num2 = 255U >> 8 - bits << bitposition;
			buffer = (byte)(((uint)buffer & ~num2) | (num2 & num));
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x000632AE File Offset: 0x000614AE
		public static void Inject(this ulong value, ref ulong buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x001B5A68 File Offset: 0x001B3C68
		public static void Inject(this ulong value, ref ulong buffer, int bitposition, int bits = 64)
		{
			ulong num = value << bitposition;
			ulong num2 = ulong.MaxValue >> 64 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x000632BC File Offset: 0x000614BC
		public static void Inject(this uint value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x001B5A9C File Offset: 0x001B3C9C
		public static void Inject(this uint value, ref ulong buffer, int bitposition, int bits = 32)
		{
			ulong num = (ulong)value << bitposition;
			ulong num2 = ulong.MaxValue >> 64 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x000632BC File Offset: 0x000614BC
		public static void Inject(this ushort value, ref ulong buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x000632CB File Offset: 0x000614CB
		public static void Inject(this ushort value, ref ulong buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFD RID: 20477 RVA: 0x000632BC File Offset: 0x000614BC
		public static void Inject(this byte value, ref ulong buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x000632CB File Offset: 0x000614CB
		public static void Inject(this byte value, ref ulong buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x000632AE File Offset: 0x000614AE
		public static void InjectUnsigned(this long value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x000632DB File Offset: 0x000614DB
		public static void InjectUnsigned(this int value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x000632DB File Offset: 0x000614DB
		public static void InjectUnsigned(this short value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x000632DB File Offset: 0x000614DB
		public static void InjectUnsigned(this sbyte value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x000632EA File Offset: 0x000614EA
		public static void Inject(this ulong value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x000632F8 File Offset: 0x000614F8
		public static void Inject(this ulong value, ref uint buffer, int bitposition, int bits = 64)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005005 RID: 20485 RVA: 0x00063307 File Offset: 0x00061507
		public static void Inject(this uint value, ref uint buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005006 RID: 20486 RVA: 0x00063316 File Offset: 0x00061516
		public static void Inject(this uint value, ref uint buffer, int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x00063307 File Offset: 0x00061507
		public static void Inject(this ushort value, ref uint buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x00063316 File Offset: 0x00061516
		public static void Inject(this ushort value, ref uint buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x00063307 File Offset: 0x00061507
		public static void Inject(this byte value, ref uint buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x00063316 File Offset: 0x00061516
		public static void Inject(this byte value, ref uint buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x000632EA File Offset: 0x000614EA
		public static void InjectUnsigned(this long value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x00063326 File Offset: 0x00061526
		public static void InjectUnsigned(this int value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x00063326 File Offset: 0x00061526
		public static void InjectUnsigned(this short value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x00063326 File Offset: 0x00061526
		public static void InjectUnsigned(this sbyte value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x00063335 File Offset: 0x00061535
		public static void Inject(this ulong value, ref ushort buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x00063343 File Offset: 0x00061543
		public static void Inject(this ulong value, ref ushort buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x00063352 File Offset: 0x00061552
		public static void Inject(this uint value, ref ushort buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x00063361 File Offset: 0x00061561
		public static void Inject(this uint value, ref ushort buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x00063352 File Offset: 0x00061552
		public static void Inject(this ushort value, ref ushort buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x00063361 File Offset: 0x00061561
		public static void Inject(this ushort value, ref ushort buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x00063352 File Offset: 0x00061552
		public static void Inject(this byte value, ref ushort buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x00063361 File Offset: 0x00061561
		public static void Inject(this byte value, ref ushort buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x00063371 File Offset: 0x00061571
		public static void Inject(this ulong value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x0006337F File Offset: 0x0006157F
		public static void Inject(this ulong value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x0006338E File Offset: 0x0006158E
		public static void Inject(this uint value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x0006339D File Offset: 0x0006159D
		public static void Inject(this uint value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x0006338E File Offset: 0x0006158E
		public static void Inject(this ushort value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600501C RID: 20508 RVA: 0x0006339D File Offset: 0x0006159D
		public static void Inject(this ushort value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600501D RID: 20509 RVA: 0x0006338E File Offset: 0x0006158E
		public static void Inject(this byte value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600501E RID: 20510 RVA: 0x0006339D File Offset: 0x0006159D
		public static void Inject(this byte value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x000633AD File Offset: 0x000615AD
		[Obsolete("Argument order changed")]
		public static ulong Extract(this ulong value, int bits, ref int bitposition)
		{
			return value.Extract(bits, ref bitposition);
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x001B5AD0 File Offset: 0x001B3CD0
		public static ulong Read(this ulong value, ref int bitposition, int bits)
		{
			ulong num = ulong.MaxValue >> 64 - bits;
			ulong result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x001B5AD0 File Offset: 0x001B3CD0
		[Obsolete("Use Read instead.")]
		public static ulong Extract(this ulong value, ref int bitposition, int bits)
		{
			ulong num = ulong.MaxValue >> 64 - bits;
			ulong result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x001B5AF8 File Offset: 0x001B3CF8
		[Obsolete("Always include the [ref int bitposition] argument. Extracting from position 0 would be better handled with a mask operation.")]
		public static ulong Extract(this ulong value, int bits)
		{
			ulong num = ulong.MaxValue >> 64 - bits;
			return value & num;
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x001B5B14 File Offset: 0x001B3D14
		public static uint Read(this uint value, ref int bitposition, int bits)
		{
			uint num = uint.MaxValue >> 32 - bits;
			uint result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x001B5B14 File Offset: 0x001B3D14
		[Obsolete("Use Read instead.")]
		public static uint Extract(this uint value, ref int bitposition, int bits)
		{
			uint num = uint.MaxValue >> 32 - bits;
			uint result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x001B5B3C File Offset: 0x001B3D3C
		[Obsolete("Always include the [ref int bitposition] argument. Extracting from position 0 would be better handled with a mask operation.")]
		public static uint Extract(this uint value, int bits)
		{
			uint num = uint.MaxValue >> 32 - bits;
			return value & num;
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x001B5B58 File Offset: 0x001B3D58
		public static uint Read(this ushort value, ref int bitposition, int bits)
		{
			uint num = 65535U >> 16 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x001B5B58 File Offset: 0x001B3D58
		[Obsolete("Use Read instead.")]
		public static uint Extract(this ushort value, ref int bitposition, int bits)
		{
			uint num = 65535U >> 16 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x001B5B84 File Offset: 0x001B3D84
		public static uint Read(this byte value, ref int bitposition, int bits)
		{
			uint num = 255U >> 8 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x001B5B84 File Offset: 0x001B3D84
		[Obsolete("Use Read instead.")]
		public static uint Extract(this byte value, ref int bitposition, int bits)
		{
			uint num = 255U >> 8 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x001B5BB0 File Offset: 0x001B3DB0
		[Obsolete("Always include the [ref int bitposition] argument. Extracting from position 0 would be better handled with a mask operation.")]
		public static byte Extract(this byte value, int bits)
		{
			uint num = 255U >> 8 - bits;
			return (byte)((uint)value & num);
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x000633B7 File Offset: 0x000615B7
		public static void Inject(this float f, ref ulong buffer, ref int bitposition)
		{
			buffer = buffer.Write(f, ref bitposition, 32);
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x000633D0 File Offset: 0x000615D0
		public static float ReadFloat(this ulong buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 32);
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x000633E5 File Offset: 0x000615E5
		[Obsolete("Use Read instead.")]
		public static float ExtractFloat(this ulong buffer, ref int bitposition)
		{
			return buffer.Extract(ref bitposition, 32);
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x001B5BD0 File Offset: 0x001B3DD0
		public static ushort InjectAsHalfFloat(this float f, ref ulong buffer, ref int bitposition)
		{
			ushort num = HalfUtilities.Pack(f);
			buffer = buffer.Write((ulong)num, ref bitposition, 16);
			return num;
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x001B5BF4 File Offset: 0x001B3DF4
		public static ushort InjectAsHalfFloat(this float f, ref uint buffer, ref int bitposition)
		{
			ushort num = HalfUtilities.Pack(f);
			buffer = buffer.Write((ulong)num, ref bitposition, 16);
			return num;
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x000633FA File Offset: 0x000615FA
		public static float ReadHalfFloat(this ulong buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Read(ref bitposition, 16));
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x0006340B File Offset: 0x0006160B
		[Obsolete("Use Read instead.")]
		public static float ExtractHalfFloat(this ulong buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Extract(ref bitposition, 16));
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x0006341C File Offset: 0x0006161C
		public static float ReadHalfFloat(this uint buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Read(ref bitposition, 16));
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x0006342D File Offset: 0x0006162D
		[Obsolete("Use Read instead.")]
		public static float ExtractHalfFloat(this uint buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Extract(ref bitposition, 16));
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x0006343E File Offset: 0x0006163E
		[Obsolete("Argument order changed")]
		public static void Inject(this ulong value, ref uint buffer, int bits, ref int bitposition)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x00063449 File Offset: 0x00061649
		[Obsolete("Argument order changed")]
		public static void Inject(this ulong value, ref ulong buffer, int bits, ref int bitposition)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x040052DB RID: 21211
		private const string overrunerror = "Write buffer overrun. writepos + bits exceeds target length. Data loss will occur.";
	}
}
