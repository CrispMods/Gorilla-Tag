using System;

// Token: 0x02000509 RID: 1289
public abstract class CosmeticCritterCatcher : CosmeticCritterHoldable
{
	// Token: 0x06001F54 RID: 8020
	public abstract bool TryToCatch(CosmeticCritter critter);

	// Token: 0x06001F55 RID: 8021
	public abstract void Catch(CosmeticCritter critter);

	// Token: 0x06001F56 RID: 8022 RVA: 0x0009E68B File Offset: 0x0009C88B
	protected virtual void OnEnable()
	{
		base.TrySetID();
		CosmeticCritterManager.Instance.RegisterComponent(this);
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x0009E69E File Offset: 0x0009C89E
	protected virtual void OnDisable()
	{
		CosmeticCritterManager.Instance.UnregisterComponent(this);
	}
}
