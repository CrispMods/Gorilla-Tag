using System;

// Token: 0x02000800 RID: 2048
public interface IFXContextParems<T> where T : FXSArgs
{
	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x060032B8 RID: 12984
	FXSystemSettings settings { get; }

	// Token: 0x060032B9 RID: 12985
	void OnPlayFX(T parems);
}
