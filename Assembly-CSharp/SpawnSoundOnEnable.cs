using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class SpawnSoundOnEnable : MonoBehaviour
{
	// Token: 0x06000328 RID: 808 RVA: 0x00076DA4 File Offset: 0x00074FA4
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

	// Token: 0x040003CD RID: 973
	public int soundSubIndex = 3;

	// Token: 0x040003CE RID: 974
	public bool triggerOnFirstEnable;

	// Token: 0x040003CF RID: 975
	private bool firstEnabledOccured;
}
