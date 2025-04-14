using System;
using UnityEngine;

// Token: 0x0200008F RID: 143
[Serializable]
public struct KeyValueStringPair
{
	// Token: 0x060003A3 RID: 931 RVA: 0x00016379 File Offset: 0x00014579
	public KeyValueStringPair(string key, string value)
	{
		this.Key = key;
		this.Value = value;
	}

	// Token: 0x04000420 RID: 1056
	public string Key;

	// Token: 0x04000421 RID: 1057
	[Multiline]
	public string Value;
}
