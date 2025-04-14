using System;
using UnityEngine;

// Token: 0x020005C8 RID: 1480
public interface IRangedVariable<T> : IVariable<T>, IVariable
{
	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x060024C4 RID: 9412
	// (set) Token: 0x060024C5 RID: 9413
	T Min { get; set; }

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x060024C6 RID: 9414
	// (set) Token: 0x060024C7 RID: 9415
	T Max { get; set; }

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x060024C8 RID: 9416
	T Range { get; }

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x060024C9 RID: 9417
	AnimationCurve Curve { get; }
}
