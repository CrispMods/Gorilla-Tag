using System;
using UnityEngine;

// Token: 0x020005D6 RID: 1494
public interface IRangedVariable<T> : IVariable<T>, IVariable
{
	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06002526 RID: 9510
	// (set) Token: 0x06002527 RID: 9511
	T Min { get; set; }

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06002528 RID: 9512
	// (set) Token: 0x06002529 RID: 9513
	T Max { get; set; }

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x0600252A RID: 9514
	T Range { get; }

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x0600252B RID: 9515
	AnimationCurve Curve { get; }
}
