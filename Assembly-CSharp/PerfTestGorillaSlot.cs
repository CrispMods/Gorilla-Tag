using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000224 RID: 548
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestGorillaSlot : MonoBehaviour
{
	// Token: 0x06000C9D RID: 3229 RVA: 0x00042BD3 File Offset: 0x00040DD3
	private void Start()
	{
		this.localStartPosition = base.transform.localPosition;
	}

	// Token: 0x04001003 RID: 4099
	public PerfTestGorillaSlot.SlotType slotType;

	// Token: 0x04001004 RID: 4100
	public Vector3 localStartPosition;

	// Token: 0x02000225 RID: 549
	public enum SlotType
	{
		// Token: 0x04001006 RID: 4102
		VR_PLAYER,
		// Token: 0x04001007 RID: 4103
		DUMMY
	}
}
