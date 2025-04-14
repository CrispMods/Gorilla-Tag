using System;

// Token: 0x020007E6 RID: 2022
public interface IFXContextParems<T> where T : FXSArgs
{
	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06003202 RID: 12802
	FXSystemSettings settings { get; }

	// Token: 0x06003203 RID: 12803
	void OnPlayFX(T parems);
}
