using System;

// Token: 0x020002B9 RID: 697
public class StaticRPCEntry
{
	// Token: 0x060010FC RID: 4348 RVA: 0x0003B998 File Offset: 0x00039B98
	public StaticRPCEntry(NetworkSystem.StaticRPCPlaceholder placeholder, byte code, NetworkSystem.StaticRPC lookupMethod)
	{
		this.placeholder = placeholder;
		this.code = code;
		this.lookupMethod = lookupMethod;
	}

	// Token: 0x040012FA RID: 4858
	public NetworkSystem.StaticRPCPlaceholder placeholder;

	// Token: 0x040012FB RID: 4859
	public byte code;

	// Token: 0x040012FC RID: 4860
	public NetworkSystem.StaticRPC lookupMethod;
}
