using System;

// Token: 0x020005D7 RID: 1495
public interface IVariable<T> : IVariable
{
	// Token: 0x170003CC RID: 972
	// (get) Token: 0x0600252C RID: 9516 RVA: 0x00049333 File Offset: 0x00047533
	// (set) Token: 0x0600252D RID: 9517 RVA: 0x0004933B File Offset: 0x0004753B
	T Value
	{
		get
		{
			return this.Get();
		}
		set
		{
			this.Set(value);
		}
	}

	// Token: 0x0600252E RID: 9518
	T Get();

	// Token: 0x0600252F RID: 9519
	void Set(T value);

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x06002530 RID: 9520 RVA: 0x00049344 File Offset: 0x00047544
	Type IVariable.ValueType
	{
		get
		{
			return typeof(T);
		}
	}
}
