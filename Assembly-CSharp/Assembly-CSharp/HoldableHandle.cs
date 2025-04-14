﻿using System;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class HoldableHandle : InteractionPoint
{
	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x0600181D RID: 6173 RVA: 0x00075A7F File Offset: 0x00073C7F
	public new HoldableObject Holdable
	{
		get
		{
			return this.holdable;
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x0600181E RID: 6174 RVA: 0x00075A87 File Offset: 0x00073C87
	public CapsuleCollider Capsule
	{
		get
		{
			return this.handleCapsuleTrigger;
		}
	}

	// Token: 0x04001ACD RID: 6861
	[SerializeField]
	private HoldableObject holdable;

	// Token: 0x04001ACE RID: 6862
	[SerializeField]
	private CapsuleCollider handleCapsuleTrigger;
}
