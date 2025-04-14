using System;

namespace emotitron.CompressionTests
{
	// Token: 0x02000C78 RID: 3192
	public class BasicWriter
	{
		// Token: 0x0600509E RID: 20638 RVA: 0x00188012 File Offset: 0x00186212
		public static void Reset()
		{
			BasicWriter.pos = 0;
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x0018801A File Offset: 0x0018621A
		public static byte[] BasicWrite(byte[] buffer, byte value)
		{
			buffer[BasicWriter.pos] = value;
			BasicWriter.pos++;
			return buffer;
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x00188031 File Offset: 0x00186231
		public static byte BasicRead(byte[] buffer)
		{
			byte result = buffer[BasicWriter.pos];
			BasicWriter.pos++;
			return result;
		}

		// Token: 0x04005318 RID: 21272
		public static int pos;
	}
}
