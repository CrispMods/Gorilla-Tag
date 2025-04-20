using System;
using UnityEngine;

// Token: 0x0200025A RID: 602
public class MonkeyeProjectileTarget : MonoBehaviour
{
	// Token: 0x06000E04 RID: 3588 RVA: 0x0003A026 File Offset: 0x00038226
	private void Awake()
	{
		this.monkeyeAI = base.GetComponent<MonkeyeAI>();
		this.notifier = base.GetComponentInChildren<SlingshotProjectileHitNotifier>();
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x0003A040 File Offset: 0x00038240
	private void OnEnable()
	{
		if (this.notifier != null)
		{
			this.notifier.OnProjectileHit += this.Notifier_OnProjectileHit;
			this.notifier.OnPaperPlaneHit += this.Notifier_OnPaperPlaneHit;
		}
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x0003A07E File Offset: 0x0003827E
	private void OnDisable()
	{
		if (this.notifier != null)
		{
			this.notifier.OnProjectileHit -= this.Notifier_OnProjectileHit;
			this.notifier.OnPaperPlaneHit -= this.Notifier_OnPaperPlaneHit;
		}
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x0003A0BC File Offset: 0x000382BC
	private void Notifier_OnProjectileHit(SlingshotProjectile projectile, Collision collision)
	{
		this.monkeyeAI.SetSleep();
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x0003A0BC File Offset: 0x000382BC
	private void Notifier_OnPaperPlaneHit(PaperPlaneProjectile projectile, Collider collider)
	{
		this.monkeyeAI.SetSleep();
	}

	// Token: 0x0400110B RID: 4363
	private MonkeyeAI monkeyeAI;

	// Token: 0x0400110C RID: 4364
	private SlingshotProjectileHitNotifier notifier;
}
