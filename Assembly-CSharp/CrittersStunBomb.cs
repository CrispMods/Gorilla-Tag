using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class CrittersStunBomb : CrittersToolThrowable
{
	// Token: 0x06000264 RID: 612 RVA: 0x0000F5DC File Offset: 0x0000D7DC
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

	// Token: 0x06000265 RID: 613 RVA: 0x0000F670 File Offset: 0x0000D870
	protected override void OnImpactCritter(CrittersPawn impactedCritter)
	{
		if (CrittersManager.instance.LocalAuthority())
		{
			impactedCritter.Stunned(this.stunDuration);
		}
	}

	// Token: 0x040002DE RID: 734
	[Header("Stun Bomb")]
	public float radius = 1f;

	// Token: 0x040002DF RID: 735
	public float stunDuration = 5f;
}
