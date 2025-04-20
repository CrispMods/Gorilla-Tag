using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class CrittersStickyGoo : CrittersActor
{
	// Token: 0x06000280 RID: 640 RVA: 0x00031EEE File Offset: 0x000300EE
	public override void Initialize()
	{
		base.Initialize();
		this.readyToDisable = false;
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00074154 File Offset: 0x00072354
	public bool CanAffect(Vector3 position)
	{
		return (base.transform.position - position).magnitude < this.range;
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00031EFD File Offset: 0x000300FD
	public void EffectApplied(CrittersPawn critter)
	{
		if (this.destroyOnApply)
		{
			this.readyToDisable = true;
		}
		CrittersManager.instance.TriggerEvent(CrittersManager.CritterEvent.StickyTriggered, this.actorId, critter.transform.position, Quaternion.LookRotation(critter.transform.up));
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00074184 File Offset: 0x00072384
	public override bool ProcessLocal()
	{
		bool result = base.ProcessLocal();
		if (this.readyToDisable)
		{
			base.gameObject.SetActive(false);
			return true;
		}
		return result;
	}

	// Token: 0x04000305 RID: 773
	[Header("Sticky Goo")]
	public float range = 1f;

	// Token: 0x04000306 RID: 774
	public float slowModifier = 0.3f;

	// Token: 0x04000307 RID: 775
	public float slowDuration = 3f;

	// Token: 0x04000308 RID: 776
	public bool destroyOnApply = true;

	// Token: 0x04000309 RID: 777
	private bool readyToDisable;
}
