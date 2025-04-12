using System;

namespace emotitron.CompressionTests
{
	// Token: 0x02000C7B RID: 3195
	public class BasicWriter
	{
		// Token: 0x060050AA RID: 20650 RVA: 0x0006386E File Offset: 0x00061A6E
		public static void Reset()
		{
			BasicWriter.pos = 0;
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x00063876 File Offset: 0x00061A76
		public static byte[] BasicWrite(byte[] buffer, byte value)
		{
			buffer[BasicWriter.pos] = value;
			BasicWriter.pos++;
			return buffer;
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x0006388D File Offset: 0x00061A8D
		public static byte BasicRead(byte[] buffer)
		{
			byte result = buffer[BasicWriter.pos];
			BasicWriter.pos++;
			return result;
		}

		// Token: 0x0400532A RID: 21290
		public static int pos;
	}
}
