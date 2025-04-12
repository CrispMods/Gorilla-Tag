using System;

// Token: 0x020005CA RID: 1482
public interface IVariable<T> : IVariable
{
	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x060024D2 RID: 9426 RVA: 0x00047F18 File Offset: 0x00046118
	// (set) Token: 0x060024D3 RID: 9427 RVA: 0x00047F20 File Offset: 0x00046120
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

	// Token: 0x060024D4 RID: 9428
	T Get();

	// Token: 0x060024D5 RID: 9429
	void Set(T value);

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x060024D6 RID: 9430 RVA: 0x00047F29 File Offset: 0x00046129
	Type IVariable.ValueType
	{
		get
		{
			return typeof(T);
		}
	}
}
