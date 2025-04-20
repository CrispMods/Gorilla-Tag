using System;

// Token: 0x02000802 RID: 2050
public interface IFXEffectContext<T> where T : IFXEffectContextObject
{
	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x060032C2 RID: 12994
	T effectContext { get; }

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x060032C3 RID: 12995
	FXSystemSettings settings { get; }
}
