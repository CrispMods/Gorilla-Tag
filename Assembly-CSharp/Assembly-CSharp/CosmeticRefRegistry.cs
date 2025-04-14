﻿using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class CosmeticRefRegistry : MonoBehaviour
{
	// Token: 0x06000856 RID: 2134 RVA: 0x0002DF00 File Offset: 0x0002C100
	private void Awake()
	{
		foreach (CosmeticRefTarget cosmeticRefTarget in this.builtInRefTargets)
		{
			this.Register(cosmeticRefTarget.id, cosmeticRefTarget.gameObject);
		}
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0002DF38 File Offset: 0x0002C138
	public void Register(CosmeticRefID partID, GameObject part)
	{
		this.partsTable[(int)partID] = part;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0002DF43 File Offset: 0x0002C143
	public GameObject Get(CosmeticRefID partID)
	{
		return this.partsTable[(int)partID];
	}

	// Token: 0x040009D9 RID: 2521
	private GameObject[] partsTable = new GameObject[6];

	// Token: 0x040009DA RID: 2522
	[SerializeField]
	private CosmeticRefTarget[] builtInRefTargets;
}
