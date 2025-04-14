using System;

// Token: 0x020005C8 RID: 1480
public interface IGorillaSliceableSimple
{
	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x060024C8 RID: 9416
	bool isActiveAndEnabled { get; }

	// Token: 0x060024C9 RID: 9417
	void SliceUpdate();

	// Token: 0x060024CA RID: 9418
	void OnEnable();

	// Token: 0x060024CB RID: 9419
	void OnDisable();
}
