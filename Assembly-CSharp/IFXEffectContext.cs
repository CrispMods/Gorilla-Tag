using System;

// Token: 0x020007E8 RID: 2024
public interface IFXEffectContext<T> where T : IFXEffectContextObject
{
	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x0600320C RID: 12812
	T effectContext { get; }

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x0600320D RID: 12813
	FXSystemSettings settings { get; }
}
