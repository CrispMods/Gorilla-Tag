using System;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class PlantablePoint : MonoBehaviour
{
	// Token: 0x06000AEC RID: 2796 RVA: 0x0003B3EA File Offset: 0x000395EA
	private void OnTriggerEnter(Collider other)
	{
		if ((this.floorMask & 1 << other.gameObject.layer) != 0)
		{
			this.plantableObject.SetPlanted(true);
		}
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0003B416 File Offset: 0x00039616
	public void OnTriggerExit(Collider other)
	{
		if ((this.floorMask & 1 << other.gameObject.layer) != 0)
		{
			this.plantableObject.SetPlanted(false);
		}
	}

	// Token: 0x04000D52 RID: 3410
	public bool shouldBeSet;

	// Token: 0x04000D53 RID: 3411
	public LayerMask floorMask;

	// Token: 0x04000D54 RID: 3412
	public PlantableObject plantableObject;
}
