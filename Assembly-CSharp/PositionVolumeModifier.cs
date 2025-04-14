using System;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class PositionVolumeModifier : MonoBehaviour
{
	// Token: 0x06000AEF RID: 2799 RVA: 0x0003B139 File Offset: 0x00039339
	public void OnTriggerStay(Collider other)
	{
		this.audioToMod.isModified = true;
	}

	// Token: 0x04000D55 RID: 3413
	public TimeOfDayDependentAudio audioToMod;
}
