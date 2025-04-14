using System;

// Token: 0x020007EB RID: 2027
public interface IFXEffectContext<T> where T : IFXEffectContextObject
{
	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x06003218 RID: 12824
	T effectContext { get; }

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06003219 RID: 12825
	FXSystemSettings settings { get; }
}
