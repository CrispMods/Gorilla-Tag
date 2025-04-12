using System;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class PositionVolumeModifier : MonoBehaviour
{
	// Token: 0x06000AF1 RID: 2801 RVA: 0x00036ADE File Offset: 0x00034CDE
	public void OnTriggerStay(Collider other)
	{
		this.audioToMod.isModified = true;
	}

	// Token: 0x04000D56 RID: 3414
	public TimeOfDayDependentAudio audioToMod;
}
