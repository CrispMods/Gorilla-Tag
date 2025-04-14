using System;
using System.Collections.Generic;

// Token: 0x020002AF RID: 687
public class StaticRPCLookup
{
	// Token: 0x060010B1 RID: 4273 RVA: 0x00050E58 File Offset: 0x0004F058
	public void Add(NetworkSystem.StaticRPCPlaceholder placeholder, byte code, NetworkSystem.StaticRPC lookupMethod)
	{
		int count = this.entries.Count;
		this.entries.Add(new StaticRPCEntry(placeholder, code, lookupMethod));
		this.eventCodeEntryLookup.Add(code, count);
		this.placeholderEntryLookup.Add(placeholder, count);
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x00050E9E File Offset: 0x0004F09E
	public NetworkSystem.StaticRPC CodeToMethod(byte code)
	{
		return this.entries[this.eventCodeEntryLookup[code]].lookupMethod;
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x00050EBC File Offset: 0x0004F0BC
	public byte PlaceholderToCode(NetworkSystem.StaticRPCPlaceholder placeholder)
	{
		return this.entries[this.placeholderEntryLookup[placeholder]].code;
	}

	// Token: 0x040012B5 RID: 4789
	public List<StaticRPCEntry> entries = new List<StaticRPCEntry>();

	// Token: 0x040012B6 RID: 4790
	private Dictionary<byte, int> eventCodeEntryLookup = new Dictionary<byte, int>();

	// Token: 0x040012B7 RID: 4791
	private Dictionary<NetworkSystem.StaticRPCPlaceholder, int> placeholderEntryLookup = new Dictionary<NetworkSystem.StaticRPCPlaceholder, int>();
}
