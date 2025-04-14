using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class CrittersStickyGoo : CrittersActor
{
	// Token: 0x06000255 RID: 597 RVA: 0x0000F61B File Offset: 0x0000D81B
	public override void Initialize()
	{
		base.Initialize();
		this.readyToDisable = false;
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000F62C File Offset: 0x0000D82C
	public bool CanAffect(Vector3 position)
	{
		return (base.transform.position - position).magnitude < this.range;
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000F65A File Offset: 0x0000D85A
	public void EffectApplied(CrittersPawn critter)
	{
		if (this.destroyOnApply)
		{
			this.readyToDisable = true;
		}
		CrittersManager.instance.TriggerEvent(CrittersManager.CritterEvent.StickyTriggered, this.actorId, critter.transform.position, Quaternion.LookRotation(critter.transform.up));
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000F69C File Offset: 0x0000D89C
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

	// Token: 0x040002D7 RID: 727
	[Header("Sticky Goo")]
	public float range = 1f;

	// Token: 0x040002D8 RID: 728
	public float slowModifier = 0.3f;

	// Token: 0x040002D9 RID: 729
	public float slowDuration = 3f;

	// Token: 0x040002DA RID: 730
	public bool destroyOnApply = true;

	// Token: 0x040002DB RID: 731
	private bool readyToDisable;
}
