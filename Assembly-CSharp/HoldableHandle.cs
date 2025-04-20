using System;
using UnityEngine;

// Token: 0x020003ED RID: 1005
public class HoldableHandle : InteractionPoint
{
	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06001867 RID: 6247 RVA: 0x00040925 File Offset: 0x0003EB25
	public new HoldableObject Holdable
	{
		get
		{
			return this.holdable;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06001868 RID: 6248 RVA: 0x0004092D File Offset: 0x0003EB2D
	public CapsuleCollider Capsule
	{
		get
		{
			return this.handleCapsuleTrigger;
		}
	}

	// Token: 0x04001B15 RID: 6933
	[SerializeField]
	private HoldableObject holdable;

	// Token: 0x04001B16 RID: 6934
	[SerializeField]
	private CapsuleCollider handleCapsuleTrigger;
}
