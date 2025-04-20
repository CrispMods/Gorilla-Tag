using System;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class PositionVolumeModifier : MonoBehaviour
{
	// Token: 0x06000B3B RID: 2875 RVA: 0x00037D9E File Offset: 0x00035F9E
	public void OnTriggerStay(Collider other)
	{
		this.audioToMod.isModified = true;
	}

	// Token: 0x04000D9B RID: 3483
	public TimeOfDayDependentAudio audioToMod;
}
