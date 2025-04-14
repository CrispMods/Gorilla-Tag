using System;
using GorillaTag.GuidedRefs;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class SlingshotProjectileHitNotifier : BaseGuidedRefTargetMono
{
	// Token: 0x14000040 RID: 64
	// (add) Token: 0x0600154A RID: 5450 RVA: 0x000686AC File Offset: 0x000668AC
	// (remove) Token: 0x0600154B RID: 5451 RVA: 0x000686E4 File Offset: 0x000668E4
	public event SlingshotProjectileHitNotifier.ProjectileHitEvent OnProjectileHit;

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x0600154C RID: 5452 RVA: 0x0006871C File Offset: 0x0006691C
	// (remove) Token: 0x0600154D RID: 5453 RVA: 0x00068754 File Offset: 0x00066954
	public event SlingshotProjectileHitNotifier.PaperPlaneProjectileHitEvent OnPaperPlaneHit;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x0600154E RID: 5454 RVA: 0x0006878C File Offset: 0x0006698C
	// (remove) Token: 0x0600154F RID: 5455 RVA: 0x000687C4 File Offset: 0x000669C4
	public event SlingshotProjectileHitNotifier.ProjectileHitEvent OnProjectileCollisionStay;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x06001550 RID: 5456 RVA: 0x000687FC File Offset: 0x000669FC
	// (remove) Token: 0x06001551 RID: 5457 RVA: 0x00068834 File Offset: 0x00066A34
	public event SlingshotProjectileHitNotifier.ProjectileTriggerEvent OnProjectileTriggerEnter;

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x06001552 RID: 5458 RVA: 0x0006886C File Offset: 0x00066A6C
	// (remove) Token: 0x06001553 RID: 5459 RVA: 0x000688A4 File Offset: 0x00066AA4
	public event SlingshotProjectileHitNotifier.ProjectileTriggerEvent OnProjectileTriggerExit;

	// Token: 0x06001554 RID: 5460 RVA: 0x000688D9 File Offset: 0x00066AD9
	public void InvokeHit(SlingshotProjectile projectile, Collision collision)
	{
		SlingshotProjectileHitNotifier.ProjectileHitEvent onProjectileHit = this.OnProjectileHit;
		if (onProjectileHit == null)
		{
			return;
		}
		onProjectileHit(projectile, collision);
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x000688ED File Offset: 0x00066AED
	public void InvokeHit(PaperPlaneProjectile projectile, Collider collider)
	{
		SlingshotProjectileHitNotifier.PaperPlaneProjectileHitEvent onPaperPlaneHit = this.OnPaperPlaneHit;
		if (onPaperPlaneHit == null)
		{
			return;
		}
		onPaperPlaneHit(projectile, collider);
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x00068901 File Offset: 0x00066B01
	public void InvokeCollisionStay(SlingshotProjectile projectile, Collision collision)
	{
		SlingshotProjectileHitNotifier.ProjectileHitEvent onProjectileCollisionStay = this.OnProjectileCollisionStay;
		if (onProjectileCollisionStay == null)
		{
			return;
		}
		onProjectileCollisionStay(projectile, collision);
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x00068915 File Offset: 0x00066B15
	public void InvokeTriggerEnter(SlingshotProjectile projectile, Collider collider)
	{
		SlingshotProjectileHitNotifier.ProjectileTriggerEvent onProjectileTriggerEnter = this.OnProjectileTriggerEnter;
		if (onProjectileTriggerEnter == null)
		{
			return;
		}
		onProjectileTriggerEnter(projectile, collider);
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x00068929 File Offset: 0x00066B29
	public void InvokeTriggerExit(SlingshotProjectile projectile, Collider collider)
	{
		SlingshotProjectileHitNotifier.ProjectileTriggerEvent onProjectileTriggerExit = this.OnProjectileTriggerExit;
		if (onProjectileTriggerExit == null)
		{
			return;
		}
		onProjectileTriggerExit(projectile, collider);
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x0006893D File Offset: 0x00066B3D
	private new void OnDestroy()
	{
		this.OnProjectileHit = null;
		this.OnProjectileCollisionStay = null;
		this.OnProjectileTriggerEnter = null;
		this.OnProjectileTriggerExit = null;
	}

	// Token: 0x0200038C RID: 908
	// (Invoke) Token: 0x0600155C RID: 5468
	public delegate void ProjectileHitEvent(SlingshotProjectile projectile, Collision collision);

	// Token: 0x0200038D RID: 909
	// (Invoke) Token: 0x06001560 RID: 5472
	public delegate void PaperPlaneProjectileHitEvent(PaperPlaneProjectile projectile, Collider collider);

	// Token: 0x0200038E RID: 910
	// (Invoke) Token: 0x06001564 RID: 5476
	public delegate void ProjectileTriggerEvent(SlingshotProjectile projectile, Collider collider);
}
