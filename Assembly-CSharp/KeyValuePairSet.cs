using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
[CreateAssetMenu(fileName = "New KeyValuePairSet", menuName = "Data/KeyValuePairSet", order = 0)]
public class KeyValuePairSet : ScriptableObject
{
	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060003D3 RID: 979 RVA: 0x00032DC7 File Offset: 0x00030FC7
	public KeyValueStringPair[] Entries
	{
		get
		{
			return this.m_entries;
		}
	}

	// Token: 0x04000453 RID: 1107
	[SerializeField]
	private KeyValueStringPair[] m_entries;
}
