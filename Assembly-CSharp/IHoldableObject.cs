using System;
using UnityEngine;

// Token: 0x020003E4 RID: 996
public interface IHoldableObject
{
	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06001826 RID: 6182
	GameObject gameObject { get; }

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06001827 RID: 6183
	// (set) Token: 0x06001828 RID: 6184
	string name { get; set; }

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06001829 RID: 6185
	bool TwoHanded { get; }

	// Token: 0x0600182A RID: 6186
	void OnHover(InteractionPoint pointHovered, GameObject hoveringHand);

	// Token: 0x0600182B RID: 6187
	void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand);

	// Token: 0x0600182C RID: 6188
	bool OnRelease(DropZone zoneReleased, GameObject releasingHand);

	// Token: 0x0600182D RID: 6189
	void DropItemCleanup();
}
