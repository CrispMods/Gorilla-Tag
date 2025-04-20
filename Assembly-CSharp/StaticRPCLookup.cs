using System;
using System.Collections.Generic;

// Token: 0x020002BA RID: 698
public class StaticRPCLookup
{
	// Token: 0x060010FD RID: 4349 RVA: 0x000AC62C File Offset: 0x000AA82C
	public void Add(NetworkSystem.StaticRPCPlaceholder placeholder, byte code, NetworkSystem.StaticRPC lookupMethod)
	{
		int count = this.entries.Count;
		this.entries.Add(new StaticRPCEntry(placeholder, code, lookupMethod));
		this.eventCodeEntryLookup.Add(code, count);
		this.placeholderEntryLookup.Add(placeholder, count);
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x0003B9B5 File Offset: 0x00039BB5
	public NetworkSystem.StaticRPC CodeToMethod(byte code)
	{
		return this.entries[this.eventCodeEntryLookup[code]].lookupMethod;
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x0003B9D3 File Offset: 0x00039BD3
	public byte PlaceholderToCode(NetworkSystem.StaticRPCPlaceholder placeholder)
	{
		return this.entries[this.placeholderEntryLookup[placeholder]].code;
	}

	// Token: 0x040012FD RID: 4861
	public List<StaticRPCEntry> entries = new List<StaticRPCEntry>();

	// Token: 0x040012FE RID: 4862
	private Dictionary<byte, int> eventCodeEntryLookup = new Dictionary<byte, int>();

	// Token: 0x040012FF RID: 4863
	private Dictionary<NetworkSystem.StaticRPCPlaceholder, int> placeholderEntryLookup = new Dictionary<NetworkSystem.StaticRPCPlaceholder, int>();
}
