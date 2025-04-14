using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class CosmeticRefRegistry : MonoBehaviour
{
	// Token: 0x06000854 RID: 2132 RVA: 0x0002DBDC File Offset: 0x0002BDDC
	private void Awake()
	{
		foreach (CosmeticRefTarget cosmeticRefTarget in this.builtInRefTargets)
		{
			this.Register(cosmeticRefTarget.id, cosmeticRefTarget.gameObject);
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x0002DC14 File Offset: 0x0002BE14
	public void Register(CosmeticRefID partID, GameObject part)
	{
		this.partsTable[(int)partID] = part;
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0002DC1F File Offset: 0x0002BE1F
	public GameObject Get(CosmeticRefID partID)
	{
		return this.partsTable[(int)partID];
	}

	// Token: 0x040009D8 RID: 2520
	private GameObject[] partsTable = new GameObject[6];

	// Token: 0x040009D9 RID: 2521
	[SerializeField]
	private CosmeticRefTarget[] builtInRefTargets;
}
