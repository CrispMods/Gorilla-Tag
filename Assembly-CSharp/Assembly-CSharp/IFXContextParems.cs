using System;

// Token: 0x020007E9 RID: 2025
public interface IFXContextParems<T> where T : FXSArgs
{
	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x0600320E RID: 12814
	FXSystemSettings settings { get; }

	// Token: 0x0600320F RID: 12815
	void OnPlayFX(T parems);
}
