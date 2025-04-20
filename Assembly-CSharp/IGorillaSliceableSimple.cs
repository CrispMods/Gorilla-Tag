using System;

// Token: 0x020005D5 RID: 1493
public interface IGorillaSliceableSimple
{
	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06002522 RID: 9506
	bool isActiveAndEnabled { get; }

	// Token: 0x06002523 RID: 9507
	void SliceUpdate();

	// Token: 0x06002524 RID: 9508
	void OnEnable();

	// Token: 0x06002525 RID: 9509
	void OnDisable();
}
