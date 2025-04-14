using System;

// Token: 0x020005C9 RID: 1481
public interface IVariable<T> : IVariable
{
	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x060024CA RID: 9418 RVA: 0x000B6EA9 File Offset: 0x000B50A9
	// (set) Token: 0x060024CB RID: 9419 RVA: 0x000B6EB1 File Offset: 0x000B50B1
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

	// Token: 0x060024CC RID: 9420
	T Get();

	// Token: 0x060024CD RID: 9421
	void Set(T value);

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x060024CE RID: 9422 RVA: 0x000B6EBA File Offset: 0x000B50BA
	Type IVariable.ValueType
	{
		get
		{
			return typeof(T);
		}
	}
}
