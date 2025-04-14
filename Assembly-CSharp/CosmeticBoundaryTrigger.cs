using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003C1 RID: 961
public class CosmeticBoundaryTrigger : GorillaTriggerBox
{
	// Token: 0x06001739 RID: 5945 RVA: 0x0007191C File Offset: 0x0006FB1C
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

	// Token: 0x0600173A RID: 5946 RVA: 0x000719C8 File Offset: 0x0006FBC8
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

	// Token: 0x040019EB RID: 6635
	public VRRig rigRef;

	// Token: 0x040019EC RID: 6636
	private static TimeSince sinceLastTryOnEvent = 0f;
}
