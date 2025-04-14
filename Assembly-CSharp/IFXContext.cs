using System;

// Token: 0x020007E4 RID: 2020
public interface IFXContext
{
	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x060031FF RID: 12799
	FXSystemSettings settings { get; }

	// Token: 0x06003200 RID: 12800
	void OnPlayFX();
}
