using System;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
public class BuilderRoomBoundary : GorillaTriggerBox
{
	// Token: 0x06001E79 RID: 7801 RVA: 0x00043A85 File Offset: 0x00041C85
	private void Awake()
	{
		this.enableOnEnterTrigger.OnEnter += this.OnEnteredBoundary;
		this.disableOnExitTrigger.OnExit += this.OnExitedBoundary;
	}

	// Token: 0x06001E7A RID: 7802 RVA: 0x00043AB5 File Offset: 0x00041CB5
	private void OnDestroy()
	{
		this.enableOnEnterTrigger.OnEnter -= this.OnEnteredBoundary;
		this.disableOnExitTrigger.OnExit -= this.OnExitedBoundary;
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x000E8F0C File Offset: 0x000E710C
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

	// Token: 0x06001E7C RID: 7804 RVA: 0x000E8F78 File Offset: 0x000E7178
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

	// Token: 0x0400221C RID: 8732
	[SerializeField]
	private SizeChangerTrigger enableOnEnterTrigger;

	// Token: 0x0400221D RID: 8733
	[SerializeField]
	private SizeChangerTrigger disableOnExitTrigger;

	// Token: 0x0400221E RID: 8734
	private VRRig rigRef;
}
