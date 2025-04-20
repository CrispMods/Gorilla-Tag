using System;

// Token: 0x02000516 RID: 1302
public abstract class CosmeticCritterCatcher : CosmeticCritterHoldable
{
	// Token: 0x06001FAA RID: 8106
	public abstract bool TryToCatch(CosmeticCritter critter);

	// Token: 0x06001FAB RID: 8107
	public abstract void Catch(CosmeticCritter critter);

	// Token: 0x06001FAC RID: 8108 RVA: 0x000457D2 File Offset: 0x000439D2
	protected virtual void OnEnable()
	{
		base.TrySetID();
		CosmeticCritterManager.Instance.RegisterComponent(this);
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x000457E5 File Offset: 0x000439E5
	protected virtual void OnDisable()
	{
		CosmeticCritterManager.Instance.UnregisterComponent(this);
	}
}
