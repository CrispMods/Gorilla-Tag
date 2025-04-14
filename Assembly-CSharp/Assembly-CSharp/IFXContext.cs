using System;

// Token: 0x020007E7 RID: 2023
public interface IFXContext
{
	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x0600320B RID: 12811
	FXSystemSettings settings { get; }

	// Token: 0x0600320C RID: 12812
	void OnPlayFX();
}
