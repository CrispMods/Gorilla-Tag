using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
[CreateAssetMenu(fileName = "New KeyValuePairSet", menuName = "Data/KeyValuePairSet", order = 0)]
public class KeyValuePairSet : ScriptableObject
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060003A1 RID: 929 RVA: 0x00016371 File Offset: 0x00014571
	public KeyValueStringPair[] Entries
	{
		get
		{
			return this.m_entries;
		}
	}

	// Token: 0x0400041F RID: 1055
	[SerializeField]
	private KeyValueStringPair[] m_entries;
}
