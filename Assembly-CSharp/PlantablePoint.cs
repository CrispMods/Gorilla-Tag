using System;
using UnityEngine;

// Token: 0x020001DE RID: 478
public class PlantablePoint : MonoBehaviour
{
	// Token: 0x06000B36 RID: 2870 RVA: 0x00037D2B File Offset: 0x00035F2B
	private void OnTriggerEnter(Collider other)
	{
		if ((this.floorMask & 1 << other.gameObject.layer) != 0)
		{
			this.plantableObject.SetPlanted(true);
		}
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x00037D57 File Offset: 0x00035F57
	public void OnTriggerExit(Collider other)
	{
		if ((this.floorMask & 1 << other.gameObject.layer) != 0)
		{
			this.plantableObject.SetPlanted(false);
		}
	}

	// Token: 0x04000D97 RID: 3479
	public bool shouldBeSet;

	// Token: 0x04000D98 RID: 3480
	public LayerMask floorMask;

	// Token: 0x04000D99 RID: 3481
	public PlantableObject plantableObject;
}
