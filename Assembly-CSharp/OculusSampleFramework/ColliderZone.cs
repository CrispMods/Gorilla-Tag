using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A48 RID: 2632
	public interface ColliderZone
	{
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06004192 RID: 16786
		Collider Collider { get; }

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06004193 RID: 16787
		Interactable ParentInteractable { get; }

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06004194 RID: 16788
		InteractableCollisionDepth CollisionDepth { get; }
	}
}
