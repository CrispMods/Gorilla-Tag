using System;

namespace emotitron.CompressionTests
{
	// Token: 0x02000CA9 RID: 3241
	public class BasicWriter
	{
		// Token: 0x060051FE RID: 20990 RVA: 0x00065293 File Offset: 0x00063493
		public static void Reset()
		{
			BasicWriter.pos = 0;
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x0006529B File Offset: 0x0006349B
		public static byte[] BasicWrite(byte[] buffer, byte value)
		{
			buffer[BasicWriter.pos] = value;
			BasicWriter.pos++;
			return buffer;
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x000652B2 File Offset: 0x000634B2
		public static byte BasicRead(byte[] buffer)
		{
			byte result = buffer[BasicWriter.pos];
			BasicWriter.pos++;
			return result;
		}

		// Token: 0x04005424 RID: 21540
		public static int pos;
	}
}
