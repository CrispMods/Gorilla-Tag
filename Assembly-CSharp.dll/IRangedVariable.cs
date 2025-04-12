using System;
using UnityEngine;

// Token: 0x020005C9 RID: 1481
public interface IRangedVariable<T> : IVariable<T>, IVariable
{
	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x060024CC RID: 9420
	// (set) Token: 0x060024CD RID: 9421
	T Min { get; set; }

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x060024CE RID: 9422
	// (set) Token: 0x060024CF RID: 9423
	T Max { get; set; }

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x060024D0 RID: 9424
	T Range { get; }

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x060024D1 RID: 9425
	AnimationCurve Curve { get; }
}
