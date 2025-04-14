using System;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class HoldableHandle : InteractionPoint
{
	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x0600181A RID: 6170 RVA: 0x000756FB File Offset: 0x000738FB
	public new HoldableObject Holdable
	{
		get
		{
			return this.holdable;
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x0600181B RID: 6171 RVA: 0x00075703 File Offset: 0x00073903
	public CapsuleCollider Capsule
	{
		get
		{
			return this.handleCapsuleTrigger;
		}
	}

	// Token: 0x04001ACC RID: 6860
	[SerializeField]
	private HoldableObject holdable;

	// Token: 0x04001ACD RID: 6861
	[SerializeField]
	private CapsuleCollider handleCapsuleTrigger;
}
