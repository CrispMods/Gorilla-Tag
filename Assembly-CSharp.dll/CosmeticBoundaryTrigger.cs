using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003C1 RID: 961
public class CosmeticBoundaryTrigger : GorillaTriggerBox
{
	// Token: 0x0600173C RID: 5948 RVA: 0x000C62C4 File Offset: 0x000C44C4
	public void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody == null)
		{
			return;
		}
		this.rigRef = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (this.rigRef == null)
		{
			return;
		}
		if (CosmeticBoundaryTrigger.sinceLastTryOnEvent.HasElapsed(0.5f, true))
		{
			GorillaTelemetry.PostShopEvent(this.rigRef, GTShopEventType.item_try_on, this.rigRef.tryOnSet.items);
		}
		this.rigRef.inTryOnRoom = true;
		this.rigRef.LocalUpdateCosmeticsWithTryon(this.rigRef.cosmeticSet, this.rigRef.tryOnSet);
		this.rigRef.myBodyDockPositions.RefreshTransferrableItems();
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x000C6370 File Offset: 0x000C4570
	public void OnTriggerExit(Collider other)
	{
		if (other.attachedRigidbody == null)
		{
			return;
		}
		this.rigRef = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (this.rigRef == null)
		{
			return;
		}
		this.rigRef.inTryOnRoom = false;
		if (this.rigRef.isOfflineVRRig)
		{
			this.rigRef.tryOnSet.ClearSet(CosmeticsController.instance.nullItem);
			CosmeticsController.instance.ClearCheckout(false);
			CosmeticsController.instance.UpdateShoppingCart();
			CosmeticsController.instance.UpdateWornCosmetics(true);
		}
		this.rigRef.LocalUpdateCosmeticsWithTryon(this.rigRef.cosmeticSet, this.rigRef.tryOnSet);
		this.rigRef.myBodyDockPositions.RefreshTransferrableItems();
	}

	// Token: 0x040019EC RID: 6636
	public VRRig rigRef;

	// Token: 0x040019ED RID: 6637
	private static TimeSince sinceLastTryOnEvent = 0f;
}
