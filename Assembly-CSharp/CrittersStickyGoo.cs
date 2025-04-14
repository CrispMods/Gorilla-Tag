using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class CrittersStickyGoo : CrittersActor
{
	// Token: 0x06000253 RID: 595 RVA: 0x0000F276 File Offset: 0x0000D476
	public override void Initialize()
	{
		base.Initialize();
		this.readyToDisable = false;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000F288 File Offset: 0x0000D488
	public bool CanAffect(Vector3 position)
	{
		return (base.transform.position - position).magnitude < this.range;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000F2B6 File Offset: 0x0000D4B6
	public void EffectApplied(CrittersPawn critter)
	{
		if (this.destroyOnApply)
		{
			this.readyToDisable = true;
		}
		CrittersManager.instance.TriggerEvent(CrittersManager.CritterEvent.StickyTriggered, this.actorId, critter.transform.position, Quaternion.LookRotation(critter.transform.up));
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000F2F8 File Offset: 0x0000D4F8
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

	// Token: 0x040002D6 RID: 726
	[Header("Sticky Goo")]
	public float range = 1f;

	// Token: 0x040002D7 RID: 727
	public float slowModifier = 0.3f;

	// Token: 0x040002D8 RID: 728
	public float slowDuration = 3f;

	// Token: 0x040002D9 RID: 729
	public bool destroyOnApply = true;

	// Token: 0x040002DA RID: 730
	private bool readyToDisable;
}
