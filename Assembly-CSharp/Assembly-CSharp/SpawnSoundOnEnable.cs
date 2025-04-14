using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class SpawnSoundOnEnable : MonoBehaviour
{
	// Token: 0x060002F9 RID: 761 RVA: 0x00012934 File Offset: 0x00010B34
	private void OnEnable()
	{
		if (CrittersManager.instance == null || !CrittersManager.instance.LocalAuthority())
		{
			return;
		}
		if (!this.triggerOnFirstEnable && !this.firstEnabledOccured)
		{
			this.firstEnabledOccured = true;
			return;
		}
		CrittersLoudNoise crittersLoudNoise = (CrittersLoudNoise)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.LoudNoise, this.soundSubIndex);
		if (crittersLoudNoise == null)
		{
			return;
		}
		crittersLoudNoise.MoveActor(base.transform.position, base.transform.rotation, false, true, true);
		crittersLoudNoise.SetImpulseVelocity(Vector3.zero, Vector3.zero);
	}

	// Token: 0x0400039A RID: 922
	public int soundSubIndex = 3;

	// Token: 0x0400039B RID: 923
	public bool triggerOnFirstEnable;

	// Token: 0x0400039C RID: 924
	private bool firstEnabledOccured;
}
