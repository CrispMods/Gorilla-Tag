using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A75 RID: 2677
	public interface ColliderZone
	{
		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060042D7 RID: 17111
		Collider Collider { get; }

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060042D8 RID: 17112
		Interactable ParentInteractable { get; }

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060042D9 RID: 17113
		InteractableCollisionDepth CollisionDepth { get; }
	}
}
