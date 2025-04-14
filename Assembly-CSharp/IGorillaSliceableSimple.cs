using System;

// Token: 0x020005C7 RID: 1479
public interface IGorillaSliceableSimple
{
	// Token: 0x170003BF RID: 959
	// (get) Token: 0x060024C0 RID: 9408
	bool isActiveAndEnabled { get; }

	// Token: 0x060024C1 RID: 9409
	void SliceUpdate();

	// Token: 0x060024C2 RID: 9410
	void OnEnable();

	// Token: 0x060024C3 RID: 9411
	void OnDisable();
}
