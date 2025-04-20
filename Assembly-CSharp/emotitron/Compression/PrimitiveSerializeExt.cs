using System;
using emotitron.Compression.HalfFloat;
using emotitron.Compression.Utilities;

namespace emotitron.Compression
{
	// Token: 0x02000C9C RID: 3228
	public static class PrimitiveSerializeExt
	{
		// Token: 0x0600511F RID: 20767 RVA: 0x00064B33 File Offset: 0x00062D33
		public static void Inject(this ByteConverter value, ref ulong buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x00064B43 File Offset: 0x00062D43
		public static void Inject(this ByteConverter value, ref uint buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x00064B53 File Offset: 0x00062D53
		public static void Inject(this ByteConverter value, ref ushort buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x00064B63 File Offset: 0x00062D63
		public static void Inject(this ByteConverter value, ref byte buffer, ref int bitposition, int bits)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x001BD94C File Offset: 0x001BBB4C
		public static ulong WriteSigned(this ulong buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x00064B73 File Offset: 0x00062D73
		public static void InjectSigned(this long value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x00064B86 File Offset: 0x00062D86
		public static void InjectSigned(this int value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x00064B86 File Offset: 0x00062D86
		public static void InjectSigned(this short value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x00064B86 File Offset: 0x00062D86
		public static void InjectSigned(this sbyte value, ref ulong buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x001BD96C File Offset: 0x001BBB6C
		public static int ReadSigned(this ulong buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x001BD990 File Offset: 0x001BBB90
		public static uint WriteSigned(this uint buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x00064B98 File Offset: 0x00062D98
		public static void InjectSigned(this long value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x00064BAB File Offset: 0x00062DAB
		public static void InjectSigned(this int value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x00064BAB File Offset: 0x00062DAB
		public static void InjectSigned(this short value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x00064BAB File Offset: 0x00062DAB
		public static void InjectSigned(this sbyte value, ref uint buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x001BD9B0 File Offset: 0x001BBBB0
		public static int ReadSigned(this uint buffer, ref int bitposition, int bits)
		{
			uint num = buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x001BD9D4 File Offset: 0x001BBBD4
		public static ushort WriteSigned(this ushort buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x00064BBD File Offset: 0x00062DBD
		public static void InjectSigned(this long value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x00064BD0 File Offset: 0x00062DD0
		public static void InjectSigned(this int value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x00064BD0 File Offset: 0x00062DD0
		public static void InjectSigned(this short value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x00064BD0 File Offset: 0x00062DD0
		public static void InjectSigned(this sbyte value, ref ushort buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x001BD9F4 File Offset: 0x001BBBF4
		public static int ReadSigned(this ushort buffer, ref int bitposition, int bits)
		{
			uint num = buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x001BDA18 File Offset: 0x001BBC18
		public static byte WriteSigned(this byte buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x00064BE2 File Offset: 0x00062DE2
		public static void InjectSigned(this long value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x00064BF5 File Offset: 0x00062DF5
		public static void InjectSigned(this int value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)(value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x00064BF5 File Offset: 0x00062DF5
		public static void InjectSigned(this short value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x00064BF5 File Offset: 0x00062DF5
		public static void InjectSigned(this sbyte value, ref byte buffer, ref int bitposition, int bits)
		{
			((uint)((int)value << 1 ^ value >> 31)).Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x001BDA38 File Offset: 0x001BBC38
		public static int ReadSigned(this byte buffer, ref int bitposition, int bits)
		{
			uint num = buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x00064C07 File Offset: 0x00062E07
		public static ulong WritetBool(this ulong buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x00064C19 File Offset: 0x00062E19
		public static uint WritetBool(this uint buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x00064C2B File Offset: 0x00062E2B
		public static ushort WritetBool(this ushort buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x0600513E RID: 20798 RVA: 0x00064C3D File Offset: 0x00062E3D
		public static byte WritetBool(this byte buffer, bool value, ref int bitposition)
		{
			return buffer.Write((ulong)(value ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x00064C4F File Offset: 0x00062E4F
		public static void Inject(this bool value, ref ulong buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x00064C61 File Offset: 0x00062E61
		public static void Inject(this bool value, ref uint buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x00064C73 File Offset: 0x00062E73
		public static void Inject(this bool value, ref ushort buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x00064C85 File Offset: 0x00062E85
		public static void Inject(this bool value, ref byte buffer, ref int bitposition)
		{
			((ulong)(value ? 1L : 0L)).Inject(ref buffer, ref bitposition, 1);
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x00064C97 File Offset: 0x00062E97
		public static bool ReadBool(this ulong buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0UL;
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x00064CA6 File Offset: 0x00062EA6
		public static bool ReadtBool(this uint buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0U;
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x00064CB5 File Offset: 0x00062EB5
		public static bool ReadBool(this ushort buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0U;
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x00064CC4 File Offset: 0x00062EC4
		public static bool ReadBool(this byte buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) != 0U;
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x001BDA5C File Offset: 0x001BBC5C
		public static ulong Write(this ulong buffer, ulong value, ref int bitposition, int bits = 64)
		{
			ulong num = value << bitposition;
			ulong num2 = ulong.MaxValue >> 64 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x001BDA98 File Offset: 0x001BBC98
		public static uint Write(this uint buffer, ulong value, ref int bitposition, int bits = 64)
		{
			uint num = (uint)value << bitposition;
			uint num2 = uint.MaxValue >> 32 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
			bitposition += bits;
			return buffer;
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x001BDAD4 File Offset: 0x001BBCD4
		public static ushort Write(this ushort buffer, ulong value, ref int bitposition, int bits = 64)
		{
			uint num = (uint)value << bitposition;
			uint num2 = 65535U >> 16 - bits << bitposition;
			buffer = (ushort)(((uint)buffer & ~num2) | (num2 & num));
			bitposition += bits;
			return buffer;
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x001BDB10 File Offset: 0x001BBD10
		public static byte Write(this byte buffer, ulong value, ref int bitposition, int bits = 64)
		{
			uint num = (uint)value << bitposition;
			uint num2 = 255U >> 8 - bits << bitposition;
			buffer = (byte)(((uint)buffer & ~num2) | (num2 & num));
			bitposition += bits;
			return buffer;
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x00064CD3 File Offset: 0x00062ED3
		public static void Inject(this ulong value, ref ulong buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x001BDB4C File Offset: 0x001BBD4C
		public static void Inject(this ulong value, ref ulong buffer, int bitposition, int bits = 64)
		{
			ulong num = value << bitposition;
			ulong num2 = ulong.MaxValue >> 64 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x00064CE1 File Offset: 0x00062EE1
		public static void Inject(this uint value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x001BDB80 File Offset: 0x001BBD80
		public static void Inject(this uint value, ref ulong buffer, int bitposition, int bits = 32)
		{
			ulong num = (ulong)value << bitposition;
			ulong num2 = ulong.MaxValue >> 64 - bits << bitposition;
			buffer &= ~num2;
			buffer |= (num2 & num);
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x00064CE1 File Offset: 0x00062EE1
		public static void Inject(this ushort value, ref ulong buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x00064CF0 File Offset: 0x00062EF0
		public static void Inject(this ushort value, ref ulong buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x00064CE1 File Offset: 0x00062EE1
		public static void Inject(this byte value, ref ulong buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x00064CF0 File Offset: 0x00062EF0
		public static void Inject(this byte value, ref ulong buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x00064CD3 File Offset: 0x00062ED3
		public static void InjectUnsigned(this long value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x00064D00 File Offset: 0x00062F00
		public static void InjectUnsigned(this int value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x00064D00 File Offset: 0x00062F00
		public static void InjectUnsigned(this short value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x00064D00 File Offset: 0x00062F00
		public static void InjectUnsigned(this sbyte value, ref ulong buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x00064D0F File Offset: 0x00062F0F
		public static void Inject(this ulong value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x00064D1D File Offset: 0x00062F1D
		public static void Inject(this ulong value, ref uint buffer, int bitposition, int bits = 64)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x00064D2C File Offset: 0x00062F2C
		public static void Inject(this uint value, ref uint buffer, ref int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x00064D3B File Offset: 0x00062F3B
		public static void Inject(this uint value, ref uint buffer, int bitposition, int bits = 32)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x00064D2C File Offset: 0x00062F2C
		public static void Inject(this ushort value, ref uint buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x00064D3B File Offset: 0x00062F3B
		public static void Inject(this ushort value, ref uint buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x00064D2C File Offset: 0x00062F2C
		public static void Inject(this byte value, ref uint buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x00064D3B File Offset: 0x00062F3B
		public static void Inject(this byte value, ref uint buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x00064D0F File Offset: 0x00062F0F
		public static void InjectUnsigned(this long value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x00064D4B File Offset: 0x00062F4B
		public static void InjectUnsigned(this int value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x00064D4B File Offset: 0x00062F4B
		public static void InjectUnsigned(this short value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x00064D4B File Offset: 0x00062F4B
		public static void InjectUnsigned(this sbyte value, ref uint buffer, ref int bitposition, int bits = 64)
		{
			buffer = buffer.Write((ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x00064D5A File Offset: 0x00062F5A
		public static void Inject(this ulong value, ref ushort buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x00064D68 File Offset: 0x00062F68
		public static void Inject(this ulong value, ref ushort buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x00064D77 File Offset: 0x00062F77
		public static void Inject(this uint value, ref ushort buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x00064D86 File Offset: 0x00062F86
		public static void Inject(this uint value, ref ushort buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x00064D77 File Offset: 0x00062F77
		public static void Inject(this ushort value, ref ushort buffer, ref int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x00064D86 File Offset: 0x00062F86
		public static void Inject(this ushort value, ref ushort buffer, int bitposition, int bits = 16)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x00064D77 File Offset: 0x00062F77
		public static void Inject(this byte value, ref ushort buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x00064D86 File Offset: 0x00062F86
		public static void Inject(this byte value, ref ushort buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x00064D96 File Offset: 0x00062F96
		public static void Inject(this ulong value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x00064DA4 File Offset: 0x00062FA4
		public static void Inject(this ulong value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write(value, ref bitposition, bits);
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x00064DB3 File Offset: 0x00062FB3
		public static void Inject(this uint value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x00064DC2 File Offset: 0x00062FC2
		public static void Inject(this uint value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x00064DB3 File Offset: 0x00062FB3
		public static void Inject(this ushort value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x00064DC2 File Offset: 0x00062FC2
		public static void Inject(this ushort value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x00064DB3 File Offset: 0x00062FB3
		public static void Inject(this byte value, ref byte buffer, ref int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x00064DC2 File Offset: 0x00062FC2
		public static void Inject(this byte value, ref byte buffer, int bitposition, int bits = 8)
		{
			buffer = buffer.Write((ulong)value, ref bitposition, bits);
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x00064DD2 File Offset: 0x00062FD2
		[Obsolete("Argument order changed")]
		public static ulong Extract(this ulong value, int bits, ref int bitposition)
		{
			return value.Extract(bits, ref bitposition);
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x001BDBB4 File Offset: 0x001BBDB4
		public static ulong Read(this ulong value, ref int bitposition, int bits)
		{
			ulong num = ulong.MaxValue >> 64 - bits;
			ulong result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x001BDBB4 File Offset: 0x001BBDB4
		[Obsolete("Use Read instead.")]
		public static ulong Extract(this ulong value, ref int bitposition, int bits)
		{
			ulong num = ulong.MaxValue >> 64 - bits;
			ulong result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x001BDBDC File Offset: 0x001BBDDC
		[Obsolete("Always include the [ref int bitposition] argument. Extracting from position 0 would be better handled with a mask operation.")]
		public static ulong Extract(this ulong value, int bits)
		{
			ulong num = ulong.MaxValue >> 64 - bits;
			return value & num;
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x001BDBF8 File Offset: 0x001BBDF8
		public static uint Read(this uint value, ref int bitposition, int bits)
		{
			uint num = uint.MaxValue >> 32 - bits;
			uint result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x001BDBF8 File Offset: 0x001BBDF8
		[Obsolete("Use Read instead.")]
		public static uint Extract(this uint value, ref int bitposition, int bits)
		{
			uint num = uint.MaxValue >> 32 - bits;
			uint result = value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x001BDC20 File Offset: 0x001BBE20
		[Obsolete("Always include the [ref int bitposition] argument. Extracting from position 0 would be better handled with a mask operation.")]
		public static uint Extract(this uint value, int bits)
		{
			uint num = uint.MaxValue >> 32 - bits;
			return value & num;
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x001BDC3C File Offset: 0x001BBE3C
		public static uint Read(this ushort value, ref int bitposition, int bits)
		{
			uint num = 65535U >> 16 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x001BDC3C File Offset: 0x001BBE3C
		[Obsolete("Use Read instead.")]
		public static uint Extract(this ushort value, ref int bitposition, int bits)
		{
			uint num = 65535U >> 16 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x001BDC68 File Offset: 0x001BBE68
		public static uint Read(this byte value, ref int bitposition, int bits)
		{
			uint num = 255U >> 8 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x001BDC68 File Offset: 0x001BBE68
		[Obsolete("Use Read instead.")]
		public static uint Extract(this byte value, ref int bitposition, int bits)
		{
			uint num = 255U >> 8 - bits;
			uint result = (uint)value >> bitposition & num;
			bitposition += bits;
			return result;
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x001BDC94 File Offset: 0x001BBE94
		[Obsolete("Always include the [ref int bitposition] argument. Extracting from position 0 would be better handled with a mask operation.")]
		public static byte Extract(this byte value, int bits)
		{
			uint num = 255U >> 8 - bits;
			return (byte)((uint)value & num);
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x00064DDC File Offset: 0x00062FDC
		public static void Inject(this float f, ref ulong buffer, ref int bitposition)
		{
			buffer = buffer.Write(f, ref bitposition, 32);
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x00064DF5 File Offset: 0x00062FF5
		public static float ReadFloat(this ulong buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 32);
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x00064E0A File Offset: 0x0006300A
		[Obsolete("Use Read instead.")]
		public static float ExtractFloat(this ulong buffer, ref int bitposition)
		{
			return buffer.Extract(ref bitposition, 32);
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x001BDCB4 File Offset: 0x001BBEB4
		public static ushort InjectAsHalfFloat(this float f, ref ulong buffer, ref int bitposition)
		{
			ushort num = HalfUtilities.Pack(f);
			buffer = buffer.Write((ulong)num, ref bitposition, 16);
			return num;
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x001BDCD8 File Offset: 0x001BBED8
		public static ushort InjectAsHalfFloat(this float f, ref uint buffer, ref int bitposition)
		{
			ushort num = HalfUtilities.Pack(f);
			buffer = buffer.Write((ulong)num, ref bitposition, 16);
			return num;
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x00064E1F File Offset: 0x0006301F
		public static float ReadHalfFloat(this ulong buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Read(ref bitposition, 16));
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x00064E30 File Offset: 0x00063030
		[Obsolete("Use Read instead.")]
		public static float ExtractHalfFloat(this ulong buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Extract(ref bitposition, 16));
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x00064E41 File Offset: 0x00063041
		public static float ReadHalfFloat(this uint buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Read(ref bitposition, 16));
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x00064E52 File Offset: 0x00063052
		[Obsolete("Use Read instead.")]
		public static float ExtractHalfFloat(this uint buffer, ref int bitposition)
		{
			return HalfUtilities.Unpack((ushort)buffer.Extract(ref bitposition, 16));
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x00064E63 File Offset: 0x00063063
		[Obsolete("Argument order changed")]
		public static void Inject(this ulong value, ref uint buffer, int bits, ref int bitposition)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x00064E6E File Offset: 0x0006306E
		[Obsolete("Argument order changed")]
		public static void Inject(this ulong value, ref ulong buffer, int bits, ref int bitposition)
		{
			value.Inject(ref buffer, ref bitposition, bits);
		}

		// Token: 0x040053D5 RID: 21461
		private const string overrunerror = "Write buffer overrun. writepos + bits exceeds target length. Data loss will occur.";
	}
}
