using System;
using GorillaTag.GuidedRefs;
using UnityEngine;

// Token: 0x02000396 RID: 918
public class SlingshotProjectileHitNotifier : BaseGuidedRefTargetMono
{
	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06001593 RID: 5523 RVA: 0x000C0A4C File Offset: 0x000BEC4C
	// (remove) Token: 0x06001594 RID: 5524 RVA: 0x000C0A84 File Offset: 0x000BEC84
	public event SlingshotProjectileHitNotifier.ProjectileHitEvent OnProjectileHit;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x06001595 RID: 5525 RVA: 0x000C0ABC File Offset: 0x000BECBC
	// (remove) Token: 0x06001596 RID: 5526 RVA: 0x000C0AF4 File Offset: 0x000BECF4
	public event SlingshotProjectileHitNotifier.PaperPlaneProjectileHitEvent OnPaperPlaneHit;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x06001597 RID: 5527 RVA: 0x000C0B2C File Offset: 0x000BED2C
	// (remove) Token: 0x06001598 RID: 5528 RVA: 0x000C0B64 File Offset: 0x000BED64
	public event SlingshotProjectileHitNotifier.ProjectileHitEvent OnProjectileCollisionStay;

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x06001599 RID: 5529 RVA: 0x000C0B9C File Offset: 0x000BED9C
	// (remove) Token: 0x0600159A RID: 5530 RVA: 0x000C0BD4 File Offset: 0x000BEDD4
	public event SlingshotProjectileHitNotifier.ProjectileTriggerEvent OnProjectileTriggerEnter;

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x0600159B RID: 5531 RVA: 0x000C0C0C File Offset: 0x000BEE0C
	// (remove) Token: 0x0600159C RID: 5532 RVA: 0x000C0C44 File Offset: 0x000BEE44
	public event SlingshotProjectileHitNotifier.ProjectileTriggerEvent OnProjectileTriggerExit;

	// Token: 0x0600159D RID: 5533 RVA: 0x0003E97F File Offset: 0x0003CB7F
	public void InvokeHit(SlingshotProjectile projectile, Collision collision)
	{
		SlingshotProjectileHitNotifier.ProjectileHitEvent onProjectileHit = this.OnProjectileHit;
		if (onProjectileHit == null)
		{
			return;
		}
		onProjectileHit(projectile, collision);
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x0003E993 File Offset: 0x0003CB93
	public void InvokeHit(PaperPlaneProjectile projectile, Collider collider)
	{
		SlingshotProjectileHitNotifier.PaperPlaneProjectileHitEvent onPaperPlaneHit = this.OnPaperPlaneHit;
		if (onPaperPlaneHit == null)
		{
			return;
		}
		onPaperPlaneHit(projectile, collider);
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x0003E9A7 File Offset: 0x0003CBA7
	public void InvokeCollisionStay(SlingshotProjectile projectile, Collision collision)
	{
		SlingshotProjectileHitNotifier.ProjectileHitEvent onProjectileCollisionStay = this.OnProjectileCollisionStay;
		if (onProjectileCollisionStay == null)
		{
			return;
		}
		onProjectileCollisionStay(projectile, collision);
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x0003E9BB File Offset: 0x0003CBBB
	public void InvokeTriggerEnter(SlingshotProjectile projectile, Collider collider)
	{
		SlingshotProjectileHitNotifier.ProjectileTriggerEvent onProjectileTriggerEnter = this.OnProjectileTriggerEnter;
		if (onProjectileTriggerEnter == null)
		{
			return;
		}
		onProjectileTriggerEnter(projectile, collider);
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x0003E9CF File Offset: 0x0003CBCF
	public void InvokeTriggerExit(SlingshotProjectile projectile, Collider collider)
	{
		SlingshotProjectileHitNotifier.ProjectileTriggerEvent onProjectileTriggerExit = this.OnProjectileTriggerExit;
		if (onProjectileTriggerExit == null)
		{
			return;
		}
		onProjectileTriggerExit(projectile, collider);
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x0003E9E3 File Offset: 0x0003CBE3
	private new void OnDestroy()
	{
		this.OnProjectileHit = null;
		this.OnProjectileCollisionStay = null;
		this.OnProjectileTriggerEnter = null;
		this.OnProjectileTriggerExit = null;
	}

	// Token: 0x02000397 RID: 919
	// (Invoke) Token: 0x060015A5 RID: 5541
	public delegate void ProjectileHitEvent(SlingshotProjectile projectile, Collision collision);

	// Token: 0x02000398 RID: 920
	// (Invoke) Token: 0x060015A9 RID: 5545
	public delegate void PaperPlaneProjectileHitEvent(PaperPlaneProjectile projectile, Collider collider);

	// Token: 0x02000399 RID: 921
	// (Invoke) Token: 0x060015AD RID: 5549
	public delegate void ProjectileTriggerEvent(SlingshotProjectile projectile, Collider collider);
}
