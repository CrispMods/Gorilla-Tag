using System;

// Token: 0x020007FE RID: 2046
public interface IFXContext
{
	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x060032B5 RID: 12981
	FXSystemSettings settings { get; }

	// Token: 0x060032B6 RID: 12982
	void OnPlayFX();
}
