using System;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class CosmeticRefRegistry : MonoBehaviour
{
	// Token: 0x06000898 RID: 2200 RVA: 0x0008ECD4 File Offset: 0x0008CED4
	private void Awake()
	{
		foreach (CosmeticRefTarget cosmeticRefTarget in this.builtInRefTargets)
		{
			this.Register(cosmeticRefTarget.id, cosmeticRefTarget.gameObject);
		}
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x00036125 File Offset: 0x00034325
	public void Register(CosmeticRefID partID, GameObject part)
	{
		this.partsTable[(int)partID] = part;
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00036130 File Offset: 0x00034330
	public GameObject Get(CosmeticRefID partID)
	{
		return this.partsTable[(int)partID];
	}

	// Token: 0x04000A1B RID: 2587
	private GameObject[] partsTable = new GameObject[6];

	// Token: 0x04000A1C RID: 2588
	[SerializeField]
	private CosmeticRefTarget[] builtInRefTargets;
}
