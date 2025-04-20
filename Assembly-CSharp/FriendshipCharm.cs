using System;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020003D7 RID: 983
public class FriendshipCharm : HoldableObject
{
	// Token: 0x060017AA RID: 6058 RVA: 0x00040037 File Offset: 0x0003E237
	private void Awake()
	{
		this.parent = base.transform.parent;
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x000C9104 File Offset: 0x000C7304
	private void LateUpdate()
	{
		if (!this.isBroken && (this.lineStart.transform.position - this.lineEnd.transform.position).IsLongerThan(this.breakBraceletLength * GTPlayer.Instance.scale))
		{
			this.DestroyBracelet();
		}
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x0004004A File Offset: 0x0003E24A
	public void OnEnable()
	{
		this.interactionPoint.enabled = true;
		this.meshRenderer.enabled = true;
		this.isBroken = false;
		this.UpdatePosition();
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x00040071 File Offset: 0x0003E271
	private void DestroyBracelet()
	{
		this.interactionPoint.enabled = false;
		this.isBroken = true;
		Debug.Log("LeaveGroup: bracelet destroyed");
		FriendshipGroupDetection.Instance.LeaveParty();
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x000C915C File Offset: 0x000C735C
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		bool flag = grabbingHand == EquipmentInteractor.instance.leftHand;
		EquipmentInteractor.instance.UpdateHandEquipment(this, flag);
		GorillaTagger.Instance.StartVibration(flag, GorillaTagger.Instance.tapHapticStrength * 2f, GorillaTagger.Instance.tapHapticDuration * 2f);
		base.transform.SetParent(flag ? this.leftHandHoldAnchor : this.rightHandHoldAnchor);
		base.transform.localPosition = Vector3.zero;
	}

	// Token: 0x060017AF RID: 6063 RVA: 0x000C91E4 File Offset: 0x000C73E4
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		bool forLeftHand = releasingHand == EquipmentInteractor.instance.leftHand;
		EquipmentInteractor.instance.UpdateHandEquipment(null, forLeftHand);
		this.UpdatePosition();
		return base.OnRelease(zoneReleased, releasingHand);
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x0004009A File Offset: 0x0003E29A
	private void UpdatePosition()
	{
		base.transform.SetParent(this.parent);
		base.transform.localPosition = this.releasePosition.localPosition;
		base.transform.localRotation = this.releasePosition.localRotation;
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x000C9220 File Offset: 0x000C7420
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

	// Token: 0x060017B2 RID: 6066 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x00030607 File Offset: 0x0002E807
	public override void DropItemCleanup()
	{
	}

	// Token: 0x04001A5D RID: 6749
	[SerializeField]
	private InteractionPoint interactionPoint;

	// Token: 0x04001A5E RID: 6750
	[SerializeField]
	private Transform rightHandHoldAnchor;

	// Token: 0x04001A5F RID: 6751
	[SerializeField]
	private Transform leftHandHoldAnchor;

	// Token: 0x04001A60 RID: 6752
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04001A61 RID: 6753
	[SerializeField]
	private Transform lineStart;

	// Token: 0x04001A62 RID: 6754
	[SerializeField]
	private Transform lineEnd;

	// Token: 0x04001A63 RID: 6755
	[SerializeField]
	private Transform releasePosition;

	// Token: 0x04001A64 RID: 6756
	[SerializeField]
	private float breakBraceletLength;

	// Token: 0x04001A65 RID: 6757
	[SerializeField]
	private LayerMask breakItemLayerMask;

	// Token: 0x04001A66 RID: 6758
	private Transform parent;

	// Token: 0x04001A67 RID: 6759
	private bool isBroken;
}
