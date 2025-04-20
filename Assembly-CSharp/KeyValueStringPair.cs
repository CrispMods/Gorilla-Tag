using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
[Serializable]
public struct KeyValueStringPair
{
	// Token: 0x060003D5 RID: 981 RVA: 0x00032DCF File Offset: 0x00030FCF
	public KeyValueStringPair(string key, string value)
	{
		this.Key = key;
		this.Value = value;
	}

	// Token: 0x04000454 RID: 1108
	public string Key;

	// Token: 0x04000455 RID: 1109
	[Multiline]
	public string Value;
}
