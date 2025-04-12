using System;

// Token: 0x020002AE RID: 686
public class StaticRPCEntry
{
	// Token: 0x060010B3 RID: 4275 RVA: 0x0003A6D8 File Offset: 0x000388D8
	public StaticRPCEntry(NetworkSystem.StaticRPCPlaceholder placeholder, byte code, NetworkSystem.StaticRPC lookupMethod)
	{
		this.placeholder = placeholder;
		this.code = code;
		this.lookupMethod = lookupMethod;
	}

	// Token: 0x040012B3 RID: 4787
	public NetworkSystem.StaticRPCPlaceholder placeholder;

	// Token: 0x040012B4 RID: 4788
	public byte code;

	// Token: 0x040012B5 RID: 4789
	public NetworkSystem.StaticRPC lookupMethod;
}
