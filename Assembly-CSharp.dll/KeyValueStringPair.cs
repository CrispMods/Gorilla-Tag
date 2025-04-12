using System;
using UnityEngine;

// Token: 0x0200008F RID: 143
[Serializable]
public struct KeyValueStringPair
{
	// Token: 0x060003A5 RID: 933 RVA: 0x00031C6C File Offset: 0x0002FE6C
	public KeyValueStringPair(string key, string value)
	{
		this.Key = key;
		this.Value = value;
	}

	// Token: 0x04000421 RID: 1057
	public string Key;

	// Token: 0x04000422 RID: 1058
	[Multiline]
	public string Value;
}
