using System;
using UnityEngine;

// Token: 0x0200024F RID: 591
public class MonkeyeProjectileTarget : MonoBehaviour
{
	// Token: 0x06000DBB RID: 3515 RVA: 0x00046379 File Offset: 0x00044579
	private void Awake()
	{
		this.monkeyeAI = base.GetComponent<MonkeyeAI>();
		this.notifier = base.GetComponentInChildren<SlingshotProjectileHitNotifier>();
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x00046393 File Offset: 0x00044593
	private void OnEnable()
	{
		if (this.notifier != null)
		{
			this.notifier.OnProjectileHit += this.Notifier_OnProjectileHit;
			this.notifier.OnPaperPlaneHit += this.Notifier_OnPaperPlaneHit;
		}
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x000463D1 File Offset: 0x000445D1
	private void OnDisable()
	{
		if (this.notifier != null)
		{
			this.notifier.OnProjectileHit -= this.Notifier_OnProjectileHit;
			this.notifier.OnPaperPlaneHit -= this.Notifier_OnPaperPlaneHit;
		}
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x0004640F File Offset: 0x0004460F
	private void Notifier_OnProjectileHit(SlingshotProjectile projectile, Collision collision)
	{
		this.monkeyeAI.SetSleep();
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x0004640F File Offset: 0x0004460F
	private void Notifier_OnPaperPlaneHit(PaperPlaneProjectile projectile, Collider collider)
	{
		this.monkeyeAI.SetSleep();
	}

	// Token: 0x040010C6 RID: 4294
	private MonkeyeAI monkeyeAI;

	// Token: 0x040010C7 RID: 4295
	private SlingshotProjectileHitNotifier notifier;
}
