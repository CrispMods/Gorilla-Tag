﻿using System;
using System.Collections.Generic;

// Token: 0x020002AF RID: 687
public class StaticRPCLookup
{
	// Token: 0x060010B4 RID: 4276 RVA: 0x000A9D94 File Offset: 0x000A7F94
	public void Add(NetworkSystem.StaticRPCPlaceholder placeholder, byte code, NetworkSystem.StaticRPC lookupMethod)
	{
		int count = this.entries.Count;
		this.entries.Add(new StaticRPCEntry(placeholder, code, lookupMethod));
		this.eventCodeEntryLookup.Add(code, count);
		this.placeholderEntryLookup.Add(placeholder, count);
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x0003A6F5 File Offset: 0x000388F5
	public NetworkSystem.StaticRPC CodeToMethod(byte code)
	{
		return this.entries[this.eventCodeEntryLookup[code]].lookupMethod;
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x0003A713 File Offset: 0x00038913
	public byte PlaceholderToCode(NetworkSystem.StaticRPCPlaceholder placeholder)
	{
		return this.entries[this.placeholderEntryLookup[placeholder]].code;
	}

	// Token: 0x040012B6 RID: 4790
	public List<StaticRPCEntry> entries = new List<StaticRPCEntry>();

	// Token: 0x040012B7 RID: 4791
	private Dictionary<byte, int> eventCodeEntryLookup = new Dictionary<byte, int>();

	// Token: 0x040012B8 RID: 4792
	private Dictionary<NetworkSystem.StaticRPCPlaceholder, int> placeholderEntryLookup = new Dictionary<NetworkSystem.StaticRPCPlaceholder, int>();
}
