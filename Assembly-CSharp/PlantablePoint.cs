using System;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class PlantablePoint : MonoBehaviour
{
	// Token: 0x06000AEA RID: 2794 RVA: 0x0003B0C6 File Offset: 0x000392C6
	private void OnTriggerEnter(Collider other)
	{
		if ((this.floorMask & 1 << other.gameObject.layer) != 0)
		{
			this.plantableObject.SetPlanted(true);
		}
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x0003B0F2 File Offset: 0x000392F2
	public void OnTriggerExit(Collider other)
	{
		if ((this.floorMask & 1 << other.gameObject.layer) != 0)
		{
			this.plantableObject.SetPlanted(false);
		}
	}

	// Token: 0x04000D51 RID: 3409
	public bool shouldBeSet;

	// Token: 0x04000D52 RID: 3410
	public LayerMask floorMask;

	// Token: 0x04000D53 RID: 3411
	public PlantableObject plantableObject;
}
