using System;
using System.Runtime.InteropServices;

// Token: 0x02000279 RID: 633
public struct RPCArgBuffer<T> where T : struct
{
	// Token: 0x06000EDA RID: 3802 RVA: 0x0004B9F2 File Offset: 0x00049BF2
	public RPCArgBuffer(T argStruct)
	{
		this.DataLength = Marshal.SizeOf(typeof(T));
		this.Data = new byte[this.DataLength];
		this.Args = argStruct;
	}

	// Token: 0x04001196 RID: 4502
	public T Args;

	// Token: 0x04001197 RID: 4503
	public byte[] Data;

	// Token: 0x04001198 RID: 4504
	public int DataLength;
}
