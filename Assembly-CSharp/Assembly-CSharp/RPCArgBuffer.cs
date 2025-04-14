using System;
using System.Runtime.InteropServices;

// Token: 0x02000279 RID: 633
public struct RPCArgBuffer<T> where T : struct
{
	// Token: 0x06000EDC RID: 3804 RVA: 0x0004BD36 File Offset: 0x00049F36
	public RPCArgBuffer(T argStruct)
	{
		this.DataLength = Marshal.SizeOf(typeof(T));
		this.Data = new byte[this.DataLength];
		this.Args = argStruct;
	}

	// Token: 0x04001197 RID: 4503
	public T Args;

	// Token: 0x04001198 RID: 4504
	public byte[] Data;

	// Token: 0x04001199 RID: 4505
	public int DataLength;
}
