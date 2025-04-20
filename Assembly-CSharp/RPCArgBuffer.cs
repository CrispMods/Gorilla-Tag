using System;
using System.Runtime.InteropServices;

// Token: 0x02000284 RID: 644
public struct RPCArgBuffer<T> where T : struct
{
	// Token: 0x06000F25 RID: 3877 RVA: 0x0003A9E7 File Offset: 0x00038BE7
	public RPCArgBuffer(T argStruct)
	{
		this.DataLength = Marshal.SizeOf(typeof(T));
		this.Data = new byte[this.DataLength];
		this.Args = argStruct;
	}

	// Token: 0x040011DC RID: 4572
	public T Args;

	// Token: 0x040011DD RID: 4573
	public byte[] Data;

	// Token: 0x040011DE RID: 4574
	public int DataLength;
}
