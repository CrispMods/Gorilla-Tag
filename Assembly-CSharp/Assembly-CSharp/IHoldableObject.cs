using System;
using UnityEngine;

// Token: 0x020003E4 RID: 996
public interface IHoldableObject
{
	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06001829 RID: 6185
	GameObject gameObject { get; }

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x0600182A RID: 6186
	// (set) Token: 0x0600182B RID: 6187
	string name { get; set; }

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x0600182C RID: 6188
	bool TwoHanded { get; }

	// Token: 0x0600182D RID: 6189
	void OnHover(InteractionPoint pointHovered, GameObject hoveringHand);

	// Token: 0x0600182E RID: 6190
	void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand);

	// Token: 0x0600182F RID: 6191
	bool OnRelease(DropZone zoneReleased, GameObject releasingHand);

	// Token: 0x06001830 RID: 6192
	void DropItemCleanup();
}
