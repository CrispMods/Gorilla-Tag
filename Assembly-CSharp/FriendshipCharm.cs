using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020003CC RID: 972
public class FriendshipCharm : HoldableObject
{
	// Token: 0x0600175D RID: 5981 RVA: 0x0007209B File Offset: 0x0007029B
	private void Awake()
	{
		this.parent = base.transform.parent;
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x000720B0 File Offset: 0x000702B0
	private void LateUpdate()
	{
		if (!this.isBroken && (this.lineStart.transform.position - this.lineEnd.transform.position).IsLongerThan(this.breakBraceletLength * GTPlayer.Instance.scale))
		{
			this.DestroyBracelet();
		}
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x00072108 File Offset: 0x00070308
	public void OnEnable()
	{
		this.interactionPoint.enabled = true;
		this.meshRenderer.enabled = true;
		this.isBroken = false;
		this.UpdatePosition();
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x0007212F File Offset: 0x0007032F
	private void DestroyBracelet()
	{
		this.interactionPoint.enabled = false;
		this.isBroken = true;
		Debug.Log("LeaveGroup: bracelet destroyed");
		FriendshipGroupDetection.Instance.LeaveParty();
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x00072158 File Offset: 0x00070358
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		bool flag = grabbingHand == EquipmentInteractor.instance.leftHand;
		EquipmentInteractor.instance.UpdateHandEquipment(this, flag);
		GorillaTagger.Instance.StartVibration(flag, GorillaTagger.Instance.tapHapticStrength * 2f, GorillaTagger.Instance.tapHapticDuration * 2f);
		base.transform.SetParent(flag ? this.leftHandHoldAnchor : this.rightHandHoldAnchor);
		base.transform.localPosition = Vector3.zero;
	}

	// Token: 0x06001762 RID: 5986 RVA: 0x000721E0 File Offset: 0x000703E0
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		bool forLeftHand = releasingHand == EquipmentInteractor.instance.leftHand;
		EquipmentInteractor.instance.UpdateHandEquipment(null, forLeftHand);
		this.UpdatePosition();
		return base.OnRelease(zoneReleased, releasingHand);
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x0007221C File Offset: 0x0007041C
	private void UpdatePosition()
	{
		base.transform.SetParent(this.parent);
		base.transform.localPosition = this.releasePosition.localPosition;
		base.transform.localRotation = this.releasePosition.localRotation;
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x0007225C File Offset: 0x0007045C
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

	// Token: 0x06001765 RID: 5989 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void DropItemCleanup()
	{
	}

	// Token: 0x04001A14 RID: 6676
	[SerializeField]
	private InteractionPoint interactionPoint;

	// Token: 0x04001A15 RID: 6677
	[SerializeField]
	private Transform rightHandHoldAnchor;

	// Token: 0x04001A16 RID: 6678
	[SerializeField]
	private Transform leftHandHoldAnchor;

	// Token: 0x04001A17 RID: 6679
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04001A18 RID: 6680
	[SerializeField]
	private Transform lineStart;

	// Token: 0x04001A19 RID: 6681
	[SerializeField]
	private Transform lineEnd;

	// Token: 0x04001A1A RID: 6682
	[SerializeField]
	private Transform releasePosition;

	// Token: 0x04001A1B RID: 6683
	[SerializeField]
	private float breakBraceletLength;

	// Token: 0x04001A1C RID: 6684
	[SerializeField]
	private LayerMask breakItemLayerMask;

	// Token: 0x04001A1D RID: 6685
	private Transform parent;

	// Token: 0x04001A1E RID: 6686
	private bool isBroken;
}
