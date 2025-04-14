using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000224 RID: 548
[GTStripGameObjectFromBuild("!GT_AUTOMATED_PERF_TEST")]
public class PerfTestGorillaSlot : MonoBehaviour
{
	// Token: 0x06000C9F RID: 3231 RVA: 0x00042F17 File Offset: 0x00041117
	private void Start()
	{
		this.localStartPosition = base.transform.localPosition;
	}

	// Token: 0x04001004 RID: 4100
	public PerfTestGorillaSlot.SlotType slotType;

	// Token: 0x04001005 RID: 4101
	public Vector3 localStartPosition;

	// Token: 0x02000225 RID: 549
	public enum SlotType
	{
		// Token: 0x04001007 RID: 4103
		VR_PLAYER,
		// Token: 0x04001008 RID: 4104
		DUMMY
	}
}
