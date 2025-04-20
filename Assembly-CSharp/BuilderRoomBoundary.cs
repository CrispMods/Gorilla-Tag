using System;
using UnityEngine;

// Token: 0x020004F5 RID: 1269
public class BuilderRoomBoundary : GorillaTriggerBox
{
	// Token: 0x06001ECF RID: 7887 RVA: 0x00044E24 File Offset: 0x00043024
	private void Awake()
	{
		this.enableOnEnterTrigger.OnEnter += this.OnEnteredBoundary;
		this.disableOnExitTrigger.OnExit += this.OnExitedBoundary;
	}

	// Token: 0x06001ED0 RID: 7888 RVA: 0x00044E54 File Offset: 0x00043054
	private void OnDestroy()
	{
		this.enableOnEnterTrigger.OnEnter -= this.OnEnteredBoundary;
		this.disableOnExitTrigger.OnExit -= this.OnExitedBoundary;
	}

	// Token: 0x06001ED1 RID: 7889 RVA: 0x000EBC48 File Offset: 0x000E9E48
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

	// Token: 0x06001ED2 RID: 7890 RVA: 0x000EBCB4 File Offset: 0x000E9EB4
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

	// Token: 0x0400226E RID: 8814
	[SerializeField]
	private SizeChangerTrigger enableOnEnterTrigger;

	// Token: 0x0400226F RID: 8815
	[SerializeField]
	private SizeChangerTrigger disableOnExitTrigger;

	// Token: 0x04002270 RID: 8816
	private VRRig rigRef;
}
