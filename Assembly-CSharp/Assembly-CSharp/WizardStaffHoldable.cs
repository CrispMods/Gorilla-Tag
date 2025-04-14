﻿using System;
using GorillaTag;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class WizardStaffHoldable : TransferrableObject
{
	// Token: 0x06000572 RID: 1394 RVA: 0x00020584 File Offset: 0x0001E784
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.tipTargetLocalPosition = this.tipTransform.localPosition;
		this.hasEffectsGameObject = (this.effectsGameObject != null);
		this.effectsHaveBeenPlayed = false;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x000205B7 File Offset: 0x0001E7B7
	internal override void OnEnable()
	{
		base.OnEnable();
		this.InitToDefault();
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x000205C5 File Offset: 0x0001E7C5
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x000205D3 File Offset: 0x0001E7D3
	private void InitToDefault()
	{
		this.cooldownRemaining = 0f;
		if (this.hasEffectsGameObject && this.effectsHaveBeenPlayed)
		{
			this.effectsGameObject.SetActive(false);
		}
		this.effectsHaveBeenPlayed = false;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00020604 File Offset: 0x0001E804
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (!base.InHand() || this.itemState == TransferrableObject.ItemStates.State1 || !GorillaParent.hasInstance || !this.hitLastFrame)
		{
			return;
		}
		if (this.velocityEstimator.linearVelocity.magnitude < this.minSlamVelocity)
		{
			return;
		}
		Vector3 up = this.tipTransform.up;
		Vector3 up2 = Vector3.up;
		if (Vector3.Angle(up, up2) > this.minSlamAngle)
		{
			return;
		}
		this.itemState = TransferrableObject.ItemStates.State1;
		this.cooldownRemaining = this.cooldown;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00020688 File Offset: 0x0001E888
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		this.cooldownRemaining -= Time.deltaTime;
		if (this.cooldownRemaining <= 0f)
		{
			this.itemState = TransferrableObject.ItemStates.State0;
			if (this.hasEffectsGameObject)
			{
				this.effectsGameObject.SetActive(false);
			}
			this.effectsHaveBeenPlayed = false;
		}
		if (base.InHand())
		{
			Vector3 position = base.transform.position;
			Vector3 end = base.transform.TransformPoint(this.tipTargetLocalPosition);
			RaycastHit raycastHit;
			if (Physics.Linecast(position, end, out raycastHit, this.tipCollisionLayerMask))
			{
				this.tipTransform.position = raycastHit.point;
				this.hitLastFrame = true;
			}
			else
			{
				this.tipTransform.localPosition = this.tipTargetLocalPosition;
				this.hitLastFrame = false;
			}
			if (this.itemState == TransferrableObject.ItemStates.State1 && this.hasEffectsGameObject && !this.effectsHaveBeenPlayed)
			{
				this.effectsGameObject.SetActive(true);
				this.effectsHaveBeenPlayed = true;
			}
		}
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00020778 File Offset: 0x0001E978
	protected override void LateUpdateReplicated()
	{
		base.LateUpdateReplicated();
		if (this.itemState == TransferrableObject.ItemStates.State1 && !this.effectsHaveBeenPlayed)
		{
			this.cooldownRemaining = this.cooldown;
		}
	}

	// Token: 0x0400065A RID: 1626
	[Tooltip("This GameObject will activate when the staff hits the ground with enough force.")]
	public GameObject effectsGameObject;

	// Token: 0x0400065B RID: 1627
	[Tooltip("The Transform of the staff's tip which will be used to determine if the staff is being slammed. Up axis (Y) should point along the length of the staff.")]
	public Transform tipTransform;

	// Token: 0x0400065C RID: 1628
	public float tipCollisionRadius = 0.05f;

	// Token: 0x0400065D RID: 1629
	public LayerMask tipCollisionLayerMask;

	// Token: 0x0400065E RID: 1630
	[Tooltip("Used to calculate velocity of the staff.")]
	public GorillaVelocityEstimator velocityEstimator;

	// Token: 0x0400065F RID: 1631
	public float cooldown = 5f;

	// Token: 0x04000660 RID: 1632
	[Tooltip("The velocity of the staff's tip must be greater than this value to activate the effect.")]
	public float minSlamVelocity = 0.5f;

	// Token: 0x04000661 RID: 1633
	[Tooltip("The angle (in degrees) between the staff's tip and the ground must be less than this value to activate the effect.")]
	public float minSlamAngle = 5f;

	// Token: 0x04000662 RID: 1634
	[DebugReadout]
	private float cooldownRemaining;

	// Token: 0x04000663 RID: 1635
	[DebugReadout]
	private bool hitLastFrame;

	// Token: 0x04000664 RID: 1636
	private Vector3 tipTargetLocalPosition;

	// Token: 0x04000665 RID: 1637
	private bool hasEffectsGameObject;

	// Token: 0x04000666 RID: 1638
	private bool effectsHaveBeenPlayed;
}
