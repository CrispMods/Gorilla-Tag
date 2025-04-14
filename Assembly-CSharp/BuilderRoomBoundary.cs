using System;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
public class BuilderRoomBoundary : GorillaTriggerBox
{
	// Token: 0x06001E76 RID: 7798 RVA: 0x00099504 File Offset: 0x00097704
	private void Awake()
	{
		this.enableOnEnterTrigger.OnEnter += this.OnEnteredBoundary;
		this.disableOnExitTrigger.OnExit += this.OnExitedBoundary;
	}

	// Token: 0x06001E77 RID: 7799 RVA: 0x00099534 File Offset: 0x00097734
	private void OnDestroy()
	{
		this.enableOnEnterTrigger.OnEnter -= this.OnEnteredBoundary;
		this.disableOnExitTrigger.OnExit -= this.OnExitedBoundary;
	}

	// Token: 0x06001E78 RID: 7800 RVA: 0x00099564 File Offset: 0x00097764
	public void OnEnteredBoundary(Collider other)
	{
		if (other.attachedRigidbody == null)
		{
			return;
		}
		if (!ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks))
		{
			return;
		}
		this.rigRef = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (this.rigRef == null || !this.rigRef.isOfflineVRRig)
		{
			return;
		}
		this.rigRef.EnableBuilderResizeWatch(true);
	}

	// Token: 0x06001E79 RID: 7801 RVA: 0x000995D0 File Offset: 0x000977D0
	public void OnExitedBoundary(Collider other)
	{
		if (other.attachedRigidbody == null)
		{
			return;
		}
		this.rigRef = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (this.rigRef == null || !this.rigRef.isOfflineVRRig)
		{
			return;
		}
		this.rigRef.EnableBuilderResizeWatch(false);
	}

	// Token: 0x0400221B RID: 8731
	[SerializeField]
	private SizeChangerTrigger enableOnEnterTrigger;

	// Token: 0x0400221C RID: 8732
	[SerializeField]
	private SizeChangerTrigger disableOnExitTrigger;

	// Token: 0x0400221D RID: 8733
	private VRRig rigRef;
}
