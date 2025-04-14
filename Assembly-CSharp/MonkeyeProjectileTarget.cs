using System;
using UnityEngine;

// Token: 0x0200024F RID: 591
public class MonkeyeProjectileTarget : MonoBehaviour
{
	// Token: 0x06000DB9 RID: 3513 RVA: 0x00046035 File Offset: 0x00044235
	private void Awake()
	{
		this.monkeyeAI = base.GetComponent<MonkeyeAI>();
		this.notifier = base.GetComponentInChildren<SlingshotProjectileHitNotifier>();
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0004604F File Offset: 0x0004424F
	private void OnEnable()
	{
		if (this.notifier != null)
		{
			this.notifier.OnProjectileHit += this.Notifier_OnProjectileHit;
			this.notifier.OnPaperPlaneHit += this.Notifier_OnPaperPlaneHit;
		}
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x0004608D File Offset: 0x0004428D
	private void OnDisable()
	{
		if (this.notifier != null)
		{
			this.notifier.OnProjectileHit -= this.Notifier_OnProjectileHit;
			this.notifier.OnPaperPlaneHit -= this.Notifier_OnPaperPlaneHit;
		}
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x000460CB File Offset: 0x000442CB
	private void Notifier_OnProjectileHit(SlingshotProjectile projectile, Collision collision)
	{
		this.monkeyeAI.SetSleep();
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x000460CB File Offset: 0x000442CB
	private void Notifier_OnPaperPlaneHit(PaperPlaneProjectile projectile, Collider collider)
	{
		this.monkeyeAI.SetSleep();
	}

	// Token: 0x040010C5 RID: 4293
	private MonkeyeAI monkeyeAI;

	// Token: 0x040010C6 RID: 4294
	private SlingshotProjectileHitNotifier notifier;
}
