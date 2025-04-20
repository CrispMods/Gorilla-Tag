using System;
using UnityEngine;

// Token: 0x020003EF RID: 1007
public interface IHoldableObject
{
	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06001873 RID: 6259
	GameObject gameObject { get; }

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06001874 RID: 6260
	// (set) Token: 0x06001875 RID: 6261
	string name { get; set; }

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001876 RID: 6262
	bool TwoHanded { get; }

	// Token: 0x06001877 RID: 6263
	void OnHover(InteractionPoint pointHovered, GameObject hoveringHand);

	// Token: 0x06001878 RID: 6264
	void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand);

	// Token: 0x06001879 RID: 6265
	bool OnRelease(DropZone zoneReleased, GameObject releasingHand);

	// Token: 0x0600187A RID: 6266
	void DropItemCleanup();
}
