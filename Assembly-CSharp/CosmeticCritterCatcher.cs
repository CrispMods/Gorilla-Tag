using System;

// Token: 0x02000509 RID: 1289
public abstract class CosmeticCritterCatcher : CosmeticCritterHoldable
{
	// Token: 0x06001F51 RID: 8017
	public abstract bool TryToCatch(CosmeticCritter critter);

	// Token: 0x06001F52 RID: 8018
	public abstract void Catch(CosmeticCritter critter);

	// Token: 0x06001F53 RID: 8019 RVA: 0x0009E307 File Offset: 0x0009C507
	protected virtual void OnEnable()
	{
		base.TrySetID();
		CosmeticCritterManager.Instance.RegisterComponent(this);
	}

	// Token: 0x06001F54 RID: 8020 RVA: 0x0009E31A File Offset: 0x0009C51A
	protected virtual void OnDisable()
	{
		CosmeticCritterManager.Instance.UnregisterComponent(this);
	}
}
