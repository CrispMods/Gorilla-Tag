using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A4B RID: 2635
	public interface ColliderZone
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x0600419E RID: 16798
		Collider Collider { get; }

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x0600419F RID: 16799
		Interactable ParentInteractable { get; }

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060041A0 RID: 16800
		InteractableCollisionDepth CollisionDepth { get; }
	}
}
