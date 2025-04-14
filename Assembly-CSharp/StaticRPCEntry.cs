using System;

// Token: 0x020002AE RID: 686
public class StaticRPCEntry
{
	// Token: 0x060010B0 RID: 4272 RVA: 0x00050E3B File Offset: 0x0004F03B
	public StaticRPCEntry(NetworkSystem.StaticRPCPlaceholder placeholder, byte code, NetworkSystem.StaticRPC lookupMethod)
	{
		this.placeholder = placeholder;
		this.code = code;
		this.lookupMethod = lookupMethod;
	}

	// Token: 0x040012B2 RID: 4786
	public NetworkSystem.StaticRPCPlaceholder placeholder;

	// Token: 0x040012B3 RID: 4787
	public byte code;

	// Token: 0x040012B4 RID: 4788
	public NetworkSystem.StaticRPC lookupMethod;
}
