using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class CrittersStunBomb : CrittersToolThrowable
{
	// Token: 0x06000291 RID: 657 RVA: 0x000743B0 File Offset: 0x000725B0
	protected override void OnImpact(Vector3 hitPosition, Vector3 hitNormal)
	{
		if (CrittersManager.instance.LocalAuthority())
		{
			Vector3 position = base.transform.position;
			List<CrittersPawn> crittersPawns = CrittersManager.instance.crittersPawns;
			for (int i = 0; i < crittersPawns.Count; i++)
			{
				CrittersPawn crittersPawn = crittersPawns[i];
				if (crittersPawn.isActiveAndEnabled && Vector3.Distance(crittersPawn.transform.position, position) < this.radius)
				{
					crittersPawn.Stunned(this.stunDuration);
				}
			}
			CrittersManager.instance.TriggerEvent(CrittersManager.CritterEvent.StunExplosion, this.actorId, position, Quaternion.LookRotation(hitNormal));
		}
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00031FFC File Offset: 0x000301FC
	protected override void OnImpactCritter(CrittersPawn impactedCritter)
	{
		if (CrittersManager.instance.LocalAuthority())
		{
			impactedCritter.Stunned(this.stunDuration);
		}
	}

	// Token: 0x0400030D RID: 781
	[Header("Stun Bomb")]
	public float radius = 1f;

	// Token: 0x0400030E RID: 782
	public float stunDuration = 5f;
}
