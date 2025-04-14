using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020003CC RID: 972
public class FriendshipCharm : HoldableObject
{
	// Token: 0x06001760 RID: 5984 RVA: 0x0007241F File Offset: 0x0007061F
	private void Awake()
	{
		this.parent = base.transform.parent;
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x00072434 File Offset: 0x00070634
	private void LateUpdate()
	{
		if (!this.isBroken && (this.lineStart.transform.position - this.lineEnd.transform.position).IsLongerThan(this.breakBraceletLength * GTPlayer.Instance.scale))
		{
			this.DestroyBracelet();
		}
	}

	// Token: 0x06001762 RID: 5986 RVA: 0x0007248C File Offset: 0x0007068C
	public void OnEnable()
	{
		this.interactionPoint.enabled = true;
		this.meshRenderer.enabled = true;
		this.isBroken = false;
		this.UpdatePosition();
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x000724B3 File Offset: 0x000706B3
	private void DestroyBracelet()
	{
		this.interactionPoint.enabled = false;
		this.isBroken = true;
		Debug.Log("LeaveGroup: bracelet destroyed");
		FriendshipGroupDetection.Instance.LeaveParty();
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x000724DC File Offset: 0x000706DC
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		bool flag = grabbingHand == EquipmentInteractor.instance.leftHand;
		EquipmentInteractor.instance.UpdateHandEquipment(this, flag);
		GorillaTagger.Instance.StartVibration(flag, GorillaTagger.Instance.tapHapticStrength * 2f, GorillaTagger.Instance.tapHapticDuration * 2f);
		base.transform.SetParent(flag ? this.leftHandHoldAnchor : this.rightHandHoldAnchor);
		base.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x00072564 File Offset: 0x00070764
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		bool forLeftHand = releasingHand == EquipmentInteractor.instance.leftHand;
		EquipmentInteractor.instance.UpdateHandEquipment(null, forLeftHand);
		this.UpdatePosition();
		return base.OnRelease(zoneReleased, releasingHand);
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x000725A0 File Offset: 0x000707A0
	private void UpdatePosition()
	{
		base.transform.SetParent(this.parent);
		base.transform.localPosition = this.releasePosition.localPosition;
		base.transform.localRotation = this.releasePosition.localRotation;
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x000725E0 File Offset: 0x000707E0
	private void OnCollisionEnter(Collision other)
	{
		if (!this.isBroken)
		{
			return;
		}
		if (this.breakItemLayerMask != (this.breakItemLayerMask | 1 << other.gameObject.layer))
		{
			return;
		}
		this.meshRenderer.enabled = false;
		this.UpdatePosition();
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void DropItemCleanup()
	{
	}

	// Token: 0x04001A15 RID: 6677
	[SerializeField]
	private InteractionPoint interactionPoint;

	// Token: 0x04001A16 RID: 6678
	[SerializeField]
	private Transform rightHandHoldAnchor;

	// Token: 0x04001A17 RID: 6679
	[SerializeField]
	private Transform leftHandHoldAnchor;

	// Token: 0x04001A18 RID: 6680
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04001A19 RID: 6681
	[SerializeField]
	private Transform lineStart;

	// Token: 0x04001A1A RID: 6682
	[SerializeField]
	private Transform lineEnd;

	// Token: 0x04001A1B RID: 6683
	[SerializeField]
	private Transform releasePosition;

	// Token: 0x04001A1C RID: 6684
	[SerializeField]
	private float breakBraceletLength;

	// Token: 0x04001A1D RID: 6685
	[SerializeField]
	private LayerMask breakItemLayerMask;

	// Token: 0x04001A1E RID: 6686
	private Transform parent;

	// Token: 0x04001A1F RID: 6687
	private bool isBroken;
}
