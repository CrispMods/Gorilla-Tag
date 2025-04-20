using System;
using GorillaTag;
using UnityEngine;

// Token: 0x0200022F RID: 559
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestGorillaSlot : MonoBehaviour
{
	// Token: 0x06000CE8 RID: 3304 RVA: 0x000390DA File Offset: 0x000372DA
	private void Start()
	{
		this.localStartPosition = base.transform.localPosition;
	}

	// Token: 0x04001049 RID: 4169
	public PerfTestGorillaSlot.SlotType slotType;

	// Token: 0x0400104A RID: 4170
	public Vector3 localStartPosition;

	// Token: 0x02000230 RID: 560
	public enum SlotType
	{
		// Token: 0x0400104C RID: 4172
		VR_PLAYER,
		// Token: 0x0400104D RID: 4173
		DUMMY
	}
}
