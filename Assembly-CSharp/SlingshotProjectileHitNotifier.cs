using System;
using GorillaTag.GuidedRefs;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class SlingshotProjectileHitNotifier : BaseGuidedRefTargetMono
{
	// Token: 0x14000040 RID: 64
	// (add) Token: 0x06001547 RID: 5447 RVA: 0x00068328 File Offset: 0x00066528
	// (remove) Token: 0x06001548 RID: 5448 RVA: 0x00068360 File Offset: 0x00066560
	public event SlingshotProjectileHitNotifier.ProjectileHitEvent OnProjectileHit;

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06001549 RID: 5449 RVA: 0x00068398 File Offset: 0x00066598
	// (remove) Token: 0x0600154A RID: 5450 RVA: 0x000683D0 File Offset: 0x000665D0
	public event SlingshotProjectileHitNotifier.PaperPlaneProjectileHitEvent OnPaperPlaneHit;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x0600154B RID: 5451 RVA: 0x00068408 File Offset: 0x00066608
	// (remove) Token: 0x0600154C RID: 5452 RVA: 0x00068440 File Offset: 0x00066640
	public event SlingshotProjectileHitNotifier.ProjectileHitEvent OnProjectileCollisionStay;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x0600154D RID: 5453 RVA: 0x00068478 File Offset: 0x00066678
	// (remove) Token: 0x0600154E RID: 5454 RVA: 0x000684B0 File Offset: 0x000666B0
	public event SlingshotProjectileHitNotifier.ProjectileTriggerEvent OnProjectileTriggerEnter;

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x0600154F RID: 5455 RVA: 0x000684E8 File Offset: 0x000666E8
	// (remove) Token: 0x06001550 RID: 5456 RVA: 0x00068520 File Offset: 0x00066720
	public event SlingshotProjectileHitNotifier.ProjectileTriggerEvent OnProjectileTriggerExit;

	// Token: 0x06001551 RID: 5457 RVA: 0x00068555 File Offset: 0x00066755
	public void InvokeHit(SlingshotProjectile projectile, Collision collision)
	{
		SlingshotProjectileHitNotifier.ProjectileHitEvent onProjectileHit = this.OnProjectileHit;
		if (onProjectileHit == null)
		{
			return;
		}
		onProjectileHit(projectile, collision);
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x00068569 File Offset: 0x00066769
	public void InvokeHit(PaperPlaneProjectile projectile, Collider collider)
	{
		SlingshotProjectileHitNotifier.PaperPlaneProjectileHitEvent onPaperPlaneHit = this.OnPaperPlaneHit;
		if (onPaperPlaneHit == null)
		{
			return;
		}
		onPaperPlaneHit(projectile, collider);
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x0006857D File Offset: 0x0006677D
	public void InvokeCollisionStay(SlingshotProjectile projectile, Collision collision)
	{
		SlingshotProjectileHitNotifier.ProjectileHitEvent onProjectileCollisionStay = this.OnProjectileCollisionStay;
		if (onProjectileCollisionStay == null)
		{
			return;
		}
		onProjectileCollisionStay(projectile, collision);
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x00068591 File Offset: 0x00066791
	public void InvokeTriggerEnter(SlingshotProjectile projectile, Collider collider)
	{
		SlingshotProjectileHitNotifier.ProjectileTriggerEvent onProjectileTriggerEnter = this.OnProjectileTriggerEnter;
		if (onProjectileTriggerEnter == null)
		{
			return;
		}
		onProjectileTriggerEnter(projectile, collider);
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x000685A5 File Offset: 0x000667A5
	public void InvokeTriggerExit(SlingshotProjectile projectile, Collider collider)
	{
		SlingshotProjectileHitNotifier.ProjectileTriggerEvent onProjectileTriggerExit = this.OnProjectileTriggerExit;
		if (onProjectileTriggerExit == null)
		{
			return;
		}
		onProjectileTriggerExit(projectile, collider);
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x000685B9 File Offset: 0x000667B9
	private new void OnDestroy()
	{
		this.OnProjectileHit = null;
		this.OnProjectileCollisionStay = null;
		this.OnProjectileTriggerEnter = null;
		this.OnProjectileTriggerExit = null;
	}

	// Token: 0x0200038C RID: 908
	// (Invoke) Token: 0x06001559 RID: 5465
	public delegate void ProjectileHitEvent(SlingshotProjectile projectile, Collision collision);

	// Token: 0x0200038D RID: 909
	// (Invoke) Token: 0x0600155D RID: 5469
	public delegate void PaperPlaneProjectileHitEvent(PaperPlaneProjectile projectile, Collider collider);

	// Token: 0x0200038E RID: 910
	// (Invoke) Token: 0x06001561 RID: 5473
	public delegate void ProjectileTriggerEvent(SlingshotProjectile projectile, Collider collider);
}
